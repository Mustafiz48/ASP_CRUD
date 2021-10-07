using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASP_CRUD.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ASP_CRUD.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;

        }




        public async Task Invoke(HttpContext httpContext)
        {
            // var controller = httpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
           // var path = httpContext.Request.Path;
           /* if (httpContext.User.Identity.IsAuthenticated)
            {
                var identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var name = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault()?.Value;

                    httpContext.Items["path"] = path;
                    httpContext.Items["name"] = name;

                }


            }*/

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
