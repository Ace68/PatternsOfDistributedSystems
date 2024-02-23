using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BrewUp.Chaos.Modules;

public class ChaosModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.TryAddSingleton<IChaosManager, ChaosManager>();
		builder.Services.AddHttpContextAccessor();

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}