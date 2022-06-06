using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Queries.Specification
{
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <value>The includes.</value>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        /// <value>The include strings.</value>
        List<string> IncludeStrings { get; }

        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
    }
}
