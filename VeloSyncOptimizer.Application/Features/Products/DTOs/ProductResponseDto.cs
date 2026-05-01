public class ProductResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string SKU { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}