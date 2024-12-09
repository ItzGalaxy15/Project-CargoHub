public class ShipmentProvider : BaseProvider<Shipment>, IShipmentProvider
{
    public ShipmentProvider(List<Shipment> mockData) : base(mockData) { }
    public ShipmentProvider() : base("test_data/shipments.json") {}

    public Shipment[] Get()
    {
        return context.ToArray();
    }

    public void Add(Shipment shipment)
    {
        context.Add(shipment);
    }

    public void Delete(Shipment shipment)
    {
        context.Remove(shipment);
    }

    public void Update(Shipment shipment, int shipmentId){
        shipment.Id = shipmentId;
        int index = context.FindIndex(ship => ship.Id == shipmentId);
        context[index] = shipment;
    }
}
