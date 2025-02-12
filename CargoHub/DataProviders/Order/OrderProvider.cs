public class OrderProvider : BaseProvider<Order>, IOrderProvider
{
    public OrderProvider(List<Order> mockData)
        : base(mockData)
    {
    }

    public OrderProvider()
        : base("data/orders.json")
    {
    }

    public Order[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Order order)
    {
        this.context.Add(order);
    }

    public void Delete(Order order)
    {
        order.IsDeleted = true;
        order.UpdatedAt = order.GetTimeStamp();
    }

    public void Update(Order order, int orderId)
    {
        order.Id = orderId;
        int index = this.context.FindIndex(ord => ord.Id == orderId);
        this.context[index] = order;
    }
}
