using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence
{
    // TO BUILD THE QUERIES
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> CreateQuery<T, TKey>(IQueryable<T> EntryPoint, ISpecifications<T, TKey> specifications) where T : BaseEntity<TKey>
        {
            IQueryable<T> query = EntryPoint; // _dbcontext.products
            if (specifications.IncludeExpression != null && specifications.IncludeExpression.Any())
            {
                foreach (var include in specifications.IncludeExpression)
                {
                    query = query.Include(include);
                }
            }
            if (specifications.ConditionExpression != null && specifications.ConditionExpression.Any())
            {
                foreach (var condition in specifications.ConditionExpression)
                {
                    query = query.Where(condition);
                }
            }
            if (specifications.OrderByExpression != null)
            {
                query = query.OrderBy(specifications.OrderByExpression);
            }
            if (specifications.OrderByDescendingExpression != null)
            {
                query = query.OrderByDescending(specifications.OrderByDescendingExpression);
            }
            if(specifications.IsPagingEnabled)
            {
                if (specifications.skip.HasValue)
                {
                    query = query.Skip(specifications.skip.Value);
                }
                if (specifications.take.HasValue)
                {
                    query = query.Take(specifications.take.Value);
                }
            }
            return query;
        }
    }
}
