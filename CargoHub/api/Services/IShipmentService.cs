public interface IShipmentService
{
    public Shipment[] GetShipments();
    public Shipment? GetShipmentById(int id);
    public ItemSmall[] GetShipmentItems(Shipment shipment);
    public Task<bool> AddShipment(Shipment shipment);
    public Task DeleteShipment(Shipment shipment);
}
