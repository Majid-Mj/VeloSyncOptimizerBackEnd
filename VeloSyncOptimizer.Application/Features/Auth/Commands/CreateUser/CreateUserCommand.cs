using MediatR;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.CreateUser;

public record CreateUserCommand : IRequest<int>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public byte RoleId { get; init; }

    public CreateUserCommand(string email, string password, string firstName, string lastName, byte roleId)
    {
        Email = email?.Trim() ?? string.Empty;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
    }
}
