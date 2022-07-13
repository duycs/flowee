using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using System.Linq.Expressions;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class SpecificationSpecification : SpecificationBase<Specification>
    {
        public SpecificationSpecification(bool isInclude, string? searchValue = "", int[]? ids = null, List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // find by ids
            if (ids != null && ids.Any())
            {
                AddCriteria(c => ids.Contains(c.Id));
            }

            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Specification, bool>> criteria = c =>
                    searchValue.Contains(c.Code.ToLower()) || searchValue.Contains(c.Name.ToLower() ?? "") || searchValue.Contains(c.Instruction.ToLower() ?? "");

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders != null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Specification.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Code ?? "");
                            }

                            break;

                        case nameof(Specification.Name):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Name ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Name ?? "");
                            }

                            break;

                        case nameof(Specification.DateCreated):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.DateCreated);
                            }
                            else
                            {
                                AddOrderBy(w => w.DateCreated);
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
                AddInclude($"{nameof(Specification.Rules)}.{nameof(Rule.Condition)}");
                AddInclude($"{nameof(Specification.Rules)}.{nameof(Rule.Setting)}");
                AddInclude($"{nameof(Specification.Rules)}.{nameof(Rule.Operator)}");
            }

        }
    }
}