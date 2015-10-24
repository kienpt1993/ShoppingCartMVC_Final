using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopingCartEF;
namespace ShoppingCartMvc.Models
{
    public  class Category_Brand
    {
        public List<Category> cate = new List<Category>();
        public List<Brand> brand = new List<Brand>();
        public List<Product> pro = new List<Product>();
     
    }
}