using MediatR;

public class GetSupplierByIdQuery : IRequest<SupplierResponseDto?>
{
    public Guid Id { get; set; }
}