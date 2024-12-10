namespace apiV2.ValidationInterfaces
{
    public interface IWarehouseValidationService
    {
        public bool IsWarehouseValid(Warehouse? warehouse, bool update = false);

        public bool IsWarehouseValidForPatch(Dictionary<string, dynamic> patch);
    }
}