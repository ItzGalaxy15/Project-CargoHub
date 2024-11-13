public class ItemGroupValidationService : IItemGroupValidationService 
{
    private readonly IItemGroupProvider _itemGroupProvider;
    public ItemGroupValidationService(IItemGroupProvider itemGroupProvider)
    {
        _itemGroupProvider = itemGroupProvider;
    }

    public bool IsItemGroupValid(ItemGroup? itemGroup, bool update = false)
    {
        if (itemGroup is null) return false;
        if (itemGroup.Id < 0) return false;


        ItemGroup[] itemGroups = _itemGroupProvider.Get();
        bool itemGroupExists = itemGroups.Any(i => i.Id == itemGroup.Id);
        if (update){
            // Put
            if (!itemGroupExists) return false;
        } else {
            // Post
            if (itemGroupExists) return false;
        }

        // if (string.IsNullOrWhiteSpace(itemGroup.Name)) return false;
        // Optional description check
        // if (!string.IsNullOrWhiteSpace(itemGroup.Description)) return false;

        return true;
    }
}