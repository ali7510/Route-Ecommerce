using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specifications
{
    public class BaseSpecification<T, TKey> : ISpecifications<T, TKey> where T : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<T, object>>> IncludeExpression { get; } = [];
        public ICollection<Expression<Func<T, bool>>> ConditionExpression { get; } = [];

        public Expression<Func<T, object>>? OrderByDescendingExpression { get; private set; }

        public Expression<Func<T, object>>? OrderByExpression { get; private set; }



        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            
            IncludeExpression.Add(includeExpression);
        }

        protected void AddCondition(Expression<Func<T, bool>> conditionExpression)
        {
            ConditionExpression.Add(conditionExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderByExpression = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescendingExpression = orderByDescendingExpression;
        }

        #region Pagination
        public int? skip { get; private set; }
        public int? take { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPagingEnabled = true;
            this.skip = (pageIndex-1)*pageSize;
            this.take = pageSize;
        }
        #endregion
    }
}
