using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public override Dictionary<string, Func<Person, object, bool>> FilterPredicateMap =>
            new Dictionary<string, Func<Person, object, bool>>
            {
                { "name", (person, value) => value is string 
                    ? person.Name == value as string 
                    : false
                },
                { "age", (person, value) => int.TryParse(value as string, out var age)
                    ? person.Age == age
                    : false
                },
            };

        public override Dictionary<string, Expression<Func<Person, object>>> SortKeySelectorMap =>
            new Dictionary<string, Expression<Func<Person, object>>>
            {
                { "name", person => person.Name },
                { "age", person => person.Age }
            };
    }

    public class FilterableBuildingRequest: BaseFilterRequest<Building>
    {
        public override Dictionary<string, Func<Building, object, bool>> FilterPredicateMap => throw new NotImplementedException();

        public override Dictionary<string, Expression<Func<Building, object>>> SortKeySelectorMap => throw new NotImplementedException();
    }
}
