using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using CTS.Applens.WorkProfiler.Entities.Base;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DebtOverideAndDataDictionaryController : BaseController
    {
        readonly string excelDebtReviewsaveTemplatePath = new ApplicationConstants().DownloadExcelTemp;
        readonly string excelDataDictionarysaveTemplatePath = new ApplicationConstants().DownloadExcelTemp;
        readonly private DebtDataDictionaryRepository objdebtDataDictionaryRepository;
        readonly private IWebHostEnvironment _hostingEnvironment;
        /// <summary>
        /// DebtOverideAndDataDictionaryController
        /// </summary>
        public DebtOverideAndDataDictionaryController(IConfiguration configuration,
            IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnvironment) : base(configuration,
                _httpContextAccessor, _hostingEnvironment)
        {
            objdebtDataDictionaryRepository = new DebtDataDictionaryRepository();
            this._hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// DebtOverRideReviewDict
        /// </summary>
        /// <param name="StartDate">This parameter is to get StartDate </param>
        /// <param name="EndDate">This parameter is to get EndDate</param>
        /// <param name="CustomerID">This parameter is to get CustomerID</param>
        /// <param name="EmployeeID">This parameter is to get EmployeeID</param>
        /// <param name="ProjectID">This parameter is to get ProjectID </param>
        /// <param name="ReviewStatus">This parameter is to get ReviewStatus</param>
        /// <returns></returns>
        /*Data Dictionary Controller Method Starts */
        [HttpPost]
        [Route("DebtOverRideReviewDict")]
        public ActionResult<DebtOverRideReviewModel> DebtOverRideReviewDict(SearchModel searchModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), Convert.ToInt64(searchModel.CustomerID), Convert.ToInt64(searchModel.ProjectID));
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            string cognizantID = "";
            if (searchModel.EmployeeID == "" || searchModel.EmployeeID == null || searchModel.EmployeeID == "undefined")
            {
                cognizantID = CognizantID;
            }
            else
            {
                cognizantID = searchModel.EmployeeID;
            }
            try
            {
                if (value)
                {
                    List<DebtOverrideReview> model = objdebtDataDictionaryRepository.GetDebtOverrideReview(Convert.ToDateTime(searchModel.StartDate,
                        CultureInfo.CurrentCulture), Convert.ToDateTime(searchModel.EndDate, CultureInfo.CurrentCulture), searchModel.CustomerID, cognizantID,
                        searchModel.ProjectID, searchModel.ReviewStatus, access);
                    return new DebtOverRideReviewModel
                    {
                        StartDate = Convert.ToDateTime(
                        searchModel.StartDate, CultureInfo.CurrentCulture),
                        EndDate = Convert.ToDateTime(searchModel.EndDate, CultureInfo.CurrentCulture),
                        EmployeeId = HttpUtility.HtmlEncode(searchModel.EmployeeID),
                        DebtOverRideReviews = model
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
        /// DebtOverRideReview
        /// </summary>
        /// <returns>Methos returns Actions results</returns>
        [HttpGet]
        [Route("DebtOverRideReview")]
        public UserProjectDetailsBaseModel DebtOverRideReview()
        {
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            HiddenFieldsModel objHiddenFieldsModel;
            List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();
            //To Get Hidden Fields
            try
            {
                objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(CognizantID);
                objHiddenFieldsModel.EmployeeId = CognizantID;
                return new UserProjectDetailsBaseModel
                {
                    EmployeeName = objHiddenFieldsModel.EmployeeName,
                    HiddenFields = objHiddenFieldsModel,
                    RolePrevilageMenus = objListRolePrivilegeModel
                };
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
        /// EditDebtReview
        /// </summary>
        /// <param name="debtClassficationID">This parameter holds debtClassficationID value</param>
        /// <param name="ticketID">This parameter holds ticketID value</param>
        /// <param name="debtResolutionID">This parameter holds debtResolutionID value</param>
        /// <param name="causeCodeID">This parameter holds causeCodeID value</param>
        /// <param name="resiDebt">This parameter holds resiDebt value</param>
        /// <param name="avdFlag">This parameter holds avdFlag value</param>
        /// <param name="reasonResiID">This parameter holds reasonResiID value</param>
        /// <param name="exComDate">This parameter holds exComDate value</param>
        /// <returns>Method returns EditDebtReview</returns>
        [HttpPost]
        public ActionResult<string> EditDebtReview(long debtClassficationID, string ticketID, long debtResolutionID,
            long causeCodeID, long resiDebt, int avdFlag, long reasonResiID, string exComDate)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,CognizantID.ToString(),null,null);

            try
            {
                if (value)
                {
                    var model = objdebtDataDictionaryRepository.EditDebtReview(debtClassficationID, ticketID, debtResolutionID
                        , causeCodeID, resiDebt, avdFlag, reasonResiID, exComDate);

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
        /// ApproveTicketsByTicketId
        /// </summary>
        /// <param name="ticketDetails">This parameter holds ticketDetails value</param>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ApproveTicketsByTicketId")]
        public ActionResult<string> ApproveTicketsByTicketId(ApproveDebtModel approveDebtModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, approveDebtModel.EmployeeID.ToString(),null, Convert.ToInt64(approveDebtModel.ProjectID));
            try
            {
                if (value)
                {
                    var objApproveTicket = Newtonsoft.Json.JsonConvert.
                        DeserializeObject<List<DebtOverrideReview>>(approveDebtModel.ticketDetails) as List<DebtOverrideReview>;
                    var result = String.Empty;
                    var model = objdebtDataDictionaryRepository.ApproveTicketsByTicketId(objApproveTicket,
                        approveDebtModel.EmployeeID, approveDebtModel.ProjectID);
                    if (model)
                    {
                        result = "success";
                    }

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
        /// DownloadDebtReviewTemplate
        /// </summary>
        /// <param name="StartDate">This parameter holds StartDate value</param>
        /// <param name="EndDate">This parameter holds EndDate value</param>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <param name="CustomerID">This parameter holds CustomerID value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <param name="IsCognizant">This parameter holds IsCognizant value</param>
        /// <param name="ReviewStatus">This parameter holds ReviewStatus value</param>
        /// <returns></returns>
        public ActionResult<string> DownloadDebtReviewTemplate(DateTime StartDate, DateTime EndDate, string EmployeeID,
            Int64 CustomerID, Int64 ProjectID, int IsCognizant, int ReviewStatus)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID.ToString(), Convert.ToInt64(CustomerID), Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    string debtReviewDumpUploadStatus = objdebtDataDictionaryRepository.
                        ExportToExcelForDebtReview(StartDate, EndDate, EmployeeID, CustomerID, ProjectID, IsCognizant,
                        ReviewStatus);
                    SerializedData = JsonConvert.SerializeObject(debtReviewDumpUploadStatus);
                    return SerializedData;
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
        /// DebtReviewUpload
        /// </summary>
        /// <param name="StartDate">This parameter holds StartDate value</param>
        /// <param name="CloseDate">This parameter holds CloseDate value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <param name="IsCognizant">This parameter holds IsCognizant value</param>
        /// <param name="ReviewStatus">This parameter holds ReviewStatus value</param>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <returns>Method returns DebtReviewCustomer details </returns>
        [HttpPost]
        [Route("DebtReviewUpload")]
        public ActionResult<string> DebtReviewUpload([FromForm] DebtReviewPostModel debtReviewPostModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null,null);
            try
            {
                if (value)
                {
                    DebtReviewUpload modelData = JsonConvert.DeserializeObject<DebtReviewUpload>(debtReviewPostModel.debtReviewUpload);
                    string filename = new ApplicationConstants().EmptyString;
                    string fileSavePath = new ApplicationConstants().EmptyString;
                    var data = "";
                    foreach (IFormFile httpPostedFile in debtReviewPostModel.files)
                    {
                        if (httpPostedFile.Length == 0)
                        {
                            continue;
                        }
                        filename = Path.GetFileName(httpPostedFile.FileName);
                        if (filename != null)
                        {
                            fileSavePath = Path.Combine(excelDebtReviewsaveTemplatePath, filename);
                            using (var stream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(stream);
                            }
                        }
                    }


                    data = objdebtDataDictionaryRepository.ProcessFileUploadForDebtReview(filename, fileSavePath,
                         (modelData.IsCognizant == 1 ? "DebtOverRideAndReview" : "DebtReviewCustomer"),
                         modelData.StartDate, modelData.CloseDate, modelData.ProjectID, modelData.IsCognizant,
                         modelData.ReviewStatus, modelData.EmployeeID);

                    return JsonConvert.SerializeObject(data);
                }
                return Unauthorized();

            }
            catch (IOException ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return new ApplicationConstants().CloseExcel;
            }
        }

        /*Debt Review & Override Controller Method End */

        /// <summary>
        /// DataDictionary
        /// </summary>
        /// <returns></returns>
        /*Data Dictionary Controller Method Starts */
        public UserProjectDetailsBaseModel DataDictionary()
        {
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            HiddenFieldsModel objHiddenFieldsModel;
            List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();
            try
            {
                objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(CognizantID);

                objHiddenFieldsModel.EmployeeId = CognizantID;
                return new UserProjectDetailsBaseModel
                {
                    EmployeeName = objHiddenFieldsModel.EmployeeName,
                    HiddenFields = objHiddenFieldsModel,
                    RolePrevilageMenus = objListRolePrivilegeModel,
                    EmployeeId = CognizantID
                };
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
        /// Get the drop down values for Application based on Portfolio IDs
        /// </summary>
        /// <param name="employeeID">This parameter holds employeeID value</param>
        /// <param name="customerID">This parameter holds customerID value</param>
        /// <param name="projectID">This parameter holds projectID value</param>
        /// <param name="mode">This parameter holds mode value</param>
        /// <param name="lstIDs">This parameter holds lstIDs value</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDropDownValuesDataDictionary")]
        public ActionResult<DataDictionaryData> GetDropDownValuesDataDictionary(DataDictionarySearchFilter searchfilterParams)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, searchfilterParams.employeeID.ToString(), Convert.ToInt64(searchfilterParams.customerID), Convert.ToInt64(searchfilterParams.projectID));
            if (!ModelState.IsValid)
            {
                return null;
            }
            try
            {
                if (value)
                {
                    var model = objdebtDataDictionaryRepository.GetDropDownValuesDataDictionary(searchfilterParams.employeeID,
                        searchfilterParams.customerID,
                        searchfilterParams.projectID,
                        searchfilterParams.mode,
                        searchfilterParams.lstIDs);
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
        /// GetGriddetails
        /// </summary>
        /// <param name="Projectid"></param>
        /// <param name="ApplicationIDs"></param>
        /// <returns>This methos returns Grid details</returns>
        [HttpPost]
        [Route("GetGriddetails")]
        public ActionResult<List<Griddata>> GetGriddetails(GridDataRequest request)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null, Convert.ToInt64(request.Projectid));
            try
            {
                if (value)
                {
                    List<Griddata> model = objdebtDataDictionaryRepository.GetGridData(request.Projectid, request.ApplicationIDs);
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
        /// AddApplicationDetails
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="applicationID"></param>
        /// <param name="reasonForResidualID"></param>
        /// <param name="avoidableFlagID"></param>
        /// <param name="debtClassificationID"></param>
        /// <param name="projectID"></param>
        /// <param name="residualDebtID"></param>
        /// <param name="causeCodeID"></param>
        /// <param name="resolutionCodeID"></param>
        /// <param name="expectedCompletionDate"></param>
        /// <param name="employeeID"></param>
        /// <param name="EffectiveDate"></param>
        /// <returns>Methos returns ApplicationDetails </returns>
        [HttpPost]
        [Route("AddApplicationDetails")]
        public ActionResult<string> AddApplicationDetails(AddApplicationDetails objAddApplicationDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAddApplicationDetails.EmployeeId.ToString(), Convert.ToInt64(objAddApplicationDetails.CustomerId), Convert.ToInt64(objAddApplicationDetails.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    string model = objdebtDataDictionaryRepository.AddApplicationDetails(objAddApplicationDetails);
                    SerializedData = JsonConvert.SerializeObject(model);
                    return SerializedData;
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
        /// AddReasonResidualAndCompDate
        /// </summary>
        /// <param name="empIds"></param>
        /// <param name="applicationIds"></param>
        /// <param name="causeIds"></param>
        /// <param name="resolutionIds"></param>
        /// <param name="debtclassiIds"></param>
        /// <param name="avoidIds"></param>
        /// <param name="resiIds"></param>
        /// <param name="Projectid"></param>
        /// <param name="employeeID"></param>
        /// <param name="reasonResiValueId"></param>
        /// <param name="compDateValue"></param>
        /// <returns>Methos returns ReasonResidualAndCompDate</returns>
        [HttpPost]
        [Route("AddReasonResidualAndCompDate")]
        public ActionResult<string> AddReasonResidualAndCompDate(AddReasonResidualAndCompDate objAddReasonResidualAndCompDate)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAddReasonResidualAndCompDate.EmployeeId.ToString(),null, Convert.ToInt64(objAddReasonResidualAndCompDate.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    string model = objdebtDataDictionaryRepository.AddReasonResidualAndCompDate(objAddReasonResidualAndCompDate);
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
        /// SaveDataDictionaryByID
        /// </summary>
        /// <param name="dataDictionaryDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveDataDictionaryByID")]
        public ActionResult<string> SaveDataDictionaryByID(DataDictionaryDetails detail)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null,null);
            try
            {
                if (value)
                {
                    var objSaveDataDictionary = Newtonsoft.Json.JsonConvert.
                        DeserializeObject<List<ProjectDataDictionary>>(detail.dataDictionaryDetails)
                        as List<ProjectDataDictionary>;
                    var result = String.Empty;
                    var model = objdebtDataDictionaryRepository.SaveDataDictionaryByID(objSaveDataDictionary);
                    if (model)
                    {
                        result = "success";
                    }

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
        /// ProjectDebtDetails
        /// </summary>
        /// <param name="ProjectID">Parameter holds Project ID value</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectDebtDetails")]
        public string ProjectDebtDetails(int ProjectID)
        {
            string dDDate = string.Empty;
            DebtDataDictionaryRepository projectDebtDetailsdate = new DebtDataDictionaryRepository();
            try
            {
                dDDate = projectDebtDetailsdate.ProjectDebtDetailsdate(ProjectID);
                if (!string.IsNullOrEmpty(dDDate))
                {
                    DateTime DDTempDat = Convert.ToDateTime(dDDate, CultureInfo.CurrentCulture);
                    dDDate = DDTempDat.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                }

                return dDDate;
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
        /// ResidualDetail
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="RowID"></param>
        /// <returns>Methos returns Residual details</returns>
        /// 
        [HttpGet]
        [Route("ResidualDetail")]
        public List<ResidualDetail> ResidualDetail(int ProjectID, int ApplicationID, int RowID)
        {
            List<ResidualDetail> residualDetail;

            DebtDataDictionaryRepository projectDebtDetailsdate = new DebtDataDictionaryRepository();
            try
            {
                residualDetail = projectDebtDetailsdate.GetResidualDetail(ProjectID, ApplicationID, RowID);
                if (residualDetail.Count > 0)
                {
                    if (!string.IsNullOrEmpty(residualDetail[0].ExpectedDate))
                    {
                        DateTime DDTempDat = Convert.ToDateTime(residualDetail[0].ExpectedDate, CultureInfo.CurrentCulture);
                        residualDetail[0].ExpectedDate = DDTempDat.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                    }
                }
                return residualDetail;
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
        /// UpdateSignOffDate
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSignOffDate")]
        public ActionResult<string> UpdateSignOffDate(SignOffDetails signOff)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, signOff.EmployeeID.ToString(),null, Convert.ToInt64(signOff.ProjectID));
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            string EmployeeID = HttpUtility.HtmlEncode(signOff.EmployeeID);
            DateTime EffectiveDate = DateTime.Parse(signOff.EffectiveDate, CultureInfo.CurrentCulture);
            string result = string.Empty;
            DebtDataDictionaryRepository signoffdate = new DebtDataDictionaryRepository();
            try
            {
                if (value)
                {
                    result = signoffdate.UpdateSignOffDate(signOff.ProjectID, signOff.ApplicationID, EffectiveDate, EmployeeID, access);
                    result = (result == "True") ? "success" : result;
                    return HttpUtility.HtmlEncode(result);
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
        /// Delete patterns of Data Dictioanry
        /// </summary>
        /// <param name="dataDictionaryDetails"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        [Route("DeleteDataDictionaryByID")]
        public ActionResult<string> DeleteDataDictionaryByID(DataDictionaryDetails detail)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, detail.EmployeeID.ToString(),null,null);
            if (value)
            {
                string model = string.Empty;
                try
                {
                    var objSaveDataDictionary = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<List<ProjectDataDictionaryDelete>>(detail.dataDictionaryDetails)
                        as List<ProjectDataDictionaryDelete>;
                    model = objdebtDataDictionaryRepository.DeleteDataDictionaryByID(objSaveDataDictionary,
                        detail.EmployeeID).ToString(CultureInfo.CurrentCulture);

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

                return model;
            }
            return Unauthorized();
        }
       
        /// <summary>
        /// Upload function for Data Dictionary 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>Method returns Json</returns>
        [HttpPost]
        [Route("DataDictionaryUploadByProject")]
        public ActionResult<string> DataDictionaryUploadByProject(Int64 ProjectID, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    string filename = new ApplicationConstants().EmptyString;
                    string fileSavePath = new ApplicationConstants().EmptyString;
                    Int64 DDUploadID = 0;
                    var data = "";
                    foreach (IFormFile httpPostedFile in Request.Form.Files)
                    {
                        if (httpPostedFile.Length == 0)
                        {
                            continue;
                        }
                        filename = Path.GetFileName(httpPostedFile.FileName);
                        string dirctoryName = System.IO.Path.GetDirectoryName(filename);
                        string fName = System.IO.Path.GetFileNameWithoutExtension(filename);
                        string validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsm");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        if (validatePath != null)
                        {
                            fileSavePath = Path.Combine(excelDataDictionarysaveTemplatePath, validatePath);
                            //if (!string.IsNullOrEmpty(fileSavePath) && !System.IO.File.Exists(fileSavePath))
                            {
                                using (var stream = new FileStream(fileSavePath, FileMode.Create))
                                {
                                    httpPostedFile.CopyTo(stream);
                                }
                            }
                        }
                    }

                    DDUploadID = objdebtDataDictionaryRepository.
                        InsertDataDictionalExcelDetailsByProject(ProjectID, EmployeeID, filename);
                    data = objdebtDataDictionaryRepository.
                        ProcessFileUploadDataDictionary(filename, fileSavePath, ProjectID, EmployeeID, DDUploadID);
                    return data;
                }
                return Unauthorized();
            }
            catch (IOException ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return new ApplicationConstants().CloseExcel;
            }
        }

        /* Common Method Starts*/
        /// <summary>
        /// Get the Drop down values for Portfolio and Applications
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="customerID"></param>
        /// <returns>Methos returns Json</returns>
        [HttpGet]
        [Route("GetDropDownValuesProjectPortfolio")]
        public string GetDropDownValuesProjectPortfolio(string employeeID, int customerID)
        {
            DataDictionaryProjects ddList;
            try
            {
                byte[] bytes = Convert.FromBase64String(employeeID);
                string EmpID = Encoding.UTF8.GetString(bytes);
                ddList = objdebtDataDictionaryRepository.GetDropDownValuesProjectPortfolio(EmpID, customerID);
                SerializedData = JsonConvert.SerializeObject(ddList);
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

            return SerializedData;
        }

        /// <summary>
        /// GetTicketRoles
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <returns>Method returns Ticket roles</returns>
        [HttpPost]
        [Route("GetTicketRoles")]
        public ActionResult<List<TicketRole>> GetTicketRoles(UserBaseDetails userDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, userDetails.employeeID.ToString(), Convert.ToInt64(userDetails.customerID), Convert.ToInt64(userDetails.projectID));
            List<TicketRole> model = new List<TicketRole>();
            try
            {
                if (value)
                {
                    if (!string.IsNullOrEmpty(userDetails.employeeID))
                    {
                        model = objdebtDataDictionaryRepository.GetTicketRoles(userDetails.employeeID, userDetails.customerID, userDetails.projectID);
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
        /// GetDebtclassification
        /// </summary>
        /// <returns>Method returns Debtclassification</returns>
        [HttpGet]
        [Route("GetDebtclassification")]
        public List<DebtClassificationModelDebt> GetDebtclassification()
        {
            List<DebtClassificationModelDebt> model = new List<DebtClassificationModelDebt>();
            try
            {
                model = objdebtDataDictionaryRepository.GetDebtClassificationmodel();
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

            return model;
        }

        /// <summary>
        /// GetCausecode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>Methos returns Causecode</returns>
        [HttpGet]
        [Route("GetCausecode")]
        public List<CauseModelDebt> GetCausecode(int ProjectID)
        {
            List<CauseModelDebt> model = new List<CauseModelDebt>();
            try
            {
                model = objdebtDataDictionaryRepository.GetCauseCode(ProjectID);
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

            return model;
        }

        /// <summary>
        /// GetAvoidableFlag
        /// </summary>
        /// <returns>Method returns AvoidableFlag</returns>
        [HttpGet]
        [Route("GetAvoidableFlag")]
        public List<AvoidableModelFlag> GetAvoidableFlag()
        {
            List<AvoidableModelFlag> model = new List<AvoidableModelFlag>();
            try
            {
                model = objdebtDataDictionaryRepository.GetAvoidableFlag();
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

            return model;
        }

        /// <summary>
        /// GetNatureOfTicket
        /// </summary>
        /// <returns>Method returns NatureOfTicket</returns>
        public List<NatureOfTicket> GetNatureOfTicket()
        {
            List<NatureOfTicket> model = new List<NatureOfTicket>();
            try
            {
                model = objdebtDataDictionaryRepository.GetNatureOfTicket();
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

            return model;
        }

        /// <summary>
        /// GetResidualDebt
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetResidualDebt")]
        public List<ResidualModelDebt> GetResidualDebt()
        {
            List<ResidualModelDebt> model = new List<ResidualModelDebt>();
            try
            {
                model = objdebtDataDictionaryRepository.GetResidualDebt();
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

            return model;
        }

        /// <summary>
        /// GetResolutioncode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetResolutioncode")]
        public List<ResolutionModelDebt> GetResolutioncode(int ProjectID)
        {
            List<ResolutionModelDebt> model = new List<ResolutionModelDebt>();
            try
            {
                model = objdebtDataDictionaryRepository.GetResolutionCode(ProjectID);
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

            return model;
        }

        /// <summary>
        /// GetApplicationdetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetApplicationdetails/{ProjectID:int}")]
        public List<ApplicationModel> GetApplicationdetails(int ProjectID)
        {
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            List<ApplicationModel> model = new List<ApplicationModel>();
            try
            {
                model = objdebtDataDictionaryRepository.GetApplicationDetails(ProjectID, access);
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

            return model;
        }

        /// <summary>
        /// Getreasonforresidual
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Getreasonforresidual")]
        public List<ReasonForResidual> Getreasonforresidual(int ProjectID)
        {
            List<ReasonForResidual> model = new List<ReasonForResidual>();
            try
            {
                model = objdebtDataDictionaryRepository.GetReasonForResidualByprojectid(ProjectID);
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

            return model;
        }

        /// <summary>
        /// GetDDErrorLogData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDDErrorLogData")]
        public ActionResult<List<ErrorLogPopUp>> GetDDErrorLogData(Int64 ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,null, Convert.ToInt64(ProjectID));
            DebtDataDictionaryRepository objDebtDataDictionaryRepository = new DebtDataDictionaryRepository();
            try
            {
                if (value)
                {
                    List<ErrorLogPopUp> errorLogCorrection = objDebtDataDictionaryRepository.GetDDErrorLogData(ProjectID);
                    return errorLogCorrection;
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
        /// AddReasonforResidual
        /// </summary>
        /// <param name="objAddReason"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> AddReasonforResidual(AddReasonforResidual objAddReason)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, objAddReason.EmployeeId.ToString(),null, Convert.ToInt64(objAddReason.ProjectId));
            if (value)
            {
                string model = string.Empty;
                try
                {
                    if (!ModelState.IsValid)
                    {
                        ModelState.Clear();
                    }
                    model = objdebtDataDictionaryRepository.AddReasonforResidual
                        (objAddReason).ToString(CultureInfo.CurrentCulture);
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

                return model;
            }
            return Unauthorized();
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Download")]
        public FileResult Download(Datadict dic)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null, Convert.ToInt64(dic.ProjectID));
            try
            {
                if (value)
                {
                    string path = string.Empty;
                    path = objdebtDataDictionaryRepository.
                    ExportToExcelForDataDictionary(dic.ProjectID);
                    //request.Path = RegexPath(request.Path);
                    if (path != null && !string.IsNullOrEmpty(path))
                    {
                        //var path = RegexPath(request.Path);
                        SanitizeStringInput sanitize = new SanitizeStringInput(path);
                        string file = sanitize.Value;
                        string fileName = (new DebtDataDictionaryRepository()).GetFileName(file.Replace("..", string.Empty));
                        string repStr = fileName.Replace("DebtReviewCustomer", "DebtReview");
                        fileName = repStr.ToString(CultureInfo.CurrentCulture);
                        string dirctoryName = Path.GetDirectoryName(file);
                        string fName = Path.GetFileNameWithoutExtension(file);
                        string validatePath = Path.Combine(dirctoryName, fName, ".xlsm");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        //string validatedPath = RegexPath(validatePath);
                        string validatedPath = validatePath;
                        if (System.IO.File.Exists(validatedPath))
                        {
                            //string validatedPath1 = RegexPath(validatedPath);
                            string validatedPath1 = validatedPath;
                            byte[] fileBook = System.IO.File.ReadAllBytes(validatedPath1);
                            var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                            {
                                //FileDownloadName = fileName
                            };
                            return fileContentResult;
                        }
                       // return null;
                    }
                    return null;
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


        private static string RegexPath(string fullPath)
        {
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

        [HttpPost]
        [Route("DownloadDebtOverrideTemplate")]
        public IActionResult DownloadDebtOverrideTemplate(SearchModel searchModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, searchModel.EmployeeID.ToString(), Convert.ToInt64(searchModel.CustomerID), Convert.ToInt64(searchModel.ProjectID));

            try
            {
                if (value)
                {
                    string path = objdebtDataDictionaryRepository.
                        ExportToExcelForDebtReview(Convert.ToDateTime(searchModel.StartDate, CultureInfo.CurrentCulture), Convert.ToDateTime(searchModel.EndDate, CultureInfo.CurrentCulture),
                        searchModel.EmployeeID, searchModel.CustomerID, searchModel.ProjectID, searchModel.isCognizant, searchModel.ReviewStatus);
                    path = RegexPath(path);
                    if (System.IO.File.Exists(path))
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(new SanitizeString(path).Value, FileMode.Open))
                        {
                            stream.CopyTo(memory);
                            stream.Close();
                            stream.Dispose();
                        }
                        string name = Path.GetFileName(new SanitizeString(path).Value);
                        memory.Position = 0;
                        Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                        return File(memory, "application/vnd.ms-excel", name);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string RemoveLastIndexCharacter(string path)
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
        /// <summary>
        /// This Method is used to GetConflictPatternsForDownload
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="EsaProjectId"></param>
        /// <returns></returns
        [HttpGet]
        [Route("GetConflictPatternDetailsForDownload")]
        public string GetConflictPatternDetailsForDownload(int ProjectId, string EsaProjectId)
        {
            try
            {
                string decodedfilename = string.Empty;
                string fileName = string.Concat("ConflictPatternDetails_", EsaProjectId, ".xlsx");
                Regex rgx = new Regex("(\\\\?([^\\/]*[\\/])*)([^\\/]+)");
                if (fileName != null)
                {
                    fileName = fileName.Replace(">", "");
                    fileName = fileName.Replace("<", "");
                    fileName = fileName.Replace("..", "");
                    if (rgx.IsMatch(fileName))
                    {
                        //var path = RegexPath(fileName);
                        var path = fileName;
                        SanitizeStringInput sanitize = new SanitizeStringInput(path);
                        string destinationTemplateFileName = Path.Combine(_hostingEnvironment.ContentRootPath, sanitize.Value);
                        var model = new DebtDataDictionaryRepository().GetConflictPatterns(ProjectId);
                        var fileBytesPath = new DebtDataDictionaryRepository().GetConflictpatternDetailsForDownload(
                            model, destinationTemplateFileName);
                        FileInfo Finfo = new FileInfo(destinationTemplateFileName);
                        //SAST Fix for PATH traversal
                        if (string.IsNullOrEmpty(destinationTemplateFileName))
                        {
                            new ExceptionUtility().LogExceptionMessage(new Exception("Empty FileName"));
                            return (HttpStatusCode.BadRequest).ToString();
                        }
                        else
                        {
                            //1. Decode the file name
                            destinationTemplateFileName = HttpUtility.UrlDecode(destinationTemplateFileName);
                            decodedfilename = Path.GetFileName(destinationTemplateFileName);

                        }
                        if (decodedfilename.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
                        {
                            //2. Find the invalid char
                            new ExceptionUtility().LogExceptionMessage(new Exception("Invalid File Char FileName"));
                            return (HttpStatusCode.BadRequest).ToString();
                        }
                        if (!destinationTemplateFileName.StartsWith(_hostingEnvironment.ContentRootPath))
                        {
                            //3. Check the composed path
                            new ExceptionUtility().LogExceptionMessage(new Exception("Invalid File Char FileName"));
                            return (HttpStatusCode.BadRequest).ToString();
                        }
                        //4. Check the File Extension
                        if (!Finfo.IsReadOnly && Finfo.Extension.ToLower(CultureInfo.CurrentCulture) == "xlsx")
                        {
                            string dirctoryName = Path.GetDirectoryName(destinationTemplateFileName);
                            string fName = Path.GetFileNameWithoutExtension(destinationTemplateFileName);
                            string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                            validatePath = RemoveLastIndexCharacter(validatePath);
                            System.IO.File.Delete(validatePath);
                        }

                        return fileBytesPath;
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

                return null;
            }
            return null;
        }

    }
}
