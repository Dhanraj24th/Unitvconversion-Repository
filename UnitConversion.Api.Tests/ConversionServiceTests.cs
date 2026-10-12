using UnitConversion.Api.Models;
using UnitConversion.Api.Services;

namespace UnitConversion.Api.Tests;

public sealed class ConversionServiceTests
{
    private readonly ConversionService _service = new(new UnitCatalog());

    public static TheoryData<string, string, decimal, decimal> LinearConversions => new()
    {
        { "m", "ft", 1m, 3.280839895013123m },
        { "kg", "g", 2.5m, 2500m },
        { "mi", "km", 1m, 1.609344m }
    };

    [Theory]
    [MemberData(nameof(LinearConversions))]
    public void Convert_LinearUnits_ReturnsExpectedResult(string from, string to, decimal value, decimal expected)
    {
        var result = _service.Convert(new ConversionRequest(from, to, value));

        Assert.Equal(expected, result.Result, 10);
        Assert.Equal(value, result.Value);
        Assert.Equal(from, result.From);
        Assert.Equal(to, result.To);
    }

    public static TheoryData<string, string, decimal, decimal> TemperatureConversions => new()
    {
        { "C", "F", 0m, 32m },
        { "F", "C", 32m, 0m },
        { "C", "K", 0m, 273.15m },
        { "K", "C", 273.15m, 0m }
    };

    [Theory]
    [MemberData(nameof(TemperatureConversions))]
    public void Convert_TemperatureUnits_UsesAffineConversion(string from, string to, decimal value, decimal expected)
    {
        var result = _service.Convert(new ConversionRequest(from, to, value));

        Assert.Equal(expected, result.Result, 10);
        Assert.Equal("temperature", result.Category);
    }

    [Fact]
    public void Convert_DifferentCategories_ThrowsConversionException()
    {
        var exception = Assert.Throws<ConversionException>(() =>
            _service.Convert(new ConversionRequest("m", "kg", 1m)));

        Assert.Contains("different categories", exception.Message);
    }

    [Fact]
    public void Convert_UnknownUnit_ThrowsConversionException()
    {
        var exception = Assert.Throws<ConversionException>(() =>
            _service.Convert(new ConversionRequest("parsec", "m", 1m)));

        Assert.Contains("Unknown source unit", exception.Message);
    }

    [Fact]
    public void Catalog_ContainsUnitsFromAllRequiredCategories()
    {
        var categories = new UnitCatalog().GetAll().Select(unit => unit.Category).Distinct();

        Assert.Contains("length", categories);
        Assert.Contains("temperature", categories);
        Assert.Contains("mass", categories);
    }
}