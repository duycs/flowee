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
        public WorkerSpecification(Expression<Func<Worker, bool>> criteria, string searchValue, List<ColumnOrder> columnOrders) : base(criteria)
        {
            if (!string.IsNullOrEmpty(searchValue))
                AddInclude(w => searchValue.Contains(w.Email) || searchValue.Contains(w.Name));

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
                            default:
                                AddOrderByDescending(w => w.DateCreated);
                                break;

                        }
                    }
                }
            }
        }
    }
}
