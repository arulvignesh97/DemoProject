using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds ApplicationModel details
    /// </summary>
    public class ApplicationModel
    {
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        [MaxLength(400)]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets ConflictPatternsCount
        /// </summary>
        public int ConflictPatternsCount { get; set; }

        /// <summary>
        /// Gets or sets EsaProjectId
        /// </summary>
        [MaxLength(200)]
        public string EsaProjectId { get; set; }
    }
    /// <summary>
    /// This class holds TicketDetailsModel details
    /// </summary>
    public class TicketDetailsModel
    {
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets IsMainSpringConfig
        /// </summary>
        public string IsMainSpringConfig { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEnabled
        /// </summary>
        public string IsDebtEnabled { get; set; }
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeId
        /// </summary>
        public int TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or sets CategoryID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// Gets or sets EffortTillDate
        /// </summary>
        public decimal EffortTillDate { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        public decimal ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets TimesheetDate
        /// </summary>
        public DateTime TimesheetDate { get; set; }
        /// <summary>
        /// Gets or sets EffortHours
        /// </summary>
        public decimal EffortHours { get; set; }
        /// <summary>
        /// Gets or sets IsNonTicket
        /// </summary>
        public string IsNonTicket { get; set; }
        /// <summary>
        /// Gets or sets IsAttributeUpdated
        /// </summary>
        public string IsAttributeUpdated { get; set; }
        /// <summary>
        /// Gets or sets TimesheetId
        /// </summary>
        public string TimesheetId { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailId
        /// </summary>
        public string TimeSheetDetailId { get; set; }

    }
    /// <summary>
    /// This class holds ProjectModel details
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// gets or sets
        /// </summary>
        public string UserTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string ProjectTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        public string ProjectTimeZoneName { get; set; }
    }
    /// <summary>
    /// This class holds PriorityModel details
    /// </summary>
    public class PriorityModel
    {
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public int PriorityId { get; set; }
        /// <summary>
        /// Gets or sets PriorityName
        /// </summary>
        public string PriorityName { get; set; }
    }
    /// <summary>
    /// This class holds AssignmentGroupModel details
    /// </summary>
    public class AssignmentGroupModel
    {
        /// <summary>
        /// Gets or sets AssignmentGroupMapID
        /// </summary>
        public Int64 AssignmentGroupMapId { get; set; }
        /// <summary>
        /// Gets or sets AssignmentGroupName
        /// </summary>
        public string AssignmentGroupName { get; set; }
    }
    /// <summary>
    /// This class holds ChooseSearchTicketDetailsModel details
    /// </summary>
    public class ChooseSearchTicketDetailsModel
    {
        /// <summary>
        /// Gets or sets TicketNumber
        /// </summary>
        public string TicketNumber { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public int TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets AssigneeID
        /// </summary>
        public string AssigneeId { get; set; }
        /// <summary>
        /// Gets or sets AssigneeName
        /// </summary>
        public string AssigneeName { get; set; }

        /// <summary>
        /// Gets or sets AssignmentGroupID
        /// </summary>
        public string AssignmentGroupId { get; set; }
        /// <summary>
        /// Gets or sets AssignmentGroupName
        /// </summary>
        public string AssignmentGroupName { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Gets or sets Dart status id
        /// </summary>
        public int DartStatusId { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets OpenDateTime
        /// </summary>
        public string OpenDateTime { get; set; }
        /// <summary>
        /// Gets or sets Closeddate
        /// </summary>
        public string Closeddate { get; set; }
        /// <summary>
        /// gets tower details
        /// </summary>
        public int TowerId { get; set; }

        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets IsDARTTicket
        /// </summary>
        public string IsDARTTicket { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets CategoryID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// Gets or sets EffortTillDate
        /// </summary>
        public decimal EffortTillDate { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        public decimal ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets IsMainSpringConfig
        /// </summary>
        public string IsMainSpringConfig { get; set; }
        /// <summary>
        /// Gets or sets TicketCreateDate
        /// </summary>
        public string TicketCreateDate { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEnabled
        /// </summary>
        public string IsDebtEnabled { get; set; }
        /// <summary>
        /// Gets or sets IsAttributeUpdated
        /// </summary>
        public bool IsAttributeUpdated { get; set; }
        /// <summary>
        /// Get or set SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Get or set SupportTypeName
        /// </summary>
        public string SupportTypeName { get; set; }
    }

    public class GridDataRequest
    {
        public int Projectid { get; set; }
        public List<ApplicationIDs> ApplicationIDs { get; set; }
    }
    /// <summary>
    /// This class holds Griddata details
    /// </summary>
    public class Griddata
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public int AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public int ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ExpectedCompletiondate
        /// </summary>
        public string ExpectedCompletiondate { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public int ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionName
        /// </summary>
        public string ResolutionName { get; set; }
        /// <summary>
        /// Gets or sets CauseID
        /// </summary>
        public int CauseId { get; set; }
        /// <summary>
        /// Gets or sets CauseName
        /// </summary>
        public string CauseName { get; set; }
        /// <summary>
        /// Gets or sets DebtID
        /// </summary>
        public int DebtId { get; set; }
        /// <summary>
        /// Gets or sets DebtName
        /// </summary>
        public string DebtName { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtName
        /// </summary>
        public string ResidualDebtName { get; set; }
        /// <summary>
        /// Gets or sets ResidualID
        /// </summary>
        public int ResidualId { get; set; }
        /// <summary>
        /// Gets or sets ResidualName
        /// </summary>
        public string ResidualName { get; set; }
        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
    }
    /// <summary>
    /// This class holds GriddataList details
    /// </summary>
    public class GriddataList
    {
        /// <summary>
        /// Gets or sets griddata
        /// </summary>
        public List<Griddata> Griddata { get; set; }
        /// <summary>
        /// Gets or sets ModifiedDate
        /// </summary>
        public string ModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets ModifiedDate
        /// </summary>
        public string EffectiveDate { get; set; }
        /// <summary>
        /// Gets or sets ApplicationCount
        /// </summary>
        public string ApplicationCount { get; set; }

    }
    /// <summary>
    /// This class hold parameters for ChooseTicketdetails method
    /// </summary>
    public class ChooseTicket
    {
        /// <summary>
        /// Gets or sets Work Item Flag
        /// </summary> 
        public char WorkItemFlag { get; set; }
        /// <summary>
        /// Gets or sets CloseDateBegin
        /// </summary>
        [MaxLength(500)]
        public string CloseDateBegin { get; set; }
        /// <summary>
        /// Gets or sets CloseDateEnd
        /// </summary>
        [MaxLength(500)]
        public string CloseDateEnd { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets TicketIDDesc
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string TicketIdDesc { get; set; }
        /// <summary>
        /// Gets or sets AssignedTo
        /// </summary>
        [MaxLength(50)]
        public string AssignedTo { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        [MaxLength(10000)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets AssignmentgroupIds
        /// </summary>
        [MaxLength(10000)]
        public string AssignmentgroupIds { get; set; }
        /// <summary>
        /// Gets or sets TowerID
        /// </summary>
        [MaxLength(40000)]
        public string TowerId { get; set; }
        /// <summary>
        /// Gets or sets CreateDateBegin
        /// </summary>
        [MaxLength(500)]
        public string CreateDateBegin { get; set; }
        /// <summary>
        /// Gets or sets CreateDateEnd
        /// </summary>
        [MaxLength(500)]
        public string CreateDateEnd { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        [MaxLength(1000)]
        public string StatusId { get; set; }
        /// <summary>
        /// Gets or sets IsDARTTicket
        /// </summary>
        public Int64 IsDARTTicket { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets AssigneeID
        /// </summary>
        [MaxLength(1000)]
        public string AssigneeId { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        [MaxLength(400)]
        public string ProjectTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        [MaxLength(400)]
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets Work Item ID
        /// </summary>
        [MaxLength(100)]
        public string WorkItemId { get; set; }
        /// <summary>
        /// Gets or sets CreatedDateFrom
        /// </summary>
        [MaxLength(50)]
        public string CreatedDateFrom { get; set; }
        /// <summary>
        /// Gets or sets CreatedDateTo
        /// </summary>
        [MaxLength(50)]
        public string CreatedDateTo { get; set; }
        /// <summary>
        /// Gets or Sets PageNo
        /// </summary>
        public Int32 PageNo { get; set; }
        /// <summary>
        /// Gets or Sets PageSize
        /// </summary>
        public Int32 PageSize { get; set; }
    }
    /// <summary>
    /// This class holds Work Item Details 
    /// </summary>
    public class WorkItemDetails
    {
        /// <summary>
        /// Get sor Sets WorkItemID
        /// </summary>
        [MaxLength(50)]
        public string WorkItemId { get; set; }
        /// <summary>
        /// Get sor Sets Description
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }
        /// <summary>
        /// Get sor Sets Application
        /// </summary>
        [MaxLength(500)]
        public string Application { get; set; }
        /// <summary>
        /// Get sor Sets Assignee
        /// </summary>
        [MaxLength(50)]
        public string Assignee { get; set; }
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; }
        /// <summary>
        /// Get sor Sets CreatedDate
        /// </summary>
        [MaxLength(50)]
        public string CreatedDate { get; set; }

    }
    /// <summary>
    /// This class holds the ChooseWorkItem
    /// </summary>
    public class ChooseWorkItem
    {
        /// <summary>
        /// Gets or sets lstWorkItemDetails
        /// </summary>
        public List<WorkItemDetails> lstWorkItemDetails { get; set; }
        /// <summary>
        ///  Gets or sets PageSize
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets TotalRowCount
        /// </summary>
        public int? TotalRowCount { get; set; }
    }

}



