using System.Text.Json;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
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

        public bool IsSupplierValidForPatch(Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any())
            {
                return false;
            }

            var validProperties = new Dictionary<string, Type>
            {
                { "code", typeof(string) },
                { "name", typeof(string) },
                { "address", typeof(string) },
                { "address_extra", typeof(string) },
                { "city", typeof(string) },
                { "zip_code", typeof(string) },
                { "country", typeof(string) },
                { "contact_name", typeof(string) },
                { "phonenumber", typeof(string) },
                { "reference", typeof(string) },
            };

            foreach (var key in patch.Keys)
            {
                if (!validProperties.ContainsKey(key))
                {
                    continue;
                }

                var expectedType = validProperties[key];
                var value = patch[key];

                if (value is JsonElement jsonElement)
                {
                    // Validate JsonElement value kinds
                    if (expectedType == typeof(string) && jsonElement.ValueKind != JsonValueKind.String && jsonElement.ValueKind != JsonValueKind.Null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}