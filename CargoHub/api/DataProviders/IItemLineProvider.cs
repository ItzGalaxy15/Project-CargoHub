public interface IItemLineProvider
{
    public Task Save();
    ItemLine[] Get();
    bool ReplaceItemLine(int id, ItemLine newItemLine);

    public void Delete(ItemLine itemLine);


}