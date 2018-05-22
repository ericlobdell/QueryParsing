using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public FilterablePersonRequest()
        {
            HandleFilter("name", (person, filterValue) =>
                FilterMatchers.StringComplete(person.Name, filterValue));

            HandleFilter("age", (person, filterValue) =>
                FilterMatchers.Int32(person.Age, filterValue));
        }
        
        //public override Dictionary<string, Expression<Func<Person, object>>> SortKeySelectorMap =>
        //    new Dictionary<string, Expression<Func<Person, object>>>
        //    {
        //        { "name", person => person.Name },
        //        { "age", person => person.Age }
        //    };
    }

    public class FilterableBuildingRequest: BaseFilterRequest<Building>
    {
        
    }

    public static class FilterMatchers
    {
        public static bool StringComplete(string fieldValue, string filterValue) =>
            filterValue.ToLower() == fieldValue.ToLower();

        public static bool Int32(int fieldValue, string filterValue) =>
            int.TryParse(filterValue, out var filterValueAsInt)
                && filterValueAsInt == fieldValue;
    }
}
