using CTS.Applens.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [AllowAnonymous]
    [Route("[Controller]/[Action]")]
    public class CookieAuthController : ControllerBase
    {
        private static string KeySamlLoginValue => "CookieAuth:SAMLLoginUrl";
        private static string KeySamlLogoutValue => "CookieAuth:SAMLLogOutUrl";

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            returnUrl = System.Net.WebUtility.HtmlEncode(returnUrl);
            if (!string.IsNullOrEmpty(returnUrl))
            {
                var url = $"{Startup.Configuration.GetValue<string>(KeySamlLoginValue)}{returnUrl}";
                return Redirect(new SanitizeString(url).Value); //Veracode Fix
            }

            return null;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var url = $"{Startup.Configuration.GetValue<string>(KeySamlLogoutValue)}";
            return Redirect(url);
        }
    }
}
