using RecipesCostManagement.Contracts.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.Contracts.RepositoryContracts
{
    public interface IRecipeIngredientsRepository
    {
        Task<List<Ingredients>> GetIngredients();
    }
}
