using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser
{
    public class QueryParser
    {
        public static IEnumerable<string> GetProperties<T>()
        {
            return typeof( T )
                .GetProperties()
                .Select( p => p.Name );
        }

        public static IEnumerable<QueryFilter> GetFilters( IEnumerable<KeyValuePair<string, StringValues>> queryParams )
        {
            var typeProperties = GetProperties<T>();

            return queryParams
                .Where( pair => typeProperties.Any( p => 
                    string.Equals( p, pair.Key, StringComparison.InvariantCultureIgnoreCase) ) )
                .Select( pair => new QueryFilter
                {
                    Key = typeProperties.Where( p =>
                         string.Equals( p, pair.Key, StringComparison.InvariantCultureIgnoreCase ) ).First(),
                    Value = pair.Value.ToString()
                });
        }
    }
}
