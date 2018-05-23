namespace QueryableRequests
{
    public static class FilterMatchers
    {
        public static bool StringComplete(string fieldValue, string filterValue) =>
            filterValue.ToLower() == fieldValue.ToLower();

        public static bool StringContains(string fieldValue, string filterValue) =>
           fieldValue.ToLower().Contains(filterValue.ToLower());

        public static bool StringStartsWith(string fieldValue, string filterValue) =>
           fieldValue.ToLower().StartsWith(filterValue.ToLower());

        public static bool Int32(int fieldValue, string filterValue) =>
            int.TryParse(filterValue, out var filterValueAsInt)
                && filterValueAsInt == fieldValue;
    }
}
