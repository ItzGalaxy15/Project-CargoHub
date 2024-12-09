using System.Text.Json;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class ItemValidationService : IItemValidationService
    {
        private readonly IItemProvider itemProvider;

        public ItemValidationService(IItemProvider itemProvider)
        {
            this.itemProvider = itemProvider;
        }

        public bool IsItemValid(Item? item, bool update = false)
        {
            if (item is null)
            {
                return false;
            }

            Item[] items = this.itemProvider.Get();
            bool itemExists = items.Any(i => i.Uid == item.Uid);
            if (update)
            {
                // Put
                if (!itemExists)
                {
                    return false;
                }
            }
            else
            {
                // Post
                if (itemExists)
                {
                    return false;
                }
            }

            // if (string.IsNullOrWhiteSpace(item.UpcCode)) return false;
            // if (string.IsNullOrWhiteSpace(item.ModelNumber)) return false;
            // if (string.IsNullOrWhiteSpace(item.CommodityCode)) return false;
            if (item.ItemLine <= 0)
            {
                return false;
            }

            if (item.ItemGroup <= 0)
            {
                return false;
            }

            if (item.ItemType <= 0)
            {
                return false;
            }

            if (item.UnitPurchaseQuantity < 0)
            {
                return false;
            }

            if (item.UnitOrderQuantity < 0)
            {
                return false;
            }

            if (item.PackOrderQuantity < 0)
            {
                return false;
            }

            if (item.SupplierId <= 0)
            {
                return false;
            }

            // if (string.IsNullOrWhiteSpace(item.SupplierCode)) return false;
            // if (string.IsNullOrWhiteSpace(item.SupplierPartNumber)) return false;
            return true;
        }

        public async Task<bool> IsItemValidForPATCH(Dictionary<string, dynamic> patch, string uid)
        {
            if (patch is null)
            {
                return false;
            }

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "code", JsonValueKind.String },
                { "description", JsonValueKind.String },
                { "short_description", JsonValueKind.String },
                { "upc_code", JsonValueKind.String },
                { "model_number", JsonValueKind.String },
                { "commodity_code", JsonValueKind.String },
                { "item_line", JsonValueKind.Number },
                { "item_group", JsonValueKind.Number },
                { "item_type", JsonValueKind.Number },
                { "unit_purchase_quantity", JsonValueKind.Number },
                { "unit_order_quantity", JsonValueKind.Number },
                { "pack_order_quantity", JsonValueKind.Number },
                { "supplier_id", JsonValueKind.Number },
                { "supplier_code", JsonValueKind.String },
                { "supplier_part_number", JsonValueKind.String },
            };

            Item[] items = this.itemProvider.Get();
            Item? item = await Task.FromResult(items.FirstOrDefault(i => i.Uid == uid));

            if (item is null)
            {
                return false;
            }

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.ContainsKey(key))
                {
                    var expectedType = validProperties[key];
                    JsonElement value = patch[key];
                    if (value.ValueKind != expectedType)
                    {
                        patch.Remove(key);

                        // remove key if not valid type
                    }
                    else
                    {
                        validKeysInPatch.Add(key);
                    }
                }
            }

            if (validKeysInPatch.Count == 0)
            {
                return false; // Change this line to check if there are valid keys
            }

            return true;
        }
    }
}