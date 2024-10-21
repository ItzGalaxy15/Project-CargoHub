public interface IInventoryProvider
{
    Inventory[] Get();
    Inventory? GetByUid(string uid);
}