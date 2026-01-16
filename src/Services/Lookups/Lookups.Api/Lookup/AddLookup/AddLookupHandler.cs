using BuildingBlocks.CQRS;

namespace Lookups.Api.Lookup.AddLookup;

public record AddLookupCommand(Models.Lookup Lookup) : ICommand<AddLookupResult>;

public record AddLookupResult(string Id);

public class AddLookupCommandHandler : ICommandHandler<AddLookupCommand, AddLookupResult>
{
    public Task<AddLookupResult> Handle(AddLookupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}