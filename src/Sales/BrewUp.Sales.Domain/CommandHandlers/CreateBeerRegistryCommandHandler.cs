using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public sealed class CreateBeerRegistryCommandHandler  :CommandHandlerBaseAsync<CreateBeerRegistry>
{
    public CreateBeerRegistryCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
    {
    }

    public override async Task ProcessCommand(CreateBeerRegistry command, CancellationToken cancellationToken = default)
    {
        var aggregate = Beer.CreateBeer(command.BeerId, command.BeerName, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid());
    }
}