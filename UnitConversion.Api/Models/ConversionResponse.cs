namespace UnitConversion.Api.Models;

public sealed record ConversionResponse(
    string Category,
    decimal Value,
    string From,
    decimal Result,
    string To);