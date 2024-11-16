namespace apiV2.ValidationInterfaces
{    
    public interface IItemGroupValidationService
    {
        public bool IsItemGroupValid(ItemGroup? itemGroup, bool update = false);
        public bool IsItemGroupValidForPatch(Dictionary<string, dynamic> patch);
    }
}