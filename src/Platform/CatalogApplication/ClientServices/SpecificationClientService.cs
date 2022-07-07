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

        public async Task<SpecificationDto> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<SpecificationDto>($"/{id}");
        }

        public async Task<IEnumerable<SpecificationDto>> Get(int?[]? ids)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SpecificationDto>>($"?ids={string.Join(",", ids)}");
        }
    }
}
