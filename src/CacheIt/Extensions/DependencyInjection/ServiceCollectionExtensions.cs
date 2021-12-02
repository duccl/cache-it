using CacheIt.Hosting;
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
    }
}