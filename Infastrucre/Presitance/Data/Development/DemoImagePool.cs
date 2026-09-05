namespace Persistence.Data.Development;

internal sealed class DemoImagePool
{
    private readonly IReadOnlyList<DemoImageLibrary.DemoImage> _images;
    private int _cursor;

    public DemoImagePool(IReadOnlyList<DemoImageLibrary.DemoImage> images)
    {
        _images = images;
    }

    public int Count => _images.Count;

    public bool IsEmpty => _images.Count == 0;

    public IReadOnlyList<DemoImageLibrary.DemoImage> Take(int count)
    {
        if (_images.Count == 0 || count <= 0)
            return Array.Empty<DemoImageLibrary.DemoImage>();

        var wanted = Math.Min(count, _images.Count);
        var taken = new List<DemoImageLibrary.DemoImage>(wanted);

        for (var index = 0; index < wanted; index++)
        {
            taken.Add(_images[_cursor % _images.Count]);
            _cursor++;
        }

        return taken;
    }

    public DemoImageLibrary.DemoImage? TakeOne()
    {
        var taken = Take(1);
        return taken.Count == 0 ? null : taken[0];
    }
}
