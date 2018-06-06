using System;

namespace QueryableRequests.Criteria
{
    public class FilterCirteria<T>
    {
        public string Key { get; set; }
        public Func<T, bool> Predicate { get; }

        public FilterCirteria(string key, string value, Func<string,Func<T, bool>> predicate)
        {
            Key = key;
            Predicate = predicate(value);
        }
    }
}
