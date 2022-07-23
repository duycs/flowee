using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using System.Linq.Expressions;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerSpecification : SpecificationBase<Worker>
    {
        public WorkerSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Worker, bool>> criteria = c =>
                searchValue.Contains(c.Email.ToLower()) || searchValue.Contains(c.FullName ?? "") || searchValue.Contains(c.Code.ToLower());

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders is not null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Worker.FullName):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.FullName ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.FullName ?? "");
                            }

                            break;

                        case nameof(Worker.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code);
                            }
                            else
                            {
                                AddOrderBy(w => w.Code);
                            }

                            break;

                        case nameof(Worker.Email):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Email);
                            }
                            else
                            {
                                AddOrderBy(w => w.Email);
                            }

                            break;

                        case nameof(Worker.DateCreated):
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
                AddInclude(w => w.Roles);
                AddInclude(w => w.Groups);
                AddInclude(w => w.WorkerSkills);
            }

        }
    }
}
