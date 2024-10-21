public class TransferService : ITransferService
{
    private ITransferProvider _transferProvider;
    public TransferService(ITransferProvider transferProvider)
    {
        _transferProvider = transferProvider;
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
    

    public async Task<bool> ReplaceTransfer(Transfer transfer)
    {
        Transfer[] transfers = _transferProvider.Get();
        if (!transfers.Any(t => t.Id == transfer.Id) || !_transferProvider.Replace(transfer))
        {
            return false;
        }

        string now = transfer.GetTimeStamp();
        transfer.UpdatedAt = now;
        
        _transferProvider.Replace(transfer);
        await _transferProvider.Save();
        return true;
    }


    public async Task DeleteTransfer(Transfer transfer)
    {
        _transferProvider.Delete(transfer);
        await _transferProvider.Save();
    }


}