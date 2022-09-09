using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Skill;
using AppShareServices.Pagging;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;

namespace AppShareServices.Services
{
    public class SkillClientService : ISkillClientService
    {
        private readonly HttpClient _httpClient;

        public SkillClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"https://localhost:7234/skills/");
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<SkillDto> Get(int id, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<SkillDto>($"{id}?isInclude={isInclude}");
        }

        public async Task<IEnumerable<SkillDto>> Get(int[] ids, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SkillDto>>($"?{ids.ToIdsQueries(isInclude)}");
        }

        public async Task<IEnumerable<EnumerationDto>> GetWorkerSkillLevels(int[] ids, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EnumerationDto>>($"worker-skill-levels?{ids.ToIdsQueries(isInclude)}");
        }

        public async Task<IEnumerable<EnumerationDto>> GetSpecificationSkillLevels(int[] ids, bool isInclude)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EnumerationDto>>($"specification-skill-levels?{ids.ToIdsQueries(isInclude)}");
        }

        public async Task<IEnumerable<MatrixSkillDto>> GetMatrixSkills(int skillId, int? workerSkillLevelId, int? specificationLevelId, bool isInclude)
        {
            var keyValueParams = new Dictionary<string, string>();
            keyValueParams.Add("skillId", skillId.ToString());
            keyValueParams.Add("workerSkillLevelId", workerSkillLevelId?.ToString());
            keyValueParams.Add("specificationLevelId", specificationLevelId?.ToString());

            return await _httpClient.GetFromJsonAsync<IEnumerable<MatrixSkillDto>>($"matrix-skills?{keyValueParams.ToQueries(isInclude)}");
        }

        public Task<IEnumerable<SkillDto>> GetSkillsByOperations(Guid[] operationIds, bool? isInclude)
        {
            throw new NotImplementedException();
        }
    }
}
