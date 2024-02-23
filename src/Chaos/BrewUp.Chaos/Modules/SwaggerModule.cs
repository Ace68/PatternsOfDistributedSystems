using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BrewUp.Chaos.Modules;

public sealed class SwaggerModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 0;

	public IServiceCollection RegisterModule(WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(SetSwaggerGenOptions);

		return builder.Services;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

	private void SetSwaggerGenOptions(SwaggerGenOptions options)
	{
		options.SwaggerDoc("v1", new OpenApiInfo
		{
			Description = "BrewUp ChaosEngineering",
			Title = "BrewUp ChaosEngineering API",
			Version = "v1",
			Contact = new OpenApiContact
			{
				Name = "BrewUp ChaosEngineering"
			}
		});
		options.OperationFilter<AddHeaderParameter>();
	}
}

internal class AddHeaderParameter : IOperationFilter
{
	/// <summary>
	/// Add un custom required header in Swagger
	/// </summary>
	/// <param name="operation"></param>
	/// <param name="context"></param>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		operation.Parameters ??= new List<OpenApiParameter>();

		operation.Parameters.Add(new OpenApiParameter
		{
			Name = "user",
			In = ParameterLocation.Header,
			Description = "insert monkey to active chaos",
			Required = false
		});
	}
}