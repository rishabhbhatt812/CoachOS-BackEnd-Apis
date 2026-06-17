using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Application.Features.Auth.Dtos
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public Guid InstituteId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}
