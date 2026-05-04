using MediatR;
using System.Data;

public class GetProductStockHandler
    : IRequestHandler<GetProductStockQuery, ProductStockDto>
{
    private readonly IDapperRepository _repo;

    public GetProductStockHandler(IDapperRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductStockDto> Handle(
        GetProductStockQuery request,
        CancellationToken ct)
    {
        // 🔹 Get stock per warehouse
        var stockData = (await _repo.QueryAsync<ProductStockWarehouseDto>(
            "inventory.sp_GetProductStock",
            new { ProductId = request.ProductId },
            CommandType.StoredProcedure,
            ct)).ToList();

        // 🔹 Get product basic info
        var product = (await _repo.QueryAsync<dynamic>(
            "SELECT Name FROM inventory.Products WHERE Id = @Id AND IsDeleted = 0",
            new { Id = request.ProductId },
            CommandType.Text,
            ct)).FirstOrDefault();

        if (product == null)
            throw new Exception("Product not found");

        return new ProductStockDto
        {
            ProductId = request.ProductId,
            ProductName = product.Name,
            TotalStock = stockData.Sum(x => x.AvailableStock),
            Warehouses = stockData
        };
    }
}