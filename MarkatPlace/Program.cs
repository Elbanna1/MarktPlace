using System.IO.Compression;
using MarkatPlace.Extensions;
using MarkatPlace.Filters;
using MarkatPlace.Middleware;
using MarkatPlace.RealTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Data;
using Persistence.Data.Development;
using Presentation.Controllers;
using ServicesAbstraction;
using Services;
using Shared.Constants;
using Shared.Responses;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

var uploadsPhysicalRoot = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "wwwroot", ImageConstants.UploadsRootFolder));

foreach (var uploadFolder in ImageConstants.AllFolders)
    Directory.CreateDirectory(Path.Combine(uploadsPhysicalRoot, uploadFolder));

builder.Services.Configure<FileStorageSettings>(options => options.UploadsRootPath = uploadsPhysicalRoot);

builder.Services.Configure<ListingInteractionSettings>(
    builder.Configuration.GetSection(ListingInteractionSettings.SectionName));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = FileUploadConstants.MaxRequestBodySizeBytes;

    options.AddServerHeader = false;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = FileUploadConstants.MaxRequestBodySizeBytes;
    options.MultipartHeadersLengthLimit = 32 * 1024;
    options.ValueLengthLimit = int.MaxValue;
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAppUrlSettings(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);

builder.Services.AddMemoryCache(options => options.SizeLimit = 512);

builder.Services.AddApiRateLimiting(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "database",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["ready"]);

builder.Services.AddSignalR();
builder.Services.AddScoped<IRealtimeNotifier, SignalRRealtimeNotifier>();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<AdminPermissionFilter>();

        options.Filters.Add<ValidationFilter>();

        options.Filters.Add<ListingInteractionFilter>();

        options.Filters.Add<PendingListingAlertFilter>();

        options.Filters.Add<ListingStatsFilter>();

        options.Filters.Add<ListingEditReviewFilter>();
    })
    .AddApplicationPart(typeof(AuthController).Assembly);

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

    options.SuppressMapClientErrors = true;
});

builder.Services.Configure<SwaggerAuthSettings>(
    builder.Configuration.GetSection(SwaggerAuthSettings.SectionName));

builder.Services.AddSwaggerWithJwt();

builder.Services.AddApiCors(builder.Configuration, builder.Environment);

builder.Services.AddReverseProxyForwardedHeaders(builder.Configuration);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);

builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseForwardedHeaders();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (builder.Configuration.GetValue<bool>("Diagnostics:SqlCounter"))
    app.UseMiddleware<SqlDiagnosticsMiddleware>();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.HasStarted || response.ContentLength > 0)
        return;

    response.ContentType = "application/json";

    var message = response.StatusCode switch
    {
        StatusCodes.Status404NotFound => "الرابط المطلوب مش موجود.",
        StatusCodes.Status405MethodNotAllowed => "الطريقة دي مش مدعومة على الرابط ده.",
        StatusCodes.Status413PayloadTooLarge =>
            $"{UserMessages.Errors.RequestTooLarge} أقصى حجم للطلب الواحد " +
            $"{FileUploadConstants.MaxRequestBodySizeMegabytes} ميجابايت.",
        StatusCodes.Status415UnsupportedMediaType => "نوع المحتوى ده مش مدعوم على الرابط ده.",
        _ => "مش قادرين ننفذ الطلب."
    };

    await response.WriteAsJsonAsync(ApiResponse.Fail(message));
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "no-referrer";
    headers["X-XSS-Protection"] = "0";
    await next();
});

if (!app.Environment.IsDevelopment() &&
    !app.Services.GetRequiredService<IOptions<SwaggerAuthSettings>>().Value.IsConfigured)
{
    app.Logger.LogWarning(
        "Swagger is locked: no {Section} credentials are configured, so every request under " +
        "{Path} is refused with 401. Set {UsernameKey} and {PasswordKey} to open it.",
        SwaggerAuthSettings.SectionName, SwaggerAuthSettings.PathPrefix,
        $"{SwaggerAuthSettings.SectionName}__{nameof(SwaggerAuthSettings.Username)}",
        $"{SwaggerAuthSettings.SectionName}__{nameof(SwaggerAuthSettings.Password)}");
}

