using Microsoft.AspNetCore.Components;
using ResilienceBlazor.Modules.Warehouses.Extensions;
using ResilienceBlazor.Modules.Warehouses.Extensions.Dtos;

namespace ResilienceBlazor.Modules.Warehouses;

public class WarehousesBase : ComponentBase, IDisposable
{
	[Inject] private IWarehousesService WarehousesService { get; set; } = default!;

	protected IQueryable<AvailabilityJson> BeersAvailabilities { get; set; } = default!;
	protected string ErrorMessage { get; set; } = string.Empty;

	protected bool WaitErrorReset;
	protected bool HideResponse = true;

	protected int GoodResponses = 0;
	protected int BadResponses = 0;

	protected async Task GetBeersAvailabilitiesWithResilienceAsync()
	{
		for (var i = 0; i < 1; i++)
		{
			await InvokeBeersAvailabilitiesWithResilienceAsync();
			await Task.Delay(100);
		}
	}

	protected async Task InvokeBeersAvailabilitiesWithResilienceAsync()
	{
		if (WaitErrorReset)
			return;

		try
		{
			var result = await WarehousesService.GetBeersAvailabilityWithResilienceAsync(CancellationToken.None);
			BeersAvailabilities = result.Results.AsQueryable();

			ErrorMessage = "Success";
			HideResponse = true;

			GoodResponses++;
		}
		catch (Exception ex)
		{
			BeersAvailabilities = new List<AvailabilityJson>().AsQueryable();
			ErrorMessage = ex.Message;
			WaitErrorReset = true;
			HideResponse = false;

			BadResponses++;
		}

		StateHasChanged();
	}

	protected async Task GetBeersWithoutResilienceAsync()
	{
		for (var i = 0; i < 10; i++)
		{
			await InvokeBeersAvailabilitiesWithoutResilienceAsync();
			await Task.Delay(100);
		}
	}

	protected async Task InvokeBeersAvailabilitiesWithoutResilienceAsync()
	{
		if (WaitErrorReset)
			return;

		try
		{
			var result = await WarehousesService.GetBeersAvailabilityWithoutResilienceAsync(CancellationToken.None);
			BeersAvailabilities = result.Results.AsQueryable();

			ErrorMessage = "Success";
			HideResponse = true;

			GoodResponses++;
		}
		catch (Exception ex)
		{
			BeersAvailabilities = new List<AvailabilityJson>().AsQueryable();
			ErrorMessage = ex.Message;
			WaitErrorReset = true;
			HideResponse = false;

			BadResponses++;
		}

		StateHasChanged();
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

	~WarehousesBase()
	{
		Dispose(false);
	}
	#endregion
}