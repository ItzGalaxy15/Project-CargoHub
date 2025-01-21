public class WarehouseProvider : BaseProvider<Warehouse>, IWarehouseProvider
{
    public WarehouseProvider(List<Warehouse> mockData)
        : base(mockData)
    {
    }

    public WarehouseProvider()
        : base("data/warehouses.json")
    {
    }

    public Warehouse[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Warehouse warehouse)
    {
        this.context.Add(warehouse);
    }

    public void Delete(Warehouse warehouse)
    {
        warehouse.IsDeleted = true;
        warehouse.UpdatedAt = warehouse.GetTimeStamp();
    }

    public void Update(Warehouse warehouse, int warehouseId)
    {
        warehouse.Id = warehouseId;
        int index = this.context.FindIndex(w => w.Id == warehouseId);
        this.context[index] = warehouse;
    }
}
