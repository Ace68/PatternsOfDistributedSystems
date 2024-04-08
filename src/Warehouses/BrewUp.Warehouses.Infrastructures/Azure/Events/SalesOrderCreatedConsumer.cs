using Azure;
using BrewUp.Warehouses.ReadModel.EventHandlers;
using BrewUp.Warehouses.ReadModel.Services;
using BrewUp.Warehouses.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Warehouses.Infrastructures.Azure.Events;

public sealed class AvailabilityUpdatedDueToProductionOrderConsumer(IAvailabilityService availabilityService,
		IEventBus eventBus,
		AzureServiceBusConfiguration azureServiceBusConfiguration, ILoggerFactory loggerFactory)
	: DomainEventConsumerBase<AvailabilityUpdatedDueToProductionOrder>(azureServiceBusConfiguration, loggerFactory)
{
	protected override IEnumerable<IDomainEventHandlerAsync<AvailabilityUpdatedDueToProductionOrder>> HandlersAsync { get; } = new List<DomainEventHandlerAsync<AvailabilityUpdatedDueToProductionOrder>>
	{
		new AvailabilityUpdatedDueToProductionOrderEventHandler(loggerFactory, availabilityService),
		new AvailabilityUpdatedDueToProductionOrderForIntegrationEventHandler(loggerFactory, eventBus)
	};
}