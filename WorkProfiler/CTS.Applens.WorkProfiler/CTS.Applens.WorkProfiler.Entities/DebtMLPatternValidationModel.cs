using System.Collections.Generic;

namespace CTS.Applens.WorkProfiler.Models
{   
    /// <summary>
    /// This class holds DebtMLPatternValidationModel details 
    /// </summary>
    public class DebtMLPatternValidationModel
    {
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
        private List<DebtMasterValues> lstreasonforResidualModel = new List<DebtMasterValues>();
        /// <summary>
        /// Gets or sets lstReasonforResidualModel
        /// </summary>
        public List<DebtMasterValues> LstReasonforResidualModel
        {
            get
            {
                return lstreasonforResidualModel;
            }
            set
            {
                lstreasonforResidualModel = value;
            }
        }
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets InitialLearningID
        /// </summary>
        public int? InitialLearningId { get; set; }
        /// <summary>
        /// Gets or sets ContLearningID
        /// </summary>
        public int? ContLearningId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int? ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationTypeID
        /// </summary>
        public int? ApplicationTypeId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationTypeName
        /// </summary>
        public string ApplicationTypeName { get; set; }
        /// <summary>
        /// Gets or sets TechnologyID
        /// </summary>
        public int? TechnologyId { get; set; }
        /// <summary>
        /// Gets or sets TechnologyName
        /// </summary>
        public string TechnologyName { get; set; }
        /// <summary>
        /// Gets or sets TicketPattern
        /// </summary>
        public string TicketPattern { get; set; }

        /// <summary>
        /// Gets or sets ticketsubPattern
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
        /// Gets or sets OverriddenPatternCount
        /// </summary> 
        public int OverriddenPatternCount { get; set; }
        /// <summary>
        /// Gets or sets PatternType
        /// </summary>
        public string PatternType { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassificationID
        /// </summary>
        public int? MLDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassificationName
        /// </summary>
        public string MLDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets MLResidualFlagID
        /// </summary>
        public int? MLResidualFlagId { get; set; }
        /// <summary>
        /// Gets or sets MLResidualFlagName
        /// </summary>
        public string MLResidualFlagName { get; set; }
        /// <summary>
        /// Gets or sets MLAvoidableFlagID
        /// </summary>
        public int? MLAvoidableFlagId { get; set; }
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
        /// Gets or sets MLResolutionCodeID
        /// </summary>
        public int? MLResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets MLResolutionCodeName
        /// </summary>
        public string MLResolutionCodeName { get; set; }
        /// <summary>
        /// Gets or sets TicketOccurence
        /// </summary> 
        public int? TicketOccurence { get; set; }
        /// <summary>
        /// Gets or sets AnalystResolutionCodeID
        /// </summary>
        public int? AnalystResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets AnalystResolutionCodeName
        /// </summary>
        public string AnalystResolutionCodeName { get; set; }
        /// <summary>
        /// Gets or sets AnalystCauseCodeID
        /// </summary>
        public int? AnalystCauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets AnalystCauseCodeName
        /// </summary>
        public string AnalystCauseCodeName { get; set; }
        /// <summary>
        /// Gets or sets AnalystDebtClassificationID
        /// </summary>
        public int? AnalystDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AnalystDebtClassificationName
        /// </summary>
        public string AnalystDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets AnalystAvoidableFlagID
        /// </summary>
        public int? AnalystAvoidableFlagId { get; set; }
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
        /// Gets or sets HasDiscrepency
        /// </summary>
        public int HasDiscrepency { get; set; }
        /// <summary>
        /// Gets or sets IsCLSignOff
        /// </summary>
        public bool IsCLSignoff { get; set; }
        /// <summary>
        /// Get or sets IsMLSignOff
        /// </summary>
        public bool IsMLSignoff { get; set; }
        /// <summary>
        /// Get or sets IsRegMLSignOff
        /// </summary>
        public bool IsRegMLSignoff { get; set; }

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
        /// Gets or sets OverriddenPatternTotalCount
        /// </summary>
        public int OverriddenPatternTotalCount { get; set; }
        /// <summary>
        /// Get or set IsRegenerated
        /// </summary>
        public int IsRegenerated { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Gets or sets PatternsOrigin
        /// </summary>
        public string PatternsOrigin { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultRuleSelected
        /// </summary>
        public int IsDefaultRuleSelected { get; set; }
        /// <summary>
        /// Gets or sets IsApprovedPatternsConflict
        /// </summary>
        public string IsApprovedPatternsConflict { get; set; }
        /// <summary>
        /// Gets or sets ApprovedFlag
        /// </summary>
        public string ApprovedFlag { get; set; }
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
    /// <summary>
    /// RegenerateApplicationDetails
    /// </summary>
    public class RegenerateApplicationDetails
    {
        /// <summary>
        /// Get or set ApplicationID
        /// </summary>
        public string ApplicationId { get; set; }

    }

    /// <summary>
    /// Conflict Patterns
    /// </summary>
    public class ConflictPatterns
    {
        /// <summary>
        /// Get or set ApplicationName
        /// </summary>
        public string ApplicationName  { get; set; }

        /// <summary>
        /// Get or set CauseCodeName
        /// </summary>
        public string CauseCodeName  { get; set; }

        /// <summary>
        /// Get or set ResolutionName
        /// </summary>
        public string ResolutionName { get; set; }

        /// <summary>
        /// Get or set DebtClassficationName
        /// </summary>
        public string DebtClassficationName { get; set; }

        /// <summary>
        /// Get or set AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }

        /// <summary>
        /// Get or set ResidualFlag
        /// </summary>
        public string ResidualFlag { get; set; }

        /// <summary>
        /// Get or set ExistingPattern
        /// </summary>
        public string ExistingPattern { get; set; }

        /// <summary>
        /// Get or set TicketCount
        /// </summary>
        public int TicketCount { get; set; }


        /// <summary>
        /// Get or set Period
        /// </summary>
        public string Period { get; set; }
    }

    public class DebtMLPatternValidationResult 
    {
        /// <summary>
        /// Get or set DebtMLPatternValidationModels
        /// </summary>
        public List<DebtMLPatternValidationModel> DebtMLPatternValidationModels { get; set; }

        /// <summary>
        /// Get or set JobStatusMessage
        /// </summary>
        public string JobStatusMessage { get; set; }

        /// <summary>
        /// Get or set AvoidableFlagID
        /// </summary>
        public int AvoidableFlagId { get; set; }

        /// <summary>
        /// Get or set DebtClassificationID
        /// </summary>
        public int DebtClassificationId { get; set; }

        /// <summary>
        /// Get or set ResidualCodeID
        /// </summary>
        public int ResidualCodeId { get; set; }

        /// <summary>
        /// Get or set OldDebtID
        /// </summary>
        public int OldDebtId { get; set; }
    }

}
