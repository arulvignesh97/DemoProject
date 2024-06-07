using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
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
using Utils = CTS.Applens.WorkProfiler.DAL.Utility;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Initial Leaning - Sprint 12.0 
    /// Re-Written the IL module in order to incorperate both App & Infra Module
    /// </summary>
    public class InitialLearningController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        CacheManager _cacheManager = new CacheManager();
        /// <summary>
        /// EffortTrackingController 
        /// </summary>
        public InitialLearningController(IConfiguration configuration,
            IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnvironment) :
            base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// Main Method for rendering the Initial Learning page
        /// This will be called from the _TopNavigationBar page
        /// </summary>
        /// <param name="EmployeeID">Logged - In User id</param>
        /// <param name="CustomerID">Selected Customer ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<UserDetailsBaseModel> Index(string EmployeeID, Int32 CustomerID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, Convert.ToInt64(CustomerID), null);
            try
            {
                if (value)
                {
                    EffortTrackingRepository objEffortTrackingRepository = new EffortTrackingRepository();
                    HiddenFieldsModel objHiddenFieldsModel;
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    var projectdetails = debtapproval.GetProjectDetailsByEmployeeID(EmployeeID, CustomerID);
                    List<RolePrivilegeModel> objListRolePrivilegeModel;
                    if (_cacheManager.IsExists("objListRolePrivilegeModel" + EmployeeID))
                    {
                        objListRolePrivilegeModel = _cacheManager.GetOrCreate<List<RolePrivilegeModel>>("objListRolePrivilegeModel" + EmployeeID, () => new List<RolePrivilegeModel>(), CacheDuration.Long);
                    }
                    else
                    {
                        objListRolePrivilegeModel = objEffortTrackingRepository.
                       GetRolePrivilageMenusForAppLens(EmployeeID, CustomerID);
                    }

                    if (_cacheManager.IsExists("objHiddenFieldsModel" + EmployeeID))
                    {
                        objHiddenFieldsModel = _cacheManager.GetOrCreate<HiddenFieldsModel>("objHiddenFieldsModel" + EmployeeID, () => new HiddenFieldsModel(), CacheDuration.Long);
                    }
                    else
                    {
                        objHiddenFieldsModel = objEffortTrackingRepository.GetHiddenFields(EmployeeID);
                    }

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
        /// Method o get the Top Filter Section Data
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ValidationResultBaseModel> GetTopFilterSection(Int32 ProjectID, int SupportTypeID, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    ILValidationResult objILValidationResult = new ILValidationResult();
                    InitialLearningRepository objInitialLearningRepository = new InitialLearningRepository();
                    MLDetails objMLDetails = objInitialLearningRepository.GetTopFiltersOnLoad(ProjectID, SupportTypeID);
                    objMLDetails.StartDate = objMLDetails.StartDate != "" ? Convert.ToString
                        (Convert.ToDateTime(objMLDetails.StartDate, CultureInfo.CurrentCulture).ToShortDateString(), CultureInfo.CurrentCulture) : Convert.ToString
                        ((DateTime.Today.AddMonths(-6)).ToShortDateString(), CultureInfo.CurrentCulture);
                    var EndDate = Convert.ToString(Convert.ToDateTime((DateTime.Today.AddDays(-1)).ToShortDateString(), CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                    objMLDetails.EndDate = objMLDetails.EndDate != "" ? Convert.ToString(Convert.ToDateTime
                        (objMLDetails.EndDate, CultureInfo.CurrentCulture).ToShortDateString(), CultureInfo.CurrentCulture) : Convert.ToString(
                            (DateTime.Today.AddDays(-1)).ToShortDateString(), CultureInfo.CurrentCulture);

                    if (objMLDetails.IsAutoClassified == "N")
                    {
                        objILValidationResult.ILValidationResultCode = 14;
                        return new ValidationResultBaseModel { MessageValue = "MLMessage", ILValidationResults = objILValidationResult };
                    }
                    else
                    {
                        objMLDetails.Result = GetProgressStatusCode(ProjectID, SupportTypeID, EmployeeID);
                        return new ValidationResultBaseModel { MessageValue = "MLTopFilters", MLDetail = objMLDetails };
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
        /// Method to get the Status Code
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        public ILValidationResult GetProgressStatusCode(Int32 ProjectID, int SupportTypeID, string EmployeeID)
        {
            // bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, Convert.ToInt64(ProjectID));
            ILValidationResult objILValidationResult;
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            InitialLearningRepository objInitialLearningRepository = new InitialLearningRepository();
            try
            {
                //if (value)
                //{
                //Check if there is any Processing Required
                var ProcessingDetails = objDebtFieldsApprovalRepository.GetProcessingRequiredOnLoad
                    (ProjectID, SupportTypeID);
                if (string.Compare(ProcessingDetails.IsMLSent, ApplicationConstants.ConstantY) == 0)
                {
                    objDebtFieldsApprovalRepository.CheckIfMLFileGenerated(ProjectID, ProcessingDetails.MLJobId,
                        InitialLearningConstants.JobTypeML, EmployeeID, SupportTypeID);
                }
                else if (string.Compare(ProcessingDetails.IsSamplingSent, ApplicationConstants.ConstantY) == 0)
                {
                    objDebtFieldsApprovalRepository.CheckIfMLFileGenerated(ProjectID, ProcessingDetails.SamplingJobId,
                        ApplicationConstants.Sampling_Flag, EmployeeID, SupportTypeID);
                }
                else if (string.Compare(ProcessingDetails.IsMLProcessingRequired, ApplicationConstants.ConstantY) == 0)
                {
                    objDebtFieldsApprovalRepository.UpdateMLPatternFromCSV(ProjectID, ProcessingDetails.MLJobId,
                        EmployeeID);
                }
                else if (string.Compare(ProcessingDetails.IsSamplingProcessingRequired,
                    ApplicationConstants.ConstantY) == 0)
                {
                    if (SupportTypeID == 1)
                    {
                        objDebtFieldsApprovalRepository.UpdateSampledTicketsFromCSV(ProjectID,
                            ProcessingDetails.SamplingJobId, EmployeeID);
                    }
                    else
                    {
                        objInitialLearningRepository.UpdateInfraSampledTicketsFromCSV(ProjectID,
                           ProcessingDetails.SamplingJobId, EmployeeID);
                    }
                }
                else if (string.Compare(ProcessingDetails.IsNoiseEliminationSent, ApplicationConstants.ConstantY) == 0)
                {
                    if (SupportTypeID == 1)
                    {
                        NoiseElimination noiseEliminationDetails;
                        noiseEliminationDetails = objDebtFieldsApprovalRepository.CheckIfNoiseOutputFileGenerated(ProjectID,
                                ProcessingDetails.NoiseEliminationJobId, SupportTypeID);
                        if (noiseEliminationDetails != null && noiseEliminationDetails.LstNoiseTicketDescription.Count > 0)
                        {
                            objDebtFieldsApprovalRepository.SaveNoiseEliminationDetails(noiseEliminationDetails, ProjectID,
                                1, "System");
                        }
                    }
                    else
                    {
                        NoiseEliminationInfra noiseInfraEliminationDetails;
                        noiseInfraEliminationDetails = objDebtFieldsApprovalRepository.
                            CheckIfNoiseOutputFileGeneratedInfra(ProjectID, ProcessingDetails.NoiseEliminationJobId,
                            SupportTypeID);
                        if (noiseInfraEliminationDetails != null
                                                && noiseInfraEliminationDetails.LstNoiseTicketDescriptionInfra.Count > 0)
                        {
                            objDebtFieldsApprovalRepository.SaveNoiseEliminationInfraDetails(
                                                        noiseInfraEliminationDetails, ProjectID, 1, "System");

                        }
                    }
                }
                else
                {
                    //mandatory else
                }
                List<GetMLDetails> lstMLDetails =
                    objDebtFieldsApprovalRepository.GetMLDetailsOnLoad(ProjectID, SupportTypeID);
                objILValidationResult = objDebtFieldsApprovalRepository.MLDetailsLoad(lstMLDetails);
                return objILValidationResult;
                //}
                //return Unauthorized();
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
        /// Method to LOad the partial view by state of the project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<PattenValidation> LoadPartialViewByState(Int32 ProjectID, int SupportTypeID, int InitialLearningStateID,
            ILValidationResult objILValidationResult, string EmployeeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, EmployeeID, null, Convert.ToInt64(ProjectID));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    if (InitialLearningStateID == 4)
                    {
                        return new PattenValidation { MessageValue = "MLDownloadUpload" };
                    }
                    if (InitialLearningStateID == 8)
                    {
                        if (SupportTypeID == 2)
                        {
                            List<MLPatternValidationInfra> debtMLPatterndetails =
                                                    objDebtFieldsApprovalRepository.GetMLpatternValidationReport(
                                                        ProjectID, EmployeeID);
                            return new PattenValidation
                            {
                                ShowAdditionalTextPattern = debtMLPatterndetails.
                                                                 Where(x => x.AdditionalTextPattern.Trim() != string.Empty
                                                                 && x.AdditionalTextPattern.Trim() != "0").
                                                                 Select(x => x).Count() > 0 ? true : false,
                                ShowSubPattern = debtMLPatterndetails.
                                                            Where(x => x.SubPattern.Trim() != string.Empty
                                                            && x.SubPattern.Trim() != "0").Select(x => x).Count() > 0 ?
                                                            true : false,
                                ShowAdditionalTextSubPattern = debtMLPatterndetails.
                                                         Where(x => x.AdditionalTextsubPattern.Trim() != string.Empty
                                                         && x.AdditionalTextsubPattern.Trim() != "0").
                                                         Select(x => x).Count() > 0 ? true : false,
                                MessageValue = "InfraMLPatternValidation",
                                MLPatternDetails = debtMLPatterndetails
                            };
                        }
                        else
                        {

                            DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
                            List<DebtMLPatternValidationModel> lstDebtMLPatternModel =
                                new List<DebtMLPatternValidationModel>();
                            List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
                            List<DebtMasterValues> lstCauseCodeModel = new List<DebtMasterValues>();
                            List<DebtMasterValues> lstResolutionCodeModel = new List<DebtMasterValues>();
                            List<DebtMasterValues> lstDebtClassificationModel = new List<DebtMasterValues>();
                            List<DebtMasterValues> lstAvoidableFlagModel = new List<DebtMasterValues>();
                            List<DebtMasterValues> lstResidualDebtModel = new List<DebtMasterValues>();
                            var debtdetails = debtApproval.GetMLDetailsOnLoad(ProjectID, SupportTypeID);
                            var debtMLPatterndetails = debtApproval.GetDebtMLPatternValidationReport(ProjectID, EmployeeID);


                            foreach (var tickets in debtMLPatterndetails)
                            {
                                lstDebtMLPatternModel.Add(new DebtMLPatternValidationModel
                                {
                                    Id = Convert.ToInt32(tickets.Id, CultureInfo.CurrentCulture),
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
                                    IsRegMLSignoff = debtdetails.Count > 0 ?
                                    Convert.ToBoolean(debtdetails[0].IsRegMLsignOff, CultureInfo.CurrentCulture) : false,
                                    OverriddenPatternTotalCount = tickets.OverridenTotalCount,
                                    IsRegenerated = tickets.IsRegenerated
                                });
                            }
                            return new PattenValidation
                            {
                                ShowAdditionalTextPattern = lstDebtMLPatternModel.
                                                                  Where(x => x.AdditionalTextPattern.Trim() != string.Empty
                                                                  && x.AdditionalTextPattern.Trim() != "0").
                                                                  Select(x => x).Count() > 0 ? true : false,
                                ShowSubPattern = lstDebtMLPatternModel.
                                                                Where(x => x.SubPattern.Trim() != string.Empty
                                                                && x.SubPattern.Trim() != "0").
                                                                Select(x => x).Count() > 0 ? true : false,
                                ShowAdditionalTextSubPattern = lstDebtMLPatternModel.
                                                             Where(x => x.AdditionalTextsubPattern.Trim() != string.Empty
                                                             && x.AdditionalTextsubPattern.Trim() != "0").
                                                             Select(x => x).Count() > 0 ? true : false,
                                MessageValue = "DebtMLPatternValidation",
                                DebtMLPatternDetails = lstDebtMLPatternModel
                            };
                        }
                    }
                    else if (InitialLearningStateID == 9)
                    {
                        if (SupportTypeID == 2)
                        {
                            NoiseEliminationInfra model;
                            var processingDetails = objDebtFieldsApprovalRepository.GetProcessingRequiredOnLoad(
                                Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), SupportTypeID);
                            string noiseEliminationJobId = processingDetails.NoiseEliminationJobId;
                            model = objDebtFieldsApprovalRepository.GetNoiseEliminationInfraData
                                            (Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), noiseEliminationJobId);
                            if (model.LstNoiseTicketDescriptionInfra != null &&
                                model.LstNoiseTicketDescriptionInfra.Count > 0)
                            {
                                return new PattenValidation { MessageValue = "NoiseEliminationInfra", NoiseEliminationInfras = model };
                            }
                            else
                            {
                                return new PattenValidation { MessageValue = "MLMessage", ILValidationResults = objILValidationResult };
                            }
                        }
                        else
                        {
                            NoiseElimination model;
                            var processingDetails = objDebtFieldsApprovalRepository.
                                GetProcessingRequiredOnLoad(Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), SupportTypeID);

                            string noiseEliminationJobId = processingDetails.NoiseEliminationJobId;
                            model = objDebtFieldsApprovalRepository.
                                GetNoiseEliminationData(Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), noiseEliminationJobId,
                                SupportTypeID);
                            if (model.LstNoiseTicketDescription != null
                                && model.LstNoiseTicketDescription.Count > 0)
                            {
                                return new PattenValidation { MessageValue = "NoiseEliminationData", NoiseEliminations = model };
                            }
                            else
                            {
                                return new PattenValidation { MessageValue = "MLMessage", ILValidationResults = objILValidationResult };
                            }
                        }
                    }
                    else if (InitialLearningStateID == 10)
                    {
                        string Selection = "Top";
                        int Filter = 0;
                        if (SupportTypeID == 1)
                        {
                            NoiseElimination filteredmodel;
                            filteredmodel = objDebtFieldsApprovalRepository.GetFilteredNoiseEliminationData(Selection,
                                Filter, ProjectID);
                            return new PattenValidation { MessageValue = "NoiseEliminationData", NoiseEliminations = filteredmodel };
                        }
                        else
                        {
                            NoiseEliminationInfra filteredmodel;
                            filteredmodel = objDebtFieldsApprovalRepository.GetFilteredNoiseEliminationDataInfra(Selection,
                                Filter, ProjectID);
                            return new PattenValidation { MessageValue = "NoiseEliminationInfra", NoiseEliminationInfras = filteredmodel };
                        }
                    }

                    else if (InitialLearningStateID == 12)
                    {
                        //Call the Sampling Grid Here
                        var debtdetails = new List<DebtSamplingModel>();
                        InitialLearningRepository initialapproval = new InitialLearningRepository();
                        DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                        List<DebtSamplingGetModel> lstApprovalModel = new List<DebtSamplingGetModel>();
                        List<DebtSamplingValues> lstDebtMasterValuesModel = new List<DebtSamplingValues>();

                        List<DebtSamplingValues> lstCauseCodeModel;
                        List<DebtSamplingValues> lstResolutionCodeModel;
                        List<DebtSamplingValues> lstDebtClassificationModel;
                        List<DebtSamplingValues> lstAvoidableFlagModel;
                        List<DebtSamplingValues> lstResidualDebtModel;


                        if (SupportTypeID == 1)
                        {
                            debtdetails = debtapproval.GetDebtSamplingData(ProjectID);
                        }
                        else
                        {
                            debtdetails = initialapproval.GetInfraDebtSamplingData(ProjectID);
                        }
                        //Getting teh master values for the drop down
                        var debtMasterdetails = debtapproval.GetDebtMasterValues(ProjectID, SupportTypeID);
                        foreach (var MasterValues in debtMasterdetails)
                        {
                            lstDebtMasterValuesModel.Add(new DebtSamplingValues
                            {
                                AttributeType = MasterValues.AttributeType,
                                AttributeTypeId = MasterValues.AttributeTypeId,
                                AttributeTypeValue = MasterValues.AttributeTypeValue
                            });
                        }
                        lstCauseCodeModel = lstDebtMasterValuesModel.Where
                            (x => x.AttributeType == "Cause Code").ToList();
                        lstResolutionCodeModel = lstDebtMasterValuesModel.Where
                                            (x => x.AttributeType == "Resolution Code").ToList();
                        lstAvoidableFlagModel = lstDebtMasterValuesModel.Where
                                            (x => x.AttributeType == "Avoidable Flag").ToList();
                        lstDebtClassificationModel = lstDebtMasterValuesModel.
                            Where(x => x.AttributeType == "Debt Classification").ToList();
                        lstResidualDebtModel = lstDebtMasterValuesModel.Where
                                            (x => x.AttributeType == "Residual Debt").ToList();


                        foreach (var tickets in debtdetails)
                        {
                            lstApprovalModel.Add(new DebtSamplingGetModel
                            {
                                TicketId = tickets.TicketId,
                                ApplicationId = tickets.ApplicationId,
                                ApplicationName = tickets.ApplicationName,
                                TowerId = tickets.TowerId,
                                TowerName = tickets.TowerName,
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

                        return new PattenValidation
                        {
                            MessageValue = "InfraMLSampledTickets",
                            SupportTypeID = SupportTypeID,
                            DebtSamplingGetModels = lstApprovalModel
                        };

                    }
                    else
                    {
                        return new PattenValidation { MessageValue = "MLMessage", ILValidationResults = objILValidationResult };
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
        /// GetMLDetails
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<GetMLDetails>> GetMLDetails(Int32 ProjectID, string UserID, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    var ProcessingDetails = debtapproval.GetProcessingRequiredOnLoad(ProjectID, SupportTypeID);
                    if (string.Compare(ProcessingDetails.IsMLSent, ApplicationConstants.ConstantY) == 0)
                    {
                        debtapproval.CheckIfMLFileGenerated(ProjectID, ProcessingDetails.MLJobId,
                            InitialLearningConstants.JobTypeML, UserID, SupportTypeID);
                    }
                    else if (string.Compare(ProcessingDetails.IsSamplingSent, ApplicationConstants.ConstantY) == 0)
                    {
                        debtapproval.CheckIfMLFileGenerated(ProjectID, ProcessingDetails.SamplingJobId,
                            ApplicationConstants.Sampling_Flag, UserID, SupportTypeID);
                    }
                    else if (string.Compare(ProcessingDetails.IsMLProcessingRequired, ApplicationConstants.ConstantY) == 0)
                    {
                        debtapproval.UpdateMLPatternFromCSV(ProjectID, ProcessingDetails.MLJobId, UserID);
                    }
                    else if (string.Compare(ProcessingDetails.IsSamplingProcessingRequired,
                        ApplicationConstants.ConstantY) == 0)
                    {
                        debtapproval.UpdateSampledTicketsFromCSV(ProjectID, ProcessingDetails.SamplingJobId, UserID);
                    }
                    else if (string.Compare(ProcessingDetails.IsNoiseEliminationSent, ApplicationConstants.ConstantY) == 0)
                    {
                        NoiseElimination noiseEliminationDetails = new NoiseElimination();
                        debtapproval.CheckIfNoiseOutputFileGenerated(ProjectID, ProcessingDetails.NoiseEliminationJobId,
                            SupportTypeID);
                        debtapproval.SaveNoiseEliminationDetails(noiseEliminationDetails, ProjectID, 1, "System");
                    }
                    else
                    {
                        //mandatory else
                    }
                    var debtdetails = debtapproval.GetMLDetailsOnLoad(ProjectID, SupportTypeID);

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

        /// <summary>
        /// Method to Get Download Upload  Section
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDownloadUploadSection(Int32 ProjectID, int SupportTypeID)
        {

            return "_MLDownloadUpload";
        }

        /// <summary>
        /// Method to validate the ML related stuff at the proceed button click
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="Datefrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="UserID"></param>
        /// <param name="OptFieldProjID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ILValidationResult> DebtValidateTicketsForML(DebtValidation Param)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, Param.UserId.ToString(), null, Convert.ToInt64(Param.ProjectId));
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    string CriteriaMet = ApplicationConstants.DefulatCriteriaMet;
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    ILValidationResult result;

                    Utils.ErrorLOGInfra("1 . in DebtValidateTicketsForML",
                 "DebtValidateTicketsForML", Param.ProjectId);
                    var debtTicketDetails = debtapproval.GetDebtValidateTicketsForML(Param.ProjectId,
                        Convert.ToDateTime(Param.DateFrom, CultureInfo.CurrentCulture),
                        Convert.ToDateTime(Param.DateTo,
                        CultureInfo.CurrentCulture), Param.UserId,
                        Convert.ToInt16(Param.OptFieldProjId, CultureInfo.CurrentCulture),
                        Param.SupportTypeId);
                    Utils.ErrorLOGInfra("2 . in DebtValidateTicketsForML Count",
                 "DebtValidateTicketsForML" + debtTicketDetails.Count, Param.ProjectId);
                    if (debtTicketDetails.Count > 0)
                    {
                        Utils.ErrorLOGInfra("3 . in DebtValidateTicketsForML Count",
                 "CriteriaMet =" + CriteriaMet, Param.ProjectId);
                        CriteriaMet = debtapproval.SaveDebtTicketDetailsAfterProcessing(Param.ProjectId, debtTicketDetails,
                            Param.UserId, Param.DateFrom, Param.DateTo, Param.SupportTypeId);
                    }
                    Utils.ErrorLOGInfra("4 . After Count >0 ",
                 "CriteriaMet =" + CriteriaMet, Param.ProjectId);
                    result = debtapproval.ValidateML(CriteriaMet, Param.ProjectId, Param.UserId, Convert.ToDateTime(Param.DateFrom,
                        CultureInfo.CurrentCulture),
                            Convert.ToDateTime(Param.DateTo, CultureInfo.CurrentCulture), Convert.ToInt16(Param.OptFieldProjId,
                            CultureInfo.CurrentCulture), Param.SupportTypeId);
                    Utils.ErrorLOGInfra("5 . After ValidateML >0 ",
                 "CriteriaMet =" + CriteriaMet, Param.ProjectId);

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
        /// GetFilteredNoiseData
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<NoiseEliminationInfra> GetFilteredNoiseDataInfra(string Selection, int Filter, int ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    NoiseEliminationInfra filteredmodel;
                    filteredmodel = objDebtFieldsApprovalRepository.GetFilteredNoiseEliminationDataInfra(Selection,
                        Filter, ProjectID);
                    return filteredmodel;
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
        public ActionResult<ILValidationResult> SaveNoiseEliminationInfraDetails(NoiseEliminationDetails noiseDetails)
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
                    ILValidationResult objILValidationResult = new ILValidationResult();
                    NoiseEliminationInfra noiseData = new NoiseEliminationInfra();
                    SanitizeStringInput SanEmployeeId = Convert.ToString(CurrentUser.ID,
                        CultureInfo.CurrentCulture);
                    noiseData.LstNoiseTicketDescriptionInfra = noiseDetails.NoiseTicketDescriptions;
                    noiseData.LstNoiseResolutionInfra = noiseDetails.NoiseResolutions;
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    string CriteriaMet = objDebtFieldsApprovalRepository.SaveNoiseEliminationInfraDetails(noiseData,
                        noiseDetails.Param.ProjectId, noiseDetails.Param.Choose, SanEmployeeId.Value);
                    if (noiseDetails.Param.Choose == 3)
                    {
                        objILValidationResult = objDebtFieldsApprovalRepository.ValidateML(CriteriaMet, noiseDetails.Param.ProjectId, SanEmployeeId.Value,
                           Convert.ToDateTime(noiseDetails.Param.DateFrom, CultureInfo.CurrentCulture), Convert.ToDateTime(noiseDetails.Param.DateTo,
                           CultureInfo.CurrentCulture), Convert.ToInt16(noiseDetails.Param.OptFieldProjId,
                           CultureInfo.CurrentCulture),
                           noiseDetails.Param.SupportTypeId);
                    }

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
        /// GetFilteredNoiseData
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<NoiseElimination> GetFilteredNoiseData(string Selection, int Filter, int ProjectID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    NoiseElimination filterednoisemodel;
                    filterednoisemodel = objDebtFieldsApprovalRepository.GetFilteredNoiseEliminationData(Selection,
                        Filter, ProjectID);
                    return filterednoisemodel;
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
        public ActionResult<ILValidationResult> NoiseEliminationInfraSkipCont(DebtValidation Param)
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
                    ILValidationResult objILValidationResult;
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    SanitizeStringInput UserID = Convert.ToString(CurrentUser.ID,
                        CultureInfo.CurrentCulture);
                    string CriteriaMet = debtapproval.UpdateNoiseInfraSkipAndContinue(Param.ProjectId, UserID.Value);
                    objILValidationResult = debtapproval.ValidateML(CriteriaMet, Param.ProjectId, UserID.Value,
                        Convert.ToDateTime(Param.DateFrom,
                        CultureInfo.CurrentCulture),
                            Convert.ToDateTime(Param.DateTo,
                            CultureInfo.CurrentCulture), Convert.ToInt16(Param.OptFieldProjId,
                            CultureInfo.CurrentCulture), Param.SupportTypeId);
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
        /// To get MLPatternValidationInfra
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<List<MLPatternValidationInfra>> GetMLPatternValidationInfra(int ProjectId, string UserID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, UserID, null, Convert.ToInt64(ProjectId));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
                    List<MLPatternValidationInfra> debtMLPatterndetails =
                        debtapproval.GetMLpatternValidationReport(ProjectId, UserID);
                    return debtMLPatterndetails;
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
        /// To Get DebtMLGetPatternOccurenceInfra
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TowerID"></param>
        /// <param name="TowerName"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="subPattern"></param>
        /// <param name="AdditionalTextPattern"></param>
        /// <param name="AdditionalTextSubPattern"></param>
        /// <param name="CauseCodeId"></param>
        /// <param name="ResolutionCodeId"></param>
        /// <param name="AvoidableFlagId"></param>
        /// <param name="DebtClassificationId"></param>
        /// <param name="ResidualFlagId"></param>
        /// <param name="CauseCodeName"></param>
        /// <param name="ResolutionCodeName"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult<PatternOccurenceInfra> DebtMLGetPatternOccurenceInfra(int ProjectID,
            int[] TowerID, string TowerName,
           string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextSubPattern,
           int CauseCodeId, int ResolutionCodeId, int AvoidableFlagId, int DebtClassificationId,
           int ResidualFlagId, string CauseCodeName, string ResolutionCodeName, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(ProjectID));
            string towerIds = string.Join(",", TowerID.Select(x => x.ToString(CultureInfo.CurrentCulture)).ToArray());
            MlPatternInfraDetails lstDebtMLPatternModel = new MlPatternInfraDetails();
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
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
                    var DebtMLPatterndetails = debtapproval.GetDebtMLPatternOccurenceReporInfra(ProjectID, towerIds,
                        TicketPattern.Trim(), subPattern.Trim(), AdditionalTextPattern.Trim(),
                        AdditionalTextSubPattern.Trim(), CauseCodeId, ResolutionCodeId);
                    var DebtMasterdetails = debtapproval.GetDebtMasterValues(ProjectID, SupportTypeID);

                    foreach (var MasterValues in DebtMasterdetails)
                    {
                        lstMLDebtMasterValuesModel.Add(new DebtMasterValues
                        {
                            AttributeType = MasterValues.AttributeType,
                            AttributeTypeId = MasterValues.AttributeTypeId,
                            AttributeTypeValue = MasterValues.AttributeTypeValue
                        });
                    }

                    lstCauseCodeModel = lstMLDebtMasterValuesModel.Where(x => x.AttributeType == "Cause Code").ToList();
                    lstAvoidableFlagModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == "Avoidable Flag").ToList();
                    lstDebtClassificationModel = lstMLDebtMasterValuesModel.
                        Where(x => x.AttributeType == "Debt Classification").ToList();
                    lstResidualDebtModel = lstMLDebtMasterValuesModel.Where(x => x.AttributeType == "Residual Debt").ToList();
                    if (DebtMLPatterndetails.Count > 0)
                    {
                        lstDebtMLPatternModel.LstDebtMLInfraDet = DebtMLPatterndetails;
                    }
                    else
                    {
                        MLPatternValidationInfra mldetailsoccurence = new MLPatternValidationInfra();
                        mldetailsoccurence.Id = 0;
                        mldetailsoccurence.TicketPattern = TicketPattern.Trim();

                        if (!string.IsNullOrEmpty(subPattern))
                        {
                            mldetailsoccurence.SubPattern = subPattern.Trim();
                        }
                        else
                        {
                            mldetailsoccurence.SubPattern = "N/A";
                        }
                        mldetailsoccurence.AdditionalTextPattern = !string.IsNullOrEmpty(AdditionalTextPattern) ?
                            AdditionalTextPattern.Trim() : "N/A";
                        mldetailsoccurence.AdditionalTextsubPattern = !string.IsNullOrEmpty(AdditionalTextPattern) ?
                            AdditionalTextPattern.Trim() : "N/A";
                        mldetailsoccurence.TicketOccurence = 0;
                        mldetailsoccurence.MLDebtClassificationId = 0;
                        mldetailsoccurence.MLDebtClassificationName = string.Empty;
                        mldetailsoccurence.MLResidualFlagId = 0;
                        mldetailsoccurence.MLResidualFlagName = string.Empty;
                        mldetailsoccurence.MLAvoidableFlagId = 0;
                        mldetailsoccurence.MLAvoidableFlagName = string.Empty;
                        mldetailsoccurence.MLCauseCodeId = CauseCodeId;
                        mldetailsoccurence.MLCauseCodeName = CauseCodeName.Trim();

                        mldetailsoccurence.TowerName = TowerName.Trim();
                        mldetailsoccurence.MLResolutionCodeId = ResolutionCodeId;
                        mldetailsoccurence.MLResolutionCode = ResolutionCodeName.Trim();
                        mldetailsoccurence.TowerId = Convert.ToInt32(towerIds,
                            CultureInfo.CurrentCulture);
                        mldetailsoccurence.MLAccuracy = string.Empty;
                        lstDebtMLPatternModel.LstDebtMLInfraDet.Add(mldetailsoccurence);

                    }

                    lstDebtMLPatternModel.LstCauseCodeModel = lstCauseCodeModel;
                    lstDebtMLPatternModel.LstAvoidableFlagModel = lstAvoidableFlagModel;
                    lstDebtMLPatternModel.LstDebtClassificationModel = lstDebtClassificationModel;
                    lstDebtMLPatternModel.LstResidualDebtModel = lstResidualDebtModel;
                    return new PatternOccurenceInfra
                    {
                        AvoidableFlagID = AvoidableFlagId,
                        DebtClassificationID = DebtClassificationId,
                        ResidualCodeID = ResidualFlagId,
                        MLPatternOccurenceInfra = lstDebtMLPatternModel
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
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<RegenerateILDetails> GetRenerateView(Int32 projectID, Int16 SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtrepo = new DebtFieldsApprovalRepository();
                    var regenerateILDetails = debtrepo.GetRegenerateILDetails(projectID, SupportTypeID);
                    return regenerateILDetails;
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
        /// Used for generate patterns functionality
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstGeneratePatternApps"></param>
        /// <param name="UserId"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<bool> GeneratePatterns(int projectID, List<RegenerateApplicationDetails> lstGeneratePatternApps,
            string userId, int customerID, int SupportTypeID)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, userId, Convert.ToInt64(customerID), Convert.ToInt64(projectID));
            try
            {
                if (value)
                {
                    bool result = false;
                    string generatepatternsresult = string.Empty;
                    DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
                    if (SupportTypeID == 2)
                    {
                        generatepatternsresult = debtApproval.GenerateInfrapatterns(projectID, lstGeneratePatternApps,
                            userId, customerID);
                    }
                    else
                    {
                        generatepatternsresult = debtApproval.Generatepatterns(projectID, lstGeneratePatternApps,
                            userId, customerID);
                    }
                    if (string.Compare(generatepatternsresult, ApplicationConstants.ConstantY) == 0)
                    {
                        result = true;

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

                return false;
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
        public ActionResult<SPDebtMLPatternValidationModel> DebtMLPatternValidationGridViewALL(int projectId, string userID, int SupportTypeId)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, userID, null, Convert.ToInt64(projectId));
            try
            {
                if (value)
                {
                    DebtFieldsApprovalRepository debtApproval = new DebtFieldsApprovalRepository();
                    SpDebtMLPatternValidationModelForViewAll lstDebtMLPatternModel =
                        new SpDebtMLPatternValidationModelForViewAll();

                    List<DebtMasterValues> lstMLDebtMasterValuesModel = new List<DebtMasterValues>();
                    List<DebtMasterValues> lstCauseCodeModel;
                    List<DebtMasterValues> lstResolutionCodeModel = new List<DebtMasterValues>();
                    List<DebtMasterValues> lstDebtClassificationModel;
                    List<DebtMasterValues> lstAvoidableFlagModel;
                    List<DebtMasterValues> lstResidualDebtModel;
                    var debtDetails = debtApproval.GetMLDetailsOnLoad(projectId, SupportTypeId);
                    SpDebtMLPatternValidationModelForViewAll debtMLPatternValidationList =
                        debtApproval.GetDebtMLPatternValidationReportForViewAll(projectId, SupportTypeId);

                    var debtMasterdetails = debtApproval.GetDebtMasterValues(projectId, SupportTypeId);
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
                            lstDebtMLPatternModel.ExistingPatternsModel.Add(new SpDebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id,
                                CultureInfo.CurrentCulture),
                                InitialLearningId = tickets.InitialLearningId,
                                ApplicationId = tickets.ApplicationId,
                                TowerId = tickets.TowerId,
                                TowerName = tickets.TowerName,
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
                                MLResolutionCode = tickets.MLResolutionCode,
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
                                IsCLSignoff = debtDetails.Count > 0 ? Convert.ToBoolean(debtDetails[0].MLSignoff,
                                CultureInfo.CurrentCulture) : false,
                                IsMLSignoff = tickets.IsMLSignoff

                            });
                        }
                    }
                    if (debtMLPatternValidationList.NewPatternsModel != null)
                    {
                        foreach (var tickets in debtMLPatternValidationList.NewPatternsModel)
                        {
                            lstDebtMLPatternModel.ExistingPatternsModel.Add(new SpDebtMLPatternValidationModel
                            {
                                Id = Convert.ToInt32(tickets.Id,
                                CultureInfo.CurrentCulture),
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
                                MLResolutionCode = tickets.MLResolutionCode,
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
                                IsCLSignoff = debtDetails.Count > 0 ? Convert.ToBoolean(debtDetails[0].MLSignoff,
                                CultureInfo.CurrentCulture) : false,
                                IsMLSignoff = tickets.IsMLSignoff
                            });
                        }
                    }

                    return new SPDebtMLPatternValidationModel { SupportTypeID = SupportTypeId, SpDebtMLPatternValidationModels = lstDebtMLPatternModel };
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