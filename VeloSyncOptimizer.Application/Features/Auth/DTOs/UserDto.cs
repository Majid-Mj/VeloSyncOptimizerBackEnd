namespace VeloSyncOptimizer.Application.Features.Users.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool IsApproved { get; set; }
}