namespace Shared.DTOs.Advertisements;

public class UploadImageModel
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Length { get; set; }
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
