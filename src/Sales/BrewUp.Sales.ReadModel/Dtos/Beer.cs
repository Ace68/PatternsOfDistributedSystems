using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Entities;

namespace BrewUp.Sales.ReadModel.Dtos;

public class Beer : EntityBase
{
    public string BeerName { get; private set; } = string.Empty;
    
    protected Beer()
    {}

    public static Beer CreateBeer(BeerId beerId, BeerName beerName) =>
        new Beer(beerId.Value.ToString(), beerName.Value);
    
    private Beer(string beerId, string beerName)
    {
        Id = beerId;
        BeerName = beerName;
    }
    
    public BeerJson ToJson() => new(Id, BeerName);
}