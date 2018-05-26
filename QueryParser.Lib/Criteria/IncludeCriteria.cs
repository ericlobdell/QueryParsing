using System;
using System.Linq.Expressions;

namespace QueryableRequests.Criteria
{
    public class IncludeCriteria<T>
    {
        public Expression<Func<T, object>> KeySelector { get; }

        public IncludeCriteria(Expression<Func<T, object>> selector)
        {
            KeySelector = selector;
        }
    }
}
