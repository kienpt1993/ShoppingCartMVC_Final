using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartMvc.Models
{
    public class ContactModel
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}