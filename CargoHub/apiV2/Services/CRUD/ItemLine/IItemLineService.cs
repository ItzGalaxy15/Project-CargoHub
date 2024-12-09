namespace apiV2.Interfaces
{
    public interface IItemLineService
    {
        public Task AddItemLine(ItemLine itemLine);

        ItemLine[] GetItemLines();

        ItemLine? GetItemLineById(int id);

        Item[] GetItemsByItemLineId(int itemLineId);

        Task ReplaceItemLine(int id, ItemLine itemLine);

        public Task DeleteItemLine(ItemLine itemLine);

        public Task PatchItemLine(int id, Dictionary<string, dynamic> patch, ItemLine itemLine);
    }
}