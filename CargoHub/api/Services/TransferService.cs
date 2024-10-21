public class TransferService : ITransferService
{
    private ITransferProvider _transferProvider;
    public TransferService(ITransferProvider transferProvider)
    {
        _transferProvider = transferProvider;
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

    public async Task<bool> AddTransfer(Transfer transfer)
    {
        Transfer[] transfers = _transferProvider.Get();
        if (transfers.Any(t => t.Id == transfer.Id))
        {
            return false;
        }
        
        string now = transfer.GetTimeStamp();
        transfer.UpdatedAt = now;
        transfer.CreatedAt = now;

        _transferProvider.Add(transfer);
        await _transferProvider.Save();
        return true;
    }


    public ItemSmall[] GetItemsByTransferId(int transferId)
    {
        return _transferProvider.GetItemsByTransferId(transferId);
    }


}