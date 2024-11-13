using ECommerce.Api.Filters;
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
            //services.AddSingleton<PayPalService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); //autoMapper
            #region Filters
            services.AddScoped<ValidateIdNotNullAttribute>();
            #endregion
            //builder.Services.AddScoped<JwtOptions>();

            return services;
        }
    }
}
