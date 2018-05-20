using System;
using System.Linq.Expressions;

namespace QueryParser.Web
{
    public class SortCriteria<T>
    {
        public SortDirection SortDirection { get; }
        public int SortPosition { get; }
        public Expression<Func<T, object>> KeySelector { get; }

        public SortCriteria(int sortPosition, Expression<Func<T, object>> selector, string sortDir = "asc")
        {
            SortDirection = sortDir == "desc" ? SortDirection.Descending : SortDirection.Ascending;
            SortPosition = sortPosition;
            KeySelector = selector;
        }
    }

    public enum SortDirection { Ascending, Descending }
}
