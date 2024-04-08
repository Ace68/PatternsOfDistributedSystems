using BrewUp.Sales.Infrastructures.Azure;
using BrewUp.Sales.Infrastructures.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Muflone.Eventstore;
using Muflone.Saga.Persistence.MongoDb;

namespace BrewUp.Sales.Infrastructures;

public static class InfrastructureHelper
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,
		MongoDbSettings mongoDbSettings,
		AzureServiceBusSettings azureServiceBusSettings,
		EventStoreSettings eventStoreSettings)
	{
		services.AddSalesMongoDb(mongoDbSettings);
		services.AddMongoSagaStateRepository(new MongoSagaStateRepositoryOptions(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName));

		services.AddMufloneEventStore(eventStoreSettings.ConnectionString);

		services.AddAzureForSalesModule(azureServiceBusSettings);

		return services;
	}
}