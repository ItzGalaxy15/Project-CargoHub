using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class ItemLineService : IItemLineService
    {
        private readonly IItemLineProvider itemLineProvider;
        private readonly IItemProvider itemProvider;

        public ItemLineService(IItemLineProvider itemLineProvider, IItemProvider itemProvider)
        {
            this.itemLineProvider = itemLineProvider;
            this.itemProvider = itemProvider;
        }

        public async Task AddItemLine(ItemLine itemLine)
        {
            string now = itemLine.GetTimeStamp();
            itemLine.UpdatedAt = now;
            itemLine.CreatedAt = now;

            this.itemLineProvider.Add(itemLine);
            await this.itemLineProvider.Save();
        }

        public ItemLine[] GetItemLines()
        {
            return this.itemLineProvider.Get();
        }

        public ItemLine? GetItemLineById(int id)
        {
            ItemLine? itemLine = this.itemLineProvider.Get().FirstOrDefault(itemLine => itemLine.Id == id);
            return itemLine;
        }

        public Item[] GetItemsByItemLineId(int itemLineId)
        {
            return this.itemProvider.Get().Where(item => item.ItemLine == itemLineId).ToArray();
        }

        public Task ReplaceItemLine(int id, ItemLine itemLine)
        {
            this.itemLineProvider.Update(id, itemLine);
            return this.itemLineProvider.Save();
        }

        public async Task DeleteItemLine(ItemLine itemLine)
        {
            this.itemLineProvider.Delete(itemLine);
            await this.itemLineProvider.Save();
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
            this.itemLineProvider.Update(id, itemLine);
            await this.itemLineProvider.Save();
        }
    }
}