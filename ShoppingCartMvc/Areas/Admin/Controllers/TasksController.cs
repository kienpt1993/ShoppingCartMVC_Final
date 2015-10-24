using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartMvc.Areas.Admin.Controllers
{
    public class TasksController : Controller
    {
        // GET: Admin/Tasks
        public ActionResult Index()
        {
            return View();
        }
    }
}