namespace apiV1.Interfaces
{
    public interface IOrderService
    {
        public Task<Order[]> GetOrders();

        public Order? GetOrderById(int id);

        public ItemSmall[] GetOrderItems(Order order);

        public int[] GetOrderIdsRelatedToShipment(int shipmentId);

        public Task AddOrder(Order order);

        public Task DeleteOrder(Order order);

        public Task ReplaceOrder(Order order, int orderId);

        public Task<bool> UpdateOrdersWithShipmentId(int shipmentId, int[] orderIds);
    }
}