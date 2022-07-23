using Microsoft.AspNetCore.Mvc;
using SkillApplication.Services;

namespace SkillAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
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

        [HttpGet("matrix-skills")]
        public IActionResult GetMatchingMatrixSkill([FromQuery] int skillId, [FromQuery] int? workerSkillLevelId, [FromQuery] int? specificationLevelId)
        {
            var matrixSkills = _skillService.FindMatrixSkill(skillId, workerSkillLevelId, specificationLevelId);
            return Ok(matrixSkills);
        }
    }
}