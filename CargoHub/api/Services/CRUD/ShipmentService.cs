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

    public async Task<bool> AddShipment(Shipment shipment){
        // Check if shipment is valid
        /*
         * all items exist
         * order id is valid
         * date is valid
         * ship_to and bill_to are valid
         etc.
        */

        // Check if shipment id is already in use
        Shipment[] shipments = _shipmentProvider.Get();
        if (shipments.Any(ord => ord.Id == shipment.Id)) return false;

        string now = shipment.GetTimeStamp();
        shipment.CreatedAt = now;
        shipment.UpdatedAt = now;
        _shipmentProvider.Add(shipment);
        await _shipmentProvider.Save();
        return true;
    }

    public async Task DeleteShipment(Shipment shipment){
        _shipmentProvider.Delete(shipment);
        await _shipmentProvider.Save();
    }

    public async Task<bool> ReplaceShipment(Shipment shipment, int shipmentId){
        // check if shipment is valid (like in AddShipment), else return false
        // so, should probably be a seperate method/service to check when a shipment is valid

        string now = shipment.GetTimeStamp();
        shipment.CreatedAt = now;
        shipment.UpdatedAt = now;

        // will return false if there is no shipment with the same id
        if (!_shipmentProvider.Replace(shipment, shipmentId)) return false;
        await _shipmentProvider.Save();

        return true;
    }
}
