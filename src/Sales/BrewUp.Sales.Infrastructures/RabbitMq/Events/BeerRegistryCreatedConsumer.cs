using BrewUp.Sales.ReadModel.EventHandlers;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Sales.Infrastructures.RabbitMq.Events;

public sealed class BeerRegistryCreatedConsumer(IBeerService beerService, IMufloneConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
    : DomainEventsConsumerBase<BeerRegistryCreated>(connectionFactory, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<BeerRegistryCreated>> HandlersAsync { get; } =
        new List<IDomainEventHandlerAsync<BeerRegistryCreated>>
        {
            new BeerRegistryCreatedEventHandler(beerService, loggerFactory)
        };
}