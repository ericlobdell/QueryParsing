using System.Collections.Generic;

namespace QueryParser.Web.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int GetId() => Id;
        private string Secret { get; set; }
        public Foo Foo { get; set; }
        public List<Pet> Pets { get; set; }
        public Car Car { get; set; }
    }

    public class Foo
    {
        public int Id { get; set; }
        public string Bar { get; set; }
        public int Baz { get; set; }
    }
}
