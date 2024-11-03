public interface ITransferService
{    
    public Task<bool> AddTransfer(Transfer transfer);

    public Transfer[] GetTransfers();

    public Transfer? GetTransferById(int id);

    public ItemSmall[] GetItemsByTransferId(int transferId);
    
    public Task<bool> ReplaceTransfer(Transfer transfer);

    public Task DeleteTransfer(Transfer transfer);

    public Task<bool> UpsertTransfer(Transfer transfer);
}