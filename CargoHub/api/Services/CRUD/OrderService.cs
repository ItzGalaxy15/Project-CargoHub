using Microsoft.AspNetCore.Razor.TagHelpers;

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

    public async Task ReplaceOrder(Order order, int orderId){
        string now = order.GetTimeStamp();
        order.CreatedAt = now;
        order.UpdatedAt = now;

        _orderProvider.Replace(order, orderId);
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
}
