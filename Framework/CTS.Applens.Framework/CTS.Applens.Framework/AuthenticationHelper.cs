using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;

namespace CTS.Applens.Framework
{
    public class AuthenticationHelper
    {
        public static void Configure(IServiceCollection services, string clientId, string authority)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Authority = authority;
                   options.Audience = clientId;
                   options.IncludeErrorDetails = true;

                   options.RequireHttpsMetadata = false;

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false,
                       ValidateIssuer = true,
                       ValidIssuer = authority,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero
                   };
               }).AddCookie(opts =>
               {
                   opts.Cookie.HttpOnly = true;
                   opts.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
               });
        }
    }
}
