namespace Shogun.Server.Tests.Support;

/// <summary>
/// A folder that exists for one test and is deleted afterwards, so tests
/// never touch the real data folder and never see each other's files.
/// </summary>
public sealed class TempFolder : IDisposable
{
    public string Path { get; } =
        System.IO.Path.Combine(System.IO.Path.GetTempPath(), "shogun-tests", Guid.NewGuid().ToString("N"));

    public TempFolder()
    {
        Directory.CreateDirectory(Path);
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, recursive: true);
        }
    }
}
