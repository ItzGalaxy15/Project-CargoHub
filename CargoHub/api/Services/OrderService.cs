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

    public int[] GetOrderIdsRelatedToShipment(int shipmentId){
        Order[] orders = _orderProvider.Get();
        int[] orderIds = orders
                            .Where(ord => ord.ShipmentId == shipmentId)
                            .Select(ord => ord.Id)
                            .ToArray();
        return orderIds;
    }
}
