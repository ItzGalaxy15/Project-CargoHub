namespace apiV1.ValidationInterfaces
{
    public interface IInventoryValidationService
    {
        public bool IsInventoryValid(Inventory? inventory, bool update = false);
    }
}