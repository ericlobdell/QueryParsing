using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace QueryParser.Web.Requests
{
    public interface IFilterableRequest
    {
        void SetQueryParams( IEnumerable<KeyValuePair<string, StringValues>> queryParams );
    }
}
