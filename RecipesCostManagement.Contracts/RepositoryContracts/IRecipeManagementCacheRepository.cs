using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.Contracts.RepositoryContracts
{
    public interface IRecipeManagementCacheRepository
    {
        T GetCache<T>(string recipeName);

        void SetCache<T>(string userName, T cachedObject);
    }
}
