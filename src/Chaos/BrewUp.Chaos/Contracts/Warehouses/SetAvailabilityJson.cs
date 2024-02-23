using BrewUp.Chaos.Contracts.Sales;

namespace BrewUp.Chaos.Contracts.Warehouses;

public record SetAvailabilityJson(string BeerId, string BeerName, Quantity Quantity);