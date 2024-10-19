public interface IShipmentService
{
    public Shipment[] GetShipments();

    public Shipment? GetShipmentById(int id);

    public ItemSmall[] GetShipmentItems(Shipment shipment);
}
