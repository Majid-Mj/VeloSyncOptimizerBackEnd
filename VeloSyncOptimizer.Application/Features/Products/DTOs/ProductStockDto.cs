public class ProductStockDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public int TotalStock { get; set; }

    public List<ProductStockWarehouseDto> Warehouses { get; set; } = new();
}