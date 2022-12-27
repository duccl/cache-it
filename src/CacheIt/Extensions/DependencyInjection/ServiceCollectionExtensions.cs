using System.Configuration;
using System.Linq;
using System.Text;
using CacheIt.Hosting;
using CacheIt.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheIt.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static void ValidateCustomRefreshOptions(IConfiguration configuration)
        {
            var customRefreshOptionsSection = configuration.GetSection(CustomRefreshOptions.SectionName);
            var refreshTimesByCacheableName = customRefreshOptionsSection.GetSection("RefreshTimesByCacheableName").GetChildren();
            var misconfiguredChildren = refreshTimesByCacheableName.Where(child => int.TryParse(child.Value, out _));
            var areThereAnyMisconfiguredChildren = misconfiguredChildren.Any();

            if (areThereAnyMisconfiguredChildren)
            {
                var builder = new StringBuilder();

                foreach(var child in misconfiguredChildren)
                    builder.AppendLine($"-> {child.Path}");

                throw new ConfigurationErrorsException($"There are Cacheables configured with type 'int', this version is defined for timestamp configurations!"
                    + $"{System.Environment.NewLine} Please review Your configurations for the following:{System.Environment.NewLine}"
                    + $"{builder.ToString()}");
            }
        }

        public static IServiceCollection AddCacheIt(this IServiceCollection services)
        {
            services.AddHostedService<Handler>();
            return services;
        }

        public static IServiceCollection AddCacheIt(this IServiceCollection services, IConfiguration configuration)
        {
            ValidateCustomRefreshOptions(configuration);
            
            services.AddHostedService<Handler>();
            services.Configure<CustomRefreshOptions>(configuration.GetSection(CustomRefreshOptions.SectionName));
            return services;
        }

    }
}