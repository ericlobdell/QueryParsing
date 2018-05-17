namespace QueryParser.Web.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int GetId() => Id;
        private string Secret { get; set; }
    }
}
