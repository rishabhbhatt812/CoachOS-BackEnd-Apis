using CoachOS.Application.Interfaces.Services;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireModuleAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _moduleCode;

        public RequireModuleAttribute(string moduleCode)
        {
            _moduleCode = moduleCode;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var moduleAccessService = context.HttpContext.RequestServices.GetRequiredService<IModuleAccessService>();

            var user = context.HttpContext.User;
            var isGlobalAdmin = user.IsInRole("GLOBAL_ADMIN") || user.IsInRole("SUPER_ADMIN");

            bool hasAccess = false;
            if (isGlobalAdmin && currentUserService.InstituteId == null)
            {
                hasAccess = true; // Bypassed for global system admin context
            }
            else if (currentUserService.InstituteId != null)
            {
                hasAccess = await moduleAccessService.IsModuleEnabledAsync(currentUserService.InstituteId.Value, _moduleCode);
            }

            if (!hasAccess)
            {
                var response = ApiResponse<object>.Fail($"[{_moduleCode}] module is not enabled for this organization.");
                context.Result = new ObjectResult(response)
                {
                    StatusCode = 403
                };
                return;
            }

            await next();
        }
    }
}
