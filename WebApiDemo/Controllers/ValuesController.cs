using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Caching;
using WebApiDemo.Common;

namespace WebApiDemo.Controllers
{
    public class ValuesController : ApiController
    {
        //RedisConnectionHelp con = new RedisConnectionHelp();
        [HttpGet]
       public IHttpActionResult SetStr(string key,string value)
        {
            RedisConnectionHelp con = new RedisConnectionHelp();
            var res = con.Redis(db => db.StringSet(key, value));
            return Ok(res);
        }

        [HttpGet]
        public IHttpActionResult SetGuid()
        {
            string key = Guid.NewGuid().ToString();
            string value = "adminTest";
            //try {
                RedisConnectionHelp con = new RedisConnectionHelp();
                var res = con.Redis(db => db.StringSet(key, value));
                return Ok(res);
            //}
            //catch(Exception ex)
            //{
            //    Function.WriteLog(ex.Message);
            //    Function.WriteLog(DateTime.Now + ": " + key);
            //    return Ok();
            //}
        }
    }
}
