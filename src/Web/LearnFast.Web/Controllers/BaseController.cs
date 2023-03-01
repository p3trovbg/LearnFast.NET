namespace LearnFast.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
