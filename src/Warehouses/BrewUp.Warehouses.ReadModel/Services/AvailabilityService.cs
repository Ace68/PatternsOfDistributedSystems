using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.ReadModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BrewUp.Warehouses.ReadModel.Services;

public sealed class AvailabilityService : ServiceBase, IAvailabilityService
{
	public AvailabilityService(ILoggerFactory loggerFactory, [FromKeyedServices("warehouses")] IPersister persister) : base(loggerFactory, persister)
	{
	}

	public async Task UpdateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			var beerAvailability = await Persister.GetByIdAsync<Dtos.Availability>(beerId.Value.ToString(), cancellationToken);
			beerAvailability.UpdateQuantity(quantity);
			await Persister.UpdateAsync(beerAvailability, cancellationToken);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating availability");
			throw;
		}
	}
}