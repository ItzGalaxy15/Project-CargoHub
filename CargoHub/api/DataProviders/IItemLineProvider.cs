public interface IItemLineProvider
{
    public Task Save();
    ItemLine[] Get();
    void ReplaceItemLine(int id, ItemLine itemLine);

    public void Delete(ItemLine itemLine);


}