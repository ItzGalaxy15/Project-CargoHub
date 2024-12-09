namespace apiV2.Interfaces
{
    public interface IOrderService
    {
        public Task<Order[]> GetOrders();
        public Order? GetOrderById(int id);
        public ItemSmall[] GetOrderItems(Order order);
        public int[] GetOrderIdsRelatedToShipment(int shipmentId);
        public Task AddOrder(Order order);
        public Task DeleteOrder(Order order);
        public Task UpdateOrder(Order order, int orderId);
        public Task<bool> UpdateOrdersWithShipmentId(int shipmentId, int[] orderIds);
        public Task PatchOrder(int id, Dictionary<string, dynamic> patch, Order order);

        public Task UpdateItemsInOrder(Order? order, ItemSmall[] items, int id);
    }
}
