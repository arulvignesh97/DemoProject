using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds SpDebtMLPatternValidationModel details
    /// </summary>
    public class SpDebtMLPatternValidationModel
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets InitialLearningID
        /// </summary>
        public int InitialLearningId { get; set; }
        /// <summary>
        /// Gets or sets ContLearningID
        /// </summary>
        public int ContLearningId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary> 
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary> 
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationTypeID
        /// </summary>
        public int ApplicationTypeId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationTypeName
        /// </summary>
        public string ApplicationTypeName { get; set; }
        /// <summary>
        /// Gets or sets TechnologyID
        /// </summary>
        public int TechnologyId { get; set; }
        /// <summary>
        /// Gets or sets TechnologyName
        /// </summary>
        public string TechnologyName { get; set; }
        /// <summary>
        /// Gets or sets TicketPattern
        /// </summary>
        public string TicketPattern { get; set; }
        /// <summary>
        /// Gets or sets  TicketsubPattern
        /// </summary>
        public string TicketSubPattern { get; set; }
        /// <summary>
        /// Gets or sets subPattern
        /// </summary>
        public string SubPattern { get; set; }
        /// <summary>
        /// Gets or sets AdditionalTextPattern
        /// </summary>
        public string AdditionalTextPattern { get; set; }
        /// <summary>
        /// Gets or sets AdditionalTextsubPattern
        /// </summary>
        public string AdditionalTextsubPattern { get; set; }
        /// <summary>
        /// Gets or sets PatternType
        /// </summary>
        public string PatternType { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassificationID
        /// </summary>
        public int MLDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassificationName
        /// </summary>
        public string MLDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets MLResidualFlagID
        /// </summary>
        public int MLResidualFlagId { get; set; }
        /// <summary>
        /// Gets or sets MLResidualFlagName
        /// </summary>
        public string MLResidualFlagName { get; set; }
        /// <summary>
        /// Gets or sets MLAvoidableFlagID
        /// </summary>
        public int MLAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets MLAvoidableFlagName
        /// </summary>
        public string MLAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets MLCauseCodeID
        /// </summary>
        public int? MLCauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets MLCauseCodeName
        /// </summary>
        public string MLCauseCodeName { get; set; }
        /// <summary>
        /// Gets or sets MLAccuracy
        /// </summary>
        public string MLAccuracy { get; set; }
        /// <summary>
        /// Gets or sets TicketOccurence
        /// </summary>
        public int TicketOccurence { get; set; }
        /// <summary>
        /// Gets or sets AnalystResolutionCodeID
        /// </summary>
        public int AnalystResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets AnalystResolutionCodeName
        /// </summary>
        public string AnalystResolutionCodeName { get; set; }
        /// <summary>
        /// Gets or sets AnalystCauseCodeID
        /// </summary>
        public int AnalystCauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets AnalystCauseCodeName
        /// </summary>
        public string AnalystCauseCodeName { get; set; }
        /// <summary>
        /// Gets or sets AnalystDebtClassificationID
        /// </summary>
        public int AnalystDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AnalystDebtClassificationName
        /// </summary>
        public string AnalystDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets AnalystAvoidableFlagID
        /// </summary>
        public int AnalystAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets AnalystAvoidableFlagName
        /// </summary>
        public string AnalystAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets SMEComments
        /// </summary>
        public string SMEComments { get; set; }
        /// <summary>
        /// Gets or sets SMEResidualFlagID
        /// </summary>
        public int SMEResidualFlagId { get; set; }
        /// <summary>
        /// Gets or sets SMEResidualFlagName
        /// </summary>
        public string SMEResidualFlagName { get; set; }
        /// <summary>
        /// Gets or sets SMEDebtClassificationID
        /// </summary>
        public int SMEDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets SMEDebtClassificationName
        /// </summary>
        public string SMEDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets SMEAvoidableFlagID
        /// </summary>
        public int SMEAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets SMEAvoidableFlagName
        /// </summary>
        public string SMEAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets SMECauseCodeID
        /// </summary>
        public int SMECauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets SMECauseCodeName
        /// </summary>
        public string SMECauseCodeName { get; set; }
        /// <summary>
        /// Gets or sets IsApprovedOrMute
        /// </summary>
        public int IsApprovedOrMute { get; set; }
        /// <summary>
        /// Gets or sets IsApproved
        /// </summary>
        public int IsApproved { get; set; }
        /// <summary>
        /// Gets or sets IsCLSignOff
        /// </summary>
        public bool IsCLSignoff { get; set; }

        public bool IsMLSignoff { get; set; }
        /// <summary>
        /// Gets or sets MLResolutionCodeID
        /// </summary>
        public int MLResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets MLResolutionCode
        /// </summary>
        public string MLResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets ReasonforResidualID
        /// </summary>
        public int ReasonforResidualId { get; set; }
        /// <summary>
        /// Gets or sets ReasonforResidual
        /// </summary>
        public string ReasonforResidual { get; set; }
        /// <summary>
        /// Gets or sets ExpectedCompletionDate
        /// </summary>
        public string ExpectedCompletionDate { get; set; }
        /// <summary>
        /// Gets or sets Discrepancy
        /// </summary>
        public string Discrepancy { get; set; }
        /// <summary>
        /// Gets or sets OverridenTotalCount
        /// </summary>
        public int OverridenTotalCount { get; set; }

        public int IsRegenerated { get; set; }
        /// <summary>
        /// Gets or sets PatternsOrigin
        /// </summary>
        public string PatternsOrigin { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultRuleSelected
        /// </summary>
        public Int32 IsDefaultRuleSelected { get; set; }
        /// <summary>
        /// Gets or sets IsApprovedPatternsConflict
        /// </summary>
        public string IsApprovedPatternsConflict { get; set; }
        /// <summary>
        /// Gets or sets ApprovedFlag
        /// </summary>
        public string ApprovedFlag { get; set; }

        /// <summary>
        /// This is a Method Declaration of LstDebtClassificationModel !
        /// </summary>
        public List<DebtMasterValues> LstDebtClassificationModel { get; set; }

        private List<DebtMasterValues> lstAvoidableFlagModel = new List<DebtMasterValues>();
        /// <summary>
        /// Gets or sets LstAvoidableFlagModel
        /// </summary>
        public List<DebtMasterValues> LstAvoidableFlagModel
        {
            get
            {
                return lstAvoidableFlagModel;
            }
            set
            {
                lstAvoidableFlagModel = value;
            }
        }
        private List<DebtMasterValues> lstResidualDebtModel = new List<DebtMasterValues>();
        /// <summary>
        /// Gets or sets lstResidualDebtModel
        /// </summary>
        public List<DebtMasterValues> LstResidualDebtModel
        {
            get
            {
                return lstResidualDebtModel;
            }
            set
            {
                lstResidualDebtModel = value;
            }
        }

        private List<DebtMasterValues> lstCauseCodeModel = new List<DebtMasterValues>();
        /// <summary>
        /// Gets or sets lstCauseCodeModel
        /// </summary>
        public List<DebtMasterValues> LstCauseCodeModel
        {
            get
            {
                return lstCauseCodeModel;
            }
            set
            {
                lstCauseCodeModel = value;
            }
        }

        private List<DebtMasterValues> lstReasonforResidualModel = new List<DebtMasterValues>();
        /// <summary>
        /// Gets or sets lstReasonforResidualModel
        /// </summary>
        public List<DebtMasterValues> LstReasonforResidualModel
        {
            get
            {
                return lstReasonforResidualModel;
            }
            set
            {
                lstReasonforResidualModel = value;
            }
        }


    }
    /// <summary>
    /// This class holds Debt Pattern Validation details
    /// </summary>
    public class GetDebtPatternValidation{
        /// <summary>
        /// This is a Method Declaration of LstNoiseResolution !
        /// </summary>
        public List<SpDebtMLPatternValidationModel> DebtMLPatternValidationModel { get; set; }
        /// <summary>
        /// Gets or sets JobStatusMessage
        /// </summary>
        public string JobStatusMessage { get; set; }
        /// <summary>
        /// Gets or sets JobFromDate
        /// </summary>
        public string JobFromDate { get; set; }
        /// <summary>
        /// Gets or sets JobToDate
        /// </summary>
        public string JobToDate { get; set; }
    }   
}
