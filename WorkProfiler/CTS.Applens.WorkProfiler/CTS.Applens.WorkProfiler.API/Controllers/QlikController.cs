using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class QlikController : BaseController
    {
        /// <summary>
        /// QlikController 
        /// </summary>
        public QlikController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor,
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {

        }

        public ActionResult<UserDetailsBaseModel> Index(string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID.ToString(), null, null);
            try
            {
                if (value)
                {
                    TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();

                    List<CustomerModel> lstcustomer = new List<CustomerModel>();
                    string cognizantID;

                    if (string.IsNullOrEmpty(CognizantID))
                    {
                        return new UserDetailsBaseModel { ErrorMessage = new AppSettings().AppsSttingsKeyValues["LoginPage"] + "q=" + Guid.NewGuid() };
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
                        //mandatory else
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
                        userDetails.EmployeeId = EmployeeID;
                        userDetails.CustomerId = CustomerID;
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
        /// Method it holds Get CurrentTimeofTimeZones
        /// </summary>
        /// <param name="UserTimeZone"></param>
        /// <returns></returns>
        public static DateTime GetCurrentTimeofTimeZones(string UserTimeZone)
        {
            DateTime timeUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(UserTimeZone);
            DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, zone);
            return currentDateTime;
        }
    }
}
