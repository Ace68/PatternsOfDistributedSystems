﻿using System.Threading.RateLimiting;
using BrewUp.Sales.SharedKernel.Configuration;
using Microsoft.AspNetCore.RateLimiting;

namespace BrewUp.Sales.Rest.Modules;

public static class ModuleExtensions
{
	private static readonly IList<IModule> RegisteredModules = new List<IModule>();
	private static RateLimitSettings _rateLimitSettings = new();
	public static string RateLimitPolicy { get; private set; } = string.Empty;

	public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
	{
		builder.AddRateLimiterPolicy();
		
		var modules = DiscoverModules();
		foreach (var module in modules
					 .Where(m => m.IsEnabled)
					 .OrderBy(m => m.Order))
		{
			module.RegisterModule(builder);
			RegisteredModules.Add(module);
		}

		return builder;
	}

	public static WebApplication MapEndpoints(this WebApplication app)
	{
		foreach (var module in RegisteredModules)
		{
			module.MapEndpoints(app);
		}

		if (_rateLimitSettings.FixedWindow.Enabled
		    || _rateLimitSettings.SlidingWindow.Enabled
		    || _rateLimitSettings.TokenBucket.Enabled
		    || _rateLimitSettings.Concurrency.Enabled)
			app.UseRateLimiter();
		
		return app;
	}

	private static IServiceCollection AddRateLimiterPolicy(this WebApplicationBuilder builder)
	{
		_rateLimitSettings = builder.Configuration.GetSection("BrewUp:RateLimitSettings")
			.Get<RateLimitSettings>()!;
		
		if (_rateLimitSettings.FixedWindow.Enabled)
        {
            RateLimitPolicy = "fixed";
            builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: RateLimitPolicy, options =>
                {
                    options.PermitLimit = _rateLimitSettings.FixedWindow.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(_rateLimitSettings.FixedWindow.TimeWindowInSeconds);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.FixedWindow.QueueLimit;
                }));
        }

        if (_rateLimitSettings.SlidingWindow.Enabled)
        {
            RateLimitPolicy = "sliding";
            builder.Services.AddRateLimiter(_ => _
                .AddSlidingWindowLimiter(policyName: RateLimitPolicy, options =>
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
            RateLimitPolicy = "token";
            builder.Services.AddRateLimiter(_ => _
                .AddTokenBucketLimiter(policyName: RateLimitPolicy, options =>
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
            RateLimitPolicy = "Concurrency";
            builder.Services.AddRateLimiter(_ => _
                .AddConcurrencyLimiter(policyName: RateLimitPolicy, options =>
                {
                    options.PermitLimit = _rateLimitSettings.Concurrency.PermitLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = _rateLimitSettings.Concurrency.QueueLimit;
                }));
        }

		return builder.Services;
	}

	private static IEnumerable<IModule> DiscoverModules()
	{
		return typeof(IModule).Assembly
			.GetTypes()
			.Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
			.Select(Activator.CreateInstance)
			.Cast<IModule>();
	}
}