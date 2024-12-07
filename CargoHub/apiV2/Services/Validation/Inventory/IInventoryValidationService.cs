namespace apiV2.ValidationInterfaces
{
    public interface IInventoryValidationService
    {
        public bool IsInventoryValid(Inventory? inventory, bool update = false);

        public bool IsInventoryValidForPatch(Dictionary<string, dynamic> patch);
    }
}