public class ShipmentProvider : BaseProvider<Shipment>, IShipmentProvider
{
    public ShipmentProvider(List<Shipment> mockData)
        : base(mockData)
    {
    }

    public ShipmentProvider()
        : base("data/shipments.json")
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
        shipment.IsDeleted = true;
        shipment.UpdatedAt = shipment.GetTimeStamp();
    }

    public void Update(Shipment shipment, int shipmentId)
    {
        shipment.Id = shipmentId;
        int index = this.context.FindIndex(ship => ship.Id == shipmentId);
        this.context[index] = shipment;
    }
}
