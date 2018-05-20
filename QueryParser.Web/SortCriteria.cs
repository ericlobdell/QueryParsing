namespace QueryParser.Web
{
    public class SortCriteria
    {
        public string PropertyName { get; }
        public SortDirection SortDirection { get; }
        public int SortPosition { get; }

        public SortCriteria(string propName, int sortPosition, string sortDir = "asc")
        {
            PropertyName = propName;
            SortDirection = sortDir == "desc" ? SortDirection.Descending : SortDirection.Ascending;
            SortPosition = sortPosition;
        }
    }

    public enum SortDirection { Ascending, Descending }
}
