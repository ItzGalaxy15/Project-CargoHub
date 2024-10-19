public class OrderProvider : BaseProvider<Order>, IOrderProvider
{
    public OrderProvider() : base("test_data/orders.json") {}

    public Order[] Get(){
        return context.ToArray();
    }
}
