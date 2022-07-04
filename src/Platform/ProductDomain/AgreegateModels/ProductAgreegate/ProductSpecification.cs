using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using ProductDomain.AgreegateModels.ProductAgreegate;
using System.Linq.Expressions;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class ProductSpecification : SpecificationBase<Product>
    {
        public ProductSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Product, bool>> criteria = c =>
                searchValue.Contains(c.Code.ToLower()) || searchValue.Contains(c.Name.ToLower() ?? "")
                || searchValue.Contains(c.Description.ToLower() ?? "") || searchValue.Contains(c.ProductLevel.ToString().ToLower() ?? ""));

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders != null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Product.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Code ?? "");
                            }

                            break;

                        case nameof(Product.Name):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Name ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Name ?? "");
                            }

                            break;

                        case nameof(Product.ProductLevel):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.ProductLevel);
                            }
                            else
                            {
                                AddOrderBy(w => w.ProductLevel);
                            }

                            break;

                        case nameof(Product.PriceStandar):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.PriceStandar);
                            }
                            else
                            {
                                AddOrderBy(w => w.PriceStandar);
                            }

                            break;

                        case nameof(Product.Price):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Price);
                            }
                            else
                            {
                                AddOrderBy(w => w.Price);
                            }

                            break;

                        case nameof(Product.DateCreated):
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
                AddInclude(w => w.Categories);
                AddInclude(w => w.Addons);
                AddInclude(w => w.ProductLevel);
            }

        }
    }
}
