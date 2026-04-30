using MediatR;

public class DeleteSupplierCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}