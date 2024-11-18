using System.Text.Json;
using apiV2.ValidationInterfaces;
namespace apiV2.Validations
{    
    public class TransferValidationService : ITransferValidationService
    {
        private readonly ITransferProvider _transferProvider;

        public TransferValidationService(ITransferProvider transferProvider)
        {
            _transferProvider = transferProvider;
        }

        public bool IsTransferValid(Transfer? transfer, bool update = false)
        {
            if (transfer == null)
            {
                return false;
            }

            Transfer[] transfers = _transferProvider.Get();
            bool transferExists = transfers.Any(t => t.Id == transfer.Id);
            if (update)
            {
                if (!transferExists)
                {
                    return false;
                }
            }
            else
            {
                if (transferExists)
                {
                    return false;
                }
            }


            if (transfer.Id < 0)
            {
                return false;
            }
            // if (string.IsNullOrWhiteSpace(transfer.Reference)) return false;
            if (transfer.TransferFrom < 0) return false;
            if (transfer.TransferTo < 0) return false;
            // if (string.IsNullOrWhiteSpace(transfer.TransferStatus)) return false;
            if (transfer.Items.Count == 0) return false;
            if (transfer.Items.Any(i => i.Amount < 0)) return false;
            // if (transfer.Items.Any(i => string.IsNullOrWhiteSpace(i.ItemId))) return false;


            return true;
        }


        public bool IsTransferValidForPATCH(Dictionary<string, dynamic> patch, int transferId)
        {

            if (patch is null || !patch.Any())
            {
                return false;
            }

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "reference", JsonValueKind.String },
                { "transfer_from", JsonValueKind.Number },
                { "transfer_to", JsonValueKind.Number },
                { "transfer_status", JsonValueKind.String },
                { "items", JsonValueKind.Array }
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
                        //remove key if not valid type
                    }
                    else
                    {
                        if (key == "items")
                        {
                            foreach (var item in value.EnumerateArray())
                            {
                                if (item.GetProperty("item_id").ValueKind != JsonValueKind.String)
                                {
                                    return false;
                                }
                            }
                        }
                        validKeysInPatch.Add(key);
                    } 
                    validKeysInPatch.Add(key);
                }
            }
            return true;
        }
    }
}