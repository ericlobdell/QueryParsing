using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public override Dictionary<string, Func<Person, Func<object, bool>>> FilterPredicateMap =>
            new Dictionary<string, Func<Person, Func<object, bool>>>
            {
                { "name", person => FilterMatcher.StringExact(person.Name)
                },
                { "age", person => FilterMatcher.Int32(person.Age)
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

    public static class FilterMatcher
    {
        public static Func<object, bool> StringExact(string fieldValue)
        {
            return (object filterValue) =>
             {
                 return filterValue is string
                 ? filterValue.ToString().ToLower() == fieldValue.ToLower()
                 : false;
             };
        }

        public static Func<object, bool> Int32(int fieldValue)
        {
            return (object filterValue) =>
            {
                return int.TryParse(filterValue as string, out var filterValueAsInt)
                ? filterValueAsInt == fieldValue
                : false;
            };
        }
    }
}
