using Microsoft.AspNetCore.WebUtilities;
using QueryableRequests;
using QueryableRequests.Criteria;
using QueryParser.Web.Models;
using QueryParser.Web.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace QueryParser.Tests
{
    public class QueryableRequestTests
    {
        TestQueryableRequest GetSut(string query)
        {
            var queryParams = QueryHelpers.ParseQuery(query);

            var sut = new TestQueryableRequest();
            sut.SetQueryParams(queryParams);

            return sut;
        }

        [Fact]
       public void Sort_parsed_with_asc_direction_if_none_specified()
        {
            var sut = GetSut("?sort=foo");

            var sort = sut.SortCriteria.Find(s => s.Key == "foo");

            Assert.Equal(SortDirection.Ascending, sort.SortDirection);
        }
    }

    class TestQueryableRequest : QueryableRequest<Person>
    {
        public TestQueryableRequest()
        {
            Sort("foo", p => p.Name);
        }
    }
}
