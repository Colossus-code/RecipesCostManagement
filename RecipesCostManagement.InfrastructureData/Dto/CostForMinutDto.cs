using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipesCostManagement.InfrastructureData.Dto
{
    public class CostForMinutDto
    {
        [JsonPropertyName("CostePorMinuto")]
        public decimal CostForMinut { get; set; }
    }
}
