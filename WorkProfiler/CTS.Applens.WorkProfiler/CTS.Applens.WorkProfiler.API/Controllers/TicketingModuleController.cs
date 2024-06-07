using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.Work_Items;
using CTS.Applens.Framework;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Translations = CTS.Applens.WorkProfiler.DAL.Translation;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketingModuleController : BaseController
    {
        ResourceManager rm = new ResourceManager("AVMDARTLite.ApplicationMessages", Assembly.GetExecutingAssembly());
        Responseclass responseobject = new Responseclass();
        private IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        CacheManager _cacheManager = new CacheManager();
        /// <summary>
        /// TicketingModuleController 
        /// </summary>
        public TicketingModuleController(IConfiguration configuration,
            IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnvironment) :
            base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
            this._configuration = configuration;
        }

        

        // GET: TicketingModule
        /// <summary>
        /// This Method is used to Index
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public ActionResult<UserDetailsBaseModel> Index(string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,EmployeeID, null, null);
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<CustomerModel> lstcustomer = new List<CustomerModel>();
                    string cognizantID = CognizantID;
                    if (string.IsNullOrEmpty(CognizantID))
                    {
                        return new UserDetailsBaseModel
                        {
                            ErrorMessage =
                            new AppSettings().AppsSttingsKeyValues["LoginPage"].ToString(CultureInfo.CurrentCulture) +
                            "q=" + Guid.NewGuid()
                        };
                    }
                    else
                    {
                        cognizantID = CognizantID;
                    }

                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    HiddenFieldsModel objHiddenFieldsModel;
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    TimeZoneInfoByCustomer objTimeZoneInfoByEmployeeID;
                    //To Get Hidden Fields
                    List<CustomerModel> objListCustomerModel;

                    objListCustomerModel = objEffortTrackingRepository.GetCustomer(cognizantID);
                    if (objListCustomerModel.Count == 0)
                    {
                        return new UserDetailsBaseModel
                        {
                            ErrorMessage = new ApplicationConstants().AccountMisConfiguredMsg
                        };
                    }
                    Int64 CustomerID = Convert.ToInt64(objListCustomerModel[0].CustomerId, CultureInfo.CurrentCulture);
                    objHiddenFieldsModel = objTicketingModuleRepository.GetHiddenFieldsForTM(cognizantID, CustomerID);
                    //To get User Menus

                    objListRolePrivilegeModel = objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(cognizantID,
                        CustomerID);
                    objTimeZoneInfoByEmployeeID = objTicketingModuleRepository.GetTimeZoneInformationByCustomer(cognizantID,
                        CustomerID);
                    if (objTimeZoneInfoByEmployeeID.UserTimeZoneName != null &&
                        objTimeZoneInfoByEmployeeID.UserTimeZoneName != "")
                    {
                        objTimeZoneInfoByEmployeeID.CurrentDate = Convert.
                            ToString(GetCurrentTimeofTimeZones(objTimeZoneInfoByEmployeeID.UserTimeZoneName), CultureInfo.CurrentCulture);

                    }
                    else if (objTimeZoneInfoByEmployeeID.ProjectTimeZoneName != null &&
                        objTimeZoneInfoByEmployeeID.ProjectTimeZoneName != "")
                    {
                        objTimeZoneInfoByEmployeeID.CurrentDate = Convert.
                            ToString(GetCurrentTimeofTimeZones(objTimeZoneInfoByEmployeeID.ProjectTimeZoneName), CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        //CCAP FIX , mandatory else
                    }

                    UserDetailsBaseModel userDetails = new UserDetailsBaseModel();
                    objHiddenFieldsModel.EmployeeId = cognizantID;
                    userDetails.EmployeeName = objHiddenFieldsModel.EmployeeName;
                    objHiddenFieldsModel.ApiURL = new AppSettings().AppsSttingsKeyValues["WebApiURL"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.ApiKeyHandler = new AppSettings().AppsSttingsKeyValues["ApiKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIValueHandler = new AppSettings().AppsSttingsKeyValues["APIValueHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthKeyHandler = new AppSettings().AppsSttingsKeyValues["APIAuthKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthValueHandler = new AppSettings().AppsSttingsKeyValues["APIAuthValueHandler"].ToString(CultureInfo.CurrentCulture);
                    userDetails.HiddenFields = objHiddenFieldsModel;
                    userDetails.RolePrevilageMenus = objListRolePrivilegeModel;
                    userDetails.TimeZoneInfoByEmployeeID = objTimeZoneInfoByEmployeeID;
                    if (objHiddenFieldsModel.LstProjectUserID.Count == 0)
                    {
                        return new UserDetailsBaseModel { ErrorMessage = ApplicationConstants.ProjectsMisConfiguredMsg };
                    }
                    else
                    {
                        return userDetails;
                    }
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
        /// This Method is used to Index
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<UserDetailsBaseModel> Index1(string EmployeeID, Int64 CustomerID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,EmployeeID,CustomerID, null);
            try
            {
                if (value)
                {
                    Int64 getcustID = CustomerID;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<CustomerModel> lstcustomer = new List<CustomerModel>();
                    string cognizantID = CognizantID;
                    if (EmployeeID == "" || EmployeeID == null)
                    {
                        cognizantID = CognizantID;
                    }
                    else
                    {
                        EmployeeID = HttpUtility.HtmlEncode(EmployeeID);
                        cognizantID = EmployeeID;
                    }
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    HiddenFieldsModel objHiddenFieldsModel;
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    //To Get Hidden Fields
                    List<CustomerModel> objListCustomerModel;
                    TimeZoneInfoByCustomer objTimeZoneInfoByEmployeeID;

                    objListCustomerModel = objEffortTrackingRepository.GetCustomer(cognizantID);
                    if (getcustID == 0)
                    {
                        if (objListCustomerModel.Count == 0)
                        {
                            return new UserDetailsBaseModel
                            {
                                ErrorMessage = new ApplicationConstants().AccountMisConfiguredMsg
                            };
                        }
                        getcustID = Convert.ToInt64(objListCustomerModel[0].CustomerId, CultureInfo.CurrentCulture);
                    }

                    objHiddenFieldsModel = objTicketingModuleRepository.GetHiddenFieldsForTM(cognizantID, getcustID);
                    //To get User Menus

                    objListRolePrivilegeModel = objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(cognizantID,
                        getcustID);
                    objTimeZoneInfoByEmployeeID = objTicketingModuleRepository.GetTimeZoneInformationByCustomer(cognizantID,
                        getcustID);
                    if (objTimeZoneInfoByEmployeeID.UserTimeZoneName != null && objTimeZoneInfoByEmployeeID.
                        UserTimeZoneName != "")
                    {
                        objTimeZoneInfoByEmployeeID.CurrentDate = Convert.
                            ToString(GetCurrentTimeofTimeZones(objTimeZoneInfoByEmployeeID.UserTimeZoneName), CultureInfo.CurrentCulture);

                    }
                    else if (objTimeZoneInfoByEmployeeID.ProjectTimeZoneName != null &&
                        objTimeZoneInfoByEmployeeID.ProjectTimeZoneName != "")
                    {
                        objTimeZoneInfoByEmployeeID.CurrentDate = Convert.
                            ToString(GetCurrentTimeofTimeZones(objTimeZoneInfoByEmployeeID.ProjectTimeZoneName), CultureInfo.CurrentCulture);

                    }
                    else
                    {
                        //CCAP FIX : Mandatory else
                    }
                    UserDetailsBaseModel userDetails = new UserDetailsBaseModel();
                    objHiddenFieldsModel.EmployeeId = cognizantID;
                    userDetails.EmployeeName = objHiddenFieldsModel.EmployeeName;
                    objHiddenFieldsModel.ApiURL = new AppSettings().AppsSttingsKeyValues["WebApiURL"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.ApiKeyHandler = new AppSettings().AppsSttingsKeyValues["ApiKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIValueHandler = new AppSettings().AppsSttingsKeyValues["APIValueHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthKeyHandler = new AppSettings().AppsSttingsKeyValues["APIAuthKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthValueHandler = new AppSettings().AppsSttingsKeyValues["APIAuthValueHandler"].
                        ToString(CultureInfo.CurrentCulture);
                    userDetails.HiddenFields = objHiddenFieldsModel;
                    userDetails.RolePrevilageMenus = objListRolePrivilegeModel;
                    userDetails.TimeZoneInfoByEmployeeID = objTimeZoneInfoByEmployeeID;
                    if (objHiddenFieldsModel.LstProjectUserID.Count == 0)
                    {
                        return new UserDetailsBaseModel { ErrorMessage = ApplicationConstants.ProjectsMisConfiguredMsg };
                    }
                    else
                    {
                        return userDetails;
                    }
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
        /// This Method is used to AddNonTicket
        /// </summary>
        /// <param name="objNonTicketModel"></param>
        /// <returns>AddNonTicketModel</returns>
        [HttpPost]
        [Route("AddNonTicket")]
        public ActionResult<TimeSheetModel> AddNonTicket(BaseInformationModel objNonTicketModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objNonTicketModel.EmployeeId.ToString(), Convert.ToInt64(objNonTicketModel.CustomerId), Convert.ToInt64(objNonTicketModel.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    TimeSheetModel objresult;
                    objresult = objTicketingModuleRepository.SaveNonTicket(objNonTicketModel);
                    return objresult;
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
        /// This Method is used to GetIconDetail
        /// </summary>
        /// <param name="objNonTicketModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetIconDetail")]
        public ActionResult<EffortDetailsData> GetIconDetail(BaseInformationModel objModel)
        //   string EmployeeID, string CustomerID, string FirsgtDateOfWeek,
        //   string LastDateOfWeek)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,objModel.EmployeeId.ToString(), Convert.ToInt64(objModel.CustomerId), null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    EffortDetailsData lstEfforts = new EffortDetailsData();
                    //AppD validation added
                    if (!string.IsNullOrEmpty(objModel.EmployeeId) && !string.IsNullOrEmpty(objModel.CustomerId) &&
                        !string.IsNullOrEmpty(objModel.FirstDateOfWeek) && !string.IsNullOrEmpty(objModel.LastDateOfWeek))
                    {

                        TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                        lstEfforts = objTicketingModuleRepository.GetEffortDetailsDataByMonth(objModel.EmployeeId,
                            objModel.CustomerId, objModel.FirstDateOfWeek, objModel.LastDateOfWeek);
                    }
                    else
                    {
                        lstEfforts.ClosedTicket = "0";
                        lstEfforts.TicketedEffort = "0";
                        lstEfforts.NonTicketedEffort = "0";
                        lstEfforts.ClosedWorkItem = "0";
                        lstEfforts.WorkItemEffort = "0";
                        lstEfforts.TotalEffort = "0";
                    }
                    return lstEfforts;
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
        /// This Method is used to LoadMenus
        /// </summary>
        /// <param name="objBasicDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult <UserDetailsBaseModel> LoadMenus(BaseInformationModel objBasicDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,objBasicDetails.EmployeeId.ToString(), Convert.ToInt64(objBasicDetails.CustomerId), null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    HiddenFieldsModel objHiddenFieldsModel;
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    UserDetailsBaseModel userDetails = new UserDetailsBaseModel();
                    objHiddenFieldsModel = objTicketingModuleRepository.GetHiddenFieldsForTM(objBasicDetails.EmployeeId,
                                                  Convert.ToInt64(objBasicDetails.CustomerId, CultureInfo.CurrentCulture));
                    objListRolePrivilegeModel = objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(objBasicDetails.EmployeeId,
                                                  Convert.ToInt64(objBasicDetails.CustomerId, CultureInfo.CurrentCulture));
                    objHiddenFieldsModel.ApiURL = new AppSettings().AppsSttingsKeyValues["WebApiURL"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.ApiKeyHandler = new AppSettings().AppsSttingsKeyValues["ApiKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIValueHandler = new AppSettings().AppsSttingsKeyValues["APIValueHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthKeyHandler = new AppSettings().AppsSttingsKeyValues["APIAuthKeyHandler"].ToString(CultureInfo.CurrentCulture);
                    objHiddenFieldsModel.APIAuthValueHandler = new AppSettings().AppsSttingsKeyValues["APIAuthValueHandler"].ToString(CultureInfo.CurrentCulture);
                    userDetails.HiddenFields = objHiddenFieldsModel;
                    userDetails.RolePrevilageMenus = objListRolePrivilegeModel;
                    return userDetails;
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
        /// This method is used to Get Language Details
        /// </summary>
        /// <returns>Language List</returns>
        [HttpGet]
        [Route("GetLanguageDetails")]
        public List<LanguageModel> GetLanguageDetails()
        {
            try
            {
                LanguageFilter languageFilter = new LanguageFilter();
                languageFilter.ModuleName = HttpUtility.HtmlEncode("TM");
                List<LanguageModel> lstLanguage;
                TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                lstLanguage = objTicketingModuleRepository.GetLanguageForDropdown(languageFilter.ModuleName);
                return lstLanguage;
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
        /// This Method is used to get user profile Details
        /// </summary>
        /// <param name="objBasicDetails"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserProfilePicture")]
        public ActionResult<Associate> GetUserProfilePicture()
        {
            Associate associate = new Associate();
            associate = CurrentUser;
            return associate;
        }
        /// <summary>
        /// This Method is used to SaveLanguage Details
        /// </summary>
        /// <param name="objBasicDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveLanguageDetails")]
        public ActionResult<string> SaveLanguageDetails(BaseInformationModel objBasicDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,objBasicDetails.EmployeeId.ToString(), null,null);
            try
            {
                if (value)
                {
                    string objLangModel;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    objLangModel = objTicketingModuleRepository.SaveLanguageForUserID(objBasicDetails);
                    return "Y";
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
        /// This Method is used to GetChartData
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<decimal> GetChartData(BaseInformationModel objBasicDetails)
        //int CustomerId, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,objBasicDetails.EmployeeId.ToString(), Convert.ToInt64(objBasicDetails.CustomerId), null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            if (value)
            {
                TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                decimal totalMandatoryHours = objTicketingModuleRepository.MandatoryHours(Convert.ToInt32(objBasicDetails.CustomerId, CultureInfo.CurrentCulture),
                    objBasicDetails.EmployeeId);
                return totalMandatoryHours;
            }
            return Unauthorized();
        }

        /// <summary>
        /// This Method is used to GetEffort DetailsChartnew
        /// </summary>
        /// <param name="objEffortChartModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEffortDetailsChartnew")]
        public ActionResult<ChartDataProd> GetEffortDetailsChartnew(EffortDetailsChartModel objEffortChartModel)
        {
            bool Check = ValidUser.IsValidAccessUser(this.CurrentUser, objEffortChartModel.EmployeeId.ToString(), Convert.ToInt64(objEffortChartModel.CustomerId), null);

            try
            {
                if (Check)
                {
                    string EmployeeID = objEffortChartModel.EmployeeId;
                    List<DateTime> datesfirst = new List<DateTime>();
                    var Listfirst = objEffortChartModel.FirstWeek.ToString(CultureInfo.CurrentCulture).Split(',');
                    foreach (string datelisteach in Listfirst)
                    {
                        datesfirst.Add(Convert.ToDateTime(datelisteach, CultureInfo.CurrentCulture));
                    }
                    List<DateTime> datelast = new List<DateTime>();
                    var datelistlast = objEffortChartModel.LastWeek.ToString(CultureInfo.CurrentCulture).Split(',');
                    foreach (string datelisteachnew in datelistlast)
                    {
                        datelast.Add(Convert.ToDateTime(datelisteachnew, CultureInfo.CurrentCulture));
                    }

                    ChartDatasourceLatent chartdata = new ChartDatasourceLatent();
                    DateTime monthEndDate;
                    DateTime monthStartDate = new DateTime(objEffortChartModel.Year, objEffortChartModel.Month, 1);
                    int daysInMonth = DateTime.DaysInMonth(objEffortChartModel.Year, objEffortChartModel.Month);
                    int currentWeekNo = 1;
                    List<ChartDetailsModel> chartDataModel = new List<ChartDetailsModel>();
                    List<ChartDataset> data = new List<ChartDataset>();
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    DataTable dtEffortDetailsByDate = new DataTable();
                    List<EffortDetailsByDate> lstEffortDetailsByDate;
                    List<ChartDataset> datesNew = new List<ChartDataset>();
                    List<ChartDataset> lstAlldatesInMonth = new List<ChartDataset>();
                    List<LabelValues> lstAlldatesInMonthConvert;
                    List<string> days = new List<string>();
                    StringBuilder weekList = new StringBuilder();
                    monthEndDate = new DateTime(objEffortChartModel.Year, objEffortChartModel.Month, daysInMonth);

                    if (monthStartDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        weekList.Append("Week").Append(currentWeekNo.ToString(CultureInfo.CurrentCulture));
                        currentWeekNo++;
                    }

                    if (monthStartDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        monthStartDate = datesfirst[0];

                    }
                    if (monthEndDate.DayOfWeek != DayOfWeek.Saturday)
                    {
                        monthEndDate = datelast[6];
                    }
                    StringBuilder sb = new StringBuilder();
                    for (var dt = monthStartDate; dt <= monthEndDate; dt = dt.AddDays(1))
                    {

                        ChartDataset date = new ChartDataset();
                        int weekNo = 0;
                        string day = "";
                        weekNo = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt,
                            CalendarWeekRule.FirstFourDayWeek,
                            DayOfWeek.Monday);
                        day = Convert.ToString(dt.DayOfWeek, CultureInfo.CurrentCulture);
                        date.WeekNo = weekNo;
                        date.WeekDay = day;
                        date.Label = Convert.ToString(dt, CultureInfo.CurrentCulture);
                        date.TicketedEffort = "";
                        date.WeekDate = dt.Day;

                        if (dt.DayOfWeek == DayOfWeek.Sunday && lstAlldatesInMonth.Count != 0)
                        {
                            weekList.Clear();
                            days.Add(" ");
                            weekList.Append("Week").Append(currentWeekNo.ToString(CultureInfo.CurrentCulture));
                            lstAlldatesInMonth.Add(new ChartDataset());
                            currentWeekNo++;
                        }
                        lstAlldatesInMonth.Add(date);
                        sb.Append(Convert.ToString(dt.Day, CultureInfo.CurrentCulture)).Append("/").Append(dt.DayOfWeek.ToString().Substring(0, 1));
                        days.Add(sb.ToString());
                        sb.Clear();
                    }

                    lstAlldatesInMonth.ForEach(s =>
                    {
                        s.NoEffort = "0";
                        s.Holiday = Convert.ToDateTime(s.Label, CultureInfo.CurrentCulture).DayOfWeek == DayOfWeek.Saturday ||
                        Convert.ToDateTime(s.Label, CultureInfo.CurrentCulture).DayOfWeek == DayOfWeek.Sunday ? "0" : "";
                        s.NoEffort = Convert.ToDateTime(s.Label, CultureInfo.CurrentCulture).DayOfWeek == DayOfWeek.Saturday ||
                        Convert.ToDateTime(s.Label, CultureInfo.CurrentCulture).DayOfWeek == DayOfWeek.Sunday ? "-1" : "0";
                    });

                    lstEffortDetailsByDate = objEffortTrackingRepository.GetEffortWeekWiseList(EmployeeID,
                     objEffortChartModel.CustomerId, monthStartDate, monthEndDate);

                    data = lstEffortDetailsByDate.Select(x => new ChartDataset
                    {
                        Label = Convert.ToDateTime(x.TimesheetDate).ToString("MM/dd/yyyy",
                        CultureInfo.CurrentCulture),
                        WeekDate = Convert.ToInt32(x.TimesheetDate.Day,
                        CultureInfo.CurrentCulture),
                        TicketedEffort = x.TicketedEffort,
                        NonTicketedEffort = x.NonTicketedEffort,
                        Holiday = x.Holiday,
                        NoEffort = x.NoEffort,
                        PartialEfforts = x.PartialEfforts,
                        FullEfforts = x.FullEfforts
                    }).ToList();

                    for (int i = 0; i < data.Count; i++)
                    {
                        string dti = Convert.ToDateTime(data[i].Label, CultureInfo.CurrentCulture).ToString("MM/dd/yyyy",
                            CultureInfo.CurrentCulture);
                        string dtTicketedEffort = Convert.ToString(data[i].TicketedEffort, CultureInfo.CurrentCulture);
                        string dtNonTicketedEffort = Convert.ToString(data[i].NonTicketedEffort, CultureInfo.CurrentCulture);
                        int dtWeekNo = Convert.ToInt32(data[i].WeekNo, CultureInfo.CurrentCulture);

                        lstAlldatesInMonth.Where(c => Convert.ToDateTime(c.Label, CultureInfo.CurrentCulture).Day == Convert.ToDateTime(dti, CultureInfo.CurrentCulture).Day &&
                        Convert.ToDateTime(c.Label, CultureInfo.CurrentCulture).Month == Convert.ToDateTime(dti,
                        CultureInfo.CurrentCulture).Month &&
                        Convert.ToDateTime(c.Label, CultureInfo.CurrentCulture).Year == Convert.ToDateTime(dti,
                        CultureInfo.CurrentCulture).Year
                        ).ToList().ForEach(cc =>
                        {
                            cc.TicketedEffort = dtTicketedEffort;
                            cc.NonTicketedEffort = dtNonTicketedEffort;
                            cc.NoEffort = "";

                            cc.PartialEfforts = ((cc.TicketedEffort == "" ? Convert.ToDecimal(0,
                               CultureInfo.CurrentCulture) :
                            Convert.ToDecimal(cc.TicketedEffort, CultureInfo.CurrentCulture)) + (cc.NonTicketedEffort == "" ?
                            Convert.ToDecimal(0, CultureInfo.CurrentCulture) : Convert.ToDecimal(cc.NonTicketedEffort,
                            CultureInfo.CurrentCulture))).ToString(CultureInfo.CurrentCulture);
                        });
                    }
                    decimal mandatoryHours = objTicketingModuleRepository.MandatoryHours
                                          (Convert.ToInt32(objEffortChartModel.CustomerId,
                                          CultureInfo.CurrentCulture), EmployeeID);
                    lstAlldatesInMonthConvert = lstAlldatesInMonth.Select(x => new LabelValues
                    {
                        PartialEfforts = (x.WeekDay != null ? Convert.ToDecimal(x.PartialEfforts,
                        CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture) : "-1"),
                        FullEfforts = x.PartialEfforts != "" ? Convert.ToDecimal(x.PartialEfforts,
                        CultureInfo.CurrentCulture) >= mandatoryHours ?
                        x.TicketedEffort : "" : "",
                        NonTicketedEffort = x.NonTicketedEffort,
                        NoEffort = x.NoEffort != "" ? x.NoEffort : "-1",
                        Holiday = x.Holiday
                    }).ToList();

                    var debtMonthTrend = new Dictionary<string, object>();
                    StringBuilder sbdays = new StringBuilder();
                    string strEffort = string.Empty;
                    for (int i = 0; i < days.Count; i++)
                    {
                        strEffort = string.IsNullOrEmpty(lstAlldatesInMonthConvert[i].NonTicketedEffort) ?
                            "0" : lstAlldatesInMonthConvert[i].NonTicketedEffort;
                        sbdays.Append(days[i]).Append("/").Append(strEffort);
                        days[i] = sbdays.ToString();
                        sbdays.Clear();
                        strEffort = string.Empty;
                    }
                    debtMonthTrend.Add("Days", days);

                    foreach (string debt in Enum.GetNames(typeof(WeeklyEffort)))
                    {
                        List<decimal> fullEffortList = new List<decimal>();
                        List<decimal> noEffortList = new List<decimal>();
                        List<decimal> holidayList = new List<decimal>();
                        List<decimal> partialEffortList = new List<decimal>();
                        List<decimal> NonTickettedEffortList = new List<decimal>();
                        decimal value;
                        foreach (var item in lstAlldatesInMonthConvert)
                        {
                            if (debt == "FullEffort")
                            {
                                value = Math.Round(Convert.ToDecimal(item.FullEfforts == "" ? "0" : item.FullEfforts, CultureInfo.CurrentCulture), 2);
                                fullEffortList.Add(value);
                            }
                            else if (debt == "NoEffort")
                            {
                                bool x = noEffortList.Count == 7 || noEffortList.Count == 15 ||
                                    noEffortList.Count == 23 || noEffortList.Count == 31;
                                if (noEffortList.Count > 1 && x)
                                {
                                    value = -1;
                                }
                                else
                                {
                                    value = Math.Round(Convert.ToDecimal(item.NoEffort == "-1" ? "-1" : item.NoEffort, CultureInfo.CurrentCulture), 2);
                                }
                                noEffortList.Add(value);
                            }
                            else if (debt == "Holiday")
                            {
                                value = Math.Round(Convert.ToDecimal(item.Holiday == "" ? "0" : item.Holiday, CultureInfo.CurrentCulture), 2);
                                holidayList.Add(value);
                            }
                            else if (debt == "NonTickettedEffort")
                            {
                                value = Math.Round(Convert.ToDecimal(item.NonTicketedEffort == "" ? "0" :
                                    item.NonTicketedEffort, CultureInfo.CurrentCulture), 2);
                                NonTickettedEffortList.Add(value);
                            }
                            else if (debt == "PartialEffort")
                            {
                                value = Math.Round(Convert.ToDecimal(item.PartialEfforts == "" ? "0" :
                                    item.PartialEfforts, CultureInfo.CurrentCulture), 2);
                                partialEffortList.Add(value);
                            }
                            else
                            {
                                //mandatory else
                            }
                        }
                        if (debt == "FullEffort")
                        {
                            debtMonthTrend.Add(debt, fullEffortList);
                        }
                        else if (debt == "NoEffort")
                        {
                            debtMonthTrend.Add(debt, noEffortList);
                        }
                        else if (debt == "Holiday")
                        {
                            debtMonthTrend.Add(debt, holidayList);
                        }
                        else if (debt == "NonTickettedEffort")
                        {

                            debtMonthTrend.Add(debt, NonTickettedEffortList);

                        }
                        else if (debt == "PartialEffort")
                        {
                            debtMonthTrend.Add(debt, partialEffortList);
                        }
                        else
                        {
                            //CCAP FIX : Mandatory else
                        }
                    }
                    EffortTrackingRepository objEffortTrackingRepository2 = new EffortTrackingRepository();
                    ChartDataProd chartmasterdata = new ChartDataProd();
                    chartmasterdata.DebtMonthTrend = debtMonthTrend;
                    chartmasterdata.TotalMandatoryHours = mandatoryHours;
                    return chartmasterdata;
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
        /// This Method is used to WeeklyEffort
        /// </summary>
        public enum WeeklyEffort
        {
            PartialEffort = 3, //#926526
            FullEffort = 2, //#1d4465
            NoEffort = 1, //#366936
            Holiday = 0,
            TickettedEffort = 1,
            NonTickettedEffort = 1
        }
        /// <summary>
        /// This Method is used to GetDetailsByProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [Route("GetDetailsByProjectID/{ProjectID}")]
        public ActionResult<ProjectNewDetails> GetDetailsByProjectID(int ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    TicketingModuleRepository objtckresp = new TicketingModuleRepository();
                    ProjectNewDetails lstStatusTypePriority = objtckresp.GetStatusPriorityTicketTypeDetails(ProjectID);
                    return lstStatusTypePriority;
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
        /// Get DetailsAddTicket
        /// </summary>
        /// <param name="objBasicDetails"></param>
        /// <returns></returns>
        [Route("GetDetailsAddTicket")]
        [HttpPost]
        public ActionResult<AddTicketNewDetails> GetDetailsAddTicket(BaseInformationModel objBasicDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(objBasicDetails.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objtckresp = new TicketingModuleRepository();
                    AddTicketNewDetails lstaddnewticketdetails =
                        objtckresp.GetDetailsAddTicket(Convert.ToInt32(objBasicDetails.ProjectId, CultureInfo.CurrentCulture)
                        , objBasicDetails.UserId, objBasicDetails.SupportTypeId);
                    return lstaddnewticketdetails;
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
        /// This Method is used to AddTicketDetails
        /// </summary>
        /// <param name="objAddTicketDetails"></param>
        /// <returns></returns>
        [Route("AddTicketDetails")]
        [HttpPost]
        public ActionResult<TimeSheetModel> AddTicketDetails(AddTicketDetails objAddTicketDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAddTicketDetails.EmployeeId.ToString(), Convert.ToInt64(objAddTicketDetails.CustomerId), Convert.ToInt64(objAddTicketDetails.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    bool allowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + CognizantID, () => "false", CacheDuration.Long), CultureInfo.CurrentCulture);
                    bool isEncryptAllow = objAddTicketDetails.TicketDescription == "" ? true : allowEncrypt;
                    if (isEncryptAllow)
                    {
                        objAddTicketDetails.OpenDate = Convert.ToDateTime(objAddTicketDetails.OpenDateUI,
                            CultureInfo.CurrentCulture);

                        TimeSheetModel objTimeSheetModel;
                        TicketingModuleRepository objtckresp = new TicketingModuleRepository();
                        int ServiceID = 0;
                        if (objAddTicketDetails.TicketId == null)
                        {
                            objAddTicketDetails.TicketId = "";
                        }
                        if (objAddTicketDetails.TicketDescription == null)
                        {
                            objAddTicketDetails.TicketDescription = "";
                        }
                        if (objAddTicketDetails.OpenDate != null)
                        {
                            //If user time zone is present
                            if (!string.IsNullOrEmpty(objAddTicketDetails.UserTimeZone))
                            {
                                objAddTicketDetails.OpenDate = ConvertTimebetweenTimeZones(Convert.
                                    ToDateTime(objAddTicketDetails.OpenDate, CultureInfo.CurrentCulture),
                                    objAddTicketDetails.UserTimeZone,
                                    objAddTicketDetails.ProjectTimeZone);
                            }
                            //if user time zone is NotFiniteNumberException present
                            else if (!string.IsNullOrEmpty(objAddTicketDetails.ProjectTimeZone))
                            {
                                objAddTicketDetails.OpenDate = ConvertTimebetweenTimeZones(Convert.
                                    ToDateTime(objAddTicketDetails.OpenDate, CultureInfo.CurrentCulture),
                                    objAddTicketDetails.ProjectTimeZone,
                                    objAddTicketDetails.ProjectTimeZone);
                            }
                            else
                            {
                                //mandatory else
                            }
                        }
                        var ticketDescriptionwithoutTag = Regex.Replace((objAddTicketDetails.TicketDescription),
                            @"(\s+|<|>)", " ");
                        objAddTicketDetails.TicketDescription = ticketDescriptionwithoutTag;
                        objTimeSheetModel = objtckresp.AddTicketDetails(objAddTicketDetails);
                        if (!string.IsNullOrEmpty(objAddTicketDetails.TicketDescription) &&
                            objAddTicketDetails.SupportTypeId == 1)
                        {
                            Translations translation = new Translations();
                            objAddTicketDetails.TicketId = objTimeSheetModel.LstOverallTicketDetails[0].TicketId;
                            var multilingualConfig = translation.GetProjectMultilinugalTranslateFields(null,
                                objAddTicketDetails.ProjectId.ToString(CultureInfo.CurrentCulture));
                            if (multilingualConfig.IsMultilingualEnable == 1)
                            {
                                bool isDescriptionTranslateField = multilingualConfig.ListTranslateFields.Count > 0 ?
                                                                    multilingualConfig.ListTranslateFields.Any(x =>
                                                                    x.ColumnName == ApplicationConstants.LanguageTranslateDescriptionColumn) : false;
                                if (isDescriptionTranslateField)
                                {
                                    var translateTickets = new List<MultilingualTranslatedValues> {
                            new  MultilingualTranslatedValues
                        {
                            TimeTickerId = Convert.ToInt64(objTimeSheetModel.LstOverallTicketDetails[0].TimeTickerId.ToString(), CultureInfo.CurrentCulture),
                            ProjectId = Convert.ToInt64(objAddTicketDetails.ProjectId),
                            IsTicketDescriptionUpdated = true,
                            TicketDescription = objAddTicketDetails.TicketDescription,
                            Languages = multilingualConfig.ListTranslateLanguage.Count > 0 ?
                            multilingualConfig.ListTranslateLanguage.Select(x => x.LanguageCode).ToList() : null,
                            Key = multilingualConfig.SubscriptionKey,
                            SupportTypeId = objTimeSheetModel.LstOverallTicketDetails[0].SupportTypeId
                        }
                           };

                                    translation.MultilingualTranslatedValue = translateTickets;

                                    TranslateValidation obj = new TranslateValidation();
                                    obj.Text = ApplicationConstants.TranslateValidateString;
                                    obj.LanguageFrom = ApplicationConstants.TranslateDestinationLanguageCode;
                                    obj.LanguageTo = ApplicationConstants.TranslateSpanishLanguageCode;
                                    obj.Key = multilingualConfig.SubscriptionKey;
                                    obj.ProjectId = Convert.ToInt64(objAddTicketDetails.ProjectId, CultureInfo.CurrentCulture);
                                    bool isSubscriptionValid = translation.CallProjectSubscriptionIsActive(obj).Result;
                                    if (isSubscriptionValid)
                                    {
                                        var lstConcatStrings = translation.ProcessForTranslation();
                                        if (translation.MultilingualTranslatedValue.Count > 0)
                                        {
                                            var translatedTicket = translation.MultilingualTranslatedValue.FirstOrDefault();
                                            objAddTicketDetails.TicketDescription = isDescriptionTranslateField && !translatedTicket.HasTicketDescriptionError ?
                                                                                            translatedTicket.TranslatedTicketDescription : objAddTicketDetails.TicketDescription;
                                        }
                                    }
                                }
                            }
                            ServiceID = objtckresp.GetServiceDetails(objAddTicketDetails);
                            objTimeSheetModel.LstOverallTicketDetails[0].ServiceId = ServiceID;
                            if (ServiceID != 0)
                            {
                                objTimeSheetModel.LstOverallTicketDetails[0].LstServiceModel.Where(o =>
                                o.ServiceId == ServiceID).FirstOrDefault().IsSelected = true;
                            }
                            objTimeSheetModel.LstOverallTicketDetails[0].AssignmentGroupId = objAddTicketDetails.AssignmentGroupId;
                        }

                        return objTimeSheetModel;
                    }
                    return null;
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
        /// GetAddTicketPopup
        /// </summary>
        /// <param name="objBasicDetails"></param>
        /// <returns></returns>
        [Route("GetAddTicketPopup")]
        [HttpPost]
        public ActionResult<TicketingModuleBaseModel> GetAddTicketPopup(BaseInformationModel objBasicDetails)
        //int CustomerID, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objBasicDetails.EmployeeId.ToString(), Convert.ToInt64(objBasicDetails.CustomerId),null);

            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objtckresp = new TicketingModuleRepository();
                    List<ApplicationProjectModel> lstAppProj;
                    lstAppProj = objtckresp.GetApplicationDetailForEmployeeid(objBasicDetails.EmployeeId,
                                                 Convert.ToInt64(objBasicDetails.CustomerId, CultureInfo.CurrentCulture));
                    List<ProjectsModel> lstAppProjDET;
                    lstAppProjDET = objtckresp.GetProjectsByCustomer(objBasicDetails.CustomerId, objBasicDetails.EmployeeId);
                    List<ProjectModel> lstprojectnew = new List<ProjectModel>();
                    var lstproject = lstAppProjDET.Select(o => new
                    {
                        o.ProjectId,
                        o.ProjectName,
                        o.UserTimeZoneId,
                        o.UserTimeZoneName,
                        o.ProjectTimeZoneId,
                        o.ProjectTimeZoneName
                    }).Distinct().
                        ToList();
                    for (int i = 0; i < lstproject.Count; i++)
                    {
                        lstprojectnew.Add(new ProjectModel
                        {
                            ProjectId = Convert.ToInt64(lstproject[i].ProjectId),
                            ProjectName = lstproject[i].ProjectName,
                            UserTimeZoneId = lstproject[i].UserTimeZoneId.ToString(CultureInfo.CurrentCulture),
                            UserTimeZoneName = lstproject[i].UserTimeZoneName,
                            ProjectTimeZoneId = lstproject[i].ProjectTimeZoneId.ToString(CultureInfo.CurrentCulture),
                            ProjectTimeZoneName = lstproject[i].ProjectTimeZoneName
                        });
                    }

                    return new TicketingModuleBaseModel { ApplicationProjectModels = lstAppProj, ProjectModels = lstprojectnew };
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
        /// This method is used to Get Work Item popup
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkItempopup")]
        public List<ADMProjectsModel> GetWorkItempopup(List<ADMProjectInput> aDMProjectInputs)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                TicketingModuleRepository objtckresp = new TicketingModuleRepository();
                List<ADMProjectsModel> lstAdmProjects;
                List<ADMProjectsModel> lstAdProjects = new List<ADMProjectsModel>();
                lstAdmProjects = objtckresp.GetADMProjectDetails(aDMProjectInputs);
                return lstAdmProjects;
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
        [HttpPost]
        [Route("GetDropDownValuesForWorkItem")]
        public ActionResult<MasterDataModel> GetDropDownValuesForWorkItem(DropdownInputs dropdownInputs)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(dropdownInputs.projectId));
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    MasterDataModel objMasterDataModel;
                    objMasterDataModel = objTicketingModuleRepository.GetDropDownValuesForWorkItem(dropdownInputs.projectId,
                        Convert.ToDateTime(dropdownInputs.startDate, CultureInfo.CurrentCulture), Convert.ToDateTime(dropdownInputs.endDate,
                        CultureInfo.CurrentCulture), access);
                    return objMasterDataModel;
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
        /// This Method is used to get sprint details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropDownValuesSprint")]
        public ActionResult<MasterDataModel> GetDropDownValuesSprint(DropdownInputs dropdownInputs)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null,null, Convert.ToInt64(dropdownInputs.projectId));
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    MasterDataModel objMasterDataModel;
                    objMasterDataModel = objTicketingModuleRepository.GetDropDownValuesSprint(dropdownInputs.projectId,
                        Convert.ToDateTime(dropdownInputs.startDate, CultureInfo.CurrentCulture),
                        Convert.ToDateTime(dropdownInputs.endDate, CultureInfo.CurrentCulture), access);
                    return objMasterDataModel;
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
        /// This Method is used to EffortGrid
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// This Method is used to GetTicketIdByCustomerID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [Route("GetTicketIdByCustomerID/{CustomerID}")]
        [HttpPost]
        public ActionResult<string> GetTicketIdByCustomerID(Int64 CustomerID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,Convert.ToInt64(CustomerID),null);
            try
            {
                if (value)
                {
                    string ticketID = "";
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    var ticketDetails = objTicketingModuleRepository.GetTicketIdByCustomerID(CustomerID);
                    ticketID = ticketDetails;
                    return ticketID;
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
        /// This Method is used to  GetTicketGrid
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstDateOfWeek"></param>
        /// <param name="LastDateOfWeek"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTicketGrid")]
        public ActionResult<TimeSheetModel> GetTicketGrid(Getticketdetailsinput inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,Convert.ToInt64(inputparam.CustomerID),null);
            try
            {
                if (value)
                {
                    TimeSheetModel objTimeSheetModel;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    objTimeSheetModel = objTicketingModuleRepository.GetWeeklyTicketDetails(inputparam.CustomerID,
                        CurrentUser.ID, inputparam.FirstDateOfWeek, inputparam.LastDateOfWeek,
                        inputparam.Mode, null, "", "0", Convert.ToInt16(inputparam.IsCognizant,
                        CultureInfo.CurrentCulture));

                    return objTimeSheetModel;
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
        /// This method is to filter the activity list by service id
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ActivityDetails> GetSelectedActivities(List<ActivityDetails> Data, int ServiceId)
        {
            if (Data != null && Data.Count() > 0 && ServiceId > 0)
            {
                Data = Data.Where(x => x.ServiceId == ServiceId).ToList();
            }

            return Data;
        }

        /// <summary>
        /// This Method is used to UpdateIsAttributeByTicketID
        /// </summary>
        /// <param name="updateIsAttribute"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateIsAttributeByTicketID")]
        public ActionResult<string> UpdateIsAttributeByTicketID(OverallTicketDetails updateIsAttribute)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(updateIsAttribute.ProjectId));
            try
            {
                if (value)
                {
                    string isAttributeUpdated = "";
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    isAttributeUpdated = objTicketingModuleRepository.UpdateIsAttributeByTicketID(updateIsAttribute);

                    return isAttributeUpdated;
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
        /// This method is used to check the Grace period met condition
        /// </summary>
        /// <param name="ClosedDate"></param>
        /// <param name="CompletedDate"></param>
        /// <param name="GracePeriod"></param>
        /// <param name="ProjectTimeZoneName"></param>
        /// <param name="UserTimeZoneName"></param>
        /// <param name="IsGracePeriodMet"></param>
        /// <param name="DARTStatusID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckIsGracePeriodMet")]
        public GracePeriodDetails CheckIsGracePeriodMet(GracePeriodMetDetails gracePeriodMetDetails)
        {
            try
            {
                GracePeriodDetails objGracePeriodDetails = new GracePeriodDetails();
                if (gracePeriodMetDetails.UserTimeZoneName == String.Empty)
                {
                    gracePeriodMetDetails.UserTimeZoneName = gracePeriodMetDetails.ProjectTimeZoneName;
                }

                if (gracePeriodMetDetails.ClosedDate != null)
                {
                    gracePeriodMetDetails.ClosedDate = ConvertToProjectTime(gracePeriodMetDetails.ClosedDate,
                            gracePeriodMetDetails.UserTimeZoneName, gracePeriodMetDetails.ProjectTimeZoneName);
                }

                if (gracePeriodMetDetails.CompletedDate != null)
                {
                    gracePeriodMetDetails.CompletedDate = ConvertToProjectTime(gracePeriodMetDetails.CompletedDate,
                           gracePeriodMetDetails.UserTimeZoneName, gracePeriodMetDetails.ProjectTimeZoneName);
                }

                gracePeriodMetDetails.IsGracePeriodMet = (gracePeriodMetDetails.DARTStatusID == 8 &&
                    gracePeriodMetDetails.ClosedDate != null ?
                                   DateTimeOffset.Now.DateTime >
                                    Convert.ToDateTime(gracePeriodMetDetails.ClosedDate,
                                    CultureInfo.CurrentCulture).AddDays(gracePeriodMetDetails.GracePeriod)
                                   ? true : false :
                                   (gracePeriodMetDetails.DARTStatusID == 9 && gracePeriodMetDetails.CompletedDate != null ?
                                   (
                                   DateTimeOffset.Now.DateTime >
                                    Convert.ToDateTime(gracePeriodMetDetails.CompletedDate,
                                    CultureInfo.CurrentCulture).AddDays(gracePeriodMetDetails.GracePeriod)
                                   ? true : false
                                   ? true
                                   : false)
                                   : false));
                objGracePeriodDetails.IsGracePeriodMet = gracePeriodMetDetails.IsGracePeriodMet;
                objGracePeriodDetails.ClosedDate = gracePeriodMetDetails.ClosedDate;
                objGracePeriodDetails.CompletedDate = gracePeriodMetDetails.CompletedDate;
                return objGracePeriodDetails;
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
        /// This Method is used to SaveErrorLogTicketData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="TicketData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveErrorLogTicketData")]
        public ActionResult<int> SaveErrorLogTicketData(SaveErrorCorrectionModel objErrCorrection)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objErrCorrection.EmployeeId.ToString(), Convert.ToInt64(objErrCorrection.CustomerId), Convert.ToInt64(objErrCorrection.ProjectId));
            int result;
            var objTicketData = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<List<ErrorLogCorrection>>(objErrCorrection.TicketDetails) as List<ErrorLogCorrection>;

            bool allowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + CognizantID, () => "false", CacheDuration.Long), CultureInfo.CurrentCulture);
            bool isAllowEncrypt = (objTicketData.Where(x => x.TicketDescription != "").Count() > 0 && !allowEncrypt) ? false : true;
            if (value)
            {
                if (isAllowEncrypt)
                {

                    try
                    {
                        TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                        result = objTicketingModuleRepository.SaveErrorLogTicketData(objTicketData, objErrCorrection.ProjectId,
                        objErrCorrection.EmployeeId, Convert.ToString(objErrCorrection.CustomerId, CultureInfo.CurrentCulture), objErrCorrection.SupportTypeId);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                        errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                        errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                        LogError(errorLogDetails, ex);

                        result = (int)ApplicationEnum.ErrorTicketsResult.Failure;
                    }
                }
                else
                {
                    result = (int)ApplicationEnum.ErrorTicketsResult.EncryptionFailure;
                }
                return result;
            }
            return Unauthorized();
        }

        /// <summary>
        /// This Method is used to DeleteAutoAssignedTickets
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="TimeTickerID"></param>
        /// <param name="TicketID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="ActivityID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="SubmitterID"></param>
        /// <param name="TxtHours"></param>
        /// <returns></returns>
        /*DELETE CR-----START*/
        [HttpPost]
        [Route("DeleteAutoAssignedTickets")]
        public ActionResult<string> DeleteAutoAssignedTickets(DeleteTicket objDeleteTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objDeleteTicket.EmployeeId.ToString(), Convert.ToInt64(objDeleteTicket.CustomerId), Convert.ToInt64(objDeleteTicket.ProjectId));
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    bool state = objDeleteTicket != null && !string.IsNullOrEmpty(objDeleteTicket.TicketId) ?
                           objTicketingModuleRepository.DeleteTicket(objDeleteTicket) : false;
                    return state == true ? "True" : "False";
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

                return "False";
            }
        }

        /*---END-----*/
        /// <summary>
        /// This Method is used to GetCountErroredTickets
        /// </summary>
        /// <param name="objAssigneInfromation">AssigneModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCountErroredTickets")]
        public ActionResult<string> GetCountErroredTickets(AssigneModel objAssigneInfromation)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAssigneInfromation.EmployeeId.ToString(),null,null);
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    bool state = objTicketingModuleRepository.GetCountErroredTickets(objAssigneInfromation.EmployeeId,
                      Convert.ToInt32(objAssigneInfromation.UserId, CultureInfo.CurrentCulture));
                    return state == true ? "True" : "False";
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

                return "False";
            }
        }

        /// <summary>
        /// This Method is used to LoadErrorLogGrid
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LoadErrorLogGrid")]
        public ActionResult<DebtEnabledFields> LoadErrorLogGrid(int ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    DebtEnabledFields debtEnabledFields;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    debtEnabledFields = objTicketingModuleRepository.GetDebtEnabledFields(ProjectID);
                    return debtEnabledFields;
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
        /// GetErrorLogTicketData
        /// </summary>
        /// <param name="objErrorLog"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetErrorLogTicketData")]
        public ActionResult<List<ErrorLogCorrection>> GetErrorLogTicketData(BaseInformationModel objErrorLog)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objErrorLog.EmployeeId.ToString(),null, Convert.ToInt64(objErrorLog.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<ErrorLogCorrection> errorlogcorrection;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    errorlogcorrection = objTicketingModuleRepository.GetErrorLogTicketData(Convert.ToInt32(objErrorLog.ProjectId, CultureInfo.CurrentCulture),
                        objErrorLog.EmployeeId, objErrorLog.SupportTypeId);
                    return errorlogcorrection;
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
        /// GetDebtFields
        /// </summary>
        /// <param name="objErrorLogTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDebtFields")]
        public ActionResult<string> GetDebtFields(BaseInformationModel objErrorLogTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(objErrorLogTicket.ProjectId));
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    PopupAttributeModel lstAttributes = new PopupAttributeModel();
                    List<LstCauseCode> lstcausecode = new List<LstCauseCode>();
                    List<LstPriorityModel> lstPriority = new List<LstPriorityModel>();
                    List<LstResolution> lstResolution = new List<LstResolution>();
                    List<LstDebtClassification> lstDebtClassification = new List<LstDebtClassification>();
                    List<LstSeverity> lstSeverity = new List<LstSeverity>();
                    List<LstMetSLA> lstPartiallyautomated = new List<LstMetSLA>();
                    lstAttributes.LstCause = objEffortTrackingRepository.GetLstProjectCauseCode(Convert.ToInt32(objErrorLogTicket.ProjectId, CultureInfo.CurrentCulture),
                        objErrorLogTicket.ApplicationId);
                    lstAttributes.LstResolution = objEffortTrackingRepository.GetLstProjectResolutionCode
                        (objErrorLogTicket.ProjectId, null);
                    lstAttributes.LstDebtClassification = objEffortTrackingRepository.GetLstDebtClassification(objErrorLogTicket.SupportTypeId);
                    lstAttributes.LstSeverity = objEffortTrackingRepository.GetLstSeverity(Convert.ToInt32(objErrorLogTicket.ProjectId, CultureInfo.CurrentCulture));
                    lstAttributes.LstMetSLA = objEffortTrackingRepository.GetIspartialautomatedDetails();
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(lstAttributes);
                    return json;
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
        /// This Method is used to GetErrorLogTickets
        /// </summary>
        /// <param name="objErrorLogTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetErrorLogTickets")]
        public ActionResult<ChooseTicketModel> GetErrorLogTickets(BaseInformationModel objErrorLogTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objErrorLogTicket.EmployeeId.ToString(), Convert.ToInt64(objErrorLogTicket.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<ProjectsModel> lstProjectModel;
                    ChooseTicketModel model = new ChooseTicketModel();
                    Int64 ProjectID = 0;
                    lstProjectModel = objTicketingModuleRepository.GetProjectsByCustomer(objErrorLogTicket.CustomerId,
                                  objErrorLogTicket.EmployeeId);
                    if (lstProjectModel.Count > 0)
                    {
                        ProjectID = lstProjectModel[0].ProjectId;
                    }

                    model.LstProjectsModel = lstProjectModel;
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
        /// This Method is Used to PopupAttribute
        /// </summary>
        /// <param name="objPopupattributeget"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PopupAttribute")]
        public ActionResult<string> PopupAttribute(Popupattributeget objPopupattributeget)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objPopupattributeget.EmployeeId.ToString(), Convert.ToInt64(objPopupattributeget.CustomerId),Convert.ToInt64(objPopupattributeget.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    List<PopupAttributeModel> lstAttributes;
                    List<PopupAttributeModel> lstAttributes1;
                    lstAttributes1 = objEffortTrackingRepository.GetDartStatus(objPopupattributeget.ProjectId,
                        objPopupattributeget.StatusId);
                    lstAttributes = objEffortTrackingRepository.GetPopupAttributeData(objPopupattributeget);
                    //Conversion takes place here
                    //user time zone is null then conversion between project time zone
                    if (string.IsNullOrEmpty(objPopupattributeget.UserTimeZoneName) && !string.
                        IsNullOrEmpty(objPopupattributeget.ProjectTimeZoneName))
                    {
                        objPopupattributeget.UserTimeZoneName = objPopupattributeget.ProjectTimeZoneName;
                    }
                    if (!string.IsNullOrEmpty(objPopupattributeget.ProjectTimeZoneName))
                    {
                        foreach (var item in lstAttributes)
                        {
                            item.TicketOpenDate = setDateText(item.TicketOpenDate, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.TicketCreatedDate = setDateText(item.TicketCreatedDate, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.ActualStartDateTime = setDateText(item.ActualStartDateTime, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.ActualEndtDateTime = setDateText(item.ActualEndtDateTime, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.ClosedDate = setDateText(item.ClosedDate, objPopupattributeget.ProjectTimeZoneName,
                                objPopupattributeget.UserTimeZoneName);
                            item.OpenDateTime = setDateText(item.OpenDateTime, objPopupattributeget.ProjectTimeZoneName,
                                objPopupattributeget.UserTimeZoneName);
                            item.CompletedDateTime = setDateText(item.CompletedDateTime, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.ReopenDateTime = setDateText(item.ReopenDateTime, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.PlannedEndDate = setDateText(item.PlannedEndDate, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                            item.PlannedStartDate = setDateText(item.PlannedStartDate, objPopupattributeget.
                                ProjectTimeZoneName, objPopupattributeget.UserTimeZoneName);
                        }
                    }

                    PopupAttributesDetails attributesDetails = new PopupAttributesDetails();
                    lstAttributes[0].LstPriority = objEffortTrackingRepository.GetLstProjectPriority(objPopupattributeget.
                        ProjectId);
                    lstAttributes[0].LstCause = objEffortTrackingRepository.GetLstProjectCauseCode(objPopupattributeget.
                        ProjectId, Convert.ToInt32(lstAttributes[0].ApplicationId, CultureInfo.CurrentCulture));
                    attributesDetails.MappedFalseCauses =
                    lstAttributes[0].LstCause.Where(x => x.CauseIsMapped == "False").ToList();
                    attributesDetails.MappedTrueCauses =
                    lstAttributes[0].LstCause.Where(x => x.CauseIsMapped == "True").ToList();
                    lstAttributes[0].LstResolution = objEffortTrackingRepository.
                            GetLstProjectResolutionCode(objPopupattributeget.ProjectId.ToString(CultureInfo.CurrentCulture), null);
                    lstAttributes[0].LstDebtClassification = objEffortTrackingRepository.
                    GetLstDebtClassification(objPopupattributeget.SupportTypeId);
                    lstAttributes[0].LstSeverity = objEffortTrackingRepository.
                        GetLstSeverity(objPopupattributeget.ProjectId);
                    lstAttributes[0].LstTicketSource = objEffortTrackingRepository.
                        GetLstTicketSource(objPopupattributeget.ProjectId);
                    lstAttributes[0].LstReleaseType = objEffortTrackingRepository.
                        GetLstReleaseType();
                    lstAttributes[0].LstBusinessImpact = objEffortTrackingRepository.
                        GetBusinessImpact();
                    lstAttributes[0].LstKEDBUpdated = objEffortTrackingRepository.
                        GetLstKEDBUpdated();
                    lstAttributes[0].LstKEDBAvailable = objEffortTrackingRepository.
                        GetkedbAvailable();
                    if (objPopupattributeget.UserTimeZoneName != "")
                    {
                        lstAttributes[0].CurrentDate = Convert.
                            ToString(GetCurrentTimeofTimeZones(objPopupattributeget.UserTimeZoneName), CultureInfo.CurrentCulture);
                    }
                    lstAttributes[0].IsDebtEnabled = objPopupattributeget.IsDebtEnabled;
                    lstAttributes[0].IsMainSpringConfig = objPopupattributeget.IsMainSpring;
                    lstAttributes[0].StatusName = objPopupattributeget.StatusName;
                    lstAttributes[0].ProjectId = objPopupattributeget.ProjectId.ToString(CultureInfo.CurrentCulture);
                    lstAttributes[0].ServiceId = objPopupattributeget.ServiceId ?? "";
                    lstAttributes[0].TicketTypeId = objPopupattributeget.TicketTypeId;
                    lstAttributes[0].GracePeriod = objPopupattributeget.GracePeriod;
                    lstAttributes[0].IsGracePeriodMet =
                            (lstAttributes1[0].DARTStatusId == "8" && lstAttributes[0].ClosedDate != null
                                    && lstAttributes[0].ClosedDate != Convert.ToDateTime("01-01-1900", CultureInfo.CurrentCulture) ?
                                   (lstAttributes[0].ClosedDate)
                                   < DateTimeOffset.Now.DateTime.AddDays(-lstAttributes[0].GracePeriod)
                                   ? true : false :
                                   (lstAttributes1[0].DARTStatusId == "9" && lstAttributes[0].CompletedDateTime != null
                                   && lstAttributes[0].CompletedDateTime != Convert.ToDateTime("01-01-1900", CultureInfo.CurrentCulture) ?
                                   (lstAttributes[0].CompletedDateTime <
                                   DateTimeOffset.Now.DateTime.AddDays(-lstAttributes[0].GracePeriod)
                                   ? true
                                   : false)
                                   : false));
                    lstAttributes[0].IsAHTagged = objPopupattributeget.IsAHTagged;
                    attributesDetails.CurrentDate = Convert.ToString(GetCurrentTimeofTimeZones(objPopupattributeget.UserTimeZoneName), CultureInfo.CurrentCulture);
                    lstAttributes[0].LstTicketType = objEffortTrackingRepository.GetLstTicketType();
                    if (lstAttributes[0].ClosedDate != null)
                    {
                        lstAttributes[0].ClosedDateProject = ConvertToProjectTime(lstAttributes[0].ClosedDate,
                                                        objPopupattributeget.UserTimeZoneName,
                                                        objPopupattributeget.ProjectTimeZoneName);
                    }
                    if (lstAttributes[0].CompletedDateTime != null)
                    {
                        lstAttributes[0].CompletedDateProject = ConvertToProjectTime(lstAttributes[0].CompletedDateTime,
                                                        objPopupattributeget.UserTimeZoneName,
                                                        objPopupattributeget.ProjectTimeZoneName);
                    }
                    attributesDetails.RowID = objPopupattributeget.RowId;
                    attributesDetails.IsAttributeUpdated = objPopupattributeget.IsAttributeUpdated;
                    attributesDetails.ProjectTimeZoneName = objPopupattributeget.ProjectTimeZoneName;
                    attributesDetails.UserTimeZoneName = objPopupattributeget.UserTimeZoneName;
                    lstAttributes[0].DARTStatusId = lstAttributes1[0].DARTStatusId;
                    TicketingModuleRepository objrespTM = new TicketingModuleRepository();

                    List<ApplicationProjectModel> lstAppProj;

                    lstAppProj = objrespTM.GetApplicationDetailForEmployeeid(objPopupattributeget.EmployeeId, objPopupattributeget.CustomerId);

                    lstAppProj = lstAppProj.Where(m => m.ProjectId == objPopupattributeget.ProjectId).ToList();

                    lstAttributes[0].ApplicationList = lstAppProj;

                    var isAppEdit = objrespTM.GetAppEnableFlag(objPopupattributeget.CustomerId);

                    attributesDetails.IsAppEditable = isAppEdit;
                    var IsTicketDescriptionOpted = objEffortTrackingRepository.IsTicketDescriptionOpted(objPopupattributeget.ProjectId);
                    attributesDetails.IsTicketDescriptionOpted = IsTicketDescriptionOpted;
                    attributesDetails.PopupAttributeModels = lstAttributes;
                    return JsonConvert.SerializeObject(attributesDetails);
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
        /// This Method is used to setDateText
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ProjectTimeZone"></param>
        /// <param name="UserTimeZone"></param>
        /// <returns></returns>
        private DateTime setDateText(DateTime dt, string ProjectTimeZone, string UserTimeZone)
        {
            DateTime dTime;
            if (!string.IsNullOrEmpty(dt.ToString(CultureInfo.CurrentCulture)) && Convert.ToDateTime(dt).Year != 1900 && Convert.
                ToDateTime(dt).Year != 0001)
            {
                dTime = ConvertTimebetweenTimeZones(dt, ProjectTimeZone, UserTimeZone);
            }
            else
            {
                dTime = dt;
            }
            return dTime;
        }

        /// <summary>
        /// This Method is used to GetAttributeDetails
        /// </summary>
        /// <param name="objTicketAttributes"></param>
        /// <returns></returns>      
        [HttpPost]
        [Route("GetAttributeDetails")]
        public ActionResult<List<TicketAttributesModel>> GetAttributeDetails(TicketAttributesModel objTicketAttributes)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(objTicketAttributes.ProjectId));
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<TicketAttributesModel> attribute = objTicketingModuleRepository.GetTicketAttributeDetails(objTicketAttributes);
                    return attribute;
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
        /// This Method is used to ChooseUnAssignedTicket
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [Route("ChooseUnAssignedTicket")]
        [HttpPost]
        public ActionResult<ChooseTicketModel> ChooseUnAssignedTicket(BaseInformationModel objUnAssignedTicket)
        //string CustomerId, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,objUnAssignedTicket.EmployeeId.ToString(), Convert.ToInt64(objUnAssignedTicket.CustomerId),null);

            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<ProjectsModel> lstProjectModel;
                    List<StatusDetails> lsTicketStatusDetails = new List<StatusDetails>();
                    ChooseTicketModel model = new ChooseTicketModel();
                    List<ApplicationsModel> lstApplicationDetails = new List<ApplicationsModel>();
                    List<AssignmentGroupModel> lstAssignmentGroupDet = new List<AssignmentGroupModel>();
                    List<TowerDetailsModel> lsttowerdetails = new List<TowerDetailsModel>();
                    Int64 projectID = 0;
                    lstProjectModel = objTicketingModuleRepository.GetProjectsByCustomer
                        (objUnAssignedTicket.CustomerId, objUnAssignedTicket.EmployeeId);
                    if (lstProjectModel.Count == 1)
                    {
                        projectID = lstProjectModel[0].ProjectId;
                        lsTicketStatusDetails = objTicketingModuleRepository.GetTicketStatusByProject(projectID);
                        lstApplicationDetails = objTicketingModuleRepository.GetApplicationsByProject(projectID);
                        lstAssignmentGroupDet = objTicketingModuleRepository.GetAssignmentGroupByProjectID(projectID,
                            objUnAssignedTicket.EmployeeId);
                        if (lstProjectModel[0].SupportTypeId != 1)
                        {
                            lsttowerdetails = objTicketingModuleRepository.GetTowerDetailsByProjectID(projectID,
                                Convert.ToInt64(objUnAssignedTicket.CustomerId, CultureInfo.CurrentCulture));
                        }
                        model.UserTimeZoneName = lstProjectModel[0].UserTimeZoneName;
                        model.ProjectTimeZoneName = lstProjectModel[0].ProjectTimeZoneName;
                        if (model.UserTimeZoneName != null && model.UserTimeZoneName != "")
                        {
                            model.CurrentTime = Convert.ToString(GetCurrentTimeofTimeZones(model.UserTimeZoneName), CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            model.CurrentTime = Convert.ToString(GetCurrentTimeofTimeZones(model.ProjectTimeZoneName), CultureInfo.CurrentCulture);
                        }
                    }
                    model.LstProjectsModel = lstProjectModel;
                    model.LsTicketStatusDetails = lsTicketStatusDetails;
                    model.LstApplicationDetails = lstApplicationDetails;
                    model.LstAssignmentGroupDetails = lstAssignmentGroupDet;
                    model.LstTowerDetails = lsttowerdetails;
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
        /// This Method is used to GetTicketStatusByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTicketStatusByProject/{ProjectID}")]
        public ActionResult<List<StatusDetails>> GetTicketStatusByProject(Int64 ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<StatusDetails> lsTicketStatusDetails;
                    lsTicketStatusDetails = objTicketingModuleRepository.GetTicketStatusByProject(ProjectID);
                    return lsTicketStatusDetails;
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
        /// This Method is used to GetApplicationDetailsByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetApplicationDetailsByProject/{ProjectID}")]
        public ActionResult<List<ApplicationsModel>> GetApplicationDetailsByProject(Int64 ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<ApplicationsModel> lstApplicationDetails;
                    lstApplicationDetails = objTicketingModuleRepository.GetApplicationsByProject(ProjectID);
                    return lstApplicationDetails;
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
        /// This Method is used to GetAssignmentGroupByProject
        /// </summary>
        /// <param name="objAssGrp"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAssignmentGroupByProject")]
        public ActionResult<List<AssignmentGroupModel>> GetAssignmentGroupByProject(BaseInformationModel objAssGrp)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAssGrp.EmployeeId.ToString(),null, Convert.ToInt64(objAssGrp.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<AssignmentGroupModel> lstAssignmentGroupModel;
                    lstAssignmentGroupModel = objTicketingModuleRepository.GetAssignmentGroupByProjectID
                                   (Convert.ToInt64(objAssGrp.ProjectId, CultureInfo.CurrentCulture), objAssGrp.EmployeeId);
                    return lstAssignmentGroupModel;
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
        /// This Method is used to GetTowerDetailsByProjectID
        /// </summary>
        /// <param name="objTowerDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTowerDetailsByProjectID")]
        public ActionResult<List<TowerDetailsModel>> GetTowerDetailsByProjectID(BaseInformationModel objTowerDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, Convert.ToInt64(objTowerDetails.CustomerId), Convert.ToInt64(objTowerDetails.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    List<TowerDetailsModel> lstTowerDetails;
                    lstTowerDetails = objTicketingModuleRepository.GetTowerDetailsByProjectID(
                        Convert.ToInt64(objTowerDetails.ProjectId, CultureInfo.CurrentCulture), Convert.ToInt64(objTowerDetails.CustomerId, CultureInfo.CurrentCulture));
                    return lstTowerDetails;
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
        /// This Method is used to GetSelectedTicketDetails
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstDateOfWeek"></param>
        /// <param name="LastDateOfWeek"></param>
        /// <param name="Mode"></param>
        /// <param name="TicketID_Desc"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [Route("GetSelectedTicketDetails")]
        [HttpPost]
        public ActionResult<TimeSheetModel> GetSelectedTicketDetails(SelectedTicketInput selectedTicketinput)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,CognizantID.ToString(),null,null);
            selectedTicketinput.EmployeeID = HttpUtility.HtmlEncode(selectedTicketinput.EmployeeID);
            selectedTicketinput.CustomerID = HttpUtility.HtmlEncode(selectedTicketinput.CustomerID);
            TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
            TimeSheetModel objTimeSheetModel;
            try
            {
                if (value)
                {
                    objTimeSheetModel = objTicketingModuleRepository.GetWeeklyTicketDetails(selectedTicketinput.CustomerID, selectedTicketinput.EmployeeID,
                    selectedTicketinput.FirstDateOfWeek, selectedTicketinput.LastDateOfWeek, selectedTicketinput.Mode, selectedTicketinput.TicketID_Desc,
                    "", selectedTicketinput.ProjectID, selectedTicketinput.isCognizant);
                    return objTimeSheetModel;
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
        /// This Method is used to GetNonTicketPopup
        /// </summary>
        /// <param name="objNonTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetNonTicketPopup")]
        public ActionResult<NonTicketPopupDetailsModel> GetNonTicketPopup(BaseInformationModel objNonTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objNonTicket.EmployeeId.ToString(), Convert.ToInt64(objNonTicket.CustomerId),null);

            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {

                    TicketingModuleRepository objrespTM = new TicketingModuleRepository();
                    NonTicketPopupDetails objNonTicketOnload;
                    List<ProjectsModel> lstappProject;
                    NonTicketPopupDetailsModel nonTicketPopupDetailsModel = new NonTicketPopupDetailsModel();
                    objNonTicketOnload = objrespTM.GetNonTicketDetailsToPopup(objNonTicket.EmployeeId,
                      Convert.ToInt64(objNonTicket.CustomerId, CultureInfo.CurrentCulture));
                    objNonTicketOnload.LstAppProj = objrespTM.GetApplicationDetailForEmployeeid(objNonTicket.EmployeeId,
                                     Convert.ToInt64(objNonTicket.CustomerId, CultureInfo.CurrentCulture));
                    objNonTicketOnload.LstExcludedWords = objrespTM.GetInvalidSuggestedActivities();
                    lstappProject = objrespTM.GetProjectsByCustomer(objNonTicket.CustomerId, objNonTicket.EmployeeId);
                    DateTime today = DateTime.Today;
                    var sDate = DateTime.Parse(objNonTicket.FirstDateOfWeek, new CultureInfo("en-US", true));
                    var eDate = DateTime.Parse(objNonTicket.LastDateOfWeek, new CultureInfo("en-US", true));
                    DateTime myDate = DateTime.Parse(objNonTicket.FirstDateOfWeek, CultureInfo.CurrentCulture);
                    nonTicketPopupDetailsModel.ProjectModelDetails = lstappProject;
                    nonTicketPopupDetailsModel.StartDate = sDate;
                    nonTicketPopupDetailsModel.EndDate = eDate;
                    nonTicketPopupDetailsModel.CurrentDate = today;
                    nonTicketPopupDetailsModel.NonTicketPopupDetail = objNonTicketOnload;
                    return nonTicketPopupDetailsModel;
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
        /// This Method is used to CauseCodeResolutionCode
        /// </summary>
        /// <param name="CauseCode"></param>
        /// <param name="TicketDescription"></param>
        /// <param name="ResolutionCode"></param>
        /// <param name="ProjectID"></param>
        /// <param name="Application"></param>
        /// <param name="ServiceID"></param>
        /// <param name="TicketTypeID"></param>
        /// <param name="TimeTickerID"></param>
        /// <param name="UserID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="ResolutionMethodName"></param>
        /// <param name="Comments"></param>
        /// <param name="TicketSummary"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CauseCodeResolutionCode")]
        public ActionResult<string> CauseCodeResolutionCode(CauseCodeResolutionCode objCauseCodeResolutionCode)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(objCauseCodeResolutionCode.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    int SupportTypeID = objCauseCodeResolutionCode.SupportTypeId;
                    if (objCauseCodeResolutionCode.TicketDescription == null)
                    {
                        objCauseCodeResolutionCode.TicketDescription = "";
                    }

                    PopupAttributeModel objTicketDetailsPopupModel = new PopupAttributeModel();
                    string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                    AESEncryption aesMod = new AESEncryption();
                    PopupAttributeModel model = new PopupAttributeModel();
                    {

                        GetDebtAvoidResidual objsp_GetDebtAvoidResidual;
                        string multilingualDesc = string.Empty;
                        string multilingualadd_text = string.Empty;
                        string isAutoClassified;
                        string autoClassificationDate;
                        string isDDAutoClassified;
                        string dDAutoClassificationDate;
                        string isAutoClassifiedInfra;
                        string autoClassificationDateInfra;
                        string isDDAutoClassifiedInfra;
                        string ddClassifiedDateInfra;

                        TicketingModuleRepository objticket = new TicketingModuleRepository();
                        var autoClassificationDetails = objticket.GetAutoClassifiedDetailsForDebt(objCauseCodeResolutionCode.
                            ProjectId);
                        var add_textforDebt = objticket.GetaddtextforDebt(objCauseCodeResolutionCode.ProjectId,
                            SupportTypeID);
                        var add_text = string.Empty;
                        if (add_textforDebt != "" && add_textforDebt != "NA")
                        {
                            if (add_textforDebt == "Resolution Remarks")
                            {
                                add_text = objCauseCodeResolutionCode.ResolutionMethodName;
                            }
                            else if (add_textforDebt == "Ticket Summary")
                            {
                                add_text = objCauseCodeResolutionCode.TicketSummary;
                            }
                            else if (add_textforDebt == "Comments")
                            {
                                add_text = objCauseCodeResolutionCode.Comments;
                            }
                            else
                            {
                                //mandatory else
                            }
                        }
                        else
                        {
                            add_text = string.Empty;
                        }
                        isAutoClassified = autoClassificationDetails.Rows[0]["IsAutoClassified"].ToString();
                        autoClassificationDate = autoClassificationDetails.Rows[0]["AutoClassificationDate"].ToString();
                        isDDAutoClassified = autoClassificationDetails.Rows[0]["IsDDAutoClassified"].ToString();
                        dDAutoClassificationDate = autoClassificationDetails.Rows[0]["DDClassifiedDate"].ToString();
                        isAutoClassifiedInfra = autoClassificationDetails.Rows[0]["IsAutoClassifiedInfra"].ToString();
                        autoClassificationDateInfra = autoClassificationDetails.Rows[0]["AutoClassificationDateInfra"].ToString();
                        isDDAutoClassifiedInfra = autoClassificationDetails.Rows[0]["IsDDAutoClassifiedInfra"].ToString();
                        ddClassifiedDateInfra = autoClassificationDetails.Rows[0]["DDClassifiedDateInfra"].ToString();

                        Translations translation = new Translations();

                        var multilingualConfig = translation.GetProjectMultilinugalTranslateFields(null, objCauseCodeResolutionCode.ProjectId);
                        if (multilingualConfig.IsMultilingualEnable == 1)
                        {
                            bool isDescriptionTranslateField = multilingualConfig.ListTranslateFields.Count > 0 ?
                                                                multilingualConfig.ListTranslateFields.Any(x => x.ColumnName == ApplicationConstants.LanguageTranslateDescriptionColumn) : false;
                            bool isResolutionRemarksTranslateField = multilingualConfig.ListTranslateFields.Count > 0 ?
                                                                multilingualConfig.ListTranslateFields.Any(x => x.ColumnName == ApplicationConstants.LanguageTranslateResolutionRemarksColumn) : false;
                            if ((isDescriptionTranslateField && !string.IsNullOrEmpty(objCauseCodeResolutionCode.TicketDescription)) ||
                                (isResolutionRemarksTranslateField && !string.IsNullOrEmpty(objCauseCodeResolutionCode.ResolutionMethodName)))
                            {
                                var translateTicket = new List<MultilingualTranslatedValues>
                        {
                            new MultilingualTranslatedValues {
                            TimeTickerId = Convert.ToInt64(objCauseCodeResolutionCode.TimeTickerId, CultureInfo.CurrentCulture),
                            ProjectId = Convert.ToInt64(objCauseCodeResolutionCode.ProjectId, CultureInfo.CurrentCulture),
                            IsTicketDescriptionUpdated = !string.IsNullOrEmpty(objCauseCodeResolutionCode.TicketDescription) && isDescriptionTranslateField,
                            TicketDescription = !string.IsNullOrEmpty(objCauseCodeResolutionCode.TicketDescription) ?
                                                    objCauseCodeResolutionCode.TicketDescription : string.Empty,
                            IsResolutionRemarksUpdated = add_textforDebt == ApplicationConstants.LanguageTranslateResolutionRemarksColumn &&
                                                            !string.IsNullOrEmpty(objCauseCodeResolutionCode.ResolutionMethodName) &&
                                                            isResolutionRemarksTranslateField,
                            ResolutionRemarks = add_textforDebt == ApplicationConstants.LanguageTranslateResolutionRemarksColumn &&
                                                    !string.IsNullOrEmpty(objCauseCodeResolutionCode.ResolutionMethodName) ?
                                                    objCauseCodeResolutionCode.ResolutionMethodName : string.Empty,
                            Languages = multilingualConfig.ListTranslateLanguage.Count > 0 ? multilingualConfig.ListTranslateLanguage.Select(x => x.LanguageCode).ToList() : null,
                            Key = multilingualConfig.SubscriptionKey,
                            SupportTypeId = SupportTypeID
                            }
                        };
                                translation.MultilingualTranslatedValue = translateTicket;

                                TranslateValidation obj = new TranslateValidation();
                                obj.Text = ApplicationConstants.TranslateValidateString;
                                obj.LanguageFrom = ApplicationConstants.TranslateDestinationLanguageCode;
                                obj.LanguageTo = ApplicationConstants.TranslateSpanishLanguageCode;
                                obj.Key = multilingualConfig.SubscriptionKey;
                                obj.ProjectId = Convert.ToInt64(objCauseCodeResolutionCode.ProjectId, CultureInfo.CurrentCulture);
                                bool isSubscriptionValid = translation.CallProjectSubscriptionIsActive(obj).Result;
                                if (isSubscriptionValid)
                                {
                                    var lstConcatStrings = translation.ProcessForTranslation();
                                    if (translation.MultilingualTranslatedValue.Count > 0)
                                    {
                                        var translatedTicket = translation.MultilingualTranslatedValue.FirstOrDefault();
                                        objCauseCodeResolutionCode.TicketDescription = isDescriptionTranslateField && !translatedTicket.HasTicketDescriptionError ?
                                                                                        translatedTicket.TranslatedTicketDescription : objCauseCodeResolutionCode.TicketDescription;
                                        objCauseCodeResolutionCode.ResolutionMethodName = add_textforDebt == ApplicationConstants.LanguageTranslateResolutionRemarksColumn &&
                                                                                        isResolutionRemarksTranslateField && !translatedTicket.HasResolutionRemarksError ?
                                                                                        translatedTicket.TranslatedResolutionRemarks : objCauseCodeResolutionCode.ResolutionMethodName;
                                    }
                                }
                            }
                        }

                        if ((isAutoClassified == "Y" && SupportTypeID == 1) || (isAutoClassifiedInfra == "Y" && SupportTypeID == 2))
                        {
                            if (objCauseCodeResolutionCode.AutoClassificationType != 0 && objCauseCodeResolutionCode.AutoClassificationType == objCauseCodeResolutionCode.classificationType)
                            {
                                responseobject = objticket.APIForAutoClassification(objCauseCodeResolutionCode.ProjectId,
                                                 objCauseCodeResolutionCode.TowerId, objCauseCodeResolutionCode.SupportTypeId,
                                                 objCauseCodeResolutionCode.ApplicationId, objCauseCodeResolutionCode.CauseCode,
                                                 objCauseCodeResolutionCode.ResolutionCode, objCauseCodeResolutionCode.TicketDescription,
                                                 add_text, objCauseCodeResolutionCode.TimeTickerId, objCauseCodeResolutionCode.UserId,
                                                 objCauseCodeResolutionCode.AutoClassificationType);
                            }
                            else
                            {
                                responseobject = null;
                            }

                            if (responseobject != null)
                            {
                                objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(responseobject.Debt, CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(responseobject.Residual, CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(responseobject.Avoidable, CultureInfo.CurrentCulture);
                                if (objCauseCodeResolutionCode.AutoClassificationType == 2)
                                {
                                    objTicketDetailsPopupModel.CauseCodeId = Convert.ToString(responseobject.CauseCode, CultureInfo.CurrentCulture);
                                    objTicketDetailsPopupModel.ResolutionCodeId = Convert.ToString(responseobject.ResolutionCode, CultureInfo.CurrentCulture);
                                }
                                return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                            }
                            else if (objCauseCodeResolutionCode.classificationType != 2 && isDDAutoClassified == "Y" && dDAutoClassificationDate == "Y")
                            {
                                objsp_GetDebtAvoidResidual = objticket.CauseCodeResolutionCode(objCauseCodeResolutionCode,
                                    isAutoClassified, isDDAutoClassified);
                                objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(objsp_GetDebtAvoidResidual.
                                    DebtClassification, CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(objsp_GetDebtAvoidResidual.
                                    ResidualDebt, CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(objsp_GetDebtAvoidResidual.
                                    AvoidableFlag, CultureInfo.CurrentCulture);

                                return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                            }
                            else
                            {
                                objticket.InsertRuleIDForTickerDetails(objCauseCodeResolutionCode.TimeTickerId, "0",
                                    objCauseCodeResolutionCode.UserId, objCauseCodeResolutionCode.ProjectId, objCauseCodeResolutionCode.SupportTypeId, null, null);
                                objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString("0", CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.ResidualDebt = Convert.ToString("0", CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString("0", CultureInfo.CurrentCulture);
                                if (objCauseCodeResolutionCode.AutoClassificationType == 2)
                                {
                                    objTicketDetailsPopupModel.CauseCodeId = Convert.ToString("0", CultureInfo.CurrentCulture);
                                    objTicketDetailsPopupModel.ResolutionCodeId = Convert.ToString("0", CultureInfo.CurrentCulture);
                                }
                                return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                            }
                        }
                        else if (isAutoClassified != "Y" && (isDDAutoClassified == "Y" && dDAutoClassificationDate == "Y"))
                        {
                            objsp_GetDebtAvoidResidual = objticket.CauseCodeResolutionCode(objCauseCodeResolutionCode,
                                isAutoClassified, isDDAutoClassified);
                            objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(objsp_GetDebtAvoidResidual.
                                DebtClassification, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(objsp_GetDebtAvoidResidual.
                                ResidualDebt, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(objsp_GetDebtAvoidResidual.
                                AvoidableFlag, CultureInfo.CurrentCulture);
                            return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                        }
                    }
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
        /// This Method is used to GetProjectName
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProjectName")]
        public string GetProjectName(string ProjectID)
        {
            try
            {
                string projectName = string.Empty;
                TicketingModuleRepository rP = new TicketingModuleRepository();
                projectName = rP.GetProjectName(ProjectID);
                return projectName;
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
        /// This Method is used to GetResolutionPriority
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CauseID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetResolutionPriority")]
        public ActionResult<List<LstResolution>> GetResolutionPriority(BaseInformationModel baseModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(baseModel.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffort = new EffortTrackingRepository();
                    List<LstResolution> lstResolution;
                    lstResolution = objEffort.GetLstProjectResolutionCode(baseModel.ProjectId, baseModel.CauseCode);
                    return lstResolution;
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
        /// This Method is used to GetESAproject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<GetProjectNameEsaID>> GetESAProject(string ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    string projectName = string.Empty;
                    TicketingModuleRepository rP = new TicketingModuleRepository();
                    List<GetProjectNameEsaID> projectDetails;
                    projectDetails = rP.GetProjectNameESAID(ProjectID);
                    return projectDetails;
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
        /// This Method is used to InsertAttributeDetails
        /// </summary>
        /// <param name="ticketAttributeAdditionParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertAttributeDetails")]
        public ActionResult<string> InsertAttributeDetails(TicketAttributeAdditionParam ticketAttributeAdditionParam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,ticketAttributeAdditionParam.UserID.ToString(),null,null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    bool allowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + CognizantID, () => "false", CacheDuration.Long), CultureInfo.CurrentCulture);
                    bool isEncryptAllow = ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription == "" ? true : allowEncrypt;

                    if (isEncryptAllow)
                    {
                        EffortTrackingRepository effortTracking = new EffortTrackingRepository();
                        InsertAttributeSaveModel insertAttributeSave = new InsertAttributeSaveModel();
                        bool isMultilingualEnabled = effortTracking.CheckIfMultilingualEnabled
                            (Convert.ToString(ticketAttributeAdditionParam.InsertAttributeList[0].ProjectId, CultureInfo.CurrentCulture), ticketAttributeAdditionParam.InsertAttributeList[0].UserId);
                        List<TicketDescriptionSummary> ticketDescriptionSummary;
                        List<string> lstColumns = new List<string>();
                        if (isMultilingualEnabled)
                        {
                            ticketDescriptionSummary = effortTracking.GetTicketValues(ticketAttributeAdditionParam.InsertAttributeList[0].TicketId,
                                Convert.ToString(ticketAttributeAdditionParam.InsertAttributeList[0].ProjectId, CultureInfo.CurrentCulture),
                                ticketAttributeAdditionParam.InsertAttributeList[0].UserId, ticketAttributeAdditionParam.SupportTypeID);
                            if (ticketDescriptionSummary != null && ticketDescriptionSummary.Count > 0)
                            {
                                insertAttributeSave.IsTicketDescriptionUpdated = !string.IsNullOrEmpty
                                    (ticketDescriptionSummary[0].TicketDescription) && !string.IsNullOrEmpty
                                    (ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription) ? ticketDescriptionSummary[0].TicketDescription.Trim().
                                    Equals(ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription.Trim()) ? "0" : "1" :
                                    !string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription) ? "1" : "0";

                                insertAttributeSave.IsTicketSummaryUpdated = !string.IsNullOrEmpty(ticketDescriptionSummary[0].
                                    TicketSummary) && !string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary) ?
                                    ticketDescriptionSummary[0].TicketSummary.Trim().Equals(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary.Trim())
                                    ? "0" : "1" : !string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary) ? "1" : "0";
                            }
                        }



                        string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                        EffortTrackingController objEffortTrackingController = new EffortTrackingController(_configuration, _httpContextAccessor, _hostingEnvironment);
                        if (ticketAttributeAdditionParam.UserTimeZone == "")
                        {
                            ticketAttributeAdditionParam.UserTimeZone = ticketAttributeAdditionParam.ProjectTimeZone;
                        }

                        var ticketDescriptionwithoutTag = Regex.Replace(string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription)
                            ? string.Empty : ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription, @"(\s+|<|>)", " ");
                        var ticketSummarywithoutTag = Regex.Replace(!string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary)
                            ? (string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary) ? "" : ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary) :
                            string.Empty, @"(\s+|<|>)", " ");
                        var ResolutionRemarkswithoutTag = Regex.Replace(string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionMethod)
                            ? string.Empty : ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionMethod, @"(\s+|<|>)", " ");


                        ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription = ticketDescriptionwithoutTag;
                        ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary = ticketSummarywithoutTag;
                        ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionMethod = ResolutionRemarkswithoutTag;
                        AESEncryption aesMod = new AESEncryption();
                        if (encryptionEnabled == "Enabled")
                        {
                            SaveAttributeModel attrEntity = new SaveAttributeModel
                            {
                                TicketID = ticketAttributeAdditionParam.InsertAttributeList[0].TicketId,
                                serviceid = ticketAttributeAdditionParam.InsertAttributeList[0].ServiceId,
                                projectId = ticketAttributeAdditionParam.InsertAttributeList[0].ProjectId,
                                Priority = ticketAttributeAdditionParam.InsertAttributeList[0].Priority,
                                Severity = ticketAttributeAdditionParam.InsertAttributeList[0].Severity,
                                Assignedto = ticketAttributeAdditionParam.InsertAttributeList[0].UserId,
                                //OnOff = ticketAttributeAdditionParam.InsertAttributeList[0].OnOff,
                                TicketSource = Convert.ToInt64(ticketAttributeAdditionParam.InsertAttributeList[0].Source),
                                ReleaseType = ticketAttributeAdditionParam.InsertAttributeList[0].ReleaseType,
                                PlannedEffort = ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEffort,
                                EstimatedWorkSize = ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize == 0 ||
                                ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize == null ? Convert.ToDecimal("0.00", CultureInfo.CurrentCulture) :
                                Convert.ToDecimal(ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize, CultureInfo.CurrentCulture),
                                Ticketcreatedate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].TicketCreateDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                PlannedStartDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].PlannedStartDateAndTime,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                PlannedEndDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEndDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                ActualStartdateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ActualStartDateTime,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                ActualEnddateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ActualEndDateTime,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                ReopenDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ReopenDate, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                //ResolvedDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ResolvedDate,
                                //ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                //RejectedTimeStamp = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].RejectedTimeStamp, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                CloseDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].CloseDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                ReleaseDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ReleaseDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                KEDBAvailableIndicator = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBAvailableIndicator,
                                KEDBUpdatedAdded = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBUpdated,
                                RCAID = ticketAttributeAdditionParam.InsertAttributeList[0].RCAID,

                                MetResponseSLA = ticketAttributeAdditionParam.InsertAttributeList[0].MetResponseSLA,
                                MetResolution = ticketAttributeAdditionParam.InsertAttributeList[0].MetResolution,
                                TicketDescription = string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription) ? string.Empty :
                                Convert.ToBase64String(aesMod.EncryptStringAsBytes(ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription,
                                AseKeyDetail.AesKeyConstVal)),
                                Application = ticketAttributeAdditionParam.InsertAttributeList[0].Application,
                                Comments = ticketAttributeAdditionParam.InsertAttributeList[0].Comments,
                                //SecondaryResources = ticketAttributeAdditionParam.InsertAttributeList[0].SecondaryResources,
                                KEDBPath = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBPath,
                                //EscalatedFlagCustomer = Convert.ToString(ticketAttributeAdditionParam.InsertAttributeList[0].EscalatedFlagCustomer),
                                //StartedDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].StartedDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                //OnHoldDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].OnHoldDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                CompletedDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].CompletedDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                //TicketCreatedBy = ticketAttributeAdditionParam.InsertAttributeList[0].TicketCreatedBy,
                                //AssignedTimeStamp = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].AssignedTimeStamp, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                ResolutionCode = ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionCode,
                                CauseCode = ticketAttributeAdditionParam.InsertAttributeList[0].CauseCode,
                                //UserId = ticketAttributeAdditionParam.UserID,
                                DebtClassificationId = ticketAttributeAdditionParam.InsertAttributeList[0].DebtClassificationId,
                                AvoidableFlag = !string.IsNullOrEmpty(Convert.ToString(ticketAttributeAdditionParam.InsertAttributeList[0].AvoidalFlagId, CultureInfo.CurrentCulture))
                                ? Convert.ToInt32(ticketAttributeAdditionParam.AvoidalFlagId, CultureInfo.CurrentCulture) : 0,
                                ResidualDebtId = ticketAttributeAdditionParam.ResidualDebtId,
                                Resolutionmethod = ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionMethod,
                                ActualEffort = ticketAttributeAdditionParam.InsertAttributeList[0].ActualEffort,
                                TicketOpenDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].TicketOpenDate, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                //TicketStatusId = ticketAttributeAdditionParam.InsertAttributeList[0].TicketStatusId,
                                TicketSummary = !string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary) ?
                                Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.IsNullOrEmpty(ticketAttributeAdditionParam.InsertAttributeList[0].
                                TicketSummary) ? "" : ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary, AseKeyDetail.AesKeyConstVal))
                                : string.Empty,
                                FlexField1 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField1,
                                FlexField2 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField2,
                                FlexField3 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField3,
                                FlexField4 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField4,
                                //Category = ticketAttributeAdditionParam.InsertAttributeList[0].Category,
                                IsPartiallyAutomated = Convert.ToInt16(ticketAttributeAdditionParam.InsertAttributeList[0].IsPartiallyAutomated),
                                AHBusinessImpact = Convert.ToInt16(ticketAttributeAdditionParam.InsertAttributeList[0].AHBusinessImpact),
                                AHImpactComments = ticketAttributeAdditionParam.InsertAttributeList[0].AHImpactComments
                            };
                            List<SaveAttributeModel> attribute = new List<SaveAttributeModel>();
                            attribute.Add(attrEntity);
                            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                            insertAttributeSave.InsertAttributeList = attribute;
                            insertAttributeSave.AvoidableFlagId = Convert.ToInt64(ticketAttributeAdditionParam.AvoidalFlagId == "" ? "0" : ticketAttributeAdditionParam.AvoidalFlagId,
                                CultureInfo.CurrentCulture);
                            insertAttributeSave.IsAttributeUpdated = ticketAttributeAdditionParam.IsAttributeUpdated;
                            insertAttributeSave.TicketStatusId = ticketAttributeAdditionParam.TicketStatusID;
                            insertAttributeSave.ResidualDebtId = ticketAttributeAdditionParam.ResidualDebtId;
                            insertAttributeSave.UserId = ticketAttributeAdditionParam.UserID;
                            insertAttributeSave.SupportTypeId = ticketAttributeAdditionParam.SupportTypeID;
                            var ListOfTicketDetails = objEffortTrackingRepository.InsertAttributeDetails(insertAttributeSave);
                            attribute.Add(attrEntity);
                            return ListOfTicketDetails;
                        }
                        else
                        {
                            SaveAttributeModel attrEntity = new SaveAttributeModel
                            {
                                TicketID = ticketAttributeAdditionParam.InsertAttributeList[0].TicketId,
                                serviceid = ticketAttributeAdditionParam.InsertAttributeList[0].ServiceId,
                                projectId = ticketAttributeAdditionParam.InsertAttributeList[0].ProjectId,
                                Priority = ticketAttributeAdditionParam.InsertAttributeList[0].Priority,
                                Severity = ticketAttributeAdditionParam.InsertAttributeList[0].Severity,
                                Assignedto = ticketAttributeAdditionParam.InsertAttributeList[0].UserId,
                                //OnOff = ticketAttributeAdditionParam.InsertAttributeList[0].OnOff,
                                TicketSource = Convert.ToInt64(ticketAttributeAdditionParam.InsertAttributeList[0].Source),
                                ReleaseType = ticketAttributeAdditionParam.InsertAttributeList[0].ReleaseType,
                                PlannedEffort = ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEffort == 0 ||
                                ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEffort == null ? Convert.ToDecimal("0.00", CultureInfo.CurrentCulture) :
                                Convert.ToDecimal(ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEffort, CultureInfo.CurrentCulture),
                                EstimatedWorkSize = ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize == 0 ||
                                ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize == null ? Convert.ToDecimal("0.00", CultureInfo.CurrentCulture) :
                                Convert.ToDecimal(ticketAttributeAdditionParam.InsertAttributeList[0].EstimatedWorkSize, CultureInfo.CurrentCulture),
                                Ticketcreatedate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].TicketCreateDate, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                PlannedStartDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].PlannedStartDateAndTime,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                PlannedEndDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].PlannedEndDate, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                ActualStartdateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ActualStartDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                ActualEnddateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ActualEndDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                ReopenDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ReopenDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                //ResolvedDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ResolvedDate,
                                //ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                //RejectedTimeStamp = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].RejectedTimeStamp, ticketAttributeAdditionParam.UserTimeZone,
                                // ticketAttributeAdditionParam.ProjectTimeZone),
                                CloseDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].CloseDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                ReleaseDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].ReleaseDate,
                                ticketAttributeAdditionParam.UserTimeZone, ticketAttributeAdditionParam.ProjectTimeZone),
                                KEDBAvailableIndicator = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBAvailableIndicator,
                                KEDBUpdatedAdded = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBUpdated,
                                RCAID = ticketAttributeAdditionParam.InsertAttributeList[0].RCAID,
                                MetResponseSLA = ticketAttributeAdditionParam.InsertAttributeList[0].MetResponseSLA,
                                MetResolution = ticketAttributeAdditionParam.InsertAttributeList[0].MetResolution,
                                TicketDescription = ticketAttributeAdditionParam.InsertAttributeList[0].TicketDescription,
                                Application = ticketAttributeAdditionParam.InsertAttributeList[0].Application,
                                Comments = ticketAttributeAdditionParam.InsertAttributeList[0].Comments,
                                //SecondaryResources = ticketAttributeAdditionParam.InsertAttributeList[0].SecondaryResources,
                                KEDBPath = ticketAttributeAdditionParam.InsertAttributeList[0].KEDBPath,
                                //EscalatedFlagCustomer = Convert.ToString(ticketAttributeAdditionParam.InsertAttributeList[0].EscalatedFlagCustomer),
                                //StartedDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].StartedDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                //OnHoldDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].OnHoldDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                CompletedDateTime = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].CompletedDateTime, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                //TicketCreatedBy = ticketAttributeAdditionParam.InsertAttributeList[0].TicketCreatedBy,
                                //AssignedTimeStamp = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].AssignedTimeStamp, ticketAttributeAdditionParam.UserTimeZone,
                                //ticketAttributeAdditionParam.ProjectTimeZone),
                                ResolutionCode = ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionCode,
                                CauseCode = ticketAttributeAdditionParam.InsertAttributeList[0].CauseCode,
                                //UserId = ticketAttributeAdditionParam.UserID,
                                DebtClassificationId = ticketAttributeAdditionParam.InsertAttributeList[0].DebtClassificationId,
                                AvoidableFlag = Convert.ToInt32(ticketAttributeAdditionParam.AvoidalFlagId == "" ? "0" : ticketAttributeAdditionParam.AvoidalFlagId, CultureInfo.CurrentCulture),
                                ResidualDebtId = ticketAttributeAdditionParam.ResidualDebtId,
                                Resolutionmethod = ticketAttributeAdditionParam.InsertAttributeList[0].ResolutionMethod,
                                ActualEffort = ticketAttributeAdditionParam.InsertAttributeList[0].ActualEffort,
                                TicketOpenDate = ConvertToProjectTime(ticketAttributeAdditionParam.InsertAttributeList[0].TicketOpenDate, ticketAttributeAdditionParam.UserTimeZone,
                                ticketAttributeAdditionParam.ProjectTimeZone),
                                //TicketStatusId = ticketAttributeAdditionParam.InsertAttributeList[0].TicketStatusId,
                                TicketSummary = ticketAttributeAdditionParam.InsertAttributeList[0].TicketSummary,
                                FlexField1 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField1,
                                FlexField2 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField2,
                                FlexField3 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField3,
                                FlexField4 = ticketAttributeAdditionParam.InsertAttributeList[0].FlexField4,
                                IsPartiallyAutomated = Convert.ToInt16(ticketAttributeAdditionParam.InsertAttributeList[0].IsPartiallyAutomated),
                                //Category = ticketAttributeAdditionParam.InsertAttributeList[0].Category
                                AHBusinessImpact = Convert.ToInt16(ticketAttributeAdditionParam.InsertAttributeList[0].AHBusinessImpact),
                                AHImpactComments = ticketAttributeAdditionParam.InsertAttributeList[0].AHImpactComments
                            };
                            List<SaveAttributeModel> attr = new List<SaveAttributeModel>();
                            attr.Add(attrEntity);
                            insertAttributeSave.InsertAttributeList = attr;
                            insertAttributeSave.AvoidableFlagId = Convert.ToInt64(ticketAttributeAdditionParam.AvoidalFlagId == "" ? "0" : ticketAttributeAdditionParam.AvoidalFlagId,
                                CultureInfo.CurrentCulture);
                            insertAttributeSave.IsAttributeUpdated = ticketAttributeAdditionParam.IsAttributeUpdated;
                            insertAttributeSave.TicketStatusId = ticketAttributeAdditionParam.TicketStatusID;
                            insertAttributeSave.ResidualDebtId = ticketAttributeAdditionParam.ResidualDebtId;
                            insertAttributeSave.UserId = ticketAttributeAdditionParam.UserID;
                            insertAttributeSave.SupportTypeId = ticketAttributeAdditionParam.SupportTypeID;
                            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                            var ListOfTicketDetails = objEffortTrackingRepository.InsertAttributeDetails(insertAttributeSave);
                            attr.Add(attrEntity);
                            return ListOfTicketDetails;
                        }
                    }
                    else
                        return "EncryptionFailed";
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
        /// This Method is used to ConvertToProjectTime
        /// </summary>
        /// <param name="dtp"></param>
        /// <param name="UserTimeZone"></param>
        /// <param name="ProjectTimeZone"></param>
        /// <returns></returns>
        private DateTime ConvertToProjectTime(DateTime? dtp, string UserTimeZone, string ProjectTimeZone)
        {

            DateTime date;
            DateTime dt = new DateTime(1900, 1, 1);
            if (dtp.ToString() == "" || dtp.ToString() == "0001")
            {
                date = Convert.ToDateTime(dt.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            }
            else
            {
                date = ConvertTimebetweenTimeZones(Convert.ToDateTime(dtp, CultureInfo.CurrentCulture), UserTimeZone, ProjectTimeZone);
            }
            return date;
        }
        /// <summary>
        /// This Method is used to ConvertTimebetweenTimeZones
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="SrcTimeZone"></param>
        /// <param name="DestTimeZone"></param>
        /// <returns></returns>
        private static DateTime ConvertTimebetweenTimeZones(DateTime dt, string SrcTimeZone, string DestTimeZone)
        {
            DateTime timeUtc = dt;
            TimeZoneInfo srcZone = TimeZoneInfo.FindSystemTimeZoneById(SrcTimeZone);
            TimeZoneInfo destZone = TimeZoneInfo.FindSystemTimeZoneById(DestTimeZone);
            DateTime srcTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, srcZone);
            DateTime destTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, destZone);
            TimeSpan sourceTSpan;
            TimeSpan destTSpan;
            TimeSpan tSpanDiff;
            sourceTSpan = srcZone.GetUtcOffset(timeUtc);
            destTSpan = destZone.GetUtcOffset(timeUtc);
            DateTime convertedDate;
            tSpanDiff = destTSpan - sourceTSpan;
            convertedDate = dt.Add(tSpanDiff);
            return convertedDate;

        }
        /// <summary>
        /// This Method is used to GetCurrentTimeofTimeZones
        /// </summary>
        /// <param name="UserTimeZone"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCurrentTimeofTimeZones")]
        public DateTime GetCurrentTimeofTimeZones(string UserTimeZone)
        {
            DateTime timeUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            if (!string.IsNullOrEmpty(UserTimeZone) && UserTimeZone != "null")
            {
                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(UserTimeZone);
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, zone);
                return currentDateTime;
            }
            else
            {
                return DateTimeOffset.Now.DateTime;
            }

        }
        /// <summary>
        /// This Method is used to SaveData
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Flag"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="IsDaily"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveData")]
        public ActionResult<bool> SaveData(Savedatainput inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(),null,null);
            try
            {
                if (value)
                {


                    bool result = true;
                    if (inputparam.IsDaily == "1")
                    {
                        inputparam.lstticketdata.ForEach(u => u.TimesheetDetails.RemoveAll(x => x.IsFreezed != "false"));
                    }
                    if (inputparam.IsDaily == "1" && inputparam.Flag == 2)
                    {
                        inputparam.lstticketdata.ForEach(u => u.TimesheetDetails.RemoveAll(y => y.IsChanged == "false"));
                    }
                    bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
                    string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
                    new TicketingModuleRepository().SaveData(inputparam.lstticketdata, inputparam.Flag, inputparam.EmployeeID, inputparam.IsDaily, access);
                    return result;
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

                return false;
            }
        }
        /// <summary>
        /// This Method is used to MandatoryHours
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MandatoryHours")]
        public ActionResult<decimal> MandatoryHours(CustomerEmployeeFilter inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(), Convert.ToInt64(inputparam.CustomerID),null);
            if (value)
            {
                TicketingModuleRepository RP = new TicketingModuleRepository();
                decimal result = RP.MandatoryHours(inputparam.CustomerID, inputparam.EmployeeID);
                return result;
            }
            return Unauthorized();
        }
        /// <summary>
        /// This Method is used to GetAssigneNameByProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="AssigneName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAssigneNameByProjectID")]
        public ActionResult<List<AssigneModel>> GetAssigneNameByProjectID(int ProjectID, string AssigneName)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null,Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    AssigneName = HttpUtility.HtmlEncode(AssigneName);
                    TicketingModuleRepository ticketingModuleRepository = new TicketingModuleRepository();
                    var result = ticketingModuleRepository.GetAssigneNameByProjectID(ProjectID, AssigneName);
                    return result;
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
        ///  This Method is used to GetUserApplicaitionDetails
        /// </summary>
        /// <param name="objInformationtModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserApplicaitionDetails")]
        public ActionResult<UserMasterDetails> GetUserApplicaitionDetails(BaseInformationModel objInformationtModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objInformationtModel.EmployeeId.ToString(), Convert.ToInt64(objInformationtModel.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    TicketingModuleRepository ticketingModuleRepository = new TicketingModuleRepository();
                    var result = ticketingModuleRepository.GetUserApplicaitionDetails(objInformationtModel.EmployeeId,
                                           Convert.ToInt32(objInformationtModel.CustomerId, CultureInfo.CurrentCulture));
                    return result;
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
        /// This method is used to Get Default Landing Page Details
        /// </summary>
        /// <returns>Language List</returns>
        [HttpPost]
        [Route("GetDefaultLandingPageDetails")]
        public ActionResult<List<DefaultLandingPage>> GetDefaultLandingPageDetails(BaseInformationModel objInformationtModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objInformationtModel.EmployeeId.ToString(), Convert.ToInt64(objInformationtModel.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<DefaultLandingPage> lstDefaultLandingPage;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    lstDefaultLandingPage = objTicketingModuleRepository.GetDefaultLandingPageDetails(objInformationtModel.EmployeeId, objInformationtModel.CustomerId);
                    return lstDefaultLandingPage;
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
        /// This method is used to Get Project Details for Default Landing
        /// </summary>
        /// <returns>Project List</returns>
        [HttpPost]
        [Route("GetProjectDetailsforDefaultLanding")]
        public ActionResult<List<ProjectDetails>> GetProjectDetailsforDefaultLanding(BaseInformationModel objInfoModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objInfoModel.EmployeeId.ToString(), Convert.ToInt64(objInfoModel.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<ProjectDetails> lstProjectDetails;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    lstProjectDetails = objTicketingModuleRepository.GetProjectDetailsforDefaultLanding(objInfoModel.CustomerId, objInfoModel.EmployeeId);
                    return lstProjectDetails;
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
        /// This method is used to Get Project Lead Details
        /// </summary>
        /// <returns>Lead List</returns>
        [HttpPost]
        [Route("GetProjectLeadDetails")]
        public ActionResult<List<LeadDetails>> GetProjectLeadDetails(BaseInformationModel objInfoModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objInfoModel.EmployeeId.ToString(),null,Convert.ToInt64(objInfoModel.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<LeadDetails> lstLeadDetails;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    lstLeadDetails = objTicketingModuleRepository.GetProjectLeadDetails(objInfoModel.ProjectId, objInfoModel.EmployeeId);
                    return lstLeadDetails;
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
        /// This method is used to Save Default Landing Page Details 
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        [Route("SaveDefaultLandingPageDetails")]
        public ActionResult<bool> SaveDefaultLandingPageDetails(PreferenceModel preference)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, preference.EmployeeID.ToString(), Convert.ToInt64(preference.AccountID),null);
            try
            {
                if (value)
                {
                    bool result;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    result = objTicketingModuleRepository.SaveDefaultLandingPageDetails(preference.EmployeeID, preference.AccountID, preference.PrivilegeID);
                    return result;
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

                return false;
            }
        }
        /// <summary>
        /// This method is used to Get Customerwise Default Page
        /// </summary>
        /// <returns>Defaulter</returns>
        [HttpPost]
        [Route("GetCustomerwiseDefaultPage")]
        public ActionResult<Int64> GetCustomerwiseDefaultPage(BaseInformationModel objInfoModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objInfoModel.EmployeeId.ToString(), Convert.ToInt64(objInfoModel.CustomerId),null);
            if (value)
            {
                Int64 Defaulter;
                TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                Defaulter = objTicketingModuleRepository.GetCustomerwiseDefaultPage(objInfoModel.EmployeeId, objInfoModel.CustomerId);
                return Defaulter;
            }
            return Unauthorized();
        }
        /// <summary>
        /// This method updates Status and Service for workitems
        /// </summary>
        /// <param name="updateWIParams"></param>
        /// <returns>result</returns>
        [HttpPost]
        [Route("UpdateWorkItemServiceandStatus")]
        public ActionResult<bool> UpdateWorkItemServiceandStatus(OverallTicketDetails updateWIParams)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, updateWIParams.EmployeeId.ToString(),null,Convert.ToInt64(updateWIParams.ProjectId));
            try
            {
                if (value)
                {
                    bool result;
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
                    result = objTicketingModuleRepository.UpdateWorkItemServiceandStatus(updateWIParams);
                    return result;
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

                return false;
            }
        }
        /// <summary>
        /// This method Validates WorkItemID
        /// </summary>
        /// <param name="workItemDetails"></param>
        /// <returns>result</returns>
        [HttpPost]
        [Route("CheckWorkItemId")]
        public List<ValidationMessages> CheckWorkItem(List<CheckDuplicate> workItemDetails)
        {
            try
            {
                List<ValidationMessages> lstMessage = new TicketingModuleRepository().CheckWorkItemrepo(workItemDetails);
                return lstMessage;
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
        /// Check whether Sprint Name Exists or Not
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckSprintName")]
        public List<ValidationMessages> CheckSprintName(List<CheckDuplicate> sprintDetails)
        {
            try
            {
                List<ValidationMessages> lstMessage = new TicketingModuleRepository().CheckSprintName(sprintDetails);
                return lstMessage;
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
        /// Saves Sprint Details
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSprintDetails")]
        public bool SaveSprintDetails(List<SavesprintDetails> sprintDetails)
        {
            try
            {
                return new TicketingModuleRepository().SaveSprintDetails(sprintDetails);
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
        /// This method Validates WorkItemID
        /// </summary>
        /// <param name="workitem"></param>
        /// <returns>result</returns>
        [HttpPost]
        [Route("AddWorkItem")]
        public bool AddWorkItem(List<AddWorkItemSave> workitem)
        {
            try
            {
                return new TicketingModuleRepository().Addworkitem(workitem);
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
        /// This Method is used to GetFooterText
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFooterText")]
        public string GetFooterText()
        {
            string FooterText = string.Empty;
            try
            {
                var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                using (var client = httpClientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(new AppSettings().AppsSttingsKeyValues["FooterAPI"], CultureInfo.CurrentCulture));
                    HttpResponseMessage responseProj = client.GetAsync("api/values").Result;
                    responseProj.EnsureSuccessStatusCode();
                    FooterText = responseProj.Content.ReadAsStringAsync().Result;
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

            return FooterText;
        }
        [HttpPost]
        [Route("GetHiddenFieldsforTM")]
        public ActionResult<UserDetailsBaseModel> GetHiddenFieldsforTM(BaseInformationModel baseInformationModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, baseInformationModel.EmployeeId.ToString(), Convert.ToInt64(baseInformationModel.CustomerId),null);
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    objListRolePrivilegeModel = objEffortTrackingRepository.GetRolePrivilageMenusForAppLens(baseInformationModel.EmployeeId,
                        Convert.ToInt64(baseInformationModel.CustomerId, CultureInfo.CurrentCulture));
                    HiddenFieldsModel objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFieldsForTM(baseInformationModel.EmployeeId,
                        Convert.ToInt64(baseInformationModel.CustomerId, CultureInfo.CurrentCulture));
                    objHiddenFieldsModel.IsADMApplicableforCustomer = new AppSettings().AppsSttingsKeyValues["IsADMApplicableforCustomer"];
                    objHiddenFieldsModel.IsExtended = new AppSettings().AppsSttingsKeyValues["IsExtended"];
                    objHiddenFieldsModel.ChooseDaysCount = new AppSettings().AppsSttingsKeyValues["ChooseDaysCount"];
                    UserDetailsBaseModel userDetails = new UserDetailsBaseModel();
                    userDetails.HiddenFields = objHiddenFieldsModel;
                    userDetails.RolePrevilageMenus = objListRolePrivilegeModel;
                    return userDetails;
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

        [HttpPost]
        [Route("GetAlgoKeyAndColumn/{projectId}/{supportTypeId}")]
        public ActionResult<NewAlgoColumnList> GetAlgoKeyAndColumnList(int projectId, int supportTypeId)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(projectId));
            NewAlgoColumnList result;
            try
            {
                if (value)
                {
                    result = new TicketingModuleRepository().GetAlgoKeyAndColumn(projectId, supportTypeId);
                    return result;
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

        [HttpPost]
        [Route("NewAlgoClassification")]
        public ActionResult<string> NewAlgoClassification(CauseCodeResolutionCodeNewAlgo objCauseCodeResolutionCode)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(objCauseCodeResolutionCode.ProjectId));
            try
            {
                if (value)
                {
                    PopupAttributeModel objTicketDetailsPopupModel = new PopupAttributeModel();
                    GetDebtAvoidResidual objsp_GetDebtAvoidResidual;
                    ResponseClassNewAlgo response;
                    string isAutoClassified;
                    string autoClassificationDate;
                    string isDDAutoClassified;
                    string dDAutoClassificationDate;
                    string isAutoClassifiedInfra;
                    string autoClassificationDateInfra;
                    string isDDAutoClassifiedInfra;
                    string ddClassifiedDateInfra;
                    string result = string.Empty;

                    TicketingModuleRepository objticket = new TicketingModuleRepository();
                    var autoClassificationDetails = objticket.GetAutoClassifiedDetailsForDebt(objCauseCodeResolutionCode.
                                            ProjectId);
                    isAutoClassified = autoClassificationDetails.Rows[0]["IsAutoClassified"].ToString();
                    autoClassificationDate = autoClassificationDetails.Rows[0]["AutoClassificationDate"].ToString();
                    isDDAutoClassified = autoClassificationDetails.Rows[0]["IsDDAutoClassified"].ToString();
                    dDAutoClassificationDate = autoClassificationDetails.Rows[0]["DDClassifiedDate"].ToString();
                    isAutoClassifiedInfra = autoClassificationDetails.Rows[0]["IsAutoClassifiedInfra"].ToString();
                    autoClassificationDateInfra = autoClassificationDetails.Rows[0]["AutoClassificationDateInfra"].ToString();
                    isDDAutoClassifiedInfra = autoClassificationDetails.Rows[0]["IsDDAutoClassifiedInfra"].ToString();
                    ddClassifiedDateInfra = autoClassificationDetails.Rows[0]["DDClassifiedDateInfra"].ToString();


                    //string userId = StringOperations.RemoveDomain(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value);
                    string userId = CognizantID;
                    if ((isAutoClassified == "Y" && objCauseCodeResolutionCode.SupportTypeId == 1) || (isAutoClassifiedInfra == "Y" && objCauseCodeResolutionCode.SupportTypeId == 2))
                    {
                        response = objticket.NewAlgoClassification(objCauseCodeResolutionCode.jsonParam, userId, Convert.ToInt32(objCauseCodeResolutionCode.ProjectId,
                            CultureInfo.CurrentCulture), objCauseCodeResolutionCode.SupportTypeId, objCauseCodeResolutionCode.TimeTickerId);
                        result = JsonConvert.SerializeObject(response);

                        if (response != null && (response.ClusterID_Desc != "0" || response.ClusterID_Resolution != "0"))
                        {
                            objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(response.DebtClassificationID, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(response.ResidualDebtID, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(response.AvoidableFlagID, CultureInfo.CurrentCulture);
                            if (objCauseCodeResolutionCode.AutoClassificationType == 2)
                            {
                                objTicketDetailsPopupModel.CauseCodeId = Convert.ToString(responseobject.CauseCode, CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.ResolutionCodeId = Convert.ToString(responseobject.ResolutionCode, CultureInfo.CurrentCulture);
                            }
                            return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                        }
                        else if (objCauseCodeResolutionCode.classificationType != 2 && isDDAutoClassified == "Y" && dDAutoClassificationDate == "Y")
                        {
                            objsp_GetDebtAvoidResidual = objticket.CauseCodeResolutionCode(objCauseCodeResolutionCode,
                                isAutoClassified, isDDAutoClassified);
                            objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(objsp_GetDebtAvoidResidual.
                                DebtClassification, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(objsp_GetDebtAvoidResidual.
                                ResidualDebt, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(objsp_GetDebtAvoidResidual.
                                AvoidableFlag, CultureInfo.CurrentCulture);

                            return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                        }
                        else
                        {
                            objticket.InsertRuleIDForTickerDetails(objCauseCodeResolutionCode.TimeTickerId, null,
                                objCauseCodeResolutionCode.UserId, objCauseCodeResolutionCode.ProjectId, objCauseCodeResolutionCode.SupportTypeId, "0", "0");
                            objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString("0", CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.ResidualDebt = Convert.ToString("0", CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString("0", CultureInfo.CurrentCulture);
                            if (objCauseCodeResolutionCode.AutoClassificationType == 2)
                            {
                                objTicketDetailsPopupModel.CauseCodeId = Convert.ToString("0", CultureInfo.CurrentCulture);
                                objTicketDetailsPopupModel.ResolutionCodeId = Convert.ToString("0", CultureInfo.CurrentCulture);
                            }
                            return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                        }
                    }
                    else if (isAutoClassified != "Y" && objCauseCodeResolutionCode.classificationType != 2 && (isDDAutoClassified == "Y" && dDAutoClassificationDate == "Y"))
                    {
                        objsp_GetDebtAvoidResidual = objticket.CauseCodeResolutionCode(objCauseCodeResolutionCode,
                            isAutoClassified, isDDAutoClassified);
                        objTicketDetailsPopupModel.DebtClassificationId = Convert.ToString(objsp_GetDebtAvoidResidual.
                            DebtClassification, CultureInfo.CurrentCulture);
                        objTicketDetailsPopupModel.ResidualDebt = Convert.ToString(objsp_GetDebtAvoidResidual.
                            ResidualDebt, CultureInfo.CurrentCulture);
                        objTicketDetailsPopupModel.AvoidableFlag = Convert.ToString(objsp_GetDebtAvoidResidual.
                            AvoidableFlag, CultureInfo.CurrentCulture);
                        return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(objTicketDetailsPopupModel);
                    }
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
    }
}
