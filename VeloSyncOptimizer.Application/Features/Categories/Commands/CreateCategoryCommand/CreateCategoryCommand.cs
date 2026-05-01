using MediatR;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
}