public interface IShipmentProvider
{
    public List<Shipment> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Shipment[] Get();
}
