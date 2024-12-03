namespace apiV2.Interfaces
{
    public interface IShipmentService
    {
        public Shipment[] GetShipments();
        public Shipment? GetShipmentById(int id);
        public ItemSmall[] GetShipmentItems(Shipment shipment);
        public Task AddShipment(Shipment shipment);
        public Task DeleteShipment(Shipment shipment);
        public Task ReplaceShipment(Shipment shipment, int shipmentId);
        public Task PatchShipment(int id, Dictionary<string, dynamic> patch, Shipment shipment);
        public Task UpdateItemsInShipment(Shipment? shipment, ItemSmall[] items, int id);
    }
}