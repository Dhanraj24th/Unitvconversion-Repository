using System.ComponentModel.DataAnnotations;

namespace UnitConversion.Api.Models;

public sealed record ConversionRequest(
    [Required, StringLength(32, MinimumLength = 1)] string From,
    [Required, StringLength(32, MinimumLength = 1)] string To,
    decimal Value);