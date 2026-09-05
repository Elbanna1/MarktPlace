namespace MarkatPlace.Tests;

internal static class RepositoryRoot
{
    public static string Path { get; } = Find();

    private static string Find()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(System.IO.Path.Combine(directory.FullName, "MarkatPlace.slnx")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new InvalidOperationException(
            "Could not find MarkatPlace.slnx above " + AppContext.BaseDirectory);
    }

    public static IEnumerable<string> SourceFiles() =>
        Directory.EnumerateFiles(Path, "*.cs", SearchOption.AllDirectories)
            .Select(file => file.Replace('\\', '/'))
            .Where(file => !file.Contains("/obj/", StringComparison.Ordinal)
                        && !file.Contains("/bin/", StringComparison.Ordinal)
                        && !file.Contains("/.vs/", StringComparison.Ordinal));

    public static string Relative(string file) =>
        file.Replace('\\', '/').Replace(Path.Replace('\\', '/') + "/", string.Empty);

    public static string ReadFile(string relativePath) =>
        File.ReadAllText(System.IO.Path.Combine(Path, relativePath.Replace('/', System.IO.Path.DirectorySeparatorChar)));
}
