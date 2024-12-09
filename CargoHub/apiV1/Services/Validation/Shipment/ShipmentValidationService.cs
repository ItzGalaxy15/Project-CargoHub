using apiV1.ValidationInterfaces;
using apiV1.Interfaces;

namespace apiV1.Validations
{
    public class ShipmentValidationService : IShipmentValidationService
    {
        private readonly IShipmentProvider _shipmentProvider;
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;
        public ShipmentValidationService(IShipmentProvider shipmentProvider, IOrderService orderService, IItemService itemService)
        {
            _shipmentProvider = shipmentProvider;
            _orderService = orderService;
            _itemService = itemService;
        }

        public bool IsShipmentValid(Shipment? shipment, bool update = false){
            if (shipment is null) return false;

            if (shipment.Id < 0) return false;

            Shipment[] shipments = _shipmentProvider.Get();
            bool shipmentExists = shipments.Any(s => s.Id == shipment.Id);
            if (update){
                // Put
                if (!shipmentExists) return false;
            } else {
                // Post
                if (shipmentExists) return false;
            }

            Order? order = _orderService.GetOrderById(shipment.OrderId);
            if (order is null) return false;
            
            if (shipment.SourceId < 0) return false;

            // Dates
            bool orderDateValid = DateOnly.TryParseExact(shipment.OrderDate, "yyyy-MM-dd", out DateOnly orderDate);;
            if (!orderDateValid) return false;
            bool requestDateValid = DateOnly.TryParseExact(shipment.RequestDate, "yyyy-MM-dd", out DateOnly requestDate);;
            if (!requestDateValid) return false;
            bool shipmentDateValid = DateOnly.TryParseExact(shipment.ShipmentDate, "yyyy-MM-dd", out DateOnly shipmentDate);;
            if (!requestDateValid) return false;

            // // if (requestDate < orderDate) return false;
            // //if (shipmentDate < requestDate) return false;

            HashSet<string> shipmentTypes = new HashSet<string>(){"I", "O"};
            // // if (!shipmentTypes.Contains(shipment.ShipmentType)) return false;

            // Voor nu? Omdat ik denk dat Transit/Delivered shipments niet veranderd moeten kunnen worden
            // // if (shipment.ShipmentStatus != "Scheduled") return false;

            // Deze properties moeten een value hebben
            // if (string.IsNullOrEmpty(shipment.CarrierCode)) return false;
            // if (string.IsNullOrEmpty(shipment.ServiceCode)) return false;

            HashSet<string> paymentTypes = new HashSet<string>(){"Manual", "Automatic"};
            if (!paymentTypes.Contains(shipment.PaymentType)) return false;
            HashSet<string> transferModes = new HashSet<string>(){"Ground", "Sea", "Air"};
            if (!transferModes.Contains(shipment.TransferMode)) return false;

            if (shipment.TotalPackageCount < 0) return false;
            if (shipment.TotalPackageWeight < 0) return false;

            /* Ik weet eerlijk gezegd nogsteeds niet hoe shipments / order werken
            * Want je kan dus items in shipments en orders veranderen, maar die moeten hetzelfde zijn ofzo?
            * En je hebt dan ook weer UpdateOrdersInShipment, waar de items dan denk ik ook aangepast kunnen worden
            * En dan dus ook in PUT en POST requests, dus ik vind het allemaal echt veel te vaag, en de Python code helpt ook echt totaal niet */

            // Checken of Items overeenkomen met order items??
            // Zou in de toekomst ook waarschijnlijk optimized moeten worden
            // foreach (ItemSmall item in shipment.Items)
            // {
            //     if (item.Amount < 0) return false;
            //     if (_itemService.GetItemById(item.ItemId) is null) return false;
            // }

            return true;
        }
    }
}
