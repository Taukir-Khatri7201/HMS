using HMS.Extensions;
using HMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class BaseController : Controller
    {
        public User loggedUser;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            loggedUser = httpContextAccessor.HttpContext.Session.Get<User>("User");
            if (loggedUser == null) loggedUser = new User();
        }
    }
}
