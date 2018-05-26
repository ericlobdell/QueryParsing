using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Primitives;
using QueryableRequests.Criteria;

namespace QueryableRequests
{
    public class QueryableRequest<T>: IQueryableRequest
    {
        IEnumerable<KeyValuePair<string, StringValues>> _queryParams = new List<KeyValuePair<string, StringValues>>();

        protected Dictionary<string, Func<string, Func<T, bool>>> FilterPredicateMap { get; } =
            new Dictionary<string, Func<string, Func<T, bool>>>();

        protected void HandleFilter(string filterKey, Func<string, Func<T, bool>> filterHandler) =>
            FilterPredicateMap.Add(filterKey, filterHandler);

        protected Dictionary<string, Expression<Func<T, object>>> SortKeySelectorMap { get; } =
            new Dictionary<string, Expression<Func<T, object>>>();

        protected void HandleSort(string sortKey, Expression<Func<T, object>> keySelector) =>
            SortKeySelectorMap.Add(sortKey, keySelector);

        protected Dictionary<string, Expression<Func<T, object>>> IncludeKeySelectorMap { get; } =
            new Dictionary<string, Expression<Func<T, object>>>();

        protected void HandleInclude(string sortKey, Expression<Func<T, object>> keySelector) =>
            IncludeKeySelectorMap.Add(sortKey, keySelector);

        public void SetQueryParams(IEnumerable<KeyValuePair<string, StringValues>> queryParams)
        {
            _queryParams = queryParams;
        }

        public List<FilterCirteria<T>> Filters => _queryParams
            .Select(q =>
            {
                if ( FilterPredicateMap.TryGetValue(q.Key.ToLower(), out var predicate) )
                    return new FilterCirteria<T>(q.Value, predicate);

                return null;
            })
            .Where(f => f != null)
            .ToList();

        public List<SortCriteria<T>> SortCriteria => GetSortInfos()
            .Select((sortInfo, sortPosition) =>
            {
                if ( SortKeySelectorMap.TryGetValue(sortInfo.Field, out var keySelector) )
                    return new SortCriteria<T>(sortPosition, keySelector, sortInfo.Direction);

                return null;
            })
            .Where(s => s != null)
            .ToList();

        public List<IncludeCriteria<T>> Includes => GetIncludes()
            .Select(include =>
            {
                if ( IncludeKeySelectorMap.TryGetValue(include, out var keySelector) )
                    return new IncludeCriteria<T>(keySelector);

                return null;
            })
            .Where(s => s != null)
            .ToList();

        private List<string> GetIncludes()
        {
            var includeParam = _queryParams
                .Where(p => p.Key.ToLower() == "include");

            if ( !includeParam.Any() )
                return new List<string>();

            return includeParam
                .First()
                .Value
                .ToString()
                .Split(',')
                .ToList();
        }

        private List<(string Field, string Direction)> GetSortInfos()
        {
            var sortParam = _queryParams
                .Where(p => p.Key.ToLower() == "sort");

            if ( !sortParam.Any() )
                return new List<(string Field, string Direction)>();

            return sortParam
                .First()
                .Value
                .ToString()
                .Split(',')
                .Select(pair =>
                {
                    var parts = pair.Split(':');
                    var sortField = parts[0].ToLower();
                    var sortDirection = parts.Length == 2 ? parts[1] : "asc";

                    return (sortField, sortDirection);
                })
                .ToList();
        }
    }
}
