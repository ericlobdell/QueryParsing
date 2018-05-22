using System;

namespace QueryParser.Web
{
    public class FilterCirteria<T>
    {
        public Func<T, string, bool> Predicate { get; }
        public string FilterValue { get; set; }

        public FilterCirteria(string value, Func<T,string, bool> predicate)
        {
            FilterValue = value;
            Predicate = predicate;
        }
    }
}
