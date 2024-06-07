using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{

    /// <summary>
    /// TicketIDSupport details
    /// </summary>
    public class TicketIDSupport
    {
        /// <summary>
        /// Get or Set TicketID
        /// </summary>

        public string TicketId { get; set; }
        /// <summary>
        /// Get or Set SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>

        public char Type { get; set; }
        /// <summary>
        /// Gets or Sets Work Item Id
        /// </summary>

        public string WorkItemId { get; set; }


    }
    /// <summary>
    /// This class holds SelectedTicketInput details
    /// </summary>
    public class SelectedTicketInput
    {
        /// <summary>
        /// Gets or Sets CustomerID
        /// </summary>
        [MaxLength(100)]
        public  string CustomerID { get; set; }
        /// <summary>
        /// Gets or Sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeID { get; set; }
        /// <summary>
        /// Gets or Sets FirstDateOfWeek
        /// </summary>
        [MaxLength(50)]
        public string FirstDateOfWeek { get; set; }
        /// <summary>
        /// Gets or Sets LastDateOfWeek
        /// </summary>
        [MaxLength(50)]
        public string LastDateOfWeek { get; set; }
        /// <summary>
        /// Gets or Sets Mode
        /// </summary>
        [MaxLength(100)]
        public string Mode { get; set; }
        /// <summary>
        /// Gets or Sets TicketID_Desc
        /// </summary>
        public List<TicketIDSupport> TicketID_Desc { get; set; }
        /// <summary>
        /// Gets or Sets TProjectID
        /// </summary>
        [MaxLength(100)]
        public string ProjectID { get; set; }
        /// <summary>
        /// Gets or Sets isCognizant
        /// </summary>
        public int? isCognizant { get; set; }
    }
    public class TimeSheetModel
    {
        /// <summary>
        /// Gets or sets lstWeekDays
        /// </summary>
        public List<WeekDays> LstWeekDays { get; set; }
        /// <summary>
        /// Gets or sets lstOverallTicketDetails
        /// </summary>
        public List<OverallTicketDetails> LstOverallTicketDetails { get; set; }
        /// <summary>
        /// Gets or sets lstTimeSheetDetails
        /// </summary>
        public List<TimeSheetDetails> LstTimeSheetDetails { get; set; }
        /// <summary>
        /// Gets or sets lstServiceDetails
        /// </summary>
        public List<ServiceDetails> LstServiceDetails { get; set; }
        /// <summary>
        /// Gets or sets lstServiceActivityDetails
        /// </summary>
        public List<ServiceActivityDetails> LstServiceActivityDetails { get; set; }
        /// <summary>
        /// Gets or sets lstTicketTypeDetails
        /// </summary>
        public List<TicketTypeDetails> LstTicketTypeDetails { get; set; }
        /// <summary>
        /// Gets or sets lstStatusDetails
        /// </summary>
        public List<StatusDetails> LstStatusDetails { get; set; }
        /// <summary>
        /// Gets or sets lstCustomerDetails
        /// </summary>
        public List<CustomerDetails> LstCustomerDetails { get; set; }
        /// <summary>
        /// Gets or sets lstTicketTypeServiceDetails
        /// </summary>
        public List<TicketTypeServiceDetails> LstTicketTypeServiceDetails { get; set; }
        /// <summary>
        /// Gets or sets lstUserLevelDetails
        /// </summary>
        public List<UserLevelDetails> LstUserLevelDetails { get; set; }
        ///Gets or Sets lstTowerDetails
        public List<TaskDetails> LstTaskDetails { get; set; }
        /// <summary>
        /// Gets or Sets lstApplicableServices
        /// </summary>
        public List<int> LstApplicableServices { get; set; }
        /// <summary>
        /// Gets or Sets lstBUBenchMarkDetails
        /// </summary>
        public List<BenchMarkDetailsModel> LstBUBenchMarkDetails { get; set; }
        /// <summary>
        /// Gets or Sets lstOrgBenchMarkDetails
        /// </summary>
        public List<BenchMarkDetailsModel> LstOrgBenchMarkDetails { get; set; }
        /// <summary>
        /// Gets or sets LstADMStatusDetails
        /// </summary>
        public List<StatusDetails> LstADMStatusDetails { get; set; }
        /// <summary>
        ///  Gets or sets LstADMMasterStatusDetails
        /// </summary>
        public List<NameIDModel> LstADMMasterStatusDetails { get; set; }
        /// <summary>
        ///  Gets or sets LstALMConfiguredDetails
        /// </summary>
        public List<ALMConfiguredDetails> LstALMConfiguredDetails { get; set; }
    }
    /// <summary>
    /// This class holds UserLevelDetails
    /// </summary>
    public class UserLevelDetails
    {
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceLevelID
        /// </summary>
        public Int32 ServiceLevelId { get; set; }
    }
    /// <summary>
    /// This class holds WeekDays details
    /// </summary>
    public class WeekDays
    {
        /// <summary>
        /// Gets or sets Date
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Gets or sets Day
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// Gets or sets DisplayDate
        /// </summary>
        public string DisplayDate { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus
        /// </summary>
        public string FreezeStatus { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public Int64 StatusId { get; set; }
    }
    /// <summary>
    /// This class holds OverallTicketDetails
    /// </summary>
    public class OverallTicketDetails : GracePeriodDetails
    {
        /// <summary>
        /// Gets or sets AssignmentGroup
        /// </summary>
        public int AssignmentGroupId{ get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        public Int64? TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        [MaxLength(100)]
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64? ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets AssignedTo
        /// </summary>
        [MaxLength(500)]
        public string AssignedTo { get; set; }
        /// <summary>
        /// Gets or sets EffortTillDate
        /// </summary>
        [MaxLength(500)]
        public string EffortTillDate { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int64? ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public Int64? ActivityId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets IsDeleted
        /// </summary>
        public int IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets TicketStatusMapID
        /// </summary>
        public Int64? TicketStatusMapId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeMapID
        /// </summary>
        public Int64? TicketTypeMapId { get; set; }
        /// <summary>
        /// Gets or sets IsSDTicket
        /// </summary>
        [MaxLength(50)]
        public string IsSDTicket { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public Int64? DARTStatusId { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        [MaxLength(500)]
        public string ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets IsNonTicket
        /// </summary>
        [MaxLength(50)]
        public string IsNonTicket { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        [MaxLength(50)]
        public string IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsEffortTracked
        /// </summary>
        [MaxLength(50)]
        public string IsEffortTracked { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEnabled
        /// </summary>
        [MaxLength(50)]
        public string IsDebtEnabled { get; set; }
        /// <summary>
        /// Gets or sets IsActivityTracked
        /// </summary>
        [MaxLength(50)]
        public string IsActivityTracked { get; set; }
        /// <summary>
        /// Gets or sets IsMainspringConfigured
        /// </summary>
        [MaxLength(50)]
        public string IsMainspringConfigured { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        [MaxLength(500)]
        public string ProjectTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        [MaxLength(500)]
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets ISTicket
        /// </summary>
        [MaxLength(50)]
        public string ISTicket { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID1
        /// </summary>
        public Int64? TimeSheetId1 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate1
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate1 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID1
        /// </summary>
        public Int64? TimeSheetDetailId1 { get; set; }
        /// <summary>
        /// Gets or sets Effort1
        /// </summary>
        [MaxLength(100)]
        public string Effort1 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus1
        /// </summary>
        public bool FreezeStatus1 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID2
        /// </summary>
        public Int64? TimeSheetId2 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate2
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate2 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID2
        /// </summary>
        public Int64? TimeSheetDetailId2 { get; set; }
        /// <summary>
        /// Gets or sets Effort2
        /// </summary>
        [MaxLength(100)]
        public string Effort2 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus2
        /// </summary>
        public bool FreezeStatus2 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID3
        /// </summary>
        public Int64? TimeSheetId3 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate3
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate3 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID3
        /// </summary>
        public Int64? TimeSheetDetailId3 { get; set; }
        /// <summary>
        /// Gets or sets Effort3
        /// </summary>
        [MaxLength(100)]
        public string Effort3 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus3
        /// </summary>
        public bool FreezeStatus3 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID4
        /// </summary>
        public Int64? TimeSheetID4 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate4
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate4 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID4
        /// </summary>
        public Int64? TimeSheetDetailId4 { get; set; }
        /// <summary>
        /// Gets or sets Effort4
        /// </summary>
        [MaxLength(100)]
        public string Effort4 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus4
        /// </summary>
        public bool FreezeStatus4 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID5
        /// </summary>
        public Int64? TimeSheetId5 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate5
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate5 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID5
        /// </summary>
        public Int64? TimeSheetDetailId5 { get; set; }
        /// <summary>
        /// Gets or sets Effort5
        /// </summary>
        [MaxLength(100)]
        public string Effort5 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus5
        /// </summary>
        public bool FreezeStatus5 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID6
        /// </summary>
        public Int64? TimeSheetId6 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate6
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate6 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID6
        /// </summary>
        public Int64? TimeSheetDetailId6 { get; set; }
        /// <summary>
        /// Gets or sets Effort6
        /// </summary>
        [MaxLength(100)]
        public string Effort6 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus6
        /// </summary>
        public bool FreezeStatus6 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID7
        /// </summary>
        public Int64? TimeSheetId7 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate7
        /// </summary>
        [MaxLength(100)]
        public string TimeSheetDate7 { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailID7
        /// </summary>
        public Int64? TimeSheetDetailId7 { get; set; }
        /// <summary>
        /// Gets or sets Effort7
        /// </summary>
        [MaxLength(100)]
        public string Effort7 { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus7
        /// </summary>
        public bool FreezeStatus7 { get; set; }
        /// <summary>
        /// Gets or sets IsAttributeUpdated
        /// </summary>
        public int IsAttributeUpdated { get; set; }
        /// <summary>
        /// Gets or sets lstServiceModel
        /// </summary>
        public List<ServiceDetails> LstServiceModel { get; set; }
        /// <summary>
        /// Gets or sets lstActivityModel
        /// </summary>
        public List<ActivityDetails> LstActivityModel { get; set; }
        /// <summary>
        /// Gets or sets lstTicketTypeModel
        /// </summary>
        public List<TicketTypeDetails> LstTicketTypeModel { get; set; }
        /// <summary>
        /// Gets or sets lstStatusDetails
        /// </summary>
        public List<StatusDetails> LstStatusDetails { get; set; }
        /// <summary>
        /// Gets or sets lstUserLevelDetails
        /// </summary>
        public List<UserLevelDetails> LstUserLevelDetails { get; set; }
        /// <summary>
        /// Gets or sets lstTicketTypeServiceDetails
        /// </summary>
        public List<TicketTypeServiceDetails> LstTicketTypeServiceDetails { get; set; }

        /// <summary>
        /// Gets or sets isAHTicket
        /// </summary>
        [MaxLength(50)]
        public string IsAHTicket { get; set; }
        /// <summary>
        /// Gets or Sets TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or sets lstTowerDetails
        /// </summary>
        public List<TaskDetails> LstTaskModel { get; set; }
        /// <summary>
        /// Gets or Sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or Sets Suggested Activity Name
        /// </summary>
        [MaxLength(100)]
        public string SuggestedActivityName { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [MaxLength(10)]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets IsALMToolConfigured
        /// </summary>
        public bool? IsALMToolConfigured { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets servicevalidatioToolTip
        /// </summary>
        [MaxLength(1000)]
        public string ServiceTitle { get; set; }
        /// <summary>
        /// Gets or sets ActivityvalidatioToolTip
        /// </summary>
        [MaxLength(1000)]
        public string ActivityTitle { get; set; }
        /// <summary>
        /// Gets or sets InfraTitle
        /// </summary>
        [MaxLength(1000)]
        public string InfraTitle { get; set; }
        /// <summary>
        /// Gets or sets TicketIDTitle
        /// </summary>
        [MaxLength(1000)]
        public string TicketIDTitle { get; set; }
        /// <summary>
        /// Gets or sets BenchMarkTitle
        /// </summary>
        [MaxLength(2000)]
        public string BenchMarkTitle { get; set; }

        /// <summary>
        /// Gets or sets BenchMarkColor
        /// </summary>
        [MaxLength(500)]
        public string BenchMarkColor { get; set; }
        /// <summary>
        /// Gets or sets GridValidationservice
        /// </summary>
        public bool GridValidationservice { get; set; }

        /// <summary>
        /// Gets or sets OpenDateTime
        /// </summary>
        public DateTime? OpenDateNTime { get; set; }


    }
    /// <summary>
    /// This class holds GracePeriodDetails
    /// </summary>
    public class GracePeriodDetails
    {
        /// <summary>
        /// Gets or Sets GracePeriod
        /// </summary>
        public int GracePeriod { get; set; }
        /// <summary>
        /// Gets or Sets ClosedDate
        /// </summary>
        public DateTime? ClosedDate { get; set; }
        /// <summary>
        ///  Gets or Sets IsAHTagged
        /// </summary>
        public bool IsAHTagged { get; set; }
        /// <summary>
        /// Gets or Sets IsGracePeriodMet
        /// </summary>
        public bool IsGracePeriodMet { get; set; }
        /// <summary>
        /// Gets or Sets IsFreezed
        /// </summary>
        public bool IsFreezed { get; set; }
        /// <summary>
        /// Gets or Sets CompletedDate
        /// </summary>
        public DateTime? CompletedDate { get; set; }
    }

    /// <summary>
    /// This class holds TimeSheetDetails
    /// </summary>
    public class TimeSheetDetails
    {
        /// <summary>
        /// Gets or sets No
        /// </summary>
        public Int64 No { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetId
        /// </summary>
        public Int64? TimeSheetId { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDate
        /// </summary>
        public string TimeSheetDate { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetDetailId
        /// </summary>
        public Int64? TimeSheetDetailId { get; set; }
        /// <summary>
        /// Gets or sets FreezeStatus
        /// </summary>
        public bool FreezeStatus { get; set; }
    }
    /// <summary>
    /// This class holds CustomerDetails
    /// </summary>
    public class CustomerDetails
    {
        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public int IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsEffortTracked
        /// </summary>
        public int IsEffortTracked { get; set; }
        /// <summary>
        /// Gets or sets IsDaily
        /// </summary>
        public int IsDaily { get; set; }
    }

    /// <summary>
    /// This class holds ServiceActivityDetails
    /// </summary>
    public class ServiceActivityDetails
    {
        // CustomerID ProjectID   ServiceID ServiceName ServiceTypeID ActivityID  ActivityName
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int64? ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets ServiceTypeID
        /// </summary>
        public Int64? ServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public Int64? ActivityId { get; set; }
        /// <summary>
        /// Gets or sets ActivityName
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// Gets or sets ServiceLevelID
        /// </summary>
        public Int32 ServiceLevelId { get; set; }
        /// <summary>
        /// Gets or sets ScopeId
        /// </summary>
        public int ScopeId { get; set; }
    }
    /// <summary>
    /// This class holds TicketTypeServiceDetails
    /// </summary>
    public class TicketTypeServiceDetails
    {
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int64? ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceTypeID
        /// </summary>
        public Int64? ServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeMappingID
        /// </summary>
        public Int64? TicketTypeMappingId { get; set; }
    }
    /// <summary>
    /// This class holds TicketTypeDetails
    /// </summary>
    public class TicketTypeDetails
    {
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets AVMTicketType
        /// </summary>
        public Int64? AVMTicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeMappingID
        /// </summary>
        public Int64? TicketTypeMappingId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public Int64? TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeName
        /// </summary>
        public string TicketTypeName { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
    }
    /// <summary>
    /// This class holds ServiceDetails
    /// </summary>
    public class ServiceDetails
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int64? ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Gets or sets ServiceLevelID
        /// </summary>
        public Int32 ServiceLevelId { get; set; }
        /// <summary>
        /// Gets or sets ScopeId
        /// </summary>
        public int ScopeId { get; set; }
    }
    /// <summary>
    /// This class holds ActivityDetails 
    /// </summary>
    public class ActivityDetails
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64? ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int64? ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public Int64? ActivityId { get; set; }
        /// <summary>
        /// Gets or sets ActivityName
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Gets or sets displayActivity
        /// </summary>
        public string DisplayActivity { get; set; }
    }
    /// <summary>
    /// This class holds StatusDetails 
    /// </summary>
    public class StatusDetails
    {
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public Int64? DARTStatusId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusName
        /// </summary>
        public string DARTStatusName { get; set; }
        /// <summary>
        /// Gets or sets TicketStatusID
        /// </summary>
        public Int64? TicketStatusId { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Gets or sets Type
        /// </summary>
        public char Type { get; set; }

    }
    /// <summary>
    /// This class holds ProjectsModel details
    /// </summary>
    public class ProjectsModel
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
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        public string ProjectTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneID
        /// </summary>
        public Int64 ProjectTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneID
        /// </summary>
        public Int64 UserTimeZoneId { get; set; }

        public int SupportTypeId { get; set; }
    }

    /// <summary>
    /// This class holds ADMProjectsModel details
    /// </summary>
    public class ADMProjectsModel
    {
        /// <summary>
        /// Gets or Sets ProjectID
        /// </summary>
        public Int64 ProjectID { get; set; }
        /// <summary>
        /// Gets or Sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or Sets IsApplensAsALM
        /// </summary>
        public bool IsApplensAsALM { get; set; }
        /// <summary>
        /// Gets or Sets IsAgile
        /// </summary>
        public bool IsAgile { get; set; }
        /// <summary>
        /// Gets or Sets IsWBS
        /// </summary>
        public bool IsWBS { get; set; }
        /// <summary>
        /// Gets or Sets IsIterative
        /// </summary>
        public bool IsIterative { get; set; }
        /// <summary>
        /// Gets or Sets IsOthers
        /// </summary>
        public bool IsOthers { get; set; }
        /// <summary>
        /// Gets or Sets IsMultiple
        /// </summary>
        public bool IsMultiple { get; set; }
        /// <summary>
        /// Gets or Sets IsKanban
        /// </summary>
        public bool IsKanban { get; set; }
        /// <summary>
        /// Gets or Sets IsEffortBased
        /// </summary>
        public bool IsEffortBased { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemMeasurement
        /// </summary>
        public string WorkItemMeasurement { get; set; }
    }
    /// <summary>
    /// This class holds ValidationMessages
    /// </summary>
    public class ValidationMessages
    {
        /// <summary>
        /// Gets or Sets ValidationMessage
        /// </summary>
        [MaxLength(500)]
        public string ValidationMessage { get; set; }
    }
    /// <summary>
    /// This class Checks Duplicate
    /// </summary>
    public class CheckDuplicate
    {
        /// <summary>
        /// Gets or Sets SprintName
        /// </summary>
        [MaxLength(1000)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets ProjectId
        /// </summary>
        public long ProjectId { get; set; }
    }
    /// <summary>
    /// This class holds ADMProjectInput
    /// </summary>
    public class ADMProjectInput
    {
        /// <summary>
        /// Gets or Sets SprintName
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or Sets ProjectId
        /// </summary>
        [MaxLength(100)]
        public string CustomerId { get; set; }
    }
    /// <summary>
    /// This class holds DropdownInputs
    /// </summary>
    public class DropdownInputs
    {
        /// <summary>
        /// Gets or Sets projectId
        /// </summary>
        public Int64 projectId { get; set; }
        /// <summary>
        /// Gets or Sets startDate
        /// </summary>
        [MaxLength(100)]
        public string startDate { get; set; }
        /// <summary>
        /// Gets or Sets ProjectId
        /// </summary>
        [MaxLength(100)]
        public string endDate { get; set; }
    }
    /// <summary>
    /// This class holds Sprint Details
    /// </summary>
    public class SavesprintDetails
    {
        /// <summary>
        /// Gets or Sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or Sets SprintName
        /// </summary>
        public string SprintName { get; set; }
        /// <summary>
        /// Gets or Sets SprintDescription
        /// </summary>
        public string SprintDescription { get; set; }
        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Gets or Sets UserID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or Sets StatusMapID
        /// </summary>
        public long? StatusMapId { get; set; }
        /// <summary>
        /// Gets or Sets PODDetailID
        /// </summary>
        public Int64 PODDetailId { get; set; }

    }
    /// <summary>
    /// This class holds Sprint Details
    /// </summary>
    public class SprintDetails
    {
        /// <summary>
        /// Gets or Sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or Sets SprintName
        /// </summary>
        public string SprintName { get; set; }
        /// <summary>
        /// Gets or Sets SprintDescription
        /// </summary>
        public string SprintDescription { get; set; }
        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or Sets UserID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or Sets StatusMapID
        /// </summary>
        public long? StatusMapId { get; set; }
        /// <summary>
        /// Gets or Sets PODDetailID
        /// </summary>
        public Int64 PODDetailId { get; set; }

    }
    /// <summary>
    /// get tower name and id
    /// </summary>
    public class TowerDetailsModel
    {
        /// <summary>
        /// Get or set TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Get or set TowerName
        /// </summary>
        public string TowerName { get; set; }
    }
    /// <summary>
    /// This class holds ApplicationsModel details
    /// </summary>
    public class ApplicationsModel
    {
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }

    /// <summary>
    /// This class holds ChooseTicketModel details
    /// </summary>
    public class ChooseTicketModel
    {
        /// <summary>
        /// Gets or sets lstProjectsModel
        /// </summary>
        public List<ProjectsModel> LstProjectsModel { get; set; }
        /// <summary>
        /// Gets or sets lstApplicationDetails
        /// </summary>
        public List<ApplicationsModel> LstApplicationDetails { get; set; }
        /// <summary>
        /// Get or set LstAssignmentGroupDetails
        /// </summary>
        public List<AssignmentGroupModel> LstAssignmentGroupDetails { get; set; }
        /// <summary>
        /// tower details list
        /// </summary>
        public List<TowerDetailsModel> LstTowerDetails { get; set; }
        /// <summary>
        /// Gets or sets lsTicketStatusDetails
        /// </summary>
        public List<StatusDetails> LsTicketStatusDetails { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        public string ProjectTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets CurrentTime
        /// </summary>
        public string CurrentTime { get; set; }
    }
    /// <summary>
    /// This class holds SaveTicketingModuleTicketDetails 
    /// </summary>
    public class SaveTicketingModuleTicketDetails
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
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public int TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketStatus
        /// </summary>
        public int TicketStatus { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        public decimal ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets TotalEffort
        /// </summary>
        public decimal TotalEffort { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        public int TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public int DARTStatusId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets TimesheetDetails 
        /// </summary>
        public List<SaveTicketingModuleTimeSheetDetails> TimesheetDetails { get; set; }
    }
    /// <summary>
    /// This class holds Requestclass details
    /// </summary>
    public class Requestclass
    {
        /// <summary>
        /// Gets or sets projectid
        /// </summary>
        public string Projectid { get; set; }

        /// <summary>
        /// Gets or sets appid
        /// </summary>
        public int Appid { get; set; }
        /// <summary>
        /// Gets or sets causeid
        /// </summary>
        public int Causeid { get; set; }
        /// <summary>
        /// Gets or sets resolutionid
        /// </summary>
        public int Resolutionid { get; set; }
        /// <summary>
        /// Gets or sets desc_text
        /// </summary>
        public string Desc_text { get; set; }
        /// <summary>
        /// Gets or sets add_text
        /// </summary>
        public string Add_text { get; set; }
    }
    public class RequestTypeclass
    {
        /// <summary>
        /// Gets or sets projectid
        /// </summary>
        public string Projectid { get; set; }

        /// <summary>
        /// Gets or sets appid
        /// </summary>
        public int Appid { get; set; }
        /// <summary>
        /// Gets or sets desc_text
        /// </summary>
        public string Desc_text { get; set; }
        /// <summary>
        /// Gets or sets add_text
        /// </summary>
        public string Add_text { get; set; }
    }
    public class RequestclassInfra
    {
        /// <summary>
        /// Gets or sets projectid
        /// </summary>
        public string Projectid { get; set; }
        /// <summary>
        /// Gets or sets TowerID
        /// </summary>
        public int Towerid { get; set; }
        /// <summary>
        /// Gets or sets causeid
        /// </summary>
        public int Causeid { get; set; }
        /// <summary>
        /// Gets or sets resolutionid
        /// </summary>
        public int Resolutionid { get; set; }
        /// <summary>
        /// Gets or sets desc_text
        /// </summary>
        public string Desc_text { get; set; }
        /// <summary>
        /// Gets or sets add_text
        /// </summary>
        public string Add_text { get; set; }
    }
    public class RequestTypeclassInfra
    {
        /// <summary>
        /// Gets or sets projectid
        /// </summary>
        public string Projectid { get; set; }

        /// <summary>
        /// Gets or sets appid
        /// </summary>
        public int Towerid { get; set; }
        /// <summary>
        /// Gets or sets desc_text
        /// </summary>
        public string Desc_text { get; set; }
        /// <summary>
        /// Gets or sets add_text
        /// </summary>
        public string Add_text { get; set; }
    }
    /// <summary>
    /// This class holds Responseclass details
    /// </summary>
    public class Responseclass
    {
        /// <summary>
        /// Gets or sets avoidable
        /// </summary>
        public string Avoidable { get; set; }
        /// <summary>
        /// Gets or sets debt
        /// </summary>
        public string Debt { get; set; }
        /// <summary>
        /// Gets or sets lw_ruleid
        /// </summary>
        public string Lw_ruleid { get; set; }
        /// <summary>
        /// Gets or sets lw_rulelevel
        /// </summary>
        public string Lw_rulelevel { get; set; }
        /// <summary>
        /// Gets or sets residual
        /// </summary>
        public string Residual { get; set; }
        /// <summary>
        /// Gets or sets ruleid
        /// </summary>
        public string Ruleid { get; set; }
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
    /// This class holds Responseclass details for NewAlgo
    /// </summary>
    public class ResponseClassNewAlgo
    {
        /// <summary>
        /// Gets or sets avoidable
        /// </summary>
        public string AvoidableFlagID { get; set; }
        /// <summary>
        /// Gets or sets debt
        /// </summary>
        public string DebtClassificationID { get; set; }
        /// <summary>
        /// Gets or sets ClusterID_Desc
        /// </summary>
        public string ClusterID_Desc { get; set; }
        /// <summary>
        /// Gets or sets ClusterID_Resolution
        /// </summary>
        public string ClusterID_Resolution { get; set; }
        /// <summary>
        /// Gets or sets residual
        /// </summary>
        public string ResidualDebtID { get; set; }
    }
    /// <summary>
    /// This class holds SaveTicketDetails details
    /// </summary>
    public class SaveTicketDetails
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
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public int TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketStatus
        /// </summary>
        public int TicketStatus { get; set; }
        /// <summary>
        /// Gets or sets ITSMEffort
        /// </summary>
        public decimal ITSMEffort { get; set; }
        /// <summary>
        /// Gets or sets TotalEffort
        /// </summary>
        public decimal TotalEffort { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        public int TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets DARTStatusID
        /// </summary>
        public int DARTStatusId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        public string Type { get; set; }
    }
    /// <summary>
    /// This class holds SaveTicketingModuleTimeSheetDetails 
    /// </summary>
    public class SaveTicketingModuleTimeSheetDetails
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public int TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketStatus
        /// </summary>
        public int TicketStatus { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets TimeSheetID
        /// </summary>
        public int TimeSheetId { get; set; }
        /// <summary>
        /// Gets or sets TimesheetDetailID
        /// </summary>
        public int TimesheetDetailId { get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        public int TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets IsNonTicket
        /// </summary>
        public bool IsNonTicket { get; set; }
        /// <summary>
        /// Gets or sets Hours
        /// </summary>
        public decimal Hours { get; set; }
        /// <summary>
        /// Gets or sets TimesheetDate
        /// </summary>
        public string TimesheetDate { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets IsChanged
        /// </summary>
        public string IsChanged { get; set; }
        /// <summary>
        /// Gets or sets IsFreezed
        /// </summary>
        public string IsFreezed { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or sets SuggestedActivityName
        /// </summary>
        public string SuggestedActivityName { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        public string Type { get; set; }

    }

    /// <summary>
    /// This class holds GetProjectNameEsaID details
    /// </summary>
    public class GetProjectNameEsaID
    {
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets ESAID
        /// </summary>
        public string ESAId { get; set; }
    }

    /// <summary>
    /// This class holds DeleteTicket details
    /// </summary>
    public class DeleteTicket
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [MaxLength(100)]
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int32 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets TimeTickerID
        /// </summary>
        [MaxLength(100)]
        public string TimeTickerId { get; set; }
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        [MaxLength(50)]
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        [MaxLength(50)]
        public string ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ActivityID
        /// </summary>
        public int ActivityId { get; set; }
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
        /// Gets or sets SubmitterID
        /// </summary>
        [MaxLength(100)]
        public string SubmitterId { get; set; }
        /// <summary>
        /// Gets or sets TxtHours
        /// </summary>
        [MaxLength(100)]
        public string TxtHours { get; set; }
        /// <summary>
        /// Gets or sets TickSupportTypeID
        /// </summary>
        public int TickSupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets Type
        /// </summary>
        [MaxLength(10)]
        public string Type { get; set; }

    }
    /// <summary>
    /// This class holds Tower details
    /// </summary>
    public class TaskDetails
    {
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets InfraTowerTransactionID
        /// </summary>
        public Int64 InfraTowerTransactionId { get; set; }
        /// <summary>
        /// Gets or sets TowerName
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets InfraTransactionTaskID
        /// </summary>
        public Int64 InfraTransactionTaskId { get; set; }
        /// <summary>
        /// Gets or sets InfraTaskName
        /// </summary>
        public string InfraTaskName { get; set; }
        /// <summary>
        /// Gets or sets IsSelected
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Gets or sets ServiceLevelID
        /// </summary>
        public int ServiceLevelId { get; set; }
    }
    /// <summary>
    /// This class for Add WorkItem
    /// </summary>
    public class AddWorkItem
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemTypeID
        /// </summary>
        public long WorkItemTypeId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemID
        /// </summary>
        public string WorkItemId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public long ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets EpicID
        /// </summary>
        public string EpicId { get; set; }
        /// <summary>
        /// Gets or sets SprintID
        /// </summary>
        public long SprintId { get; set; }
        /// <summary>
        /// Gets or sets UserStoryID
        /// </summary>
        public string UserStoryId { get; set; }
        /// <summary>
        /// Gets or sets Assignee
        /// </summary>
        public string Assignee { get; set; }
        /// <summary>
        /// Gets or sets SprintStartDate
        /// </summary>
        public DateTime SprintStartDate { get; set; }
        /// <summary>
        /// Gets or sets SprintEndDate
        /// </summary>
        public DateTime SprintEndDate { get; set; }
        /// <summary>
        /// Gets or sets PlannedStartDate
        /// </summary>
        public DateTime PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets PlannedEndDate
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public long StatusId { get; set; }
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public long PriorityId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemTitle
        /// </summary>
        public string WorkItemTitle { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets PlannedEffort
        /// </summary>
        public decimal PlannedEstimate { get; set; }
        /// <summary>
        /// Gets or sets EstimationPoints
        /// </summary>
        public string EstimationPoints { get; set; }
        /// <summary>
        /// Gets or sets Mark as Milestone or Expedite
        /// </summary>
        public bool? IsMileStoneMet { get; set; }
        /// <summary>
        /// Gets or Sets BugPhaseTypeMapId
        /// </summary>
        public short? BugPhaseTypeMapId { get; set; }
        /// <summary>
        /// Gets or Sets ReMapSprintDetailsId
        /// </summary>
        public long? ReMapSprintDetailsId { get; set; }
    }

    /// <summary>
    /// This class for Add WorkItem
    /// </summary>
    public class AddWorkItemSave
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemTypeID
        /// </summary>
        public long WorkItemTypeId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemID
        /// </summary>
        public string WorkItemId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public long ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets EpicID
        /// </summary>
        public string EpicId { get; set; }
        /// <summary>
        /// Gets or sets SprintID
        /// </summary>
        public long SprintId { get; set; }
        /// <summary>
        /// Gets or sets UserStoryID
        /// </summary>
        public string UserStoryId { get; set; }
        /// <summary>
        /// Gets or sets Assignee
        /// </summary>
        public string Assignee { get; set; }
        /// <summary>
        /// Gets or sets SprintStartDate
        /// </summary>
        public string SprintStartDate { get; set; }
        /// <summary>
        /// Gets or sets SprintEndDate
        /// </summary>
        public string SprintEndDate { get; set; }
        /// <summary>
        /// Gets or sets PlannedStartDate
        /// </summary>
        public string PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets PlannedEndDate
        /// </summary>
        public string PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public long StatusId { get; set; }
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public long PriorityId { get; set; }
        /// <summary>
        /// Gets or sets WorkItemTitle
        /// </summary>
        public string WorkItemTitle { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets PlannedEffort
        /// </summary>
        public decimal PlannedEstimate { get; set; }
        /// <summary>
        /// Gets or sets EstimationPoints
        /// </summary>
        public string EstimationPoints { get; set; }
        /// <summary>
        /// Gets or sets Mark as Milestone or Expedite
        /// </summary>
        public bool? IsMileStoneMet { get; set; }
        /// <summary>
        /// Gets or Sets BugPhaseTypeMapId
        /// </summary>
        public short? BugPhaseTypeMapId { get; set; }
        /// <summary>
        /// Gets or Sets ReMapSprintDetailsId
        /// </summary>
        public long? ReMapSprintDetailsId { get; set; }
    }
    /// <summary>
    /// Accept input for Getticketdetails
    /// </summary>
    public class Getticketdetailsinput
    {
        /// <summary>
        /// Accept input for CustomerID
        /// </summary>
        [MaxLength(100)]
        public string CustomerID {get; set;}
        
        /// <summary>
        /// Accept input for FirstDateOfWeek
        /// </summary>
        [MaxLength(100)]
        public string FirstDateOfWeek { get; set; }
        /// <summary>
        /// Accept input for LastDateOfWeek
        /// </summary>
        [MaxLength(100)]
        public string LastDateOfWeek { get; set; }
        /// <summary>
        /// Accept input for Mode
        /// </summary>
        [MaxLength(500)]
        public string Mode { get; set; }
        /// <summary>
        /// Accept input for IsCognizant
        /// </summary>
        [MaxLength(50)]
        public string IsCognizant { get; set; }
    }
    /// <summary>
    /// Accept input for Savedata
    /// </summary>
    public class Savedatainput
    {
        /// <summary>
        /// Accept input for lstticketData
        /// </summary>
        public List<SaveTicketingModuleTicketDetails> lstticketdata { get; set; }
        /// <summary>
        /// Accept input for Flag
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// Accept input for EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeID { get; set; }
        /// <summary>
        /// Accept input for IsDaily
        /// </summary>
        [MaxLength(50)]
        public string IsDaily { get; set; }
    }

}