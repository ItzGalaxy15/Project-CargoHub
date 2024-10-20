public interface IItemTypeProvider
{
    public List<ItemType> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public ItemType[] Get();
    public void Delete(int id);
}
