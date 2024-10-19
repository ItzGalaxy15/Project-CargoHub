public interface ITransferProvider
{
    public List<Transfer> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Transfer[] Get();
}