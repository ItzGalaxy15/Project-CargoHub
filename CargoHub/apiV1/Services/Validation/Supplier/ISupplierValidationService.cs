namespace apiV1.ValidationInterfaces
{
    public interface ISupplierValidationService
    {
        public bool IsSupplierValid(Supplier? supplier, bool update = false);
    }
}