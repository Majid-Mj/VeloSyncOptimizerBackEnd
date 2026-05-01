public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }

    public List<CategoryResponseDto> Children { get; set; } = new();
}