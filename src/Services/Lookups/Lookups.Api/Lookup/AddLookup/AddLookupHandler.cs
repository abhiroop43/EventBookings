using BuildingBlocks.CQRS;
using FluentValidation;
using Lookups.Api.Data;

namespace Lookups.Api.Lookup.AddLookup;

public record AddLookupCommand(Models.Lookup Lookup) : ICommand<AddLookupResult>;

public record AddLookupResult(string Id);

public class AddLookupCommandValidator : AbstractValidator<AddLookupCommand>
{
    public AddLookupCommandValidator()
    {
        RuleFor(x => x.Lookup).NotNull().WithMessage("Lookup cannot be null");
        RuleFor(x => x.Lookup.LookupType).NotEmpty().WithMessage("Lookup Type cannot be empty");
        RuleFor(x => x.Lookup.Value).NotEmpty().WithMessage("Lookup Value cannot be empty");
        RuleFor(x => x.Lookup.Key).NotEmpty().WithMessage("Lookup Key cannot be empty");
    }
}

public class AddLookupCommandHandler(ILookupRepository repository) : ICommandHandler<AddLookupCommand, AddLookupResult>
{
    public async Task<AddLookupResult> Handle(AddLookupCommand command, CancellationToken cancellationToken)
    {
        var lookup = command.Lookup;
        await repository.AddAsync(lookup, cancellationToken);

        return new AddLookupResult(lookup.Id.ToString());
    }
}