using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecipesCostManagement.Contracts.DomainEntities;
using RecipesCostManagement.Contracts.ServiceContracts;
using RecipesCostManagement.WebApi.Controllers;
using RecipesCostManagement.WebApi.Models;

namespace RecipesCostManagement.ControllerTestSuite
{
    public class ControllerUnitTest
    {
        private readonly RecipesController _recipesController;
        
        private readonly Mock<IRecipeManagementService> _serviceMock;

        private readonly Mock<ILogger<RecipesController>> _mockLogger = new Mock<ILogger<RecipesController>>(); 

        public ControllerUnitTest()
        {
            _serviceMock = new Mock<IRecipeManagementService>();
  
            _recipesController = new RecipesController(_serviceMock.Object, _mockLogger.Object);

        }

        [Fact]
        public async void Assert_CodeStatus200_When_NotRecipeFound()
        {
            //Arrange
            string recipeName = "Macarrones";

            _serviceMock.Setup(response => response.GetRecipeCostByRecipeName(recipeName)).ReturnsAsync(new RecipeCost());
            
            var expectedResult = 200;
            var expectedMessage = $"The recipe by name {recipeName} weren't found";
            //Act

            var actionResult = await _recipesController.GetRecipeByName(recipeName);

            ObjectResult response = actionResult as ObjectResult;
            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.True(response.Value.Equals(expectedMessage));

        }
        [Fact]
        public async void Assert_CodeStatus500_When_UnexpectedExceptionHasThrown()
        {

            //Arrange
            _serviceMock.Setup(response => response.GetRecipeCostByRecipeName(It.IsAny<string>())).Throws(new Exception());
            var expectedResult = 500;
            var expectedMessage = "Unexpected error happend.";
            //Act

            var actionResult = await _recipesController.GetRecipeByName(It.IsAny<string>());

            ObjectResult response = actionResult as ObjectResult;
            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.True(response.Value.Equals(expectedMessage));
        }
        [Fact]
        public async void Assert_CodeStatus200_When_EverythingGoneOkay()
        {

            //Arrange

            RecipeCost recipeCost = new RecipeCost
            {
                RecipeName = "Macarroncicos con tomatico.",
                RequieredCost = 3.28m
            };

            RecipeCostModel expectedMessage = new RecipeCostModel
            {
                RecipeName = recipeCost.RecipeName,
                CostRecipe = recipeCost.RequieredCost
            };

            _serviceMock.Setup(response => response.GetRecipeCostByRecipeName(It.IsAny<string>())).ReturnsAsync(recipeCost);

            var expectedResult = 200;
            //Act

            var actionResult = await _recipesController.GetRecipeByName("Macarroncicos con tomatico.");

            ObjectResult response = actionResult as ObjectResult;

            RecipeCostModel responseValue = response.Value as RecipeCostModel;

            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.NotNull(response.Value);
            Assert.True(responseValue.CostRecipe == expectedMessage.CostRecipe);
        }
    }
}