using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryParser.Web.Requests
{
    public abstract class BaseFilterRequest<T>: IFilterableRequest
    {
        IEnumerable<KeyValuePair<string, StringValues>> _queryParams = new List<KeyValuePair<string, StringValues>>();
        IEnumerable<string> _propertyPaths;

        public abstract Dictionary<string, Func<T, Func<object, bool>>> FilterPredicateMap { get; }
        public abstract Dictionary<string, Expression<Func<T, object>>> SortKeySelectorMap { get; }

        public void SetQueryParams(IEnumerable<KeyValuePair<string, StringValues>> queryParams)
        {
            _queryParams = queryParams;
        }

        public bool HasFilters => Filters.Any();
        public bool HasSort => SortCriteria.Any();

        public IEnumerable<FilterCirteria<T>> Filters => _queryParams
            .Select(q =>
            {
                if ( FilterPredicateMap.TryGetValue(q.Key.ToLower(), out var predicate) )
                    return new FilterCirteria<T>(q.Value, predicate);

                return null;
            })
            .Where(f => f != null);

        public IEnumerable<SortCriteria<T>> SortCriteria => GetSorts();

        private IEnumerable<SortCriteria<T>> GetSorts()
        {
            var sort = new List<SortCriteria<T>>();
            var sortParam = _queryParams
                .Where(p => p.Key.ToLower() == "sort");

            if ( !sortParam.Any() )
                return sort;

            var sorts = sortParam
                .First()
                .Value
                .ToString()
                .Split(',');

            sort = sorts
                .Select((pair, sortPosition) =>
                {
                    var parts = pair.Split(':');
                    var sortField = parts[0].ToLower();
                    var sortDirection = parts.Length == 2 ? parts[1] : "asc";

                    if ( SortKeySelectorMap.TryGetValue(sortField, out var keySelector) )
                        return new SortCriteria<T>(sortPosition, keySelector, sortDirection);

                    return null;
                })
                .Where(s => s != null)
                .ToList();

            return sort;
        }
    }
}
