using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public override Dictionary<string, Func<object, Func<Person, bool>>> FilterPredicateMap =>
            new Dictionary<string, Func<object, Func<Person, bool>>>
            {
                { "name", filterValue => person => 
                    FilterMatchers.StringComplete(person.Name, filterValue)
                },
                { "age", filterValue => person => 
                    FilterMatchers.Int32(person.Age, filterValue)
                },
            };

        public override Dictionary<string, Expression<Func<Person, object>>> SortKeySelectorMap =>
            new Dictionary<string, Expression<Func<Person, object>>>
            {
                { "name", person => person.Name },
                { "age", person => person.Age }
            };
    }

    //public class FilterableBuildingRequest: BaseFilterRequest<Building>
    //{
    //    public override Dictionary<string, Func<Building, object, bool>> FilterPredicateMap => throw new NotImplementedException();

    //    public override Dictionary<string, Expression<Func<Building, object>>> SortKeySelectorMap => throw new NotImplementedException();
    //}

    public static class FilterMatchers
    {
        public static bool StringComplete(string fieldValue, object filterValue) =>
            filterValue is string
                && filterValue.ToString().ToLower() == fieldValue.ToLower();

        public static bool Int32(int fieldValue, object filterValue) =>
            int.TryParse(filterValue as string, out var filterValueAsInt)
                && filterValueAsInt == fieldValue;
    }
}
