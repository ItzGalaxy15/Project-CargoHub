using apiV1.Interfaces;

namespace apiV1.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseProvider warehouseProvider;

        public WarehouseService(IWarehouseProvider warehouseProvider)
        {
            this.warehouseProvider = warehouseProvider;
        }

        public Warehouse[] GetWarehouses()
        {
            return this.warehouseProvider.Get();
        }

        public Warehouse? GetWarehouseById(int id)
        {
            Warehouse[] warehouses = this.GetWarehouses();
            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.Id == id);
            return warehouse;
        }

        public async Task AddWarehouse(Warehouse warehouse)
        {
            string now = warehouse.GetTimeStamp();
            warehouse.CreatedAt = now;
            warehouse.UpdatedAt = now;
            this.warehouseProvider.Add(warehouse);
            await this.warehouseProvider.Save();
        }

        public async Task ReplaceWarehouse(Warehouse warehouse, int warehouseId)
        {
            string now = warehouse.GetTimeStamp();
            warehouse.UpdatedAt = now;
            this.warehouseProvider.Update(warehouse, warehouseId);
            await this.warehouseProvider.Save();
        }

        public async Task DeleteWarehouse(Warehouse warehouse)
        {
            this.warehouseProvider.Delete(warehouse);
            await this.warehouseProvider.Save();
        }
    }
}