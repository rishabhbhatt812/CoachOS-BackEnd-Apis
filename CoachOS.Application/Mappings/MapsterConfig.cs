using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoachOS.Application.Mappings
{
    public static class MapsterConfig
    {
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            
            // Mapster doesn't natively map DateOnly <-> DateTime because DateOnly is immutable struct without a default parameterless constructor.
            // Register all possible combinations of DateOnly <-> DateTime (including nullability variations)
            typeAdapterConfig.NewConfig<DateOnly, DateTime>()
                .MapWith(src => src.ToDateTime(TimeOnly.MinValue));

            typeAdapterConfig.NewConfig<DateTime, DateOnly>()
                .MapWith(src => DateOnly.FromDateTime(src));

            typeAdapterConfig.NewConfig<DateOnly?, DateTime?>()
                .MapWith(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);

            typeAdapterConfig.NewConfig<DateTime?, DateOnly?>()
                .MapWith(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : (DateOnly?)null);

            typeAdapterConfig.NewConfig<DateOnly, DateTime?>()
                .MapWith(src => src.ToDateTime(TimeOnly.MinValue));

            typeAdapterConfig.NewConfig<DateTime?, DateOnly>()
                .MapWith(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : default);

            typeAdapterConfig.NewConfig<DateOnly?, DateTime>()
                .MapWith(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : default);

            typeAdapterConfig.NewConfig<DateTime, DateOnly?>()
                .MapWith(src => DateOnly.FromDateTime(src));

            // Prevent "immutable type" error on self-mappings of DateOnly/DateOnly?
            typeAdapterConfig.NewConfig<DateOnly, DateOnly>()
                .MapWith(src => src);

            typeAdapterConfig.NewConfig<DateOnly?, DateOnly?>()
                .MapWith(src => src);

            typeAdapterConfig.NewConfig<DateOnly?, DateOnly>()
                .MapWith(src => src.HasValue ? src.Value : default);

            typeAdapterConfig.NewConfig<DateOnly, DateOnly?>()
                .MapWith(src => src);

            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
            typeAdapterConfig.Compile();

            services.AddSingleton(typeAdapterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
