using MediatR;

public class GetProductByIdQuery : IRequest<ProductByIdDto?>
{
    public int Id { get; set; }
}