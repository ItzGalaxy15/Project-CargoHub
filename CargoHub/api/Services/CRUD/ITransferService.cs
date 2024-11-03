public interface ITransferService
{    
    public Task AddTransfer(Transfer transfer);

    public Transfer[] GetTransfers();

    public Transfer? GetTransferById(int id);

    public ItemSmall[] GetItemsByTransferId(int transferId);
    
    public Task ReplaceTransfer(Transfer transfer);

    public Task DeleteTransfer(Transfer transfer);
}