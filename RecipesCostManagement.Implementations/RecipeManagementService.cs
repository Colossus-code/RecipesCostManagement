
using RecipesCostManagement.Contracts.DomainEntities;
using RecipesCostManagement.Contracts.RepositoryContracts;
using RecipesCostManagement.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.Implementations
{
    public class RecipeManagementService : IRecipeManagementService
    {
        private readonly IRecipeCostForMinutRepository _repositoryCost;
        private readonly IRecipeIngredientsRepository _repositoryIngredients;
        private readonly IRecipeManagementCacheRepository _repositoryCahe;
        private readonly IRecipeRepository _repositoryRecipe;

        public RecipeManagementService(IRecipeCostForMinutRepository repoCost, IRecipeIngredientsRepository repoIngredients, IRecipeManagementCacheRepository repoCache, IRecipeRepository repoRrecipe)
        {
            _repositoryCost = repoCost;
            _repositoryIngredients = repoIngredients;
            _repositoryCahe = repoCache;
            _repositoryRecipe = repoRrecipe;
        }
        public async Task<RecipeCost> GetRecipeCostByRecipeName(string recipeName)
        {
            RecipeCost recipeCost = _repositoryCahe.GetCache<RecipeCost>(recipeName);

            if(recipeCost != null) return recipeCost;

            List<Recipe> recipes = await _repositoryRecipe.GetRecipesList();

            Recipe choosenRecipe = GetRecipe(recipeName, recipes);

            if (choosenRecipe == null) return null;

            List<Ingredients> ingredients = await _repositoryIngredients.GetIngredients();

            CostForMinut costForMinut = await _repositoryCost.GetCost();

            List<Ingredients> choosenIngedients = SelectIngredientsForRecipe(choosenRecipe, ingredients);

            return GetRecipeCost(choosenIngedients, choosenRecipe, costForMinut);
        }


        private Recipe GetRecipe(string recipeName, List<Recipe> recipes)
        {
            Recipe selectedRecipe = recipes.FirstOrDefault(e => e.Name == recipeName);

            if (selectedRecipe == null) return null;

            return selectedRecipe;
        }
        private List<Ingredients> SelectIngredientsForRecipe(Recipe choosenRecipe, List<Ingredients> allIngredients)
        {
            List<Ingredients> choosenIngredients = new List<Ingredients>();

            foreach(string ingredientsName in choosenRecipe.NeedIngredients.Keys)
            {
                Ingredients ingredient = allIngredients.FirstOrDefault(e => e.ProductName == ingredientsName);

                choosenIngredients.Add(ingredient);
            }

            return choosenIngredients;

        }
        private RecipeCost GetRecipeCost(List<Ingredients> choosenIngedients, Recipe choosenRecipe, CostForMinut costForMinut)
        {
            RecipeCost recipeTotalCost = new RecipeCost();

            foreach(string ingredientName in choosenRecipe.NeedIngredients.Keys)
            {
                decimal amountOfIngredientNeeded = choosenRecipe.NeedIngredients.FirstOrDefault(e => e.Key == ingredientName).Value; 
                decimal costForIngredient = choosenIngedients.FirstOrDefault(e => e.ProductName == ingredientName).Price * amountOfIngredientNeeded;

                recipeTotalCost.RequieredCost += costForIngredient; 
            }

            recipeTotalCost.RequieredCost += choosenRecipe.TimeMinCooking * costForMinut.Cost; 

            recipeTotalCost.RecipeName = choosenRecipe.Name;    

            _repositoryCahe.SetCache<RecipeCost>(choosenRecipe.Name, recipeTotalCost);

            return recipeTotalCost;
        }
    }
}
