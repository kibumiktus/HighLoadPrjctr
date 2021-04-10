using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Settings
{
    public class CacheSettings
    {
        public enum CacheType
        {
            Empty = 1,
            Probabilistic = 2,
            Simple = 3
        }
        public int CacheLifeTimeInMinutes { get; set; }
        public CacheType CacheStrategy { get; set; }
    }
    
}
