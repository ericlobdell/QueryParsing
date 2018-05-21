﻿using Microsoft.AspNetCore.Mvc;
using QueryParser.Web.Requests;

namespace QueryParser.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Route("person")]
        public IActionResult Get(FilterablePersonRequest req)
        {
            return Ok( req.FilterPredicateMap.Keys );
        }

        [HttpGet]
        [Route("building")]
        public IActionResult Get(FilterableBuildingRequest req)
        {
            return Ok(req.FilterPredicateMap.Keys);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
