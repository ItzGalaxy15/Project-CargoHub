public interface IShipmentService
{
    public Shipment[] GetShipments();
    public Shipment? GetShipmentById(int id);
    public ItemSmall[] GetShipmentItems(Shipment shipment);
    public Task AddShipment(Shipment shipment);
    public Task DeleteShipment(Shipment shipment);
    public Task ReplaceShipment(Shipment shipment, int shipmentId);
}
