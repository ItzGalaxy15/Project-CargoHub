public interface ISupplierService
{
    public Supplier[] GetSuppliers();
    public Supplier? GetSupplierById(int id);
    public Task<bool> AddSupplier(Supplier supplier);
}
