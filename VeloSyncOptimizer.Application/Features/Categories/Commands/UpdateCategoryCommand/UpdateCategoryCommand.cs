using MediatR;

public class UpdateCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? ParentId { get; set; }
}