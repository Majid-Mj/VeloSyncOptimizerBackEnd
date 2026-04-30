using MediatR;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}