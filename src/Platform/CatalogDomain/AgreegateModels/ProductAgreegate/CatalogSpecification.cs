using AppShareDomain.Models;
using AppShareServices.Pagging;
using AppShareServices.Queries.Specification;
using System.Linq.Expressions;

namespace CatalogDomain.AgreegateModels.CatalogAgreegate
{
    public class CatalogSpecification : SpecificationBase<Catalog>
    {
        public CatalogSpecification(bool isInclude, string? searchValue = "", List<ColumnOrder>? columnOrders = null) : base(isInclude)
        {
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                Expression<Func<Catalog, bool>> criteria = c =>
                searchValue.Contains(c.Code.ToLower()) || searchValue.Contains(c.Name.ToLower() ?? "") || searchValue.Contains(c.Description.ToLower() ?? "");

                AddCriteria(criteria);
            }

            // order by columns
            if (columnOrders != null && columnOrders.Any())
            {
                foreach (var columnOrder in columnOrders)
                {
                    switch (columnOrder.Name)
                    {
                        case nameof(Catalog.Code):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Code ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Code ?? "");
                            }

                            break;

                        case nameof(Catalog.Name):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Name ?? "");
                            }
                            else
                            {
                                AddOrderBy(w => w.Name ?? "");
                            }

                            break;

                        case nameof(Catalog.PriceStandar):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.PriceStandar);
                            }
                            else
                            {
                                AddOrderBy(w => w.PriceStandar);
                            }

                            break;

                        case nameof(Catalog.Price):
                            if (columnOrder.Order == Order.DESC)
                            {
                                AddOrderByDescending(w => w.Price);
                            }
                            else
                            {
                                AddOrderBy(w => w.Price);
                            }

                            break;

                        case nameof(Catalog.DateCreated):
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
                AddInclude(w => w.Addons);
            }

        }
    }
}
