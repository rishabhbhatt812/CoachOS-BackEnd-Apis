using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Application.Features.Auth.Dtos
{
    public class RegisterInstituteResponse
    {
        public Guid InstituteId { get; set; }
        public Guid AdminUserId { get; set; }
        public string InstituteName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
    }
}
