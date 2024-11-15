using apiV2.Interfaces;

namespace apiV2.Services
{
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

        public async Task AddWarehouse(Warehouse warehouse)
        {
            string now = warehouse.GetTimeStamp();
            warehouse.CreatedAt = now;
            warehouse.UpdatedAt = now;
            _warehouseProvider.Add(warehouse);
            await _warehouseProvider.Save();
        }

        public async Task ReplaceWarehouse(Warehouse warehouse, int warehouseId)
        {
            string now = warehouse.GetTimeStamp();
            warehouse.UpdatedAt = now;
            _warehouseProvider.Replace(warehouse, warehouseId);
            await _warehouseProvider.Save();

        }
        public async Task DeleteWarehouse(Warehouse warehouse)
        {
            _warehouseProvider.Delete(warehouse);
            await _warehouseProvider.Save();
        }
    }
}