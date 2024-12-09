public interface IShipmentProvider
{
    public List<Shipment> context { get; set; }

    public string? path { get; set; }

    public Task Save();

    public Shipment[] Get();

    public void Add(Shipment shipment);

    public void Delete(Shipment shipment);

    public void Update(Shipment shipment, int shipmentId);
}
