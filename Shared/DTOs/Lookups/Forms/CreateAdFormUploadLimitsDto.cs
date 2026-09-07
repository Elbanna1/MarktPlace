namespace Shared.DTOs.Lookups.Forms;

public class CreateAdFormUploadLimitsDto
{
    public int MaxRequestSizeMb { get; set; }

    public int MaxImages { get; set; }

    public int MaxImageSizeMb { get; set; }

    public int MaxVideoSizeMb { get; set; }

    public int MaxDocumentSizeMb { get; set; }

    public string TooLargeMessage { get; set; } = default!;
}
