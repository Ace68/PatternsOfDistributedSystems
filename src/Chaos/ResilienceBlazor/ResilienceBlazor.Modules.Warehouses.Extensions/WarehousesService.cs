using ResilienceBlazor.Modules.Warehouses.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;

namespace ResilienceBlazor.Modules.Warehouses.Extensions;

public sealed class WarehousesService(ResilienceWarehousesClient resilienceWarehousesClient, WarehousesClient warehousesClient) : IWarehousesService
{
	public Task<PagedResult<AvailabilityJson>>
		GetBeersAvailabilityWithResilienceAsync(CancellationToken cancellationToken) =>
		resilienceWarehousesClient.GetAvailabilityAsync(cancellationToken);

	public Task<PagedResult<AvailabilityJson>>
		GetBeersAvailabilityWithoutResilienceAsync(CancellationToken cancellationToken) =>
		warehousesClient.GetAvailabilityAsync(cancellationToken);
}