using System.Diagnostics.Metrics;

namespace BrewUp.Sales.SharedKernel.Metrics;

public class SalesMetrics
{
	private readonly Counter<decimal> _beerSoldCounter;

	public SalesMetrics(IMeterFactory meterFactory)
	{
		var meter = meterFactory.Create("BrewUp.Sales");
		_beerSoldCounter = meter.CreateCounter<decimal>("brewup.beer.sold");
	}

	public void BeerSold(string beerName, decimal quantity)
	{
		_beerSoldCounter.Add(quantity, new KeyValuePair<string, object?>("brewup.beer.name", beerName));
	}
}