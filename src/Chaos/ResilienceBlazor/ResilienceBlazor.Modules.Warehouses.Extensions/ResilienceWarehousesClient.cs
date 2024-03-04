using ResilienceBlazor.Modules.Warehouses.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;
using System.Net.Http.Json;

namespace ResilienceBlazor.Modules.Warehouses.Extensions;

public class ResilienceWarehousesClient(HttpClient client)
{
	public async Task<PagedResult<AvailabilityJson>> GetAvailabilityAsync(CancellationToken cancellationToken)
		=> await client.GetFromJsonAsync<PagedResult<AvailabilityJson>>("v1/warehouses/availabilities", cancellationToken)
		   ?? new PagedResult<AvailabilityJson>(Enumerable.Empty<AvailabilityJson>(), 0, 0, 0);
}