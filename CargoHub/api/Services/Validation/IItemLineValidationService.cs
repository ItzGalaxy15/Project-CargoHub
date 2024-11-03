public interface IItemLineValidationService
{
    public bool IsItemLineValid(ItemLine? itemLine, bool update = false);
}