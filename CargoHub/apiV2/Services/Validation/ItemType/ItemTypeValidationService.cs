using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class ItemTypeValidationService : IItemTypeValidationService
    {
        private readonly IItemTypeProvider _itemTypeProvider;
        public ItemTypeValidationService(IItemTypeProvider itemTypeProvider){
            _itemTypeProvider = itemTypeProvider;
        }

        public async Task<bool> IsItemTypeValidForPOST(ItemType newItemType){
            if (newItemType == null) return false;
            if (newItemType.Id < 0) return false;
            ItemType[] itemTypes = _itemTypeProvider.Get();
            ItemType? itemType = await Task.FromResult(itemTypes.FirstOrDefault(it => it.Id == newItemType.Id));
            if (itemType != null) return false;
            // if (string.IsNullOrWhiteSpace(newItemType.Name)) return false;
            return true;
        }

        public async Task<bool> IsItemTypeValidForPUT(ItemType updatedItemType, int itemTypeId)
        {
            if (updatedItemType == null) return false;
            if (updatedItemType.Id < 0) return false;
            ItemType[] itemTypes = _itemTypeProvider.Get();
            ItemType? itemType = await Task.FromResult(itemTypes.FirstOrDefault(it => it.Id == updatedItemType.Id));
            int index = itemTypes.ToList().FindIndex(it => it.Id == itemTypeId);
            if (index == -1) return false;
            if (itemType == null) return false;
            // if (string.IsNullOrWhiteSpace(updatedItemType.Name)) return false;
            return true;
        }

        public async Task<bool> IsItemTypeValidForPATCH(Dictionary<string, dynamic> patch, int itemTypeId){
            if (patch == null) return false;
            var validProperties = new HashSet<string> {
                "name",
                "description",
            };
            ItemType[] itemTypes = _itemTypeProvider.Get();
            ItemType? itemType = await Task.FromResult(itemTypes.FirstOrDefault(it => it.Id == itemTypeId));

            if (itemType == null) return false;

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.Contains(key))
                {
                    validKeysInPatch.Add(key);
                }
            }

            if (!validKeysInPatch.Any())
            {
                return false;
            }
            return true;
        }
    }
}