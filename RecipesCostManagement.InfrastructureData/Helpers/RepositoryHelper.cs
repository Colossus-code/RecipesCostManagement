using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipesCostManagement.InfrastructureData.Helpers
{
    public static class RepositoryHelper
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<List<T>> GetList<T>(string rootPath)
        {
            HttpResponseMessage get = await httpClient.GetAsync(rootPath); 

            string responseAsString = await get.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseAsString)) return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(responseAsString);
        }
        public static async Task<T> GetObject<T>(string rootPath, T defaultValue)
        {
            HttpResponseMessage get = await httpClient.GetAsync(rootPath);

            string responseAsString = await get.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseAsString)) return defaultValue;

            return JsonSerializer.Deserialize<T>(responseAsString);
        }
    }
}
