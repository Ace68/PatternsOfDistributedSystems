﻿using ResilienceBlazor.Modules.Sales.Extensions.Dtos;
using ResilienceBlazor.Shared.Configuration;

namespace ResilienceBlazor.Modules.Sales.Extensions;

public interface ISalesService
{
	Task<PagedResult<SalesOrderJson>> GetSalesOrdersWithResilienceAsync(CancellationToken cancellationToken);
	Task<PagedResult<SalesOrderJson>> GetSalesOrdersWithoutResilienceAsync(CancellationToken cancellationToken);

	Task<PagedResult<CustomerJson>> GetCustomersAsync(CancellationToken cancellationToken);

	Task<PagedResult<BeerJson>> GetBeersAsync(CancellationToken cancellationToken);

	Task CreateSalesOrderAsync(SalesOrderJson salesOrder, CancellationToken cancellationToken);
}