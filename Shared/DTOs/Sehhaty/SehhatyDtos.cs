using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Shared.DTOs.Sehhaty;

public class SehhatyImageDto
{
    public Guid Id { get; set; }

    public string Url { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}

public abstract class CreateSehhatyListingRequest
{
    public string Name { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public List<IFormFile> Images { get; set; } = new();
}

public interface ISehhatyGalleryEdit
{
    List<Guid> RemoveImageIds { get; set; }
}

public abstract class SehhatyListingDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string OwnerName { get; set; } = default!;

    public ListingModuleType ListingType { get; set; }

    public string ListingTypeName { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMapsUrl { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public List<SehhatyImageDto> Images { get; set; } = new();

    public string? PrimaryImageUrl { get; set; }

    public string ShareUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ApprovedAt
    {
        get => _approvedAt;

        set => _approvedAt = value is { } moment
            ? DateTime.SpecifyKind(moment, DateTimeKind.Utc)
            : null;
    }

    private DateTime? _approvedAt;

    [JsonIgnore]
    public abstract ListingModuleType StatsListingType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public abstract class SehhatyFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public string? Name { get; set; }

    public SehhatySortBy SortBy { get; set; } = SehhatySortBy.Newest;
}

public class CreateDoctorRequest : CreateSehhatyListingRequest
{
    public DoctorSpecialty? Specialty { get; set; }

    public string? OtherSpecialty { get; set; }
}

public class UpdateDoctorRequest : CreateDoctorRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class DoctorDto : SehhatyListingDto
{
    public DoctorSpecialty Specialty { get; set; }

    public string SpecialtyName { get; set; } = default!;

    public string? OtherSpecialty { get; set; }

    public string DisplaySpecialty { get; set; } = default!;

    public override ListingModuleType StatsListingType => ListingModuleType.Doctor;
}

public class DoctorFilterParams : SehhatyFilterParams
{
    public DoctorSpecialty? Specialty { get; set; }
}

public class CreateMedicalLaboratoryRequest : CreateSehhatyListingRequest;

public class UpdateMedicalLaboratoryRequest : CreateMedicalLaboratoryRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class MedicalLaboratoryDto : SehhatyListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.MedicalLaboratory;
}

public class MedicalLaboratoryFilterParams : SehhatyFilterParams;

public class CreateDentalLaboratoryRequest : CreateSehhatyListingRequest;

public class UpdateDentalLaboratoryRequest : CreateDentalLaboratoryRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class DentalLaboratoryDto : SehhatyListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.DentalLaboratory;
}

public class DentalLaboratoryFilterParams : SehhatyFilterParams;

public class CreateRadiologyLaboratoryRequest : CreateSehhatyListingRequest;

public class UpdateRadiologyLaboratoryRequest : CreateRadiologyLaboratoryRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class RadiologyLaboratoryDto : SehhatyListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.RadiologyLaboratory;
}

public class RadiologyLaboratoryFilterParams : SehhatyFilterParams;

public class CreateMedicalCenterRequest : CreateSehhatyListingRequest;

public class UpdateMedicalCenterRequest : CreateMedicalCenterRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class MedicalCenterDto : SehhatyListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.MedicalCenter;
}

public class MedicalCenterFilterParams : SehhatyFilterParams;

public class CreateHospitalRequest : CreateSehhatyListingRequest;

public class UpdateHospitalRequest : CreateHospitalRequest, ISehhatyGalleryEdit
{
    public List<Guid> RemoveImageIds { get; set; } = new();
}

public class HospitalDto : SehhatyListingDto
{
    public override ListingModuleType StatsListingType => ListingModuleType.Hospital;
}

public class HospitalFilterParams : SehhatyFilterParams;
