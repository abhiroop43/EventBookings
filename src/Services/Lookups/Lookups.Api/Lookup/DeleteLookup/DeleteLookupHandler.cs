using BuildingBlocks.CQRS;
using Lookups.Api.Data;
using Lookups.Api.Exceptions;
using MongoDB.Bson;

namespace Lookups.Api.Lookup.DeleteLookup;

public record DeleteLookupCommand(ObjectId Id) : ICommand<DeleteLookupResult>;

public record DeleteLookupResult(bool IsDeleted);

public class DeleteLookupCommandHandler(ILookupRepository repository)
    : ICommandHandler<DeleteLookupCommand, DeleteLookupResult>
{
    public async Task<DeleteLookupResult> Handle(
        DeleteLookupCommand command,
        CancellationToken cancellationToken
    )
    {
        var success = await repository.DeleteAsync(command.Id, cancellationToken);

        return !success
            ? throw new LookupNotFoundException(command.Id.ToString())
            : new DeleteLookupResult(true);
    }
}
