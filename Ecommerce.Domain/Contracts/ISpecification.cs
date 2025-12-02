using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // Include expressions for related entities
        public ICollection<Expression<Func<TEntity, Object>>> IncludeExpression { get; }

        // Where conditions

        public Expression<Func<TEntity, bool>>? Criteria { get; }


        // Sorting

        public Expression<Func<TEntity, object>>? OrderBy { get; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; }

        // Pagination

        public int Take { get; }
        public int Skip { get; }

        public bool IsPaginated { get; }

    }
}
