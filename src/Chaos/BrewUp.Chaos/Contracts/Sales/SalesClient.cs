namespace BrewUp.Chaos.Contracts.Sales;

public class SalesClient(HttpClient client)
{
	public async Task<PagedResult<SalesOrderJson>> GetSalesOrdersAsync(CancellationToken cancellationToken)
		=> await client.GetFromJsonAsync<PagedResult<SalesOrderJson>>("v1/sales", cancellationToken)
		   ?? new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);

	public async Task PostSalesOrderAsync(SalesOrderJson salesOrder, CancellationToken cancellationToken)
		=> await client.PostAsJsonAsync("v1/sales", salesOrder, cancellationToken);
}