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
        [HttpGet]
       public IHttpActionResult SetStr(string key,string value)
        {
            RedisConnectionHelp con = new RedisConnectionHelp();
            var res = con.Redis(db => db.StringSet(key, value));
            return Ok(res);
        }
    }
}
