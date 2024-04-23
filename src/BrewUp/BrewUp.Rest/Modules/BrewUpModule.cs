using BrewUp.Mediator;
using BrewUp.Mediator.Endpoints;

namespace BrewUp.Rest.Modules;

public class BrewUpModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder) => builder.Services.AddMediator();

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints.MapMediatorEndpoints();
}