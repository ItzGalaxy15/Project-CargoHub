namespace apiV1.Interfaces
{
    public interface ITransferService
    {
        public Task AddTransfer(Transfer transfer);

        public Transfer[] GetTransfers();

        public Transfer? GetTransferById(int id);

        public ItemSmall[] GetItemsByTransferId(int transferId);

        public Task UpdateTransfer(Transfer transfer, int transferId);

        public Task DeleteTransfer(Transfer transfer);
    }
}