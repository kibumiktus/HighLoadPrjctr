using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using API.Cache;

namespace API.Repositories
{
    public class InfoRepository
    {
        private ApplicationContext _applicationContext;
        private ICache _cache;

        public InfoRepository(ApplicationContext databaseSettings, ICache cache)
        {
            _applicationContext = databaseSettings;
            _cache = cache;
        }

        public async Task Store(Info info)
        {
            _applicationContext.Add(info);
            await _applicationContext.SaveChangesAsync();
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
                var count = _applicationContext.Info.Count();
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
                var last = _applicationContext.Info.OrderByDescending(x => x.Created).First();
                _cache.Add("LastElement", last);
                return last;
            }
        }
    }
}
