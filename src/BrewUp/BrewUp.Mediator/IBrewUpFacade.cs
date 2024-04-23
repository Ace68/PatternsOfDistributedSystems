using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;

namespace BrewUp.Mediator;

public interface IBrewUpFacade
{
	Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken);
	Task<PagedResult<SalesOrderJson>> GetOrdersAsync(CancellationToken cancellationToken);
}