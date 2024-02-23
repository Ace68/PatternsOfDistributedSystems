using BrewUp.Chaos.Contracts.Sales;
using Polly.Simmy;
using Polly.Simmy.Fault;
using Polly.Simmy.Latency;
using Polly.Simmy.Outcomes;

namespace BrewUp.Chaos.Modules;

public class SalesModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		var httpClientBuilder = builder.Services.AddHttpClient<SalesClient>(client => client.BaseAddress = new Uri(builder.Configuration["Chaos:Sales:BaseUrl"]!));

		//// Configure the standard resilience handler
		//httpClientBuilder
		//	.AddStandardResilienceHandler()
		//	.Configure(options =>
		//	{
		//		// Update attempt timeout to 1 second
		//		options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);

		//		// Update circuit breaker to handle transient errors and InvalidOperationException
		//		options.CircuitBreaker.ShouldHandle = args => args.Outcome switch
		//		{
		//			var outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
		//			{ Exception: InvalidOperationException } => PredicateResult.True(),
		//			_ => PredicateResult.False()
		//		};

		//		// Update retry strategy to handle transient errors and InvalidOperationException
		//		options.Retry.ShouldHandle = args => args.Outcome switch
		//		{
		//			var outcome when HttpClientResiliencePredicates.IsTransient(outcome) => PredicateResult.True(),
		//			{ Exception: InvalidOperationException } => PredicateResult.True(),
		//			_ => PredicateResult.False()
		//		};
		//	});

		// Configure the chaos injection
		httpClientBuilder.AddResilienceHandler("sales-chaos", (builder, context) =>
		{
			// Get IChaosManager from dependency injection
			var chaosManager = context.ServiceProvider.GetRequiredService<IChaosManager>();

			builder
				.AddChaosLatency(new ChaosLatencyStrategyOptions
				{
					EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
					InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
					Latency = TimeSpan.FromSeconds(5)
				})
				.AddChaosFault(new ChaosFaultStrategyOptions
				{
					EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
					InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
					FaultGenerator = new FaultGenerator().AddException(() => new InvalidOperationException("Chaos strategy injection for Sales!"))
				})
				.AddChaosOutcome(new ChaosOutcomeStrategyOptions<HttpResponseMessage>
				{
					EnabledGenerator = args => chaosManager.IsChaosEnabledAsync(args.Context),
					InjectionRateGenerator = args => chaosManager.GetInjectionRateAsync(args.Context),
					OutcomeGenerator = new OutcomeGenerator<HttpResponseMessage>().AddResult(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError))
				});
		});

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/chaos/sales",
			async (SalesClient client, HttpContext httpContext, CancellationToken cancellationToken) =>
				await client.GetSalesOrdersAsync(cancellationToken));

		endpoints.MapPost("/chaos/sales",
			async (SalesOrderJson body, SalesClient client, HttpContext httpContext, CancellationToken cancellationToken) =>
				await client.PostSalesOrderAsync(body, cancellationToken));

		return endpoints;
	}
}