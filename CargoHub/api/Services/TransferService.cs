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


}