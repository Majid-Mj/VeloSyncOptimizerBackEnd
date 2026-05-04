using MediatR;

public class CreateProductCommand : IRequest<int>
{
    public string SKU { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }

    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }

    public string UnitOfMeasure { get; set; } = "PCS";

    public int ReorderQty { get; set; }
    public int SafetyStockDays { get; set; }
    public int LeadTimeDays { get; set; }

    public bool IsPerishable { get; set; }
    public int? ShelfLifeDays { get; set; }
}