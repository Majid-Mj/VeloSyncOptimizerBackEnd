using FluentValidation;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Category Id is required");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name != null);

        RuleFor(x => x.ParentId)
            .Must(id => id == null || id > 0)
            .WithMessage("Invalid ParentId");
    }
}