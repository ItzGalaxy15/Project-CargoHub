namespace apiV2.ValidationInterfaces
{
    public interface ISupplierValidationService
    {
        public bool IsSupplierValid(Supplier? supplier, bool update = false);

        public bool IsSupplierValidForPatch(Dictionary<string, dynamic> patch);
    }
}