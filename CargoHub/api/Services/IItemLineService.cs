public interface IItemLineService
{
    ItemLine[] GetItemLines();

    ItemLine? GetItemLineById(int id);
    Item[] GetItemsByItemLineId(int itemLineId);


}