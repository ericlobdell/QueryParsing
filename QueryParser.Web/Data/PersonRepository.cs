using QueryParser.Web.Models;
using QueryParser.Web.Requests;
using System.Collections.Generic;
using System.Linq;

namespace QueryParser.Web.Data
{
    public class PersonRepository
    {
        TestContext _db;
        public PersonRepository(TestContext db)
        {
            _db = db;
        }

        public void Add(params Person[] persons)
        {
            _db.Person.AddRange( persons );
            _db.SaveChanges();

        }

        public IEnumerable<Person> Get( FilterablePersonRequest request)
        {
            var query = _db.Person.AsQueryable();

            query = QueryBuilder.MapFiltersToQuery( request.Filters, query );

            return query.ToList();
        }
    }
}
