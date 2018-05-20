using System;

namespace QueryParser.Web
{
    public class FilterCirteria<T>
    {
        public Func<T, bool> Predicate { get; }

        public FilterCirteria(object value, Func<object, Func<T, bool>> predicate)
        {
            Predicate = predicate(value);
        }
    }
}
