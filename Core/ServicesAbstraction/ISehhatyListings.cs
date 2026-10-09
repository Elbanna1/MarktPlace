using Domain.Entities;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Sehhaty;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ISehhatyRepository<TListing, TImage, in TFilter>
    where TListing : SehhatyListing<TImage>
    where TImage : SehhatyListingImage
    where TFilter : SehhatyFilterParams
{
    Task<TListing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false);

    Task<TListing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<TListing> Items, int TotalCount)> GetPagedAsync(
        TFilter filter, CancellationToken cancellationToken = default);

    Task AddAsync(TListing listing, CancellationToken cancellationToken = default);

    Task AddImagesAsync(IEnumerable<TImage> images, CancellationToken cancellationToken = default);

    void RemoveImage(TImage image);

    void Update(TListing listing);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface ISehhatyListingService<in TCreate, in TUpdate, TDto, in TFilter>
    where TCreate : CreateSehhatyListingRequest
    where TUpdate : TCreate, ISehhatyGalleryEdit
    where TDto : SehhatyListingDto
    where TFilter : SehhatyFilterParams
{
    Task<TDto> CreateAsync(
        string userId, TCreate request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<TDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, TUpdate request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default);

    Task<PaginatedResult<TDto>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default);

    Task<TDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IDoctorService
    : ISehhatyListingService<CreateDoctorRequest, UpdateDoctorRequest, DoctorDto, DoctorFilterParams>;

public interface IMedicalLaboratoryService
    : ISehhatyListingService<CreateMedicalLaboratoryRequest, UpdateMedicalLaboratoryRequest,
        MedicalLaboratoryDto, MedicalLaboratoryFilterParams>;

public interface IDentalLaboratoryService
    : ISehhatyListingService<CreateDentalLaboratoryRequest, UpdateDentalLaboratoryRequest,
        DentalLaboratoryDto, DentalLaboratoryFilterParams>;

public interface IRadiologyLaboratoryService
    : ISehhatyListingService<CreateRadiologyLaboratoryRequest, UpdateRadiologyLaboratoryRequest,
        RadiologyLaboratoryDto, RadiologyLaboratoryFilterParams>;

public interface IMedicalCenterService
    : ISehhatyListingService<CreateMedicalCenterRequest, UpdateMedicalCenterRequest,
        MedicalCenterDto, MedicalCenterFilterParams>;

public interface IHospitalService
    : ISehhatyListingService<CreateHospitalRequest, UpdateHospitalRequest, HospitalDto, HospitalFilterParams>;
