using BrewUp.Sales.Domain;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Sales.SharedKernel.CustomTypes;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;
using BrewUp.Warehouses.Facade;

namespace BrewUp.Sales.Facade;

public sealed class SalesFacade(ISalesDomainService salesDomainService,
	IWarehousesFacade warehousesFacade,
	ISalesQueryService salesQueryService) : ISalesFacade
{
	public async Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken)
	{
		if (body.SalesOrderId.Equals(string.Empty))
			body = body with { SalesOrderId = Guid.NewGuid().ToString() };

		// Check Availability
		var beerRows = new List<SalesOrderRowJson>();
		foreach (var row in body.Rows)
		{
			var beerAvailabilityResult = await warehousesFacade.GetAvailabilityAsync(row.BeerId, cancellationToken);
			
			beerRows.Add(new SalesOrderRowJson
			{
				BeerId = row.BeerId,
				BeerName = row.BeerName,
				Quantity = beerAvailabilityResult.Results.Any()
					? row.Quantity with {Value = beerAvailabilityResult.Results.First().Availability.Available}
					: row.Quantity with {Value = 0}
			});
		}

		// Create SalesOrder
		await salesDomainService.CreateSalesOrderAsync(new SalesOrderId(new Guid(body.SalesOrderId)),
			new SalesOrderNumber(body.SalesOrderNumber), new OrderDate(body.OrderDate),
			new CustomerId(body.CustomerId), new CustomerName(body.CustomerName),
			beerRows, cancellationToken);

		return body.SalesOrderId;
	}

	public async Task<PagedResult<SalesOrderJson>> GetOrdersAsync(CancellationToken cancellationToken)
	{
		return await salesQueryService.GetSalesOrdersAsync(0, 30, cancellationToken);
	}
}