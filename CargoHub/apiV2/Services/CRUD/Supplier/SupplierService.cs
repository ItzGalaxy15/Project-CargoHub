using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierProvider supplierProvider;

        public SupplierService(ISupplierProvider supplierProvider)
        {
            this.supplierProvider = supplierProvider;
        }

        public Supplier[] GetSuppliers()
        {
            return this.supplierProvider.Get();
        }

        public Supplier? GetSupplierById(int id)
        {
            Supplier[] suppliers = this.supplierProvider.Get();
            Supplier? supplier = suppliers.FirstOrDefault(sup => sup.Id == id);
            return supplier;
        }

        public async Task AddSupplier(Supplier supplier)
        {
            string now = supplier.GetTimeStamp();
            supplier.CreatedAt = now;
            supplier.UpdatedAt = now;
            this.supplierProvider.Add(supplier);
            await this.supplierProvider.Save();
        }

        public async Task DeleteSupplier(Supplier supplier)
        {
            this.supplierProvider.Delete(supplier);
            await this.supplierProvider.Save();
        }

        public async Task ReplaceSupplier(Supplier supplier, int supplierId)
        {
            string now = supplier.GetTimeStamp();
            supplier.UpdatedAt = now;

            this.supplierProvider.Update(supplier, supplierId);
            await this.supplierProvider.Save();
        }

        public async Task ModifySupplier(int id, Dictionary<string, dynamic> patch, Supplier supplier)
        {
            foreach (var (key, value) in patch)
            {
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "code":
                            supplier.Code = jsonElement.GetString()!;
                            break;

                        case "name":
                            supplier.Name = jsonElement.GetString()!;
                            break;

                        case "address":
                            supplier.Address = jsonElement.GetString()!;
                            break;

                        case "address_extra":
                            supplier.AddressExtra = jsonElement.GetString()!;
                            break;

                        case "city":
                            supplier.City = jsonElement.GetString()!;
                            break;

                        case "zip_code":
                            supplier.ZipCode = jsonElement.GetString()!;
                            break;

                        case "country":
                            supplier.Country = jsonElement.GetString()!;
                            break;

                        case "contact_name":
                            supplier.ContactName = jsonElement.GetString()!;
                            break;

                        case "phonenumber":
                            supplier.Phonenumber = jsonElement.GetString()!;
                            break;

                        case "reference":
                            supplier.Reference = jsonElement.GetString()!;
                            break;
                    }
                }
            }

            supplier.UpdatedAt = supplier.GetTimeStamp();
            this.supplierProvider.Update(supplier, id);
            await this.supplierProvider.Save();
        }
    }
}