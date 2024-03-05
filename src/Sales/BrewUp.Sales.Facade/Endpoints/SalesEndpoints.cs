using BrewUp.Sales.Facade.Validators;
using BrewUp.Shared.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrewUp.Sales.Facade.Endpoints;

public static class SalesEndpoints
{
	public static IEndpointRouteBuilder MapSalesEndpoints(this IEndpointRouteBuilder endpoints, string rateLimitPolicy)
	{
		var group = endpoints.MapGroup("/v1/sales/")
			.RequireRateLimiting(rateLimitPolicy)
			.WithTags("Sales");

		group.MapPost("/", HandleCreateOrder)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status201Created)
			.WithName("CreateSalesOrder");

		group.MapGet("/", HandleGetOrders)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetSalesOrders");
		
		group.MapGet("/beers", HandleGetBeers)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetBeersRegistry");

		return endpoints;
	}

	private static async Task<IResult> HandleCreateOrder(
		ISalesFacade salesUpFacade,
		IValidator<SalesOrderJson> validator,
		ValidationHandler validationHandler,
		SalesOrderJson body,
		CancellationToken cancellationToken)
	{
		await validationHandler.ValidateAsync(validator, body);
		if (!validationHandler.IsValid)
			return Results.BadRequest(validationHandler.Errors);

		var salesOrderId = await salesUpFacade.CreateOrderAsync(body, cancellationToken);

		return Results.Created(new Uri($"/v1/sales/{salesOrderId}", UriKind.Relative), salesOrderId);
	}

	private static async Task<IResult> HandleGetOrders(
		ISalesFacade salesUpFacade,
		CancellationToken cancellationToken)
	{
		var orders = await salesUpFacade.GetOrdersAsync(cancellationToken);

		return Results.Ok(orders);
	}
	
	private static async Task<IResult> HandleGetBeers(
		ISalesFacade salesUpFacade,
		CancellationToken cancellationToken)
	{
		var beers = await salesUpFacade.GetBeersAsync(cancellationToken);

		return Results.Ok(beers);
	}
}