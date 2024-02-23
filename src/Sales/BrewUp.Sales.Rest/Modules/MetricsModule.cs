using BrewUp.Sales.SharedKernel.Metrics;
using OpenTelemetry.Metrics;

namespace BrewUp.Sales.Rest.Modules;

public class MetricsModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton<SalesMetrics>();

		builder.Services.AddOpenTelemetry()
			.WithMetrics(provider =>
			{
				provider.AddPrometheusExporter();

				provider.AddMeter("Microsoft.AspNetCore.Hosting",
					"Microsoft.AspNetCore.Server.Kestrel");
				provider.AddView("http.server.request.duration",
					new ExplicitBucketHistogramConfiguration
					{
						Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05,
							0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
					});
			});

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPrometheusScrapingEndpoint();

		var group = endpoints.MapGroup("/metrics/")
			.WithTags("Metrics");

		group.MapGet("/", () => "Hello OpenTelemetry! ticks:"
					 + DateTime.Now.Ticks.ToString()[^3..]);

		return endpoints;
	}
}