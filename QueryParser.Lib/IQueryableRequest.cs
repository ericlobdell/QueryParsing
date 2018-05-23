using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace QueryableRequests
{
    public interface IQueryableRequest
    {
        void SetQueryParams(IEnumerable<KeyValuePair<string, StringValues>> queryParams);
    }
}
