using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Models;
using UnitConversion.Api.Services;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ConversionsController(IConversionService conversionService, IUnitCatalog catalog) : ControllerBase
{
    [HttpPost("conversions")]
    [ProducesResponseType<ConversionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<ConversionResponse> Convert([FromBody] ConversionRequest request)
    {
        try
        {
            return Ok(conversionService.Convert(request));
        }
        catch (ConversionException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Conversion could not be completed.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("units")]
    [ProducesResponseType<IEnumerable<UnitInfo>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<UnitInfo>> GetUnits([FromQuery] string? category = null)
    {
        var units = catalog.GetAll()
            .Where(unit => category is null || string.Equals(unit.Category, category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(unit => unit.Category)
            .ThenBy(unit => unit.Symbol)
            .Select(unit => new UnitInfo(unit.Symbol, unit.Name, unit.Category));

        return Ok(units);
    }
}