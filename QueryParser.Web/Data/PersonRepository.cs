using QueryableRequests;
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
            _db.Person.AddRange(persons);
            _db.SaveChanges();
        }

        public IEnumerable<Person> Get(QueryablePersonRequest request)
        {
            var query = new QueryBuilder<Person>(_db.Person)
                .Filter(request.Filters)
                .Sort(request.SortCriteria)
                .Build();

            return query.ToList();
        }
    }
}
