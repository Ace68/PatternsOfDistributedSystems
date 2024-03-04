using BrewUp.Sales.ReadModel.Dtos;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;
using BrewUp.Shared.ReadModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BrewUp.Sales.ReadModel.Services;

public sealed class BeerService(ILoggerFactory loggerFactory, [FromKeyedServices("sales")] IPersister persister,
        IQueries<Beer> queries) : ServiceBase(loggerFactory, persister), IBeerService
{

    public async Task CreateBeerRegistryAsync(BeerId beerId, BeerName beerName, CancellationToken cancellationToken)
    {
        var beer = Beer.CreateBeer(beerId, beerName);
        await Persister.InsertAsync(beer, cancellationToken);
    }

    public async Task<PagedResult<BeerJson>> GetBeersAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var beers = await queries.GetByFilterAsync(null, page, pageSize, cancellationToken);

            return beers.TotalRecords > 0
                ? new PagedResult<BeerJson>(beers.Results.Select(r => r.ToJson()), beers.Page, beers.PageSize, beers.TotalRecords)
                : new PagedResult<BeerJson>(Enumerable.Empty<BeerJson>(), 0, 0, 0);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error reading Beer Registry");
            throw;
        }
    }
}