﻿using ResilienceBlazor.Shared.CustomTypes;

namespace ResilienceBlazor.Modules.Sales.Extensions.Dtos;

public class SalesOrderRowJson
{
	public Guid BeerId { get; set; } = Guid.Empty;
	public string BeerName { get; set; } = string.Empty;
	public Quantity Quantity { get; set; } = new(0, string.Empty);
	public Price Price { get; set; } = new(0, string.Empty);
}