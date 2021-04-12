using System;
using API.Cache;
using API.Repositories;
using API.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public static class DependencyExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICache>(c =>
            {
                var cacheStrategy = c.GetService<CacheSettings>().CacheStrategy;

                return cacheStrategy switch
                {
                    CacheSettings.CacheType.Empty => c.GetService<EmptyCache>(),
                    CacheSettings.CacheType.Probabilistic => c.GetService<ProbabilisticCache>(),
                    CacheSettings.CacheType.Simple => c.GetService<SimpleCache>(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            });
            services.AddTransient<EmptyCache>();
            services.AddTransient<ProbabilisticCache>();
            services.AddTransient<SimpleCache>();
            services.AddScoped<InfoRepository>();
            services.AddScoped<ApplicationContext>();
        }

        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            T GetSetting<T>(IServiceProvider serviceProvider)
            {
                return serviceProvider.GetService<IConfiguration>().GetSection(typeof(T).Name).Get<T>();
            }
            
            services.AddTransient(GetSetting<DatabaseSettings>);
            services.AddTransient(GetSetting<CacheSettings>);
        }
    }
}
