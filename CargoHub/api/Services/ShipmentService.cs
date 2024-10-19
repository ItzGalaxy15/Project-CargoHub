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

    public Shipment? GetShipmentById(int id){
        Shipment[] shipments = _shipmentProvider.Get();
        Shipment? shipment = shipments.FirstOrDefault(ship => ship.Id == id);
        return shipment;
    }
}
