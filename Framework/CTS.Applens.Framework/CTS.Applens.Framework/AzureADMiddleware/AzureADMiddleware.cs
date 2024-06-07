using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.Framework
{
    public class AzureADMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public static string accesstoken = "";
        public static CookieContainer cookie;

        public AzureADMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;

            _logger = logFactory.CreateLogger("MyMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey("X-MS-TOKEN-AAD-ACCESS-TOKEN") )
            {
                accesstoken=httpContext.Request.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"][0];
            }
            var cookieContainer = new CookieContainer();
            var uriString = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            foreach (var c in httpContext.Request.Cookies)
            {
                cookieContainer.Add(new Uri(uriString), new Cookie(c.Key, c.Value));
            }
            cookie = cookieContainer;
            await _next(httpContext); // calling next middleware

        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseAzureADMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AzureADMiddleware>();
        }
    }
}







