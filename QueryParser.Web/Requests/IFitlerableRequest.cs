using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace QueryParser.Web.Requests
{
    public interface IFilterableRequest
    {
        IEnumerable<FilterCirteria> GetFilters();
        void SetQueryParams( IEnumerable<KeyValuePair<string, StringValues>> queryParams );
    }
}
