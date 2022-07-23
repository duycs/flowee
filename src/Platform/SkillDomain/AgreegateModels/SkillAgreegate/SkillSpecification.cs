using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using System.Linq.Expressions;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class SkillSpecification : SpecificationBase<Skill>
    {
        public SkillSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Skill, bool>> criteria = c =>
                searchValue.Contains(c.Code.ToLower()) || searchValue.Contains(c.Name.ToLower() ?? "") || searchValue.Contains(c.Description.ToLower() ?? "");

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders is not null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Skill.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Code ?? "");
                            }

                            break;

                        case nameof(Skill.Name):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Name ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Name ?? "");
                            }

                            break;

                        default:
                            AddOrderByDescending(w => w.DateCreated);
                            break;
                    }
                }
            }

            // include related entity
            if (IsInclude)
            {
                AddInclude(w => w.MatrixSkills);
            }

        }
    }
}
