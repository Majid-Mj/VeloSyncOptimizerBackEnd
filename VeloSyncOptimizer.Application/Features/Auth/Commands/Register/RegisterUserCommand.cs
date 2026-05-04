
using MediatR;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand : IRequest<int>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public int RoleId { get; init; }

    public RegisterUserCommand(string email, string password, string firstName, string lastName, int roleId)
    {
        Email = email?.Trim() ?? string.Empty;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
    }
}
