using System;

namespace QueryableRequests.Criteria
{
    public class FilterCirteria<T>
    {
        public Func<T, string, bool> Predicate { get; }
        public string Value { get; }

        public FilterCirteria(string value, Func<T,string, bool> predicate)
        {
            Value = value;
            Predicate = predicate;
        }
    }
}
