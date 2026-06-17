using System;

namespace CoachOS.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? InstituteId { get; }
        string? RoleCode { get; }
    }
}
