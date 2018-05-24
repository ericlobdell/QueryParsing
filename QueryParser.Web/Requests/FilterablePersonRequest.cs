using QueryableRequests;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class QueryablePersonRequest: QueryableRequest<Person>
    {
        public QueryablePersonRequest()
        {
            HandleFilter("name", filterValue => person =>
                FilterMatchers.StringComplete(person.Name, filterValue));

            HandleFilter("age", filterValue => person =>
                FilterMatchers.Int32(person.Age, filterValue));

            HandleSort("name", person => person.Name);

            HandleSort("age", person => person.Age);
        }
    }
}
