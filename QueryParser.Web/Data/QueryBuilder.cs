using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web.Data
{
    public class QueryBuilder<T>
    {
        IQueryable<T> _query;

        public QueryBuilder(IQueryable<T> query)
        {
            _query = query;
        }

        public QueryBuilder<T> Filter(IEnumerable<FilterCirteria<T>> filters)
        {
            foreach ( var filter in filters )
                _query = _query.Where(p => filter.Predicate(p)(filter.Value));

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

            IOrderedQueryable<T> ApplyOrderBy(SortCriteria<T> sort) => sort.SortDirection == SortDirection.Ascending ?
                _query.OrderBy(sort.KeySelector) :
                _query.OrderByDescending(sort.KeySelector);

            IOrderedQueryable<T> ApplyThenBy(SortCriteria<T> sort) => sort.SortDirection == SortDirection.Ascending ?
                orderedQuery.ThenBy(sort.KeySelector) :
                orderedQuery.ThenByDescending(sort.KeySelector);

            return this;
        }

        public IQueryable<T> Build() => _query;
    }
}
