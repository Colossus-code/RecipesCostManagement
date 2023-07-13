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
    public class RecipeCostForMinutRepository : IRecipeCostForMinutRepository
    {
        private readonly string _rootPath;

        public RecipeCostForMinutRepository(IConfiguration cofiguration)
        {
            _rootPath = cofiguration.GetSection("ApiCalls:CostForMinut").Value;
        }

        public async Task<CostForMinut> GetCost()
        {
            CostForMinutDto costForMinutDto = await RepositoryHelper.GetObject<CostForMinutDto>(_rootPath, null);

            if (costForMinutDto == null) return new CostForMinut();

            return TransformToEntityList(costForMinutDto);

        }

        private CostForMinut TransformToEntityList(CostForMinutDto costForMinutDto)
        {
            return new CostForMinut
            {
                Cost = costForMinutDto.CostForMinut
            };
        }
    }
}
