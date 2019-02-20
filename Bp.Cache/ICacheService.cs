using System;
using System.Threading.Tasks;

namespace Bp.Cache
{
	public interface ICacheService
	{
		T GetOrCreate<T>(string cacheKey, Func<Task<T>> createTaskFunc);
		T GetOrCreate<T>(string cacheKey, Func<Task<T>> createTaskFunc, int cacheInterval);
	}
}