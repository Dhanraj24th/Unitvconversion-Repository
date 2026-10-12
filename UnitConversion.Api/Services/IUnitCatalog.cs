namespace UnitConversion.Api.Services;

public interface IUnitCatalog
{
    bool TryGet(string symbol, out UnitDefinition unit);
    IReadOnlyCollection<UnitDefinition> GetAll();
}