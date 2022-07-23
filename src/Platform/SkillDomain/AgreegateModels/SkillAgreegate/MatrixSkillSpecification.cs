using AppShareServices.Queries.Specification;
using SkillDomain.AgreegateModels.SkillAgreegate;
using System.Linq.Expressions;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class MatrixSkillSpecification : SpecificationBase<MatrixSkill>
    {
        public MatrixSkillSpecification(bool isInclude, int skillId, int? specificationSkillLevelId, int? workerSkillLevelId) : base(isInclude)
        {
            if (skillId > 0)
            {
                Expression<Func<MatrixSkill, bool>> criteria = c => c.SkillId == skillId;
                AddCriteria(criteria);
            }
            else
            {
                return;
            }

            if (specificationSkillLevelId is not null && specificationSkillLevelId > 0)
            {
                Expression<Func<MatrixSkill, bool>> criteria = c => c.SpecificationSkillLevelId == specificationSkillLevelId;
                AddCriteria(criteria);
            }

            if (workerSkillLevelId is not null && workerSkillLevelId > 0)
            {
                Expression<Func<MatrixSkill, bool>> criteria = c => c.WorkerSkillLevelId == workerSkillLevelId;
                AddCriteria(criteria);
            }

            // include related entity
            if (IsInclude)
            {
                AddInclude(w => w.Skill);
                AddInclude(w => w.WorkerSkillLevel);
                AddInclude(w => w.SpecificationSkillLevel);
                AddInclude(w => w.Action);
                AddInclude(w => w.Result);
            }

        }
    }
}
