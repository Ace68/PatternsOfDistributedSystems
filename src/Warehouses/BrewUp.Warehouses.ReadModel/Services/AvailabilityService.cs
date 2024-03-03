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
			var beerAvailability = await availabilitiesQueries.GetByFilterAsync(a => a.BeerId.Equals(beerId.Value.ToString()), 0, 0,
				cancellationToken);
			if (beerAvailability.TotalRecords.Equals(0))
			{
				var availability = Dtos.Availability.Create(beerId, beerName, quantity);
				await Persister.InsertAsync(availability, cancellationToken);	
			}
			else
			{
				var availability = beerAvailability.Results.First();
				availability.UpdateAvailability(quantity);
				await Persister.UpdateAsync(availability, cancellationToken);
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating availability");
			throw;
		}
	}
}