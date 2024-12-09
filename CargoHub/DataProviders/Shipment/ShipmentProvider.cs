public class ShipmentProvider : BaseProvider<Shipment>, IShipmentProvider
{
    public ShipmentProvider(List<Shipment> mockData)
        : base(mockData)
    {
    }

    public ShipmentProvider()
        : base("test_data/shipments.json")
    {
    }

    public Shipment[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Shipment shipment)
    {
        this.context.Add(shipment);
    }

    public void Delete(Shipment shipment)
    {
        this.context.Remove(shipment);
    }

    public void Update(Shipment shipment, int shipmentId)
    {
        shipment.Id = shipmentId;
        int index = this.context.FindIndex(ship => ship.Id == shipmentId);
        this.context[index] = shipment;
    }
}
