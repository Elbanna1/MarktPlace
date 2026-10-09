using System.Reflection;
using ServicesAbstraction;
using Shared.DTOs.Advertisements;

namespace MarkatPlace.Tests;

internal sealed class StubFileService : IFileService
{
    public Task<StoredFile> SaveAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
        Task.FromResult(Stored(file, folder));

    public Task<IReadOnlyList<StoredFile>> SaveImagesAsync(
        IReadOnlyList<UploadImageModel> files, string folder, int existingCount = 0,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<StoredFile>>(files.Select(file => Stored(file, folder)).ToList());

    public Task<StoredFile> SaveDocumentAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
        Task.FromResult(Stored(file, folder));

    public Task<StoredFile> SaveVideoAsync(UploadImageModel file, string folder, CancellationToken cancellationToken = default) =>
        Task.FromResult(Stored(file, folder));

    public void Delete(string relativePath)
    {
    }

    public string BuildPublicUrl(string relativePath) => "https://example.test" + relativePath;

    private static StoredFile Stored(UploadImageModel file, string folder)
    {
        var path = $"/uploads/{folder}/{Guid.NewGuid():N}.jpg";
        return new StoredFile { FileName = file.FileName, RelativePath = path, Url = "https://example.test" + path };
    }
}

public class SilentProxy<T> : DispatchProxy where T : class
{
    public static T Create() => DispatchProxy.Create<T, SilentProxy<T>>();

    public static SilentProxy<T> Of(T proxy) => (SilentProxy<T>)(object)proxy;

    public List<(string Method, object?[] Args)> Calls { get; } = new();

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        Calls.Add((targetMethod!.Name, args ?? []));

        var returnType = targetMethod!.ReturnType;

        if (returnType == typeof(Task))
            return Task.CompletedTask;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var result = returnType.GetGenericArguments()[0];

            return typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(result)
                .Invoke(null, [result.IsValueType ? Activator.CreateInstance(result) : null]);
        }

        return returnType.IsValueType && returnType != typeof(void) ? Activator.CreateInstance(returnType) : null;
    }
}
