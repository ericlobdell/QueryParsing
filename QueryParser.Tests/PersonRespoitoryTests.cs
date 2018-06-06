using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QueryParser.Web.Data;
using QueryParser.Web.Models;
using QueryParser.Web.Requests;
using System.Linq;
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

        [Fact]
        public void Get_applies_includes_nav_properties_when_specified()
        {
            var david = new Person
            {
                Name = "David",
                Foo = new Foo { Bar = "a" },
                Car = new Car { Model = "Honda" }
            };

            var tom = new Person
            {
                Name = "Tom",
                Foo = new Foo { Bar = "b" }
            };

            var sut = GetSut();
            sut.Add(david, tom);

            var queryString = $"?name={david.Name}&include=car";
            var parsedQuery = QueryHelpers.ParseQuery(queryString);
            var request = new QueryablePersonRequest();
            request.SetQueryParams(parsedQuery);

            var results = sut.Get(request).First();

            Assert.NotNull(results.Car);
            Assert.Equal(david.Car.Model, results.Car.Model);
        }

        [Fact]
        public void Get_does_not_includes_nav_properties_when_not_specified()
        {
            var david = new Person
            {
                Name = "David",
                Foo = new Foo { Bar = "a" },
                Car = new Car { Model = "Honda" }
            };

            var tom = new Person
            {
                Name = "Tom",
                Foo = new Foo { Bar = "b" }
            };

            var sut = GetSut();
            sut.Add(david, tom);

            var queryString = $"?name={david.Name}";
            var parsedQuery = QueryHelpers.ParseQuery(queryString);
            var request = new QueryablePersonRequest();
            request.SetQueryParams(parsedQuery);

            var results = sut.Get(request).First();

            Assert.Null(results.Car);
        }
    }
}
