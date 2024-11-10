using ECommerce.Core;
using ECommerce.Core.Helper;
using ECommerce.InfraStructure;
using System.Reflection;

namespace ECommerce.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationSevices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ImageService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); //autoMapper
            //builder.Services.AddScoped<JwtOptions>();

            return services;
        }
    }
}
