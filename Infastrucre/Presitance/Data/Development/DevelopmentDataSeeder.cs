using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Settings;

namespace Persistence.Data.Development;

public static class DevelopmentDataSeeder
{
    private const string DemoPassword = "Demo@12345";
    private const string DemoEmailDomain = "demo.markatplace.local";

    private const int MinImagesPerItem = 3;
    private const int MaxImagesPerAdvertisement = 7;

    public sealed record DemoSeedReport
    {
        public bool Skipped { get; init; }
        public string? SkipReason { get; init; }

        public int ImagesDownloaded { get; init; }
        public int ImagesReused { get; init; }
        public int ImagesFailed { get; init; }
        public IReadOnlyDictionary<string, int> ImagesByFolder { get; init; } =
            new Dictionary<string, int>();

        public int UsersCreated { get; init; }
        public int ProfileImagesAssigned { get; init; }
        public int AdvertisementsCreated { get; init; }
        public int AdvertisementsIllustrated { get; init; }
        public int WorkshopsCreated { get; init; }
        public int WorkshopsIllustrated { get; init; }
        public int CraftsmenCreated { get; init; }
        public int CraftsmenIllustrated { get; init; }
        public int PostsCreated { get; init; }
        public int PostsIllustrated { get; init; }

        public int TotalImagesLinked { get; init; }
    }

    public static async Task<DemoSeedReport> SeedAsync(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var environment = services.GetRequiredService<IHostEnvironment>();

        if (!environment.IsDevelopment())
        {
            return new DemoSeedReport
            {
                Skipped = true,
                SkipReason =
                    $"The demo data seeder only runs in the Development environment; the current " +
                    $"environment is {environment.EnvironmentName}."
            };
        }

        if (configuration.GetValue("DemoData:Enabled", false) is false)
            return new DemoSeedReport { Skipped = true, SkipReason = "DemoData:Enabled is false." };

        var db = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var fileService = services.GetRequiredService<IFileService>();

        var library = DemoImageLibrary.Create(fileService, ResolveUploadsRoot(services), logger);
        var pools = await DownloadPoolsAsync(library, logger, cancellationToken);
        library.SaveManifest();

        if (pools.Values.All(pool => pool.IsEmpty))
        {
            return new DemoSeedReport
            {
                Skipped = true,
                SkipReason = "No demo image could be downloaded or reused (is the machine offline?).",
                ImagesFailed = library.FailedCount
            };
        }

        var (users, usersCreated) = await SeedUsersAsync(userManager, logger, cancellationToken);

        if (users.Count == 0)
        {
            return new DemoSeedReport
            {
                Skipped = true,
                SkipReason = "No demo user is available to own the demo records.",
                ImagesDownloaded = library.DownloadedCount,
                ImagesReused = library.ReusedCount,
                ImagesFailed = library.FailedCount
            };
        }

        var profileImagesAssigned = await AssignProfileImagesAsync(db, pools, cancellationToken);

        var advertisementsCreated = await SeedAdvertisementsAsync(db, users, cancellationToken);
        var workshopsCreated = await SeedWorkshopsAsync(db, users, cancellationToken);
        var craftsmenCreated = await SeedCraftsmenAsync(db, users, cancellationToken);
        var postsCreated = await SeedPostsAsync(db, users, cancellationToken);

        var advertisementsIllustrated = await IllustrateAdvertisementsAsync(db, pools, cancellationToken);
        var workshopsIllustrated = await IllustrateWorkshopsAsync(db, pools, cancellationToken);
        var craftsmenIllustrated = await IllustrateCraftsmenAsync(db, pools, cancellationToken);
        var postsIllustrated = await IllustratePostsAsync(db, pools, cancellationToken);

        var totalLinked =
            await db.AdvertisementImages.CountAsync(cancellationToken) +
            await db.WorkshopImages.CountAsync(cancellationToken) +
            await db.CraftsmanImages.CountAsync(cancellationToken) +
            await db.LostFoundImages.CountAsync(cancellationToken) +
            await db.Users.CountAsync(user => user.ProfileImageUrl != null, cancellationToken);

        return new DemoSeedReport
        {
            ImagesDownloaded = library.DownloadedCount,
            ImagesReused = library.ReusedCount,
            ImagesFailed = library.FailedCount,
            ImagesByFolder = library.CountsByFolder(),
            UsersCreated = usersCreated,
            ProfileImagesAssigned = profileImagesAssigned,
            AdvertisementsCreated = advertisementsCreated,
            AdvertisementsIllustrated = advertisementsIllustrated,
            WorkshopsCreated = workshopsCreated,
            WorkshopsIllustrated = workshopsIllustrated,
            CraftsmenCreated = craftsmenCreated,
            CraftsmenIllustrated = craftsmenIllustrated,
            PostsCreated = postsCreated,
            PostsIllustrated = postsIllustrated,
            TotalImagesLinked = totalLinked
        };
    }

