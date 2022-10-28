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

        [HttpGet("worker-skill-levels")]
        public IActionResult GetWorkerLevelSkills([FromQuery] int[]? ids)
        {
            var workerSkillLevels = _repositoryService.List<WorkerSkillLevel>(ids);
            var workerSkillLevelDtos = _mappingService.Map<List<EnumerationDto>>(workerSkillLevels);
            return Ok(workerSkillLevelDtos);
        }

        [HttpGet("specification-skill-levels")]
        public IActionResult GetSpecificationLevelSkills([FromQuery] int[]? ids)
        {

            var specificationSkillLevels = _repositoryService.List<SpecificationSkillLevel>(ids);
            var specificationSkillLevelDtos = _mappingService.Map<List<EnumerationDto>>(specificationSkillLevels);
            return Ok(specificationSkillLevelDtos);
        }

    }
}