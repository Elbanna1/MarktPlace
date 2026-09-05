using Shared.DTOs.Lookups.Read;

namespace Services.ReadConfigs;

public sealed record ReadConfigSchema(string Module, IReadOnlyList<ReadOperationDto> Operations)
{
    public ReadOperationDto? Find(string key) =>
        Operations.FirstOrDefault(operation => operation.Key == key);

    public IReadOnlyDictionary<string, ReadOperationDto> ToMap() =>
        Operations.ToDictionary(operation => operation.Key);
}
