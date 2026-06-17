using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoachOS.Application.Validators
{
    public static class ValidationConfig
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
