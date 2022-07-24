using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Skill;
using Refit;

namespace WorkerApplication.Services
{
    public interface ISkillClientService
    {
        [Get("/skills/{id}")]
        public Task<SkillDto> Get(int id, bool isInclude);

        [Get("/skills")]
        public Task<IEnumerable<SkillDto>> Get(int[] ids, bool isInclude);

        [Get("/skills/workerSkillLevels")]
        public Task<IEnumerable<EnumerationDto>> GetSkillLevels(int[] ids, bool isInclude);
    }
}
