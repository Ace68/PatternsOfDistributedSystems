using ResilienceBlazor.Shared.CustomTypes;

namespace ResilienceBlazor.Modules.Warehouses.Extensions.Dtos;

public record AvailabilityJson(string BeerId, string BeerName, Quantity Quantity);