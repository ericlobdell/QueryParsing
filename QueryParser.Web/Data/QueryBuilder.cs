using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryParser.Web.Data
{
    public static class QueryBuilder
    {
        public static IQueryable<T> MapFiltersToQuery<T>(IEnumerable<QueryFilter> filters,  IQueryable<T> query )
        {
            foreach ( var filter in filters )
            {
                query = query.Where( p => p
                    .GetType()
                    .GetProperty( filter.PropertyName )
                    .GetValue( p ).ToString() == filter.Value );
            }

            return query;
        }
    }
}
