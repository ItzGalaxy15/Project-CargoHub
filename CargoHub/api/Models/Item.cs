using System.Text.Json.Serialization;

public class Item : Base
{

    [JsonPropertyName("uid")]
    public required string Uid { get; set;}

    [JsonPropertyName("code")]
    public required string Code { get; set;}

    [JsonPropertyName("description")]
    public required string Description { get; set;}

    [JsonPropertyName("short_description")]
    public required string ShortDescription { get; set; }

    [JsonPropertyName("upc_code")]
    public required string UpcCode  { get; set; }
    
    [JsonPropertyName("model_number")]
    public required string ModelNumber { get; set; }

    [JsonPropertyName("commodity_code")]
    public required string CommodityCode { get; set; }

    [JsonPropertyName("item_line")]
    public required int ItemLine { get; set; }

    [JsonPropertyName("item_group")]
    public required int ItemGroup { get; set; }

    [JsonPropertyName("item_type")]
    public required int ItemType { get; set; }

    [JsonPropertyName("unit_purchase_quantity")]
    public required int UnitPurchaseQuantity { get; set; }

    [JsonPropertyName("unit_order_quantity")]
    public required int UnitOrderQuantity { get; set; }
    
    [JsonPropertyName("pack_order_quantity")]
    public required int PackOrderQuantity { get; set; }

    [JsonPropertyName("supplier_id")]
    public required int SupplierId { get; set; }

    [JsonPropertyName("supplier_code")]
    public required string SupplierCode { get; set; }

    [JsonPropertyName("supplier_part_number")]
    public required string SupplierPartNumber { get; set; }

}