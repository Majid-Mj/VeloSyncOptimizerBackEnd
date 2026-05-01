using MediatR;

public class UpdateCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }
}