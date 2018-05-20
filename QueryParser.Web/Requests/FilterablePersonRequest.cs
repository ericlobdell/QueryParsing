using System;
using System.Collections.Generic;
using System.Linq;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public override Dictionary<string, Func<Person, object, bool>> FilterPredicateMap =>
            new Dictionary<string, Func<Person, object, bool>>
            {
                { "Name", (person , value) => value is string 
                    ? person.Name == value as string 
                    : false
                },
                { "Age", (person , value) => int.TryParse(value.ToString(), out var age)
                    ? person.Age == age
                    : false
                },
            };
    }

    public class FilterableBuildingRequest: BaseFilterRequest<Building>
    {
        public override Dictionary<string, Func<Building, object, bool>> FilterPredicateMap => throw new NotImplementedException();
    }
}
