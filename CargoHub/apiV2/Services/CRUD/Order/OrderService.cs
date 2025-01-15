using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderProvider orderProvider;
        private readonly IInventoryService inventoryService;
        private readonly IShipmentService shipmentService;
        private readonly IShipmentProvider shipmentProvider;

        public OrderService(IOrderProvider orderProvider, IInventoryService inventoryService, IShipmentService shipmentService, IShipmentProvider shipmentProvider)
        {
            this.orderProvider = orderProvider;
            this.inventoryService = inventoryService;
            this.shipmentService = shipmentService;
            this.shipmentProvider = shipmentProvider;
        }

        public async Task<Order[]> GetOrders()
        {
            var order = await Task.Run(() => this.orderProvider.Get());
            return order;
        }

        public Order? GetOrderById(int id)
        {
            Order[] orders = this.orderProvider.Get();
            Order? order = orders.FirstOrDefault(ord => ord.Id == id);
            return order;
        }

        public ItemSmall[] GetOrderItems(Order order)
        {
            return order.Items.ToArray();
        }

        public int[] GetOrderIdsRelatedToShipment(int shipmentId)
        {
            Order[] orders = this.orderProvider.Get();
            int[] orderIds = orders
                                .Where(ord => ord.ShipmentId == shipmentId)
                                .Select(ord => ord.Id)
                                .ToArray();
            return orderIds;
        }

        public async Task AddOrder(Order order)
        {
            string now = order.GetTimeStamp();
            order.CreatedAt = now;
            order.UpdatedAt = now;

            foreach (var item in order.Items)
            {
                var inventory = await this.inventoryService.GetInventoryByItemId(item.ItemId);
                if (inventory != null)
                {
                    inventory.TotalOnHand = inventory.TotalExpected + inventory.TotalOrdered + inventory.TotalAllocated + inventory.TotalAvailable;
                    inventory.UpdatedAt = now;

                    var patch = new Dictionary<string, dynamic>
                    {
                        { "total_expected", inventory.TotalExpected },
                        { "total_on_hand", inventory.TotalOnHand },
                    };
                    await this.inventoryService.ModifyInventory(inventory.Id, patch, inventory);
                }
                else
                {
                    throw new Exception($"Inventory empty for item {item.ItemId}");
                }
            }

            if (order.ShipmentId.HasValue)
            {
                Shipment? shipment = this.shipmentService.GetShipmentById(order.ShipmentId.Value);
                if (shipment == null)
                {
                    throw new Exception("Shipment not found");
                }

                shipment.Items.AddRange(order.Items);
                shipment.UpdatedAt = now;
                this.shipmentProvider.Update(shipment, shipment.Id);
                await this.shipmentProvider.Save();
            }

            this.orderProvider.Add(order);
            await this.orderProvider.Save();
        }

        public async Task DeleteOrder(Order order)
        {
            this.orderProvider.Delete(order);
            await this.orderProvider.Save();
        }

        public async Task UpdateOrder(Order order, int orderId)
        {
            string now = order.GetTimeStamp();
            order.UpdatedAt = now;

            this.orderProvider.Update(order, orderId);
            await this.orderProvider.Save();
        }

        public async Task<bool> UpdateOrdersWithShipmentId(int shipmentId, int[] orderIds)
        {
            HashSet<int> orderIdsSet = new(orderIds);
            Order[] orders = this.orderProvider.Get();
            foreach (Order order in orders)
            {
                if (orderIdsSet.Contains(order.Id))
                {
                    order.ShipmentId = shipmentId;
                    order.OrderStatus = "Packed";
                    order.UpdatedAt = order.GetTimeStamp();
                }
                else if (order.ShipmentId == shipmentId)
                {
                    order.ShipmentId = -1;
                    order.OrderStatus = "Scheduled";
                    order.UpdatedAt = order.GetTimeStamp();
                }
            }

            await this.orderProvider.Save();
            return true;
        }

        public async Task PatchOrder(int id, Dictionary<string, dynamic> patch, Order order)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "order_status":
                            order.OrderStatus = jsonElement.GetString()!;
                            break;
                        case "source_id":
                            order.SourceId = jsonElement.GetInt32()!;
                            break;
                        case "order_date":
                            order.OrderDate = jsonElement.GetString()!;
                            break;
                        case "request_date":
                            order.RequestDate = jsonElement.GetString()!;
                            break;
                        case "reference":
                            order.Reference = jsonElement.GetString()!;
                            break;
                        case "warehouse_id":
                            order.WarehouseId = jsonElement.GetInt32()!;
                            break;
                        case "ship_to":
                            order.ShipTo = jsonElement.GetInt32()!;
                            break;
                        case "bill_to":
                            order.BillTo = jsonElement.GetInt32()!;
                            break;
                        case "shipment_id":
                            order.ShipmentId = jsonElement.GetInt32()!;
                            break;
                        case "total_amount":
                            order.TotalAmount = jsonElement.GetDouble()!;
                            break;
                        case "total_discount":
                            order.TotalDiscount = jsonElement.GetDouble()!;
                            break;
                        case "total_tax":
                            order.TotalTax = jsonElement.GetDouble()!;
                            break;
                        case "total_surcharge":
                            order.TotalSurcharge = jsonElement.GetDouble()!;
                            break;
                        case "items":
                            List<ItemSmall> items = new List<ItemSmall>();
                            foreach (var item in jsonElement.EnumerateArray())
                            {
                                items.Add(new ItemSmall
                                {
                                    ItemId = item.GetProperty("item_id").GetString()!,
                                    Amount = item.GetProperty("amount").GetInt32(),
                                });
                            }
                            order.Items = items;
                            break;
                    }
                }
            }

            order.UpdatedAt = order.GetTimeStamp();
            this.orderProvider.Update(order, id);
            await this.orderProvider.Save();
        }

        public async Task UpdateItemsInOrder(Order? order, ItemSmall[] items, int orderId)
        {
            order!.Items.AddRange(items);
            order!.UpdatedAt = order!.GetTimeStamp();
            this.orderProvider.Update(order!, orderId);
            await this.orderProvider.Save();
        }

        public async Task UpdateItemInOrderAndShipment(int orderId, ItemSmall updatedItem)
        {
            // Update item in order
            Order? order = this.GetOrderById(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            var existingItem = order.Items.FirstOrDefault(i => i.ItemId == updatedItem.ItemId);
            if (existingItem != null)
            {
                existingItem.Amount = updatedItem.Amount;
                order.UpdatedAt = order.GetTimeStamp();
                this.orderProvider.Update(order, orderId);
                await this.orderProvider.Save();
            }

            // Update item in shipment
            if (order.ShipmentId.HasValue)
            {
                Shipment? shipment = this.shipmentService.GetShipmentById(order.ShipmentId.Value);
                if (shipment == null)
                {
                    throw new Exception("Shipment not found");
                }

                await this.shipmentService.PatchItemInShipment(shipment, updatedItem);
            }
            else
            {
                throw new Exception("Order does not have a shipment associated with it");
            }
        }
    }
}