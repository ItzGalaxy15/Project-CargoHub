public class WarehouseProvider : BaseProvider<Warehouse>, IWarehouseProvider
{
    public WarehouseProvider(List<Warehouse> mockData) : base(mockData) { }
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

    public void Update(Warehouse warehouse, int warehouseId)
    {
        warehouse.Id = warehouseId;
        int index = context.FindIndex(w => w.Id == warehouseId);
        context[index] = warehouse;
    }
}
