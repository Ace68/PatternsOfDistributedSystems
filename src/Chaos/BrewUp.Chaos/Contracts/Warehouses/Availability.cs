namespace BrewUp.Chaos.Contracts.Warehouses;

public record Availability(decimal Requested, decimal Available, string UnitOfMeasure);