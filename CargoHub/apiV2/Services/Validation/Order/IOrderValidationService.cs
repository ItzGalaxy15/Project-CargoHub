namespace apiV2.ValidationInterfaces
{
    public interface IOrderValidationService
    {
        public bool IsOrderValid(Order order, bool update = false);

        public bool IsOrderValidForPATCH(Dictionary<string, dynamic> patch, int orderId);
    }
}