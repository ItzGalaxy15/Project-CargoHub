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

    public async Task<bool> AddOrder(Order order){
        // Check if order is valid
        /*
         * all items exist
         * warehouse exists
         * date is valid
         * ship_to and bill_to are valid
         etc.
        */

        // Check if order id is already in use
        Order[] orders = _orderProvider.Get();
        if (orders.Any(ord => ord.Id == order.Id)) return false;

        string now = order.GetTimeStamp();
        order.CreatedAt = now;
        order.UpdatedAt = now;
        _orderProvider.Add(order);
        await _orderProvider.Save();
        return true;
    }

    public async Task DeleteOrder(Order order){
        _orderProvider.Delete(order);
        await _orderProvider.Save();
    }
}
