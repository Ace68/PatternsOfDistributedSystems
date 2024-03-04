using BrewUp.Sales.Domain.CommandHandlers;
using BrewUp.Sales.SharedKernel.Commands;
using BrewUp.Sales.SharedKernel.Events;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;
using Muflone.SpecificationTests;

namespace BrewUp.Sales.Domain.Tests.Entities;

public sealed class CreateBeerRegistrySuccessfully : CommandSpecification<CreateBeerRegistry>
{
    private readonly BeerId _beerId = new BeerId(Guid.NewGuid());
    private readonly BeerName _beerName = new BeerName("Muflone IPA");
    
    private readonly Guid _correlationId = Guid.NewGuid();
    
    protected override IEnumerable<DomainEvent> Given()
    {
        yield break;
    }

    protected override CreateBeerRegistry When()
    {
        return new CreateBeerRegistry(_beerId, _beerName, _correlationId);
    }

    protected override ICommandHandlerAsync<CreateBeerRegistry> OnHandler()
    {
        return new CreateBeerRegistryCommandHandler(Repository, new NullLoggerFactory());
    }

    protected override IEnumerable<DomainEvent> Expect()
    {
        yield return new BeerRegistryCreated(_beerId, _beerName, _correlationId);
    }
}