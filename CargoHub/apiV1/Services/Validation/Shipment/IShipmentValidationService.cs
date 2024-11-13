namespace apiV1.ValidationInterface
{
    public interface IShipmentValidationService
    {
        public bool IsShipmentValid(Shipment? shipment, bool update = false);
    }
}