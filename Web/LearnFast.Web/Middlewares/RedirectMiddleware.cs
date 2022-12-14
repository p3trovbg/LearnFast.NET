namespace LearnFast.Web.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class RedirectMiddleware
    {
        private readonly RequestDelegate next;

        public RedirectMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/Identity/Account/Login" ||
                context.Request.Path == "/Identity/Account/Register")
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect($"/{context.User.Identity.Name}");
                }
            }

            await this.next.Invoke(context);
        }
    }
}
