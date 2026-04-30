using MediatR;

public class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public Guid? ParentId { get; set; }
}