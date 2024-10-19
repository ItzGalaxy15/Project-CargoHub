public interface IOrderService
{
    public Task<Order[]> GetOrders();

    public Order? GetOrderById(int id);

    public ItemSmall[] GetOrderItems(Order order);
}
