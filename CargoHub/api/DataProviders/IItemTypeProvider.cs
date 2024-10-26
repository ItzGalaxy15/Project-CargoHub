public interface IItemTypeProvider
{
    public List<ItemType> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public ItemType[] Get();
    public void Add(ItemType itemType);
    public void Delete(ItemType itemType);
    public bool Update(ItemType itemType, int itemTypeId);
}
