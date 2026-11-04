using Shared.Enums;

namespace Shared.DTOs.Lookups.Forms;

public class CreateAdFormPaymentDto
{
    public bool IsSubjectToPayment { get; set; }

    public ListingModuleType AdvertisementType { get; set; }

    public string AdvertisementTypeKey { get; set; } = default!;

    public string ConfigEndpoint { get; set; } = default!;

    public string SubmitEndpoint { get; set; } = default!;

    public string ResubmitEndpoint { get; set; } = default!;

    public string StatusEndpoint { get; set; } = default!;

    public string Method { get; set; } = "POST";

    public string ContentType { get; set; } = default!;

    public string Section { get; set; } = default!;

    public IReadOnlyList<FormFieldDto> Fields { get; set; } = Array.Empty<FormFieldDto>();
}
