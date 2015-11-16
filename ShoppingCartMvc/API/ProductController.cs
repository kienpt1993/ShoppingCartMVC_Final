using ShopingCartEF;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoppingCartMvc
{
    public class ProductController : ApiController
    {

        private ShoppingCartEntities db = new ShoppingCartEntities();
        // GET api/<controller>
        public List<News> Get()
        {
            var product = db.News;
            return product.ToList();
        }
        // ADO 
        // ORM (EF Bltoolkit, Dapper

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}