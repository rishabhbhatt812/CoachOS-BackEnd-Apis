using System.ComponentModel.DataAnnotations;

namespace CoachOS.Application.Features.Auth.Dtos;

public class CreateUserRequest
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Mobile { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string RoleCode { get; set; } = string.Empty;
}
