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
    public class RecipeIngredientsRepository : IRecipeIngredientsRepository
    {
        private readonly string _rootPath;

        public RecipeIngredientsRepository(IConfiguration cofiguration)
        {
            _rootPath = cofiguration.GetSection("ApiCalls:Ingredients").Value;
        }

        public async Task<List<Ingredients>> GetIngredients()
        {
            List<IngredientsDto> ingredientsDto = await RepositoryHelper.GetList<IngredientsDto>(_rootPath);

            if (ingredientsDto.Count == 0) return new List<Ingredients>();

            return TransformToEntityList(ingredientsDto);

        }

        private List<Ingredients> TransformToEntityList(List<IngredientsDto> ingredientsDto)
        {
            List<Ingredients> ingredients = new List<Ingredients>();

            foreach(IngredientsDto ingredientDto in ingredientsDto)
            {
                ingredients.Add(

                    new Ingredients
                    {
                        ProductName = ingredientDto.ProductName,
                        Price = ingredientDto.Price
                    }
                );
            }

            return ingredients; 
        }
    }
}
