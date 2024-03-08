using ResilienceBlazor.Modules.Warehouses.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;

namespace ResilienceBlazor.Modules.Warehouses.Extensions;

public interface IWarehousesService
{
	Task<PagedResult<AvailabilityJson>> GetBeersAvailabilityWithResilienceAsync(CancellationToken cancellationToken);
	Task<PagedResult<AvailabilityJson>> GetBeersAvailabilityWithoutResilienceAsync(CancellationToken cancellationToken);
}