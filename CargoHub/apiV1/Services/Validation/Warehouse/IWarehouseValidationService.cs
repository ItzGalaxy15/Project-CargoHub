public interface IWarehouseValidationService
{
    public bool IsWarehouseValid(Warehouse? warehouse, bool update = false);
}