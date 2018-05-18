using QueryParser.Web.Requests;
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

        public QueryBuilder<T> Filter(IEnumerable<QueryFilter> filters)
        {
            foreach ( var filter in filters )
            {
                _query = _query.Where(p => p
                    .GetPropValue(filter.PropertyName)
                    .ToString() == filter.Value);
            }

            return this;
        }

        public QueryBuilder<T> Sort(IEnumerable<QuerySort> sorts)
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

            IOrderedQueryable<T> ApplyOrderBy(QuerySort sort) => sort.SortDirection == SortDirection.Ascending ?
                _query.OrderBy(x => x.GetPropValue(sort.PropertyName)) :
                _query.OrderByDescending(x => x.GetPropValue(sort.PropertyName));

            IOrderedQueryable<T> ApplyThenBy(QuerySort sort) => sort.SortDirection == SortDirection.Ascending ?
                orderedQuery.ThenBy(x => x.GetPropValue(sort.PropertyName)) :
                orderedQuery.ThenByDescending(x => x.GetPropValue(sort.PropertyName));

            return this;
        }

        public IQueryable<T> Build() => _query;
    }
}
