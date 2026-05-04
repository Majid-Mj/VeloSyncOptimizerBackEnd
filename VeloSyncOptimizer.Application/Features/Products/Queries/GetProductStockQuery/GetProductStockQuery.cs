using MediatR;

public class GetProductStockQuery : IRequest<ProductStockDto>
{
    public int ProductId { get; set; }
}