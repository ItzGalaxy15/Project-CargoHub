using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
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

        public async Task ModifyWarehouse(int id, Dictionary<string, dynamic> patch, Warehouse warehouse)
        {
            foreach (var (key, value) in patch)
            {
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "code":
                            warehouse.Code = jsonElement.GetString()!;
                            break;

                        case "name":
                            warehouse.Name = jsonElement.GetString()!;
                            break;

                        case "address":
                            warehouse.Address = jsonElement.GetString()!;
                            break;

                        case "zip":
                            warehouse.Zip = jsonElement.GetString()!;
                            break;

                        case "city":
                            warehouse.City = jsonElement.GetString()!;
                            break;

                        case "province":
                            warehouse.Province = jsonElement.GetString()!;
                            break;

                        case "country":
                            warehouse.Country = jsonElement.GetString()!;
                            break;

                        case "contact":
                            if (jsonElement.ValueKind == JsonValueKind.Object)
                            {
                                var contact = warehouse.Contact;
                                foreach (var contactProperty in jsonElement.EnumerateObject())
                                {
                                    switch (contactProperty.Name)
                                    {
                                        case "name":
                                            contact.Name = contactProperty.Value.GetString()!;
                                            break;

                                        case "phone":
                                            contact.Phone = contactProperty.Value.GetString()!;
                                            break;

                                        case "email":
                                            contact.Email = contactProperty.Value.GetString()!;
                                            break;
                                    }
                                }

                                warehouse.Contact = contact;
                            }

                            break;
                    }
                }
            }

            warehouse.UpdatedAt = warehouse.GetTimeStamp();
            this.warehouseProvider.Update(warehouse, id);
            await this.warehouseProvider.Save();
        }
    }
}