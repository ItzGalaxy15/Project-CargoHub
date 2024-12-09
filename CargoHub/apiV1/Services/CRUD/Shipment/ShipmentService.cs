using apiV1.Interfaces;

namespace apiV1.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentProvider shipmentProvider;

        public ShipmentService(IShipmentProvider shipmentProvider)
        {
            this.shipmentProvider = shipmentProvider;
        }

        public Shipment[] GetShipments()
        {
            return this.shipmentProvider.Get();
        }

        public Shipment? GetShipmentById(int id)
        {
            Shipment[] shipments = this.shipmentProvider.Get();
            Shipment? shipment = shipments.FirstOrDefault(ship => ship.Id == id);
            return shipment;
        }

        public ItemSmall[] GetShipmentItems(Shipment shipment)
        {
            return shipment.Items.ToArray();
        }

        public async Task AddShipment(Shipment shipment)
        {
            string now = shipment.GetTimeStamp();
            shipment.CreatedAt = now;
            shipment.UpdatedAt = now;
            this.shipmentProvider.Add(shipment);
            await this.shipmentProvider.Save();
        }

        public async Task DeleteShipment(Shipment shipment)
        {
            this.shipmentProvider.Delete(shipment);
            await this.shipmentProvider.Save();
        }

        public async Task ReplaceShipment(Shipment shipment, int shipmentId)
        {
            string now = shipment.GetTimeStamp();
            shipment.UpdatedAt = now;
            this.shipmentProvider.Update(shipment, shipmentId);
            await this.shipmentProvider.Save();
        }
    }
}
