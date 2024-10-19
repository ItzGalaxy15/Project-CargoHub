public class SupplierProvider : BaseProvider<Supplier>, ISupplierProvider
{
    public SupplierProvider() : base("test_data/suppliers.json"){}

    public Supplier[] Get(){
        return context.ToArray();
    }
    
    public void Add(Supplier supplier){
        context.Add(supplier);
    }

    public void Delete(Supplier supplier){
        context.Remove(supplier);
    }

    public bool Replace(Supplier supplier){
        int index = context.FindIndex(sup => sup.Id == supplier.Id);
        if (index == -1) return false;
        context[index] = supplier;
        return true;
    }
}
