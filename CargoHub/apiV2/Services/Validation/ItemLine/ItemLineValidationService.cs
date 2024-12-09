using System.Text.Json;
using System.Text.Json.Nodes;
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

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "name", JsonValueKind.String },
                { "description", JsonValueKind.String }
            };

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
                    }
                    else
                    {
                        validKeysInPatch.Add(key);
                    }
                }
            }
            if (validKeysInPatch.Count == 0) return false;
            return true;
        }

    }
}