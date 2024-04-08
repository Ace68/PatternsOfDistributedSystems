using BrewUp.Warehouses.Domain.CommandHandlers;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;

namespace BrewUp.Warehouses.Infrastructures.Azure.Commands;

public sealed class UpdateAvailabilityDueToProductionOrderConsumer(IRepository repository,
		AzureServiceBusConfiguration azureServiceBusConfiguration,
		ILoggerFactory loggerFactory)
	: CommandConsumerBase<UpdateAvailabilityDueToProductionOrder>(azureServiceBusConfiguration, loggerFactory)
{
	protected override ICommandHandlerAsync<UpdateAvailabilityDueToProductionOrder> HandlerAsync { get; } = new UpdateAvailabilityDueToProductionOrderCommandHandler(repository, loggerFactory);
}