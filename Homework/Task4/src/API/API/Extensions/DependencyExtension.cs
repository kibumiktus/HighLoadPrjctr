using API.Cache;
using API.Repositories;
using API.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class DependencyExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICache, EmptyCache>();
            services.AddScoped<InfoRepository>();
            services.AddScoped<ApplicationContext>();
            services.AddTransient<DatabaseSettings>();
            services.AddTransient<CacheSettings>();
        }
    }
}
