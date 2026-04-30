public class CategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid? ParentId { get; set; }

    public List<CategoryResponseDto> Children { get; set; } = new();
}