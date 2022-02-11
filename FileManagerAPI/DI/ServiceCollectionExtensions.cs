using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileManagerAPI.AutoMapper.Profiles;
using System.Reflection;
using FileManager.Services.Contracts.Files;
using FileManager.Services.Services.Files;

namespace FileManagerAPI.DI
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (services != null)
            {
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                services.AddTransient<IFileUploadService, FileUploadService>();
                //services.AddTransient<IUserService, UserService>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMappings(this IServiceCollection services)
        {
            if (services != null)
            {
                services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
            }
        }
    }
}
