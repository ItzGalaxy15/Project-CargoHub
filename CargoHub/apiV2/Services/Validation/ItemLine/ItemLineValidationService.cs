using System.Text.Json;
using apiV2.ValidationInterfaces;
namespace apiV2.Validations
{    
    public class ItemLineValidationService : IItemLineValidationService
    {

        private readonly IItemLineProvider _itemLineProvider;
        public ItemLineValidationService(IItemLineProvider itemLineProvider)
        {
            _itemLineProvider = itemLineProvider;
        }

        public bool IsItemLineValid(ItemLine? itemLine, bool update = false)
        {
            if (itemLine is null) return false;
            if (itemLine.Id < 0) return false;

            ItemLine[] itemLines = _itemLineProvider.Get();
            bool itemLineExists = itemLines.Any(i => i.Id == itemLine.Id);
            if (update)
            {
                // Put 
                if (!itemLineExists) return false; 
            }
            else
            {
                // Post
                if (itemLineExists) return false;
            }

            // if (string.IsNullOrWhiteSpace(itemLine.Name)) return false;

            return true;
        }   


        public bool IsItemLineValidForPATCH(Dictionary<string, dynamic> patch)
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