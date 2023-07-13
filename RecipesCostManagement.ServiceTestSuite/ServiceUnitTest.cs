using Moq;
using RecipesCostManagement.Contracts.DomainEntities;
using RecipesCostManagement.Contracts.RepositoryContracts;
using RecipesCostManagement.Implementations;

namespace RecipesCostManagement.ServiceTestSuite
{
    public class ServiceUnitTest
    {
        private readonly Mock<IRecipeCostForMinutRepository> _repositoryCostMock;
        private readonly Mock<IRecipeIngredientsRepository> _repositoryIngredientsMock;
        private readonly Mock<IRecipeManagementCacheRepository> _repositoryCacheMock;
        private readonly Mock<IRecipeRepository> _repositoryRecipeMock;

        private readonly RecipeManagementService _recipeManagementService;
        public ServiceUnitTest()
        {
            _repositoryCacheMock = new Mock<IRecipeManagementCacheRepository>();
            _repositoryCostMock = new Mock<IRecipeCostForMinutRepository>();
            _repositoryIngredientsMock = new Mock<IRecipeIngredientsRepository>();
            _repositoryRecipeMock = new Mock<IRecipeRepository>();

            _recipeManagementService = new RecipeManagementService(_repositoryCostMock.Object, _repositoryIngredientsMock.Object, _repositoryCacheMock.Object, _repositoryRecipeMock.Object);

        }
        [Fact]
        public async void Assert_NotNull_When_ProductsFoundInCache()
        {

            //Arrange
            RecipeCost recipeInCache = GetRecipe();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns(recipeInCache);

            //Act

            RecipeCost response = await _recipeManagementService.GetRecipeCostByRecipeName(recipeInCache.RecipeName);

            //Assert

            Assert.NotNull(response);

        }
        [Fact]
        public async void Assert_PriceEqualAsExpectedOnes_When_ProductsFoundInCache()
        {

            //Arrange
            RecipeCost recipeInCache = GetRecipe();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns(recipeInCache);

            //Act

            RecipeCost response = await _recipeManagementService.GetRecipeCostByRecipeName(recipeInCache.RecipeName);

            //Assert

            Assert.Equal(response.RequieredCost, recipeInCache.RequieredCost);

        }
        [Fact]
        public async void Assert_NotNull_When_RecipeFound()
        {

            //Arrange
            var expectedRecipe = GetRecipe();

            var ingredients = GetProductIngredients();

            var recipes = GetRecipes();

            var cost = GetCost();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns((RecipeCost) null);

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(recipes);

            _repositoryIngredientsMock.Setup(e => e.GetIngredients()).ReturnsAsync(ingredients);

            _repositoryCostMock.Setup(e => e.GetCost()).ReturnsAsync(cost);  

            //Act

            var response = await _recipeManagementService.GetRecipeCostByRecipeName(expectedRecipe.RecipeName);

            //Assert

            Assert.NotNull(response);

        }
        [Fact]
        public async void Assert_PricesAreEquals_When_RecipeIsWhatWeExpected()
        {

            //Arrange
            var expectedRecipe = GetRecipe();

            var ingredients = GetProductIngredients();

            var recipes = GetRecipes();

            var cost = GetCost();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns((RecipeCost)null);

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(recipes);

            _repositoryIngredientsMock.Setup(e => e.GetIngredients()).ReturnsAsync(ingredients);

            _repositoryCostMock.Setup(e => e.GetCost()).ReturnsAsync(cost);
            //Act

            var response = await _recipeManagementService.GetRecipeCostByRecipeName(expectedRecipe.RecipeName);

            //Assert

            Assert.Equal(response.RequieredCost, expectedRecipe.RequieredCost);

        }

        [Fact]
        public async void Assert_NotEqual_When_RecipeNotMaches()
        {

            //Arrange

            var notExpectedRecipe = GetRecipe();
            
            var recipes = GetRecipes();

            var ingredients = GetProductIngredients();

            var cost = GetCost();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns((RecipeCost)null);

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(recipes);

            _repositoryIngredientsMock.Setup(e => e.GetIngredients()).ReturnsAsync(ingredients);

            _repositoryCostMock.Setup(e => e.GetCost()).ReturnsAsync(cost);

            //Act

            var response = await _recipeManagementService.GetRecipeCostByRecipeName("Ensalada");

            //Assert

            Assert.NotEqual(response.RequieredCost , notExpectedRecipe.RequieredCost);

        }