    private static string ResolveUploadsRoot(IServiceProvider services)
    {
        var configured = services.GetService<IOptions<FileStorageSettings>>()?.Value.UploadsRootPath;

        if (!string.IsNullOrWhiteSpace(configured))
            return configured;

        var environment = services.GetRequiredService<IWebHostEnvironment>();
        var webRoot = string.IsNullOrWhiteSpace(environment.WebRootPath)
            ? Path.Combine(environment.ContentRootPath, "wwwroot")
            : environment.WebRootPath;

        return Path.GetFullPath(Path.Combine(webRoot, ImageConstants.UploadsRootFolder));
    }

    private static async Task<Dictionary<string, DemoImagePool>> DownloadPoolsAsync(
        DemoImageLibrary library, ILogger logger, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(DemoImageCatalog.UserAgent);

        var pools = new Dictionary<string, DemoImagePool>(StringComparer.Ordinal);

        foreach (var (topicName, topic) in DemoImageCatalog.Topics)
        {
            var images = await library.EnsureAsync(topic.Sources, topic.Folder, httpClient, cancellationToken);
            pools[topicName] = new DemoImagePool(images);

            if (images.Count == 0)
                logger.LogWarning("Demo image topic {Topic} produced no usable image.", topicName);
        }

        return pools;
    }

    private static DemoImagePool Pool(IReadOnlyDictionary<string, DemoImagePool> pools, string topic) =>
        pools.TryGetValue(topic, out var pool) ? pool : new DemoImagePool(Array.Empty<DemoImageLibrary.DemoImage>());

    private static async Task<(List<ApplicationUser> Users, int Created)> SeedUsersAsync(
        UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken cancellationToken)
    {
        var users = new List<ApplicationUser>(DemoRecords.Users.Length);
        var created = 0;

        foreach (var demo in DemoRecords.Users)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await userManager.FindByNameAsync(demo.UserName);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = demo.UserName,
                    Email = $"{demo.UserName}@{DemoEmailDomain}",
                    EmailConfirmed = true,
                    FirstName = demo.FirstName,
                    SecondName = demo.SecondName,
                    Governorate = LocationConstants.Governorate,
                    Center = demo.Center,
                    PhoneNumber = demo.Phone,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, DemoPassword);
                if (!result.Succeeded)
                {
                    logger.LogWarning("Demo user {UserName} could not be created: {Errors}",
                        demo.UserName, string.Join("; ", result.Errors.Select(error => error.Description)));
                    continue;
                }

