using Microsoft.Extensions.Primitives;
using QueryParser.Web.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web.Requests
{
    public abstract class BaseFilterRequest<T>: IFilterableRequest
    {
        IEnumerable<KeyValuePair<string, StringValues>> _queryParams = new List<KeyValuePair<string, StringValues>>();
        IEnumerable<string> _propertyPaths;


        public void SetQueryParams(IEnumerable<KeyValuePair<string, StringValues>> queryParams)
        {
            _queryParams = queryParams;
            _propertyPaths = typeof(T).GetPropertyPaths();
        }

        public bool HasFilters => Filters.Any();
        public bool HasSort => Sorts.Any();

        public IEnumerable<FilterCirteria> Filters => GetFilters().Where(f => FilterPredicateMap.ContainsKey(f.PropertyPath));
        public IEnumerable<SortCriteria> Sorts { get; }
        public abstract Dictionary<string, Func<T, object ,bool>> FilterPredicateMap { get; }

        protected IEnumerable<SortCriteria> GetSorts()
        {
            var sort = new List<SortCriteria>(); ;
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
                .Select((pair, index) =>
                {
                    var parts = pair.Split(':');
                    var propName = GetPropertyName(parts[0]);
                    var sortDir = parts.Length == 2 ? parts[1] : "asc";

                    return new SortCriteria(propName, index, sortDir);
                })
                .Where(s => IsPropertyName(s.PropertyName))
                .ToList();

            return sort;
        }

        bool PropertyNameComparisonPredicate(string prop, string test)
        {
            var parts = prop.Split(".");
            return parts.Any(p => string.Equals(p, test, StringComparison.InvariantCultureIgnoreCase));
        }

        private string GetPropertyName(string test) => _propertyPaths
            .Where(prop => PropertyNameComparisonPredicate(prop, test))
            .FirstOrDefault();

        private bool IsPropertyName(string test) => _propertyPaths
            .Any(prop => PropertyNameComparisonPredicate(prop, test));

        public IEnumerable<FilterCirteria> GetFilters()
        {
            return _queryParams
                .Where(pair => IsPropertyName(pair.Key))
                .Select(pair => new FilterCirteria
                {
                    PropertyPath = GetPropertyName(pair.Key),
                    Value = pair.Value.ToString()
                });
        }
    }
}
