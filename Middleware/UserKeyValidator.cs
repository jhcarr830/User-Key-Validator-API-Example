using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ExampleAPI
{
    public class UserKeyValidator
    {
        private readonly RequestDelegate _next;

        public UserKeyValidator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            string userName = context.Request.Headers["user-name"];

            if (userName == null)
            {
                context.Response.StatusCode = 400; // bad request
                await context.Response.WriteAsync("User name is missing");
                return;
            }

            string userKey = context.Request.Headers["user-key"];

            if (userKey == null)
            {
                context.Response.StatusCode = 400; // bad request
                await context.Response.WriteAsync("User key is missing");
                return;
            }

            var user = Globals.userList.Find(usr => usr.UserName.ToLower() == userName.ToLower());

            if (user == null)
            {
                context.Response.StatusCode = 401; // unauthorized
                await context.Response.WriteAsync("Unknown User Name");
                return;
            }

            if (userKey.ToLower() != user.UserKey.ToLower())
            {
                context.Response.StatusCode = 401; // unauthorized
                await context.Response.WriteAsync("Invalid User Key");
                return;
            }

            Globals.CurrentUser = user;

            await _next.Invoke(context);
        }
    }
}
