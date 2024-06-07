using System;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds InsertAttributeModel details
    /// </summary>
    public class InsertAttributeModel
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        [MaxLength(200)]
        public string TicketId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets TicketOpenDate
        /// </summary>
        public DateTime TicketOpenDate { get; set; }
        /// <summary>
        /// Gets or sets Serviceid
        /// </summary>
        public int ServiceId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets TicketStatusID
        /// </summary>
        [MaxLength(50)]
        public string TicketStatusId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId
        {
            get;
            set;
        }

        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Priority
        /// </summary>
        public Int64 Priority
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Severity
        /// </summary>
        public Int64 Severity
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Assignedto
        /// </summary>
        [MaxLength(400)]
        public string AssignedTo
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Onoff
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string OnOff
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Source
        /// </summary>
        public Int64 Source
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets ReleaseType
        /// </summary>
        public Int64 ReleaseType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedEffort
        /// </summary>
        public decimal? PlannedEffort
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets EstimatedWorkSize
        /// </summary>
        public decimal? EstimatedWorkSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ActualWorkSize
        /// </summary>
        public Int32 ActualWorkSize
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualEffort
        /// </summary>
        public decimal ActualEffort { get; set; }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Resolvedby
        /// </summary>
        [MaxLength(200)]
        public string ResolvedBy
        {
            get;
            set;
        }
        //// [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Ticketcreatedate
        /// </summary>
        public DateTime? TicketCreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Causecode
        /// </summary>
        public Int64 CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public Int64 ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets PlannedStartDateandTime
        /// </summary>
        public DateTime? PlannedStartDateAndTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedEndDate
        /// </summary>
        public DateTime? PlannedEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualStartdateTime
        /// </summary>
        public DateTime? ActualStartDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualEnddateTime
        /// </summary>
        public DateTime? ActualEndDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ReopenDate
        /// </summary>
        public DateTime? ReopenDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Resolveddate
        /// </summary>
        public DateTime? ResolvedDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets RejectedTimeStamp
        /// </summary>
        public DateTime? RejectedTimeStamp
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CloseDate
        /// </summary>
        public DateTime? CloseDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Closedby
        /// </summary>
        [MaxLength(200)]
        public string Closedby
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ReleaseDate
        /// </summary>
        public DateTime? ReleaseDate
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets KEDBAvailableIndicator
        /// </summary>
        public Int64 KEDBAvailableIndicator
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets KEDBUpdated
        /// </summary>
        public Int64 KEDBUpdated
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets ElevateFlagInternal
        /// </summary>
        [MaxLength(8)]
        public string ElevateFlagInternal
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets RCAID
        /// </summary>
        [MaxLength(100)]
        public string RCAID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedDuration
        /// </summary>
        public decimal? PlannedDuration
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualDuration
        /// </summary>
        public decimal? ActualDuration
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets MetResponseSLA
        /// </summary>
        [MaxLength(400)]
        public string MetResponseSLA
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets MetAcknowledgementSLA
        /// </summary>
        [MaxLength(400)]
        public string MetAcknowledgementSLA
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets MetResolution
        /// </summary>
        [MaxLength(400)]
        public string MetResolution
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string TicketDescription
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets TicketSummary
        /// </summary>
        [MaxLength(4000)]
        
        public string TicketSummary
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets NatureoftheTicket
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string NatureOfTheTicket
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public Int64 Application
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        [MaxLength(4000)]
        public string Comments
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets RepeatedIncident
        /// </summary>
        [MaxLength(200)]
        public string RepeatedIncident
        {
            get;
            set;
        }
        //// [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        ////// <summary>
        /// Gets or sets RelatedTickets
        /// </summary>
        [MaxLength(200)]
        public string RelatedTickets
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets TicketCreatedBy
        /// </summary>
        [MaxLength(10000)]
        public string TicketCreatedBy
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets KEDBPath
        /// </summary>
        [MaxLength(1000)]
        public string KEDBPath
        {
            get;
            set;
        }
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets EscalatedFalgCustomer
        /// </summary>
        [MaxLength(200)]
        public string EscalatedFlagCustomer
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets ApprovedBy
        /// </summary>
        [MaxLength(400)]
        public string ApprovedBy
        {
            get;
            set;
        }
        
        //[RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets ReasonforRejection
        /// </summary>
        [MaxLength(4000)]
        public string ReasonForRejection
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets StartedDateTime
        /// </summary>
        public DateTime? StartedDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets WIPDateTime
        /// </summary>
        public DateTime? WIPDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets OnHoldDateTime
        /// </summary>
        public DateTime? OnHoldDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CompletedDateTime
        /// </summary>
        public DateTime? CompletedDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CancelledDateTime
        /// </summary>
        public DateTime? CancelledDateTime
        {
            get;
            set;
        }
        //   [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets SecondaryResources
        /// </summary>
        [MaxLength(200)]
        public string SecondaryResources
        {
            get;
            set;
        }
        
        //  [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets OutageDuration
        /// </summary>
        [MaxLength(100)]
        public string OutageDuration
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets AssignedTimeStamp
        /// </summary>
        public DateTime? AssignedTimeStamp
        {
            get;
            set;
        }
        // [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(50)]
        public string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public int DebtClassificationId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets AvoidalFlagId
        /// </summary>
        public int AvoidalFlagId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ResidualDebtId
        /// </summary>
        public int ResidualDebtId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Resolutionmethod
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string ResolutionMethod
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField1
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField1
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField2
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField2
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField3
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField3
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField4
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField4
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Category
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Category
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Type
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets IsDescriptionUpdated
        /// </summary>
        [MaxLength(100)]
        public string IsDescriptionUpdated
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets IsTicketSummaryUpdated
        /// </summary>
        [MaxLength(100)]
        public string IsTicketSummaryUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Is Partially Automated
        /// </summary>
        /// 
        public Int32 IsPartiallyAutomated
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set the Business Impact
        /// </summary>
        public Int16 AHBusinessImpact
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Impact comment
        /// </summary>
        public string AHImpactComments
        {
            get;
            set;
        }

    }
    public class SaveAttributeModel
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        [MaxLength(200)]
        public string TicketID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets serviceid
        /// </summary>
        public int serviceid
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets projectId
        /// </summary>
        public Int64 projectId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Priority
        /// </summary>
        public Int64 Priority
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Severity
        /// </summary>
        public Int64 Severity
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Assignedto
        /// </summary>
        [MaxLength(400)]
        public string Assignedto
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ReleaseType
        /// </summary>
        public Int64 ReleaseType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets EstimatedWorkSize
        /// </summary>
        public decimal? EstimatedWorkSize
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Ticketcreatedate
        /// </summary>
        public DateTime? Ticketcreatedate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualStartdateTime
        /// </summary>
        public DateTime? ActualStartdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ActualEnddateTime
        /// </summary>
        public DateTime? ActualEnddateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ReopenDate
        /// </summary>
        public DateTime? ReopenDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CloseDate
        /// </summary>
        public DateTime? CloseDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets KEDBAvailableIndicator
        /// </summary>
        public Int64 KEDBAvailableIndicator
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets KEDBUpdatedAdded
        /// </summary>
        public Int64 KEDBUpdatedAdded
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets MetResponseSLA
        /// </summary>
        [MaxLength(400)]
        public string MetResponseSLA
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets MetResolution
        /// </summary>
        [MaxLength(400)]
        public string MetResolution
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string TicketDescription
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public Int64 Application
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets KEDBPath
        /// </summary>
        [MaxLength(1000)]
        public string KEDBPath
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CompletedDateTime
        /// </summary>
        public DateTime? CompletedDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public Int64 ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public int DebtClassificationId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets Resolutionmethod
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string Resolutionmethod
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public Int64 CauseCode { get; set; }
        /// <summary>
        /// Gets or sets TicketOpenDate
        /// </summary>
        public DateTime TicketOpenDate { get; set; }
        /// <summary>
        /// Gets or sets ActualEffort
        /// </summary>
        public decimal ActualEffort { get; set; }
        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        [MaxLength(4000)]
        public string Comments
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedEffort
        /// </summary>
        public decimal? PlannedEffort
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedEndDate
        /// </summary>
        public DateTime? PlannedEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets PlannedStartDate
        /// </summary>
        public DateTime? PlannedStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets RCAID
        /// </summary>
        [MaxLength(100)]
        public string RCAID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ReleaseDate
        /// </summary>
        public DateTime? ReleaseDate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets TicketSummary
        /// </summary>
        [MaxLength(4000)]
        public string TicketSummary
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public int AvoidableFlag
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ResidualDebtId
        /// </summary>
        public int ResidualDebtId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets TicketSource
        /// </summary>
        public Int64 TicketSource
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField1
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField1
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField2
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField2
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField3
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField3
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets FlexField4
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FlexField4
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Is Partially Automated
        /// </summary>
        /// 
        public Int32 IsPartiallyAutomated
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Business Impact
        /// </summary>
        public Int16 AHBusinessImpact
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Impact comment
        /// </summary>
        public string AHImpactComments
        {
            get;
            set;
        }


    }
}
