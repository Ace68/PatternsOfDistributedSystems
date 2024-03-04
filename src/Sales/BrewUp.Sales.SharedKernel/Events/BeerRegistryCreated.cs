using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Messages.Events;

namespace BrewUp.Sales.SharedKernel.Events;

public sealed class BeerRegistryCreated(BeerId aggregateId, BeerName beerName, Guid correlationId) : DomainEvent(aggregateId, correlationId)
{
    public readonly BeerId BeerId = aggregateId;
    public readonly BeerName BeerName = beerName;
}