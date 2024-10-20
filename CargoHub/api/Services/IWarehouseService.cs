public interface IWarehouseService
{
    public Warehouse[] GetWarehouses();
    public Warehouse? GetWarehouseById(int id);

    public Task<bool> AddWarehouse(Warehouse warehouse);
    public Task DeleteWarehouse(Warehouse warehouse);
    public Task<bool> ReplaceWarehouse(Warehouse warehouse, int warehouseId);
}
