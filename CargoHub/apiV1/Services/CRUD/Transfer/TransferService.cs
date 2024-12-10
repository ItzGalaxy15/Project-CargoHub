using apiV1.Interfaces;

namespace apiV1.Services
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
    }
}