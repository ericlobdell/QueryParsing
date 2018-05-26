using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using QueryableRequests.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace QueryableRequests
{
    public class QueryBuilder<T>
        where T : class
    {
        IQueryable<T> _query;

        public QueryBuilder(IQueryable<T> query)
        {
            _query = query;
        }

        public QueryBuilder<T> Filter(IEnumerable<FilterCirteria<T>> filters)
        {
            foreach ( var filter in filters )
                _query = _query
                    .Where(filter.Predicate)
                    .AsQueryable();

            return this;
        }

        public QueryBuilder<T> Sort(IEnumerable<SortCriteria<T>> sorts)
        {
            if ( !sorts.Any() )
                return this;

            var orderedSorts = sorts.OrderBy(s => s.SortPosition);
            var firstSort = orderedSorts.First();
            var remainingSorts = orderedSorts.Skip(1);

            var orderedQuery = ApplyOrderBy(firstSort);

            foreach ( var s in remainingSorts )
                orderedQuery = ApplyThenBy(s);

            _query = orderedQuery.AsQueryable();

            return this;

            IOrderedQueryable<T> ApplyOrderBy(SortCriteria<T> sort) =>
                sort.SortDirection == SortDirection.Ascending
                    ? _query.OrderBy(sort.KeySelector)
                    : _query.OrderByDescending(sort.KeySelector);

            IOrderedQueryable<T> ApplyThenBy(SortCriteria<T> sort) =>
                sort.SortDirection == SortDirection.Ascending
                    ? orderedQuery.ThenBy(sort.KeySelector)
                    : orderedQuery.ThenByDescending(sort.KeySelector);
        }

        public QueryBuilder<T> Include(IEnumerable<IncludeCriteria<T>> includes)
        {
            var includableQueryable = _query as IIncludableQueryable<T,object>;

            foreach ( var i in includes )
            {
                includableQueryable = includableQueryable.Include(i.KeySelector);
            }

            _query = includableQueryable.AsQueryable();

            return this;
        }

        public IQueryable<T> Build() => _query;
    }

    
}
