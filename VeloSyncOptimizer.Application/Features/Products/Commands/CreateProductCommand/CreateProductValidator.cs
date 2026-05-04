using FluentValidation;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(300);

        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UnitCost).GreaterThanOrEqualTo(0);

        RuleFor(x => x.ReorderQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LeadTimeDays).GreaterThanOrEqualTo(0);

        RuleFor(x => x.UnitOfMeasure).NotEmpty();

        RuleFor(x => x.ShelfLifeDays)
            .GreaterThan(0)
            .When(x => x.IsPerishable);
    }
}