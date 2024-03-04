using BrewUp.Sales.ReadModel.Services;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;

namespace BrewUp.Sales.ReadModel.EventHandlers;

public sealed class BeerRegistryCreatedEventHandler(IBeerService beerService, ILoggerFactory loggerFactory) 
    : DomainEventHandlerBase<BeerRegistryCreated>(loggerFactory)
{
    public override async Task HandleAsync(BeerRegistryCreated @event, CancellationToken cancellationToken = new ())
    {
        cancellationToken.ThrowIfCancellationRequested();
        await beerService.CreateBeerRegistryAsync(@event.BeerId, @event.BeerName, cancellationToken);
    }
}