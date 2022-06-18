using AppShareServices.Models;
using AppShareServices.Queries.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerSpecification : SpecificationBase<Worker>
    {
        public WorkerSpecification(Expression<Func<Worker, bool>> criteria, bool isInclude, string? searchValue, List<ColumnOrder>? columnOrders) : base(criteria)
        {
            //if (!string.IsNullOrEmpty(searchValue))
            //    criteria.(w => searchValue.Contains(w.Email) || searchValue.Contains(w.FullName));

            var fieldNames = typeof(Worker).GetMembers().Select(m => m.Name);

            if (columnOrders.Any())
            {
                foreach (var fieldName in fieldNames)
                {
                    var columnOrder = columnOrders.Where(c => fieldNames.Contains(c.Name)).FirstOrDefault();
                    if (columnOrder != null)
                    {
                        switch (columnOrder.Name)
                        {
                            // TODO: order by clumns

                            default:
                                AddOrderByDescending(w => w.DateCreated);
                                break;

                        }
                    }
                }
            }

            if ((bool)isInclude)
            {
                AddInclude(w => w.Role);
            }

        }
    }
}
