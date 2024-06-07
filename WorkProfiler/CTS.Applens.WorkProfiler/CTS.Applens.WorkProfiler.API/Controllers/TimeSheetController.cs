using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Configuration;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("AzureADAuth")]
    public class TimeSheetController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
        readonly bool isAppServiceEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["isAppServiceEnabled"], CultureInfo.CurrentCulture);
        CacheManager _cacheManager = new CacheManager();
        //KeyCloak Added
        private readonly IConfiguration _configuration;
        //KeyCok ENd

        /// <summary>
        /// TimeSheetController
        /// </summary>
        public TimeSheetController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnvironment) :
            base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
            this._httpContextAccessor = _httpContextAccessor;
            this._configuration = configuration;
        }

        /// GET: EffortManagement
        /// <summary>
        /// This method is used to return initial page
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]

        public UserProjectDetailsBaseModel Index()
        {
           
            UserProjectDetailsBaseModel userDetailsBaseModel = new UserProjectDetailsBaseModel();
            try
            {
                string employeeId;
                if (isAppServiceEnabled)
                {
                    employeeId = new KeyCloak().GetUserId(httpContextAccessor,_configuration).Item1;
                }
                else if (isKeyCloakEnabled)
                {
                    //KeyCloak Added Code
                    (employeeId, _) = new KeyCloak().GetUserId(_httpContextAccessor, _configuration);
                    employeeId ??= string.Empty;
                    //KeyCloak End
                }
                else
                {
                    employeeId = _httpContextAccessor.HttpContext.User.Identity.Name.Contains("\\") ?
                                                  _httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]
                                                 : string.Empty;
                }
                HiddenFieldsModel objHiddenFieldsModel1 = _cacheManager.GetOrCreate<HiddenFieldsModel>("objHiddenFieldsModel" + employeeId, () => new HiddenFieldsModel(), CacheDuration.Long);
                userDetailsBaseModel.CustomerId = HiddenFields.CustomerId;
                userDetailsBaseModel.EmployeeName = HiddenFields.EmployeeName;
                userDetailsBaseModel.HiddenFields = HiddenFields;
                userDetailsBaseModel.RolePrevilageMenus = RolePrivilege;
                userDetailsBaseModel.HiddenUserProjectDetails = HiddenFields.LstProjectUserID;
                userDetailsBaseModel.UnfreezeGracePeriod = new ApplicationConstants().UnfreezeDay;
                HiddenFieldsModel objHiddenFieldsModel;
                if (_cacheManager.IsExists("CustomerId" + employeeId) && Convert.ToInt32((_cacheManager.GetOrCreate<string>("CustomerId" + employeeId,
                    null, CacheDuration.Long)) == "" ? null : (_cacheManager.GetOrCreate<string>
                    ("CustomerId" + employeeId, null, CacheDuration.Long)), CultureInfo.CurrentCulture) != 0)
                {
                    Int32 CustomerID = 0;
                    string value = _cacheManager.GetOrCreate<string>("CustomerId" + employeeId, () => string.Empty, CacheDuration.Long);
                    if (value != "")
                    {
                        CustomerID = Convert.ToInt32(value, CultureInfo.CurrentCulture);
                    }
                    String cognizantID = Convert.ToString(_cacheManager.GetOrCreate<string>("UserId" + employeeId, () => string.Empty, CacheDuration.Long), CultureInfo.CurrentCulture);
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFieldsForTM(cognizantID, CustomerID);
                }
                else
                {
                    objHiddenFieldsModel = HiddenFields;
                }

                if (objHiddenFieldsModel.IsEffortConfigured == 1)
                {
                    userDetailsBaseModel.ErrorMessage = "Index";
                }
                else
                {
                    userDetailsBaseModel.ErrorMessage = "_NotAuthorized";
                }

                TimeZoneInfoByCustomer objTimeZoneInfoByEmployeeID = new TimeZoneInfoByCustomer();
                if (_cacheManager.IsExists("TimeZoneInfoByEmployeeID" + employeeId))
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    objTimeZoneInfoByEmployeeID = objTicketingModuleRepository.GetTimeZoneInformationByCustomer(HiddenFields.EmployeeId,
                        Convert.ToInt64(HiddenFields.CustomerId, CultureInfo.CurrentCulture));
                }

                objTimeZoneInfoByEmployeeID = _cacheManager.GetOrCreate<TimeZoneInfoByCustomer>("TimeZoneInfoByEmployeeID" + employeeId, () => objTimeZoneInfoByEmployeeID, CacheDuration.Long);
                List<CustomerModel> objListCustomerModel = _cacheManager.GetOrCreate<List<CustomerModel>>("UserWiseCustomer" + employeeId, () => new List<CustomerModel>(), CacheDuration.Long);
                if (userDetailsBaseModel.RolePrevilageMenus == null || userDetailsBaseModel.RolePrevilageMenus.Count <= 0)
                {
                    List<RolePrivilegeModel> objListPrevilageModel;
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    Int32 CustomerID = 0;
                    string value = _cacheManager.GetOrCreate<string>("CustomerId" + employeeId, () => string.Empty, CacheDuration.Long);
                    if (value != "")
                    {
                        CustomerID = Convert.ToInt32(value, CultureInfo.CurrentCulture);
                    }

                    objListPrevilageModel =
                                objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(employeeId, CustomerID);

                    userDetailsBaseModel.RolePrevilageMenus = objListPrevilageModel;
                }

                userDetailsBaseModel.UserWiseCustomer = objListCustomerModel;
                userDetailsBaseModel.TimeZoneInfoByEmployeeID = objTimeZoneInfoByEmployeeID;
                objHiddenFieldsModel.IsADMApplicableforCustomer = new AppSettings().AppsSttingsKeyValues["IsADMApplicableforCustomer"];
                objHiddenFieldsModel.IsExtended = new AppSettings().AppsSttingsKeyValues["IsExtended"];
                objHiddenFieldsModel.ChooseDaysCount = new AppSettings().AppsSttingsKeyValues["ChooseDaysCount"];
                userDetailsBaseModel.HiddenFields = objHiddenFieldsModel;
                return userDetailsBaseModel;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = CurrentUser.ID;
                LogError(errorLogDetails, ex);

                return null;
            }
        }

        /// <summary>
        /// This Method is used to GetTimeSheetData
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTimeSheetDataDaily")]
        public ActionResult<List<TimeSheetDataDaily>> GetTimeSheetDataDaily(ApprovalUnfreezeInputParams InputData)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,Convert.ToInt64(InputData.CustomerId),null);
            try
            {
                if (value)
                {
                    List<TimeSheetDataDaily> model;
                    model = new TimeSheetRepository().GetTimeSheetDataDaily(InputData);
                    return model;
                }
                return Unauthorized();
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
        /// This Method is used to GetTimeSheetData
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTimeSheetDataWeekly")]
        public ActionResult<List<TimeSheetData>> GetTimeSheetDataWeekly(ApprovalUnfreezeInputParams InputData)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,Convert.ToInt64(InputData.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<TimeSheetData> model;
                    model = new TimeSheetRepository().GetTimeSheetData(InputData);
                    return model;
                }
                return Unauthorized();
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
        /// This Method is used to GetAssignessDownload
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAssignessDownload")]
        public ActionResult<string> GetAssignessDownload(ApprovalUnfreezeInputParams InputData)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null, Convert.ToInt64(InputData.CustomerId),null);
            try
            {
                if (value)
                {
                    if (!ModelState.IsValid)
                    {
                        ModelState.Clear();
                    }
                    string path = string.Empty;
                    if (InputData.IsDaily == 1)
                    {
                        List<TimeSheetDataDaily> model;
                        model = new TimeSheetRepository().GetTimeSheetDataDaily(InputData);
                        path = new ContinuousLearningRepository().ExportToExcelTEST(model);
                    }
                    else
                    {
                        List<TimeSheetData> model;
                        model = new TimeSheetRepository().GetTimeSheetData(InputData);
                        path = new ContinuousLearningRepository().ExportToExcelWeekly(model);
                    }

                    return path;
                }
                return Unauthorized();
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
        /// This Method is used to GetAssignessOrDefaulters
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAssignessOrDefaulters")]
        public ActionResult<ApproveUnfreeze> GetAssignessOrDefaulters(ApprovalUnfreezeInputParams2 InputData)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, InputData.EmployeeId.ToString(),Convert.ToInt64(InputData.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    ApproveUnfreeze assigness = new ApproveUnfreeze();
                    assigness.AssignessOrDefaulters = new TimeSheetRepository().GetAssignessOrDefaulters(InputData);
                    assigness.UnfreezeGracePeriod = new ApplicationConstants().UnfreezeDay;
                    return assigness;
                }
                return Unauthorized();
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
        /// This Method is used to UpdateTimeSheetData
        /// </summary>
        /// <param name="lstApproveUnfreezeTimesheet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateTimeSheetData/{isDaily}/{userid}")]
        public bool UpdateTimeSheetData(bool isDaily, List<ApproveUnfreezeTimesheet> lstApproveUnfreezeTimesheet, string userid)
        {
            try
            {
                bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
                string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
                Int64 customerID = lstApproveUnfreezeTimesheet.Select(x => x.CustomerId).FirstOrDefault();
                var result = new TimeSheetRepository().UpdateTimeSheetData(lstApproveUnfreezeTimesheet, customerID, isDaily, userid, access);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return false;
            }
        }
        /// <summary>
        /// This Method is used to GetCalendarView
        /// </summary>
        /// <returns></returns>
        public static string GetCalendarView()
        {
            return new ApplicationConstants().UnfreezeDay;
        }

        /// <summary>
        /// This Method is used to GetCalendarData
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCalendarData")]
        public ActionResult<List<CalendarViewData>> GetCalendarData(ApprovalUnfreezeInputParams2 InputData)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null, Convert.ToInt64(InputData.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    var model = new TimeSheetRepository().GetCalendarData(InputData);
                    return model;
                }
                return Unauthorized();
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
        /// This Method is used to QlikSense
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpGet]
        public string QlikSense(int CustomerID)
        {
            string path = new AppSettings().AppsSttingsKeyValues["QlikSenseUrl"];
            return (path + CustomerID);
        }

        /// <summary>
        /// This Method is used to DetailedTimesheetReport
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpGet]
        public string DetailedTimesheetReport(int CustomerID)
        {
            string path = new AppSettings().AppsSttingsKeyValues["DetailedTimesheetReportUrl"];
            return (path + CustomerID);
        }

        /// <summary>
        /// This Method is used to GetTicketDetailsPopUp
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTicketDetailsPopUp")]
        public ActionResult<List<TicketDetails>> GetTicketDetailsPopUp(UserTicketDetail inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null, Convert.ToInt64(inputparam.CustomerID),null);
            try
            {
                if (value)
                {
                    var model = new TimeSheetRepository().GetTicketDetailsPopUp(inputparam.CustomerID, inputparam.FromDate, inputparam.ToDate, inputparam.SubmitterId, inputparam.TsApproverId);
                    return model;
                }
                return Unauthorized();
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
        /// This Method is used to GetTicketDetailsForDownload
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTicketDetailsForDownload")]
        public FileResult GetTicketDetailsForDownload(UserTicketDetail inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null, Convert.ToInt64(inputparam.CustomerID),null);
            try
            {
                if (value)
                {
                    //Download ticketdown = new Download();
                    string fileName = string.Concat("TimesheetDetails", DateTime.Now.ToFileTimeUtc().ToString(CultureInfo.CurrentCulture), ".xlsx");
                    string destinationTemplateFileName = Path.Combine(_hostingEnvironment.ContentRootPath, fileName);
                    var model = new TimeSheetRepository().GetTicketDetailsPopUp(inputparam.CustomerID, inputparam.FromDate, inputparam.ToDate, inputparam.SubmitterId, inputparam.TsApproverId);

                    var fileBytes = new TimeSheetRepository().GetTicketDetailsForDownload(model, destinationTemplateFileName,
                        inputparam.CustomerID, inputparam.IsCognizant, inputparam.IsADMApplicableforCustomer);
                    FileInfo Finfo = new FileInfo(destinationTemplateFileName);
                    if (!Finfo.IsReadOnly)
                    {
                        System.IO.File.Delete(destinationTemplateFileName);
                    }

                    var fileContentResult = new FileContentResult(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet)
                    {
                        //FileDownloadName = destinationTemplateFileName 
                    };
                    return fileContentResult;
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
    }
}
