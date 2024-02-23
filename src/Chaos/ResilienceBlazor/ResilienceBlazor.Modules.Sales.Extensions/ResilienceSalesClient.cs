using ResilienceBlazor.Modules.Sales.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;
using System.Net.Http.Json;

namespace ResilienceBlazor.Modules.Sales.Extensions;

public class ResilienceSalesClient(HttpClient client, AppConfiguration configuration)
{
	public async Task<PagedResult<SalesOrderJson>> GetSalesOrdersAsync(CancellationToken cancellationToken)
		=> await client.GetFromJsonAsync<PagedResult<SalesOrderJson>>("v1/sales", cancellationToken)
		   ?? new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);

	public async Task PostSalesOrderAsync(SalesOrderJson salesOrder, CancellationToken cancellationToken)
		=> await client.PostAsJsonAsync("v1/sales", salesOrder, cancellationToken);
}