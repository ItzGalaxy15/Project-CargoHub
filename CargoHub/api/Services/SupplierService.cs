public class SupplierService : ISupplierService
{
    private readonly ISupplierProvider _supplierProvider;
    public SupplierService(ISupplierProvider supplierProvider){
        _supplierProvider = supplierProvider;
    }

    public Supplier[] GetSuppliers(){
        return _supplierProvider.Get();
    }

    public Supplier? GetSupplierById(int id){
        Supplier[] suppliers = _supplierProvider.Get();
        Supplier? supplier = suppliers.FirstOrDefault(sup => sup.Id == id);
        return supplier;
    }

    public async Task<bool> AddSupplier(Supplier supplier){
        // check if supplier is valid, like no duplicate id, else return false
        Supplier[] suppliers = _supplierProvider.Get();
        if (suppliers.Any(sup => sup.Id == supplier.Id)) return false;

        string now = supplier.GetTimeStamp();
        supplier.CreatedAt = now;
        supplier.UpdatedAt = now;
        _supplierProvider.Add(supplier);
        await _supplierProvider.Save();
        return true;
    }

    public async Task DeleteSupplier(Supplier supplier){
        _supplierProvider.Delete(supplier);
        await _supplierProvider.Save();
    }

    public async Task<bool> ReplaceSupplier(Supplier supplier){
        // check if supplier is valid (like in AddSupplier), else return false
        // so, should probably be a seperate method/service to check when a supplier is valid

        string now = supplier.GetTimeStamp();
        supplier.CreatedAt = now;
        supplier.UpdatedAt = now;

        // will return false if there is no supplier with the same id
        if (!_supplierProvider.Replace(supplier)) return false;
        await _supplierProvider.Save();

        return true;
    }
}
