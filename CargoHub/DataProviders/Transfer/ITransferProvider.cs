public interface ITransferProvider
{
    public List<Transfer> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Transfer[] Get();

    public void Add(Transfer transfer);

    public void Update(Transfer transfer, int transferId);

    public void Delete(Transfer transfer);

    public ItemSmall[] GetItemsByTransferId(int transferId);

}