public class SupplierResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}