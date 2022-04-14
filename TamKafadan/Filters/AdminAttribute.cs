using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TamKafadan.Filters
{
    public class AdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {            
            var admin = context.HttpContext.Session.GetString("admin");
            if (string.IsNullOrEmpty(admin))
            {
                admin = context.HttpContext.Request.Cookies["admin"];
                if (string.IsNullOrEmpty(admin))
                {
                    context.Result = new RedirectToActionResult("Index", "Dashboard", "Admin");

                }
            }
        }
    }
}
