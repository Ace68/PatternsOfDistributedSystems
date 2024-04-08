using BrewUp.Warehouses.Infrastructures.Azure.Commands;
using BrewUp.Warehouses.Infrastructures.Azure.Events;
using BrewUp.Warehouses.ReadModel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.Azure;
using Muflone.Transport.Azure.Abstracts;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Warehouses.Infrastructures.Azure;

public static class AzureServiceBusHelper
{
	public static IServiceCollection AddAzureForWarehousesModule(this IServiceCollection services,
		AzureServiceBusSettings azureServiceBusSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetRequiredService<IRepository>();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		AzureServiceBusConfiguration azureServiceBusConfiguration = new(azureServiceBusSettings.ConnectionString,
			string.Empty, azureServiceBusSettings.ClientId, 1);

		services.AddMufloneTransportAzure(azureServiceBusConfiguration);

		serviceProvider = services.BuildServiceProvider();
		var consumers = serviceProvider.GetRequiredService<IEnumerable<IConsumer>>();
		consumers = consumers.Concat(new List<IConsumer>
		{
			new UpdateAvailabilityDueToProductionOrderConsumer(repository,
				azureServiceBusConfiguration,
				loggerFactory),
			new AvailabilityUpdatedDueToProductionOrderConsumer(serviceProvider.GetRequiredService<IAvailabilityService>(),
				serviceProvider.GetRequiredService<IEventBus>(),
				azureServiceBusConfiguration, loggerFactory)
		});
		services.AddMufloneAzureConsumers(consumers);

		return services;
	}
}