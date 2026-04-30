using MediatR;

public class GetSupplierByIdHandler
    : IRequestHandler<GetSupplierByIdQuery, SupplierResponseDto?>
{
    private readonly ICategoryRepository _repo;

    public GetSupplierByIdHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<SupplierResponseDto?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken ct)
    {
        var supplier = (await _repo.QueryAsync<SupplierResponseDto>(
            "inventory.sp_GetSupplierById",
            new { request.Id }))
            .FirstOrDefault();

        if (supplier == null)
            throw new Exception("Supplier not found");

        return supplier;
    }
}