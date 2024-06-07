using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContinuousLearningController : BaseController
    {
        // GET: ContinuousLearning
        private readonly ContinuousLearningRepository objcontLearningRepository;

        /// <summary>
        /// ContinuousLearningController
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="_httpContextAccessor"></param>
        public ContinuousLearningController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor) : base(configuration, _httpContextAccessor)
        {
            objcontLearningRepository = new ContinuousLearningRepository();
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ContinuousLearningBaseModel> Index(string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID.ToString(), null, null);
            ContinuousLearningBaseModel continuousLearningBaseModel = new ContinuousLearningBaseModel();
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    HiddenFieldsModel objHiddenFieldsModel;
                    List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();

                    string cognizantID;
                    if (string.IsNullOrEmpty(EmployeeID) || EmployeeID == "undefined")
                    {
                        cognizantID = CognizantID;
                    }
                    else
                    {
                        EmployeeID = HttpUtility.HtmlEncode(EmployeeID);
                        cognizantID = EmployeeID;
                    }

                    objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(cognizantID);
                    objHiddenFieldsModel.EmployeeId = cognizantID;
                    continuousLearningBaseModel.HiddenFields = objHiddenFieldsModel;
                    continuousLearningBaseModel.RolePrevilageMenus = objListRolePrivilegeModel;
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

            return continuousLearningBaseModel;
        }

        /// <summary>
        /// CheckIfAllPatternsAreSignedOff
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ActionResult<bool> CheckIfAllPatternsAreSignedOff(long ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            bool returnValue = false;
            try
            {
                if (value)
                {
                    returnValue = objcontLearningRepository.CheckIfAllPatternsAreSignedOff(ProjectID);
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

            return returnValue;
        }


        /// <summary>
        /// SaveCLValues
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [HttpPost]
        public int SaveCLValues(CLPatternsSignOff pattern)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            int result = 0;
            try
            {
                result = objcontLearningRepository.SaveCLPatterns(pattern);
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

        /// <summary>
        /// Method to get Continuous Learning Enrichment Date
        /// </summary>
        /// <param name="ProjectId">Project ID</param>
        /// <returns>CL Enrichment dates</returns>
        [HttpPost]
        public ActionResult<LearningEnrichmentDate> GetLearningEnrichmentDates(int ProjectId)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectId));
            LearningEnrichmentDate objLearningEnrichmentDate = new LearningEnrichmentDate();
            try
            {
                if (value)
                {
                    objLearningEnrichmentDate = objcontLearningRepository.GetLearningEnrichmentDates(ProjectId);
                    return objLearningEnrichmentDate;
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
        /// ExportGridToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        [HttpPost]
        public string ExportGridToExcel(List<CLExcel> excel)
        {
            string path = string.Empty;
            try
            {

                path = objcontLearningRepository.ExportToExcel(excel);

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

            return path;
        }

        /// <summary>
        /// DownloadExcel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        //[DeleteFileAttribute] 
        [Route("DownloadExcel")]
        public FileResult DownloadExcel(ExcelDownload file)
        {
            string fullPath = string.Empty;
            try
            {
                fullPath = new ApplicationConstants().DownloadExcelTemp + file.FileName.Replace("..", string.Empty);
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

            string dirctoryName = System.IO.Path.GetDirectoryName(fullPath);
            string fName = System.IO.Path.GetFileNameWithoutExtension(fullPath);
            string validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
            validatePath = RemoveLastIndexCharacter(validatePath);

            if (System.IO.File.Exists(validatePath))
            {
                byte[] fileBook = System.IO.File.ReadAllBytes(validatePath);

                var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel") { };
                return fileContentResult;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
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
        /// <summary>
        /// GetDropDownValuesApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="portfolioID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<Application>> GetDropDownValuesApplication(long projectID, long portfolioID, long CustomerID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, Convert.ToInt64(CustomerID), Convert.ToInt64(projectID));
            List<Application> application = new List<Application>();
            try
            {
                if (value)
                {
                    application = objcontLearningRepository.GetDropDownValuesApplication(projectID, portfolioID,
                        CustomerID);
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
        /// CheckCognizantCustomer
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<CustomerCognizant> CheckCognizantCustomer(string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, null);
            CustomerCognizant custCog = new CustomerCognizant();
            try
            {
                if (value)
                {
                    custCog = objcontLearningRepository.CheckCognizantCustomer(EmployeeID);
                    return custCog;
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
        /// SaveConfigValues
        /// </summary>
        /// <param name="clconfig"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> SaveConfigValues(CLConfig clconfig)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            string result = "";
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, clconfig.UserId.ToString(), null, Convert.ToInt64(clconfig.ProjectId));
            try
            {
                if (value)
                {
                    result = objcontLearningRepository.SaveConfigValues(clconfig);
                    return result;

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
        /// ToShowCLConfig
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<CLConfig> ToShowCLConfig(int ProjectId, string UserID)
        {
            CLConfig toShowCLConfig = new CLConfig();
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectId));
            try
            {
                if (value)
                {
                    toShowCLConfig = objcontLearningRepository.ToShowCLConfig(ProjectId, UserID);
                    return toShowCLConfig;
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
        /// GetDropDownValuesProjectPortfolio
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="isPortfolio"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> GetDropDownValuesProjectPortfolio(string employeeID, long CustomerID, long ProjectID,
            int isPortfolio)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, employeeID.ToString(), Convert.ToInt64(CustomerID), Convert.ToInt64(ProjectID));
            ContinuousLearningList cLList = new ContinuousLearningList();
            try
            {
                if (value)
                {
                    cLList = objcontLearningRepository.GetDropDownValuesProjectPortfolio(employeeID, CustomerID,
                        ProjectID, isPortfolio);
                    return JsonConvert.SerializeObject(cLList);
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
        /// DebtMLPatternValidationGridContinuous
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="AppID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DebtMLPatternValidationResult> DebtMLPatternValidationGridContinuous(int ProjectId, int SupportType)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectId));
            DebtMLPatternValidationResult debtMLPatternValidationResult = new DebtMLPatternValidationResult();
            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
            ContinuousLearningRepository contLearningRepository = new ContinuousLearningRepository();
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel = new List<DebtMLPatternValidationModel>();
            List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstCauseCodeModel;
            List<DebtMasterValues> lstDebtClassificationModel;
            List<DebtMasterValues> lstAvoidableFlagModel;
            List<DebtMasterValues> lstResidualDebtModel;
            try
            {
                if (value)
                {
                    var debtMLPatterndetails = contLearningRepository.GetDebtMLPatternValidationReportContinuous(ProjectId);
                    var debtMasterdetails = debtApproval.GetDebtMasterValues(ProjectId, SupportType);

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
                    debtMLPatternValidationResult.JobStatusMessage = debtMLPatterndetails.JobStatusMessage;
                    if (debtMLPatterndetails.DebtMLPatternValidationModel != null)
                    {
                        foreach (var tickets in debtMLPatterndetails.DebtMLPatternValidationModel)
                        {
                            lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id),
                                ContLearningId = tickets.ContLearningId,
                                ApplicationId = tickets.ApplicationId,
                                ApplicationName = tickets.ApplicationName,
                                ApplicationTypeId = tickets.ApplicationTypeId,
                                ApplicationTypeName = tickets.ApplicationTypeName,
                                TechnologyId = tickets.TechnologyId,
                                TechnologyName = tickets.TechnologyName,
                                TicketPattern = tickets.TicketPattern,
                                MLDebtClassificationId = tickets.MLDebtClassificationId,
                                MLDebtClassificationName = tickets.MLDebtClassificationName,
                                MLResidualFlagId = tickets.MLResidualFlagId,
                                MLResidualFlagName = tickets.MLResidualFlagName,
                                MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                                MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                                MLAccuracy = tickets.MLAccuracy,
                                MLCauseCodeId = tickets.MLCauseCodeId,
                                MLCauseCodeName = tickets.MLCauseCodeName,
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
                                MLResolutionCode = tickets.MLResolutionCode,
                                MLResolutionCodeId = tickets.MLResolutionCodeId,
                                PatternsOrigin = tickets.PatternsOrigin,
                                IsDefaultRuleSelected = tickets.IsDefaultRuleSelected,
                                IsApprovedPatternsConflict = tickets.IsApprovedPatternsConflict,
                                SubPattern = tickets.SubPattern,
                                AdditionalTextPattern = tickets.AdditionalTextPattern,
                                AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
                                ApprovedFlag = tickets.ApprovedFlag,
                                JobStatusMessage = debtMLPatterndetails.JobStatusMessage,
                                JobFromDate = debtMLPatterndetails.JobFromDate,
                                JobToDate = debtMLPatterndetails.JobToDate
                            });
                        }
                    }

                    debtMLPatternValidationResult.DebtMLPatternValidationModels = lstDebtMLPatternModel;
                    return debtMLPatternValidationResult;
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
        /// DebtMLGetPatternOccurence
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="AppID"></param>
        /// <param name="ApplicationName"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="CauseCodeId"></param>
        /// <param name="ResolutionCodeId"></param>
        /// <param name="AvoidableFlagId"></param>
        /// <param name="DebtClassificationId"></param>
        /// <param name="ResidualFlagId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DebtMLPatternValidationResult> DebtMLGetPatternOccurence(int ProjectID, int PatternApplicationID,
            int CauseCodeId, int ResolutionCodeId, string TicketPattern, string TicketSubPattern,
            string AddiPattern, string AddiSubPattern, int AvoidableFlagID, int ResidualFlagID,
            int DebtClassificationID, int SupportType, Int32 OldDebtID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            DebtMLPatternValidationResult debtMLPatternValidationResult = new DebtMLPatternValidationResult();
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel = new List<DebtMLPatternValidationModel>();
            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
            ContinuousLearningRepository continuousRepository = new ContinuousLearningRepository();
            List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
            List<DebtMasterValues> lstCauseCodeModel;
            List<DebtMasterValues> lstDebtClassificationModel;
            List<DebtMasterValues> lstAvoidableFlagModel;
            List<DebtMasterValues> lstResidualDebtModel;
            try
            {
                if (value)
                {
                    var debtMLPatternDetails = continuousRepository.GetDebtMLPatternOccurenceReportContinuous(ProjectID,
                        PatternApplicationID, CauseCodeId, ResolutionCodeId, TicketPattern.Trim(), TicketSubPattern.Trim(),
                        AddiPattern.Trim(), AddiSubPattern.Trim());
                    var DebtMasterdetails = debtApproval.GetDebtMasterValues(ProjectID, SupportType);

                    debtMLPatternValidationResult.AvoidableFlagId = AvoidableFlagID;
                    debtMLPatternValidationResult.DebtClassificationId = DebtClassificationID;
                    debtMLPatternValidationResult.ResidualCodeId = ResidualFlagID;
                    debtMLPatternValidationResult.OldDebtId = OldDebtID;

                    foreach (var masterValues in DebtMasterdetails)
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

                    if (debtMLPatternDetails.Count > 0)
                    {
                        foreach (var tickets in debtMLPatternDetails)
                        {
                            lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id),
                                TicketPattern = tickets.TicketPattern,
                                TicketOccurence = tickets.TicketOccurence,
                                MLDebtClassificationId = tickets.MLDebtClassificationId,
                                MLDebtClassificationName = tickets.MLDebtClassificationName,
                                MLResidualFlagId = tickets.MLResidualFlagId,
                                MLResidualFlagName = tickets.MLResidualFlagName,
                                MLAvoidableFlagId = tickets.MLAvoidableFlagId,
                                MLAvoidableFlagName = tickets.MLAvoidableFlagName,
                                SMECauseCodeId = tickets.SMECauseCodeId,
                                SMECauseCodeName = tickets.SMECauseCodeName,
                                MLCauseCodeId = tickets.MLCauseCodeId,
                                MLCauseCodeName = tickets.MLCauseCodeName,
                                MLResolutionCodeId = tickets.MLResolutionCodeId,
                                MLResolutionCodeName = tickets.MLResolutionCode,
                                LstCauseCodeModel = lstCauseCodeModel,
                                LstAvoidableFlagModel = lstAvoidableFlagModel,
                                LstDebtClassificationModel = lstDebtClassificationModel,
                                LstResidualDebtModel = lstResidualDebtModel,
                                ApplicationName = tickets.ApplicationName,
                                ApplicationId = tickets.ApplicationId,
                                SubPattern = tickets.SubPattern,
                                AdditionalTextPattern = tickets.AdditionalTextPattern,
                                AdditionalTextsubPattern = tickets.AdditionalTextsubPattern,
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
                            TicketOccurence = 0,
                            MLDebtClassificationId = 0,
                            MLDebtClassificationName = string.Empty,
                            MLResidualFlagId = 0,
                            MLResidualFlagName = string.Empty,
                            MLAvoidableFlagId = 0,
                            MLAvoidableFlagName = string.Empty,
                            SMECauseCodeId = CauseCodeId,
                            SMECauseCodeName = string.Empty,
                            LstCauseCodeModel = lstCauseCodeModel,
                            LstAvoidableFlagModel = lstAvoidableFlagModel,
                            LstDebtClassificationModel = lstDebtClassificationModel,
                            LstResidualDebtModel = lstResidualDebtModel,
                            MLResolutionCodeId = ResolutionCodeId,
                            ApplicationId = Convert.ToInt32(PatternApplicationID),
                            SubPattern = TicketSubPattern,
                            AdditionalTextPattern = AddiPattern,
                            AdditionalTextsubPattern = AddiSubPattern,
                            MLCauseCodeId = 0
                        });
                    }

                    debtMLPatternValidationResult.DebtMLPatternValidationModels = lstDebtMLPatternModel;
                    return debtMLPatternValidationResult;
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
        /// VerifyJobStatus
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ActionResult<VerifiedJobStatusResult> VerifyJobStatus(int ProjectId)
        {
            bool valid = false;
            string date = string.Empty;
            bool isCLEnabled = false;
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectId));
            try
            {
                if (value)
                {
                    JobDetails jobdetails = objcontLearningRepository.VerifyJobStatus(ProjectId);
                    if (jobdetails.IsCLEnabled == "1")
                    {
                        if (!string.IsNullOrEmpty(jobdetails.JobStatus) && Convert.ToBoolean(jobdetails.JobStatus, CultureInfo.CurrentCulture))
                        {
                            valid = true;
                            date = Convert.ToDateTime(jobdetails.JobDate, CultureInfo.CurrentCulture).ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                            isCLEnabled = true;

                        }
                        else
                        {
                            valid = false;
                            date = Convert.ToDateTime(jobdetails.JobDate, CultureInfo.CurrentCulture).ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
                            isCLEnabled = true;
                        }
                    }
                    else
                    {
                        valid = false;
                        date = null;
                        isCLEnabled = false;
                    }

                    return new VerifiedJobStatusResult { Status = valid, Date = date, IsCLEnabled = isCLEnabled };
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
        /// GetCLDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<CLDetails>> GetCLDetails(int projectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    ContinuousLearningRepository debtapproval = new ContinuousLearningRepository();
                    var debtdetails = debtapproval.GetCLDetails(projectID);
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
    }
}
