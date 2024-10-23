public interface IItemLineService
{
    ItemLine[] GetItemLines();
    ItemLine? GetItemLineById(int id);
    Item[] GetItemsByItemLineId(int itemLineId);
    bool ReplaceItemLine(int id, ItemLine newItemLine);

    public Task DeleteItemLine(ItemLine itemLine);
}