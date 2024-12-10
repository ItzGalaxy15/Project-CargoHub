using apiV1.Interfaces;

namespace apiV1.Services
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
    }
}