app.UseMiddleware<SwaggerBasicAuthMiddleware>();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger", out var remainder) &&
        (remainder.Value is null ||
         remainder.Value.Length == 0 ||
         string.Equals(remainder.Value, "/", StringComparison.Ordinal) ||
         string.Equals(remainder.Value, "/index.html", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(remainder.Value, "/index.js", StringComparison.OrdinalIgnoreCase)))
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
            context.Response.Headers.Remove(Microsoft.Net.Http.Headers.HeaderNames.ETag);
            return Task.CompletedTask;
        });
    }

    await next();
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/{ApiVersions.V1}/swagger.json", "MarkatPlace V1");
    options.SwaggerEndpoint($"/swagger/{ApiVersions.AdminV2}/swagger.json", "MarkatPlace Admin V2");
    options.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger"))
        .ExcludeFromDescription();
}

app.UseHttpsRedirection();

app.UseResponseCompression();

var uploadContentTypes = new FileExtensionContentTypeProvider();

foreach (var imageFormat in ImageFormatCatalog.All)
{
    foreach (var extension in imageFormat.Extensions)
        uploadContentTypes.Mappings[extension] = imageFormat.CanonicalContentType;
}

foreach (var uploadFormat in DocumentFormatCatalog.All.Concat(VideoFormatCatalog.All))
{
    foreach (var extension in uploadFormat.Extensions)
        uploadContentTypes.Mappings[extension] = uploadFormat.CanonicalContentType;
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPhysicalRoot),
    RequestPath = $"/{ImageConstants.UploadsRootFolder}",
    ContentTypeProvider = uploadContentTypes,
    ServeUnknownFileTypes = false,
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.Headers;
        headers.CacheControl = "public,max-age=31536000,immutable";
        headers["Access-Control-Allow-Origin"] = "*";

        headers["Content-Security-Policy"] = "default-src 'none'; sandbox";
    }
});

app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = uploadContentTypes });

app.UseCors(CorsExtensions.PolicyName);

app.UseResponseCaching();

app.UseAuthentication();

app.UseMiddleware<AccountStatusMiddleware>();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false })
    .DisableRateLimiting()
    .AllowAnonymous();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            durationMs = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                durationMs = entry.Value.Duration.TotalMilliseconds,

                error = entry.Value.Exception is null ? null : "See server logs."
            })
        });
    }
})
    .DisableRateLimiting()
    .AllowAnonymous();

app.MapHub<NotificationHub>("/hubs/notifications").RequireCors(CorsExtensions.PolicyName);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider, app.Configuration);

    if (app.Environment.IsDevelopment())
    {
        var demoLogger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("MarkatPlace.DemoData");

        try
        {
            var report = await DevelopmentDataSeeder.SeedAsync(
                scope.ServiceProvider, app.Configuration, demoLogger);

            if (report.Skipped)
            {
                demoLogger.LogInformation("Demo data seeding skipped: {Reason}", report.SkipReason);
            }
            else
            {
                demoLogger.LogInformation(
                    "Demo data ready. Images: {Downloaded} downloaded, {Reused} reused, {Failed} failed, " +
                    "{Linked} linked to records. Created: {Users} users, {Ads} ads, {Workshops} workshops, " +
                    "{Craftsmen} craftsmen, {Posts} posts. Illustrated: {AdsImg} ads, {WorkshopsImg} workshops, " +
                    "{CraftsmenImg} craftsmen, {PostsImg} posts, {Avatars} profile pictures.",
                    report.ImagesDownloaded, report.ImagesReused, report.ImagesFailed, report.TotalImagesLinked,
                    report.UsersCreated, report.AdvertisementsCreated, report.WorkshopsCreated,
                    report.CraftsmenCreated, report.PostsCreated,
                    report.AdvertisementsIllustrated, report.WorkshopsIllustrated,
                    report.CraftsmenIllustrated, report.PostsIllustrated, report.ProfileImagesAssigned);
            }
        }
        catch (Exception exception)
        {
            demoLogger.LogError(exception, "Demo data seeding failed; the application will start without it.");
        }
    }
}

app.Run();
