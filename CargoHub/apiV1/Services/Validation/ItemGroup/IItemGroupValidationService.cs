namespace apiV1.ValidationInterfaces
{    
    public interface IItemGroupValidationService
    {
        public bool IsItemGroupValid(ItemGroup? itemGroup, bool update = false);
    }
}