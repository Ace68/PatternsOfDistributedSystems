using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrewUp.Sales.Facade.Endpoints;

public static class SalesEndpoints
{
	public static IEndpointRouteBuilder MapSalesEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/v1/sales/")
			.WithTags("Sales");

		group.MapGet("/", HandleGetOrders)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetSalesOrders");

		return endpoints;
	}

	private static async Task<IResult> HandleGetOrders(
		ISalesFacade salesUpFacade,
		CancellationToken cancellationToken)
	{
		var orders = await salesUpFacade.GetOrdersAsync(cancellationToken);

		return Results.Ok(orders);
	}
}