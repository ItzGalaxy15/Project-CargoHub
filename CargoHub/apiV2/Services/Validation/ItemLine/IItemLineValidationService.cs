namespace apiV2.ValidationInterfaces
{
    public interface IItemLineValidationService
    {
        public bool IsItemLineValid(ItemLine? itemLine, bool update = false);

        public bool IsItemLineValidForPATCH(Dictionary<string, dynamic> patch);
    }
}