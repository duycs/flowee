using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using System.Linq.Expressions;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class JobSpecification : SpecificationBase<Job>
    {
        public JobSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Job, bool>> criteria = c =>
                    searchValue.Contains(c.Description.ToLower() ?? "");

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders is not null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Job.ProductId):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.ProductId);
                            }
                            else
                            {
                                AddOrderBy(w => w.ProductId);
                            }

                            break;

                        case nameof(Job.JobStatus):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.JobStatus);
                            }
                            else
                            {
                                AddOrderBy(w => w.JobStatus);
                            }

                            break;

                        case nameof(Job.StartTime):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.StartTime);
                            }
                            else
                            {
                                AddOrderBy(w => w.StartTime);
                            }

                            break;

                        case nameof(Job.EndTime):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.EndTime);
                            }
                            else
                            {
                                AddOrderBy(w => w.EndTime);
                            }

                            break;

                        case nameof(Job.Operations):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Operations.OrderByDescending(o => o.OrderNumber));
                            }
                            else
                            {
                                AddOrderBy(w => w.Operations.OrderBy(o => o.OrderNumber));
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
                AddInclude(j => j.JobStatus);
                AddInclude($"{nameof(Job.Operations)}.{nameof(Operation.Steps)}.{nameof(Step.StepStatus)}");
                AddInclude($"{nameof(Job.Operations)}.{nameof(Operation.StructType)}");
            }

        }
    }
}