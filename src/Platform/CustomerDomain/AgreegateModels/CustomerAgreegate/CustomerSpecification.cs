using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using System.Linq.Expressions;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class CustomerSpecification : SpecificationBase<Customer>
    {
        public CustomerSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Customer, bool>> criteria = c =>
                searchValue.Contains(c.Email.ToLower()) || searchValue.Contains(c.FullName.ToLower() ?? "")
                || searchValue.Contains(c.Code.ToLower())
                || searchValue.Contains(c.LastName.ToLower() ?? "") || searchValue.Contains(c.FirstName.ToLower() ?? "")
                || searchValue.Contains(c.Description.ToLower());

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders != null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Customer.FullName):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.FullName ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.FullName ?? "");
                            }

                            break;

                        case nameof(Customer.FirstName):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.FirstName ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.FirstName ?? "");
                            }

                            break;

                        case nameof(Customer.LastName):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.LastName ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.LastName ?? "");
                            }

                            break;

                        case nameof(Customer.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code);
                            }
                            else
                            {
                                AddOrderBy(w => w.Code);
                            }

                            break;

                        case nameof(Customer.Email):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Email);
                            }
                            else
                            {
                                AddOrderBy(w => w.Email);
                            }

                            break;

                        case nameof(Customer.PriorityLevel):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.PriorityLevel);
                            }
                            else
                            {
                                AddOrderBy(w => w.PriorityLevel);
                            }

                            break;

                        case nameof(Customer.Currency):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Currency);
                            }
                            else
                            {
                                AddOrderBy(w => w.Currency);
                            }

                            break;

                        case nameof(Customer.DateCreated):
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
                AddInclude(w => w.PaymentMethods);
                AddInclude(w => w.Currency);
                AddInclude(w => w.PriorityLevel);
            }

        }
    }
}
