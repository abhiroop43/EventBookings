using BuildingBlocks.Exceptions;

namespace Lookups.Api.Exceptions;

public class LookupNotFoundException(string lookupId) : NotFoundException(nameof(Lookup), lookupId);