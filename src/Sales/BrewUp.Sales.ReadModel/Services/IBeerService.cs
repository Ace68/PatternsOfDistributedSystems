using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;

namespace BrewUp.Sales.ReadModel.Services;

public interface IBeerService
{
    Task CreateBeerRegistryAsync(BeerId beerId, BeerName beerName, CancellationToken cancellationToken);
    Task<PagedResult<BeerJson>> GetBeersAsync(int page, int pageSize, CancellationToken cancellationToken);
}