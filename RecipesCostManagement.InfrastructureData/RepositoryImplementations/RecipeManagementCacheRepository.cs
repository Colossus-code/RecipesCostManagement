using Microsoft.Extensions.Caching.Memory;
using RecipesCostManagement.Contracts.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.InfrastructureData.RepositoryImplementations
{
    public class RecipeManagementCacheRepository : IRecipeManagementCacheRepository
    {

        private readonly IMemoryCache _memoryCache;

        public RecipeManagementCacheRepository(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;

        }
        public T GetCache<T>(string recipeName)
        {
            var response = _memoryCache.Get(recipeName);

            return (T)response;
        }

        public void SetCache<T>(string userName, T cachedObject) 
        {
            _memoryCache.Set(userName, cachedObject);
        }
    }
}
