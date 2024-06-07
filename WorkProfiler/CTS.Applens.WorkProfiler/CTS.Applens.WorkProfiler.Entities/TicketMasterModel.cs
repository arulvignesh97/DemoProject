namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds TicketMasterModel details
    /// </summary>
    public class TicketMasterModel
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
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
    }
    /// <summary>
    /// This class holds TicketDescriptionDetails
    /// </summary>
    public class TicketDescriptionDetails
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
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
    /// <summary>
    /// This is used the value for TicketDescriptionSummary
    /// </summary>
    public class TicketDescriptionSummary
    {
        /// <summary>
        ///Gets or Sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        ///Gets or Sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        ///Gets or Sets TicketSummary
        /// </summary>
        public string TicketSummary { get; set; }
        /// <summary>
        ///Gets or Sets SupportType
        /// </summary>
        public int SupportType { get; set; }
        /// <summary>
        ///Gets or Sets FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        ///Gets or Sets FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        ///Gets or Sets FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        ///Gets or Sets FlexField4
        /// </summary>
        public string FlexField4 { get; set; }
    }
    /// <summary>
    /// TicketDetail
    /// </summary>
    public class TicketDetail
    {
        /// <summary>
        /// Get or Set TicketID
        /// </summary>
        public string TicketID { get; set; }
        /// <summary>
        /// Get or Set TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Get or Set OpenDate
        /// </summary>
        public string OpenDate { get; set; }
        /// <summary>
        /// Get or Set Priority
        /// </summary>
        public string Priority { get; set; }
        /// <summary>
        /// Get or Set Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Get or Set Application
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// Get or Set ExternalLoginID
        /// </summary>
        public string ExternalLoginID { get; set; }
        /// <summary>
        /// Get or Set Assignee
        /// </summary>
        public string Assignee { get; set; }
        /// <summary>
        /// Get or Set ModifiedDateTime
        /// </summary>
        public string ModifiedDateTime { get; set; }
        /// <summary>
        /// Get or Set ReopenDate
        /// </summary>
        public string ReopenDate { get; set; }
        /// <summary>
        /// Get or Set TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Get or Set CloseDate
        /// </summary>
        public string CloseDate { get; set; }
        /// <summary>
        /// Get or Set Ticketsource
        /// </summary>
        public string Ticketsource { get; set; }
        /// <summary>
        /// Get or Set SecAssignee
        /// </summary>
        public string SecAssignee { get; set; }
        /// <summary>
        /// Get or Set PlannedEndDate
        /// </summary>
        public string PlannedEndDate { get; set; }
        /// <summary>
        /// Get or Set Severity
        /// </summary>
        public string Severity { get; set; }
        /// <summary>
        /// Get or Set ReleaseType
        /// </summary>
        public string ReleaseType { get; set; }
        /// <summary>
        /// Get or Set PlannedEffort
        /// </summary>
        public string PlannedEffort { get; set; }
        /// <summary>
        /// Get or Set EstimatedWorkSize
        /// </summary>
        public string EstimatedWorkSize { get; set; }
        /// <summary>
        /// Get or Set ActualWorkSize
        /// </summary>
        public string ActualWorkSize { get; set; }
        /// <summary>
        /// Get or Set PlannedStartDateandTime
        /// </summary>
        public string PlannedStartDateandTime  { get; set; }
        /// <summary>
        /// Get or Set RejectedTimeStamp
        /// </summary>
        public string RejectedTimeStamp { get; set; }
        /// <summary>
        /// Get or Set KEDBAvailableIndicator
        /// </summary>
        public string KEDBAvailableIndicator { get; set; }
        /// <summary>
        /// Get or Set KEDBupdated
        /// </summary>
        public string KEDBupdated { get; set; }
        /// <summary>
        /// Get or Set ElevateFlagInternal
        /// </summary>
        public string ElevateFlagInternal { get; set; }
        /// <summary>
        /// Get or Set RCAID
        /// </summary>
        public string RCAID { get; set; }
        /// <summary>
        /// Get or Set MetResponseSLA
        /// </summary>
        public string MetResponseSLA { get; set; }
        /// <summary>
        /// Get or Set MetAcknowledgementSLA
        /// </summary>
        public string MetAcknowledgementSLA { get; set; }
        /// <summary>
        /// Get or Set MetResolution
        /// </summary>
        public string MetResolution { get; set; }
        /// <summary>
        /// Get or Set Resolvedby
        /// </summary>
        public string Resolvedby { get; set; }
        /// <summary>
        /// Get or Set ActualStartdateTime
        /// </summary>
        public string ActualStartdateTime { get; set; }
        /// <summary>
        /// Get or Set ActualEnddateTime
        /// </summary>
        public string ActualEnddateTime { get; set; }
        /// <summary>
        /// Get or Set PlannedDuration
        /// </summary>
        public string PlannedDuration { get; set; }
        /// <summary>
        /// Get or Set Actualduration
        /// </summary>
        public string Actualduration { get; set; }
        /// <summary>
        /// Get or Set TicketSummary
        /// </summary>
        public string TicketSummary { get; set; }
        /// <summary>
        /// Get or Set NatureOfTheTicket
        /// </summary>
        public string NatureOfTheTicket { get; set; }
        /// <summary>
        /// Get or Set Comments
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Get or Set RepeatedIncident
        /// </summary>
        public string RepeatedIncident { get; set; }
        /// <summary>
        /// Get or Set RelatedTickets
        /// </summary>
        public string RelatedTickets { get; set; }
        /// <summary>
        /// Get or Set TicketCreatedBy
        /// </summary>
        public string TicketCreatedBy { get; set; }
        /// <summary>
        /// Get or Set KEDBPath
        /// </summary>
        public string KEDBPath { get; set; }
        /// <summary>
        /// Get or Set EscalatedFlagCustomer
        /// </summary>
        public string EscalatedFlagCustomer { get; set; }
        /// <summary>
        /// Get or Set ApprovedBy
        /// </summary>
        public string ApprovedBy { get; set; }
        /// <summary>
        /// Get or Set ReasonForRejection
        /// </summary>
        public string ReasonForRejection { get; set; }
        /// <summary>
        /// Get or Set StartedDateTime
        /// </summary>
        public string StartedDateTime { get; set; }
        /// <summary>
        /// Get or Set WIPDateTime
        /// </summary>
        public string WIPDateTime { get; set; }
        /// <summary>
        /// Get or Set OnHoldDateTime
        /// </summary>
        public string OnHoldDateTime { get; set; }
        /// <summary>
        /// Get or Set CompletedDateTime
        /// </summary>
        public string CompletedDateTime { get; set; }
        /// <summary>
        /// Get or Set CancelledDateTime
        /// </summary>
        public string CancelledDateTime { get; set; }
        /// <summary>
        /// Get or Set OutageDuration
        /// </summary>
        public string OutageDuration { get; set; }
        /// <summary>
        /// Get or Set AssignedTimeStamp
        /// </summary>
        public string AssignedTimeStamp { get; set; }
        /// <summary>
        /// Get or Set DebtClassification
        /// </summary>
        public string DebtClassification { get; set; }
        /// <summary>
        /// Get or Set AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Get or Set ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Get or Set ResolutionRemarks
        /// </summary>
        public string ResolutionRemarks { get; set; }
        /// <summary>
        /// Get or Set Causecode
        /// </summary>
        public string Causecode { get; set; }
        /// <summary>
        /// Get or Set ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Get or Set ITSMEffort
        /// </summary>
        public string ITSMEffort { get; set; }
        /// <summary>
        /// Get or Set AssignmentGroup
        /// </summary>
        public string AssignmentGroup { get; set; }
        /// <summary>
        /// Get or Set ExpectedCompletionDate
        /// </summary>
        public string ExpectedCompletionDate { get; set; }
        /// <summary>
        /// Get or Set ReasonforResidual
        /// </summary>
        public string ReasonforResidual { get; set; }
        /// <summary>
        /// Get or Set FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        /// Get or Set FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        /// Get or Set FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        /// Get or Set FlexField4
        /// </summary>
        public string FlexField4 { get; set; }
        /// <summary>
        /// Get or Set Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Get or Set Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get or Set EmployeeID
        /// </summary>
        public string EmployeeID { get; set; }
        /// <summary>
        /// Get or Set EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// Get or Set TicketUploadTrackID
        /// </summary>
        public string TicketUploadTrackID { get; set; }
        /// <summary>
        /// Get or Set IsTicketDescriptionModified
        /// </summary>
        public string IsTicketDescriptionModified { get; set; } = "0";
        /// <summary>
        /// Get or Set IsTicketSummaryModified
        /// </summary>
        public string IsTicketSummaryModified { get; set; } = "0";
        /// <summary>
        /// Get or Set Tower
        /// </summary>
        public string Tower { get; set; }
        /// <summary>
        /// Get or Set IsPartiallyAutomated
        /// </summary>
        public string IsPartiallyAutomated { get; set; } = "0";

        /// <summary>
        /// Get or Set TicketDescriptionBasePattern
        /// </summary>
        public string TicketDescriptionBasePattern { get;set;}

        /// <summary>
        /// Get or Set TicketDescriptionSubPattern
        /// </summary>
        public string TicketDescriptionSubPattern { get; set; }

        /// <summary>
        /// Get or Set ResolutionRemarksBasePattern
        /// </summary>
        public string ResolutionRemarksBasePattern { get; set; }

        /// <summary>
        /// Get or Set ResolutionRemarksSubPattern
        /// </summary>
        public string ResolutionRemarksSubPattern { get; set; }

        /// <summary>
        /// Get or Set ServiceName
        /// </summary>
        public string ServiceName { get; set; }

    }

    /// <summary>
    /// ColumnMappting
    /// </summary>
    public class ColumnMappting
    {
        /// <summary>
        /// Gets or Sets ServiceDartColumn
        /// </summary>
        public string ServiceDartColumn { get; set; }
        /// <summary>
        /// Gets or Sets ProjectColumn
        /// </summary>
        public string ProjectColumn { get; set; }
    }
}
