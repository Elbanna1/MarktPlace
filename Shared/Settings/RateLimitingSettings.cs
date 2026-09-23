namespace Shared.Settings;

public class RateLimitingSettings
{
    public const string SectionName = "RateLimiting";

    public bool Enabled { get; set; } = true;

    /// <summary>An address is a poor identity — a whole CGNAT block, campus or office can share
    /// one — so the anonymous ceiling is deliberately generous and only catches scraping.</summary>
    public RateLimitWindowSettings Anonymous { get; set; } = new()
    {
        PermitPerWindow = 1200,
        WindowSeconds = 60,
        SegmentsPerWindow = 6
    };

    /// <summary>A signed-in caller is partitioned by account, so the ceiling can be tighter than
    /// the anonymous one without a second user's traffic ever counting against it.</summary>
    public RateLimitWindowSettings Authenticated { get; set; } = new()
    {
        PermitPerWindow = 600,
        WindowSeconds = 60,
        SegmentsPerWindow = 6
    };

    /// <summary>Sign-in, registration and password reset: the credential-stuffing surface, kept
    /// strict on purpose.</summary>
    public RateLimitWindowSettings Auth { get; set; } = new()
    {
        PermitPerWindow = 20,
        WindowSeconds = 60,
        SegmentsPerWindow = 4
    };
}

public class RateLimitWindowSettings
{
    private int _permitPerWindow = 600;
    private int _windowSeconds = 60;
    private int _segmentsPerWindow = 6;
    private int _queueLimit;

    public int PermitPerWindow
    {
        get => _permitPerWindow;
        set => _permitPerWindow = value > 0 ? value : 1;
    }

    public int WindowSeconds
    {
        get => _windowSeconds;
        set => _windowSeconds = value > 0 ? value : 60;
    }

    /// <summary>How many slices the window is divided into. More slices means a caller that spends
    /// its budget recovers gradually instead of waiting out the whole window, which is what makes a
    /// sliding window kinder to ordinary bursts than the fixed window this replaced.</summary>
    public int SegmentsPerWindow
    {
        get => _segmentsPerWindow;
        set => _segmentsPerWindow = value is > 0 and <= 60 ? value : 6;
    }

    /// <summary>Kept at zero: queueing a request that is over the limit buys latency and holds a
    /// connection open, which is the opposite of what is wanted under abuse.</summary>
    public int QueueLimit
    {
        get => _queueLimit;
        set => _queueLimit = value >= 0 ? value : 0;
    }

    public TimeSpan Window => TimeSpan.FromSeconds(WindowSeconds);
}
