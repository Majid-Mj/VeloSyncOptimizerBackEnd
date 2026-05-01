using FluentValidation;

public class DeleteSupplierValidator : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Supplier Id is required");
    }
}