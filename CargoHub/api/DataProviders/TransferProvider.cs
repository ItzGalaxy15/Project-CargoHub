public class TransferProvider : BaseProvider<Transfer>, ITransferProvider
{
    public TransferProvider() : base("test_data/transfers.json"){}

    public Transfer[] Get(){
        return context.ToArray();
    }


    public ItemSmall[] GetItemsByTransferId(int transferId)
    {
        Transfer? transfer = context.FirstOrDefault(t => t.Id == transferId);
        return transfer?.Items.ToArray() ?? Array.Empty<ItemSmall>();
    }

    public void Add(Transfer transfer)
    {
        context.Add(transfer);
    }

    public bool Replace(Transfer transfer)
    {
        int index = context.FindIndex(t => t.Id == transfer.Id);
        if (index == -1) return false;
        context[index] = transfer;
        return true;
    }
}