using QueryableRequests;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class QueryablePersonRequest: QueryableRequest<Person>
    {
        public QueryablePersonRequest()
        {
            Filter("name", filterValue => person =>
                FilterMatchers.StringComplete(person.Name, filterValue));

            Filter("age", filterValue => person =>
                FilterMatchers.Int32(person.Age, filterValue));

            Sort("name", person => person.Name);

            Sort("age", person => person.Age);

            Include("pets", person => person.Pets);
            Include("car", person => person.Car);
        }
    }
}
