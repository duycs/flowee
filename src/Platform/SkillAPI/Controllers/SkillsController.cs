using AppShareDomain.DTOs;
using AppShareDomain.Models;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using Microsoft.AspNetCore.Mvc;
using SkillApplication.Services;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;

        public SkillsController(ISkillService skillService, IRepositoryService repositoryService, IMappingService mappingService)
        {
            _skillService = skillService;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int[]? ids, [FromQuery] bool isInclude = true)
        {
            var skillDtos = _skillService.FindSkills(ids, isInclude);
            return Ok(skillDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromQuery] bool isInclude = true)
        {
            var skillDto = _skillService.FindSkill(id, isInclude);
            return Ok(skillDto);
        }

        [HttpGet("worker-level-skills")]
        public IActionResult GetWorkerLevelSkills([FromQuery] int[]? ids, [FromQuery] bool isInclude = true)
        {
            var workerSkillLevels = _repositoryService.List<WorkerSkillLevel>(ids);
            var workerSkillLevelDtos = _mappingService.Map<List<EnumerationDto>>(workerSkillLevels);
            return Ok(workerSkillLevelDtos);
        }

        [HttpGet("matrix-skills")]
        public IActionResult GetMatchingMatrixSkill([FromQuery] int skillId, [FromQuery] int? workerSkillLevelId, [FromQuery] int? specificationLevelId)
        {
            var matrixSkills = _skillService.FindMatrixSkill(skillId, workerSkillLevelId, specificationLevelId);
            return Ok(matrixSkills);
        }
    }
}