using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Include Expressions
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }
        #endregion

        #region Where Conditions
        public Expression<Func<TEntity, bool>> Criteria { get; }



        public BaseSpecification(Expression<Func<TEntity, bool>> CriteriaExp)
        {
            Criteria = CriteriaExp;
        }


        #endregion

        #region Sorting

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; } 

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }



        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        #endregion

        #region Pagination

        public int Take {get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;

        }

        #endregion
    }
}
