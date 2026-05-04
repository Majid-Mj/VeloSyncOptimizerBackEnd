using MediatR;
using VeloSyncOptimizer.Application.Common.Interfaces.Repositories;
using VeloSyncOptimizer.Domain.Entities;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, int>
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<Supplier> _supplierRepo;

    public CreateProductHandler(
        IGenericRepository<Product> productRepo,
        IGenericRepository<Category> categoryRepo,
        IGenericRepository<Supplier> supplierRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _supplierRepo = supplierRepo;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
    {
        // 🔹 Normalize
        request.SKU = request.SKU.Trim().ToUpper();
        request.Name = request.Name.Trim();

        // 🔴 1. SKU uniqueness
        var skuExists = await _productRepo.ExistsAsync(
            x => x.SKU == request.SKU && !x.IsDeleted, ct);

        if (skuExists)
            throw new Exception("SKU already exists");

        // 🔴 2. Category validation
        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _categoryRepo.ExistsAsync(
                x => x.Id == request.CategoryId && !x.IsDeleted, ct);

            if (!categoryExists)
                throw new Exception("Invalid Category");
        }

        // 🔴 3. Supplier validation
        if (request.SupplierId.HasValue)
        {
            var supplierExists = await _supplierRepo.ExistsAsync(
                x => x.Id == request.SupplierId && x.IsActive, ct);

            if (!supplierExists)
                throw new Exception("Invalid Supplier");
        }

        // 🔴 4. Create entity
        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            Description = request.Description,

            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId,

            UnitCost = request.UnitCost,
            UnitPrice = request.UnitPrice,
            UnitOfMeasure = request.UnitOfMeasure,

            ReorderQty = request.ReorderQty,
            SafetyStockDays = request.SafetyStockDays,
            LeadTimeDays = request.LeadTimeDays,

            IsPerishable = request.IsPerishable,
            ShelfLifeDays = request.ShelfLifeDays,

            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _productRepo.AddAsync(product, ct);
        await _productRepo.SaveChangesAsync(ct);

        return product.Id;
    }
}