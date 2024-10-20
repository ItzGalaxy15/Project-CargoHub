public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseProvider _warehouseProvider;
    public WarehouseService(IWarehouseProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;
    }

    public Warehouse[] GetWarehouses()
    {
        return _warehouseProvider.Get();
    }

    public Warehouse? GetWarehouseById(int id)
    {
        Warehouse[] warehouses = GetWarehouses();
        Warehouse? warehouse = warehouses.FirstOrDefault(w => w.Id == id);
        return warehouse;
    }

    public async Task<bool> AddWarehouse(Warehouse warehouse)
    {
        // Check if warehouse is valid
        
        // Check if warehouse id is already in use
        Warehouse[] warehouses = GetWarehouses();
        if (warehouses.Any(w => w.Id == warehouse.Id)) return false;

        string now = warehouse.GetTimeStamp();
        warehouse.CreatedAt = now;
        warehouse.UpdatedAt = now;
        _warehouseProvider.Add(warehouse);
        await _warehouseProvider.Save();
        return true;
    }
    public Task DeleteWarehouse(Warehouse warehouse)
    {
        throw new NotImplementedException();
    }


    public Task<bool> ReplaceWarehouse(Warehouse warehouse, int warehouseId)
    {
        throw new NotImplementedException();
    }

}