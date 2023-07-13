using Microsoft.Extensions.Configuration;
using RecipesCostManagement.Contracts.DomainEntities;
using RecipesCostManagement.Contracts.RepositoryContracts;
using RecipesCostManagement.InfrastructureData.Dto;
using RecipesCostManagement.InfrastructureData.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.InfrastructureData.RepositoryImplementations
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _rootPath;

        public RecipeRepository(IConfiguration cofiguration)
        {
            _rootPath = cofiguration.GetSection("ApiCalls:Recipe").Value; 
        }
        public async Task<List<Recipe>> GetRecipesList()
        {
            List<RecipeDto> recipeDto = await RepositoryHelper.GetList<RecipeDto>(_rootPath);

            if (recipeDto.Count == 0) return new List<Recipe>();

            return TransformToEntityList(recipeDto);

        }

        private List<Recipe> TransformToEntityList(List<RecipeDto> recipesDto)
        {

            List<Recipe> recipes = new List<Recipe>();

            foreach (RecipeDto recipeDto in recipesDto)
            {
                recipes.Add(

                    new Recipe
                    {
                        Name = recipeDto.Name,
                        NeedIngredients = recipeDto.NeedIngredients,
                        TimeMinCooking = recipeDto.TimeCooking * 60
                        
                    }
                );
            }

            return recipes;

        }
    }
}
