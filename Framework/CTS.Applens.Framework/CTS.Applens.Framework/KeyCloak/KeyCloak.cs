using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace CTS.Applens.Framework
{
    public class KeyCloak
    {
        /// <summary>
        /// Method to get UserId if KeyCloakEnabled or Not, if keyCloakEnabled it will fetch from respective 
        /// User claim value else will be User's FirstorDefaultValue.
        /// Note: Take two params => (IHttpContextAccessor| HttpContext) typed firstparam and IConfiguration typed secondparam
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContextData">Handled as Generic Type inorder to Handle both IHttpContextAccessor or HttpContextType</param>
        /// <param name="configuration">IConfigurationTyped configdata</param>
        /// <returns></returns>
        public (string, string) GetUserId<T>(T httpContextData, IConfiguration configuration)
        {
            string userId = null;
            string associateName = null;
            bool isKeyCloakEnabled = configuration.GetValue<bool>(KeyCloakConstants.KeyCloakEnabled);
            bool isAppServiceEnabled = configuration.GetValue<bool>("isAppServiceEnabled");
            string authProvider = configuration.GetSection(KeyCloakConstants.KeyCloakOidcAuthProvider).Value;
            string splitter = configuration.GetSection(KeyCloakConstants.KeyCloakOidcSplitter).Value;
            string PrincipalName = configuration.GetSection(KeyCloakConstants.PrincipalName).Value;
            PropertyInfo prop = httpContextData.GetType().GetProperty("HttpContext");
            var httpContextValue = (HttpContext)prop.GetValue(httpContextData);
            if (httpContextValue != null)
            {
                if (isKeyCloakEnabled)
                {

                    switch (authProvider)
                    {
                        case "oidc":

                            var UserIdClaim = httpContextValue.User.Claims.FirstOrDefault(x => x.Type
                                                                                 .ToUpperInvariant() == PrincipalName.ToUpperInvariant());
                            if(UserIdClaim != null)
                            {
                                userId = UserIdClaim.Value.Split(splitter)[0];
                            }
                            else
                            {
                                userId = httpContextValue.User.Claims.FirstOrDefault(x => x.Type
                                                                                 .ToUpperInvariant() == KeyCloakConstants.PreferredUserName.ToUpperInvariant())
                                                                                .Value;
                                associateName = userId;
                                break;
                            }
                            associateName = httpContextValue.User.Claims.
                                First(x => x.Type.ToLower(CultureInfo.CurrentCulture).Contains(KeyCloakConstants.GivenName, StringComparison.InvariantCultureIgnoreCase)).Value;

                            break;

                        case "saml":
                            userId = httpContextValue.User.Claims.FirstOrDefault(x => x.Type.ToLower() == KeyCloakConstants.PreferredUserName).Value.Split(splitter)[0];
                            associateName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}",
                                httpContextValue.User.Claims.FirstOrDefault(x => x.Type.ToUpperInvariant() == KeyCloakConstants.SurName.ToUpperInvariant()).Value,
                                httpContextValue.User.Claims.FirstOrDefault(x => x.Type.ToUpperInvariant() == KeyCloakConstants.GivenName.ToUpperInvariant()).Value);
                            break;

                        case "AzureAd":
                            userId = httpContextValue.User.Claims.FirstOrDefault(x => x.Type.ToLower() == KeyCloakConstants.PreferredUserName).Value.Split(splitter)[0];
                            associateName = httpContextValue.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name").Value.Split("(")[0].Trim();
                            break;

                        default:
                            break;
                    }
                }
                else if (isAppServiceEnabled)
                {
                    if (httpContextValue.Request.Headers.ContainsKey("X-MS-TOKEN-AAD-ACCESS-TOKEN"))
                    {
                        var azureToken = httpContextValue.Request.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"][0];
                        (userId, associateName) = ValidateToken(azureToken, splitter);
                    }
                }
                else if (!isKeyCloakEnabled && httpContextValue.User.Identity.IsAuthenticated)
                {
                    //Take the User Id from the HttpContext
                    userId = httpContextValue.User.Claims.FirstOrDefault().Value;
                }
                else
                {
                    return (null, null);
                }
            }
            return (userId, associateName);
        }
        private static (string, string) ValidateToken(string token, string splitter)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            if (jwtSecurityToken != null)
            {
                var name = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "name").Value;
                var id = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "upn").Value.Split(splitter)[0];
                return (id, name);
            }
            return ("", "");
        }
    }
}
