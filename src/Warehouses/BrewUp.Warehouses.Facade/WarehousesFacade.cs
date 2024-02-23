using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;
using BrewUp.Shared.ReadModel;
using BrewUp.Warehouses.SharedKernel.Commands;
using BrewUp.Warehouses.SharedKernel.Contracts;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Facade;

public sealed class WarehousesFacade(IServiceBus serviceBus,
	IQueries<ReadModel.Dtos.Availability> availabilitiesQueries) : IWarehousesFacade
{

	public async Task SetAvailabilityAsync(SetAvailabilityJson availability, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		UpdateAvailabilityDueToProductionOrder command =
			new(new BeerId(new Guid(availability.BeerId)), Guid.NewGuid(), new BeerName(availability.BeerName),
				availability.Quantity);

		await serviceBus.SendAsync(command, cancellationToken);
	}

	public async Task<PagedResult<BeerAvailabilityJson>> GetAvailabilitiesAsync(CancellationToken cancellationToken)
	{
		var availabilities = await availabilitiesQueries.GetByFilterAsync(null, 0, 100, cancellationToken);

		return availabilities.TotalRecords > 0
			? new PagedResult<BeerAvailabilityJson>(availabilities.Results.Select(r => r.ToJson()), availabilities.Page, availabilities.PageSize, availabilities.TotalRecords)
			: new PagedResult<BeerAvailabilityJson>(Enumerable.Empty<BeerAvailabilityJson>(), 0, 0, 0);
	}
}