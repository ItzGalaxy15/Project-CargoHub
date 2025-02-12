using System.Text.Json;
using apiV2.ValidationInterfaces;
using apiV2.Interfaces;

namespace apiV2.Validations
{
    public class ShipmentValidationService : IShipmentValidationService
    {
        private readonly IShipmentProvider shipmentProvider;
        private readonly IOrderService orderService;
        private readonly IItemService itemService;

        public ShipmentValidationService(IShipmentProvider shipmentProvider, IOrderService orderService, IItemService itemService)
        {
            this.shipmentProvider = shipmentProvider;
            this.orderService = orderService;
            this.itemService = itemService;
        }

        public bool isItemSmallValid(ItemSmall[] item)
        {
            foreach (var i in item)
            {
                if (i == null)
                {
                    return false;
                }

                if (i.Amount < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsShipmentValid(Shipment? shipment, bool update = false)
        {
            if (shipment is null)
            {
                return false;
            }

            if (shipment.Id < 0)
            {
                return false;
            }

            Shipment[] shipments = this.shipmentProvider.Get();
            bool shipmentExists = shipments.Any(s => s.Id == shipment.Id);
            if (update)
            {
                // Put
                if (!shipmentExists)
                {
                    return false;
                }
            }
            else
            {
                // Post
                if (shipmentExists)
                {
                    return false;
                }
            }

            Order? order = this.orderService.GetOrderById(shipment.OrderId);
            if (order is null)
            {
                return false;
            }

            if (shipment.SourceId < 0)
            {
                return false;
            }

            // Dates
            bool orderDateValid = DateOnly.TryParseExact(shipment.OrderDate, "yyyy-MM-dd", out DateOnly orderDate);
            if (!orderDateValid)
            {
                return false;
            }

            bool requestDateValid = DateOnly.TryParseExact(shipment.RequestDate, "yyyy-MM-dd", out DateOnly requestDate);
            if (!requestDateValid)
            {
                return false;
            }

            bool shipmentDateValid = DateOnly.TryParseExact(shipment.ShipmentDate, "yyyy-MM-dd", out DateOnly shipmentDate);
            if (!requestDateValid)
            {
                return false;
            }

            // //if (requestDate < orderDate) return false;
            // //if (shipmentDate < requestDate) return false;
            HashSet<string> shipmentTypes = new HashSet<string>() { "I", "O" };
            HashSet<string> paymentTypes = new HashSet<string>() { "Manual", "Automatic" };
            if (!paymentTypes.Contains(shipment.PaymentType))
            {
                return false;
            }

            HashSet<string> transferModes = new HashSet<string>() { "Ground", "Sea", "Air" };
            if (!transferModes.Contains(shipment.TransferMode))
            {
                return false;
            }

            if (shipment.TotalPackageCount < 0)
            {
                return false;
            }

            if (shipment.TotalPackageWeight < 0)
            {
                return false;
            }

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

        public async Task<bool> IsShipmentValidForPATCH(Dictionary<string, dynamic> patch, int shipmentId)
        {
            List<string> listsOfStatuses = new List<string> { "Pending", "Transit", "Delivered" };
            if (patch == null)
            {
                return false;
            }

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "order_id", JsonValueKind.Number },
                { "source_id", JsonValueKind.Number },
                { "total_package_count", JsonValueKind.Number },
                { "total_package_weight", JsonValueKind.Number },
                { "order_date", JsonValueKind.String },
                { "request_date", JsonValueKind.String },
                { "shipment_date", JsonValueKind.String },
                { "shipment_type", JsonValueKind.String },
                { "shipment_status", JsonValueKind.String },
                { "notes", JsonValueKind.String },
                { "carrier_code", JsonValueKind.String },
                { "carrier_description", JsonValueKind.String },
                { "service_code", JsonValueKind.String },
                { "payment_type", JsonValueKind.String },
                { "transfer_mode", JsonValueKind.String },
                { "items", JsonValueKind.Array },
            };
            Shipment[] shipments = this.shipmentProvider.Get();
            Shipment? shipment = await Task.FromResult(shipments.FirstOrDefault(l => l.Id == shipmentId));

            if (shipment == null)
            {
                return false;
            }

            if (patch.ContainsKey("shipment_status"))
            {
                var shipmentStatusElement = patch["shipment_status"];
                string? shipmentStatus = shipmentStatusElement is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.String
                        ? jsonElement.GetString()
                        : shipmentStatusElement as string;
                if (shipmentStatus == null || !listsOfStatuses.Contains(shipmentStatus))
                {
                    return false;
                }
            }

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

                        // remove key if not valid type
                    }
                    else
                    {
                        validKeysInPatch.Add(key);
                    }
                }
            }

            if (!validKeysInPatch.Any())
            {
                return false;
            }

            return true;
        }

        public bool IsShipmentCommitValid(Shipment shipment)
        {
            if (shipment is null)
            {
                return false;
            }

            if (shipment.ShipmentStatus == "Delivered")
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsShipmentItemValid(ItemSmall patchItem, int id)
        {
            Shipment[] shipments = this.shipmentProvider.Get();
            Shipment? shipment = await Task.FromResult(shipments.FirstOrDefault(l => l.Id == id));

            if (shipment is null)
            {
                return false;
            }

            if (patchItem is null)
            {
                return false;
            }

            if (patchItem.Amount < 0)
            {
                return false;
            }

            var existingItem = shipment.Items.FirstOrDefault(item => item.ItemId == patchItem.ItemId);
            if (existingItem is null)
            {
                return false;
            }

            if (existingItem.ItemId != patchItem.ItemId)
            {
                return false;
            }

            return true;
        }
    }
}
