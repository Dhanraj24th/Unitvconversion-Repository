namespace UnitConversion.Api.Services;

/// <summary>Central registry of supported units and their mapping to each category's base unit.</summary>
public sealed class UnitCatalog : IUnitCatalog
{
    private readonly IReadOnlyDictionary<string, UnitDefinition> _units;

    public UnitCatalog()
    {
        var units = new[]
        {
            new UnitDefinition("m", "meter", "length", 1m),
            new UnitDefinition("km", "kilometer", "length", 1_000m),
            new UnitDefinition("cm", "centimeter", "length", 0.01m),
            new UnitDefinition("mm", "millimeter", "length", 0.001m),
            new UnitDefinition("mi", "mile", "length", 1_609.344m),
            new UnitDefinition("yd", "yard", "length", 0.9144m),
            new UnitDefinition("ft", "foot", "length", 0.3048m),
            new UnitDefinition("in", "inch", "length", 0.0254m),
            new UnitDefinition("kg", "kilogram", "mass", 1m),
            new UnitDefinition("g", "gram", "mass", 0.001m),
            new UnitDefinition("mg", "milligram", "mass", 0.000001m),
            new UnitDefinition("lb", "pound", "mass", 0.45359237m),
            new UnitDefinition("oz", "ounce", "mass", 0.028349523125m),
            new UnitDefinition("t", "metric tonne", "mass", 1_000m),
            new UnitDefinition("C", "degree Celsius", "temperature", 1m),
            new UnitDefinition("F", "degree Fahrenheit", "temperature", 5m / 9m, -160m / 9m),
            new UnitDefinition("K", "kelvin", "temperature", 1m, -273.15m)
        };

        _units = units.ToDictionary(unit => unit.Symbol, StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGet(string symbol, out UnitDefinition unit) => _units.TryGetValue(symbol.Trim(), out unit!);

    public IReadOnlyCollection<UnitDefinition> GetAll() => _units.Values.ToArray();
}