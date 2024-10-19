public class ShipmentProvider : BaseProvider<Shipment>, IShipmentProvider
{
    public ShipmentProvider() : base("test_data/shipments.json") {}

    public Shipment[] Get()
    {
        return context.ToArray();
    }
}
