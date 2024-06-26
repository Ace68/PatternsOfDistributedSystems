﻿namespace BrewUp.Chaos.Contracts.Sales;

public record SalesOrderJson(string SalesOrderId, string SalesOrderNumber, Guid CustomerId, string CustomerName, DateTime OrderDate,
	IEnumerable<SalesOrderRowJson> Rows);
