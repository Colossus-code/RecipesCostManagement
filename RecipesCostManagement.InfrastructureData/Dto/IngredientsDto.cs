using System.Text.Json.Serialization;

namespace RecipesCostManagement.InfrastructureData.Dto
{
    public class IngredientsDto
    {
        [JsonPropertyName("nombre")]
        public string ProductName { get; set; }

        [JsonPropertyName("precio")]
        public decimal Price { get; set; }    
    }
}