public interface ITransferService
{
    public Transfer[] GetTransfers();

    public Transfer? GetTransferById(int id);

    public ItemSmall[] GetItemsByTransferId(int transferId); // New method

}