namespace apiV1.ValidationInterfaces
{
    public interface IItemValidationService
    {
        public bool IsItemValid(Item? item, bool update = false);
    }
}