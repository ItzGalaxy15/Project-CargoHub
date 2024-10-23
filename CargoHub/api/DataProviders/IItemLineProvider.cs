public interface IItemLineProvider
{
    ItemLine[] Get();
    ItemLine? GetById(int id);

}