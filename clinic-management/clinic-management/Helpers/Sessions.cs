using clinic_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace clinic_management.Helpers
{
    public class Sessions
    {
        public class AuthorizationFilter : System.Web.Mvc.ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext != null)
                {
                    HttpSessionStateBase Session = filterContext.HttpContext.Session;
                    if (Session["staffid"] == null || Session["usertype"] == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                            { "controller", "Login" },
                            { "action", "Index" }
                            });
                    }
                }
            }
        }
    }
}