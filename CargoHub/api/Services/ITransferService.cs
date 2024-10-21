public interface ITransferService
{
    public Transfer[] GetTransfers();

    public Transfer? GetTransferById(int id);

    public Task<bool> AddTransfer(Transfer transfer);

    public ItemSmall[] GetItemsByTransferId(int transferId); // New method

}