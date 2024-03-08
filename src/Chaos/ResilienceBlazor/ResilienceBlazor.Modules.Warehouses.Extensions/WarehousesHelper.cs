using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Simmy;
using Polly.Simmy.Fault;
using Polly.Simmy.Latency;
using Polly.Simmy.Outcomes;
using ResilienceBlazor.Shared.Configuration;
using System.Net.Http.Headers;

namespace ResilienceBlazor.Modules.Warehouses.Extensions;

public static class WarehousesHelper
{
	public static IServiceCollection AddResilienceWarehousesModule(this IServiceCollection services, AppConfiguration configuration)
	{
		services.AddScoped<IWarehousesService, WarehousesService>();

		var httpClientBuilder = services.AddHttpClient<ResilienceWarehousesClient>(client =>
			{
				client.BaseAddress = new Uri(configuration.BrewUpWarehousesUri);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			});

		// Configure the standard resilience handler
		httpClientBuilder
			.AddStandardResilienceHandler()
			.Configure(options =>
			{
				// Update attempt timeout to 1 second
				options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);

				// Update circuit breaker to handle transient errors and InvalidOperationException
				options.CircuitBreaker.ShouldHandle = args => args.Outcome switch
				{
					{ } outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
					{ Exception: InvalidOperationException } => PredicateResult.True(),
					_ => PredicateResult.False()
				};

				// Update retry strategy to handle transient errors and InvalidOperationException
				options.Retry.ShouldHandle = args => args.Outcome switch
				{
					{ } outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
					{ Exception: InvalidOperationException } => PredicateResult.True(),
					_ => PredicateResult.False()
				};
			});

		// Configure the chaos injection
		httpClientBuilder.AddResilienceHandler("warehouses-chaos", (pipelineBuilder, context) =>
		{
			pipelineBuilder
				.AddChaosLatency(new ChaosLatencyStrategyOptions
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					Latency = TimeSpan.FromSeconds(5)
				})
				.AddChaosFault(new ChaosFaultStrategyOptions
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					FaultGenerator = new FaultGenerator().AddException(() => new InvalidOperationException("Chaos strategy injection for Warehouses!"))
				})
				.AddChaosOutcome(new ChaosOutcomeStrategyOptions<HttpResponseMessage>
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					OutcomeGenerator = new OutcomeGenerator<HttpResponseMessage>().AddResult(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError))
				});
		});

		return services;
	}

	public static IServiceCollection AddWarehousesModule(this IServiceCollection services, AppConfiguration configuration)
	{
		var httpClientBuilder = services.AddHttpClient<WarehousesClient>(client =>
		{
			client.BaseAddress = new Uri(configuration.BrewUpWarehousesUri);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		});

		// Configure the chaos injection
		httpClientBuilder.AddResilienceHandler("warehouses-chaos", (pipelineBuilder, context) =>
		{
			pipelineBuilder
				.AddChaosLatency(new ChaosLatencyStrategyOptions
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					Latency = TimeSpan.FromSeconds(5)
				})
				.AddChaosFault(new ChaosFaultStrategyOptions
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					FaultGenerator = new FaultGenerator().AddException(() => new InvalidOperationException("Chaos strategy injection for Warehouses!"))
				})
				.AddChaosOutcome(new ChaosOutcomeStrategyOptions<HttpResponseMessage>
				{
					EnabledGenerator = _ => new ValueTask<bool>(true),
					InjectionRateGenerator = _ => new ValueTask<double>(0.05),
					OutcomeGenerator = new OutcomeGenerator<HttpResponseMessage>().AddResult(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError))
				});
		});

		return services;
	}

}