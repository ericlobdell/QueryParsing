using QueryableRequests;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: QueryableRequest<Person>
    {
        public FilterablePersonRequest()
        {
            HandleFilter("name", (person, filterValue) =>
                FilterMatchers.StringComplete(person.Name, filterValue));

            HandleFilter("age", (person, filterValue) =>
                FilterMatchers.Int32(person.Age, filterValue));

            HandleSort("name", person => person.Name);

            HandleSort("age", person => person.Age);
        }
    }
}
