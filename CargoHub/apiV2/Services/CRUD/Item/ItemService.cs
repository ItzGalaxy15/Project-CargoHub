using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
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

        public async Task PatchItem(string uid, Dictionary<string, dynamic> patch, Item item)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "code":
                            item.Code = value.GetString();
                            break;
                        case "description":
                            item.Description = value.GetString();
                            break;
                        case "short_description":
                            item.ShortDescription = value.GetString();
                            break;
                        case "upc_code":
                            item.UpcCode = value.GetString();
                            break;
                        case "model_number":
                            item.ModelNumber = value.GetString();
                            break;
                        case "commodity_code":
                            item.CommodityCode = value.GetString();
                            break;
                        case "item_line":
                            item.ItemLine = value.GetInt32();
                            break;
                        case "item_group":
                            item.ItemGroup = value.GetInt32();
                            break;
                        case "item_type":
                            item.ItemType = value.GetInt32();
                            break;
                        case "unit_purchase_quantity":
                            item.UnitPurchaseQuantity = value.GetInt32();
                            break;
                        case "unit_order_quantity":
                            item.UnitOrderQuantity = value.GetInt32();
                            break;
                        case "pack_order_quantity":
                            item.PackOrderQuantity = value.GetInt32();
                            break;
                        case "supplier_id":
                            item.SupplierId = value.GetInt32();
                            break;
                        case "supplier_code":
                            item.SupplierCode = value.GetString();
                            break;
                        case "supplier_part_number":
                            item.SupplierPartNumber = value.GetString();
                            break;
                    }
                }
            }

            item.UpdatedAt = item.GetTimeStamp();
            this.itemProvider.Update(item, uid);
            await this.itemProvider.Save();
        }
    }
}