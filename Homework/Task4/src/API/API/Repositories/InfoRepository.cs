using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using API.Cache;
using System;
using System.Threading;

namespace API.Repositories
{
    public class InfoRepository
    {
        private ICache _cache;
        private IServiceProvider _serviceProvider;

        public InfoRepository(ICache cache, IServiceProvider serviceProvider)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
        }

        public async Task Store(Info info)
        {
            ApplicationContext applicationContext = _serviceProvider.GetService<ApplicationContext>();

            applicationContext.Add(info);
            await applicationContext.SaveChangesAsync();
        }

        private object _getCountLocker = new object();
        public async Task<int> GetCount()
        {
            object cached;
            if ((cached = _cache["Count"]) != null)
                return (int)cached;
            lock (_getCountLocker)
            {
                if ((cached = _cache["Count"]) != null)
                    return (int)cached;
                ApplicationContext applicationContext = _serviceProvider.GetService<ApplicationContext>();
                
                var count = applicationContext.Info.Count();
                _cache.Add("Count", count);
                return count;
            }
            
        }

        private object _getLastLocker = new object();
        public async Task<Info> GetLastAsync()
        {
            object cached;
            if ((cached = _cache["LastElement"]) != null)
                return (Info)cached;
            lock (_getLastLocker)
            {
                if ((cached = _cache["LastElement"]) != null)
                    return (Info)cached;
                ApplicationContext applicationContext = _serviceProvider.GetService<ApplicationContext>();
                var last = applicationContext.Info.OrderByDescending(x => x.Created).First();
                _cache.Add("LastElement", last);
                return last;
            }
        }
    }
}
