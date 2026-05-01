public class ProductResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string SKU { get; set; } = default!;

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}