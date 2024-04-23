using BrewUp.Sales.Domain;
using BrewUp.Sales.Facade.Validators;
using BrewUp.Sales.Infrastructures;
using BrewUp.Sales.ReadModel.Dtos;
using BrewUp.Sales.ReadModel.Queries;
using BrewUp.Sales.ReadModel.Services;
using BrewUp.Shared.ReadModel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Sales.Facade;

public static class SalesHelper
{
	public static IServiceCollection AddSales(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddValidatorsFromAssemblyContaining<SalesOrderValidator>();
		services.AddSingleton<ValidationHandler>();

		services.AddScoped<ISalesFacade, SalesFacade>();
		services.AddScoped<ISalesDomainService, SalesDomainService>();
		services.AddScoped<ISalesQueryService, SalesQueryService>();
		services.AddScoped<IQueries<SalesOrder>, SalesOrderQueries>();

		return services;
	}

	public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services)
	{
		services.AddSalesMongoDb();

		return services;
	}
}