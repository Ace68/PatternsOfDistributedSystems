using BrewUp.Sales.Infrastructures;
using BrewUp.Sales.Infrastructures.Azure;
using BrewUp.Sales.Infrastructures.MongoDb;

namespace BrewUp.Sales.Rest.Modules;

public class InfrastructureModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 90;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		var mongoDbSettings = builder.Configuration.GetSection("BrewUp:MongoDbSettings")
			.Get<MongoDbSettings>()!;

		var rabbitMqSettings = builder.Configuration.GetSection("BrewUp:AzureServiceBus")
			.Get<AzureServiceBusSettings>()!;

		var eventStoreSettings = builder.Configuration.GetSection("BrewUp:EventStore")
		.Get<EventStoreSettings>()!;

		builder.Services.AddInfrastructure(mongoDbSettings, rabbitMqSettings, eventStoreSettings);

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}