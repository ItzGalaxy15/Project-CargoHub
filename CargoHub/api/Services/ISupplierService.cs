public interface ISupplierService
{
    public Supplier[] GetSuppliers();
    public Supplier? GetSupplierById(int id);
    public Task<bool> AddSupplier(Supplier supplier);
    public Task DeleteSupplier(Supplier supplier);
    public Task<bool> ReplaceSupplier(Supplier supplier, int supplierId);
}
