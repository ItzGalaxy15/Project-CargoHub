public class OrderProvider : BaseProvider<Order>, IOrderProvider
{
    public OrderProvider() : base("data/orders.json") {}
}
