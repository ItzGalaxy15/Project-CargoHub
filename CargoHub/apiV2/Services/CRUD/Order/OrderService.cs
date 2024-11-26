using apiV2.Interfaces;
using System.Text.Json;

namespace apiV2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderProvider _orderProvider;
        public OrderService(IOrderProvider orderProvider){
            _orderProvider = orderProvider;
        }

        public async Task<Order[]> GetOrders(){
            var order = await Task.Run(() => _orderProvider.Get());
            return order;
        }

        public Order? GetOrderById(int id){
            Order[] orders = _orderProvider.Get();
            Order? order = orders.FirstOrDefault(ord => ord.Id == id);
            return order;
        }

        public ItemSmall[] GetOrderItems(Order order){
            return order.Items.ToArray();
        }

        public int[] GetOrderIdsRelatedToShipment(int shipmentId){
            Order[] orders = _orderProvider.Get();
            int[] orderIds = orders
                                .Where(ord => ord.ShipmentId == shipmentId)
                                .Select(ord => ord.Id)
                                .ToArray();
            return orderIds;
        }

        public async Task AddOrder(Order order){
            string now = order.GetTimeStamp();
            order.CreatedAt = now;
            order.UpdatedAt = now;
            _orderProvider.Add(order);
            await _orderProvider.Save();
        }

        public async Task DeleteOrder(Order order){
            _orderProvider.Delete(order);
            await _orderProvider.Save();
        }

        public async Task UpdateOrder(Order order, int orderId){
            string now = order.GetTimeStamp();
            order.UpdatedAt = now;

            _orderProvider.Update(order, orderId);
            await _orderProvider.Save();
        }

        public async Task<bool> UpdateOrdersWithShipmentId(int shipmentId, int[] orderIds){
            // Maybe check if all orderIds are valid, instead of ignoring the wrong ones
            // -> return false not implemented yet

            HashSet<int> orderIdsSet = new(orderIds);
            Order[] orders = _orderProvider.Get();
            foreach (Order order in orders){
                if (orderIdsSet.Contains(order.Id)){
                    order.ShipmentId = shipmentId;
                    order.OrderStatus = "Packed";
                    order.UpdatedAt = order.GetTimeStamp();
                } else if (order.ShipmentId == shipmentId){
                    order.ShipmentId = -1;
                    order.OrderStatus = "Scheduled";
                    order.UpdatedAt = order.GetTimeStamp();
                }
            }

            await _orderProvider.Save();
            return true;
        }


        public async Task PatchOrder( int id, Dictionary<string, dynamic> patch, Order order){
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
                            order.Items = jsonElement.EnumerateArray()
                                            .Select(item => new ItemSmall{
                                                ItemId = item.GetProperty("item_id").GetString()!,
                                                Amount = item.GetProperty("amount").GetInt32()
                                            })
                                            .ToList();
                            break;
                    }
                }
            }
            order.UpdatedAt = order.GetTimeStamp();
            _orderProvider.Update(order, id);
            await _orderProvider.Save();
        }
    }
}
