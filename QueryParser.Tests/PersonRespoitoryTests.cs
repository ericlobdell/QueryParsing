using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QueryParser.Web.Data;
using QueryParser.Web.Models;
using QueryParser.Web.Requests;
using Xunit;

namespace QueryParser.Tests
{
    public class PersonRespoitoryTests
    {
        private PersonRepository GetSut()
        {
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;

            return new PersonRepository(new TestContext(options));
        }

        [Fact]
        public void Get_applies_filters_to_query()
        {
            var david = new Person
            {
                Name = "David",
                Foo = new Foo { Bar = "a" }
            };

            var tom = new Person
            {
                Name = "Tom",
                Foo = new Foo { Bar = "b" }
            };

            var sut = GetSut();
            sut.Add(david, tom);

            var queryString = $"?name={tom.Name}&bar={tom.Foo.Bar}";
            var parsedQuery = QueryHelpers.ParseQuery(queryString);
            var request = new QueryablePersonRequest();
            request.SetQueryParams(parsedQuery);

            var results = sut.Get(request);

            Assert.Contains(results, p => p.Name == tom.Name);
            Assert.DoesNotContain(results, p => p.Name == david.Name);
        }
    }
}
