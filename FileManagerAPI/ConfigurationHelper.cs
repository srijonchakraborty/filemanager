using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SMC.TenderAPI.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(!string.IsNullOrEmpty(outputPath) ? outputPath : Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string environment, string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(!string.IsNullOrEmpty(outputPath) ? outputPath : Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public static T GetApplicationConfiguration<T>(IConfigurationRoot iConfig)
        {
            var configuration = default(T);

            iConfig?
                .GetSection(typeof(T).Name)?
                .Bind(configuration);

            return configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public static IConfigurationSection GetConfigurationSection<T>(IConfigurationRoot iConfig)
        {
            return iConfig?.GetSection(typeof(T).Name);
        }        
    }
}
