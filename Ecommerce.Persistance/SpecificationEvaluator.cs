using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistance
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = EntryPoint;
            if (specification is not null)
            {
                if (specification.IncludeExpression != null && specification.IncludeExpression.Any())
                {
                    // Where
                    if (specification.Criteria != null)
                    {
                        query = query.Where(specification.Criteria);
                    }



                    // include
                    query = specification.IncludeExpression.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

                    // OrderBy
                    if (specification.OrderBy != null)
                    {
                        query = query.OrderBy(specification.OrderBy);
                    }
                    // OrderByDescending
                    if (specification.OrderByDescending != null)
                    {
                        query = query.OrderByDescending(specification.OrderByDescending);
                    }

                    // Pagination
                    if (specification.IsPaginated)
                    {
                        query = query.Skip(specification.Skip).Take(specification.Take);

                    }
                }
            }
            return query;
        }


    }
}
