using FluentValidation;

namespace VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TotalCapacity).GreaterThanOrEqualTo(0);
    }
}