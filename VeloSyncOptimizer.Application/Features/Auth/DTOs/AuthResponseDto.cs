using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }
}
