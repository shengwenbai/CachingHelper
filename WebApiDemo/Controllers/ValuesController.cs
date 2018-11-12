using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Caching;

namespace WebApiDemo.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            if (Cache.Exists(id.ToString()))
                return Cache.Get<string>(id.ToString());
            else
                return "notfound";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            Cache.Insert<string>(id.ToString(),value,600);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
