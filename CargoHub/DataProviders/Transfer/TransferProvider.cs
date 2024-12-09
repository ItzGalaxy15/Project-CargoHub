public class TransferProvider : BaseProvider<Transfer>, ITransferProvider
{
    public TransferProvider(List<Transfer> mockData)
        : base(mockData)
    {
    }

    public TransferProvider()
        : base("test_data/transfers.json")
    {
    }

    public void Add(Transfer transfer)
    {
        this.context.Add(transfer);
    }

    public Transfer[] Get()
    {
        return this.context.ToArray();
    }

    public ItemSmall[] GetItemsByTransferId(int transferId)
    {
        Transfer? transfer = this.context.FirstOrDefault(t => t.Id == transferId);
        return transfer?.Items.ToArray() ?? Array.Empty<ItemSmall>();
    }

    public void Update(Transfer transfer, int transferId)
    {
        transfer.Id = transferId;
        int index = this.context.FindIndex(ord => ord.Id == transferId);
        this.context[index] = transfer;
    }

    public void Delete(Transfer transfer)
    {
        this.context.Remove(transfer);
    }
}