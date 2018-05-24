﻿using System;
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
        IEnumerable<string> _propertyPaths;

        protected Dictionary<string, Func<string, Func<T, bool>>> FilterPredicateMap { get; } = new Dictionary<string, Func<string, Func<T, bool>>>();

        protected void HandleFilter(string filterKey, Func<string, Func<T, bool>> filterHandler) =>
            FilterPredicateMap.Add(filterKey, filterHandler);

        protected Dictionary<string, Expression<Func<T, object>>> SortKeySelectorMap { get; } = new Dictionary<string, Expression<Func<T, object>>>();

        protected void HandleSort(string sortKey, Expression<Func<T, object>> keySelector) =>
            SortKeySelectorMap.Add(sortKey, keySelector);

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

        public IEnumerable<SortCriteria<T>> SortCriteria => GetSorts();

        private IEnumerable<SortCriteria<T>> GetSorts()
        {
            var sortCriteria = new List<SortCriteria<T>>();
            var sortParam = _queryParams
                .Where(p => p.Key.ToLower() == "sort");

            if ( !sortParam.Any() )
                return sortCriteria;

            var sorts = sortParam
                .First()
                .Value
                .ToString()
                .Split(',');

            sortCriteria = sorts
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

            return sortCriteria;
        }
    }
}
