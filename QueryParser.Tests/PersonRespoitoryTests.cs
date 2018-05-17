using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QueryParser.Web.Data;
using QueryParser.Web.Models;
using QueryParser.Web.Requests;
using System;
using System.Collections.Generic;
using System.Text;
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
                Name = "David"
            };

            var tom = new Person
            {
                Name = "Tom"
            };

            var sut = GetSut();
            sut.Add(david, tom);

            var queryString = $"?name={tom.Name}&foo=bar";
            var parsedQuery = QueryHelpers.ParseQuery(queryString);
            var request = new FilterablePersonRequest();
            request.SetQueryParams(parsedQuery);

            var results = sut.Get(request);

            Assert.Contains(results, p => p.Name == tom.Name);
            Assert.DoesNotContain(results, p => p.Name == david.Name);
        }
    }
}
