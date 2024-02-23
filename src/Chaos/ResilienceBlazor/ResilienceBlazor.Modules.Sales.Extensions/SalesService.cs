using ResilienceBlazor.Modules.Sales.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;

namespace ResilienceBlazor.Modules.Sales.Extensions;

public sealed class SalesService(SalesClient salesClient, ResilienceSalesClient resilienceSalesClient) : ISalesService
{
	public async Task<PagedResult<SalesOrderJson>>
		GetSalesOrdersWithResilienceAsync(CancellationToken cancellationToken) =>
		await resilienceSalesClient.GetSalesOrdersAsync(cancellationToken);

	public async Task<PagedResult<SalesOrderJson>> GetSalesOrdersWithoutResilienceAsync(CancellationToken cancellationToken) =>
		await salesClient.GetSalesOrdersAsync(cancellationToken);

	public async Task CreateSalesOrderAsync(SalesOrderJson salesOrder, CancellationToken cancellationToken) =>
			await resilienceSalesClient.PostSalesOrderAsync(salesOrder, cancellationToken);
}