using UnitConversion.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IUnitCatalog, UnitCatalog>();
builder.Services.AddScoped<IConversionService, ConversionService>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
