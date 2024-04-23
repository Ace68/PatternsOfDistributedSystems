using BrewUp.Mediator.Validators;
using BrewUp.Shared.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrewUp.Mediator.Endpoints;

public static class MediatorEndpoints
{
	public static IEndpointRouteBuilder MapMediatorEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/v1/brewup/")
			.WithTags("BrewUp");

		group.MapPost("/", HandleCreateOrder)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status201Created)
			.WithName("OrderBeers");

		return endpoints;
	}

	public static async Task<IResult> HandleCreateOrder(
		IBrewUpFacade brewUpFacade,
		IValidator<SalesOrderJson> validator,
		ValidationHandler validationHandler,
		SalesOrderJson body,
		CancellationToken cancellationToken)
	{

		await validationHandler.ValidateAsync(validator, body);
		if (!validationHandler.IsValid)
			return Results.BadRequest(validationHandler.Errors);

		var orderId = await brewUpFacade.CreateOrderAsync(body, cancellationToken);

		return Results.Created($"/v1/brewup/orders/{orderId}", orderId);
	}
}