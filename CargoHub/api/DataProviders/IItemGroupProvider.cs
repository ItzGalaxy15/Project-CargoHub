public interface IItemGroupProvider
{
    public List<ItemGroup> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public  ItemGroup[] Get();
    public void Add(ItemGroup itemGroup);
    public void Delete(ItemGroup itemGroup);
    public bool Replace(ItemGroup itemGroup, int itemGroupId);
}
