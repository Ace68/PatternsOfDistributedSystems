using BrewUp.Sales.Domain.CommandHandlers;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Sales.Infrastructures.Azure.Commands;

public class UpdateAvailabilityDueToWarehousesNotificationConsumer(IRepository repository,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory)
	: CommandConsumerBase<UpdateAvailabilityDueToWarehousesNotification>(azureServiceBusConfiguration, loggerFactory)
{
	protected override ICommandHandlerAsync<UpdateAvailabilityDueToWarehousesNotification> HandlerAsync { get; } = new UpdateAvailabilityDueToWarehousesNotificationCommandHandler(repository, loggerFactory);
}