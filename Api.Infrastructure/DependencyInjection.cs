using Api.Application.Common;
using Api.Infrastructure.Authentication;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //reading config
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //singleton
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddSingleton<IEnumExtensions, EnumExtensions>();

            //transient
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

            //scoped
            services.AddScoped<DapperContext>(provider => new DapperContext(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
