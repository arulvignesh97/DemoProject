using CTS.Applens.WorkProfiler.DAL.Mainspring;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Common;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// This class holds MainspringController details
    /// </summary>
    public class MainspringController : BaseController
    {
        MainspringRepository objMainspringRepository = new MainspringRepository();

        /// <summary>
        /// MainspringController 
        /// </summary>
        public MainspringController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor,
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {

        }

        /// <summary>
        /// GET: Mainspring
        /// </summary>
        /// <returns></returns>
        public static string Index()
        {
            return "BaseMeasures";
        }

        /// <summary>
        /// GetBaseMeasureFiltermainspringavailability
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseMeasureFiltermainspringavailability")]
        public ActionResult<MainspringProjectModel> GetBaseMeasureFiltermainspringavailability(int ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    MainspringProjectModel model = new MainspringProjectModel();

                    MainspringProjectModel LstMainspringavailabilityModel;
                    LstMainspringavailabilityModel = objMainspringRepository.
                        GetBaseMeasureFilterData(ProjectID);
                    return LstMainspringavailabilityModel;
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
        /// GetBaseMeasureLoadFactorProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseMeasureLoadFactorProject")]
        public ActionResult<bool> GetBaseMeasureLoadFactorProject(string ProjectID, string MetricName)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    bool isProjectSpecificBaseMeasure;
                    MainspringRepository objMainspringRepository = new MainspringRepository();
                    isProjectSpecificBaseMeasure = objMainspringRepository.
                        GetBaseMeasureLoadFactorProject(ProjectID, MetricName);
                    return isProjectSpecificBaseMeasure;
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
        /// GetBaseMeasureValueLoadFactor
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <param name="ReportPeriodID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseMeasureValueLoadFactor")]
        public ActionResult<string> GetBaseMeasureValueLoadFactor(string ProjectID, string MetricName, int ReportPeriodID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            MainspringRepository objMainspringRepository = new MainspringRepository();
            try
            {
                if (value)
                {
                    var baseMeasureValue = objMainspringRepository.GetBaseMeasureValueLoadFactor(ProjectID,
                        MetricName, ReportPeriodID);
                    return baseMeasureValue;
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
        /// SaveLoadFactor
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <param name="ReportPeriodID"></param>
        /// <param name="LoadFactor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveLoadFactor")]
        public ActionResult<string> SaveLoadFactor(SaveLoadFactor factor)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(factor.ProjectID));
            try
            {
                if (value)
                {
                    string model = string.Empty;
                    MainspringRepository objMainspringRepository = new MainspringRepository();
                    model = objMainspringRepository.SaveLoadFactor(factor.ProjectID, factor.MetricName, factor.ReportPeriodID, factor.LoadFactor);
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
        /// GetTicket Summary Filter ServiceList
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ServiceFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<MainspringServiceListModel>> GetTicketSummaryFilterServiceList(int ProjectID, int ServiceFilter)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    MainspringServiceListModel model = new MainspringServiceListModel();
                    List<MainspringServiceListModel> lstMainspringServiceModel;
                    lstMainspringServiceModel = objMainspringRepository.
                        GetTicketSummaryFilterServiceList(ProjectID, ServiceFilter);
                    return lstMainspringServiceModel;
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
        /// SaveBase MeasureODC
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserId"></param>
        /// <param name="lstBaseMeasure"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBaseMeasureData")]
        public ActionResult<string> SaveBaseMeasureData(SaveBaseMeasure mainspring)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, mainspring.UserID.ToString(), null, Convert.ToInt64(mainspring.ProjectID));
            try
            {
                if (value)
                {
                    bool isSuccess;
                    MainspringRepository objMainspringRepository = new MainspringRepository();
                    isSuccess = objMainspringRepository.SaveBaseMeasure(mainspring.ProjectID,
                        mainspring.UserID, mainspring.lstBaseMeasures);
                    return "Success";
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
        /// Save TicketSummary BaseMeasure ODC
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="lstTicketSummaryBaseODC"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTicketSummaryBaseMeasureODC")]
        public ActionResult<string> SaveTicketSummaryBaseMeasureODC(SaveMainSpringBaseMeasures mainMeasures)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, mainMeasures.UserId.ToString(), null, Convert.ToInt64(mainMeasures.ProjectID));
            try
            {
                if (value)
                {
                    MainspringRepository objMainspringRepository = new MainspringRepository();
                    // isSuccess = 
                    objMainspringRepository.SaveTicketSummaryBaseMeasureODC(mainMeasures.ProjectID, mainMeasures.FrequencyID,
                    mainMeasures.ReportFrequencyID, mainMeasures.lstTicketSummaryBaseODC, mainMeasures.UserId);
                    return "Success";
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
        /// GetTicket Summery BaseMeasure ODCList
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTicketSummeryBaseMeasureODCList")]
        public ActionResult<BaseMeasureProjectwiseODCDetails> GetTicketSummeryBaseMeasureODCList(int ProjectID, int? FrequencyID, string ServiceIDs,
            int? ReportFrequencyID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    int totalCount = 0;
                    int completedCount = 0;
                    decimal progress = 0;
                    int progressPercent = 0;
                    MainspringRepository objMainspringRepository = new MainspringRepository();
                    List<MainspringUserDefinedBaseMeasureModel> lstMainspringTicketSummaryModel;
                    lstMainspringTicketSummaryModel = objMainspringRepository.
                        GetTicketSummeryBaseMeasureODCList(ProjectID, FrequencyID, ServiceIDs, ReportFrequencyID);
                    totalCount = lstMainspringTicketSummaryModel.Where(x => x.ServiceId == 1 ||
                    x.ServiceId == 3 || x.ServiceId == 4 || x.ServiceId == 11 || x.ServiceId == 16).ToList().Count;
                    completedCount = lstMainspringTicketSummaryModel.Where(x => x.TicketSummaryValue != "").ToList().Count;
                    if (completedCount > 0)
                    {
                        progress = Convert.ToDecimal(Convert.ToDecimal(completedCount) / Convert.ToDecimal(totalCount));
                        progressPercent = Convert.ToInt32(Convert.ToDecimal(progress) * 100);
                    }
                    else
                    {
                        //CCAP FIX
                    }
                    if (lstMainspringTicketSummaryModel != null && lstMainspringTicketSummaryModel.Count > 0)
                    {
                        lstMainspringTicketSummaryModel = lstMainspringTicketSummaryModel.Where(x => x.ServiceId == 1
                        || x.ServiceId == 3 || x.ServiceId == 4 || x.ServiceId == 11 || x.ServiceId == 16).ToList()
                        .ToList();
                    }
                    if (lstMainspringTicketSummaryModel != null && lstMainspringTicketSummaryModel.Count > 0)
                    {
                        lstMainspringTicketSummaryModel = lstMainspringTicketSummaryModel.Where(a => ServiceIDs.Split(',').
                        Contains(a.ServiceId.ToString(CultureInfo.CurrentCulture))).Select(a => a).ToList()
                                 .ToList();

                    }

                    return new BaseMeasureProjectwiseODCDetails
                    {
                        MainspringAvailabilityModels = lstMainspringTicketSummaryModel,
                        TotalCount = totalCount,
                        CompletedCount = completedCount,
                        ProgressPercent = progressPercent
                    };
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
        /// Main spring filter
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MPSFilters")]
        public ActionResult<List<MainspringProjectModel>> MPSFilters(CommonModel empModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, empModel.EmployeeID.ToString(), Convert.ToInt64(empModel.CustomerID), null);
            try
            {
                if (value)
                {
                    MainspringProjectModel model = new MainspringProjectModel();
                    List<MainspringProjectModel> lstMainspringProjectsModel;
                    lstMainspringProjectsModel = objMainspringRepository.GetMainspringProjectDetails(
                        int.Parse(string.IsNullOrEmpty(empModel.CustomerID) == true ? "0" : empModel.CustomerID, CultureInfo.CurrentCulture), empModel.EmployeeID);
                    return lstMainspringProjectsModel;
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
        /// Base Measure Report List
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="frequencyID"></param>
        /// <param name="serviceIDs"></param>
        /// <param name="metricsIDs"></param>
        /// <param name="reportFrequencyID"></param>
        /// <param name="baseMeasureType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<BaseMeasureReportModel>> BaseMeasureReportList(int projectID, int? frequencyID, string serviceIDs,
            string metricsIDs,
            int? reportFrequencyID, string baseMeasureType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    serviceIDs = HttpUtility.HtmlEncode(serviceIDs);
                    metricsIDs = HttpUtility.HtmlEncode(metricsIDs);
                    List<BaseMeasureReportModel> model;
                    model = objMainspringRepository.BaseMeasureReportList(projectID, frequencyID, serviceIDs, metricsIDs,
                        reportFrequencyID, "metric report");

                    if (model != null && model.Count > 0 && !string.IsNullOrEmpty(serviceIDs))
                    {
                        model = model.Where(a => serviceIDs.Split(',').Contains(a.ServiceId.ToString(CultureInfo.CurrentCulture))).Select(a => a).
                            ToList();
                    }
                    if (model != null && model.Count > 0 && !string.IsNullOrEmpty(metricsIDs))
                    {
                        model = model.Where(a => metricsIDs.Split(',').Contains(a.MetricId.ToString(CultureInfo.CurrentCulture))).Select(a => a).
                            ToList();
                    }

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
        /// Ticket Summary Base Measure Report List
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="frequencyID"></param>
        /// <param name="serviceIDs"></param>
        /// <param name="reportFrequencyID"></param>
        /// <param name="baseMeasureType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<BaseMeasureTicketReportModel>> TicketSummaryBMReportList(int projectID, int? frequencyID, string serviceIDs,
            int? reportFrequencyID, string baseMeasureType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    baseMeasureType = HttpUtility.HtmlEncode(baseMeasureType);
                    serviceIDs = HttpUtility.HtmlEncode(serviceIDs);
                    List<BaseMeasureTicketReportModel> model;
                    model = objMainspringRepository.TicketSummaryBMReportList(projectID, frequencyID, serviceIDs,
                        reportFrequencyID, baseMeasureType);

                    if (model != null && model.Count > 0 && !string.IsNullOrEmpty(serviceIDs))
                    {
                        model = model.Where(a => serviceIDs.Split(',').Contains(a.ServiceId)).Select(a => a).
                            ToList();
                    }

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
        /// GetBaseMeasure Projectwise SearchUser DefinedList
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="BaseMeasureSystemDefinedOrUserDefined"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetBaseMeasureProjectwiseSearchList")]
        public ActionResult<BaseMeasureProjectwiseDetails> GetBaseMeasureProjectwiseSearchList(int ProjectID, int? FrequencyID,
      string ServiceIDs, string MetricsIDs, int? ReportFrequencyID,
      string BaseMeasureSystemDefinedOrUserDefined)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            string baseMeasureProgressAvailableCount;
            string baseMeasureProgressTotalCount;
            string progressPercentage;
            IList<SearchBaseMeasureParams> LstMainspringavailabilityModel;
            MainspringRepository objMainspringRepository = new MainspringRepository();
            try
            {
                if (value)
                {
                    if (BaseMeasureSystemDefinedOrUserDefined == "user defined")
                    {
                        BaseMeasureSystemDefinedOrUserDefined = HttpUtility.HtmlEncode(BaseMeasureSystemDefinedOrUserDefined);
                        List<MainspringBaseMeasureProgressModel> lstMainspringBaseMeasureProgressModel;
                        lstMainspringBaseMeasureProgressModel = objMainspringRepository.
                            GetBaseMeasureProgress(ProjectID, FrequencyID, ServiceIDs, MetricsIDs, ReportFrequencyID,
                            BaseMeasureSystemDefinedOrUserDefined, access);
                        List<MainspringBaseMeasureProgressModel> lstBaseMeasureSelectedProgress =
                        new List<MainspringBaseMeasureProgressModel>();
                        if (lstMainspringBaseMeasureProgressModel != null && lstMainspringBaseMeasureProgressModel.Count > 0 && BaseMeasureSystemDefinedOrUserDefined == "user defined")
                        {

                            lstBaseMeasureSelectedProgress = lstMainspringBaseMeasureProgressModel.
                                    Where(a => a.BaseMeasureType == "user defined").Select(a => a).ToList();
                        }
                        else if (lstMainspringBaseMeasureProgressModel != null && lstMainspringBaseMeasureProgressModel.Count > 0 && !(BaseMeasureSystemDefinedOrUserDefined == "user defined"))
                        {
                            lstBaseMeasureSelectedProgress = lstMainspringBaseMeasureProgressModel.
                                Where(a => a.BaseMeasureType == "system defined").Select(a => a).ToList();
                        }

                        baseMeasureProgressAvailableCount = Convert.ToString(Convert.ToInt32(lstBaseMeasureSelectedProgress.
                            FirstOrDefault().ValuesAvailableCount), CultureInfo.CurrentCulture);
                        baseMeasureProgressTotalCount = Convert.ToString(Convert.ToInt32(lstBaseMeasureSelectedProgress.
                            FirstOrDefault().ValuesTotalCount), CultureInfo.CurrentCulture);
                        progressPercentage = Convert.ToString(Convert.ToInt32(Math.Round(lstBaseMeasureSelectedProgress.
                            FirstOrDefault().ProgressPercentage, 2, MidpointRounding.AwayFromZero)), CultureInfo.CurrentCulture);

                        if (BaseMeasureSystemDefinedOrUserDefined == "user defined")
                        {
                            LstMainspringavailabilityModel = objMainspringRepository.
                                GetBaseMeasureProjectwiseSearch(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                                ReportFrequencyID, BaseMeasureSystemDefinedOrUserDefined);
                        }
                        else
                        {
                            LstMainspringavailabilityModel = objMainspringRepository.
                          GetBaseMeasureProjectwiseSearch(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                                ReportFrequencyID, BaseMeasureSystemDefinedOrUserDefined);
                        }

                        if (LstMainspringavailabilityModel != null && LstMainspringavailabilityModel.Count > 0)
                        {
                            LstMainspringavailabilityModel = LstMainspringavailabilityModel.
                                Where(a => ServiceIDs.Split(',').Contains(a.ServiceId.ToString(CultureInfo.CurrentCulture)) && MetricsIDs.Split(',').
                                Contains(a.MetricId.ToString(CultureInfo.CurrentCulture))).Select(a => a).ToList().GroupBy(g => new
                                {
                                    g.BaseMeasureId,
                                    g.ServiceId
                                })
                                     .Select(g => g.First())
                                     .ToList();

                        }

                        return new BaseMeasureProjectwiseDetails()
                        {
                            BaseMeasureSystemDefinedOrUserDefined = BaseMeasureSystemDefinedOrUserDefined,
                            MainspringAvailabilityModels = LstMainspringavailabilityModel,
                            BaseMeasureProgressTotalCount = baseMeasureProgressTotalCount,
                            BaseMeasureProgressAvailableCount = baseMeasureProgressAvailableCount,
                            ProgressPercentage = progressPercentage
                        };
                    }
                    else if (BaseMeasureSystemDefinedOrUserDefined == "ODC")
                    {
                        int totalCount = 0;
                        int completedCount = 0;
                        decimal progress = 0;
                        int progressPercent = 0;
                        LstMainspringavailabilityModel = objMainspringRepository.
                            GetBaseMeasureProjectwiseSearch(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                            ReportFrequencyID, BaseMeasureSystemDefinedOrUserDefined);
                        totalCount = LstMainspringavailabilityModel.GroupBy(g => new
                        { g.BaseMeasureId, g.ServiceId, g.PriorityId, g.SupportCategory, g.Technology }).ToList().Count;
                        completedCount = LstMainspringavailabilityModel.Where(x => x.BaseMeasureValue != "").GroupBy(g =>
                        new { g.BaseMeasureId, g.ServiceId, g.PriorityId, g.SupportCategory, g.Technology }).ToList().Count;
                        if (completedCount > 0)
                        {
                            progress = Convert.ToDecimal(Convert.ToDecimal(completedCount) / Convert.ToDecimal(totalCount));
                            progressPercent = Convert.ToInt32(Convert.ToDecimal(progress) * 100);
                        }
                        else
                        {
                            //CCAP Fix
                        }
                        if (LstMainspringavailabilityModel != null && LstMainspringavailabilityModel.Count > 0)
                        {
                            LstMainspringavailabilityModel = LstMainspringavailabilityModel.Where(a => ServiceIDs.Split(',').
                            Contains(a.ServiceId.ToString(CultureInfo.CurrentCulture)) && MetricsIDs.Split(',').Contains(a.MetricId.ToString(CultureInfo.CurrentCulture))).
                            Select(a => a).ToList().GroupBy(g => new
                            {
                                g.BaseMeasureId,
                                g.ServiceId,
                                g.PriorityId,
                                g.SupportCategory,
                                g.Technology
                            })
                                     .Select(g => g.First())
                                     .ToList();

                        }

                        return new BaseMeasureProjectwiseDetails()
                        {
                            BaseMeasureSystemDefinedOrUserDefined = BaseMeasureSystemDefinedOrUserDefined,
                            MainspringAvailabilityModels = LstMainspringavailabilityModel,
                            BaseMeasureProgressTotalCount = totalCount.ToString(CultureInfo.CurrentCulture),
                            BaseMeasureProgressAvailableCount = completedCount.ToString(CultureInfo.CurrentCulture),
                            ProgressPercentage = progressPercent.ToString(CultureInfo.CurrentCulture)
                        };

                    }
                    else
                    {
                        return null;
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
