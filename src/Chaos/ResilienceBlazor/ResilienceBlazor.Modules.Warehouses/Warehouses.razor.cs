using Microsoft.AspNetCore.Components;

namespace ResilienceBlazor.Modules.Warehouses;

public class WarehousesBase : ComponentBase, IDisposable
{

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