using Shared.DTOs.Admin;

namespace ServicesAbstraction;

public interface IUploadsDiagnostics
{
    Task<UploadsDiagnosticsDto> ScanAsync(CancellationToken cancellationToken = default);
}
