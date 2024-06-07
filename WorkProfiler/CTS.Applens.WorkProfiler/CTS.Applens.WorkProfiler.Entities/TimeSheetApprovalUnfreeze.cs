using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds TimeSheetDataDaily details
    /// </summary>
    public class TimeSheetDataDaily
    {
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate
        /// </summary>
        public DateTime TimeSheetDate { get; set; }
        /// <summary>
        /// Gets or sets TotalHours
        /// </summary>
        public decimal TotalHours { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetStatus
        /// </summary>
        public string TimeSheetStatus { get; set; }
        /// <summary>
        /// Gets or sets TimesheetStatusId
        /// </summary>
        public int TimesheetStatusId { get; set; }
        /// <summary>
        /// Gets or sets TimesheetId
        /// </summary>
        public Int64 TimesheetId { get; set; }
        /// <summary>
        /// Gets or sets ProjectId
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets IsFreezed
        /// </summary>
        public bool IsFreezed { get; set; }
        /// <summary>
        /// Gets or sets RejectionComments
        /// </summary>
        public string RejectionComments { get; set; }
        /// <summary>
        /// Gets or sets IsApprove
        /// </summary>
        public bool IsApprove { get; set; }
        /// <summary>
        /// Gets or sets IsUnfreeze
        /// </summary>
        public bool IsUnfreeze { get; set; }
    }

    public class TimeSheetDataExcel
    {
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetStatus
        /// </summary>
        public string TimeSheetStatus { get; set; }
        /// <summary>
        /// Gets or sets TotalHours
        /// </summary>
        public decimal TotalHours { get; set; }
        /// <summary>
        /// Gets or sets RejectionCommects
        /// </summary>
        public string RejectionCommects { get; set; }

        /// <summary>
        /// Gets or sets TotalHours1
        /// </summary>
        public decimal TotalHours1 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours2
        /// </summary>
        public decimal TotalHours2 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours3
        /// </summary>
        public decimal TotalHours3 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours4
        /// </summary>
        public decimal TotalHours4 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours5
        /// </summary>
        public decimal TotalHours5 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours6
        /// </summary>
        public decimal TotalHours6 { get; set; }

        /// <summary>
        /// Gets or sets TotalHours7
        /// </summary>
        public decimal TotalHours7 { get; set; }
        
        /// <summary>
        /// Gets or sets IsFreezed
        /// </summary>
        public bool IsFreezed { get; set; }

    }

    public class DateHead
    {
        public DateTime Date1 { get; set; }

        public DateTime Date2 { get; set; }

        public DateTime Date3 { get; set; }

        public DateTime Date4 { get; set; }

        public DateTime Date5 { get; set; }

        public DateTime Date6 { get; set; }

        public DateTime Date7 { get; set; }
    }

    /// <summary>
    /// This class holds TimeSheetData details
    /// </summary>
    public class TimeSheetData
    {
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetStatus
        /// </summary>
        public string TimeSheetStatus { get; set; }
        /// <summary>
        /// Gets or sets TotalHours
        /// </summary>
        public decimal TotalHours { get; set; }
        /// <summary>
        /// Gets or sets RejectionCommects
        /// </summary>
        public string RejectionCommects { get; set; }
        /// <summary>
        /// Gets or sets DailyTimeSheetData
        /// </summary>
        public List<TimeSheetDataDaily> DailyTimeSheetData { get; set; }
        /// <summary>
        /// Gets or sets IsFreezed
        /// </summary>
        public bool IsFreezed { get; set; }
        /// <summary>
        /// Gets or sets IsApprove
        /// </summary>
        public bool IsApprove { get; set; }
        /// <summary>
        /// Gets or sets IsUnfreeze
        /// </summary>
        public bool IsUnfreeze { get; set; }

    }
    /// <summary>
    /// This Class is used to pass the Input parameters to all the Stored Procedures
    /// </summary>
    public class ApprovalUnfreezeInputParams
    {        
        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        [MaxLength(500)]
        public string StartDate { get; set; }
        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        [MaxLength(500)]
        public string EndDate { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets AssignessOrDefaultersID
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string AssignessOrDefaultersID { get; set; }
        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        public int CustomerId { get; set; }        
        /// <summary>
        /// Gets or sets IsDaily
        /// </summary>
        public int IsDaily { get; set; }
        /// <summary>
        /// Gets or sets DefaulterId
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string DefaulterId { get; set; }
    }
    /// <summary>
    /// This class holds ApprovalUnfreezeInputParams2 details
    /// </summary>
    public class ApprovalUnfreezeInputParams2
    {
        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        [MaxLength(500)]
        public string StartDate { get; set; }
        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        [MaxLength(500)]
        public string EndDate { get; set; }
        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
    }
    /// <summary>
    /// This class holds ApproveUnfreeze Data 
    /// </summary>
    public class ApproveUnfreeze
    {
        /// <summary>
        /// Gets or sets AssignessOrDefaulters
        /// </summary>
        public List<AssignessOrDefaulters> AssignessOrDefaulters { get; set; }
        /// <summary>
        /// Gets or sets UnfreezeGracePeriod
        /// </summary>
        public string UnfreezeGracePeriod { get; set; }
    }
    /// <summary>
    /// This class holds AssignessOrDefaulters details
    /// </summary>
    public class AssignessOrDefaulters
    {
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets IsDefaulter
        /// </summary>
        public string IsDefaulter { get; set; }
    }
    /// <summary>
    /// This class holds ApproveUnfreezeTimesheet details
    /// </summary>
    public class ApproveUnfreezeTimesheet
    {
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate
        /// </summary>
        public DateTime TimeSheetDate { get; set; }
        /// <summary>
        /// Gets or sets TimesheetId
        /// </summary>
        public Int64 TimesheetId { get; set; }
        /// <summary>
        /// Gets or sets IsApproval
        /// </summary>
        public bool IsApproval { get; set; }
        /// <summary>
        /// Gets or sets StatusId
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Gets or sets SubmitterId
        /// </summary>
        public string SubmitterId { get; set; }
        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
    }
    /// <summary>
    /// This class holds CalendarViewData details
    /// </summary>
    public class CalendarViewData
    {
        public string DateValue { get; set; }

        public string DayValue { get; set; }
        /// <summary>
        /// Gets or sets TimesheetStatus
        /// </summary>
        public string TimesheetStatus { get; set; }
        /// <summary>
        /// Gets or sets TimesheetStatusID
        /// </summary>
        public int TimesheetStatusId { get; set; }
        /// <summary>
        /// Gets or sets ResultValue
        /// </summary>
        public int ResultValue { get; set; }
    }
    
    /// <summary>
    /// This class holds TicketDetails
    /// </summary>
    public class TicketDetails
    {
        /// <summary>
        /// Gets or sets TicketId
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets Service
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Gets or sets Activity
        /// </summary>
        public string Activity { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        public int? ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets EffortTillDate
        /// </summary>
        public decimal EffortTillDate { get; set; }
        /// <summary>
        /// Gets or sets MarkAsDataEntry
        /// </summary>
        public bool MarkAsDataEntry { get; set; }
        /// <summary>
        /// Gets or sets ProjectId
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets Tower
        /// </summary>
        public string Tower { get; set; }
        /// <summary>
        /// Gets or sets TimesheetDate
        /// </summary>
        public DateTime TimesheetDate { get; set; }
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public string DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public int IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsEfforTracked
        /// </summary>
        public bool IsEfforTracked { get; set; }
        /// <summary>
        /// Gets or sets IsITSMLinked
        /// </summary>
        public bool IsITSMLinked { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEnabled
        /// </summary>
        public bool IsDebtEnabled { get; set; }
        /// <summary>
        /// Gets or sets IsAcitivityTracked
        /// </summary>
        public bool IsAcitivityTracked { get; set; }
        /// <summary>
        /// Gets or sets IsMainspringConfigured
        /// </summary>
        public bool IsMainspringConfigured { get; set; }
       
    }
    /// <summary>
    /// Hold Employee Id details
    /// </summary>
    public class TimesheetDetails
    {
        /// <summary>
        /// Gets or Sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
    }

    /// <summary>
    /// UserTicketDetail
    /// </summary>
    public class UserTicketDetail
    {
        /// <summary>
        /// Gets or Sets CustomerID
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// Gets or Sets FromDate
        /// </summary>
        [MaxLength(100)]
        public string FromDate { get; set; }
        /// <summary>
        /// Gets or Sets ToDate
        /// </summary>
        [MaxLength(100)] 
        public string ToDate { get; set; }
        /// <summary>
        /// Gets or Sets SubmitterId
        /// </summary>
        [MaxLength(100)] 
        public string SubmitterId { get; set; }
        /// <summary>
        /// Gets or Sets IsCognizant
        /// </summary>
        public bool IsCognizant { get; set; }
        /// <summary>
        /// Gets or Sets IsADMApplicableforCustomer
        /// </summary>
        public bool IsADMApplicableforCustomer { get; set; }
        /// <summary>
        /// Gets or Sets TsApproverId
        /// </summary>
        [MaxLength(100)]
        public string TsApproverId { get; set; }
    }

}
