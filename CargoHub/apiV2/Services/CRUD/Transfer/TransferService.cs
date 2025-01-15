using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
{
    public class TransferService : ITransferService
    {
        private ITransferProvider transferProvider;
        private IInventoryService inventoryService;

        public TransferService(ITransferProvider transferProvider, IInventoryService inventoryService)
        {
            this.transferProvider = transferProvider;
            this.inventoryService = inventoryService;
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

        public async Task CommitTransfer(int transferId)
        {
            Transfer? transfer = this.GetTransferById(transferId);
            if (transfer == null)
            {
                throw new Exception("Transfer not found");
            }

            List<string> listOfStatuses = new List<string> { "Pending", "Transit", "Transferred" };
            int currentStatus = listOfStatuses.IndexOf(transfer.TransferStatus);

            if (currentStatus == -1)
            {
                transfer.TransferStatus = listOfStatuses[0];
            }
            else if (currentStatus < listOfStatuses.Count - 1)
            {
                transfer.TransferStatus = listOfStatuses[currentStatus + 1];
            }
            else
            {
                throw new Exception("Transfer status is already 'Transferred'. No update needed.");
            }

            string now = transfer.GetTimeStamp();
            transfer.UpdatedAt = now;

            foreach (var item in transfer.Items)
            {
                var inventory = await this.inventoryService.GetInventoryByItemId(item.ItemId);
                if (inventory != null)
                {
                    if (transfer.TransferStatus == "Transit")
                    {
                        inventory.TotalExpected += item.Amount;
                    }
                    else if (transfer.TransferStatus == "Transferred")
                    {
                        inventory.TotalExpected -= item.Amount;
                        inventory.TotalAllocated += item.Amount;
                        Console.WriteLine("Item transferred: " + item.ItemId);
                    }

                    inventory.TotalOnHand = inventory.TotalExpected + inventory.TotalOrdered + inventory.TotalAllocated + inventory.TotalAvailable;
                    inventory.UpdatedAt = now;

                    var patch = new Dictionary<string, dynamic>
                    {
                        { "total_expected", inventory.TotalExpected },
                        { "total_allocated", inventory.TotalAllocated },
                        { "total_on_hand", inventory.TotalOnHand },
                    };
                    await this.inventoryService.ModifyInventory(inventory.Id, patch, inventory);
                }
            }

            this.transferProvider.Update(transfer, transferId);
            await this.transferProvider.Save();
        }
    }
}