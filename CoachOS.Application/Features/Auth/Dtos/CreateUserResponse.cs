using System;

namespace CoachOS.Application.Features.Auth.Dtos;

public class CreateUserResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
}
