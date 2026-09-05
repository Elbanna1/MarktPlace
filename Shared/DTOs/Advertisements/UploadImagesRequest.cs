using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Advertisements;

public class UploadImagesRequest
{
    public List<IFormFile> Images { get; set; } = new();
}
