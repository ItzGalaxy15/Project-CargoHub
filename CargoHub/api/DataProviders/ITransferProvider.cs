public interface ITransferProvider
{
    public List<Transfer> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Transfer[] Get();

    public ItemSmall[] GetItemsByTransferId(int transferId);

}