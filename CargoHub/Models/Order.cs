using System.Text.Json.Serialization;

public class Order : Base
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("source_id")]
    public required int SourceId { get; set; }

    [JsonPropertyName("order_date")]
    public required string OrderDate { get; set; }

    [JsonPropertyName("request_date")]
    public required string RequestDate { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("reference_extra")]
    public required string ReferenceExtra { get; set; }

    [JsonPropertyName("order_status")]
    public required string OrderStatus { get; set; }

    [JsonPropertyName("notes")]
    public required string Notes { get; set; }

    [JsonPropertyName("shipping_notes")]
    public required string ShippingNotes { get; set; }

    [JsonPropertyName("picking_notes")]
    public required string PickingNotes { get; set; }

    [JsonPropertyName("warehouse_id")]
    public required int WarehouseId { get; set; }

    [JsonPropertyName("ship_to")]
    public required int? ShipTo { get; set; }

    [JsonPropertyName("bill_to")]
    public required int? BillTo { get; set; }

    [JsonPropertyName("shipment_id")]
    public required int? ShipmentId { get; set; }

    [JsonPropertyName("total_amount")]
    public required double TotalAmount { get; set; }

    [JsonPropertyName("total_discount")]
    public required double TotalDiscount { get; set; }

    [JsonPropertyName("total_tax")]
    public required double TotalTax { get; set; }

    [JsonPropertyName("total_surcharge")]
    public required double TotalSurcharge { get; set; }

    [JsonPropertyName("items")]
    public required List<ItemSmall> Items { get; set; }
}
