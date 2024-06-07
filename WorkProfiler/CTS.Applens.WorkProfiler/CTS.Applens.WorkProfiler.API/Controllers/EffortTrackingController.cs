using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// EffortTrackingController
    /// </summary>
    public class EffortTrackingController : BaseController
    {
        public static readonly string FlagIL = InitialLearningConstants.Flag;
        readonly string excelsaveTemplatePath = new ApplicationConstants().DownloadExcelTemp;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// EffortTrackingController 
        /// </summary>
        public EffortTrackingController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor,
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
            this._configuration = configuration;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <returns></returns>
        public ActionResult<UserDetailsBaseModel> Index(string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, null);
            string cognizantID;
            if (EmployeeID == "" || EmployeeID == null)
            {
                cognizantID = CognizantID;
            }
            else
            {
                EmployeeID = HttpUtility.HtmlEncode(EmployeeID);
                cognizantID = EmployeeID;
            }
            TicketingModuleRepository objTicketingModuleRepository = new TicketingModuleRepository();
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            HiddenFieldsModel objHiddenFieldsModel;
            List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();
            List<CustomerModel> objListCustomerModel;
            string errorMsg = string.Empty;
            try
            {
                if (value)
                {
                    objListCustomerModel = objEffortTrackingRepository.GetCustomer(cognizantID);
                    if (objListCustomerModel.Count == 0)
                    {
                        return new UserDetailsBaseModel { ErrorMessage = new ApplicationConstants().AccountMisConfiguredMsg };
                    }
                    else
                    {
                        //CCAP FIX
                    }
                    Int64 CustomerID = Convert.ToInt64(objListCustomerModel[0].CustomerId, CultureInfo.CurrentCulture);
                    objHiddenFieldsModel = objTicketingModuleRepository.GetHiddenFieldsForTM(cognizantID, CustomerID);
                    objHiddenFieldsModel.EmployeeId = cognizantID;
                    if (objHiddenFieldsModel.LstProjectUserID.Count == 0)
                    {
                        return new UserDetailsBaseModel { ErrorMessage = ApplicationConstants.ProjectsMisConfiguredMsg };
                    }
                    else
                    {
                        return new UserDetailsBaseModel
                        {
                            EmployeeName = objHiddenFieldsModel.EmployeeName,
                            HiddenFields = objHiddenFieldsModel,
                            RolePrevilageMenus = objListRolePrivilegeModel,
                            ErrorMessage = "Success"
                        };
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
        /// NoiseEliminationSkipCont
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> NoiseEliminationSkipCont(MLDetailsParam MLParams)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, MLParams.UserId.ToString(), null, Convert.ToInt64(MLParams.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            ILValidationResult objILValidationResult;
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            try
            {
                if (value)
                {
                    string CriteriaMet = debtapproval.UpdateNoiseSkipAndContinue(MLParams.ProjectId, MLParams.UserId);

                    objILValidationResult = debtapproval.ValidateML(CriteriaMet, MLParams.ProjectId, MLParams.UserId,
                        Convert.ToDateTime(MLParams.DateFrom, CultureInfo.CurrentCulture),
                            Convert.ToDateTime(MLParams.DateTo, CultureInfo.CurrentCulture), Convert.ToInt16(MLParams.OptFieldProjId, CultureInfo.CurrentCulture), MLParams.SupportTypeId);
                    return objILValidationResult;
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
        /// GetTicketsForNoiseElimination
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> GetTicketsForNoiseElimination(DebtValidation Param)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, Param.AssociateId.ToString(), null, Convert.ToInt64(Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }

            string jobSuccess;
            DebtFieldsApprovalRepository Debtresp = new DebtFieldsApprovalRepository();
            Param.AssociateId = HttpUtility.HtmlEncode(Param.AssociateId);
            try
            {
                if (value)
                {
                    jobSuccess = Debtresp.GetTicketsForNoiseElimination(HttpUtility.HtmlEncode(Convert.ToString(Param.ProjectId, CultureInfo.CurrentCulture)), Param.AssociateId, 1);

                    return jobSuccess;
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
        /// Getting dropdown values for copy
        /// </summary>
        /// <param name="CustomerID">Customer ID</param>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<CopyFields>> GetDropDownValuesForCopy(long customerID, long projectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, customerID, projectID);
            List<CopyFields> application = new List<CopyFields>();
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            try
            {
                if (value)
                {
                    application = objEffortTrackingRepository.GetDropDownValuesForCopy(customerID, projectID);
                    return application;
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
            return Unauthorized();
        }

        /// <summary>
        /// GetUserName
        /// </summary>
        /// <returns></returns>
        public HiddenFieldsModel GetUserName()
        {
            string AssociateID = CognizantID;

            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            HiddenFieldsModel objHiddenFieldsModel;
            List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();
            try
            {
                objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(AssociateID);

                return objHiddenFieldsModel;
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
        /// GetCustomer
        /// </summary>
        /// <returns></returns>
        public List<CustomerModel> GetCustomer()
        {
            string AssociateID = CognizantID;
            List<CustomerModel> objListCustomerModel;
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            try
            {
                objListCustomerModel = objEffortTrackingRepository.GetCustomer(AssociateID);
                return objListCustomerModel;
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
        /// DebtDownloadTemplate
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> DebtDownloadTemplate(string projectID, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
            HttpResponseMessage response = new HttpResponseMessage();
            string LOBDumpUploadStatus = string.Empty;
            try
            {
                if (value)
                {
                    LOBDumpUploadStatus = debtApproval.ExportToExcelForML(Convert.ToInt32(projectID, CultureInfo.CurrentCulture), SupportTypeID);
                    return LOBDumpUploadStatus;
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

            return Unauthorized();
        }

        /// <summary>
        /// MLBaseDetailsDown
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="actionValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> MLBaseDetailsDown(string ProjectID, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            HttpResponseMessage response = new HttpResponseMessage();
            string LOBDumpUploadStatus = string.Empty;

            try
            {
                if (value)
                {
                    LOBDumpUploadStatus = debtapproval.UpdatePatternId(Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), SupportTypeID);
                    return LOBDumpUploadStatus;
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
            return Unauthorized();
        }

        /// <summary>
        /// MLEntireBaseDetailsDown
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="actionValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string MLEntireBaseDetailsDown(string ProjectID, int actionValue)
        {
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            HttpResponseMessage response = new HttpResponseMessage();
            string LOBDumpUploadStatus = string.Empty;

            try
            {
                //CCAP FIX
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
            return LOBDumpUploadStatus;
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileResult Download(string file)
        {
            string ext = Path.GetExtension(file);
            string fName;
            string validatePath;
            string filestring = Path.GetFileName(file);
            string dirctoryName = Path.GetDirectoryName(file);
            fName = Path.GetFileNameWithoutExtension(file);
            //VeraCode Fix
            if (ext == ".xlsm")
            {
                validatePath = Path.Combine(dirctoryName, fName, ".xlsm");
            }
            else
            {
                validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
            }

            validatePath = RemoveLastIndexCharacter(validatePath);
            validatePath = Logger.RegexPath(validatePath);

            if (System.IO.File.Exists(new SanitizeString(validatePath).Value))
            {
                byte[] fileBook = System.IO.File.ReadAllBytes(new SanitizeString(validatePath).Value);
                var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                {
                    FileDownloadName = filestring.Replace("..", string.Empty)
                };

                return fileContentResult;
            }

            return null;
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
        /// GetMLDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<GetMLDetails>> GetMLDetails(int projectID, string UserID, int SupportTypeId)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    var ProcessingDetails = debtapproval.GetProcessingRequiredOnLoad(projectID, 1);
                    string IsMLProcessingRequired = ProcessingDetails.IsMLProcessingRequired;
                    string IsSamplingProcessingRequired = ProcessingDetails.IsSamplingProcessingRequired;
                    string MLJobId = ProcessingDetails.MLJobId;
                    string SamplingJobId = ProcessingDetails.SamplingJobId;
                    string IsMLSent = ProcessingDetails.IsMLSent;
                    string NoiseEliminationJobId = ProcessingDetails.NoiseEliminationJobId;
                    string IsNoiseEliminationSent = ProcessingDetails.IsNoiseEliminationSent;
                    string IsSamplingSent = ProcessingDetails.IsSamplingSent;
                    if (string.Compare(IsMLSent, FlagIL) == 0)
                    {
                        debtapproval.CheckIfMLFileGenerated(projectID, MLJobId, InitialLearningConstants.JobTypeML,
                            UserID, SupportTypeId);
                    }
                    else if (string.Compare(IsSamplingSent, FlagIL) == 0)
                    {
                        debtapproval.CheckIfMLFileGenerated(projectID, SamplingJobId,
                            InitialLearningConstants.JobTypeSampling, UserID, SupportTypeId);
                    }
                    else if (string.Compare(IsMLProcessingRequired, FlagIL) == 0)
                    {
                        UpdateMLPatternFromCSV(projectID, MLJobId, UserID);
                    }
                    else if (string.Compare(IsSamplingProcessingRequired, FlagIL) == 0)
                    {
                        UpdateSampledTicketsFromCSV(projectID, SamplingJobId, UserID);
                    }
                    else
                    {
                        //mandatory else
                    }

                    var debtdetails = debtapproval.GetMLDetailsOnLoad(projectID, 1);
                    return debtdetails;
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

        ///// <summary>
        ///// CheckAutoClassified
        ///// </summary>
        ///// <param name="projectID"></param>
        ///// <returns></returns>

        /// <summary>
        /// UpdateMLPatternFromCSV
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="MLJobId"></param>
        /// <param name="UserID"></param>
        public ActionResult UpdateMLPatternFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectId));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    var debtTicketDetails = debtapproval.UpdateMLPatternFromCSV(ProjectId, MLJobId, UserID);
                    return Ok();
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
            return Unauthorized();
        }

        /// <summary>
        /// SaveDebtSamplingDetails
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProjectID"></param>
        /// <param name="lstDebtSampling"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContentResult> SaveDebtSamplingDetails(string UserId, string ProjectID, int SupportId,
           List<DebtSamplingGetModel> lstDebtSampling)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserId, null, Convert.ToInt64(ProjectID));
            string result = "";

            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            InitialLearningRepository infradebtapproval = new InitialLearningRepository();

            List<GetDebtSamplingDetails> lstDebtSamplingModified = new List<GetDebtSamplingDetails>();

            HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted };

            try
            {
                if (value)
                {
                    if (SupportId == 1)
                    {
                        foreach (DebtSamplingGetModel debtsampling in lstDebtSampling)
                        {
                            lstDebtSamplingModified.Add(new GetDebtSamplingDetails
                            {
                                TicketId = debtsampling.TicketId,
                                TicketDescription = debtsampling.TicketDescription,
                                AdditionalText = debtsampling.AdditionalText,
                                DebtClassificationId = debtsampling.DebtClassificationId.ToString(),
                                AvoidableFlagId = debtsampling.AvoidableFlagId.ToString(),
                                PresenceOfOptional = debtsampling.PresenceOfOptional,
                                OptionalField = debtsampling.OptionalField,
                                CauseCode = debtsampling.CauseCode.ToString(CultureInfo.CurrentCulture),
                                ResolutionCode = debtsampling.ResolutionCode.ToString(CultureInfo.CurrentCulture),
                                ResidualDebtId = debtsampling.ResidualDebtId.ToString(),
                                DescBaseWorkPattern = debtsampling.DescBaseWorkPattern,
                                DescSubWorkPattern = debtsampling.DescSubWorkPattern,
                                ResBaseWorkPattern = debtsampling.ResBaseWorkPattern,
                                ResSubWorkPattern = debtsampling.ResSubWorkPattern,
                                ApplicationId = debtsampling.ApplicationId.ToString()
                            });
                        }

                        result = debtapproval.SaveDebtSamplingDetails(UserId, ProjectID, lstDebtSamplingModified);

                    }
                    else
                    {
                        foreach (DebtSamplingGetModel debtsampling in lstDebtSampling)
                        {
                            lstDebtSamplingModified.Add(new GetDebtSamplingDetails
                            {
                                TicketId = debtsampling.TicketId,
                                TicketDescription = debtsampling.TicketDescription,
                                AdditionalText = debtsampling.AdditionalText,
                                DebtClassificationId = debtsampling.DebtClassificationId.ToString(),
                                AvoidableFlagId = debtsampling.AvoidableFlagId.ToString(),
                                PresenceOfOptional = debtsampling.PresenceOfOptional,
                                OptionalField = debtsampling.OptionalField,
                                CauseCode = debtsampling.CauseCode.ToString(CultureInfo.CurrentCulture),
                                ResolutionCode = debtsampling.ResolutionCode.ToString(CultureInfo.CurrentCulture),
                                ResidualDebtId = debtsampling.ResidualDebtId.ToString(),
                                DescBaseWorkPattern = debtsampling.DescBaseWorkPattern,
                                DescSubWorkPattern = debtsampling.DescSubWorkPattern,
                                ResBaseWorkPattern = debtsampling.ResBaseWorkPattern,
                                ResSubWorkPattern = debtsampling.ResSubWorkPattern,
                                TowerId = debtsampling.TowerId
                            });
                        }

                        result = infradebtapproval.SaveDebtSamplingDetails(UserId, ProjectID, lstDebtSamplingModified);

                    }
                    if (string.Compare(result, FlagIL) == 0)
                    {
                        response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        result = "success";
                    }
                    return Content(result);
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
        /// <param name="UserId"></param>
        /// <param name="ProjectID"></param>
        /// <param name="lstDebtSampling"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> SubmitDebtSamplingDetails(DebtValidationSamplingDetails debtValidationSamplingDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, debtValidationSamplingDetails.Param.AssociateId, null,
                Convert.ToInt64(debtValidationSamplingDetails.Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }

            string JobSuccess = string.Empty;
            ILValidationResult objILValidationResult = new ILValidationResult();
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            InitialLearningRepository infradebtapproval = new InitialLearningRepository();
            List<GetDebtSamplingDetails> lstDebtSamplingModified = new List<GetDebtSamplingDetails>();
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted };
            try
            {
                if (value)
                {
                    if (debtValidationSamplingDetails.Param.SupportTypeId == 1)
                    {
                        foreach (DebtSamplingGetModel debtsampling in debtValidationSamplingDetails.DebtSamplings)
                        {
                            lstDebtSamplingModified.Add(new GetDebtSamplingDetails
                            {
                                TicketId = debtsampling.TicketId,
                                TicketDescription = debtsampling.TicketDescription,
                                AdditionalText = debtsampling.AdditionalText,
                                DebtClassificationId = debtsampling.DebtClassificationId.ToString(),
                                AvoidableFlagId = debtsampling.AvoidableFlagId.ToString(),
                                CauseCode = debtsampling.CauseCode.ToString(CultureInfo.CurrentCulture),
                                ResolutionCode = debtsampling.ResolutionCode.ToString(CultureInfo.CurrentCulture),
                                PresenceOfOptional = debtsampling.PresenceOfOptional,
                                OptionalField = debtsampling.OptionalField,
                                ResidualDebtId = debtsampling.ResidualDebtId.ToString(),
                                DescBaseWorkPattern = debtsampling.DescBaseWorkPattern,
                                DescSubWorkPattern = debtsampling.DescSubWorkPattern,
                                ResBaseWorkPattern = debtsampling.ResBaseWorkPattern,
                                ResSubWorkPattern = debtsampling.ResSubWorkPattern,
                                ApplicationId = debtsampling.ApplicationId.ToString()

                            });

                        }


                        //result = 
                        debtapproval.SubmitDebtSamplingDetails(debtValidationSamplingDetails.Param.UserId,
                        Convert.ToInt32(debtValidationSamplingDetails.Param.ProjectId),
                            lstDebtSamplingModified);
                    }
                    else
                    {
                        foreach (DebtSamplingGetModel debtsampling in debtValidationSamplingDetails.DebtSamplings)
                        {
                            lstDebtSamplingModified.Add(new GetDebtSamplingDetails
                            {
                                TicketId = debtsampling.TicketId,
                                TicketDescription = debtsampling.TicketDescription,
                                AdditionalText = debtsampling.AdditionalText,
                                DebtClassificationId = debtsampling.DebtClassificationId.ToString(),
                                AvoidableFlagId = debtsampling.AvoidableFlagId.ToString(),
                                CauseCode = debtsampling.CauseCode.ToString(CultureInfo.CurrentCulture),
                                ResolutionCode = debtsampling.ResolutionCode.ToString(CultureInfo.CurrentCulture),
                                PresenceOfOptional = debtsampling.PresenceOfOptional,
                                OptionalField = debtsampling.OptionalField,
                                ResidualDebtId = debtsampling.ResidualDebtId.ToString(),
                                DescBaseWorkPattern = debtsampling.DescBaseWorkPattern,
                                DescSubWorkPattern = debtsampling.DescSubWorkPattern,
                                ResBaseWorkPattern = debtsampling.ResBaseWorkPattern,
                                ResSubWorkPattern = debtsampling.ResSubWorkPattern,
                                TowerId = debtsampling.TowerId

                            });

                        }


                        // result =
                        infradebtapproval.SubmitDebtSamplingDetails(debtValidationSamplingDetails.Param.UserId,
                        Convert.ToInt32(debtValidationSamplingDetails.Param.ProjectId),
                        lstDebtSamplingModified);
                    }
                    //Call ML on success
                    //trigger ML after for submitted data
                    JobSuccess = MLDatSetBindingAfterSamplingForCSVCreation(debtValidationSamplingDetails.Param);
                    if (string.Compare(JobSuccess, FlagIL) == 0)
                    {
                        JobSuccess = debtapproval.UpdateSamplingSubmitFlag(Convert.ToInt32(debtValidationSamplingDetails.Param.ProjectId),
                            debtValidationSamplingDetails.Param.UserId, debtValidationSamplingDetails.Param.SupportTypeId);
                    }

                    List<GetMLDetails> lstMLDetails =
                    debtapproval.GetMLDetailsOnLoad(Convert.ToInt32(debtValidationSamplingDetails.Param.ProjectId),
                    debtValidationSamplingDetails.Param.SupportTypeId);
                    objILValidationResult = debtapproval.MLDetailsLoad(lstMLDetails);
                }
                return objILValidationResult;

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

            return Unauthorized();

        }

        /// <summary>
        /// MLDatSetBindingAfterSamplingForCSVCreation
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        public static string MLDatSetBindingAfterSamplingForCSVCreation(DebtValidation Param)
        {
            string JobSuccess = string.Empty;
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            InitialLearningRepository initialapproval = new InitialLearningRepository();

            JobSuccess = debtapproval.MLDatSetBindingAfterSamplingForCSVCreation(Param.ProjectId, Param.AssociateId,
                Param.SupportTypeId);

            return JobSuccess;
        }

        /// <summary>
        /// SaveDebtPatternValidation
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstDebtMLPatternModel"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContentResult> SaveDebtPatternValidation(int ProjectID,
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel, string UserId, int SupportType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserId, null, Convert.ToInt64(ProjectID));
            string result = string.Empty;
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted };

            ArrayList paramList = new ArrayList();
            try
            {
                if (value)
                {
                    paramList.Add(ProjectID);
                    paramList.Add(lstDebtMLPatternModel);
                    paramList.Add(UserId);


                    if (paramList.Count > 0)
                    {
                        result = debtapproval.SaveDebtPatternValidationDetails(ProjectID, lstDebtMLPatternModel,
                            UserId, SupportType);
                        if (string.Compare(result, FlagIL) == 0)
                        {
                            response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
                        }
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        result = "success";
                    }
                }
                return Content(result);
            }
            catch (Exception)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            }
            return Unauthorized();
        }

        /// <summary>
        /// Used to copy patterns functionality
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstCopyPatternsModel"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContentResult> CopyPatterns(int projectID, List<DebtMLPatternValidationModel> lstCopyPatternsModel,
            string userId)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, userId, null, Convert.ToInt64(projectID));
            string result = string.Empty;
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
                    HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted };

                    ArrayList paramList = new ArrayList();

                    paramList.Add(projectID);
                    paramList.Add(lstCopyPatternsModel);
                    paramList.Add(userId);

                    if (paramList.Count > 0)
                    {
                        result = debtApproval.CopyPatterns(projectID, lstCopyPatternsModel, userId);
                        if (string.Compare(result, FlagIL) == 0)
                        {
                            response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
                        }
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        result = "success";
                    }
                }
                return Content(result);

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

            return Unauthorized();
        }


        /// <summary>
        /// DebtMLGetPatternOccurence
        /// </summary>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <param name="AppID">This parameter holds AppID value</param>
        /// <param name="ApplicationName">This parameter holds ApplicationName value</param>
        /// <param name="TicketPattern">This parameter holds TicketPattern value</param>
        /// <param name="subPattern">This parameter holds subPattern value</param>
        /// <param name="AdditionalTextPattern">This parameter holds AdditionalTextPattern value</param>
        /// <param name="AdditionalTextSubPattern">This parameter holds AdditionalTextSubPattern value</param>
        /// <param name="CauseCodeId">This parameter holds CauseCodeId value</param>
        /// <param name="ResolutionCodeId">This parameter holds ResolutionCodeId value</param>
        /// <param name="AvoidableFlagId">This parameter holds AvoidableFlagId value</param>
        /// <param name="DebtClassificationId">This parameter holds DebtClassificationId value</param>
        /// <param name="ResidualFlagId">This parameter holds ResidualFlagId value</param>
        /// <param name="CauseCodeName">This parameter holds CauseCodeName value</param>
        /// <param name="ResolutionCodeName"></param>
        /// <returns>Method returns DebtMLGetPatternOccurence</returns>
        public ActionResult<PatternOccurenceInfra> DebtMLGetPatternOccurence(int ProjectID, int[] AppID, string ApplicationName,
            string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextSubPattern,
            int CauseCodeId, int ResolutionCodeId, int AvoidableFlagId, int DebtClassificationId,
            int ResidualFlagId, string CauseCodeName, string ResolutionCodeName, int SupportType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel = new List<DebtMLPatternValidationModel>();
            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
            List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstCauseCodeModel;
            List<DebtMasterValues> lstResolutionCodeModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstDebtClassificationModel;
            List<DebtMasterValues> lstAvoidableFlagModel;
            List<DebtMasterValues> lstResidualDebtModel;
            try
            {
                if (value)
                {
                    string appIds = string.Join(",", AppID.Select(x => x.ToString(CultureInfo.CurrentCulture)).ToArray());
                    var DebtMLPatterndetails = debtApproval.GetDebtMLPatternOccurenceReport(ProjectID, appIds,
                        TicketPattern.Trim(), subPattern.Trim(), AdditionalTextPattern.Trim(),
                        AdditionalTextSubPattern.Trim(), CauseCodeId, ResolutionCodeId);
                    var debtMasterdetails = debtApproval.GetDebtMasterValues(ProjectID, SupportType);

                    foreach (var masterValues in debtMasterdetails)
                    {
                        lstMLDebtMasterValuesModel.Add(new DebtMasterValues
                        {
                            AttributeType = masterValues.AttributeType,
                            AttributeTypeId = masterValues.AttributeTypeId,
                            AttributeTypeValue = masterValues.AttributeTypeValue
                        });
                    }
                    lstCauseCodeModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.CauseCode).ToList();
                    lstAvoidableFlagModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.AvoidableFlag).ToList();
                    lstDebtClassificationModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.DebtClassification).ToList();
                    lstResidualDebtModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.ResidualDebt).ToList();
                    if (DebtMLPatterndetails.Count > 0)
                    {
                        foreach (var tickets in DebtMLPatterndetails)
                        {
                            lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id),
                                TicketPattern = tickets.TicketPattern,
                                SubPattern = tickets.SubPattern,
                                AdditionalTextPattern = tickets.AdditionalTextPattern,
                                AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
                                TicketOccurence = tickets.TicketOccurence,
                                MLDebtClassificationId = tickets.MLDebtClassificationId,
                                MLDebtClassificationName = tickets.MLDebtClassificationName,
                                MLResidualFlagId = tickets.MLResidualFlagId,
                                MLResidualFlagName = tickets.MLResidualFlagName,
                                MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                                MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                                MLCauseCodeId = Convert.ToInt32(tickets.MLCauseCodeId, CultureInfo.CurrentCulture),
                                MLCauseCodeName = tickets.MLCauseCodeName,
                                MLResolutionCodeId = tickets.MLResolutionCodeId,
                                MLResolutionCodeName = tickets.MLResolutionCode,
                                LstCauseCodeModel = lstCauseCodeModel,
                                LstAvoidableFlagModel = lstAvoidableFlagModel,
                                LstDebtClassificationModel = lstDebtClassificationModel,
                                LstResidualDebtModel = lstResidualDebtModel,
                                ApplicationName = tickets.ApplicationName,
                                ApplicationId = tickets.ApplicationId,
                                MLAccuracy = tickets.MLAccuracy

                            });
                        }
                    }

                    else
                    {
                        lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                        {
                            Id = 0,
                            TicketPattern = TicketPattern,
                            SubPattern = (subPattern == null || subPattern == "") ? "N/A" : subPattern,
                            AdditionalTextPattern = !(AdditionalTextPattern == null || AdditionalTextPattern == "") ?
                                                  AdditionalTextPattern : "N/A", //CCAP FIX
                            AdditionalTextsubPattern = !(AdditionalTextPattern == null || AdditionalTextPattern == "") ?
                                                    AdditionalTextPattern : "N/A",
                            TicketOccurence = 0,
                            MLDebtClassificationId = 0,
                            MLDebtClassificationName = string.Empty,
                            MLResidualFlagId = 0,
                            MLResidualFlagName = string.Empty,
                            MLAvoidableFlagId = 0,
                            MLAvoidableFlagName = string.Empty,
                            MLCauseCodeId = CauseCodeId,
                            MLCauseCodeName = CauseCodeName,
                            MLResolutionCodeName = ResolutionCodeName,
                            LstCauseCodeModel = lstCauseCodeModel,
                            LstAvoidableFlagModel = lstAvoidableFlagModel,
                            LstDebtClassificationModel = lstDebtClassificationModel,
                            LstResidualDebtModel = lstResidualDebtModel,
                            ApplicationName = ApplicationName,
                            MLResolutionCodeId = ResolutionCodeId,
                            ApplicationId = Convert.ToInt32(appIds, CultureInfo.CurrentCulture),
                            MLAccuracy = string.Empty
                        });
                    }

                    return new PatternOccurenceInfra
                    {
                        AvoidableFlagID = AvoidableFlagId,
                        DebtClassificationID = DebtClassificationId,
                        ResidualCodeID = ResidualFlagId,
                        DebtMLPatternValidationModelDetails = lstDebtMLPatternModel
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
        /// GetDebtSampling
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public ActionResult<List<DebtSamplingGetModel>> GetDebtSampling(int projectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            List<DebtSamplingGetModel> lstApprovalModel = new List<DebtSamplingGetModel>();
            List<DebtSamplingValues> lstDebtMasterValuesModel = new List<DebtSamplingValues>();

            List<DebtSamplingValues> lstCauseCodeModel;
            List<DebtSamplingValues> lstResolutionCodeModel;
            List<DebtSamplingValues> lstDebtClassificationModel;
            List<DebtSamplingValues> lstAvoidableFlagModel;
            List<DebtSamplingValues> lstResidualDebtModel;
            try
            {
                if (value)
                {
                    var debtdetails = debtapproval.GetDebtSamplingData(projectID);
                    var debtMasterdetails = debtapproval.GetDebtSamplingDataValues(projectID, 1);
                    foreach (var MasterValues in debtMasterdetails)
                    {
                        lstDebtMasterValuesModel.Add(new DebtSamplingValues
                        {
                            AttributeType = MasterValues.AttributeType,
                            AttributeTypeId = MasterValues.AttributeTypeId,
                            AttributeTypeValue = MasterValues.AttributeTypeValue
                        });
                    }
                    lstCauseCodeModel = lstDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.CauseCode).ToList();
                    lstResolutionCodeModel = lstDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.ResolutionCode).ToList();
                    lstAvoidableFlagModel = lstDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.AvoidableFlag).ToList();
                    lstDebtClassificationModel = lstDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.DebtClassification).ToList();
                    lstResidualDebtModel = lstDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.ResidualDebt).ToList();

                    foreach (var tickets in debtdetails)
                    {
                        lstApprovalModel.Add(new DebtSamplingGetModel
                        {
                            TicketId = tickets.TicketId,
                            ApplicationId = tickets.ApplicationId,
                            ApplicationName = tickets.ApplicationName,
                            CauseCodeId = tickets.CauseCodeId,
                            CauseCode = tickets.CauseCode,
                            ResolutionCodeId = tickets.ResolutionCodeId,
                            ResolutionCode = tickets.ResolutionCode,
                            DebtClassificationId = tickets.DebtClassificationId,
                            DebtClassificationName = tickets.DebtClassificationName,
                            ResidualDebtId = tickets.ResidualDebtId,
                            ResidualDebt = tickets.ResidualDebt,
                            TicketDescription = tickets.TicketDescription,
                            AvoidableFlagId = tickets.AvoidableFlagId,
                            AvoidableFlagName = tickets.AvoidableFlagName,
                            AdditionalText = tickets.AdditionalText,
                            PresenceOfOptional = tickets.PresenceOfOptional,
                            OptionalField = tickets.OptionalField,
                            ResBaseWorkPattern = tickets.ResBaseWorkPattern,
                            ResSubWorkPattern = tickets.ResSubWorkPattern,
                            DescBaseWorkPattern = tickets.DescBaseWorkPattern,
                            DescSubWorkPattern = tickets.DescSubWorkPattern,


                            LstCauseCodeModel = lstCauseCodeModel,
                            LstResolutionCodeModel = lstResolutionCodeModel,
                            LstResidualDebtModel = lstResidualDebtModel,

                            LstAvoidableFlagModel = lstAvoidableFlagModel,
                            LstDebtClassificationModel = lstDebtClassificationModel,

                        });
                    }
                }
                return lstApprovalModel;
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
            return Unauthorized();

        }

        /// <summary>
        /// UpdateSamplingFlag
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AssociateID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> UpdateSamplingFlag(Int32 ProjectID, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, Int32 SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, AssociateID, null, Convert.ToInt64(ProjectID));
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            ILValidationResult objILValidationResult;
            InitialLearningController objInitialLearningController = new InitialLearningController(_configuration, _httpContextAccessor, _hostingEnvironment);
            try
            {
                if (value)
                {
                    var lstGetSamplingSentds = debtapproval.UpdateSamplingFlag(ProjectID, AssociateID, StartDate,
                        EndDate, OptionalFieldProj, SupportTypeID);
                    objILValidationResult = objInitialLearningController.GetProgressStatusCode(ProjectID, SupportTypeID,
                        AssociateID);
                    return objILValidationResult;
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
        /// UpdateNoiseEliminationFlag
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AssociateID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<GetMLDetails>> UpdateNoiseEliminationFlag(Int32 projectID, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, AssociateID, null, Convert.ToInt64(projectID));
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            try
            {
                if (value)
                {
                    var lstGetSamplingSentds = debtapproval.UpdateNoiseEliminationFlag(projectID, AssociateID,
                        StartDate, EndDate, OptionalFieldProj, SupportTypeID);
                    return lstGetSamplingSentds;
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
        /// UpdateSampledTicketsFromCSV
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="MLJobId"></param>
        /// <param name="UserID"></param>
        public ActionResult UpdateSampledTicketsFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectId));
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            try
            {
                if (value)
                {
                    var debtTicketDetails = debtapproval.UpdateSampledTicketsFromCSV(ProjectId, MLJobId, UserID);
                    return Ok();
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
            return Unauthorized();
        }

        /// <summary>
        /// MLSendDataForSampling
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult<string> MLSendDataForSampling(MLDetailsParam MLUserdetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, MLUserdetails.AssociateId, null, null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    string jobSuccess;
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    DataSet DS_CSVCreation = new DataSet();
                    DS_CSVCreation.Locale = CultureInfo.InvariantCulture;
                    SanitizeStringInput sanAssociateID = MLUserdetails.AssociateId;
                    DataTable ordersTable = DS_CSVCreation.Tables.Add("SentOrReceive");
                    jobSuccess = debtapproval.SamplingDatSetBindingForCSVCreation(MLUserdetails);
                    return jobSuccess;
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
        /// Get the Status oF THE Project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="UserID"></param>
        /// <param name="OptFieldProjID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> UpdateOptUpload(MLDetailsParam MLParams)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, MLParams.ProjectId);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    ILValidationResult result;
                    string CriteriaMet = debtapproval.UpdateOptUpload(MLParams.ProjectId, MLParams.SupportTypeId);
                    result = debtapproval.ValidateML(CriteriaMet, MLParams.ProjectId, MLParams.UserId, Convert.ToDateTime(MLParams.DateFrom, CultureInfo.CurrentCulture),
                            Convert.ToDateTime(MLParams.DateTo, CultureInfo.CurrentCulture), Convert.ToInt16(MLParams.OptFieldProjId, CultureInfo.CurrentCulture), MLParams.SupportTypeId);
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
        /// GetOptionalFieldsOnLoad
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<ProjOptionalFields>> GetOptionalFieldsOnLoad(int projectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    List<ProjOptionalFields> lstProjOptional = debtapproval.GetOptionalFieldsOnLoad(projectID);
                    return lstProjOptional;
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
        /// <summary>
        /// SignoffDebtPatternValidation
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="DateFrom"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult<ContentResult> SignoffDebtPatternValidation(DebtValidation Param)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    string result = "";
                    SanitizeStringInput sanAssociateID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    HttpResponseMessage response = new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted };
                    result = debtapproval.SignOffDebtPatternValidation(Param.ProjectId, Convert.ToDateTime(Param.DateFrom, CultureInfo.CurrentCulture), sanAssociateID.Value, Param.SupportTypeId);
                    if (result == FlagIL)
                    {
                        response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        result = "success";
                    }

                    return Content(result);
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
        /// DebtMLPatternValidationGrid
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<DebtMLPatternValidationModel>> DebtMLPatternValidationGrid(int ProjectId, string UserID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectId));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
                    List<DebtMLPatternValidationModel> lstDebtMLPatternModel = new List<DebtMLPatternValidationModel>();
                    List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
                    List<DebtMasterValues> lstCauseCodeModel;
                    List<DebtMasterValues> lstResolutionCodeModel = new List<DebtMasterValues>();
                    List<DebtMasterValues> lstDebtClassificationModel;
                    List<DebtMasterValues> lstAvoidableFlagModel;
                    List<DebtMasterValues> lstResidualDebtModel;
                    var debtdetails = debtApproval.GetMLDetailsOnLoad(ProjectId, 1);
                    var debtMLPatterndetails = debtApproval.GetDebtMLPatternValidationReport(ProjectId, UserID);
                    var debtMasterdetails = debtApproval.GetDebtMasterValues(ProjectId, 2);

                    foreach (var masterValues in debtMasterdetails)
                    {
                        lstMLDebtMasterValuesModel.Add(new DebtMasterValues
                        {
                            AttributeType = masterValues.AttributeType,
                            AttributeTypeId = masterValues.AttributeTypeId,
                            AttributeTypeValue = masterValues.AttributeTypeValue
                        });
                    }
                    lstCauseCodeModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.CauseCode).ToList();
                    lstAvoidableFlagModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.AvoidableFlag).
                        ToList();
                    lstDebtClassificationModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.DebtClassification).ToList();
                    lstResidualDebtModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.ResidualDebt).ToList();

                    foreach (var tickets in debtMLPatterndetails)
                    {
                        lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                        {
                            Id = Convert.ToInt32(tickets.Id),
                            InitialLearningId = tickets.InitialLearningId,
                            ApplicationId = tickets.ApplicationId,
                            ApplicationName = tickets.ApplicationName,
                            ApplicationTypeId = tickets.ApplicationTypeId,
                            ApplicationTypeName = tickets.ApplicationTypeName,
                            TechnologyId = tickets.TechnologyId,
                            TechnologyName = tickets.TechnologyName,
                            TicketPattern = tickets.TicketPattern,
                            SubPattern = tickets.SubPattern,
                            AdditionalTextPattern = tickets.AdditionalTextPattern,
                            AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
                            MLDebtClassificationId = tickets.MLDebtClassificationId,
                            MLDebtClassificationName = tickets.MLDebtClassificationName,
                            MLResidualFlagId = tickets.MLResidualFlagId,
                            MLResidualFlagName = tickets.MLResidualFlagName,
                            MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                            MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                            MLAccuracy = tickets.MLAccuracy,
                            MLCauseCodeId = tickets.MLCauseCodeId,
                            MLCauseCodeName = tickets.MLCauseCodeName,
                            MLResolutionCodeId = tickets.MLResolutionCodeId,
                            MLResolutionCodeName = tickets.MLResolutionCode,
                            TicketOccurence = tickets.TicketOccurence,
                            AnalystResolutionCodeId = tickets.AnalystResolutionCodeId,
                            AnalystResolutionCodeName = tickets.AnalystResolutionCodeName,
                            AnalystCauseCodeId = tickets.AnalystCauseCodeId,
                            AnalystCauseCodeName = tickets.AnalystCauseCodeName,

                            AnalystDebtClassificationId = tickets.AnalystDebtClassificationId,
                            AnalystDebtClassificationName = tickets.AnalystDebtClassificationName,
                            AnalystAvoidableFlagId = tickets.AnalystAvoidableFlagId,
                            AnalystAvoidableFlagName = tickets.AnalystAvoidableFlagName,
                            SMEComments = tickets.SMEComments,
                            SMEResidualFlagId = tickets.SMEResidualFlagId,
                            SMEResidualFlagName = tickets.SMEResidualFlagName,
                            SMEDebtClassificationId = tickets.SMEDebtClassificationId,
                            SMEDebtClassificationName = tickets.SMEDebtClassificationName,
                            SMEAvoidableFlagId = tickets.SMEAvoidableFlagId,
                            SMEAvoidableFlagName = tickets.SMEAvoidableFlagName,
                            SMECauseCodeId = tickets.SMECauseCodeId,
                            SMECauseCodeName = tickets.SMECauseCodeName,
                            IsApprovedOrMute = tickets.IsApprovedOrMute,
                            LstCauseCodeModel = lstCauseCodeModel,
                            LstAvoidableFlagModel = lstAvoidableFlagModel,
                            LstDebtClassificationModel = lstDebtClassificationModel,
                            LstResidualDebtModel = lstResidualDebtModel,
                            IsApproved = tickets.IsApproved,
                            IsMLSignoff = tickets.IsMLSignoff,
                            IsRegMLSignoff = debtdetails.Count > 0 ? Convert.ToBoolean(debtdetails[0].IsRegMLsignOff) : false,
                            OverriddenPatternTotalCount = tickets.OverridenTotalCount,
                            IsRegenerated = tickets.IsRegenerated
                        });
                    }

                    return lstDebtMLPatternModel;
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
        /// Update IL count details
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <param name="TicketConsidered"></param>
        /// <param name="TicketAnalysed"></param>
        /// <param name="SamplingCount"></param>
        /// <param name="PatternCount"></param>
        /// <param name="ApprovedCount"></param>
        /// <param name="MuteCount"></param>
        /// <param name="userid"></param>
        public ActionResult UpdateILCountDetails(DebtValidation Param)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, Param.UserId, null, Convert.ToInt64(Param.ProjectId));
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            try
            {
                if (value)
                {
                    objDebtFieldsApprovalRepository.UpdateILCountDetails(Convert.ToString(Param.Choose, CultureInfo.CurrentCulture), Param.ProjectId, Param.TicketConsidered,
                        Param.TicketAnalysed, Param.SamplingCount, Param.PatternCount, Param.ApproveCount, Param.MuteCount, Param.UserId, Param.SupportTypeId);
                    return Ok();
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
        /// Used to get details for view all screen
        /// </summary>
        /// <param name="ProjectId">Project ID</param>
        /// <param name="DateFrom">From Date</param>
        /// <param name="DateTo">To Date</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DebtMLPatternValidationModelForViewAll> DebtMLPatternValidationGridViewALL(int projectId, DateTime dateFrom, DateTime dateTo, string userID, int SupportType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, userID, null, Convert.ToInt64(projectId));
            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
            DebtMLPatternValidationModelForViewAll lstDebtMLPatternModel = new DebtMLPatternValidationModelForViewAll();
            List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstCauseCodeModel;
            List<DebtMasterValues> lstResolutionCodeModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstDebtClassificationModel;
            List<DebtMasterValues> lstAvoidableFlagModel;
            List<DebtMasterValues> lstResidualDebtModel;
            try
            {
                if (value)
                {
                    var debtDetails = debtApproval.GetMLDetailsOnLoad(projectId, 1);
                    SpDebtMLPatternValidationModelForViewAll debtMLPatternValidationList =
                        debtApproval.GetDebtMLPatternValidationReportForViewAll(projectId, SupportType);

                    var debtMasterdetails = debtApproval.GetDebtMasterValues(projectId, SupportType);
                    foreach (var masterValues in debtMasterdetails)
                    {
                        lstMLDebtMasterValuesModel.Add(new DebtMasterValues
                        {
                            AttributeType = masterValues.AttributeType,
                            AttributeTypeId = masterValues.AttributeTypeId,
                            AttributeTypeValue = masterValues.AttributeTypeValue
                        });
                    }

                    lstCauseCodeModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.CauseCode).ToList();
                    lstAvoidableFlagModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.AvoidableFlag).ToList();
                    lstDebtClassificationModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.DebtClassification).ToList();
                    lstResidualDebtModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == ApplicationConstants.ResidualDebt).ToList();

                    if (debtMLPatternValidationList.ExistingPatternsModel != null)
                    {
                        foreach (var tickets in debtMLPatternValidationList.ExistingPatternsModel)
                        {
                            lstDebtMLPatternModel.ExistingPatterns.Add(new DebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id),
                                InitialLearningId = tickets.InitialLearningId,
                                ApplicationId = tickets.ApplicationId,
                                ApplicationName = tickets.ApplicationName,
                                ApplicationTypeId = tickets.ApplicationTypeId,
                                ApplicationTypeName = tickets.ApplicationTypeName,
                                TechnologyId = tickets.TechnologyId,
                                TechnologyName = tickets.TechnologyName,
                                TicketPattern = tickets.TicketPattern,
                                SubPattern = tickets.SubPattern,
                                AdditionalTextPattern = tickets.AdditionalTextPattern,
                                AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
                                MLDebtClassificationId = tickets.MLDebtClassificationId,
                                MLDebtClassificationName = tickets.MLDebtClassificationName,
                                MLResidualFlagId = tickets.MLResidualFlagId,
                                MLResidualFlagName = tickets.MLResidualFlagName,
                                MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                                MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                                MLAccuracy = tickets.MLAccuracy,
                                MLCauseCodeId = tickets.MLCauseCodeId,
                                MLCauseCodeName = tickets.MLCauseCodeName,
                                MLResolutionCodeId = tickets.MLResolutionCodeId,
                                MLResolutionCodeName = tickets.MLResolutionCode,
                                TicketOccurence = tickets.TicketOccurence,
                                AnalystResolutionCodeId = tickets.AnalystResolutionCodeId,
                                AnalystResolutionCodeName = tickets.AnalystResolutionCodeName,
                                AnalystCauseCodeId = tickets.AnalystCauseCodeId,
                                AnalystCauseCodeName = tickets.AnalystCauseCodeName,

                                AnalystDebtClassificationId = tickets.AnalystDebtClassificationId,
                                AnalystDebtClassificationName = tickets.AnalystDebtClassificationName,
                                AnalystAvoidableFlagId = tickets.AnalystAvoidableFlagId,
                                AnalystAvoidableFlagName = tickets.AnalystAvoidableFlagName,
                                SMEComments = tickets.SMEComments,
                                SMEResidualFlagId = tickets.SMEResidualFlagId,
                                SMEResidualFlagName = tickets.SMEResidualFlagName,
                                SMEDebtClassificationId = tickets.SMEDebtClassificationId,
                                SMEDebtClassificationName = tickets.SMEDebtClassificationName,
                                SMEAvoidableFlagId = tickets.SMEAvoidableFlagId,
                                SMEAvoidableFlagName = tickets.SMEAvoidableFlagName,
                                SMECauseCodeId = tickets.SMECauseCodeId,
                                SMECauseCodeName = tickets.SMECauseCodeName,
                                IsApprovedOrMute = tickets.IsApprovedOrMute,
                                LstCauseCodeModel = lstCauseCodeModel,
                                LstAvoidableFlagModel = lstAvoidableFlagModel,
                                LstDebtClassificationModel = lstDebtClassificationModel,
                                LstResidualDebtModel = lstResidualDebtModel,
                                IsApproved = tickets.IsApproved,
                                IsCLSignoff = debtDetails.Count > 0 ? Convert.ToBoolean(debtDetails[0].MLSignoff) : false,
                                IsMLSignoff = tickets.IsMLSignoff,
                                OverriddenPatternTotalCount = tickets.OverridenTotalCount
                            });
                        }
                    }

                    if (debtMLPatternValidationList.NewPatternsModel != null)
                    {
                        foreach (var tickets in debtMLPatternValidationList.NewPatternsModel)
                        {
                            lstDebtMLPatternModel.ExistingPatterns.Add(new DebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id),
                                InitialLearningId = tickets.InitialLearningId,
                                ApplicationId = tickets.ApplicationId,
                                ApplicationName = tickets.ApplicationName,
                                ApplicationTypeId = tickets.ApplicationTypeId,
                                ApplicationTypeName = tickets.ApplicationTypeName,
                                TechnologyId = tickets.TechnologyId,
                                TechnologyName = tickets.TechnologyName,
                                TicketPattern = tickets.TicketPattern,
                                SubPattern = tickets.SubPattern,
                                AdditionalTextPattern = tickets.AdditionalTextPattern,
                                AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
                                MLDebtClassificationId = tickets.MLDebtClassificationId,
                                MLDebtClassificationName = tickets.MLDebtClassificationName,
                                MLResidualFlagId = tickets.MLResidualFlagId,
                                MLResidualFlagName = tickets.MLResidualFlagName,
                                MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                                MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                                MLAccuracy = tickets.MLAccuracy,
                                MLCauseCodeId = tickets.MLCauseCodeId,
                                MLCauseCodeName = tickets.MLCauseCodeName,
                                MLResolutionCodeId = tickets.MLResolutionCodeId,
                                MLResolutionCodeName = tickets.MLResolutionCode,
                                TicketOccurence = tickets.TicketOccurence,
                                AnalystResolutionCodeId = tickets.AnalystResolutionCodeId,
                                AnalystResolutionCodeName = tickets.AnalystResolutionCodeName,
                                AnalystCauseCodeId = tickets.AnalystCauseCodeId,
                                AnalystCauseCodeName = tickets.AnalystCauseCodeName,

                                AnalystDebtClassificationId = tickets.AnalystDebtClassificationId,
                                AnalystDebtClassificationName = tickets.AnalystDebtClassificationName,
                                AnalystAvoidableFlagId = tickets.AnalystAvoidableFlagId,
                                AnalystAvoidableFlagName = tickets.AnalystAvoidableFlagName,
                                SMEComments = tickets.SMEComments,
                                SMEResidualFlagId = tickets.SMEResidualFlagId,
                                SMEResidualFlagName = tickets.SMEResidualFlagName,
                                SMEDebtClassificationId = tickets.SMEDebtClassificationId,
                                SMEDebtClassificationName = tickets.SMEDebtClassificationName,
                                SMEAvoidableFlagId = tickets.SMEAvoidableFlagId,
                                SMEAvoidableFlagName = tickets.SMEAvoidableFlagName,
                                SMECauseCodeId = tickets.SMECauseCodeId,
                                SMECauseCodeName = tickets.SMECauseCodeName,
                                IsApprovedOrMute = tickets.IsApprovedOrMute,
                                LstCauseCodeModel = lstCauseCodeModel,
                                LstAvoidableFlagModel = lstAvoidableFlagModel,
                                LstDebtClassificationModel = lstDebtClassificationModel,
                                LstResidualDebtModel = lstResidualDebtModel,
                                IsApproved = tickets.IsApproved,
                                IsCLSignoff = debtDetails.Count > 0 ? Convert.ToBoolean(debtDetails[0].MLSignoff) : false,
                                IsMLSignoff = tickets.IsMLSignoff,
                                OverriddenPatternTotalCount = tickets.OverridenTotalCount
                            });
                        }
                    }

                    return lstDebtMLPatternModel;
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
        /// GetStartedToInitial
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public ActionResult<UserDetailsBaseModel> GetStartedToInitial(string EmployeeID, int CustomerID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, Convert.ToInt64(CustomerID), null);
            EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
            HiddenFieldsModel objHiddenFieldsModel = new HiddenFieldsModel();
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            try
            {
                if (value)
                {
                    var projectdetails = debtapproval.GetProjectDetailsByEmployeeID(EmployeeID, CustomerID);
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    objListRolePrivilegeModel = objEffortTrackingRepository.
                        GetRolePrivilageMenusForAppLens(EmployeeID, CustomerID);
                    objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(EmployeeID);
                    return new UserDetailsBaseModel
                    {
                        EmployeeName = objHiddenFieldsModel.EmployeeName,
                        HiddenFields = objHiddenFieldsModel,
                        RolePrevilageMenus = objListRolePrivilegeModel,
                        ProjectDetails = projectdetails
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
        /// MLSendDataForML
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="AssociateID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> MLSendDataForML(DebtValidation Param)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, Param.AssociateId, null, Convert.ToInt64(Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    string jobSuccess;
                    jobSuccess = debtapproval.MLDatSetBindingForCSVCreation(Convert.ToInt32(Param.ProjectId), Param.AssociateId, Convert.ToInt32(Param.SupportTypeId));
                    return jobSuccess;
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
        /// MLUpdateInitialLearningState
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AssociateID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<GetMLDetails>> MLUpdateInitialLearningState(int projectID, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, AssociateID, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    var IsMLSentOrReceivedDS = debtapproval.MLUpdateInitialLearningStateDetails(projectID, AssociateID,
                        StartDate, EndDate, OptionalFieldProj, SupportTypeID);
                    return IsMLSentOrReceivedDS;
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
        /// DebtValidateTicketsForML
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="UserID"></param>
        /// <param name="IsSMTicket"></param>
        /// <param name="IsDARTTicket"></param>
        /// <param name="OptFieldProjID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> DebtValidateTicketsForML(DebtValidation Param)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }

            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, Param.UserId, null, Convert.ToInt64(Param.ProjectId));
            string CriteriaMet = string.Empty;
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            ILValidationResult result = new ILValidationResult();
            var debtTicketDetails = debtapproval.GetDebtValidateTicketsForML(Convert.ToInt32(Param.ProjectId),
                Convert.ToDateTime(Param.DateFrom, CultureInfo.CurrentCulture), Convert.ToDateTime(Param.DateTo, CultureInfo.CurrentCulture), Param.UserId,
                Convert.ToInt16(Param.OptFieldProjId, CultureInfo.CurrentCulture), Param.SupportTypeId);

            try
            {
                if (value)
                {
                    if (debtTicketDetails.Count > 0)
                    {

                        CriteriaMet = debtapproval.SaveDebtTicketDetailsAfterProcessing(Convert.ToInt32(Param.ProjectId),
                            debtTicketDetails, Param.UserId, Param.DateFrom, Param.DateTo, Param.SupportTypeId);
                    }
                    else
                    {
                        CriteriaMet = "Not Enough";
                    }
                    result = debtapproval.ValidateML(CriteriaMet, Param.ProjectId, Param.UserId, Convert.ToDateTime(Param.DateFrom, CultureInfo.CurrentCulture),
                    Convert.ToDateTime(Param.DateTo, CultureInfo.CurrentCulture), Convert.ToInt16(Param.OptFieldProjId, CultureInfo.CurrentCulture), Param.SupportTypeId);
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
            }

            return result;
        }

        ///// <summary>
        ///// MLExcelUpload
        ///// </summary>
        ///// <param name="AssociateID"></param>
        ///// <param name="ProjectID"></param>
        ///// <param name="OptfieldProj"></param>
        ///// <returns></returns>


        /// <summary>
        /// CauseCodeResolutionCode
        /// </summary>
        /// <param name="CauseCode"></param>
        /// <param name="TicketDescription"></param>
        /// <param name="ResolutionCode"></param>
        /// <param name="ProjectID"></param>
        /// <param name="Application"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<PopupAttributeModel> CauseCodeResolutionCode(int CauseCode, string TicketDescription, int ResolutionCode,
            string ProjectID, string Application)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            PopupAttributeModel objTicketDetailsPopupModel = new PopupAttributeModel();
            PopupAttributeModel model = new PopupAttributeModel();
            {


                GetDebtAvoidResidual objsp_GetDebtAvoidResidual;
                string isAutoClassified;
                string isDDAutoClassified;
                string dDAutoClassificationDate;


                EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                try
                {
                    if (value)
                    {
                        DataTable autoClassificationDetails = objEffortTrackingRepository.
                            GetAutoClassifiedDetailsForDebt(ProjectID.ToString(CultureInfo.CurrentCulture));
                        isAutoClassified = autoClassificationDetails.Rows[0]["IsAutoClassified"].ToString();
                        isDDAutoClassified = autoClassificationDetails.Rows[0]["IsDDAutoClassified"].ToString();
                        dDAutoClassificationDate = autoClassificationDetails.Rows[0]["DDClassifiedDate"].ToString();

                        if (isAutoClassified == "Y" || (isDDAutoClassified == "Y" && dDAutoClassificationDate == "Y"))
                        {
                            objsp_GetDebtAvoidResidual = objEffortTrackingRepository.
                                CauseCodeResolutionCode(CauseCode, TicketDescription, ResolutionCode, ProjectID, Application
                                , isAutoClassified, isDDAutoClassified);
                            objTicketDetailsPopupModel.DebtClassificationId = Convert.
                                ToString(objsp_GetDebtAvoidResidual.DebtClassification, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.ResidualDebt = Convert.
                                ToString(objsp_GetDebtAvoidResidual.ResidualDebt, CultureInfo.CurrentCulture);
                            objTicketDetailsPopupModel.AvoidableFlag = Convert.
                                ToString(objsp_GetDebtAvoidResidual.AvoidableFlag, CultureInfo.CurrentCulture);
                        }

                        return objTicketDetailsPopupModel;
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


        /// <summary>
        /// ConvertTimebetweenTimeZones
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
            DateTime SrcTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, srcZone);
            DateTime DestTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, destZone);
            TimeSpan SourceTSpan;
            TimeSpan DestTSpan;
            TimeSpan TSpanDiff;
            SourceTSpan = srcZone.GetUtcOffset(timeUtc);
            DestTSpan = destZone.GetUtcOffset(timeUtc);
            DateTime ConvertedDate;
            TSpanDiff = DestTSpan - SourceTSpan;
            ConvertedDate = dt.Add(TSpanDiff);
            return ConvertedDate;

        }

        /// <summary>
        /// GetCurrentTimeofTimeZones
        /// </summary>
        /// <param name="UserTimeZone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCurrentTimeofTimeZones/{UserTimeZone}")]
        public DateTime GetCurrentTimeofTimeZones(string UserTimeZone)
        {
            DateTime timeUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(UserTimeZone);
            DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, zone);
            return currentDateTime;
        }

        /// <summary>
        /// GetWeekOfMonth
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
            {
                date = date.AddDays(1);
            }

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }


        /// <summary>
        /// WeeklyEffort
        /// </summary>
        public enum WeeklyEffort
        {
            PartialEffort = 3, //#926526
            FullEffort = 2, //#1d4465
            NoEffort = 1, //#366936
            Holiday = 0
        }

        /// <summary>
        /// ToDataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// GetTicketInfoDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TicketID"></param>
        /// <returns></returns>
        [Route("GetTicketInfoDetails")]
        [HttpPost]
        public ActionResult<string> GetTicketInfoDetails(BaseInformationModel baseInformationModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(baseInformationModel.ProjectId));
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    var isTicketIDPresent = objEffortTrackingRepository.GetTicketInfoDetails(Convert.ToInt64(baseInformationModel.ProjectId, CultureInfo.CurrentCulture),
                        baseInformationModel.TicketId,
                        baseInformationModel.SupportTypeId);
                    return isTicketIDPresent;
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
        /// GetChartDatasourceLatent
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ChartDatasourceLatent GetChartDatasourceLatent(DataTable dt)
        {
            try
            {
                ChartDatasourceLatent datasource = new ChartDatasourceLatent();


                datasource.Category = new List<ChartCategoryDataset>();
                datasource.Datasets = new List<ChartDataset>();

                if (dt != null)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 0)
                        {
                            ChartDataset dataset = new ChartDataset();
                            dataset.Seriesname = dt.Columns[i].Caption;
                            dataset.Data = new List<ChartDetailsModel>();

                            foreach (DataRow row in dt.Rows)
                            {
                                dataset.Data.Add(new ChartDetailsModel
                                {
                                    Label = row[i].ToString(),
                                    Value = row[i].ToString()
                                });

                            }

                            datasource.Datasets.Add(dataset);
                        }
                        else
                        {
                            ChartCategoryDataset dataset = new ChartCategoryDataset();
                            dataset.Category = new List<ChartCategorieslabel>();
                            foreach (DataRow row in dt.Rows)
                            {
                                dataset.Category.Add(new ChartCategorieslabel { Label = row[i].ToString() });
                                ChartCategories category = new ChartCategories();
                                category.Label = row[i].ToString();


                            }
                            datasource.Category.Add(dataset);

                        }
                    }

                }
                return datasource;
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
        /// GetProjectBasedTicketStatusDetail
        /// </summary>
        /// <param name="CloseDateBegin"></param>
        /// <param name="CloseDateEnd"></param>
        /// <param name="ProjectID"></param>
        /// <param name="TicketID_Desc"></param>
        /// <param name="AssignedTo"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="CreateDateBegin"></param>
        /// <param name="CreateDateEnd"></param>
        /// <param name="StatusID"></param>
        /// <param name="IsDARTTicket"></param>
        /// <param name="AssigneeID"></param>
        /// <param name="ProjectTimeZoneName"></param>
        /// <param name="UserTimeZoneName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChooseTicketDetails")]
        public ActionResult<ChooseTicketBaseModel> ChooseTicketDetails(ChooseTicket objChooseTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(objChooseTicket.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    TicketingModuleController objTicketingModuleController = new TicketingModuleController(_configuration, _httpContextAccessor, _hostingEnvironment);
                    if (objChooseTicket.WorkItemFlag == 'T')
                    {
                        List<ChooseSearchTicketDetailsModel> timesheetList;
                        timesheetList = objEffortTrackingRepository.GetSearchTicketsForET(objChooseTicket);
                        if (string.IsNullOrEmpty(objChooseTicket.UserTimeZoneName))
                        {
                            objChooseTicket.UserTimeZoneName = objChooseTicket.ProjectTimeZoneName;
                        }

                        foreach (var item in timesheetList)
                        {
                            item.OpenDateTime = ConvertTimebetweenTimeZones(Convert.ToDateTime(item.OpenDateTime, CultureInfo.CurrentCulture),
                                objChooseTicket.ProjectTimeZoneName, objChooseTicket.UserTimeZoneName).ToString(CultureInfo.CurrentCulture);
                            if (item.Closeddate != null)
                            {
                                item.Closeddate = ConvertTimebetweenTimeZones(Convert.ToDateTime(item.Closeddate, CultureInfo.CurrentCulture),
                                    objChooseTicket.ProjectTimeZoneName, objChooseTicket.UserTimeZoneName).ToString(CultureInfo.CurrentCulture);
                            }
                        }

                        return new ChooseTicketBaseModel { ReturnType = "T", TimeSheetList = timesheetList };
                    }
                    else
                    {
                        List<WorkItemDetails> workitemList = new List<WorkItemDetails>();
                        ChooseWorkItem objChooseWorkItem = objEffortTrackingRepository.ChooseWorkItems(objChooseTicket);
                        return new ChooseTicketBaseModel { ReturnType = "C", ChooseWorkItemDetail = objChooseWorkItem };
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
        /// SaveNoiseEliminationDetails
        /// </summary>
        /// <param name="lstNoiseTicketDescription"></param>
        /// <param name="lstNoiseResolution"></param>
        /// <param name="Projectid"></param>
        /// <param name="Choose"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> SaveNoiseEliminationDetails(NoiseEliminationDetails noiseDetails)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(noiseDetails.Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    ILValidationResult objILValidationResult;
                    NoiseElimination noiseData = new NoiseElimination();
                    SanitizeStringInput SanEmployeeId = Convert.ToString(_httpContextAccessor.HttpContext.Session.GetString("UserId"), CultureInfo.CurrentCulture);
                    noiseData.LstNoiseTicketDescription = noiseDetails.NoiseTicketDescriptions;
                    noiseData.LstNoiseResolution = noiseDetails.NoiseResolutions;
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    string CriteriaMet = objDebtFieldsApprovalRepository.SaveNoiseEliminationDetails(noiseData,
                        noiseDetails.Param.ProjectId, noiseDetails.Param.Choose, SanEmployeeId.Value);

                    objILValidationResult = objDebtFieldsApprovalRepository.ValidateML(CriteriaMet, noiseDetails.Param.ProjectId, SanEmployeeId.Value,
                       Convert.ToDateTime(noiseDetails.Param.DateFrom, CultureInfo.CurrentCulture),
                           Convert.ToDateTime(noiseDetails.Param.DateTo, CultureInfo.CurrentCulture),
                           Convert.ToInt16(noiseDetails.Param.OptFieldProjId, CultureInfo.CurrentCulture), noiseDetails.Param.SupportTypeId);
                    return objILValidationResult;
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
        /// GetIconDetails
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ActionResult<IconDetails> GetIconDetails(string Choose, int ProjectId, int SupportID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectId));
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            IconDetails model;
            try
            {
                if (value)
                {
                    model = objDebtFieldsApprovalRepository.GetIconDetails(Choose, ProjectId, SupportID);
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
        /// CreateInitialLearningID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContentResult> CreateInitialLearningID(int ProjectID, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, Convert.ToInt64(ProjectID));
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            string result = string.Empty;
            try
            {
                if (value)
                {
                    result = objDebtFieldsApprovalRepository.CreateInitialLearningID(ProjectID, EmployeeID);

                    return Content(result);
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