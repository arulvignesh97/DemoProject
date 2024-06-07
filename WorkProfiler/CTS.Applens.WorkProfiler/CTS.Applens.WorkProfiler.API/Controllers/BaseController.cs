using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Extensions;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.API.Utilities;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using KeyVaultService;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;
using CTS.Applens.KeyVaultService;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("AzureADAuth")]
    public class BaseController : ControllerBase, IActionFilter
    {
        private string cognizantID;
        private bool _allowEncrypt;
        public static readonly string ConnectionString = new AppSettings().AppsSttingsKeyValues["ConnectionStrings:AppLensConnection"];
        readonly string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
        readonly string encryptionEnabledAPI = new AppSettings().AppsSttingsKeyValues["EncryptionEnabledAPI"];
        readonly string middlewareURL = new AppSettings().AppsSttingsKeyValues["MiddlewareURL"];
        readonly bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
        readonly bool isAppServiceEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["isAppServiceEnabled"], CultureInfo.CurrentCulture);
        CacheManager _cacheManager = new CacheManager();
        public string CognizantID
        {
            get { return cognizantID; }
            set { cognizantID = value; }
        }
        public bool AllowEncrypt
        {
            get { return _allowEncrypt; }
            set { _allowEncrypt = value; }
        }

        private HiddenFieldsModel objHiddenFieldsModel;
        public HiddenFieldsModel HiddenFields
        {
            get { return objHiddenFieldsModel; }
            set { objHiddenFieldsModel = value; }
        }

        public string SerializedData { get; set; }

        private List<RolePrivilegeModel> objListRolePrivilegeModel;

        public List<RolePrivilegeModel> RolePrivilege
        {
            get { return objListRolePrivilegeModel; }
            set { objListRolePrivilegeModel = value; }
        }

        private const string rolePrivilege = "RolePrivilege_{0}_{1}";
        
        //KeyCloak Added Property
        public string associateName { get; set; }
        // KeyCloak End

        #region Private readonly fields
        public readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostingEnvironment;
        #endregion

        public Associate CurrentUser => GetCurrentUser();

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="_httpContextAccessor"></param>
        public BaseController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor)
        {
            //Via Dependency Injection , Inject the configuration data from appsettings.json file

            _ = new DBContext(configuration);
            _ = new AppSettings(configuration);

            this.httpContextAccessor = _httpContextAccessor;
            this.configuration = configuration;
        }

        public BaseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _ = new DBContext(configuration);
            _ = new AppSettings(configuration);
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.hostingEnvironment = hostingEnvironment;
        }
        #endregion

        /// <summary>
        /// GetCurrentUser
        /// </summary>
        /// <returns></returns>
        [Route("GetCurrentUser")]
        public Associate GetCurrentUser()
        {
            try
            {
      
                string accessToken = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
                if (isAppServiceEnabled)
                {
                    string cognizantID = new KeyCloak().GetUserId(httpContextAccessor, configuration).Item1;
                }
                else if(isKeyCloakEnabled)
                {
                    (cognizantID, associateName) = new KeyCloak().GetUserId(httpContextAccessor, configuration);
                    //Kick out if the user is not Authenticated
                    if (string.IsNullOrEmpty(cognizantID))
                    {
                        return null;
                    }
                }
                else
                {
                    if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        return null;
                    }
                    //Take the User Id from the HttpContext
                    string cognizantID = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                    //string cognizantID = "934376";
                }
                EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                Associate associate = new Associate();
                associate = new AssociateDetails().GetCurrentUserDetails(StringOperations.RemoveDomain(cognizantID), associateName, accessToken, configuration);
                associate.Customers = objEffortTrackingRepository.GetProjectCustomer(cognizantID, 1);
                associate.Projects = objEffortTrackingRepository.GetProjectCustomer(cognizantID, 2);
                return associate;
                //return new AssociateDetails().GetCurrentUserDetails(StringOperations.RemoveDomain(cognizantID), associateName, accessToken,configuration);

            }
            catch(Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }

        }

        /// Calling the below functions only if the respective sessions are null to avoid the performance hit
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Avoid the performance hit
        }

        /// <summary>
        /// Calling the below functions only if the respective sessions are null to avoid the performance hit
        /// GetLanguageForUserlD
        /// GetCustomer
        /// GetHiddenFieldsForTM
        /// GetRolePrivilageMenusForAppLens
        /// </summary>
        /// <param name="filterContext"></param>

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (isAppServiceEnabled)
                {
                    if (new KeyCloak().GetUserId(httpContextAccessor, configuration).Item2 != null)
                    {
                        cognizantID = new KeyCloak().GetUserId(httpContextAccessor, configuration).Item1;
                    }
                }
                else if(isKeyCloakEnabled)
                {
                    (cognizantID, associateName) = new KeyCloak().GetUserId(httpContextAccessor, configuration);
                }
                else
                {
                    cognizantID = httpContextAccessor.HttpContext.User.Identity.Name.Contains("\\") ?
                                               httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]
                                              : string.Empty;
                }              
                if (!string.IsNullOrEmpty(cognizantID))
                {
                   _cacheManager.GetOrCreate<String>("UserId" + cognizantID, () => cognizantID, CacheDuration.Long);//
                    LanguageModel objLangModel = new LanguageModel();
                    var EmployeeID = cognizantID;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    TimeZoneInfoByCustomer objTimeZoneInfoByEmployeeID = new TimeZoneInfoByCustomer();
                   _cacheManager.GetOrCreate<String>("UserId" + cognizantID, () => cognizantID, CacheDuration.Long);
                    if (!_cacheManager.IsExists("CurrentCulture" + cognizantID))
                    {
                        objLangModel = objTicketingModuleRepository.GetLanguageForUserlD(EmployeeID);
                        if (objLangModel != null && objLangModel.Language != null)
                        {
                            _cacheManager.GetOrCreate<String>("CurrentCulture" + cognizantID, () => objLangModel.Language, CacheDuration.Long);
                        }
                    }

                    objHiddenFieldsModel = new HiddenFieldsModel();
                    List<CustomerModel> objListCustomerModel = new List<CustomerModel>();
                    if (!_cacheManager.IsExists("CustomerId" + cognizantID))
                    {
                        objListCustomerModel = objEffortTrackingRepository.GetCustomer(cognizantID);
                    }

                    Int32 CustomerID;

                    if (objListCustomerModel.Count > 0)
                    {
                        CustomerID = Convert.ToInt32(_cacheManager.GetOrCreate<String>
                            ("CustomerId" + cognizantID, () => objListCustomerModel[0].CustomerId, 
                            CacheDuration.Long), CultureInfo.CurrentCulture);

                        if (!_cacheManager.IsExists("UserWiseCustomer" + cognizantID))
                        {
                            objListCustomerModel = _cacheManager.GetOrCreate<List<CustomerModel>>("UserWiseCustomer" + cognizantID, () => objListCustomerModel, CacheDuration.Long);
                        }

                        HiddenFieldsModel hiddenFldModel = new HiddenFieldsModel();
                        if (!_cacheManager.IsExists("objHiddenFieldsModel" + cognizantID))
                        {
                            hiddenFldModel = objEffortTrackingRepository.GetHiddenFieldsForTM(cognizantID, CustomerID);
                        }

                        objHiddenFieldsModel.EmployeeId = cognizantID;
                        objHiddenFieldsModel = _cacheManager.GetOrCreate<HiddenFieldsModel>("objHiddenFieldsModel" + cognizantID, () => hiddenFldModel, CacheDuration.Long);

                        if (!_cacheManager.IsExists("TimeZoneInfoByEmployeeID" + cognizantID))
                        {
                            objTimeZoneInfoByEmployeeID = objTicketingModuleRepository.GetTimeZoneInformationByCustomer(cognizantID, CustomerID);
                        }

                        objTimeZoneInfoByEmployeeID = _cacheManager.GetOrCreate<TimeZoneInfoByCustomer>
                        ("TimeZoneInfoByEmployeeID" + cognizantID, () => objTimeZoneInfoByEmployeeID, 
                        CacheDuration.Long);

                        objListRolePrivilegeModel = new List<RolePrivilegeModel>();
                        if (!_cacheManager.IsExists("objListRolePrivilegeModel" + cognizantID))
                        {
                            objListRolePrivilegeModel =
                                objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(cognizantID, CustomerID);
                        }
                        
                        if (encryptionEnabled == "Enabled" || encryptionEnabledAPI == "Enabled" )
                        {
                                
                            try
                            {
                                //var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                                //var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                                //using (var client = httpClientFactory.CreateClient())
                                //{ 
                                //    client.BaseAddress = new Uri(middlewareURL);
                                //    HttpResponseMessage res = client.GetAsync($"values/get").Result;
                                //    string key = res.Content.ReadAsAsync<string>().Result;
                                string userId = new AppSettings().AppsSttingsKeyValues["AzureServiceUserID"].ToString();
                                string domainName = new AppSettings().AppsSttingsKeyValues["DomainName"].ToString();
                                string key = MiddlewareKey.GetKeyVaultAsync(userId, domainName, ConnectionString).Result;
                                _cacheManager.GetOrCreate<byte[]>("aesKeyconst", () => Convert.FromBase64String(key), CacheDuration.Long);
                                    AllowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + cognizantID, () => "true", CacheDuration.Long), CultureInfo.CurrentCulture);
                                // }
                            }
                            catch (Exception ex)
                            {
                                string key = new AppSettings().AppsSttingsKeyValues["Middlewarekey"].ToString();
                                _cacheManager.GetOrCreate<byte[]>("aesKeyconst", () => Convert.FromBase64String(key), CacheDuration.Long);
                                AllowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + cognizantID, () => "true", CacheDuration.Long), CultureInfo.CurrentCulture);
                            }
                        }
                        else
                        {
                           // byte[] aesKey = new byte[32];
                          //  _cacheManager.GetOrCreate<byte[]>("aesKeyconstAPI", () => aesKey, CacheDuration.Long);
                          //  AllowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + cognizantID, () => "true", CacheDuration.Long), CultureInfo.CurrentCulture);
                          //  stringBuilder.Append("Entered ELSE block. Allow Encrypt : " + AllowEncrypt + "\n");
                           // System.IO.File.AppendAllText(filename, stringBuilder.ToString());
                        }
                            

                        if (objHiddenFieldsModel.LstProjectUserID.Count == 0)
                        {
                            return;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
            }
        }

        /// <summary>
        /// This method is used to GetHiddenfields
        /// </summary>
        /// <param name="customerId">This parameter holds customerId value</param>
        /// <returns></returns>
        public string GetHiddenfields(int customerId)
        {
            SetSessionFixation("CustomerId", customerId.ToString(CultureInfo.CurrentCulture));
            return "Success";
        }
        /// <summary>
        /// This method is used to SetSessionFixation
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="sessionValue"></param>
        /// <param name="old_Context"></param>
        private void SetSessionFixation(string sessionKey, string sessionValue)
        {
            HttpContext context = httpContextAccessor.HttpContext;
            context.Session.SetString(sessionKey, sessionValue);
        }
        /// <summary>
        /// This method is used to GetRoles
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="customerid"></param>
        /// <returns>Method returns Roles</returns>
        [HttpGet]
        [Route("GetRoles")]
        public List<Roles> GetRoles(string mode, int customerid, string userid)
        {
            try
            {
                TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                var resp = objTicketingModuleRepository.GetRoles(new SanitizeString(mode).Value,new SanitizeString(userid).Value, customerid);

                return resp;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }
        /// <summary>
        /// This Method is used to Change Current Language Culture.
        /// </summary>
        /// <returns></returns>
        public int ChangeCurrentLangCulture(string id)
        {
            httpContextAccessor.HttpContext.Session.SetString("CurrentCulture", id.ToString(CultureInfo.CurrentCulture));
            return 1;
        }

        /// Get My Assosicate URL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMyAssociateURL")]
        public string GetMyAssociateURL()
        {
            return new AppSettings().AppsSttingsKeyValues["MyAssociateURL"].ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get Home Page URL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHomePage")]
        public string GetHomePage()
        {
            return new AppSettings().AppsSttingsKeyValues["HomePage"].ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get nav menu URL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNavMenuUrl")]
        public Dictionary<string, string> GetNavMenuUrl()
        {

            return new Dictionary<string, string>
            {
                ["qlikSenseUrl"] = new AppSettings().AppsSttingsKeyValues["QlikSenseUrl"].ToString(CultureInfo.CurrentCulture),
                ["detailedTimesheetReportUrl"] = new AppSettings().AppsSttingsKeyValues["DetailedTimesheetReportUrl"].ToString(CultureInfo.CurrentCulture),
                ["sprintReviewClosureUrl"] = new AppSettings().AppsSttingsKeyValues["SprintReviewClosureUrl"].ToString(CultureInfo.CurrentCulture),
                ["smartExeMetricsUrl"] = new AppSettings().AppsSttingsKeyValues["SmartExeMetricsUrl"].ToString(CultureInfo.CurrentCulture),
                ["basemeasureReportUrl"] = new AppSettings().AppsSttingsKeyValues["BasemeasureReportUrl"].ToString(CultureInfo.CurrentCulture),
                ["servicePerformanceReportUrl"] = new AppSettings().AppsSttingsKeyValues["ServicePerformanceReportUrl"].ToString(CultureInfo.CurrentCulture),
            };
        }

        /// <summary>
        /// Used To Download Template
        /// </summary>
        /// <param name="Path">Path</param>
        /// <param name="IsMacroTemplate">Template Type</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DownloadTemplate")]
        public FileResult DownloadTemplate(DownloadRequest request)
        {
            try
            {
                if (request != null && !string.IsNullOrEmpty(request.Path))
                {
                    string contentType = string.Empty;
                    string Type = string.Empty;
                    contentType = request.IsMacroTemplate == true ? "application/octet-stream" : "application/vnd.ms-excel";
                    Type = request.IsMacroTemplate == true ? ".xlsm" : ".xlsx";
                    
                    if (!string.IsNullOrEmpty(request.Path))
                    {
                        var path = RegexPath(request.Path);
                        //var path = request.Path;
                        SanitizeStringInput sanitize = new SanitizeStringInput(path);
                        string dirctoryName = System.IO.Path.GetDirectoryName(sanitize.Value);
                        string fileNameWoExt = System.IO.Path.GetFileNameWithoutExtension(sanitize.Value);
                        string validatePath = System.IO.Path.Combine(dirctoryName, fileNameWoExt, Type);
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        //string validatedPath = RegexPath(validatePath);
                        string validatedPath = validatePath;
                        if (System.IO.File.Exists(validatedPath))
                        {
                            //string validatedPath1 = RegexPath(validatedPath);
                            string validatedPath1 = validatedPath;
                            validatedPath1 = RegexPath(validatedPath);
                            byte[] fileBook = System.IO.File.ReadAllBytes(validatedPath1);
                            var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                            {
                                FileDownloadName = System.IO.Path.GetFileName(validatedPath1)
                            };
                            return fileContentResult;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns></returns>
        private string RemoveLastIndexCharacter(string path)
        {
            if (path.Length > 0)
            {
                int place = path.LastIndexOf(@"\");
                string result = path.Remove(place, (@"\").Length);
                return result;
            }
            else
            {
                return "";
            }
        }

        private string RegexPath(string fullPath)
        {
            Regex rgxPath = new Regex("(\\\\?([^\\/]*[\\/])*)([^\\/]+)");
            if (fullPath != null)
            {
                fullPath = fullPath.Replace(">", "");
                fullPath = fullPath.Replace("<", "");
                fullPath = fullPath.Replace("..", "");
                return fullPath;
            }
            else
            {
                return "";
            }
        }

        protected void LogError(ErrorLogDetails errorLogDetails, Exception ex)
        {
            errorLogDetails.LogSeverity = Convert.ToString(LogSeverity.Medium, CultureInfo.CurrentCulture);
            errorLogDetails.LogLevel = Convert.ToString(LogLevel.Error, CultureInfo.CurrentCulture);
            errorLogDetails.HostName = Environment.MachineName;
            errorLogDetails.CreatedDate = DateTimeOffset.Now.DateTime.ToString(CultureInfo.CurrentCulture);
            errorLogDetails.Technology = Convert.ToString(Technology.DotNetCore, CultureInfo.CurrentCulture);
            errorLogDetails.ModuleName = Constants.ApplicationName;
            errorLogDetails.ProcessId = Process.GetCurrentProcess().Id;
            errorLogDetails.ErrorCode = Convert.ToString(HttpStatusCode.InternalServerError, CultureInfo.CurrentCulture);
            errorLogDetails.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
            errorLogDetails.StackTrace = ex.Message;
            errorLogDetails.AdditionalField_1 = string.Empty;
            errorLogDetails.AdditionalField_2 = string.Empty;
            LoggerAPI.LogError(ex, errorLogDetails);
        }

    }
}
