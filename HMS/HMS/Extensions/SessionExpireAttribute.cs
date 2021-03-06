using HMS.Extensions;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HMS.Extensions
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var user = context.HttpContext.Session.Get<User>("User");
            if (user == null)
            {
                context.Result = new RedirectResult("/Home/Logout");
            }
            base.OnActionExecuted(context);
        }
    }
}
