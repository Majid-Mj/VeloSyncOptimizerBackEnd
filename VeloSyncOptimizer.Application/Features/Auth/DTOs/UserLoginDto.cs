using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloSyncOptimizer.Application.Features.Auth.DTOs;

public class UserLoginDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public int RoleId { get; set; }
    public string RoleName { get; set; }  
}
