// <copyright file="SearchTicketDetails.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    /// <summary>
    /// The SearchTicketDetails Class holds Project, Application, Hierarchy and Ticket Details
    /// </summary>
    public class SearchTicketDetails
    {
        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the Ticket ID.
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// Gets or sets the Application Name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the Tower Name.
        /// </summary>
        public string TowerName { get; set; }
        

        /// <summary>
        /// Gets or sets the Business Cluster 1 Name.
        /// </summary>
        public string Hierarchy1 { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster 2 Name.
        /// </summary>
        public string Hierarchy2 { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster 3 Name.
        /// </summary>
        public string Hierarchy3 { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster 4 Name.
        /// </summary>
        public string Hierarchy4 { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster 5 Name.
        /// </summary>
        public string Hierarchy5 { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster 6 Name.
        /// </summary>
        public string Hierarchy6 { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Description.
        /// </summary>
        public string TicketDescription { get; set; }

        /// <summary>
        /// Gets or sets the Service.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the Service Group.
        /// </summary>
        public string ServiceGroup { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Type.
        /// </summary>
        public string TicketType { get; set; }

        /// <summary>
        /// Gets or sets the Cause Code.
        /// </summary>
        public string CauseCode { get; set; }

        /// <summary>
        /// Gets or sets the Resolution Code.
        /// </summary>
        public string ResolutionCode { get; set; }

        /// <summary>
        /// Gets or sets the Avoidable Flag.
        /// </summary>
        public string AvoidableFlag { get; set; }

        /// <summary>
        /// Gets or sets the Priority.
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the AppLens Status.
        /// </summary>
        public string AppLensStatus { get; set; }

        /// <summary>
        /// Gets or sets the Effort Till Date.
        /// </summary>
        public string EffortTillDate { get; set; }

        /// <summary>
        /// Gets or sets the Insertion Mode.
        /// </summary>
        public string InsertionMode { get; set; }

        /// <summary>
        /// Gets or sets the Data Entry Complete.
        /// </summary>
        public string DataEntryComplete { get; set; }

        /// <summary>
        /// Gets or sets the Open Date.
        /// </summary>
        public string OpenDate { get; set; }

        /// <summary>
        /// Gets or sets the Closed Date.
        /// </summary>
        public string ClosedDate { get; set; }

        /// <summary>
        /// Gets or sets the Primary Assignee.
        /// </summary>
        public string Assignee { get; set; }
        
        /// <summary>
        /// Gets or sets the Reopen Date.
        /// </summary>
        public string ReopenDate { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the Severity.
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// Gets or sets the Debt Classification.
        /// </summary>
        public string DebtClassification { get; set; }

        /// <summary>
        /// Gets or sets the Release Type.
        /// </summary>
        public string ReleaseType { get; set; }

        /// <summary>
        /// Gets or sets the Planned Effort.
        /// </summary>
        public string PlannedEffort { get; set; }

        /// <summary>
        /// Gets or sets the Estimated Work Size.
        /// </summary>
        public string EstimatedWorkSize { get; set; }

        /// <summary>
        /// Gets or sets the Actual Work Size.
        /// </summary>
        public string ActualWorkSize { get; set; }

        /// <summary>
        /// Gets or sets the Planned Start DateTime.
        /// </summary>
        public string PlannedStartDate { get; set; }

        /// <summary>
        /// Gets or sets the Planned End DateTime.
        /// </summary>
        public string PlannedEndDate { get; set; }

        /// <summary>
        /// Gets or sets the New Status DateTime.
        /// </summary>
        public string NewStatusDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Rejected TimeStamp.
        /// </summary>
        public string RejectedTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the KEDB Available Indicator.
        /// </summary>
        public string KEDBAvailableIndicator { get; set; }

        /// <summary>
        /// Gets or sets the KEDB Updated.
        /// </summary>
        public string KEDBUpdated { get; set; }

        /// <summary>
        /// Gets or sets the Elevate Flag Internal.
        /// </summary>
        public string ElevateFlagInternal { get; set; }

        /// <summary>
        /// Gets or sets the RCA ID.
        /// </summary>
        public string RCAID { get; set; }

        /// <summary>
        /// Gets or sets the Met Response SLA.
        /// </summary>
        public string MetResponseSLA { get; set; }

        /// <summary>
        /// Gets or sets the Met Acknowledgement SLA.
        /// </summary>
        public string MetAcknowledgementSLA { get; set; }

        /// <summary>
        /// Gets or sets the Met Resolution.
        /// </summary>
        public string MetResolution { get; set; }

        /// <summary>
        /// Gets or sets the Actual Start DateTime.
        /// </summary>
        public string ActualStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Actual End DateTime.
        /// </summary>
        public string ActualEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Actual Duration.
        /// </summary>
        public string ActualDuration { get; set; }

        /// <summary>
        /// Gets or sets the Nature Of The Ticket.
        /// </summary>
        public string NatureOfTheTicket { get; set; }

        /// <summary>
        /// Gets or sets the Comments.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the Repeated Incident.
        /// </summary>
        public string RepeatedIncident { get; set; }

        /// <summary>
        /// Gets or sets the Related Tickets.
        /// </summary>
        public string RelatedTickets { get; set; }

        /// <summary>
        /// Gets or sets the KEDB Path.
        /// </summary>
        public string KEDBPath { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Created By.
        /// </summary>
        public string TicketCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the Escalated Flag Customer.
        /// </summary>
        public string EscalatedFlagCustomer { get; set; }

        /// <summary>
        /// Gets or sets the Approved By.
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the Started DateTime.
        /// </summary>
        public string StartedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Work Inprogress Date Time.
        /// </summary>
        public string WIPDateTime { get; set; }

        /// <summary>
        /// Gets or sets the OnHold DateTime.
        /// </summary>
        public string OnHoldDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Completed DateTime.
        /// </summary>
        public string CompletedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Cancelled DateTime.
        /// </summary>
        public string CancelledDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Outage Duration.
        /// </summary>
        public string OutageDuration { get; set; }

        /// <summary>
        /// Gets or sets the Residual Debt.
        /// </summary>
        public string ResidualDebt { get; set; }

        /// <summary>
        /// Gets or sets the Resolution Remarks.
        /// </summary>
        public string ResolutionRemarks { get; set; }

        /// <summary>
        /// FlexField1
        /// </summary>
        public string FlexField1 { get; set; }

        /// <summary>
        /// FlexField2
        /// </summary>
        public string FlexField2 { get; set; }

        /// <summary>
        /// FlexField3
        /// </summary>
        public string FlexField3 { get; set; }

        /// <summary>
        /// FlexField4
        /// </summary>
        public string FlexField4 { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ExternalLogin ID
        /// </summary>
        public string ClientUserId { get; set; }
    }
}
