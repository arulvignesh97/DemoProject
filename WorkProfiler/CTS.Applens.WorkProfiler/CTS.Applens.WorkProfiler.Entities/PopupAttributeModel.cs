using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds details
    /// </summary>
    public class PopupAttributeModel : GracePeriodDetails
    {
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        [MaxLength(400)]
        /// <summary>
        /// Declaration of Application list
        /// </summary>
        public List<ApplicationProjectModel> ApplicationList { get; set; }
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets TowerName
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or sets SeverityID
        /// </summary>
        public string SeverityId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public string DARTStatusId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusName
        /// </summary>
        public string DARTStatusName { get; set; }
        /// <summary>
        /// Gets or sets TicketStatusID
        /// </summary>
        public string TicketStatusId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public string TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public string CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeId
        /// </summary>
        public string ResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public string DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets TicketSourceID
        /// </summary>
        public string TicketSourceId { get; set; }
        /// <summary>
        /// Gets or sets ReleaseTypeId
        /// </summary>
        public string ReleaseTypeId { get; set; }
        /// <summary>
        /// Gets or sets kedbUpdateId
        /// </summary>
        public string KedbUpdateId { get; set; }
        /// <summary>
        /// Gets or sets KedbAvailableId
        /// </summary>
        public string KedbAvailableId { get; set; }
        /// <summary>
        /// Gets or sets PriotityID
        /// </summary>
        public string PriotityId { get; set; }

        /// <summary>
        ///Get or Set the Business Impact ID
        /// </summary>
        public int BusinessImpactId { get; set; }

        /// <summary>
        /// Get or Set the Imapct Comments
        /// </summary>
        public string ImpactComments { get; set; }
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketOpenDate
        /// </summary>
        public System.DateTime TicketOpenDate { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets Priority
        /// </summary>
        public string Priority { get; set; }
        /// <summary>
        /// Gets or sets NatureoftheTicket
        /// </summary>
        public string NatureoftheTicket { get; set; }
        /// <summary>
        /// Gets or sets ApplicationId
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public string ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionMethodID
        /// </summary>
        public string ResolutionMethodId { get; set; }
        /// <summary>
        /// Gets or sets MetResponseSLAID
        /// </summary>
        public string MetResponseSLAId { get; set; }
        /// <summary>
        /// Gets or sets MetAcknowledgementSLAID
        /// </summary>
        public string MetAcknowledgementSLAId { get; set; }
        /// <summary>
        /// Gets or sets MetResolutionID
        /// </summary>
        public string MetResolutionId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtType
        /// </summary>
        public string DebtType { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets Severity
        /// </summary>
        public string Severity { get; set; }
        /// <summary>
        /// Gets or sets AssignedTo
        /// </summary>
        public string AssignedTo { get; set; }
        /// <summary>
        /// Gets or sets TicketSource
        /// </summary>
        public string TicketSource { get; set; }
        /// <summary>
        /// Gets or sets ReleaseType
        /// </summary>
        public string ReleaseType { get; set; }
        /// <summary>
        /// Gets or sets EstimatedWorkSize
        /// </summary>
        public string EstimatedWorkSize { get; set; }
        /// <summary>
        /// Gets or sets CurrentDate
        /// </summary>
        public string CurrentDate { get; set; }
        /// <summary>
        /// Gets or sets ActualEffort
        /// </summary>
        public decimal ActualEffort { get; set; }
        /// <summary>
        /// Gets or sets TicketCreatedDate
        /// </summary>
        public System.DateTime TicketCreatedDate { get; set; }
        /// <summary>
        /// Gets or sets ActualStartDateTime
        /// </summary>
        public System.DateTime ActualStartDateTime { get; set; }
        /// <summary>
        /// Gets or sets ActualEndtDateTime
        /// </summary>
        public System.DateTime ActualEndtDateTime { get; set; }

        /// <summary>
        /// Gets or sets ClosedDate
        /// </summary>
        public System.DateTime ClosedDate { get; set; }
        /// <summary>
        /// Gets or sets KEDBUpdated
        /// </summary>
        public string KEDBUpdated { get; set; }
        /// <summary>
        /// Gets or sets KedbAvailable
        /// </summary>
        public string KedbAvailable { get; set; }
        /// <summary>
        /// Gets or sets KEDBPath
        /// </summary>
        public string KEDBPath { get; set; }
        /// <summary>
        /// Gets or sets RCAID
        /// </summary>
        public string RCAId { get; set; }
        /// <summary>
        /// Gets or sets MetResponseSLA
        /// </summary>
        public string MetResponseSLA { get; set; }
        /// <summary>
        /// Gets or sets MetAcknowledgementSLA
        /// </summary>
        public string MetAcknowledgementSLA { get; set; }
        /// <summary>
        /// Gets or sets MetResolution
        /// </summary>
        public string MetResolution { get; set; }
        /// <summary>
        /// Gets or sets OpenDateTime
        /// </summary>
        public System.DateTime OpenDateTime { get; set; }
        /// <summary>
        /// Gets or sets WIPDateTime
        /// </summary>
        public System.DateTime WIPDateTime { get; set; }
        /// <summary>
        /// Gets or sets CompletedDateTime
        /// </summary>
        public System.DateTime CompletedDateTime { get; set; }
        /// <summary>
        /// Gets or sets ReopenDateTime
        /// </summary>
        public System.DateTime ReopenDateTime { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public string StatusId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionMethodName
        /// </summary>
        public string ResolutionMethodName { get; set; }
        /// <summary>
        /// Gets or sets CustomerTimeZone
        /// </summary>
        public string CustomerTimeZone { get; set; }
        /// <summary>
        /// Gets or sets IsMainSpringConfig
        /// </summary>
        public string IsMainSpringConfig { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEnabled
        /// </summary>
        public string IsDebtEnabled { get; set; }
        /// <summary>
        /// This is a Declaration of LstPriority !
        /// </summary>
        public List<LstPriorityModel> LstPriority { get; set; }
        /// <summary>
        /// This is a Declaration of LstCause !
        /// </summary>
        public List<LstCauseCode> LstCause { get; set; }
        /// <summary>
        /// This is a Declaration of LstResolution !
        /// </summary>
        public List<LstResolution> LstResolution { get; set; }
        /// <summary>
        /// This is a Declaration of LstDebtClassification !
        /// </summary>
        public List<LstDebtClassification> LstDebtClassification { get; set; }
        /// <summary>
        /// This is a Declaration of LstSeverity !
        /// </summary>
        public List<LstSeverity> LstSeverity { get; set; }
        /// <summary>
        /// This is a Declaration of LstTicketSource !
        /// </summary>
        public List<LstTicketSource> LstTicketSource { get; set; }
        /// <summary>
        /// This is a Declaration of LstReleaseType !
        /// </summary>
        public List<LstReleaseType> LstReleaseType { get; set; }
        /// <summary>
        /// This is a Declaration of LstKEDBUpdated !
        /// </summary>
        public List<LstKEDBUpdated> LstKEDBUpdated { get; set; }
        /// <summary>
        /// This is a Declaration of LstkedbAvailable !
        /// </summary>
        public List<LstkedbAvailable> LstKEDBAvailable { get; set; }
        /// <summary>
        /// This is a Declaration of LstTicketType !
        /// </summary>
        public List<LstTicketType> LstTicketType { get; set; }
        /* Addtional Columns */

        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Gets or sets PlannedEffort
        /// </summary>
        public string PlannedEffort { get; set; }
        /// <summary>
        /// Gets or sets PlannedEndDate
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets PlannedStartDate
        /// </summary>
        public DateTime PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets ReleaseDate
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Gets or sets TicketCreatedBy
        /// </summary>
        public string TicketCreatedBy { get; set; }
        /// <summary>
        /// Gets or sets TicketStatus
        /// </summary>
        public string TicketStatus { get; set; }
        /// <summary>
        /// Gets or sets TicketSummary
        /// </summary>
        public string TicketSummary { get; set; }
        /// <summary>
        /// Gets or sets FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        /// Gets or sets FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        /// Gets or sets FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        /// Gets or sets FlexField4
        /// </summary>
        public string FlexField4 { get; set; }
        /// <summary>
        /// Gets or sets Category
        /// </summary>
        public string Category { get; set; }
        /// Gets or sets Debt Classification Mode
        /// </summary>
        public string DebtClassificationMode { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }

        /// <summary>
        /// This is a Declaration of LstAvoidableFlagModel !
        /// </summary>
        public List<AvoidableFlag> LstAvoidableFlagModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstResidualDebtModel !
        /// </summary>
        public List<ResidualDebt> LstResidualDebtModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstEscalatedFlagCustomerModel !
        /// </summary>
        public List<EscalatedFlagCustomerModel> LstEscalatedFlagCustomerModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstOutageFlagModel !
        /// </summary>
        public List<OutageFlagModel> LstOutageFlagModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstWarrantyIssueModel !
        /// </summary>
        public List<WarrantyIssueModel> LstWarrantyIssueModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstNatureoftheticketModel !
        /// </summary>
        public List<NatureoftheticketModel> LstNatureoftheticketModel { get; set; }
        /// <summary>
        /// This is a Declaration of LstMetSLA !
        /// </summary>
        public List<LstMetSLA> LstMetSLA { get; set; }
        /// <summary>
        /// Gets or sets OptionalAttributeType
        /// </summary>
        public int OptionalAttributeType { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField1Configured
        /// </summary>
        public bool IsFlexField1Configured { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField2Configured
        /// </summary>
        public bool IsFlexField2Configured { get; set; }
        /// <summary>
        ///  Gets or sets IsFlexField3Configured
        /// </summary>
        public bool IsFlexField3Configured { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField4Configured
        /// </summary>
        public bool IsFlexField4Configured { get; set; }
        /// <summary>
        /// Gets or sets ClosedDate by Project Time zone
        /// </summary>
        public DateTime? ClosedDateProject { get; set; }
        /// <summary>
        /// Gets or sets CompletedDate by Project Time zone
        /// </summary>
        public DateTime? CompletedDateProject { get; set; }
        /// <summary>
        ///  Gets or sets IsResolutionReConfigured
        /// </summary>
        public bool IsResolutionReConfigured { get; set; }

        /// <summary>
        /// Gets or sets IsPartiallyAutomated
        /// </summary>
        public Int16 IsPartiallyAutomated { get; set; }

        /// <summary>
        /// Gets or sets AutoClassificationAppType
        /// </summary>
        public short AutoClassificationType { get; set; }
        public List<GetBusinessImpact> LstBusinessImpact { get; set; }
        /// <summary>
        /// Gets or sets AssignedUserID
        /// </summary>
        public string AssignedUserID { get; set; }
    }
    /// <summary>
    /// This class holds AttributeTypes details
    /// </summary>
    public class AttributeTypes
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets AttributeTypeValue
        /// </summary>
        public string AttributeTypeValue { get; set; }
    }
    /// <summary>
    /// This class holds ResidualDebt details
    /// </summary>
    public class ResidualDebt
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds LstMetSLA details
    /// </summary>
    public class LstMetSLA
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds LstkedbAvailable details
    /// </summary>
    public class LstkedbAvailable
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets KEDBAvailableName
        /// </summary>
        public string KEDBAvailableName { get; set; }
    }
    /// <summary>
    /// This class holds LstTicketType details
    /// </summary>
    public class LstTicketType
    {
        /// <summary>
        /// Gets or sets TicTypeId
        /// </summary>
        public string TicTypeId { get; set; }
        /// <summary>
        /// Gets or sets TicTypeName
        /// </summary>
        public string TicTypeName { get; set; }
    }
    /// <summary>
    /// This class holds LstPriorityModel details
    /// </summary>
    public class LstPriorityModel
    {
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public string PriorityId { get; set; }
        /// <summary>
        /// Gets or sets PriorityName
        /// </summary>
        public string PriorityName { get; set; }
    }
    /// <summary>
    /// This class holds LstCauseCode details
    /// </summary>
    public class LstCauseCode
    {
        /// <summary>
        /// Gets or sets CauseID
        /// </summary>
        public string CauseId { get; set; }
        /// <summary>
        /// Gets or sets CauseName
        /// </summary>
        public string CauseName { get; set; }

        /// <summary>
        /// Gets or sets CauseMapCount
        /// </summary>
        public Int32 CauseMapCount { get; set; }
        /// <summary>
        /// Gets or sets CauseIsMapped
        /// </summary>
        public string CauseIsMapped { get; set; }
    }
    /// <summary>
    /// This class holds LstResolution details
    /// </summary>
    public class LstResolution
    {
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public string ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionName
        /// </summary>
        public string ResolutionName { get; set; }
        /// <summary>
        /// Gets or sets IsMapped
        /// </summary>
        public string IsMapped { get; set; }
        /// <summary>
        /// Gets or sets MapCount
        /// </summary>
        public Int32 MapCount { get; set; }
    }
    /// <summary>
    /// This class holds LstDebtClassification details
    /// </summary>
    public class LstDebtClassification
    {
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public string DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationName
        /// </summary>
        public string DebtClassificationName { get; set; }
    }
    /// <summary>
    /// This class holds LstSeverity details
    /// </summary>
    public class LstSeverity
    {
        /// <summary>
        /// Gets or sets SeverityID
        /// </summary>
        public string SeverityId { get; set; }
        /// <summary>
        /// Gets or sets SeverityName
        /// </summary>
        public string SeverityName { get; set; }
    }
    /// <summary>
    /// This class holds LstTicketSource details
    /// </summary>
    public class LstTicketSource
    {
        /// <summary>
        /// Gets or sets TicketSourceID
        /// </summary>
        public string TicketSourceId { get; set; }
        /// <summary>
        /// Gets or sets TicketSourceName
        /// </summary>
        public string TicketSourceName { get; set; }
    }
    /// <summary>
    /// This class holds LstReleaseType details
    /// </summary>
    public class LstReleaseType
    {
        /// <summary>
        /// Gets or sets ReleaseTypeID
        /// </summary>
        public string ReleaseTypeId { get; set; }
        /// <summary>
        /// Gets or sets ReleaseTypeName 
        /// </summary>
        public string ReleaseTypeName { get; set; }
    }
    /// <summary>
    /// Gets BusinessImpact
    /// </summary>
    public class GetBusinessImpact
    {
        /// <summary>
        /// Gets or sets BusinessImpactId
        /// </summary>
        public Int16 BusinessImpactId { get; set; }
        /// <summary>
        /// Gets or sets BusinessImpactName
        /// </summary>
        [MaxLength(20)]
        public string BusinessImpactName { get; set; }
    }
    /// <summary>
    /// This class holds LstKEDBUpdated details
    /// </summary>
    public class LstKEDBUpdated
    {
        /// <summary>
        /// Gets or sets KEDBUpdatedID
        /// </summary>
        public string KEDBUpdatedId { get; set; }
        /// <summary>
        /// Gets or sets KEDBUpdatedName
        /// </summary>
        public string KEDBUpdatedName { get; set; }
    }
    /// <summary>
    /// This class holds EscalatedFlagCustomerModel details
    /// </summary>
    public class EscalatedFlagCustomerModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds NatureoftheticketModel details
    /// </summary>
    public class NatureoftheticketModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds OutageFlagModel details
    /// </summary>
    public class OutageFlagModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds WarrantyIssueModel details
    /// </summary>
    public class WarrantyIssueModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds AvoidableFlag details
    /// </summary>
    public class AvoidableFlag
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// This class holds TicketAttributesModel details
    /// </summary>
    public class TicketAttributesModel
    {
        ///// <summary>
        ///// Gets or sets ProjectID
        ///// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceId
        /// </summary>
        public Int64 ServiceId { get; set; }
        /// <summary>
        /// Gets or sets AttributeName
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string AttributeName { get; set; }
        /// <summary>
        /// Gets or sets ColumnMappingName
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ColumnMappingName { get; set; }
        /// <summary>
        /// Gets or sets ProjectStatusID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ProjectStatusId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public Int32 DARTStatusId { get; set; }

        /// <summary>
        /// Gets or sets AttributeType
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string AttributeType { get; set; }
        /// <summary>
        /// Gets or sets TicketStatusID
        /// </summary>
        public Int32 TicketStatusID { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public Int64 TicketTypeID { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeId  
        /// </summary>
        public int SupportTypeId { get; set; }

        /// <summary>
        /// Gets or sets IsCognizant  
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsAttributeUpdated  
        /// </summary>
        [MaxLength(100)]
        public string IsAttributeUpdated { get; set; }
    }
    /// <summary>
    /// This class holds CauseCodeResolutionCode method's parameter
    /// </summary>
    public class CauseCodeResolutionCode
    {

        public int classificationType { get; set; }

        /// <summary>
        /// Gets or sets TowerID  
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID  
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets TowerName
        /// </summary>
        [MaxLength(400)]
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public int CauseCode { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public int ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [MaxLength(50)]
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        [MaxLength(400)]
        public string Application { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        [MaxLength(200)]
        public string ServiceId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        [MaxLength(200)]
        public string TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        [MaxLength(200)]
        public string TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionMethodName
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string ResolutionMethodName { get; set; }
        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        [MaxLength(2000)]
        public string Comments { get; set; }
        /// <summary>
        /// Gets or sets TicketSummary
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string TicketSummary { get; set; }
        /// <summary>      
        /// This declartion for Debt Classification Mode
        /// </summary>
        [MaxLength(200)]
        public string DebtClassificationMode { get; set; }
        /// <summary>      
        /// This declartion for AutoClassification Type
        /// </summary>        
        public int AutoClassificationType { get; set; }

    }
    /// <summary>
    /// this holds parameter for Popupattribute method
    /// </summary>
    public class Popupattributeget : GracePeriodDetails
    {
        ///// <summary>
        ///// Gets or sets ProjectID
        ///// </summary>
        public int ProjectId { get; set; }
        ///// <summary>
        ///// Gets or sets TicketID
        ///// </summary>
        [MaxLength(200)]
        public string TicketId { get; set; }
        ///// <summary>
        ///// Gets or sets ServiceID
        ///// </summary>
        [MaxLength(200)]
        public string ServiceId { get; set; }
        ///// <summary>
        ///// Gets or sets StatusID
        ///// </summary>
        public int StatusId { get; set; }
        ///// <summary>
        ///// Gets or sets StatusName
        ///// </summary>
        [MaxLength(200)]
        public string StatusName { get; set; }
        ///// <summary>
        ///// Gets or sets TicketTypeId
        ///// </summary>
        [MaxLength(200)]
        public string TicketTypeId { get; set; }
        ///// <summary>
        ///// Gets or sets UserID
        ///// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        ///// <summary>
        ///// Gets or sets CustomerTimeZone
        ///// </summary>
        [MaxLength(200)]
        public string CustomerTimeZone { get; set; }
        ///// <summary>
        ///// Gets or sets IsDebtEnabled
        ///// </summary>
        [MaxLength(200)]
        public string IsDebtEnabled { get; set; }
        ///// <summary>
        ///// Gets or sets IsMainSpring
        ///// </summary>
        [MaxLength(200)]
        public string IsMainSpring { get; set; }
        ///// <summary>
        ///// Gets or sets RowID
        ///// </summary>
        [MaxLength(int.MaxValue)]
        public string RowId { get; set; }
        ///// <summary>
        ///// Gets or sets IsAttributeUpdated
        ///// </summary>
        [MaxLength(100)]
        public string IsAttributeUpdated { get; set; }
        ///// <summary>
        ///// Gets or sets ProjectTimeZoneName
        ///// </summary>
        [MaxLength(400)]
        public string ProjectTimeZoneName { get; set; }
        ///// <summary>
        ///// Gets or sets UserTimeZoneName
        ///// </summary>
        [MaxLength(400)]
        public string UserTimeZoneName { get; set; }
        ///// <summary>
        ///// Gets or sets SupportTypeID
        ///// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or Sets Employee ID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or Set Customer ID
        /// </summary>
        public long CustomerId { get; set; }
    }

    public class CauseCodeResolutionCodeNewAlgo : CauseCodeResolutionCode
    {
        [DataType(DataType.Text)]
        public string jsonParam { get; set; }
    }

    public class NewAlgoColumnList
    {
        public string AlgoKey { get; set; }
        public int TransactionId { get; set; }
        public List<string> ColumnList { get; set; }
    }
}
