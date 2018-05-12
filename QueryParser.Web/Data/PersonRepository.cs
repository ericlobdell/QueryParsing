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

        public int Add(Person person)
        {
            _db.Person.Add( person );
            _db.SaveChanges();

            return person.Id;
        }

        public IEnumerable<Person> Get( FilterablePersonRequest request)
        {
            var query = _db.Person.AsQueryable();

            foreach ( var filter in request.Filters )
            {
                query = query.Where( p => p
                    .GetType()
                    .GetProperty( filter.PropertyName )
                    .GetValue( p ).ToString() == filter.Value ); 
            }

            return query.ToList();
        }
    }
}
