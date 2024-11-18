using apiV1.Interfaces;

namespace apiV1.Services
{    
    public class ItemService : IItemService
    {
        private readonly IItemProvider _itemProvider;

        public ItemService(IItemProvider itemProvider)
        {
            _itemProvider = itemProvider;
        }

        public async Task AddItem(Item item)
        {
            // date is valid
            string now = item.GetTimeStamp();
            item.UpdatedAt = now;
            item.CreatedAt = now;

            _itemProvider.Add(item);
            await _itemProvider.Save();
        }

        public Item[] GetItems()
        {
            return _itemProvider.Get();
        }

        public Item? GetItemById(string uid)
        {
            Item[] items = _itemProvider.Get();
            return _itemProvider.Get().FirstOrDefault(i => i.Uid == uid);
        }

        public async Task<Dictionary<string, int>> GetItemTotalsByUid(string uid)
        {
            var itemTotaluid = await Task.Run(() => _itemProvider.GetItemTotalsByUid(uid));
            return itemTotaluid;
        }

        public async Task DeleteItem(Item item)
        {
            _itemProvider.Delete(item);
            await _itemProvider.Save();
        }

        public async Task ReplaceItem(Item item)
        {

            string now = item.GetTimeStamp();
            item.UpdatedAt = now;

            _itemProvider.Replace(item);
            await _itemProvider.Save();
        }


        public Item[] GetItemsFromItemLines(int itemLineId)
        {
            Item[] items = _itemProvider.Get()
                            .Where(i => i.ItemLine == itemLineId)
                            .ToArray();
            Console.WriteLine($"Items for ItemLine {itemLineId}: {items.Length}");
            return items;
        }
        

        public Item[] GetItemsFromSupplierId(int supplierId)
        {
            Item[] items = _itemProvider.Get();
            Item[] itemsFromSupplier = items
                                        .Where(item => item.SupplierId == supplierId)
                                        .ToArray();
            return itemsFromSupplier;
        }

        public Item[] GetItemsForItemGroups(int itemGroupId)
        {
            Item[] items = _itemProvider.Get()
                            .Where(i => i.ItemGroup == itemGroupId)
                            .ToArray();
            return items;
        }

    }
}