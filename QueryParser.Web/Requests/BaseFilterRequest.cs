using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web.Requests
{
    public class BaseFilterRequest<T> : IFilterableRequest
    {
        IEnumerable<KeyValuePair<string, StringValues>> _queryParams = new List<KeyValuePair<string, StringValues>>();

        public void SetQueryParams( IEnumerable<KeyValuePair<string, StringValues>> queryParams )
        {
            _queryParams = queryParams;
        }

        public IEnumerable<QueryFilter> Filters => GetFilters();

        public IEnumerable<QueryFilter> GetFilters()
        {
            var propertyNames = typeof( T )
                .GetProperties()
                .Select( p => p.Name );

            return _queryParams
                .Where( pair => propertyNames.Any( p =>
                    string.Equals( p, pair.Key, StringComparison.InvariantCultureIgnoreCase ) ) )
                .Select( pair => new QueryFilter
                {
                    PropertyName = propertyNames.Where( p =>
                         string.Equals( p, pair.Key, StringComparison.InvariantCultureIgnoreCase ) ).First(),
                    Value = pair.Value.ToString()
                } );
        }
    }
}
