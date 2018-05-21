using System;

namespace QueryParser.Web
{
    public class FilterCirteria<T>
    {
        public Func<T, bool> Predicate { get; }

        public FilterCirteria(string value, Func<string, Func<T, bool>> predicate)
        {
            Predicate = predicate(value);
        }
    }
}
