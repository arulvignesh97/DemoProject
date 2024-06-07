using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.API.Utilities;
using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using Prometheus;

namespace CTS.Applens.WorkProfiler.API
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder()
          .SetBasePath(System.IO.Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));            
            new AppSettings(configuration);
            Configuration = configuration;
            if (Configuration.GetValue<bool>(Constants.KeyEnableSaml))
            {
                //Enable only for SAML                
                Configuration = CookieAuthenticationHelper.ConfigurationBuilder(Configuration);
            }
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IGraphAPI, GraphAPI>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IApplensLogging, ApplensLogging>();

            services.AddCors();
            services.AddHsts(option => {
                option.Preload = false;
                option.IncludeSubDomains = true;
                option.MaxAge = System.TimeSpan.FromDays(365);
            });
            if (Configuration.GetValue<bool>("KeyCloakEnabled"))
            {
                AuthenticationHelper.Configure(services, Configuration["KeyCloakOidc:ClientId"],
                     string.Concat(Configuration["KeyCloakOidc:Authority"], Configuration["KeyCloakOidc:Realm"]));
            }
            else if (Configuration.GetValue<bool>("EnableAzureADSAML")) 
            {
                CookieAuthenticationHelper.Configure(services);
                services.AddMvc(o =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                    o.Filters.Add(typeof(BaseExceptionFilter));
                });
            }
            else
            {
                services.AddAuthentication(IISDefaults.AuthenticationScheme)
                /*  .AddCookie(opts =>
                  {
                      opts.Cookie.HttpOnly = true;
                  });*/;
             
            }
            AuthorizeHandler.Configure(services, Configuration.GetValue<bool>("isAppServiceEnabled"));
            services.AddControllers()
                   .AddNewtonsoftJson(options =>
                   {
                       options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpMetrics(); 
            app.UseMetricServer();
            if (Configuration.GetValue<bool>("isAppServiceEnabled"))
            {
                app.UseAzureADMiddleware();
            }
            app.UseAuthentication();
            app.UseRouting();
            ///app.UseSession();
            app.UseCors(
              options => options.SetIsOriginAllowed(x => _ = true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
