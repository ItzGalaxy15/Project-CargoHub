public class WarehouseValidationService : IWarehouseValidationService 
{
    private readonly IWarehouseProvider _warehouseProvider;
    public WarehouseValidationService(IWarehouseProvider warehouseProvider)
    {
        _warehouseProvider = warehouseProvider;
    }

    public bool IsWarehouseValid(Warehouse? warehouse, bool update = false)
    {
        if (warehouse is null) return false;
        if (warehouse.Id < 1) return false;


        Warehouse[] warehouses = _warehouseProvider.Get();
        bool warehouseExists = warehouses.Any(w => w.Id == warehouse.Id);
        if (update){
            // Put
            if (!warehouseExists) return false;
        } else {
            // Post
            if (warehouseExists) return false;
        }

        if (string.IsNullOrWhiteSpace(warehouse.Code)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.Name)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.Address)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.Zip)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.City)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.Province)) return false;
        if (string.IsNullOrWhiteSpace(warehouse.Country)) return false;

        // Contact validation
        if (warehouse.Contact is not null)
        {
            if (string.IsNullOrWhiteSpace(warehouse.Contact.Name)) return false;
            if (string.IsNullOrWhiteSpace(warehouse.Contact.Phone)) return false;
            if (string.IsNullOrWhiteSpace(warehouse.Contact.Email)) return false;
            // Check for valid email format
            if (!IsValidEmail(warehouse.Contact.Email)) return false;
        }
        else
        {
            return false;
        }

        return true;
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            // Check if there's a period in the domain part(the part after @)
            string domain = addr.Host;
            if (!domain.Contains(".")) return false;
        
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

}