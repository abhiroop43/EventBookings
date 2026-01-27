namespace Lookups.Api.Dtos;

public record AddLookupChildDto(
    string Key,
    string Value,
    string LookupType,
    List<AddLookupChildDto> Children
);

public record AddLookupDto(
    string Key,
    string Value,
    string LookupType,
    List<AddLookupChildDto> Children
);
