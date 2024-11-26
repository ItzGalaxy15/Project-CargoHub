using System.Text.Json;
using apiV2.Interfaces;
namespace apiV2.Services
{    
    public class ItemLineService : IItemLineService
    {
        private readonly IItemLineProvider _itemLineProvider;
        private readonly IItemProvider _itemProvider;

        public ItemLineService(IItemLineProvider itemLineProvider, IItemProvider itemProvider)
        {
            _itemLineProvider = itemLineProvider;
            _itemProvider = itemProvider;
        }

        public async Task AddItemLine(ItemLine itemLine)
        {
            string now = itemLine.GetTimeStamp();
            itemLine.UpdatedAt = now;
            itemLine.CreatedAt = now;

            _itemLineProvider.Add(itemLine);
            await _itemLineProvider.Save();
        }
        public ItemLine[] GetItemLines()
        {
            return _itemLineProvider.Get();
        }

        public ItemLine? GetItemLineById(int id)
        {
            ItemLine? itemLine = _itemLineProvider.Get().FirstOrDefault(itemLine => itemLine.Id == id);
            return itemLine;
        }

        public Item[] GetItemsByItemLineId(int itemLineId)
        {
            return _itemProvider.Get().Where(item => item.ItemLine == itemLineId).ToArray();
        }

        public Task ReplaceItemLine(int id, ItemLine itemLine)
        {
            _itemLineProvider.Update(id, itemLine);
            return _itemLineProvider.Save();
        }

        public async Task DeleteItemLine(ItemLine itemLine)
        {
            _itemLineProvider.Delete(itemLine);
            await _itemLineProvider.Save();
        }

        public async Task PatchItemLine(int id, Dictionary<string, dynamic> patch, ItemLine itemLine)
        {
            foreach (var (key, value) in patch)
            {
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "name":
                            itemLine.Name = jsonElement.GetString()!;
                            break;

                        case "description":
                            itemLine.Description = jsonElement.GetString()!;
                            break;
                    }
                }
            }

            itemLine.UpdatedAt = itemLine.GetTimeStamp();
            _itemLineProvider.Update(id, itemLine);
            await _itemLineProvider.Save();

        }

    }
}