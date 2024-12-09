public interface IOrderProvider
{
    public List<Order> context { get; set; }

    public string? path { get; set; }

    public Task Save();

    public Order[] Get();

    public void Add(Order order);

    public void Delete(Order order);

    public void Update(Order order, int orderId);
}
