using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Queries.Specification
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        private Func<T, bool> _compiledExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationBase{T}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        protected SpecificationBase(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Default do not filter
        /// </summary>
        protected SpecificationBase()
        {
            Criteria = c => true;
        }

        /// <summary>
        /// Gets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        public Expression<Func<T, bool>> Criteria { get; private set; }
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }


        protected virtual void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Gets the compiled expression.
        /// </summary>
        /// <value>The compiled expression.</value>
        private Func<T, bool> CompiledExpression => _compiledExpression ?? (_compiledExpression = Criteria.Compile());

        /// <summary>
        /// Determines whether [is satisfied by] [the specified entity].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if [is satisfied by] [the specified entity]; otherwise, <c>false</c>.</returns>
        public bool IsSatisfiedBy(T entity)
        {
            return CompiledExpression(entity);
        }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <value>The includes.</value>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        /// <value>The include strings.</value>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Adds the include.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Adds the include.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
    }
}
