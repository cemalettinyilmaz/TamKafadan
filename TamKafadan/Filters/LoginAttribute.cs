using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TamKafadan.Filters
{
    public class LoginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.Session.GetString("kullaniciAdi");
            if (string.IsNullOrEmpty(email))
            {
                email = context.HttpContext.Request.Cookies["kullaniciAdi"];
                if (string.IsNullOrEmpty(email))
                {
                    context.Result = new RedirectToActionResult("GirisEmail", "Giris", "");

                }
            }
        }
    }
}
