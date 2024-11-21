using ECommerce.Api.Filters;
using ECommerce.Core;
using ECommerce.Core.Helper;
using ECommerce.Core.IServices;
using ECommerce.InfraStructure;
using ECommerce.InfraStructure.LoggerService;
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
            #region Middleware

            //services.AddTransient<GlobalExeptionHandlingMiddleware>();

            #endregion

            // looger
            services.AddSingleton<ILoggerManager, LoggerManager>();

            return services;
        }
    }
}
