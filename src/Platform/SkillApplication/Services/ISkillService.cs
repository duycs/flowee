using AppShareDomain.DTOs.Skill;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public interface ISkillService
    {
        public SkillDto FindSkill(int skillId, bool isInclude = false);
        public List<SkillDto> FindSkills(int[] skillIds, bool isInclude = false);
    }
}
