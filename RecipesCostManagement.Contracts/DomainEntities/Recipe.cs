using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipesCostManagement.Contracts.DomainEntities
{
    public class Recipe
    {

        public string Name { get; set; }

        public Dictionary<string, decimal> NeedIngredients { get; set; }

        public decimal TimeMinCooking { get; set; }
    }
}
