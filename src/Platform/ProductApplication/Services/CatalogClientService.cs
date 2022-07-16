using AppShareDomain.DTOs.Catalog;
using AppShareServices.Pagging;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;

namespace ProductApplication.Services
{
    public class CatalogClientService : ICatalogClientService
    {
        private readonly HttpClient _httpClient;

        public CatalogClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"https://localhost:7170/catalogs/");
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<CatalogDto> Get(int id, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<CatalogDto>($"{id}?isInclude={isInclude}");
        }

        public async Task<IEnumerable<CatalogDto>> Get(int[] ids, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CatalogDto>>($"?{ids.ToIdsQueries(isInclude)}");
        }
    }
}
