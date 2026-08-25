using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ISpecifications<T, TKey> where T : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<T, object>>> IncludeExpression { get; }
        public ICollection<Expression<Func<T, bool>>> ConditionExpression { get; }
        public Expression<Func<T, object>>? OrderByExpression { get; }
        public Expression<Func<T, object>>? OrderByDescendingExpression { get; }
        public int? skip { get; }
        public int? take { get; }
        public bool IsPagingEnabled { get; }


    }
}
