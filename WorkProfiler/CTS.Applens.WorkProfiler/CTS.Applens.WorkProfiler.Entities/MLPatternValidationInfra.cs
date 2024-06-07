using System.Collections.Generic;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// MLPatternValidationInfra
    /// </summary>
    public class MLPatternValidationInfra
    {
        /// <summary>
        /// Gets or Sets ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or Sets InitialLearningID
        /// </summary>
        public int InitialLearningId { get; set; }
        /// <summary>
        /// Gets or Sets TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or Sets TowerName
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or Sets IsApprovedOrMute
        /// </summary>
        public int IsApprovedOrMute { get; set; }
        /// <summary>
        /// Gets or Sets IsMLSignOff
        /// </summary>
        public bool IsMLSignoff { get; set; }
        /// <summary>
        /// Gets or Sets IsRegenerated
        /// </summary>
        public bool IsRegenerated { get; set; }
        /// <summary>
        /// Gets or Sets MLAccuracy
        /// </summary>
        public string MLAccuracy { get; set; }
        /// <summary>
        /// Gets or Sets AnalystAvoidableFlagID
        /// </summary>
        public int AnalystAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or Sets AnalystAvoidableFlagName
        /// </summary>
        public string AnalystAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or Sets AnalystCauseCodeID
        /// </summary>
        public int AnalystCauseCodeId { get; set; }
        /// <summary>
        /// Gets or Sets AnalystCauseCodeName
        /// </summary>
        public string AnalystCauseCodeName { get; set; }
        /// <summary>
        /// Gets or Sets AnalystDebtClassificationID
        /// </summary>
        public int AnalystDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or Sets AnalystDebtClassificationName
        /// </summary>
        public string AnalystDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or Sets AnalystResolutionCodeID
        /// </summary>
        public int AnalystResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or Sets AnalystResolutionCodeName
        /// </summary>
        public string AnalystResolutionCodeName { get; set; }
        /// <summary>
        /// Gets or Sets MLAvoidableFlagID
        /// </summary>
        public int MLAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or Sets MLAvoidableFlagName
        /// </summary>
        public string MLAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or Sets MLCauseCodeID
        /// </summary>
        public int? MLCauseCodeId { get; set; }
        /// <summary>
        /// Gets or Sets MLCauseCodeName
        /// </summary>
        public string MLCauseCodeName { get; set; }
        /// <summary>
        /// Gets or Sets MLDebtClassificationID
        /// </summary>
        public int MLDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or Sets MLDebtClassificationName
        /// </summary>
        public string MLDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or Sets MLResidualFlagID
        /// </summary>
        public int MLResidualFlagId { get; set; }
        /// <summary>
        /// Gets or Sets MLResidualFlagName
        /// </summary>
        public string MLResidualFlagName { get; set; }
        /// <summary>
        /// Gets or Sets MLResolutionCode
        /// </summary>
        public string MLResolutionCode { get; set; }
        /// <summary>
        /// Gets or Sets MLResolutionCodeID
        /// </summary>
        public int MLResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or Sets OverridenTotalCount
        /// </summary>
        public int OverridenTotalCount { get; set; }
        /// <summary>
        /// Gets or Sets SMEAvoidableFlagID
        /// </summary>

        public int SMEAvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or Sets SMEAvoidableFlagName
        /// </summary>
        public string SMEAvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or Sets SMECauseCodeID
        /// </summary>
        public int SMECauseCodeId { get; set; }
        /// <summary>
        /// Gets or Sets SMECauseCodeName
        /// </summary>
        public string SMECauseCodeName { get; set; }
        /// <summary>
        /// Gets or Sets SMEComments
        /// </summary>
        public string SMEComments { get; set; }
        /// <summary>
        /// Gets or Sets SMEDebtClassificationID
        /// </summary>
        public int SMEDebtClassificationId { get; set; }
        /// <summary>
        /// Gets or Sets SMEDebtClassificationName
        /// </summary>
        public string SMEDebtClassificationName { get; set; }
        /// <summary>
        /// Gets or Sets SMEResidualFlagID
        /// </summary>
        public int SMEResidualFlagId { get; set; }
        /// <summary>
        /// Gets or Sets SMEResidualFlagName
        /// </summary>
        public string SMEResidualFlagName { get; set; }
        /// <summary>
        /// Gets or Sets SubPattern
        /// </summary>
        public string SubPattern { get; set; }
        /// <summary>
        /// Gets or Sets TicketOccurence
        /// </summary>

        public int TicketOccurence { get; set; }
        /// <summary>
        /// Gets or Sets TicketPattern
        /// </summary>
        public string TicketPattern { get; set; }
        /// <summary>
        /// Gets or Sets AdditionalTextPattern
        /// </summary>
        public string AdditionalTextPattern { get; set; }
        /// <summary>
        /// Gets or Sets AdditionalTextsubPattern
        /// </summary>
        public string AdditionalTextsubPattern { get; set; }
        /// <summary>
        /// Gets or Sets TowerID
        /// </summary>
    }

    public class MlPatternInfraDetails
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


        private List<MLPatternValidationInfra> lstMLInfraDetails = new List<MLPatternValidationInfra>();
        public List<MLPatternValidationInfra> LstDebtMLInfraDet
        {
            get
            {
                return lstMLInfraDetails;
            }
            set
            {
                lstMLInfraDetails = value;
            }
        }
    }
}
