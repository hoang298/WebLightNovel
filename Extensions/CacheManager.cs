using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
namespace WebLightNovel.Extensions
{

    public class CacheManager
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;
        private static readonly object _lockObject = new object();

        private static CacheManager _instance;

        public static CacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheManager();
                        }
                    }
                }
                return _instance;
            }
        }

        // Phương thức để lấy ObjectCache
        public ObjectCache GetCache()
        {
            return _cache;
        }

        public void SetCache(string key, object value, DateTimeOffset absoluteExpiration)
        {
            _cache.Set(key, value, absoluteExpiration);
        }
    }

}