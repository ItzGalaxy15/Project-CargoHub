namespace apiV2.Interfaces
{
    public interface ISupplierService
    {
        public Supplier[] GetSuppliers();

        public Supplier? GetSupplierById(int id);

        public Task AddSupplier(Supplier supplier);

        public Task DeleteSupplier(Supplier supplier);

        public Task ReplaceSupplier(Supplier supplier, int supplierId);

        public Task ModifySupplier(int id, Dictionary<string, dynamic> patch, Supplier supplier);
    }
}