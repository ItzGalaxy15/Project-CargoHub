public interface IOrderService
{
    public Task<Order[]> GetOrders();

    public Order? GetOrderById(int id);

    public ItemSmall[] GetOrderItems(Order order);

    public int[] GetOrderIdsRelatedToShipment(int shipmentId);
}
