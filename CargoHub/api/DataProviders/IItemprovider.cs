public interface IItemProvider
{
    public List<Item> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Item[] Get();
}