namespace Shared.DTOs.Lookups.Forms;

public class CreateAdFormSubmitDto
{
    public CreateAdFormSubmitDto() { }

    public CreateAdFormSubmitDto(string endpoint, string method = "POST",
        string contentType = "multipart/form-data", bool requiresAuthentication = true)
    {
        Endpoint = endpoint;
        Method = method;
        ContentType = contentType;
        RequiresAuthentication = requiresAuthentication;
    }

    public string Endpoint { get; set; } = default!;

    public string Method { get; set; } = "POST";

    public string ContentType { get; set; } = "multipart/form-data";

    public bool RequiresAuthentication { get; set; } = true;
}
