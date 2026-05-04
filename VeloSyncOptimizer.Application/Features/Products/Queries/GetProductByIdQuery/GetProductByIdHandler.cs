using MediatR;
using System.Data;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductByIdDto?>
{
    private readonly IDapperRepository _repo;

    public GetProductByIdHandler(IDapperRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductByIdDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken ct)
    {
        var result = await _repo.QueryAsync<ProductByIdDto>(
            "inventory.sp_GetProductById",
            new { Id = request.Id },
            commandType: CommandType.StoredProcedure,
            ct);

        return result.FirstOrDefault();
    }
}