public class ProductStockWarehouseDto
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = default!;

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }

    public int AvailableStock { get; set; }
}