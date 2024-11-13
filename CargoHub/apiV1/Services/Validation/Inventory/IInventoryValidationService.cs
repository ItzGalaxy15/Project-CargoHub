public interface IInventoryValidationService
{
    public bool IsInventoryValid(Inventory? inventory, bool update = false);
}
