using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MarkatPlace.Middleware;
using Presentation.Controllers;
using Presentation.Extensions;
using Shared.Constants;
using Shared.Exceptions;
using Xunit;

namespace MarkatPlace.Tests;

public class RequestSizeLimitTests
{
    private const long Megabyte = 1024 * 1024;

    private static readonly Type[] Controllers =
        typeof(AuthController).Assembly.GetTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
            .ToArray();

    [Fact]
    public void The_platform_ceiling_is_the_documented_256_megabytes()
    {
        Assert.Equal(256 * Megabyte, FileUploadConstants.MaxRequestBodySizeBytes);
        Assert.Equal(256, FileUploadConstants.MaxRequestBodySizeMegabytes);
    }

    [Fact]
    public void The_ceiling_is_generous_but_never_unlimited()
    {
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes < long.MaxValue);
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes < 1024L * 1024 * 1024);
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes
                    >= ImageConstants.MaxImagesPerItem * ImageConstants.MaxFileSizeBytes
                       + FileUploadConstants.MaxVideoSizeBytes);
    }

    [Fact]
    public void No_endpoint_asks_for_more_than_the_platform_ceiling()
    {
        var offenders = EndpointLimits()
            .Where(limit => limit.Bytes > FileUploadConstants.MaxRequestBodySizeBytes)
            .Select(limit => $"{limit.Endpoint} allows {limit.Bytes} bytes")
            .ToList();

        Assert.True(offenders.Count == 0,
            "Kestrel would reject these before the action ran: " + string.Join(", ", offenders));
    }

    [Fact]
    public void Every_listing_upload_endpoint_accepts_the_whole_ceiling()
    {
        var short_of_the_ceiling = EndpointLimits()
            .Where(limit => limit.IsListingUpload)
            .Where(limit => limit.Bytes < FileUploadConstants.MaxRequestBodySizeBytes)
            .Select(limit => $"{limit.Endpoint} stops at {limit.Bytes / Megabyte} MB")
            .ToList();

        Assert.True(short_of_the_ceiling.Count == 0,
            "A listing form would answer 413 below the documented limit: " +
            string.Join(", ", short_of_the_ceiling));
    }

    [Fact]
    public void The_workshops_form_that_failed_in_production_accepts_the_whole_ceiling()
    {
        var limits = EndpointLimits()
            .Where(limit => limit.Endpoint.StartsWith("WorkshopsController.", StringComparison.Ordinal))
            .ToList();

        Assert.NotEmpty(limits);
        Assert.All(limits, limit =>
            Assert.Equal(FileUploadConstants.MaxRequestBodySizeBytes, limit.Bytes));
    }

    [Fact]
    public void Kestrel_and_the_multipart_reader_share_the_one_documented_ceiling()
    {
        var program = RepositoryRoot.ReadFile("MarkatPlace/Program.cs");

        Assert.Contains(
            "options.Limits.MaxRequestBodySize = FileUploadConstants.MaxRequestBodySizeBytes;",
            program);
        Assert.Contains(
            "options.MultipartBodyLengthLimit = FileUploadConstants.MaxRequestBodySizeBytes;",
            program);
    }

    [Fact]
    public void Nginx_accepts_at_least_as_large_a_body_as_the_application_does()
    {
        var config = RepositoryRoot.ReadFile("deploy/nginx/api.shopiklopik.com.conf");
        var match = Regex.Match(config, @"^\s*client_max_body_size\s+(\d+)([kmg]?);", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        Assert.True(match.Success, "The committed Nginx site declares no client_max_body_size.");

        var multiplier = match.Groups[2].Value.ToLowerInvariant() switch
        {
            "k" => 1024L,
            "m" => 1024L * 1024,
            "g" => 1024L * 1024 * 1024,
            _ => 1L
        };

        var nginxLimit = long.Parse(match.Groups[1].Value) * multiplier;

        Assert.True(nginxLimit >= FileUploadConstants.MaxRequestBodySizeBytes,
            $"Nginx allows {nginxLimit} bytes but the application accepts up to " +
            $"{FileUploadConstants.MaxRequestBodySizeBytes}. Requests between the two never reach " +
            "ASP.NET Core: Nginx answers 413 itself, and the browser reports it as a CORS failure.");

        Assert.True(nginxLimit <= FileUploadConstants.ReverseProxyMaxBodySizeBytes,
            "Nginx must not accept a body the application will refuse; it is the layer that can " +
            "reject an oversized upload cleanly.");
    }

    [Fact]
    public void The_committed_Nginx_site_answers_an_oversized_upload_with_CORS_headers()
    {
        var config = RepositoryRoot.ReadFile("deploy/nginx/api.shopiklopik.com.conf");

        Assert.Contains("error_page 413", config);
        Assert.Contains("Access-Control-Allow-Origin", config);
        Assert.Contains("https://shopiklopik.com", config);
    }

    [Fact]
    public void The_committed_Nginx_site_forwards_the_headers_the_application_relies_on()
    {
        var config = RepositoryRoot.ReadFile("deploy/nginx/api.shopiklopik.com.conf");

        Assert.Contains("X-Forwarded-Proto $scheme", config);
        Assert.Contains("X-Forwarded-For   $proxy_add_x_forwarded_for", config);
        Assert.Contains("proxy_set_header Upgrade           $http_upgrade", config);
    }

    [Fact]
    public async Task A_request_below_the_limit_passes_through_untouched()
    {
        var context = NewContext();
        var middleware = new GlobalExceptionHandlingMiddleware(
            _ =>
            {
                context.Response.StatusCode = StatusCodes.Status201Created;
                return Task.CompletedTask;
            },
            NullLogger<GlobalExceptionHandlingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status201Created, context.Response.StatusCode);
        Assert.Equal("https://shopiklopik.com", context.Response.Headers["Access-Control-Allow-Origin"]);
    }

    [Fact]
    public async Task A_request_above_the_limit_is_answered_413_and_not_500()
    {
        var (status, body, _) = await RunAsync(
            new BadHttpRequestException("Request body too large.", StatusCodes.Status413PayloadTooLarge));

        Assert.Equal(StatusCodes.Status413PayloadTooLarge, status);
        Assert.False(body.GetProperty("success").GetBoolean());
    }

    [Fact]
    public async Task The_oversized_request_error_names_the_limit_in_Arabic()
    {
        var (_, body, _) = await RunAsync(
            new BadHttpRequestException("Request body too large.", StatusCodes.Status413PayloadTooLarge));

        var message = body.GetProperty("message").GetString()!;

        Assert.True(ArabicText.IsArabic(message), message);
        Assert.Contains(FileUploadConstants.MaxRequestBodySizeMegabytes.ToString(), message);
        Assert.Contains("ميجابايت", message);
    }

    [Fact]
    public async Task A_multipart_body_over_the_reader_limit_is_answered_413_in_Arabic()
    {
        var (status, body, _) = await RunAsync(
            new InvalidDataException("Multipart body length limit exceeded."));

        Assert.Equal(StatusCodes.Status413PayloadTooLarge, status);
        Assert.True(ArabicText.IsArabic(body.GetProperty("message").GetString()!));
    }

    [Fact]
    public async Task A_malformed_body_is_answered_400_in_Arabic_rather_than_500()
    {
        var (status, body, _) = await RunAsync(
            new BadHttpRequestException("Unexpected end of request content."));

        Assert.Equal(StatusCodes.Status400BadRequest, status);
        Assert.True(ArabicText.IsArabic(body.GetProperty("message").GetString()!));
    }

    [Theory]
    [InlineData(StatusCodes.Status413PayloadTooLarge)]
    [InlineData(StatusCodes.Status400BadRequest)]
    public async Task An_error_response_keeps_the_CORS_headers_the_browser_needs(int statusCode)
    {
        var (_, _, headers) = await RunAsync(
            new BadHttpRequestException("rejected", statusCode));

        Assert.Equal("https://shopiklopik.com", headers["Access-Control-Allow-Origin"]);
        Assert.Equal("true", headers["Access-Control-Allow-Credentials"]);
        Assert.Equal("Origin", headers["Vary"]);
    }

    [Fact]
    public async Task An_application_error_response_keeps_the_CORS_headers_too()
    {
        var (status, _, headers) = await RunAsync(new NotFoundException("مش موجود."));

        Assert.Equal(StatusCodes.Status404NotFound, status);
        Assert.Equal("https://shopiklopik.com", headers["Access-Control-Allow-Origin"]);
    }

    [Fact]
    public async Task An_unexpected_failure_keeps_the_CORS_headers_too()
    {
        var (status, _, headers) = await RunAsync(new InvalidOperationException("boom"));

        Assert.Equal(StatusCodes.Status500InternalServerError, status);
        Assert.Equal("https://shopiklopik.com", headers["Access-Control-Allow-Origin"]);
    }

    [Fact]
    public async Task A_single_file_over_the_largest_accepted_size_is_refused_before_it_is_buffered()
    {
        var file = new OversizedFormFile(FileUploadConstants.MaxSingleFileSizeBytes + 1);

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => file.ToUploadModelAsync());

        Assert.True(ArabicText.IsArabic(exception.Message), exception.Message);
        Assert.False(file.WasRead, "The file was copied into memory before its size was checked.");
    }

    [Fact]
    public async Task A_single_file_within_the_accepted_size_is_still_buffered()
    {
        var file = new OversizedFormFile(16, content: [1, 2, 3, 4]);

        var upload = await file.ToUploadModelAsync();

        Assert.NotNull(upload);
        Assert.True(file.WasRead);
    }

    [Fact]
    public void Per_file_validation_rules_are_unchanged_by_the_larger_request_ceiling()
    {
        Assert.Equal(5 * Megabyte, ImageConstants.MaxFileSizeBytes);
        Assert.Equal(50 * Megabyte, FileUploadConstants.MaxVideoSizeBytes);
        Assert.Equal(10 * Megabyte, FileUploadConstants.MaxDocumentSizeBytes);
        Assert.Equal(10, ImageConstants.MaxImagesPerItem);
    }

    private sealed record EndpointLimit(string Endpoint, long Bytes, bool IsListingUpload);

    private static IEnumerable<EndpointLimit> EndpointLimits()
    {
        foreach (var controller in Controllers)
        {
            foreach (var method in controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var limit = method.GetCustomAttributes()
                    .OfType<IRequestSizeLimitMetadata>()
                    .FirstOrDefault();

                if (limit?.MaxRequestBodySize is not { } bytes)
                    continue;

                var consumes = method.GetCustomAttribute<ConsumesAttribute>();
                var isMultipart = consumes?.ContentTypes.Contains("multipart/form-data") == true;

                var isListingUpload = isMultipart
                    && !controller.Name.StartsWith("Admin", StringComparison.Ordinal)
                    && !controller.Name.StartsWith("BannerBooking", StringComparison.Ordinal);

                yield return new EndpointLimit(
                    $"{controller.Name}.{method.Name}", bytes, isListingUpload);
            }
        }
    }

    private static DefaultHttpContext NewContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Path = "/api/workshops";
        context.Request.Headers.Origin = "https://shopiklopik.com";
        context.Response.Body = new MemoryStream();
        context.Response.Headers["Access-Control-Allow-Origin"] = "https://shopiklopik.com";
        context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
        context.Response.Headers["Vary"] = "Origin";
        context.Features.Set<IHttpResponseBodyFeature>(
            new StreamResponseBodyFeature(context.Response.Body));
        return context;
    }

    private static async Task<(int Status, JsonElement Body, IHeaderDictionary Headers)> RunAsync(
        Exception failure)
    {
        var context = NewContext();

        var middleware = new GlobalExceptionHandlingMiddleware(
            _ => throw failure, NullLogger<GlobalExceptionHandlingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var json = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

        return (context.Response.StatusCode, JsonDocument.Parse(json).RootElement, context.Response.Headers);
    }

    private sealed class OversizedFormFile : IFormFile
    {
        private readonly byte[] _content;

        public OversizedFormFile(long length, byte[]? content = null)
        {
            Length = length;
            _content = content ?? [];
        }

        public bool WasRead { get; private set; }

        public string ContentType { get; set; } = "image/jpeg";

        public string ContentDisposition { get; set; } = string.Empty;

        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();

        public long Length { get; }

        public string Name { get; set; } = "Images";

        public string FileName { get; set; } = "huge.jpg";

        public void CopyTo(Stream target)
        {
            WasRead = true;
            target.Write(_content);
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            WasRead = true;
            return target.WriteAsync(_content, cancellationToken).AsTask();
        }

        public Stream OpenReadStream()
        {
            WasRead = true;
            return new MemoryStream(_content);
        }
    }
}
