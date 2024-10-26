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

        if (order.Id < 1) return false;

        Order[] orders = _orderProvider.Get();
        bool orderExists = orders.Any(o => o.Id == order.Id);

        if (update){
            // Put
            if (!orderExists) return false;
        } else {
            // Post
            if (orderExists) return false;
        }

        if (order.SourceId < 1) return false;

        // Voor nu? Omdat ik denk dat Packed/Delivered orders niet veranderd moeten kunnen worden
        if (order.OrderStatus != "Scheduled") return false;

        // Dates
        bool orderDateValid = DateTime.TryParse(order.OrderDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime orderDate);
        if (!orderDateValid) return false;
        bool requestDateValid = DateTime.TryParse(order.RequestDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime requestDate);
        if (!requestDateValid) return false;
        if (requestDate < orderDate) return false;

        if (string.IsNullOrWhiteSpace(order.Reference)) return false;

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
}
