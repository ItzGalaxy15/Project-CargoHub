public interface IItemLineProvider
{
    ItemLine[] Get();
    ItemLine? GetById(int id);
    bool ReplaceItemLine(int id, ItemLine newItemLine); // New method


}