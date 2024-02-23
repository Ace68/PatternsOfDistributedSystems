using Polly;

namespace BrewUp.Chaos;

internal class ChaosManager(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor) : IChaosManager
{
	private const string UserQueryParam = "user";

	private const string TestUser = "monkey";

	public ValueTask<bool> IsChaosEnabledAsync(ResilienceContext context)
	{
		if (environment.IsDevelopment() &&
			contextAccessor.HttpContext is { } devContext &&
			devContext.Request.Query.TryGetValue(UserQueryParam, out var devValues) &&
			devValues == TestUser)
		{
			// Enable chaos for 'monkey' user even in production 
			return ValueTask.FromResult(true);
		}

		// This condition is demonstrative and not recommended to use in real apps.
		if (environment.IsProduction() &&
			contextAccessor.HttpContext is { } httpContext &&
			httpContext.Request.Query.TryGetValue(UserQueryParam, out var values) &&
			values == TestUser)
		{
			// Enable chaos for 'monkey' user even in production 
			return ValueTask.FromResult(true);
		}

		return ValueTask.FromResult(false);
	}

	public ValueTask<double> GetInjectionRateAsync(ResilienceContext context)
	{
		if (environment.IsDevelopment())
		{
			return ValueTask.FromResult(0.05);
		}

		if (environment.IsProduction())
		{
			return ValueTask.FromResult(0.03);
		}

		return ValueTask.FromResult(0.0);
	}
}