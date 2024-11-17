using System.Text.Json;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class OrderValidationService : IOrderValidationService 
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IWarehouseService _warehouseService;
        private readonly IClientService _clientService;
        private readonly IShipmentService _shipmentService;
        private readonly IItemService _itemService;

        public OrderValidationService(IOrderProvider orderProvider, IWarehouseService warehouseService, 
                                    IClientService clientService, IShipmentService shipmentService,
                                    IItemService itemService){
            _orderProvider = orderProvider;
            _warehouseService = warehouseService;
            _clientService = clientService;
            _shipmentService = shipmentService;
            _itemService = itemService;
        }

        public bool IsOrderValid(Order? order, bool update = false)
        {
            if (order is null) return false;

            if (order.Id < 0) return false;

            Order[] orders = _orderProvider.Get();
            bool orderExists = orders.Any(o => o.Id == order.Id);

            if (update){
                // Put
                if (!orderExists) return false;
            } else {
                // Post
                if (orderExists) return false;
            }

            if (order.SourceId < 0) return false;

            // Voor nu? Omdat ik denk dat Packed/Delivered orders niet veranderd moeten kunnen worden
            if (order.OrderStatus != "Scheduled") return false;

            // Dates
            bool orderDateValid = DateTime.TryParse(order.OrderDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime orderDate);
            if (!orderDateValid) return false;
            bool requestDateValid = DateTime.TryParse(order.RequestDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime requestDate);
            if (!requestDateValid) return false;
            if (requestDate < orderDate) return false;

            // if (string.IsNullOrWhiteSpace(order.Reference)) return false;

            if (_warehouseService.GetWarehouseById(order.WarehouseId) is null) return false;

            // Zou in de toekomst misschien hier gedaan worden in een single iteration (voor optimization)
            if (order.ShipTo != null && (_clientService.GetClientById((int)order.ShipTo) is null)) return false;
            if (order.BillTo != null && (_clientService.GetClientById((int)order.BillTo) is null)) return false;

            if (order.ShipmentId != null) {
                Shipment? shipment = _shipmentService.GetShipmentById((int)order.ShipmentId);
                if (shipment is null) return false;
                if (shipment.OrderId != order.Id) return false;
            }

            if (order.TotalAmount < 0) return false;
            if (order.TotalDiscount < 0) return false;
            if (order.TotalTax < 0) return false;
            if (order.TotalSurcharge < 0) return false;

            // Zou in de toekomst ook waarschijnlijk optimized moeten worden
            foreach (ItemSmall item in order.Items)
            {
                if (item.Amount < 0) return false;
                if (_itemService.GetItemById(item.ItemId) is null) return false;
            }

            return true;
        }

        public async Task<bool> IsOrderValidForPATCH(Dictionary<string, dynamic> patch, int orderId)
        {
            if (patch is null || !patch.Any()) return false;
            
            var validProperties = new Dictionary<string, JsonValueKind> {
                {"order_status", JsonValueKind.String},
                {"source_id", JsonValueKind.Number},
                {"order_date", JsonValueKind.String},
                {"request_date", JsonValueKind.String},
                {"reference", JsonValueKind.String},
                {"warehouse_id", JsonValueKind.Number},
                {"ship_to", JsonValueKind.Number},
                {"bill_to", JsonValueKind.Number},
                {"shipment_id", JsonValueKind.Number},
                {"total_amount", JsonValueKind.Number},
                {"total_discount", JsonValueKind.Number},
                {"total_tax", JsonValueKind.Number},
                {"total_surcharge", JsonValueKind.Number},
                {"items", JsonValueKind.Array},
            };

            Order[] orders = _orderProvider.Get();
            Order? order = await Task.FromResult(orders.FirstOrDefault(o => o.Id == orderId));

            if (order is null) return false;

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.ContainsKey(key))
                {
                    var expectedType = validProperties[key];
                    JsonElement value = patch[key];
                    if (value.ValueKind != expectedType)
                    {
                        patch.Remove(key);
                        //remove key if not valid type
                    }
                    else
                    {
                        if (key == "items")
                        {
                            foreach (var item in value.EnumerateArray())
                            {
                                if (item.GetProperty("item_id").ValueKind != JsonValueKind.String)
                                {
                                    return false;
                                }
                            }
                        }
                        validKeysInPatch.Add(key);
                    }
                }
            }

            if (!validKeysInPatch.Any()) return false;
            return true;

        }

    }
}
