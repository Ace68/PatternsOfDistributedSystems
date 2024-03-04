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
	protected IEnumerable<BeerJson> Beers { get; set; } = Enumerable.Empty<BeerJson>();

	protected override async Task OnInitializedAsync()
	{
		await GetCustomersAsync();
		await GetBeersAsync();

		await base.OnInitializedAsync();
	}

	private async Task GetCustomersAsync()
	{
		Customers = (await SalesService.GetCustomersAsync(CancellationToken.None)).Results;
	}

	private async Task GetBeersAsync()
	{
		Beers = (await SalesService.GetBeersAsync(CancellationToken.None)).Results;
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
		List<SalesOrderRowJson> rows = Beers.Select(beer => new SalesOrderRowJson
		{
			BeerId = new Guid(beer.BeerId),
			BeerName = beer.BeerName,
			Quantity = new Quantity(10, "Lt"),
			Price = new Price(10, "€")
		}).ToList();

		var salesOrderNumber =
			$"{DateTime.UtcNow.Year:0000}{DateTime.UtcNow.Month:00}{DateTime.UtcNow.Day:00}-{DateTime.UtcNow.Hour:00}{DateTime.UtcNow.Minute:00}";
		var salesOrder = new SalesOrderJson(Guid.NewGuid().ToString(),
			salesOrderNumber,
			customer.CustomerId, customer.CustomerName,
			orderDate,
			rows
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