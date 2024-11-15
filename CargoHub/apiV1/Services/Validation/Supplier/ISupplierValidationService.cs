public interface ISupplierValidationService
{
    public bool IsSupplierValid(Supplier? supplier, bool update = false);
}
