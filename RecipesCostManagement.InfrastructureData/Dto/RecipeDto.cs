using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipesCostManagement.InfrastructureData.Dto
{
    public class RecipeDto
    {
        [JsonPropertyName("nombre")]
        public string Name { get; set; }

        [JsonPropertyName("ingredientes")]
        public Dictionary<string, decimal> NeedIngredients { get; set; }

        [JsonPropertyName("horasCocinado")]
        public decimal TimeCooking { get; set; }
    }
}
