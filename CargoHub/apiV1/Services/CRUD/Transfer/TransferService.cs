using apiV1.Interfaces;
namespace apiV1.Services
{    
    public class TransferService : ITransferService
    {
        private ITransferProvider _transferProvider;
        public TransferService(ITransferProvider transferProvider)
        {
            _transferProvider = transferProvider;
        }


        public async Task AddTransfer(Transfer transfer)
        {
            
            string now = transfer.GetTimeStamp();
            transfer.UpdatedAt = now;
            transfer.CreatedAt = now;

            _transferProvider.Add(transfer);
            await _transferProvider.Save();
        }


        public Transfer[] GetTransfers()
        {
            return _transferProvider.Get();
        }



        public Transfer? GetTransferById(int id)
        {
            Transfer[] transfers = _transferProvider.Get();
            Transfer? transfer = transfers.FirstOrDefault(transfer => transfer.Id == id);
            return transfer;
        }

        public ItemSmall[] GetItemsByTransferId(int transferId)
        {
            return _transferProvider.GetItemsByTransferId(transferId);
        }
        

        public async Task<bool> ReplaceTransfer(Transfer transfer, int transferId)
        {

            string now = transfer.GetTimeStamp();
            transfer.UpdatedAt = now;
            
            _transferProvider.Update(transfer, transferId);
            await _transferProvider.Save();
            return true;
        }


        public async Task DeleteTransfer(Transfer transfer)
        {
            _transferProvider.Delete(transfer);
            await _transferProvider.Save();
        }


    }
}