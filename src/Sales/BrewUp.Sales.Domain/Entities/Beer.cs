using BrewUp.Sales.SharedKernel.Events;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Core;

namespace BrewUp.Sales.Domain.Entities;

public class Beer : AggregateRoot
{
    private BeerName _beerName;
    
    protected Beer()
    {}
    
    internal static Beer CreateBeer(BeerId beerId, BeerName beerName, Guid correlationId)
    {
        return new Beer(beerId, beerName, correlationId);
    }
    
    private Beer(BeerId beerId, BeerName beerName, Guid correlationId)
    {
        RaiseEvent(new BeerRegistryCreated(beerId, beerName, correlationId));
    }
    
    private void Apply(BeerRegistryCreated @event)
    {
        Id = @event.BeerId;
        
        _beerName = @event.BeerName;
    }
}