using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShoppingCartMvc.Filters
{
    public class CustomerLoginFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /// kiem tra session cua khach, neu chua yeu co yeu cau dang nhap
            /// 
            var session = filterContext.HttpContext.Session["Customer"];
            Controller controller = filterContext.Controller as Controller;
            if(controller != null)
            {
                if(session == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary {
                        { "controller", "Customer" },
                        {"action","Login" },
                        {"returnUrl", controller.HttpContext.Request.Url.AbsolutePath }
                    });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
