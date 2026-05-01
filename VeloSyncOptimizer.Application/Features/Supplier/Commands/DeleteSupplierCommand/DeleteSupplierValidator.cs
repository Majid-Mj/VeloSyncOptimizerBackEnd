using FluentValidation;

public class DeleteSupplierValidator : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Supplier Id is required");
    }
}