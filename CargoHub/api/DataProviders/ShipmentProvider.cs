public class ShipmentProvider : BaseProvider<Shipment>, IShipmentProvider
{
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

    public bool Replace(Shipment shipment){
        int index = context.FindIndex(ship => ship.Id == shipment.Id);
        if (index == -1) return false;
        context[index] = shipment;
        return true;
    }
}
