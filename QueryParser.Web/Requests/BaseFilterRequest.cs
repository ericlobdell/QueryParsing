using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web.Requests
{
    public class BaseFilterRequest<T>: IFilterableRequest
    {
        IEnumerable<KeyValuePair<string, StringValues>> _queryParams = new List<KeyValuePair<string, StringValues>>();
        IEnumerable<string> _propertyNames;

        public void SetQueryParams(IEnumerable<KeyValuePair<string, StringValues>> queryParams)
        {
            _queryParams = queryParams;
            _propertyNames = GetPropertyPaths(typeof(T));
        }

        public bool HasFilters => Filters.Any();
        public bool HasSort => Sorts.Any();

        public IEnumerable<QueryFilter> Filters => GetFilters();
        public IEnumerable<QuerySort> Sorts => GetSorts();

        private IEnumerable<QuerySort> GetSorts()
        {
            var sort = new List<QuerySort>(); ;
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

                    return new QuerySort(propName, index, sortDir);
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

        private string GetPropertyName(string test) => _propertyNames
            .Where(prop => PropertyNameComparisonPredicate(prop, test))
            .FirstOrDefault();

        private bool IsPropertyName(string test) => _propertyNames
            .Any(prop => PropertyNameComparisonPredicate(prop, test));

        public IEnumerable<QueryFilter> GetFilters()
        {
            return _queryParams
                .Where(pair => IsPropertyName(pair.Key))
                .Select(pair => new QueryFilter
                {
                    PropertyName = GetPropertyName(pair.Key),
                    Value = pair.Value.ToString()
                });
        }


        public List<string> GetPropertyPaths(Type t)
        {
            List<string> GetPaths(Type type, string parent, List<string> paths)
            {
                var properties = type.GetProperties();

                foreach ( var property in properties )
                {
                    var path = string.IsNullOrWhiteSpace(parent) ? property.Name : $"{parent}.{property.Name}";

                    if ( property.PropertyType.Assembly == type.Assembly )
                        GetPaths(property.PropertyType, path, paths);

                    else paths.Add(path);
                }

                return paths;
            }

            return GetPaths(t, "", new List<string>());
        }
    }
}