        [Fact]
        public async void Assert_Null_When_NotFoundRecipes()
        {

            //Arrange
            var recipeName = GetRecipe().RecipeName;

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns((RecipeCost)null);

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(new List<Recipe>());

            //Act

            var response = await _recipeManagementService.GetRecipeCostByRecipeName(recipeName);

            //Assert

            Assert.Null(response);

        }

        [Fact]
        public async void Assert_Exception_When_NotFoundRecipeListRootPath()
        {

            //Arrange
            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns(new RecipeCost());

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).Throws(new Exception());

            //Act && Assert

            Assert.ThrowsAsync<Exception>(() => _recipeManagementService.GetRecipeCostByRecipeName(It.IsAny<string>()));
        }
        [Fact]
        public async void Assert_Exception_When_NotFoundIngredientsListRootPath()
        {

            //Arrange

            var recipes = GetRecipes();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns(new RecipeCost());

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(recipes);

            _repositoryIngredientsMock.Setup(e => e.GetIngredients()).Throws(new Exception());  

            //Act && Assert

            Assert.ThrowsAsync<Exception>(() => _recipeManagementService.GetRecipeCostByRecipeName(It.IsAny<string>()));

        }
        public async void Assert_Exception_When_NotFoundCostListRootPath()
        {

            //Arrange

            var recipes = GetRecipes();

            var ingredients = GetProductIngredients();

            _repositoryCacheMock.Setup(e => e.GetCache<RecipeCost>(It.IsAny<string>())).Returns(new RecipeCost());

            _repositoryRecipeMock.Setup(e => e.GetRecipesList()).ReturnsAsync(recipes);

            _repositoryIngredientsMock.Setup(e => e.GetIngredients()).ReturnsAsync(ingredients);

            _repositoryCostMock.Setup(e => e.GetCost()).Throws(new Exception());

            //Act && Assert

            Assert.ThrowsAsync<Exception>(() => _recipeManagementService.GetRecipeCostByRecipeName(It.IsAny<string>()));

        }

        private RecipeCost GetRecipe()
        {
            return new RecipeCost
            {
                RecipeName = "Macarroncicos con tomate",
                RequieredCost = 5.7175m
            };
        }

        private List<Recipe> GetRecipes()
        {
            List<Recipe> recipes = new List<Recipe>
            {
                new Recipe
                {
                    Name = "Macarroncicos con tomate",
                    TimeMinCooking = 25,
                    NeedIngredients = new Dictionary<string, decimal>()

                },
                new Recipe
                {
                    Name = "Ensalada",
                    TimeMinCooking = 5,
                    NeedIngredients = new Dictionary<string, decimal>()
                }

            };

            var recipe = recipes.FirstOrDefault();

            recipe.NeedIngredients.Add("Pasta", 0.5m);
            recipe.NeedIngredients.Add("Tomate", 0.25m);
            recipe.NeedIngredients.Add("Cebolla", 0.75m);

            var otherRecipe = recipes.Last();

            otherRecipe.NeedIngredients.Add("Tomate", 0.25m);
            otherRecipe.NeedIngredients.Add("Cebolla", 0.75m);

            return recipes;
        }

        private List<Ingredients> GetProductIngredients()
        {
            return new List<Ingredients>
            {
                new Ingredients
                {
                    ProductName = "Pasta",
                    Price = 2.99m
                },                
                new Ingredients
                {
                    ProductName = "Tomate",
                    Price = 0.75m
                },                
                new Ingredients
                {
                    ProductName = "Cebolla",
                    Price = 2.38m
                },
            };
        }

        private CostForMinut GetCost()
        {
            return new CostForMinut
            {
                Cost = 0.09m
            };
        }
    }
}