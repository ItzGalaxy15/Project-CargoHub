namespace apiV1.ValidationInterfaces
{    
    public interface IWarehouseValidationService
    {
        public bool IsWarehouseValid(Warehouse? warehouse, bool update = false);
    }
}