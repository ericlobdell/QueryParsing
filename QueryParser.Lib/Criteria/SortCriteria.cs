using System;
using System.Linq.Expressions;

namespace QueryableRequests.Criteria
{
    public class SortCriteria<T>
    {
        public string Key { get; set; }
        public SortDirection SortDirection { get; }
        public int SortPosition { get; }
        public Expression<Func<T, object>> KeySelector { get; }

        public SortCriteria(string key, int sortPosition, Expression<Func<T, object>> selector, string sortDir = "asc")
        {
            Key = key;
            SortDirection = sortDir == "desc" ? SortDirection.Descending : SortDirection.Ascending;
            SortPosition = sortPosition;
            KeySelector = selector;
        }
    }

    public enum SortDirection { Ascending, Descending }
}
