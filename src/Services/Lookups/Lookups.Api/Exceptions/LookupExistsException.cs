using BuildingBlocks.Exceptions;

namespace Lookups.Api.Exceptions;

public class LookupExistsException(string message) : BadRequestException(message)
{
}