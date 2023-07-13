using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesCostManagement.Contracts.DomainEntities;
using RecipesCostManagement.Contracts.ServiceContracts;
using RecipesCostManagement.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipesCostManagement.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeManagementService _recipeManagementService;
        private readonly ILogger<RecipesController> _logger;
        public RecipesController(IRecipeManagementService recipeManagement, ILogger<RecipesController> logger)
        {
            _recipeManagementService = recipeManagement;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetRecipe")]
        public async Task<IActionResult> GetRecipeByName([Required] string recipeName)
        {
            try
            {
                RecipeCost recipeCostByRecipeName = await _recipeManagementService.GetRecipeCostByRecipeName(recipeName);

                if (recipeCostByRecipeName == null || recipeCostByRecipeName.RecipeName == null)
                {
                    _logger.LogError($"Error - {StatusCodes.Status404NotFound} the product introduced by name {recipeName} doesn't exist.");
                    return Ok($"The recipe by name {recipeName} weren't found");

                }
                else
                {
                    RecipeCostModel response = new RecipeCostModel
                    {
                        RecipeName = recipeName,
                        CostRecipe = Math.Round(recipeCostByRecipeName.RequieredCost, 2)
                    };

                    return Ok(response);
                }
            }catch(Exception ex)
            {
                _logger.LogCritical($"Critical error happened. {ex.Message}");
                return StatusCode(500, "Unexpected error happend.");
            }
        }
    }
}
