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


}