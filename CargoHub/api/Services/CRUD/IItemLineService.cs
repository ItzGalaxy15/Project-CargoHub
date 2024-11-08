public interface IItemLineService
{
    ItemLine[] GetItemLines();
    ItemLine? GetItemLineById(int id);
    Item[] GetItemsByItemLineId(int itemLineId);
    Task ReplaceItemLine(int id, ItemLine itemLine);

    public Task DeleteItemLine(ItemLine itemLine);
}