                created++;
            }

            users.Add(user);
        }

        return (users, created);
    }

    private static async Task<int> AssignProfileImagesAsync(
        AppDbContext db, IReadOnlyDictionary<string, DemoImagePool> pools, CancellationToken cancellationToken)
    {
        var malePool = Pool(pools, DemoImageCatalog.Names.AvatarMale);
        var femalePool = Pool(pools, DemoImageCatalog.Names.AvatarFemale);

        if (malePool.IsEmpty && femalePool.IsEmpty)
            return 0;

        var femaleUserNames = DemoRecords.Users
            .Where(demo => !demo.IsMale)
            .Select(demo => demo.UserName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var users = await db.Users
            .Where(user => user.ProfileImageUrl == null)
            .OrderBy(user => user.CreatedAt)
            .ToListAsync(cancellationToken);

        var assigned = 0;

        foreach (var user in users)
        {
            var isFemale = user.UserName is not null && femaleUserNames.Contains(user.UserName);
            var pool = isFemale ? femalePool : malePool;

            var image = pool.TakeOne() ?? (isFemale ? malePool : femalePool).TakeOne();
            if (image is null)
                continue;

            user.ProfileImagePath = image.RelativePath;
            user.ProfileImageUrl = image.Url;
            user.UpdatedAt = DateTime.UtcNow;
            assigned++;
        }

        if (assigned > 0)
            await db.SaveChangesAsync(cancellationToken);

        return assigned;
    }

    private static async Task<int> SeedAdvertisementsAsync(
        AppDbContext db, List<ApplicationUser> users, CancellationToken cancellationToken)
    {
        if (await db.Advertisements.AnyAsync(cancellationToken))
            return 0;

        var featureIds = await db.Features
            .OrderBy(feature => feature.Id)
            .Select(feature => feature.Id)
            .Take(24)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var random = new Random(20260724);

        for (var index = 0; index < DemoRecords.Advertisements.Length; index++)
        {
            var demo = DemoRecords.Advertisements[index];
            var owner = users[index % users.Count];

            var createdAt = now.AddDays(-(index % (AdvertisementConstants.ActiveDurationDays - 10) + 1));

            var advertisement = new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = demo.Title,
                Description = demo.Description,
                Price = demo.Price,
                Negotiable = index % 3 != 0,
                ListingType = demo.Listing,
                CategoryId = (int)CategoryType.Cars,
                SubCategoryId = (int)demo.SubCategory,
                Governorate = LocationConstants.Governorate,
                Center = owner.Center,
                PhoneNumber = owner.PhoneNumber!,
                OwnerId = owner.Id,
                Status = AdvertisementStatus.Active,
                Views = random.Next(15, 400),
                CreatedAt = createdAt,
                PublishedAt = createdAt,
                ExpireAt = createdAt.AddDays(AdvertisementConstants.ActiveDurationDays),
                Brand = demo.Brand,
                Model = demo.Model,
                ManufacturingYear = demo.Year,
                Color = demo.Color,
                Kilometers = demo.Kilometers,
                Transmission = demo.Transmission,
                FuelType = demo.Fuel,
                Condition = demo.Condition,
                EngineCC = demo.EngineCc,
                BodyType = demo.Body,
                DoorsCount = demo.Doors,
                CoolingType = demo.Cooling,
                MachineType = demo.MachineType,
                WorkingHours = demo.WorkingHours,
                PowerValue = demo.PowerHorsepower,
                PowerUnit = demo.PowerHorsepower is null ? null : PowerUnit.Horsepower,

                RentSystems = CarCatalog.Combine(
                    demo.RentSystem is { } system ? new[] { system } : null),
                DailyPrice = demo.RentSystem == RentSystem.Daily ? demo.Price : null,
                WeeklyPrice = demo.RentSystem == RentSystem.Weekly ? demo.Price : null,
                MonthlyPrice = demo.RentSystem == RentSystem.Monthly ? demo.Price : null,
                MinimumRentPeriod = demo.RentSystem is null ? null : 1,
                DriverIncluded = demo.RentSystem is null ? null : index % 2 == 0,

                DamageLevel = demo.DamageLevel,
                IsMoving = demo.DamageLevel is null ? null : true,
                EngineWorks = demo.DamageLevel is null ? null : true,
                SellAsParts = demo.DamageLevel is null ? null : false,

                InterestedIn = demo.InterestedIn,
                DifferencePayment = demo.InterestedIn is null ? null : true,

                LicenseStatus = demo.SubCategory == SubCategoryType.HeavyEquipment
                    ? Shared.Enums.LicenseStatus.NotRequired
                    : Shared.Enums.LicenseStatus.Valid,
                LicenseExpiryDate = demo.SubCategory == SubCategoryType.HeavyEquipment
                    ? null
                    : createdAt.AddMonths(9),

                DetailedAddress = $"{owner.Center} - {demo.Title}"
            };

            if (featureIds.Count > 0 && demo.SubCategory is SubCategoryType.Private or SubCategoryType.Taxi)
            {
                foreach (var featureId in featureIds.OrderBy(_ => random.Next()).Take(6).Distinct())
                    advertisement.AdvertisementFeatures.Add(new AdvertisementFeature { FeatureId = featureId });
            }

            db.Advertisements.Add(advertisement);
        }

        await db.SaveChangesAsync(cancellationToken);
        return DemoRecords.Advertisements.Length;
    }

    private static async Task<int> IllustrateAdvertisementsAsync(
        AppDbContext db, IReadOnlyDictionary<string, DemoImagePool> pools, CancellationToken cancellationToken)
    {
        var advertisements = await db.Advertisements
            .Where(advertisement => !advertisement.Images.Any())
            .OrderBy(advertisement => advertisement.CreatedAt)
            .ToListAsync(cancellationToken);

        if (advertisements.Count == 0)
            return 0;

        var interior = Pool(pools, DemoImageCatalog.Names.CarInterior);
        var random = new Random(20260724);
        var illustrated = 0;

        foreach (var advertisement in advertisements)
        {
            var demo = DemoRecords.Advertisements.FirstOrDefault(candidate =>
                candidate.Title == advertisement.Title);

            var topic = demo?.Topic ?? TopicForSubCategory(advertisement.SubCategoryId);
            var pool = Pool(pools, topic);

            if (pool.IsEmpty)
                continue;

            var wanted = random.Next(MinImagesPerItem, MaxImagesPerAdvertisement + 1);
            var withInterior = demo?.WithInterior ?? IsCar(advertisement.SubCategoryId);

            var images = pool.Take(withInterior ? Math.Max(MinImagesPerItem, wanted - 2) : wanted).ToList();

            if (withInterior && !interior.IsEmpty)
                images.AddRange(interior.Take(2));

            if (images.Count == 0)
                continue;

            for (var index = 0; index < images.Count; index++)
            {
                db.AdvertisementImages.Add(new AdvertisementImage
                {
                    Id = Guid.NewGuid(),
                    AdvertisementId = advertisement.Id,
                    FileName = images[index].FileName,
                    ImagePath = images[index].RelativePath,
                    ImageUrl = images[index].Url,
                    IsPrimary = index == 0,
                    CreatedAt = advertisement.CreatedAt
                });
            }

            illustrated++;
        }

        await db.SaveChangesAsync(cancellationToken);
        return illustrated;
    }

    private static bool IsCar(int subCategoryId) =>
        subCategoryId is (int)SubCategoryType.Private or (int)SubCategoryType.Taxi;

    private static string TopicForSubCategory(int subCategoryId) => subCategoryId switch
    {
        (int)SubCategoryType.Taxi => DemoImageCatalog.Names.CarTaxi,
        (int)SubCategoryType.Motorcycles => DemoImageCatalog.Names.Motorcycle,
        (int)SubCategoryType.HeavyEquipment => DemoImageCatalog.Names.HeavyEquipment,
        _ => DemoImageCatalog.Names.CarSedan
    };

    private static async Task<int> SeedWorkshopsAsync(
        AppDbContext db, List<ApplicationUser> users, CancellationToken cancellationToken)
    {
        if (await db.Workshops.AnyAsync(cancellationToken))
            return 0;

        var now = DateTime.UtcNow;

        for (var index = 0; index < DemoRecords.Workshops.Length; index++)
        {
            var demo = DemoRecords.Workshops[index];
            var owner = users[index % users.Count];

            db.Workshops.Add(new Workshop
            {
                Id = Guid.NewGuid(),
                UserId = owner.Id,
                Name = demo.Name,
                WorkshopType = demo.Type,
                OtherWorkshopType = demo.OtherType,
                Governorate = LocationConstants.Governorate,
                Center = owner.Center,
                Address = demo.Address,
                PhoneNumber = owner.PhoneNumber!,
                WhatsApp = owner.PhoneNumber!,
                Email = owner.Email,
                AdTitle = demo.AdTitle,
                AdDescription = demo.AdDescription,
                CreatedAt = now.AddDays(-(index + 1) * 3)
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return DemoRecords.Workshops.Length;
    }

    private static async Task<int> IllustrateWorkshopsAsync(
        AppDbContext db, IReadOnlyDictionary<string, DemoImagePool> pools, CancellationToken cancellationToken)
    {
        var workshops = await db.Workshops
            .Where(workshop => !workshop.Images.Any())
            .OrderBy(workshop => workshop.CreatedAt)
            .ToListAsync(cancellationToken);

        if (workshops.Count == 0)
            return 0;

        var illustrated = 0;

        foreach (var workshop in workshops)
        {
            var topic = DemoRecords.Workshops
                .FirstOrDefault(demo => demo.Name == workshop.Name)?.Topic
                ?? DemoImageCatalog.Names.WorkshopCarpentry;

            var pool = Pool(pools, topic);
            var images = pool.Take(MinImagesPerItem + 1);

            if (images.Count == 0)
                continue;

            for (var index = 0; index < images.Count; index++)
            {
                db.WorkshopImages.Add(new WorkshopImage
                {
                    Id = Guid.NewGuid(),
                    WorkshopId = workshop.Id,
                    FileName = images[index].FileName,
                    ImagePath = images[index].RelativePath,
                    ImageUrl = images[index].Url,
                    IsPrimary = index == 0,
                    CreatedAt = workshop.CreatedAt
                });
            }

            illustrated++;
        }

        await db.SaveChangesAsync(cancellationToken);
        return illustrated;
    }

    private static async Task<int> SeedCraftsmenAsync(
        AppDbContext db, List<ApplicationUser> users, CancellationToken cancellationToken)
    {
        if (await db.Craftsmen.AnyAsync(cancellationToken))
            return 0;

        var now = DateTime.UtcNow;

        for (var index = 0; index < DemoRecords.Craftsmen.Length; index++)
        {
            var demo = DemoRecords.Craftsmen[index];
            var owner = users[(index + 3) % users.Count];

            db.Craftsmen.Add(new Craftsman
            {
                Id = Guid.NewGuid(),
                UserId = owner.Id,
                Name = demo.Name,
                Specialization = demo.Specialization,
                ExperienceLevel = demo.Experience,
                Governorate = LocationConstants.Governorate,
                Center = owner.Center,
                Address = demo.Address,
                PhoneNumber = owner.PhoneNumber!,
                WhatsApp = owner.PhoneNumber!,
                Email = owner.Email,
                AdTitle = demo.AdTitle,
                AdDescription = demo.AdDescription,
                CreatedAt = now.AddDays(-(index + 1) * 4)
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return DemoRecords.Craftsmen.Length;
    }

    private static async Task<int> IllustrateCraftsmenAsync(
        AppDbContext db, IReadOnlyDictionary<string, DemoImagePool> pools, CancellationToken cancellationToken)
    {
        var craftsmen = await db.Craftsmen
            .Where(craftsman => !craftsman.Images.Any())
            .OrderBy(craftsman => craftsman.CreatedAt)
            .ToListAsync(cancellationToken);

        if (craftsmen.Count == 0)
            return 0;

        var illustrated = 0;

        foreach (var craftsman in craftsmen)
        {
            var demo = DemoRecords.Craftsmen.FirstOrDefault(candidate => candidate.Name == craftsman.Name);
            var portraitPool = Pool(pools, demo?.PortraitTopic ?? DemoImageCatalog.Names.CraftsmanTechnician);
            var workPool = Pool(pools, demo?.WorkTopic ?? DemoImageCatalog.Names.WorkshopCarpentry);

            var images = new List<DemoImageLibrary.DemoImage>();

            var portrait = portraitPool.TakeOne();
            if (portrait is not null)
                images.Add(portrait);

            images.AddRange(workPool.Take(portrait is null ? MinImagesPerItem : MinImagesPerItem - 1));

            if (images.Count == 0)
                continue;

            for (var index = 0; index < images.Count; index++)
            {
                db.CraftsmanImages.Add(new CraftsmanImage
                {
                    Id = Guid.NewGuid(),
                    CraftsmanId = craftsman.Id,
                    FileName = images[index].FileName,
                    ImagePath = images[index].RelativePath,
                    ImageUrl = images[index].Url,
                    IsPrimary = index == 0,
                    CreatedAt = craftsman.CreatedAt
                });
            }

            illustrated++;
        }

        await db.SaveChangesAsync(cancellationToken);
        return illustrated;
    }

    private static async Task<int> SeedPostsAsync(
        AppDbContext db, List<ApplicationUser> users, CancellationToken cancellationToken)
    {
        if (await db.LostFoundPosts.AnyAsync(cancellationToken))
            return 0;

        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        for (var index = 0; index < DemoRecords.Posts.Length; index++)
        {
            var demo = DemoRecords.Posts[index];
            var owner = users[(index + 5) % users.Count];

            db.LostFoundPosts.Add(new LostFoundPost
            {
                Id = Guid.NewGuid(),
                UserId = owner.Id,
                PostType = demo.Type,
                Status = PostStatus.Active,
                Name = $"{owner.FirstName} {owner.SecondName}",
                ItemName = demo.ItemName,
                Description = demo.Description,
                PhoneNumber = owner.PhoneNumber!,
                Governorate = LocationConstants.Governorate,
                Center = owner.Center,
                LostDate = demo.Type == PostType.Lost ? today.AddDays(-demo.DaysAgo) : null,
                FoundDate = demo.Type == PostType.Found ? today.AddDays(-demo.DaysAgo) : null,
                CreatedAt = now.AddDays(-demo.DaysAgo)
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return DemoRecords.Posts.Length;
    }

    private static async Task<int> IllustratePostsAsync(
        AppDbContext db, IReadOnlyDictionary<string, DemoImagePool> pools, CancellationToken cancellationToken)
    {
        var posts = await db.LostFoundPosts
            .Where(post => !post.Images.Any())
            .OrderBy(post => post.CreatedAt)
            .ToListAsync(cancellationToken);

        if (posts.Count == 0)
            return 0;

        var illustrated = 0;

        foreach (var post in posts)
        {
            var topic = DemoRecords.Posts
                .FirstOrDefault(demo => demo.ItemName == post.ItemName)?.Topic
                ?? DemoImageCatalog.Names.ItemBag;

            var images = Pool(pools, topic).Take(2);

            if (images.Count == 0)
                continue;

            for (var index = 0; index < images.Count; index++)
            {
                db.LostFoundImages.Add(new LostFoundImage
                {
                    Id = Guid.NewGuid(),
                    PostId = post.Id,
                    FileName = images[index].FileName,
                    ImagePath = images[index].RelativePath,
                    ImageUrl = images[index].Url,
                    IsPrimary = index == 0,
                    CreatedAt = post.CreatedAt
                });
            }

            illustrated++;
        }

        await db.SaveChangesAsync(cancellationToken);
        return illustrated;
    }
}
