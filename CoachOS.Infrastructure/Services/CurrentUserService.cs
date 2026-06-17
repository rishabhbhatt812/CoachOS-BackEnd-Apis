using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CoachOS.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                if (Guid.TryParse(userIdStr, out var userId))
                {
                    return userId;
                }
                return null;
            }
        }

        public Guid? InstituteId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var role = httpContext.User?.FindFirst(ClaimTypes.Role)?.Value ?? httpContext.User?.FindFirst("role")?.Value;
                    if (role == "GLOBAL_ADMIN" || role == "SUPER_ADMIN")
                    {
                        if (httpContext.Request.Headers.TryGetValue("X-Institute-Id", out var headerVal))
                        {
                            if (Guid.TryParse(headerVal, out var overriddenId))
                            {
                                return overriddenId;
                            }
                            if (headerVal == "system_global")
                            {
                                return null;
                            }
                        }
                    }
                }

                var instituteIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("InstituteId")?.Value;
                if (Guid.TryParse(instituteIdStr, out var instituteId))
                {
                    return instituteId;
                }
                return null;
            }
        }

        public string? RoleCode
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            }
        }
    }
}
