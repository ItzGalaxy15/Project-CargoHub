public class OrderProvider : BaseProvider<Order>, IOrderProvider
{
    public OrderProvider() : base("test_data/orders.json") {}

    public Order[] Get(){
        return context.ToArray();
    }

    public void Add(Order order){
        context.Add(order);
    }

    public void Delete(Order order){
        context.Remove(order);
    }

    public bool Replace(Order order, int orderId){
        int index = context.FindIndex(ord => ord.Id == orderId);
        if (index == -1) return false;
        context[index] = order;
        return true;
    }
}
