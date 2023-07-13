using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipesCostManagement.Contracts.DomainEntities
{
    public class Ingredients
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }
    }
}
