using AppShareDomain.DTOs.Specification;
using AppShareServices.Pagging;
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
            _httpClient.BaseAddress = new Uri($"https://localhost:7174/Specifications/");
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<SpecificationDto> Get(int id, bool isInclude)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SpecificationDto>($"{id}?isInclude={isInclude}") ?? new SpecificationDto();
            }
            catch (TaskCanceledException taskCanceledException)
            {
                return new SpecificationDto();
            }
        }

        public async Task<IEnumerable<SpecificationDto>> Get(int[] ids, bool isInclude)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<SpecificationDto>>($"?{ids.ToIdsQueries(isInclude)}") ?? new List<SpecificationDto>();
            }
            catch (TaskCanceledException taskCanceledException)
            {
                return new List<SpecificationDto>();
            }
        }
    }
}
