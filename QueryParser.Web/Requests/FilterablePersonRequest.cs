using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryParser.Web.Models;

namespace QueryParser.Web.Requests
{
    public class FilterablePersonRequest: BaseFilterRequest<Person>
    {
        public override Dictionary<string, Func<string, Func<Person, bool>>> FilterPredicateMap =>
            new Dictionary<string, Func<string, Func<Person, bool>>>
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

    public class FilterableBuildingRequest: BaseFilterRequest<Building>
    {
        public override Dictionary<string, Func<string, Func<Building, bool>>> FilterPredicateMap =>
            new Dictionary<string, Func<string, Func<Building, bool>>>
            {
                { "name", filterValue => building =>
                    FilterMatchers.StringComplete(building.Name, filterValue)
                },
                { "floorcount", filterValue => building =>
                    FilterMatchers.Int32(building.FloorCount, filterValue)
                },
            };

        public override Dictionary<string, Expression<Func<Building, object>>> SortKeySelectorMap =>
            new Dictionary<string, Expression<Func<Building, object>>>
            {
                { "name", building => building.Name },
                { "age", building => building.FloorCount }
            };
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
