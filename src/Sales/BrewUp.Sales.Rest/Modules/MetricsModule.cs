using OpenTelemetry.Metrics;

namespace BrewUp.Sales.Rest.Modules;

public class MetricsModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddOpenTelemetry()
			.WithMetrics(meterProviderBuilder =>
			{
				meterProviderBuilder.AddPrometheusExporter();

				meterProviderBuilder.AddMeter("Microsoft.AspNetCore.Hosting",
					"Microsoft.AspNetCore.Server.Kestrel");

				meterProviderBuilder.AddView("http.server.request.duration",
					new ExplicitBucketHistogramConfiguration
					{
						Boundaries = new[]
						{
							0, 0.005, 0.01, 0.025, 0.05,
							0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10
						}
					});

				//meterProviderBuilder.AddView("http.server.active_requests", 
				//	new )
			});

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPrometheusScrapingEndpoint();

		return endpoints;
	}
}