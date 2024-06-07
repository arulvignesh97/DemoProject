using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds GetDebtTicketsForValidation details
    /// </summary>
    public class GetDebtTicketsForValidation
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary> 
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public string DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagId
        /// </summary>
        public string AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtId
        /// </summary>
        public string ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets OptionalField
        /// </summary>
        public string OptionalField { get; set; }
        /// <summary>
        /// Gets or sets TowerID
        /// </summary>
        public Int32? TowerId { get; set; }
    }
    /// <summary>
    /// This class holds GetProjectDetailsById details
    /// </summary>
    public class GetProjectDetailsById
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set;}
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public int? SupportTypeId { get; set; }
    }
    /// <summary>
    /// This class holds MLDebtResult details
    /// </summary>
    public class MLDebtResult
    {
        /// <summary>
        /// Gets or sets CriteriaMet
        /// </summary>
        public string CriteriaMet { get; set; }
        /// <summary>
        /// Gets or sets ValidTicketPercentage
        /// </summary>
        public decimal ValidTicketPercentage { get; set; }
    }
    /// <summary>
    /// This class holds GetMLJobDetails details
    /// </summary>
    public class GetMLJobDetails
    {
        /// <summary>
        /// Gets or sets MLJobId
        /// </summary>
        public string MLJobId { get; set; }
        /// <summary>
        /// Gets or sets DataPath
        /// </summary>
        public string DataPath { get; set; }
        /// <summary>
        /// Gets or sets FileName
        /// </summary>
        public string FileName { get; set; }
    }
    /// <summary>
    /// This class is used for copy functionality
    /// </summary>
    public class CopyFields
    {
        /// <summary>
        /// Used to get and set LobID
        /// </summary>
        public int LobId { get; set; }
        /// <summary>
        /// Used to get and set lobName
        /// </summary>
        public string LobName { get; set; }
        /// <summary>
        /// Used to get and set portfolioID
        /// </summary>
        public int PortfolioId { get; set; }
        /// <summary>
        /// Used to get and set portfolioName
        /// </summary>
        public string PortfolioName { get; set; }
        /// <summary>
        /// Used to get and set appGroupID
        /// </summary>
        public int AppGroupId { get; set; }
        /// <summary>
        /// Used to get and set appGroupName
        /// </summary>
        public string AppGroupName { get; set; }
        /// <summary>
        /// Used to get and set applicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Used to get and set applicationName
        /// </summary>
        public string ApplicationName { get; set; }


    }

    /// <summary>
    /// This class holds GetMLDetails details
    /// </summary>
    public class GetMLDetails
    {
        /// <summary>
        /// Gets or sets MLStatus
        /// </summary>
        public string MLStatus { get; set; }
        /// <summary>
        /// Gets or sets NoiseEliminationSent
        /// </summary>
        public string NoiseEliminationSent { get; set; }
        /// <summary>
        /// Gets or sets MLSentBy
        /// </summary>
        public string MLSentBy { get; set; }
        /// <summary>
        /// Gets or sets MLSentDate
        /// </summary>
        public string MLSentDate { get; set; }
        /// <summary>
        /// Gets or sets SamplingSentDate
        /// </summary>
        public string SamplingSentDate { get; set; }
        /// <summary>
        /// Gets or sets NoiseSentDate
        /// </summary>
        public string NoiseSentDate { get; set; }
        /// <summary>
        /// Gets or sets DataValidationDate
        /// </summary>
        public string DataValidationDate { get; set; }
        /// <summary>
        /// Gets or sets SamplingSentBy
        /// </summary>
        public string SamplingSentBy { get; set; }
        /// <summary>
        /// Gets or sets MlReceiveddate
        /// </summary>
        public string MlReceiveddate { get; set; }
        /// <summary>
        /// Gets or sets SamplingSentOrReceivedStatus
        /// </summary>
        public string SamplingSentOrReceivedStatus { get; set; }
        /// <summary>
        /// Gets or sets SamplingInProgressStatus
        /// </summary>
        public string SamplingInProgressStatus { get; set; }
        /// <summary>
        /// Gets or sets startDate
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Get or Set IsRegenerated
        /// </summary>
        public bool IsRegenerated { get; set; }
        /// <summary>
        /// Get or Set RegStartDate
        /// </summary>
        public string RegStartDate { get; set; }
        /// <summary>
        /// Get or Set RegEndDate
        /// </summary>
        public string RegEndDate { get; set; }
        /// <summary>
        /// Gets or sets IsSDTicket
        /// </summary>
        public bool IsSDTicket { get; set; }
        /// <summary>
        /// Gets or sets MLSignoff
        /// </summary>
        public int MLSignoff { get; set; }
        /// <summary>
        /// Gets or sets OptionalFieldID
        /// </summary>
        public int OptionalFieldId { get; set; }
        /// <summary>
        /// Gets or sets AutoclassificationDatestring
        /// </summary>
        public string AutoclassificationDatestring { get; set; }
        /// <summary>
        /// Gets or sets IsDartTicket
        /// </summary>
        public bool IsDartTicket { get; set; }
        /// <summary>
        /// Gets or sets ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets IsAutoClassified
        /// </summary>
        public string IsAutoClassified { get; set; }
        /// <summary>
        /// Gets or sets IsRegMLsignOff
        /// </summary>
        public int IsRegMLsignOff { get; set; }
        /// <summary>
        /// Gets or sets RegenerateCount
        /// </summary>
        public int RegenerateCount { get; set; }
    }
    /// <summary>
    /// This class holds DebtMLPatternSaveModel details
    /// </summary>
    public class DebtMLPatternSaveModel
    {
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }


        /// <summary>
        /// Gets or sets Tower Name
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationType
        /// </summary>
        public string ApplicationType { get; set; }
        /// <summary>
        /// Gets or sets Technology
        /// </summary>
        public string Technology { get; set; }
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public string DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassification
        /// </summary>
        public string MLDebtClassification { get; set; }
        /// <summary>
        /// Gets or sets MLAvoidableFlag
        /// </summary>
        public string MLAvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets MLResidualDebt
        /// </summary>
        public string MLResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets MLCauseCode
        /// </summary>
        public string MLCauseCode { get; set; }
        /// <summary>
        /// Gets or sets MLResolutionCode
        /// </summary>
        public string MLResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets MLWorkPattern
        /// </summary>
        public string MLWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets DescSubWorkPattern
        /// </summary>
        public string DescSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResBaseWorkPattern
        /// </summary>
        public string ResBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResSubWorkPattern
        /// </summary>
        public string ResSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets MLRuleAccuracy
        /// </summary>
        public string MLRuleAccuracy { get; set; }
        /// <summary>
        /// Gets or sets SMEApproval
        /// </summary>
        public string SMEApproval { get; set; }
        /// <summary>
        /// Gets or sets SMEApproval
        /// </summary>
        public int TicketOccurence { get; set; }
        /// <summary>
        /// Gets or sets Classifiedby
        /// </summary>
        public string Classifiedby { get; set; }
    }

    /// <summary>
    /// This class holds DebtSampledTicketsSaveModel details
    /// </summary>
    public class DebtSampledTicketsSaveModel
    {
        /// <summary>
        /// Gets or sets ESAProjectID
        /// </summary>
        public string ESAProjectId { get; set; }
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets AdditionalText
        /// </summary>
        public string AdditionalText { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationType
        /// </summary>
        public string ApplicationType { get; set; }
        /// <summary>
        /// Gets or sets Technology
        /// </summary>
        public string Technology { get; set; }
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public string DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets MLDebtClassification
        /// </summary>
        public string MLDebtClassification { get; set; }
        /// <summary>
        /// Gets or sets MLAvoidableFlag
        /// </summary>
        public string MLAvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets MLResidualDebt
        /// </summary> 
        public string MLResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets MLCauseCode
        /// </summary>
        public string MLCauseCode { get; set; }
        /// <summary>
        /// Gets or sets MLWorkPattern
        /// </summary>
        public string MLWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets DescBaseWorkPattern
        /// </summary>
        public string DescBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets DescSubWorkPattern
        /// </summary>
        public string DescSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResBaseWorkPattern
        /// </summary>
        public string ResBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResSubWorkPattern
        /// </summary>
        public string ResSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets MLResolutionCode
        /// </summary>
        public string MLResolutionCode { get; set; }
        /// <summary>
        ///  Gets or sets Tower Name
        /// </summary>
        public string TowerName { get; set; }
    }
    /// <summary>
    /// This class holds FilePath details
    /// </summary>
    public class FilePath
    {
        /// <summary>
        /// Gets or sets OutputPath
        /// </summary>
        public string OutputPath { get; set; }
        /// <summary>
        /// Gets or sets ErrorPath
        /// </summary>
        public string ErrorPath { get; set; }
    }

    /// <summary>
    /// This class holds FilePathNoiseEl details
    /// </summary>
    public class FilePathNoiseEl
    {
        /// <summary>
        /// Gets or sets OutputPathDesc
        /// </summary>
        public string OutputPathDesc { get; set; }
        /// <summary>
        /// Gets or sets OutputPathOpt
        /// </summary>
        public string OutputPathOpt { get; set; }
        /// <summary>
        /// Gets or sets ErrorPath
        /// </summary>
        public string ErrorPath { get; set; }
        /// <summary>
        /// Gets or sets PresenceOfOptField
        /// </summary>
        public bool PresenceOfOptField { get; set; }
    }
    /// <summary>
    /// This class holds MLSamplingProcess details
    /// </summary>
    public class MLSamplingProcess
    {
        /// <summary>
        /// Gets or sets IsMLProcessingRequired
        /// </summary>
        public string IsMLProcessingRequired { get; set; }
        /// <summary>
        /// Gets or sets IsSamplingProcessingRequired
        /// </summary>
        public string IsSamplingProcessingRequired { get; set; }
        /// <summary>
        /// Gets or sets IsMLSent
        /// </summary>
        public string IsMLSent { get; set; }
        /// <summary>
        /// Gets or sets IsSamplingSent
        /// </summary>
        public string IsSamplingSent { get; set; }
        /// <summary>
        /// Gets or sets MLJobId
        /// </summary>
        public string MLJobId { get; set; }
        /// <summary>
        /// Gets or sets IsNoiseEliminationSent
        /// </summary>
        public string IsNoiseEliminationSent { get; set; }
        /// <summary>
        /// Gets or sets NoiseEliminationJobId
        /// </summary>
        public string NoiseEliminationJobId { get; set; }
        /// <summary>
        /// Gets or sets SamplingJobId
        /// </summary>
        public string SamplingJobId { get; set; }
        /// <summary>
        /// Gets or sets ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    /// <summary>
    /// This class holds GetDebtFieldsForUploadExcel details
    /// </summary>
    public class GetDebtFieldsForUploadExcel
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeValue
        /// </summary>
        public string CauseCodeValue { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeValue
        /// </summary>
        public string ResolutionCodeValue { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationValue
        /// </summary>
        public string DebtClassificationValue { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagValue
        /// </summary>
        public string AvoidableFlagValue { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtValue
        /// </summary>
        public string ResidualDebtValue { get; set; }
        /// <summary>
        /// Gets or sets OptionalFieldProj
        /// </summary>
        public string OptionalFieldProj { get; set; }
        /// <summary>
        /// Gets or sets IsTicketDescriptionUpdated
        /// </summary>
        public string IsTicketDescriptionUpdated { get; set; }
        /// <summary>
        /// Gets or sets IsTicketSummaryUpdated
        /// </summary>
        public string IsTicketSummaryUpdated { get; set; }
        /// <summary>
        /// Gets or sets TowerName
        /// </summary>
        public string TowerName { get; set; }


    }

    /// <summary>
    /// This class holds NoiseEliminationTicketDescription details
    /// </summary>
    public class NoiseEliminationTicketDescription
    {
        /// <summary>
        /// Gets or sets Keywords
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// Gets or sets Frequency
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// Gets or sets Isactive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets Isactive
        /// </summary>
        public bool IsDeleted { get; set; }
    }
    /// <summary>
    /// This class holds NoiseEliminationResolutionRemarks details
    /// </summary>
    public class NoiseEliminationResolutionRemarks
    {
        /// <summary>
        /// Gets or sets Keywords
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// Gets or sets Frequency
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// Gets or sets Isactive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets Isactive
        /// </summary>
        public bool IsDeleted { get; set; }
    }
    /// <summary>
    /// This class holds NoiseElimination details
    /// </summary>
    public class NoiseElimination
    {
        /// <summary>
        /// This is a Method Declaration of LstNoiseResolution !
        /// </summary>
        public List<NoiseEliminationResolutionRemarks> LstNoiseResolution { get; set; }
        /// <summary>
        /// This is a Method Declaration of LstNoiseTicketDescription !
        /// </summary>
        public List<NoiseEliminationTicketDescription> LstNoiseTicketDescription { get; set; }
        /// <summary>
        /// Gets or sets totaldesc
        /// </summary>
        public int TotalDesc { get; set; }
        /// <summary>
        /// Gets or sets totalopt
        /// </summary>
        public int TotalOpt { get; set; }
    }
    /// <summary>
    /// This class holds Icon Details
    /// </summary>
    public class IconDetails
    {
        /// <summary>
        /// Gets or sets TicketAnalysed
        /// </summary>
        public int TicketAnalysed { get; set; }
        /// <summary>
        /// Gets or sets TicketConsidered
        /// </summary>
        public int TicketConsidered { get; set; }
        /// <summary>
        /// Gets or sets SamplingCount
        /// </summary>
        public int SamplingCount { get; set; }
        /// <summary>
        /// Gets or sets PatternCount
        /// </summary>
        public int PatternCount { get; set; }
        /// <summary>
        /// Gets or sets ApprovedCount
        /// </summary>
        public int ApprovedCount { get; set; }
        /// <summary>
        /// Gets or sets MuteCount
        /// </summary>
        public int MuteCount { get; set; }
    }


    #region New Region for App & Infra Section
    /// <summary>
    /// ILValidationResult - Rewritten the logi of IL
    /// Once the SaveDebtTicketDetailsAfterProcessing function is called
    /// Handled few of the DB request from here itself.
    /// </summary>
    public class ILValidationResult
    {
        /// <summary>
        /// property to show the various messages in Key
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ILMessageKey { get; set; }
        /// <summary>
        /// property to show the various messages based on the business validation
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ILMessage { get; set; }
        /// <summary>
        /// ProgressbarMessage - this message is used to bind the circular progress bar message
        /// </summary>
        public ILProgressBarMessage ProgressbarMessage { get; set; }
        /// <summary>
        /// Final Result on where to stand
        /// </summary>
        public int ILValidationResultCode { get; set; }
        /// <summary>
        /// In case of any errors
        /// </summary>
        public bool IsError { get; set; }
    }
    /// <summary>
    /// Class for input parameters
    /// </summary>
    public class InputParam
    {
        /// <summary>
        /// Gets or Sets the Project ID
        /// </summary>
        public Int32 ProjectId { get; set; }
        /// <summary>
        /// Gets or Sets the SupportTypeID 
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or Sets the InitialLearningStateID
        /// </summary>
        public int InitialLearningStateId { get; set; }
        /// <summary>
        /// Gets or Sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// property to show the various messages based on the business validation
        /// </summary>
        public string ILMessage { get; set; }
        /// <summary>
        /// ProgressbarMessage - this message is used to bind the circular progress bar message
        /// </summary>
        public ILProgressBarMessage ProgressBarMessage { get; set; }
        /// <summary>
        /// Final Result on where to stand
        /// </summary>
        public int ILValidationResultCode { get; set; }
        /// <summary>
        /// In case of any errors
        /// </summary>
        public bool IsError { get; set; }
    }

    /// <summary>
    /// This class is used to bind the progrss bar message in IL screen
    /// </summary>
    public class ILProgressBarMessage
    {
        /// <summary>
        /// Data Availablity status message
        /// </summary>
        public string Level1 { get; set; }
        /// <summary>
        /// Noise message
        /// </summary>
        public string Level2 { get; set; }
        /// <summary>
        /// Sampling Message
        /// </summary>
        public string Level3 { get; set; }
        /// <summary>
        /// ML Message
        /// </summary>
        public string Level4 { get; set; }
        /// <summary>
        /// NA Message
        /// </summary>
        public string Level5 { get; set; }
    }
    /// <summary>
    /// This class holds NoiseElimination Infra details
    /// </summary>
    public class NoiseEliminationInfra
    {
        /// <summary>
        /// This is a Method Declaration of LstNoiseResolution !
        /// </summary>
        public List<NoiseEliminationResolutionRemarks> LstNoiseResolutionInfra { get; set; }
        /// <summary>
        /// This is a Method Declaration of LstNoiseTicketDescription !
        /// </summary>
        public List<NoiseEliminationTicketDescription> LstNoiseTicketDescriptionInfra { get; set; }
        /// <summary>
        /// Gets or sets totaldesc
        /// </summary>
        public int TotalDesc { get; set; }
        /// <summary>
        /// Gets or sets totalopt
        /// </summary>
        public int TotalOpt { get; set; }
    }

    /// <summary>
    /// RegenerateILDetails
    /// </summary>
    public class RegenerateILDetails
    {
        /// <summary>
        /// SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// LstApplication
        /// </summary>
        public List<RegenerateAVMDetails> LstApplication { get; set; }
        /// <summary>
        /// LstTower
        /// </summary>
        public List<RegenerateInfraDetails> LstTower { get; set; }
    }
    /// <summary>
    /// RegenerateAVMDetails
    /// </summary>
    public class RegenerateAVMDetails
    {
        /// <summary>
        /// ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
    /// <summary>
    /// RegenerateInfraDetails
    /// </summary>
    public class RegenerateInfraDetails
    {
        /// <summary>
        /// TowerID
        /// </summary>
        public Int64 TowerId { get; set; }
        /// <summary>
        /// TowerName
        /// </summary>
        public string TowerName { get; set; }
    }
    /// <summary>
    /// DebtValidation
    /// </summary>
    public class DebtValidation
    {
        /// <summary>
        /// AssociateID
        /// </summary>
        [MaxLength(100)]
        public string AssociateId { get; set; }
        /// <summary>
        /// EmployeeId
        /// </summary>
        [MaxLength(100)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Choose
        /// </summary>
        public int Choose { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>
        public Int32 ProjectId { get; set; }
        /// <summary>
        /// DateFrom
        /// </summary>
        [MaxLength(500)]
        public string DateFrom { get; set; }
        /// <summary>
        /// DateTo
        /// </summary>
        [MaxLength(500)]
        public string DateTo { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [MaxLength(100)]
        public string UserId { get; set; }
        /// <summary>
        /// IsSMTicket
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string IsSMTicket { get; set; }
        /// <summary>
        /// IsDARTTicket
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string IsDARTTicket { get; set; }
        /// <summary>
        /// OptFieldProjID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string OptFieldProjId { get; set; }
        /// <summary>
        /// OptfieldProj
        /// </summary>
        public Int16 OptFieldProj { get; set; }
        /// <summary>
        /// SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// TicketConsidered
        /// </summary>
        public int TicketConsidered { get; set; }
        /// <summary>
        /// TicketAnalysed
        /// </summary>
        public int TicketAnalysed { get; set; }
        /// <summary>
        /// SamplingCount
        /// </summary>
        public int SamplingCount { get; set; }
        /// <summary>
        /// PatternCount
        /// </summary>
        public int PatternCount { get; set; }
        /// <summary>
        /// ApproveCount
        /// </summary>
        public int ApproveCount { get; set; }
        /// <summary>
        /// MuteCount
        /// </summary>
        public int MuteCount { get; set; }
    }
    #endregion
}
