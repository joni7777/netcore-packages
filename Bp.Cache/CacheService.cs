using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Bp.Cache
{
	public class CacheService : ICacheService
	{
		private readonly int _cacheInterval;
		private readonly IMemoryCache _cache;
		private static ConcurrentDictionary<string, object> _keyLock;

		public CacheService(IMemoryCache cache, IConfiguration config, int cacheInterval)
		{
			_cache = cache;
			_cacheInterval = config.GetValue<int>("Data:CacheInterval");
			_keyLock = new ConcurrentDictionary<string, object>();
		}

		public T GetOrCreate<T>(string cacheKey, Func<Task<T>> createTaskFunc) =>
			GetOrCreate(cacheKey, createTaskFunc, _cacheInterval);

		public T GetOrCreate<T>(string cacheKey, Func<Task<T>> createTaskFunc, int cacheInterval)
		{
			if (_cache.TryGetValue<T>(cacheKey, out var result))
			{
				return result;
			}

			lock (_keyLock.GetOrAdd(cacheKey, new Guid()))
			{
				if (_cache.TryGetValue<T>(cacheKey, out result))
				{
					return result;
				}

				var createTask = createTaskFunc.Invoke();
				createTask.Wait();
				_cache.Set(cacheKey, createTask.Result, TimeSpan.FromMinutes(cacheInterval));
				return _cache.Get<T>(cacheKey);
			}
		}
	}
}