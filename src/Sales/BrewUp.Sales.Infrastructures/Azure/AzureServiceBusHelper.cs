using BrewUp.Sales.Infrastructures.Azure.Commands;
using BrewUp.Sales.Infrastructures.Azure.Events;
using BrewUp.Sales.ReadModel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.Azure;
using Muflone.Transport.Azure.Abstracts;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Sales.Infrastructures.Azure;

public static class AzureServiceBusHelper
{
	public static IServiceCollection AddAzureForSalesModule(this IServiceCollection services,
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
			new CreateSalesOrderConsumer(repository,
				azureServiceBusConfiguration,
				loggerFactory),
			new SalesOrderCreatedConsumer(serviceProvider.GetRequiredService<ISalesOrderService>(),
				serviceProvider.GetRequiredService<IEventBus>(),
				azureServiceBusConfiguration, loggerFactory),

			new AvailabilityUpdatedForNotificationConsumer(serviceProvider.GetRequiredService<IServiceBus>(),
				azureServiceBusConfiguration,
				loggerFactory),

			new UpdateAvailabilityDueToWarehousesNotificationConsumer(repository, azureServiceBusConfiguration,
				loggerFactory),
			new AvailabilityUpdatedDueToWarehousesNotificationConsumer(serviceProvider.GetRequiredService<IAvailabilityService>(),
				azureServiceBusConfiguration, loggerFactory)
		});

		services.AddMufloneAzureConsumers(consumers);

		return services;
	}
}