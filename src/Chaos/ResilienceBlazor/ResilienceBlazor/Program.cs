using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using ResilienceBlazor;
using ResilienceBlazor.Modules.Sales.Extensions;
using ResilienceBlazor.Modules.Warehouses.Extensions;
using ResilienceBlazor.Shared;
using ResilienceBlazor.Shared.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Configuration
builder.Services.AddSingleton(_ => builder.Configuration.GetSection("BrewApp:AppConfiguration")
	.Get<AppConfiguration>());
builder.Services.AddApplicationService();
#endregion

builder.Services.AddFluentUIComponents();
builder.Services.AddBlazoredSessionStorage();

#region Modules
builder.Services.AddResilienceSalesModule(builder.Configuration.GetSection("BrewApp:AppConfiguration")
	.Get<AppConfiguration>()!);
builder.Services.AddSalesModule(builder.Configuration.GetSection("BrewApp:AppConfiguration")
	.Get<AppConfiguration>()!);

builder.Services.AddResilienceWarehousesModule(builder.Configuration.GetSection("BrewApp:AppConfiguration")
	.Get<AppConfiguration>()!);
builder.Services.AddWarehousesModule(builder.Configuration.GetSection("BrewApp:AppConfiguration")
	.Get<AppConfiguration>()!);
#endregion

await builder.Build().RunAsync();
