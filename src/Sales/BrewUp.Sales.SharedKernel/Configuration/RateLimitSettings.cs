namespace BrewUp.Sales.SharedKernel.Configuration;

public class RateLimitSettings
{
    public FixedWindow FixedWindow { get; set; } = new();
    public SlidingWindow SlidingWindow { get; set; } = new();
    public TokenBucket TokenBucket { get; set; } = new();
    public Concurrency Concurrency { get; set; } = new();
}

public class FixedWindow
{
    public bool Enabled { get; set; } = false;
    public int PermitLimit { get; set; } = 4;
    public int TimeWindowInSeconds { get; set; } = 12;
    public int QueueLimit { get; set; } = 2;
}

public class SlidingWindow
{
    public bool Enabled { get; set; } = false;
    public int PermitLimit { get; set; } = 4;
    public int SecondsPerWindow { get; set; } = 30;
    public int SegmentsPerWindow { get; set; } = 3;
    public int QueueLimit { get; set; } = 2;
}

public class TokenBucket
{
    public bool Enabled { get; set; } = false;
    public int TokenLimit { get; set; } = 4;
    public int QueueLimit { get; set; } = 2;
    public int ReplenishmentPeriod { get; set; } = 30;
    public int TokensPerPeriod { get; set; } = 3;
    public bool AutoReplenishment { get; set; } = false;
}

public class Concurrency 
{
    public bool Enabled { get; set; } = false;
    public int PermitLimit { get; set; } = 4;
    public int QueueLimit { get; set; } = 2;
}