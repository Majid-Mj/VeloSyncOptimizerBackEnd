using FluentValidation;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.DTOs;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();
        RuleFor(v => v.Password).NotEmpty().MinimumLength(6);
        RuleFor(v => v.FirstName).NotEmpty();
        RuleFor(v => v.LastName).NotEmpty();
        RuleFor(v => v.RoleId).GreaterThan(0);
    }
}
