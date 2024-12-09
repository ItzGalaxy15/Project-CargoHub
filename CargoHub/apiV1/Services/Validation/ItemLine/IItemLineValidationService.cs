namespace apiV1.ValidationInterfaces
{
    public interface IItemLineValidationService
    {
        public bool IsItemLineValid(ItemLine? itemLine, bool update = false);
    }
}