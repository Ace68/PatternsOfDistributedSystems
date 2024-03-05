using System.Threading.RateLimiting;
using BrewUp.Sales.SharedKernel.Configuration;
using Microsoft.AspNetCore.RateLimiting;

namespace BrewUp.Sales.Rest.Modules;

public class RateLimitModule : IModule
{
    public bool IsEnabled => false;
    public int Order => 0;
    
    private RateLimitSettings _rateLimitSettings = new();
    private string _rateLimitPolicy = string.Empty;
    
    public IServiceCollection RegisterModule(WebApplicationBuilder builder)
    {
        
        _rateLimitSettings = builder.Configuration.GetSection("BrewUp:RateLimitSettings")
            .Get<RateLimitSettings>()!;

        if (_rateLimitSettings.FixedWindow.Enabled)
        {
            _rateLimitPolicy = "fixed";
            builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: _rateLimitPolicy, options =>
                {
                    options.PermitLimit = _rateLimitSettings.FixedWindow.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(_rateLimitSettings.FixedWindow.TimeWindowInSeconds);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.FixedWindow.QueueLimit;
                }));
        }

        if (_rateLimitSettings.SlidingWindow.Enabled)
        {
            _rateLimitPolicy = "sliding";
            builder.Services.AddRateLimiter(_ => _
                .AddSlidingWindowLimiter(policyName: _rateLimitPolicy, options =>
                {
                    options.PermitLimit = _rateLimitSettings.SlidingWindow.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(_rateLimitSettings.SlidingWindow.SecondsPerWindow);
                    options.SegmentsPerWindow = _rateLimitSettings.SlidingWindow.SegmentsPerWindow;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.SlidingWindow.QueueLimit;
                }));
        }

        if (_rateLimitSettings.TokenBucket.Enabled)
        {
            _rateLimitPolicy = "token";
            builder.Services.AddRateLimiter(_ => _
                .AddTokenBucketLimiter(policyName: _rateLimitPolicy, options =>
                {
                    options.TokenLimit = _rateLimitSettings.TokenBucket.TokenLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.TokenBucket.QueueLimit;
                    options.ReplenishmentPeriod = TimeSpan.FromSeconds(_rateLimitSettings.TokenBucket.ReplenishmentPeriod);
                    options.TokensPerPeriod = _rateLimitSettings.TokenBucket.TokensPerPeriod;
                    options.AutoReplenishment = _rateLimitSettings.TokenBucket.AutoReplenishment;
                }));
        }

        if (_rateLimitSettings.Concurrency.Enabled)
        {
            _rateLimitPolicy = "Concurrency";
            builder.Services.AddRateLimiter(_ => _
                .AddConcurrencyLimiter(policyName: _rateLimitPolicy, options =>
                {
                    options.PermitLimit = _rateLimitSettings.Concurrency.PermitLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.Concurrency.QueueLimit;
                }));
        }
        
        return builder.Services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        if (_rateLimitSettings.FixedWindow.Enabled
            || _rateLimitSettings.SlidingWindow.Enabled
            || _rateLimitSettings.TokenBucket.Enabled
            || _rateLimitSettings.Concurrency.Enabled)
        {
            //app.MapRateLimiter();
        }

        return endpoints;
    }
}