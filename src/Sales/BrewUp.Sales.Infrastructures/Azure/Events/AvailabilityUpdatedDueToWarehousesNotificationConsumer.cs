using BrewUp.Sales.ReadModel.EventHandlers;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Sales.Infrastructures.Azure.Events;

public sealed class AvailabilityUpdatedDueToWarehousesNotificationConsumer(IAvailabilityService availabilityService,
		AzureServiceBusConfiguration azureServiceBusConfiguration, ILoggerFactory loggerFactory)
	: DomainEventConsumerBase<AvailabilityUpdatedDueToWarehousesNotification>(azureServiceBusConfiguration, loggerFactory)
{
	protected override IEnumerable<IDomainEventHandlerAsync<AvailabilityUpdatedDueToWarehousesNotification>> HandlersAsync { get; } = new List<IDomainEventHandlerAsync<AvailabilityUpdatedDueToWarehousesNotification>>
	{
		new AvailabilityUpdatedDueToWarehousesNotificationEventHandler(loggerFactory, availabilityService)
	};
}