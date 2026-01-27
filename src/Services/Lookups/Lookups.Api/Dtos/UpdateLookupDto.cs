namespace Lookups.Api.Dtos;

public record UpdateLookupChildDto(
    string Id,
    string Key,
    string Value,
    string LookupType,
    List<UpdateLookupChildDto> Children
);

public record UpdateLookupDto(
    string Id,
    string Key,
    string Value,
    string LookupType,
    List<UpdateLookupChildDto> Children
);
