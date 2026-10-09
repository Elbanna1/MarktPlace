namespace Shared.DTOs.Admin;

public class UploadsDiagnosticsDto
{
    public bool StoredInsideApplicationDirectory { get; set; }

    public int TotalImages { get; set; }

    public int MissingImages { get; set; }

    public List<UploadsFolderDiagnosticsDto> Folders { get; set; } = new();

    public List<string> SampleMissingPaths { get; set; } = new();
}

public class UploadsFolderDiagnosticsDto
{
    public string Folder { get; set; } = default!;

    public int Total { get; set; }

    public int Missing { get; set; }
}
