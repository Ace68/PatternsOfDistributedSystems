using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using System.Reflection;

namespace ResilienceBlazor;

public class AppBase : ComponentBase, IDisposable
{
	[Inject] private LazyAssemblyLoader AssemblyLoader { get; set; } = default!;
	[Inject] private ILogger<App> Logger { get; set; } = default!;

	protected readonly List<Assembly> LazyLoadedAssemblies = new();

	protected async Task OnNavigateAsync(NavigationContext args)
	{
		try
		{
			switch (args.Path)
			{
				case "sales":
					{
						var assemblies = await AssemblyLoader.LoadAssembliesAsync(new List<string>
						{
							"ResilienceBlazor.Modules.Sales.wasm"
						});
						LazyLoadedAssemblies.AddRange(assemblies);
						break;
					}

				case "warehouses":
					{
						var assemblies = await AssemblyLoader.LoadAssembliesAsync(new List<string>
					{
						"ResilienceBlazor.Modules.Warehouses.wasm"
					});
						LazyLoadedAssemblies.AddRange(assemblies);
						break;
					}
			}
		}
		catch (Exception ex)
		{
			Logger.LogError($"Error Loading spares page: {ex}");
		}
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

	~AppBase()
	{
		Dispose(false);
	}
	#endregion
}