using Microsoft.AspNetCore.Components;
using ResilienceBlazor.Modules.Sales.Extensions;
using ResilienceBlazor.Modules.Sales.Extensions.Dtos;
using ResilienceBlazor.Shared.CustomTypes;

namespace ResilienceBlazor.Modules.Sales;

public class SalesBase : ComponentBase, IDisposable
{
	[Inject] private ISalesService SalesService { get; set; } = default!;

	protected IQueryable<SalesOrderJson> SalesOrders { get; set; } = default!;
	protected string ErrorMessage { get; set; } = string.Empty;

	private bool _waitErrorReset;

	protected async Task GetSalesOrdersWithResilienceAsync()
	{
		if (_waitErrorReset)
			return;

		try
		{
			var result = await SalesService.GetSalesOrdersWithResilienceAsync(CancellationToken.None);
			SalesOrders = result.Results.AsQueryable();

			ErrorMessage = "Success";
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.Message;
			_waitErrorReset = true;
		}
	}

	protected async Task GetSalesOrdersWithoutResilienceAsync()
	{
		if (_waitErrorReset)
			return;

		try
		{
			var result = await SalesService.GetSalesOrdersWithoutResilienceAsync(CancellationToken.None);
			SalesOrders = result.Results.AsQueryable();

			ErrorMessage = "Success";
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.Message;
			_waitErrorReset = true;
		}
	}

	protected async Task CreateSalesOrderAsync()
	{
		var salesOrder = new SalesOrderJson(Guid.NewGuid().ToString(),
			$"{DateTime.UtcNow.Year:0000}{DateTime.UtcNow.Month:00}{DateTime.UtcNow.Day:00}",
			Guid.NewGuid(), "Muflone",
			DateTime.UtcNow,
			new List<SalesOrderRowJson>
			{
				new()
				{
					BeerId = new Guid("c94bc922-9a2e-4264-8800-a40e1d7d534b"),
					BeerName = "Muflone IPA",
					Quantity = new Quantity(10, "Lt"),
					Price = new Price(10, "€")
				}
			}
			);
		await SalesService.CreateSalesOrderAsync(salesOrder, CancellationToken.None);
	}

	#region Dispose
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
		}
	}
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~SalesBase()
	{
		Dispose(false);
	}
	#endregion

}