namespace UnitConversion.Api.Services;

public sealed record UnitDefinition(
    string Symbol,
    string Name,
    string Category,
    decimal ScaleToBase,
    decimal OffsetToBase = 0m);