using BuildingBlocks.CQRS;
using Lookups.Api.Data;

namespace Lookups.Api.Lookup.AddLookup;

public record AddLookupCommand(Models.Lookup Lookup) : ICommand<AddLookupResult>;

public record AddLookupResult(string Id);

public class AddLookupCommandHandler(ILookupRepository repository) : ICommandHandler<AddLookupCommand, AddLookupResult>
{
    public async Task<AddLookupResult> Handle(AddLookupCommand command, CancellationToken cancellationToken)
    {
        var lookup = command.Lookup;
        await repository.AddAsync(lookup, cancellationToken);

        return new AddLookupResult(lookup.Id.ToString());
    }
}