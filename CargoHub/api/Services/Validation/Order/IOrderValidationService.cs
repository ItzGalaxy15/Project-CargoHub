public interface IOrderValidationService
{
    public bool IsOrderValid(Order order, bool update = false);
}
