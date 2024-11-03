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
    

    public async Task ReplaceTransfer(Transfer transfer)
    {

        string now = transfer.GetTimeStamp();
        transfer.UpdatedAt = now;
        
        _transferProvider.Replace(transfer);
        await _transferProvider.Save();
    }


    public async Task DeleteTransfer(Transfer transfer)
    {
        _transferProvider.Delete(transfer);
        await _transferProvider.Save();
    }


}