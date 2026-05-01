using MediatR;

public class GetProductsQuery : IRequest<PagedResult<ProductResponseDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}