public class TransferProvider : BaseProvider<Transfer>, ITransferProvider
{
    public TransferProvider() : base("test_data/transfers.json"){}


    public void Add(Transfer transfer)
    {
        context.Add(transfer);
    }

    
    public Transfer[] Get(){
        return context.ToArray();
    }


    public ItemSmall[] GetItemsByTransferId(int transferId)
    {
        Transfer? transfer = context.FirstOrDefault(t => t.Id == transferId);
        return transfer?.Items.ToArray() ?? Array.Empty<ItemSmall>();
    }


    public void Update(Transfer transfer, int orderId)
    {
        transfer.Id = orderId;
        int index = context.FindIndex(ord => ord.Id == orderId);
        context[index] = transfer;
    }


    public void Replace(Transfer transfer)
    {
        int index = context.FindIndex(t => t.Id == transfer.Id);
        context[index] = transfer;
    }

    public void Delete(Transfer transfer)
    {
        context.Remove(transfer);
    }
}