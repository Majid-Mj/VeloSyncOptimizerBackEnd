using MediatR;
using VeloSyncOptimizer.Application.Features.Auth.DTOs;

namespace VeloSyncOptimizer.Application.Features.Auth.Commands.Login
{
    public class LoginUserCommand : IRequest<AuthResponseDto>
    {
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim() ?? string.Empty;
        }
        public string Password { get; set; } = string.Empty;
    }
}
