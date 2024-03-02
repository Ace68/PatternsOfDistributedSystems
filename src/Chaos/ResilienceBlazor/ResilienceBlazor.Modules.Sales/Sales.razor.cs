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

	protected bool WaitErrorReset;
	protected bool HideResponse = true;

	protected int GoodResponses = 0;
	protected int BadResponses = 0;

	protected IEnumerable<CustomerJson> Customers { get; set; } = Enumerable.Empty<CustomerJson>();

	protected override async Task OnInitializedAsync()
	{
		Customers = await SalesService.GetCustomersAsync(CancellationToken.None);
		await base.OnInitializedAsync();
	}

	protected async Task GetSalesOrdersWithResilienceAsync()
	{
		for (var i = 0; i < 10; i++)
		{
			await InvokeSalesOrdersWithResilienceAsync();
			await Task.Delay(100);
		}
	}

	protected async Task InvokeSalesOrdersWithResilienceAsync()
	{
		if (WaitErrorReset)
			return;

		try
		{
			var result = await SalesService.GetSalesOrdersWithResilienceAsync(CancellationToken.None);
			SalesOrders = result.Results.AsQueryable();

			ErrorMessage = "Success";
			HideResponse = true;

			GoodResponses++;
		}
		catch (Exception ex)
		{
			SalesOrders = new List<SalesOrderJson>().AsQueryable();
			ErrorMessage = ex.Message;
			WaitErrorReset = true;
			HideResponse = false;

			BadResponses++;
		}

		StateHasChanged();
	}

	protected async Task GetSalesOrdersWithoutResilienceAsync()
	{
		for (var i = 0; i < 10; i++)
		{
			await InvokeSalesOrdersWithoutResilienceAsync();
			await Task.Delay(100);
		}
	}

	protected async Task InvokeSalesOrdersWithoutResilienceAsync()
	{
		if (WaitErrorReset)
			return;

		try
		{
			var result = await SalesService.GetSalesOrdersWithoutResilienceAsync(CancellationToken.None);
			SalesOrders = result.Results.AsQueryable();

			ErrorMessage = "Success";
			HideResponse = true;

			GoodResponses++;
		}
		catch (Exception ex)
		{
			SalesOrders = new List<SalesOrderJson>().AsQueryable();
			ErrorMessage = ex.Message;
			WaitErrorReset = true;
			HideResponse = false;

			BadResponses++;
		}

		StateHasChanged();
	}

	protected async Task CreateSalesOrdersAsync()
	{
		foreach (var customer in Customers)
		{
			await CreateSalesOrderAsync(customer, DateTime.UtcNow.AddHours(2));
		}
	}

	protected async Task CreateSalesOrderAsync(CustomerJson customer, DateTime orderDate)
	{
		var salesOrderNumber =
			$"{DateTime.UtcNow.Year:0000}{DateTime.UtcNow.Month:00}{DateTime.UtcNow.Day:00}-{DateTime.UtcNow.Hour:00}{DateTime.UtcNow.Minute:00}";
		var salesOrder = new SalesOrderJson(Guid.NewGuid().ToString(),
			salesOrderNumber,
			customer.CustomerId, customer.CustomerName,
			orderDate,
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

	protected void ResetError()
	{
		WaitErrorReset = false;
		HideResponse = true;
		ErrorMessage = string.Empty;

		GoodResponses = 0;
		BadResponses = 0;
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