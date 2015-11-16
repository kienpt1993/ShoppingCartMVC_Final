using ShopingCartEF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoppingCartMvc.API
{
    public class StudentController : ApiController
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();
        // GET api/<controller>
        public List<Student> Get()
        {
            return db.Students.ToList();
        }

        // GET api/<controller>/5
        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        // POST api/<controller>
        public void Post([FromBody]Student model)
        {
            db.Students.Add(model);
            db.SaveChanges();
        }

        // PUT api/<controller>/5
        public void Put([FromBody]Student model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            Student product = db.Students.Find(id);
            db.Students.Remove(product);
            db.SaveChanges();
        }
    }
}