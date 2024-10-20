
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

    public Task<bool> AddWarehouse(Warehouse warehouse)
    {
        throw new NotImplementedException();
    }

    public Task DeleteWarehouse(Warehouse warehouse)
    {
        throw new NotImplementedException();
    }

    public Warehouse? GetWarehouseById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReplaceWarehouse(Warehouse warehouse, int warehouseId)
    {
        throw new NotImplementedException();
    }
}