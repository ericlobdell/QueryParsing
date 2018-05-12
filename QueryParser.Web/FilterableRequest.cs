using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace QueryParser.Web
{
    public class FilterablePersonRequest : BaseFilterRequest<Person>
    {
        
    }

    public interface IFilterable
    {
        IEnumerable<QueryFilter> GetFilters();
        void SetQueryParams( IEnumerable<KeyValuePair<string, StringValues>> queryParams );
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GetId() => Id;
        private string Secret { get; set; }
    }
}
