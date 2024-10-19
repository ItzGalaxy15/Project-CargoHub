public interface ISupplierService
{
    public Supplier[] GetSuppliers();

    public Supplier? GetSupplierById(int id);
}
