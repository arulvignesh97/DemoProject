using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.Framework
{
    public class AuthorizeHandler : AuthorizationHandler<AuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement)
        {
                
            if (requirement.Auth  || context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
        }
        public static void Configure(IServiceCollection services, bool isAppServiceEnabled)
        {
            services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AzureADAuth", policy => policy.Requirements.Add(new AuthRequirement
                {
                    Auth = isAppServiceEnabled,
                }));
            });
        }
    }
    public class AuthRequirement : IAuthorizationRequirement
    {
        public bool Auth { get; set; }
    }
    
}
