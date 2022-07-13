using CatalogApplication.DTOs;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;

namespace CatalogApplication.ClientServices
{
    public class SpecificationClientService : ISpecificationClientService
    {
        private readonly HttpClient _httpClient;

        public SpecificationClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SpecificationDto> Get(int id, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<SpecificationDto>($"/{id}?isInclude={isInclude}");
        }

        public async Task<IEnumerable<SpecificationDto>> Get(int?[]? ids, bool isInclude)
        {
            var queryIds = "";
            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    queryIds.Concat($"ids={id}");
                }
            }

            return await _httpClient.GetFromJsonAsync<IEnumerable<SpecificationDto>>($"?{queryIds}&isInclude={isInclude}");
        }
    }
}
