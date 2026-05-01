using System.ComponentModel.DataAnnotations;

namespace VeloSyncOptimizer.Application.Features.Auth.DTOs;

public class RegisterRequestDto
{
    // ── FIRST NAME ──────────────────────────────────────────
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$",
        ErrorMessage = "First name can only contain letters, spaces, hyphens or apostrophes")]
    public string FirstName { get; set; } = string.Empty;

    // ── LAST NAME ───────────────────────────────────────────
    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$",
        ErrorMessage = "Last name can only contain letters, spaces, hyphens or apostrophes")]
    public string LastName { get; set; } = string.Empty;

    // ── EMAIL ───────────────────────────────────────────────
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    [RegularExpression(
        @"^[a-zA-Z0-9][a-zA-Z0-9._%+\-]*@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    // ── PASSWORD ────────────────────────────────────────────
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
    public string Password { get; set; } = string.Empty;

    // ── CONFIRM PASSWORD ────────────────────────────────────
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    [Range(1, 10, ErrorMessage = "Invalid Role selected")]
    public int RoleId { get; set; }
}
