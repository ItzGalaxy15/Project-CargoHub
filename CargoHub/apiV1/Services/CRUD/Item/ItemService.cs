using apiV1.Interfaces;

namespace apiV1.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemProvider itemProvider;

        public ItemService(IItemProvider itemProvider)
        {
            this.itemProvider = itemProvider;
        }

        public async Task AddItem(Item item)
        {
            // date is valid
            string now = item.GetTimeStamp();
            item.UpdatedAt = now;
            item.CreatedAt = now;

            this.itemProvider.Add(item);
            await this.itemProvider.Save();
        }

        public Item[] GetItems()
        {
            return this.itemProvider.Get();
        }

        public Item? GetItemById(string uid)
        {
            Item[] items = this.itemProvider.Get();
            return this.itemProvider.Get().FirstOrDefault(i => i.Uid == uid);
        }

        public async Task<Dictionary<string, int>> GetItemTotalsByUid(string uid)
        {
            var itemTotaluid = await Task.Run(() => this.itemProvider.GetItemTotalsByUid(uid));
            return itemTotaluid;
        }

        public async Task DeleteItem(Item item)
        {
            this.itemProvider.Delete(item);
            await this.itemProvider.Save();
        }

        public async Task UpdateItem(Item item, string uid)
        {
            string now = item.GetTimeStamp();
            item.UpdatedAt = now;

            this.itemProvider.Update(item, uid);
            await this.itemProvider.Save();
        }

        public Item[] GetItemsFromItemLines(int itemLineId)
        {
            Item[] items = this.itemProvider.Get()
                            .Where(i => i.ItemLine == itemLineId)
                            .ToArray();
            Console.WriteLine($"Items for ItemLine {itemLineId}: {items.Length}");
            return items;
        }

        public Item[] GetItemsFromSupplierId(int supplierId)
        {
            Item[] items = this.itemProvider.Get();
            Item[] itemsFromSupplier = items
                                        .Where(item => item.SupplierId == supplierId)
                                        .ToArray();
            return itemsFromSupplier;
        }

        public Item[] GetItemsForItemGroups(int itemGroupId)
        {
            Item[] items = this.itemProvider.Get()
                            .Where(i => i.ItemGroup == itemGroupId)
                            .ToArray();
            return items;
        }
    }
}