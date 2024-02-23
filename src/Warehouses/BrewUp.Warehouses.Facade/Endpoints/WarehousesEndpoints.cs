using BrewUp.Warehouses.Facade.Validators;
using BrewUp.Warehouses.SharedKernel.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrewUp.Warehouses.Facade.Endpoints;

public static class WarehousesEndpoints
{
	public static IEndpointRouteBuilder MapWarehousesEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/v1/wareHouses/")
			.WithTags("Warehouses");

		group.MapPost("/availabilities", HandleSetAvailabilities)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("SetAvailabilities");

		group.MapGet("/availabilities", HandleGetAvailabilities)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetAvailabilities");

		return endpoints;
	}

	public static async Task<IResult> HandleSetAvailabilities(
		IWarehousesFacade warehousesFacade,
		IValidator<SetAvailabilityJson> validator,
		ValidationHandler validationHandler,
		SetAvailabilityJson body,
		CancellationToken cancellationToken)
	{
		await warehousesFacade.SetAvailabilityAsync(body, cancellationToken);

		return Results.Ok();
	}

	public static async Task<IResult> HandleGetAvailabilities(
		IWarehousesFacade warehousesFacade,
		CancellationToken cancellationToken)
	{
		var availabilities = await warehousesFacade.GetAvailabilitiesAsync(cancellationToken);

		return Results.Ok(availabilities);
	}
}