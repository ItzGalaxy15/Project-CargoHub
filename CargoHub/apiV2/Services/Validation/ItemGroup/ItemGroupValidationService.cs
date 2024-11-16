using System.Text.Json;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
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

        public bool IsItemGroupValidForPatch(Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any()) return false;

            var validProperties = new Dictionary<string, Type>
            {
                { "name", typeof(string) },
                { "description", typeof(string) }
            };

            foreach (var key in patch.Keys)
            {
                if (!validProperties.ContainsKey(key)) continue;

                var expectedType = validProperties[key];
                var value = patch[key];

                if (value is JsonElement jsonElement)
                {
                    // Validate JsonElement value kinds
                    if (expectedType == typeof(string) && jsonElement.ValueKind != JsonValueKind.String && jsonElement.ValueKind != JsonValueKind.Null) return false;
                }
            }

            return true;
        }
    }
}