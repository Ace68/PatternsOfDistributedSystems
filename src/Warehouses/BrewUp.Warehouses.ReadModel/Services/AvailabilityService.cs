using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.ReadModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BrewUp.Warehouses.ReadModel.Services;

public sealed class AvailabilityService(ILoggerFactory loggerFactory, [FromKeyedServices("warehouses")] IPersister persister,
	IQueries<ReadModel.Dtos.Availability> availabilitiesQueries) : ServiceBase(loggerFactory, persister), IAvailabilityService
{
	public async Task UpdateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		try
		{
			var beerAvailability = await availabilitiesQueries.GetByIdAsync(beerId.Value.ToString(), cancellationToken);
			if (beerAvailability != null || string.IsNullOrEmpty(beerAvailability.BeerName))
			{
				var availability = Dtos.Availability.Create(beerId, beerName, quantity);
				await Persister.InsertAsync(availability, cancellationToken);	
			}
			else
			{
				beerAvailability.UpdateAvailability(quantity);
				await Persister.UpdateAsync(beerAvailability, cancellationToken);
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating availability");
			throw;
		}
	}
}