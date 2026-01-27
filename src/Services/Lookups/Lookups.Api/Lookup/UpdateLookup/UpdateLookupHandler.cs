using BuildingBlocks.CQRS;
using FluentValidation;
using Lookups.Api.Data;
using Lookups.Api.Exceptions;

namespace Lookups.Api.Lookup.UpdateLookup;

public record UpdateLookupCommand(Models.Lookup Lookup) : ICommand<UpdateLookupResult>;

public record UpdateLookupResult(bool IsUpdated);

public class UpdateLookupCommandValidator : AbstractValidator<UpdateLookupCommand>
{
    public UpdateLookupCommandValidator()
    {
        RuleFor(x => x.Lookup).NotNull().WithMessage("Lookup cannot be null");
        RuleFor(x => x.Lookup.Id).NotEmpty().WithMessage("Lookup Id cannot be empty");
        RuleFor(x => x.Lookup.LookupType).NotEmpty().WithMessage("Lookup Type cannot be empty");
        RuleFor(x => x.Lookup.Value).NotEmpty().WithMessage("Lookup Value cannot be empty");
        RuleFor(x => x.Lookup.Key).NotEmpty().WithMessage("Lookup Key cannot be empty");
    }
}

public class UpdateLookupHandler(ILookupRepository repository)
    : ICommandHandler<UpdateLookupCommand, UpdateLookupResult>
{
    public async Task<UpdateLookupResult> Handle(
        UpdateLookupCommand command,
        CancellationToken cancellationToken
    )
    {
        var lookup = command.Lookup;
        var isUpdated = await repository.UpdateAsync(lookup, cancellationToken);

        return isUpdated
            ? new UpdateLookupResult(true)
            : throw new LookupNotFoundException(lookup.Id.ToString());
    }
}
