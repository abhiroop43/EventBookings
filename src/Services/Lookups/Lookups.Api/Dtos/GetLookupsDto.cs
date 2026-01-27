namespace Lookups.Api.Dtos;

public record GetLookupsChildDto(
    string Id,
    string Key,
    string Value,
    string LookupType,
    List<GetLookupsChildDto> Children
);

public record GetLookupsDto(
    string Id,
    string Key,
    string Value,
    string LookupType,
    List<GetLookupsChildDto> Children
);
