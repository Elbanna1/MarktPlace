using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Home;
using Shared.Enums;

namespace Services;

public class HomeService : IHomeService
{
    private readonly IAdminHomeRepository _repository;
    private readonly ILookupService _lookups;

    public HomeService(IAdminHomeRepository repository, ILookupService lookups)
    {
        _repository = repository;
        _lookups = lookups;
    }

    public async Task<HomeConfigurationDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var sections = await _repository.GetSectionsAsync(cancellationToken);

        var visible = sections
            .Where(section => section.IsVisible)
            .Select(Map)
            .ToList();

        var categories = await _lookups.GetCategoriesAsync(cancellationToken);

        return new HomeConfigurationDto
        {
            Sections = visible,
            Categories = categories
        };
    }

    private static HomeSectionDto Map(HomeSection section) =>
        new()
        {
            Id = section.Id,
            Key = section.Key,
            Type = section.Type,
            TypeName = section.Type.ToString(),
            Title = section.Title,
            TitleEn = section.TitleEn,
            Subtitle = section.Subtitle,
            SortOrder = section.SortOrder,
            ImageUrl = section.ImageUrl,
            LinkUrl = section.LinkUrl,
            LinkText = section.LinkText,
            CategoryId = section.CategoryId,
            CategoryName = section.Category?.NameAr,
            ItemCount = section.ItemCount,

            BannerLocation = section.Type switch
            {
                HomeSectionType.Slider1 => Shared.Enums.BannerLocation.HomeSlider1,
                HomeSectionType.Slider2 => Shared.Enums.BannerLocation.HomeSlider2,
                _ => null
            }
        };
}
