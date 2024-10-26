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

    public ItemSmall[] GetShipmentItems(Shipment shipment){
        return shipment.Items.ToArray();
    }

    public async Task AddShipment(Shipment shipment){
        string now = shipment.GetTimeStamp();
        shipment.CreatedAt = now;
        shipment.UpdatedAt = now;
        _shipmentProvider.Add(shipment);
        await _shipmentProvider.Save();
    }

    public async Task DeleteShipment(Shipment shipment){
        _shipmentProvider.Delete(shipment);
        await _shipmentProvider.Save();
    }

    public async Task ReplaceShipment(Shipment shipment, int shipmentId){
        string now = shipment.GetTimeStamp();
        shipment.CreatedAt = now;
        shipment.UpdatedAt = now;

        _shipmentProvider.Replace(shipment, shipmentId);
        await _shipmentProvider.Save();
    }
}
