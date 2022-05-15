using CacheIt.Hosting;
using CacheIt.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheIt.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheIt(this IServiceCollection services)
        {
            services.AddHostedService<Handler>();
            return services;
        }

        public static IServiceCollection AddCacheIt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Handler>();
            services.Configure<CustomRefreshOptions>(configuration.GetSection(CustomRefreshOptions.SectionName));
            return services;
        }

    }
}