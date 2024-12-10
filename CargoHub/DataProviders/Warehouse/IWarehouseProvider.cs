public interface IWarehouseProvider
{
    public List<Warehouse> context { get; set; }

    public string? path { get; set; }

    public Task Save();

    public Warehouse[] Get();

    public void Add(Warehouse warehouse);

    public void Delete(Warehouse warehouse);

    public void Update(Warehouse warehouse, int warehouseId);
}
