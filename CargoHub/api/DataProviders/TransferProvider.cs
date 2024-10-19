public class TransferProvider : BaseProvider<Transfer>, ITransferProvider
{
    public TransferProvider() : base("test_data/transfers.json"){}

    public Transfer[] Get(){
        return context.ToArray();
    }
}