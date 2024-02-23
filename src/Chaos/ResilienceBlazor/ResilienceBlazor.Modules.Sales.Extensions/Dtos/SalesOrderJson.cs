namespace ResilienceBlazor.Modules.Sales.Extensions.Dtos;

public record SalesOrderJson(string SalesOrderId, string SalesOrderNumber, Guid CustomerId, string CustomerName, DateTime OrderDate,
	IEnumerable<SalesOrderRowJson> Rows);