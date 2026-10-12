using UnitConversion.Api.Models;

namespace UnitConversion.Api.Services;

public sealed class ConversionService(IUnitCatalog catalog) : IConversionService
{
    public ConversionResponse Convert(ConversionRequest request)
    {
        if (!catalog.TryGet(request.From, out var from))
        {
            throw new ConversionException($"Unknown source unit '{request.From}'. See GET /api/v1/units for supported units.");
        }

        if (!catalog.TryGet(request.To, out var to))
        {
            throw new ConversionException($"Unknown target unit '{request.To}'. See GET /api/v1/units for supported units.");
        }

        if (!string.Equals(from.Category, to.Category, StringComparison.Ordinal))
        {
            throw new ConversionException($"Units '{from.Symbol}' and '{to.Symbol}' belong to different categories.");
        }

        var baseValue = request.Value * from.ScaleToBase + from.OffsetToBase;
        var result = (baseValue - to.OffsetToBase) / to.ScaleToBase;

        return new ConversionResponse(from.Category, request.Value, from.Symbol, result, to.Symbol);
    }
}