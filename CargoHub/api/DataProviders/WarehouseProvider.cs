public class WarehouseProvider : BaseProvider<Warehouse>, IWarehouseProvider
{
    public WarehouseProvider() : base("test_data/warehouses.json") {}

    public Warehouse[] Get()
    {
        return context.ToArray();
    }

    public void Add(Warehouse warehouse)
    {
        context.Add(warehouse);
    }

    public void Delete(Warehouse warehouse)
    {
        context.Remove(warehouse);
    }

    public bool Replace(Warehouse warehouse, int warehouseId){
        int index = context.FindIndex(w => w.Id == warehouseId);
        if (index == -1) return false;
        context[index] = warehouse;
        return true;
    }
}
