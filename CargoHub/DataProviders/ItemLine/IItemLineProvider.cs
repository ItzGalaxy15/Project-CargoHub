public interface IItemLineProvider
{
    public List<ItemLine> context { get; set; }
    public string? path { get; set; }
    public Task Save();
    ItemLine[] Get();
    public void Add(ItemLine itemLine);
    void Update(int id, ItemLine itemLine);
    public void Delete(ItemLine itemLine);


}