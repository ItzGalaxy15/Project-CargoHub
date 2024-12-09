namespace apiV2.Interfaces
{
    public interface IWarehouseService
    {
        public Warehouse[] GetWarehouses();

        public Warehouse? GetWarehouseById(int id);

        public Task AddWarehouse(Warehouse warehouse);

        public Task DeleteWarehouse(Warehouse warehouse);

        public Task ReplaceWarehouse(Warehouse warehouse, int warehouseId);

        public Task ModifyWarehouse(int id, Dictionary<string, dynamic> patch, Warehouse warehouse);
    }
}