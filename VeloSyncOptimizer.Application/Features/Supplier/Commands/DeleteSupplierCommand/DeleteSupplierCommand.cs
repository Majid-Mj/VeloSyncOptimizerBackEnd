using MediatR;

public class DeleteSupplierCommand : IRequest<bool>
{
    public int Id { get; set; }
}