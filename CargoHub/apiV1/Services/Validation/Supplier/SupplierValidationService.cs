using apiV1.ValidationInterfaces;

namespace apiV1.Validations
{
    public class SupplierValidationService : ISupplierValidationService
    {
        private readonly ISupplierProvider supplierProvider;

        public SupplierValidationService(ISupplierProvider supplierProvider)
        {
            this.supplierProvider = supplierProvider;
        }

        public bool IsSupplierValid(Supplier? supplier, bool update = false)
        {
            if (supplier is null)
            {
                return false;
            }

            if (supplier.Id < 0)
            {
                return false;
            }

            Supplier[] suppliers = this.supplierProvider.Get();
            bool supplierExists = suppliers.Any(s => s.Id == supplier.Id);

            if (update)
            {
                // Put
                if (!supplierExists)
                {
                    return false;
                }
            }
            else
            {
                // Post
                if (supplierExists)
                {
                    return false;
                }
            }

            // Deze properties moeten een value hebben
            // if (string.IsNullOrWhiteSpace(supplier.Code)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.Name)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.Address)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.City)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.ZipCode)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.Country)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.ContactName)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.Phonenumber)) return false;
            // if (string.IsNullOrWhiteSpace(supplier.Reference)) return false;
            return true;
        }
    }
}