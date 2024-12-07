using Microsoft.AspNetCore.Razor.TagHelpers;
using apiV1.Interfaces;

namespace apiV1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderProvider orderProvider;

        public OrderService(IOrderProvider orderProvider)
        {
            this.orderProvider = orderProvider;
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
            this.orderProvider.Add(order);
            await this.orderProvider.Save();
        }

        public async Task DeleteOrder(Order order)
        {
            this.orderProvider.Delete(order);
            await this.orderProvider.Save();
        }

        public async Task ReplaceOrder(Order order, int orderId)
        {
            string now = order.GetTimeStamp();
            order.UpdatedAt = now;

            this.orderProvider.Update(order, orderId);
            await this.orderProvider.Save();
        }

        public async Task<bool> UpdateOrdersWithShipmentId(int shipmentId, int[] orderIds)
        {
            // Maybe check if all orderIds are valid, instead of ignoring the wrong ones
            // -> return false not implemented yet
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
    }
}
