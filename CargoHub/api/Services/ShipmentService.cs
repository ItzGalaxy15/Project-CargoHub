public class ShipmentService : IShipmentService
{
    private readonly IShipmentProvider _shipmentProvider;
    public ShipmentService(IShipmentProvider shipmentProvider)
    {
        _shipmentProvider = shipmentProvider;
    }

    public Shipment[] GetShipments()
    {
        return _shipmentProvider.Get();
    }
}
