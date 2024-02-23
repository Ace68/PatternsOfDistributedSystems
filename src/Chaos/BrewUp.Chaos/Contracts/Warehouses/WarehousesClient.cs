namespace BrewUp.Chaos.Contracts.Warehouses;

public class WarehousesClient(HttpClient client)
{
	public async Task<PagedResult<BeerAvailabilityJson>> GetAvailabilitiesAsync(CancellationToken cancellationToken)
		=> await client.GetFromJsonAsync<PagedResult<BeerAvailabilityJson>>("v1/warehouses/availabilities", cancellationToken)
		   ?? new PagedResult<BeerAvailabilityJson>(Enumerable.Empty<BeerAvailabilityJson>(), 0, 0, 0);

	public async Task PostAvailabilitiesAsync(SetAvailabilityJson salesOrder, CancellationToken cancellationToken)
		=> await client.PostAsJsonAsync("v1/warehouses/availabilities", salesOrder, cancellationToken);
}