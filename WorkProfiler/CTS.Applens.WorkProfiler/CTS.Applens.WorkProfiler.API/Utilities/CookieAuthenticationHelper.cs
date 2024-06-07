using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.API.Utilities
{
    public class CookieAuthenticationHelper
    {
        private static string KeyPurpose => "CookieAuth:Purpose";
        private static string KeyCertificate => "CookieAuth:Certificate";
        private static string KeyCookieName => "CookieAuth:CookieName";
        private static string KeyCookieDomain => "CookieAuth:CookieDomain";
        private static string KeyCookieLogin => "CookieAuth:CookieLogin";
        private static string KeyCertificatePass => "CookieAuth:CertificatePassword";
        private static string KeyDirectoryPath => "CookieAuth:KeyDirectoryPath";
        private static string KeyCookieLogout => "CookieAuth:CookieLogOut";
        private static string KeyCookieTimeout => "CookieAuth:CookieTimeOut";
        private static string KeyJsonPath => "CookieAuthJsonPath:CookieFarm";



        public static void Configure(IServiceCollection services)
        {
            // the purpose must match with SAML app
            string _dataProtectorPurpose = Startup.Configuration.GetValue<string>(KeyPurpose);

            IDataProtectionProvider dpProvider = DataProtectionProvider.Create(new DirectoryInfo(Startup.Configuration.GetValue<string>(KeyDirectoryPath)),
                setup => setup.DisableAutomaticKeyGeneration());

            IDataProtector dp = dpProvider.CreateProtector(_dataProtectorPurpose);


            #region CookieAuthentication 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.Cookie.HttpOnly = true;
                            options.Cookie.IsEssential = true;
                            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                            options.Cookie.SameSite = SameSiteMode.Lax;
                            options.Cookie.Name = Startup.Configuration.GetValue<string>(KeyCookieName);
                            options.Cookie.Domain = Startup.Configuration.GetValue<string>(KeyCookieDomain) != null ? Startup.Configuration.GetValue<string>(KeyCookieDomain) : null;
                            options.Cookie.Path = "/";
                            options.LoginPath = new PathString(Startup.Configuration.GetValue<string>(KeyCookieLogin));
                            options.LogoutPath = new PathString(Startup.Configuration.GetValue<string>(KeyCookieLogout));
                            options.ExpireTimeSpan = TimeSpan.FromMinutes(Startup.Configuration.GetValue<double>(KeyCookieTimeout));
                            options.SlidingExpiration = true;
                            options.TicketDataFormat = new TicketDataFormat(dp);
                            options.Events = new CookieAuthenticationEvents
                            {
                            };
                        });
            #endregion

        }

        public static IConfiguration ConfigurationBuilder(IConfiguration Configuration)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            builder.AddJsonFile(Environment.GetEnvironmentVariable(Configuration.GetValue<string>(KeyJsonPath)));
            Configuration = builder.Build();
            return Configuration;
        }
    }
}
