using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CoachOS.Api.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModuleAccessAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _moduleCode;

        public ModuleAccessAttribute(string moduleCode)
        {
            _moduleCode = moduleCode;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var moduleAccessService = context.HttpContext.RequestServices.GetRequiredService<IModuleAccessService>();

            var user = context.HttpContext.User;
            var isGlobalAdmin = user.IsInRole("GLOBAL_ADMIN") || user.IsInRole("SUPER_ADMIN");

            if (isGlobalAdmin)
            {
                if (_moduleCode == "ATTENDANCE")
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (currentUserService.InstituteId == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var globalAdminHasAccess = await moduleAccessService.IsModuleEnabledAsync(currentUserService.InstituteId.Value, _moduleCode);
                if (!globalAdminHasAccess)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                await next();
                return;
            }

            if (currentUserService.UserId == null || currentUserService.InstituteId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasAccess = await moduleAccessService.IsModuleEnabledAsync(currentUserService.InstituteId.Value, _moduleCode);
            if (!hasAccess)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
