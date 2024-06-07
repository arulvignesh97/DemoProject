using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds ErrorLogCorrection details
    /// </summary>
    public class ErrorLogCorrection
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public int? TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets Assignee
        /// </summary>
        public string Assignee { get; set; }
        /// <summary>
        /// Gets or sets OpenDate
        /// </summary>
        public DateTime OpenDate { get; set; }
        /// <summary>
        /// Gets or sets OpenDate2
        /// </summary>
        public string OpenDate2 { get; set; }
        /// <summary>
        /// Gets or sets Priority
        /// </summary>
        public string Priority { get; set; }
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public int? PriorityId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public int? ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string MTicketDescription { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public bool IsTicketDescriptionModified { get; set; }
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// Gets or sets IsManual
        /// </summary>
        public int IsManual { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int? ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets ExternalLoginID
        /// </summary>
        public string ExternalLoginId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets severityID
        /// </summary>
        public int? SeverityId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public int? AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public int? DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public int? ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public int? CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets IsDeleted
        /// </summary>
        public byte IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets Severity
        /// </summary>
        public string Severity { get; set; }
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
        /// Gets or sets Causecode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// AssignmentGroupID
        /// </summary>
        public int AssignmentGroupId { get; set; }
        /// <summary>
        /// TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// TowerName
        /// </summary>
        public string Tower { get; set; }
        /// <summary>
        /// AssignmentGroup
        /// </summary>
        public string AssignmentGroup { get; set; }

        /// <summary>
        /// Check for A/H Ticket
        /// </summary>
        public bool IsAHTicket { get; set; }

        /// <summary>
        /// Gets or sets Is Partially Automated
        /// </summary>
        public Int32 IsPartiallyAutomated { get; set; }

    }
    /// <summary>
    /// This class holds DebtEnabledFields details
    /// </summary>
    public class DebtEnabledFields
    {
        /// <summary>
        /// Gets or sets isDebtEnabled
        /// </summary>
        public bool IsDebtEnabled { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public bool ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public bool DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public bool AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public bool ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets Causecode
        /// </summary>
        public bool Causecode { get; set; }
    }
    public class SaveErrorCorrectionModel
    {
        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        [DataType(DataType.Text)]
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [DataType(DataType.Text)]
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [DataType(DataType.Text)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        [DataType(DataType.Text)]
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets ticketDetails
        /// </summary>
        [DataType(DataType.Text)]
        public string TicketDetails { get; set; }
    }
}
