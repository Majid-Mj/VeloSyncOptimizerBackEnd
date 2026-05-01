using MediatR;

public class DeleteCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
}