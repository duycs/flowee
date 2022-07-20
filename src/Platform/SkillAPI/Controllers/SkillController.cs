using Microsoft.AspNetCore.Mvc;
using SkillApplication.Services;

namespace SkillAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("{skillId}")]
        public IActionResult GetSkill(int skillIds)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetSkills([FromQuery] int skillIds)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMatchingMatrixSkill([FromQuery] int skillId, [FromQuery] int workerSkillLevelId, [FromQuery] int specificationLevelId)
        {
            var matchingMatrixSkill = _skillService.FindMatchingMatrixSkill(skillId, workerSkillLevelId, specificationLevelId);
            return Ok(matchingMatrixSkill);
        }
    }
}