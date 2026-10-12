# Unit Conversion API

A versioned ASP.NET Core Web API for converting values across length, mass, and temperature units. Conversion data is held in an injectable catalog so additional units or a persistent catalog can be introduced without changing the conversion endpoint.

## Requirements

- .NET 10 SDK

## Run locally

From the repository root:

```powershell
dotnet restore
dotnet run --project UnitConversion.Api
```

The launch profile prints the local HTTP and HTTPS addresses. In Development, the OpenAPI document is available at `/openapi/v1.json`; the health check is at `/health`.

## API

### Convert a value

`POST /api/v1/conversions`

```json
{
  "From": "mi",
  "To": "km",
  "Value": 2
}
```

Example response:

```json
{
  "Category": "length",
  "Value": 2,
  "From": "mi",
  "Result": 3.218688,
  "To": "km"
}
```

Unit symbols are case-insensitive. An unsupported unit or a conversion between categories returns HTTP 400 with Problem Details.

### List supported units

`GET /api/v1/units` returns all supported units. Filter by category with `GET /api/v1/units?category=length`.

Supported symbols:

- Length: `m`, `km`, `cm`, `mm`, `mi`, `yd`, `ft`, `in`
- Mass: `kg`, `g`, `mg`, `lb`, `oz`, `t`
- Temperature: `C`, `F`, `K`

## Test

```powershell
dotnet test
```

## Design notes

- The API is versioned under `/api/v1`. The conversion endpoint and unit discovery endpoint have separate responsibilities.
- A unit catalog describes each unit by category and its scale and offset relative to that category's base unit. This supports both multiplicative conversions and affine temperature conversions through one algorithm.
- The catalog and conversion service are behind interfaces and registered with dependency injection. The current catalog is intentionally in-memory; a database-backed implementation can replace it if units need to be maintained dynamically.
- Decimal arithmetic is used for conversion factors and values to avoid unnecessary binary floating-point artifacts in typical decimal input/output.
- Conversion results are not rounded by the service so callers retain precision and may format values for their own use case.
