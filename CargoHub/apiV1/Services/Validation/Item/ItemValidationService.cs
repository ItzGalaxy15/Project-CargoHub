public class ItemValidationService : IItemValidationService
{

    private readonly IItemProvider _itemProvider;
    public ItemValidationService(IItemProvider itemProvider)
    {
        _itemProvider = itemProvider;
    }

    public bool IsItemValid(Item? item, bool update = false)
    {
        if (item is null) return false;

        Item[] items = _itemProvider.Get();
        bool itemExists = items.Any(i => i.Uid == item.Uid);
        if (update)
        {
            // Put 
            if (!itemExists) return false; 
        }
        else
        {
            // Post
            if (itemExists) return false;
        }

        // if (string.IsNullOrWhiteSpace(item.UpcCode)) return false;
        // if (string.IsNullOrWhiteSpace(item.ModelNumber)) return false;
        // if (string.IsNullOrWhiteSpace(item.CommodityCode)) return false;
        if (item.ItemLine <= 0) return false;
        if (item.ItemGroup <= 0) return false;
        if (item.ItemType <= 0) return false;
        if (item.UnitPurchaseQuantity < 0) return false;
        if (item.UnitOrderQuantity < 0) return false;
        if (item.PackOrderQuantity < 0) return false;
        if (item.SupplierId <= 0) return false;
        // if (string.IsNullOrWhiteSpace(item.SupplierCode)) return false;
        // if (string.IsNullOrWhiteSpace(item.SupplierPartNumber)) return false;

        return true;
    }   

}