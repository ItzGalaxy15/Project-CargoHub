public class OrderService : IOrderService
{
    private readonly IOrderProvider _orderProvider;
    public OrderService(IOrderProvider orderProvider){
        _orderProvider = orderProvider;
    }

    public async Task<Order[]> GetOrders(){
        return _orderProvider.Get();
    }
}
