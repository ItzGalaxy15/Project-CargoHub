public class OrderService : IOrderService
{
    private readonly IOrderProvider _orderProvider;
    public OrderService(IOrderProvider orderProvider){
        _orderProvider = orderProvider;
    }

    public async Task<Order[]> GetOrders(){
        return _orderProvider.Get();
    }

    public Order? GetOrderById(int id){
        Order[] orders = _orderProvider.Get();
        Order? order = orders.FirstOrDefault(ord => ord.Id == id);
        return order;
    }

    public ItemSmall[] GetOrderItems(Order order){
        return order.Items.ToArray();
    }
}
