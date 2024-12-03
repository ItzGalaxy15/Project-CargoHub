using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
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
            shipment.UpdatedAt = now;

            _shipmentProvider.Update(shipment, shipmentId);
            await _shipmentProvider.Save();
        }

        public async Task PatchShipment(int id, Dictionary<string, dynamic> patch, Shipment shipment){
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "order_id":
                            shipment.OrderId = jsonElement.GetInt32();
                            break;
                        case "source_id":
                            shipment.SourceId = jsonElement.GetInt32();
                            break;
                        case "order_date":
                            shipment.OrderDate = jsonElement.GetString()!;
                            break;
                        case "request_date":
                            shipment.RequestDate = jsonElement.GetString()!;
                            break;
                        case "shipment_date":
                            shipment.ShipmentDate = jsonElement.GetString()!;
                            break;
                        case "shipment_type":
                            shipment.ShipmentType = jsonElement.GetString()!;
                            break;
                        case "shipment_status":
                            shipment.ShipmentStatus = jsonElement.GetString()!;
                            break;
                        case "notes":
                            shipment.Notes = jsonElement.GetString()!;
                            break;
                        case "carrier_code":
                            shipment.CarrierCode = jsonElement.GetString()!;
                            break;
                        case "carrier_description":
                            shipment.CarrierDescription = jsonElement.GetString()!;
                            break;
                        case "service_code":
                            shipment.ServiceCode = jsonElement.GetString()!;
                            break;
                        case "payment_type":
                            shipment.PaymentType = jsonElement.GetString()!;
                            break;
                        case "transfer_mode":
                            shipment.TransferMode = jsonElement.GetString()!;
                            break;
                        case "total_package_count":
                            shipment.TotalPackageCount = jsonElement.GetInt32();
                            break;
                        case "total_package_weight":
                            shipment.TotalPackageWeight = jsonElement.GetDouble();
                            break;
                        case "items":
                            List<ItemSmall> items = new List<ItemSmall>();
                            foreach (var item in jsonElement.EnumerateArray())
                            {
                                items.Add(new ItemSmall
                                {
                                    ItemId = item.GetProperty("item_id").GetString()!,
                                    Amount = item.GetProperty("amount").GetInt32()
                                });
                            }
                            shipment.Items = items;
                            break;
                    }
                }
            }
            shipment.UpdatedAt = shipment.GetTimeStamp();
            _shipmentProvider.Update(shipment, id);
            await _shipmentProvider.Save();
        }

        public async Task UpdateItemsInShipment(Shipment? shipment, ItemSmall[] items, int id){
            shipment!.Items.AddRange(items);
            shipment.UpdatedAt = shipment.GetTimeStamp();
            _shipmentProvider.Update(shipment, id);
            await _shipmentProvider.Save();
        }
    }
}