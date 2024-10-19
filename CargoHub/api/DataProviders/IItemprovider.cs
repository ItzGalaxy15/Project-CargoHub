public interface IItemProvider
{
    public List<Item> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Item[] Get();
    public void Add(Item item);
    public void Delete(Item item);

    public bool Replace(Item item);

    public Dictionary<string, int> GetItemTotalsByUid(string uid);

}