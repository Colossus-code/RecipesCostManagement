using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesCostManagement.Contracts.DomainEntities
{
    public class RecipeCost
    {
        public string RecipeName { get; set; }
        public decimal RequieredCost { get; set; }
    }
}
