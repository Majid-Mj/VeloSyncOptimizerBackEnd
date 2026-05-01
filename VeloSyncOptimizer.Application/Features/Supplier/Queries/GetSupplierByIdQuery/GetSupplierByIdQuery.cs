using MediatR;

public class GetSupplierByIdQuery : IRequest<SupplierResponseDto?>
{
    public int Id { get; set; }
}