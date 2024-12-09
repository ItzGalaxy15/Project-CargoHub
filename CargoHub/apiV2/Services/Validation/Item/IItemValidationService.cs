namespace apiV2.ValidationInterfaces
{
    public interface IItemValidationService
    {
        public bool IsItemValid(Item? item, bool update = false);

        public Task<bool> IsItemValidForPATCH(Dictionary<string, dynamic> patch, string uid);
    }
}