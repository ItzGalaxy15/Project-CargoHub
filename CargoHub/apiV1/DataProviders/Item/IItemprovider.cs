public interface IItemProvider
{
    public List<Item> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Item[] Get();
    public void Add(Item item);
    public void Delete(Item item);

    public void Replace(Item item);
    public void Update(Item item, string uid);



    public Dictionary<string, int> GetItemTotalsByUid(string uid);



}