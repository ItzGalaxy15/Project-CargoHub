using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class TransferService : ITransferService
    {
        private ITransferProvider transferProvider;

        public TransferService(ITransferProvider transferProvider)
        {
            this.transferProvider = transferProvider;
        }

        public async Task AddTransfer(Transfer transfer)
        {
            string now = transfer.GetTimeStamp();
            transfer.UpdatedAt = now;
            transfer.CreatedAt = now;

            this.transferProvider.Add(transfer);
            await this.transferProvider.Save();
        }

        public Transfer[] GetTransfers()
        {
            return this.transferProvider.Get();
        }

        public Transfer? GetTransferById(int id)
        {
            Transfer[] transfers = this.transferProvider.Get();
            Transfer? transfer = transfers.FirstOrDefault(transfer => transfer.Id == id);
            return transfer;
        }

        public ItemSmall[] GetItemsByTransferId(int transferId)
        {
            return this.transferProvider.GetItemsByTransferId(transferId);
        }

        public async Task UpdateTransfer(Transfer transfer, int transferId)
        {
            string now = transfer.GetTimeStamp();
            transfer.UpdatedAt = now;

            this.transferProvider.Update(transfer, transferId);
            await this.transferProvider.Save();
        }

        public async Task DeleteTransfer(Transfer transfer)
        {
            this.transferProvider.Delete(transfer);
            await this.transferProvider.Save();
        }

        public async Task PatchTransfer(int id, Dictionary<string, dynamic> patch, Transfer transfer)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "reference":
                            transfer.Reference = jsonElement.GetString()!;
                            break;
                        case "transfer_from":
                            transfer.TransferFrom = jsonElement.GetInt32();
                            break;
                        case "transfer_to":
                            transfer.TransferTo = jsonElement.GetInt32();
                            break;
                        case "transfer_status":
                            transfer.TransferStatus = jsonElement.GetString()!;
                            break;
                        case "items":
                            transfer.Items = jsonElement.EnumerateArray()
                                            .Select(item => new ItemSmall
                                            {
                                                ItemId = item.GetProperty("item_id").GetString()!,
                                                Amount = item.GetProperty("amount").GetInt32(),
                                            })
                                            .ToList();
                            break;
                    }
                }
            }

            transfer.UpdatedAt = transfer.GetTimeStamp();
            this.transferProvider.Update(transfer, id);
            await this.transferProvider.Save();
        }
    }
}