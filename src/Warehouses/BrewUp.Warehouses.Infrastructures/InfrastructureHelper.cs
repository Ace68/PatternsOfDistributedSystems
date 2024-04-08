using BrewUp.Warehouses.Infrastructures.Azure;
using BrewUp.Warehouses.Infrastructures.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Eventstore;
using Muflone.Saga.Persistence.MongoDb;

namespace BrewUp.Warehouses.Infrastructures;

public static class InfrastructureHelper
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,
		MongoDbSettings mongoDbSettings,
		AzureServiceBusSettings azureServiceBusSettings,
		EventStoreSettings eventStoreSettings)
	{
		services.AddWarehousesMongoDb(mongoDbSettings);
		services.AddMongoSagaStateRepository(new MongoSagaStateRepositoryOptions(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName));

		services.AddMufloneEventStore(eventStoreSettings.ConnectionString);

		services.AddAzureForWarehousesModule(azureServiceBusSettings);

		return services;
	}
}