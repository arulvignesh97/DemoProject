using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds LanguageFilter
    /// </summary>
    public class LanguageFilter
    {
        /// <summary>
        /// Gets or Sets ModuleName
        /// </summary>
        [MaxLength(20)]
        public string ModuleName { get; set; }
    }
    /// <summary>
    /// This class holds CustomerEmployeeFilter details
    /// </summary>
    public class CustomerEmployeeFilter
    {
        /// <summary>
        /// Gets or Sets CustomerID
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// Gets or Sets EmployeeID
        /// </summary>
        [MaxLength(100)]
        public string EmployeeID { get; set; }
    }
    /// <summary>
    /// AssigneModel
    /// </summary>
    public class AssigneModel
    {
        /// <summary>
        /// Gets and sets AssigneName
        /// </summary>
        [MaxLength(100)]
        public string AssigneName { get; set; }
        /// <summary>
        /// Gets and sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets and sets UserID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Gets and sets ProjectID
        /// </summary>
        public int? ProjectId { get; set; }
    }

    /// <summary>
    /// BaseInformationModel
    /// </summary>
    public class BaseInformationModel
    {
        /// <summary>
        /// EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// CustomerID
        /// </summary>
        [MaxLength(50)]
        public string CustomerId { get; set; }
        /// <summary>
        /// FirstDateOfWeek
        /// </summary>
        [MaxLength(50)]
        public string FirstDateOfWeek { get; set; }
        /// <summary>
        /// LastDateOfWeek
        /// </summary>
        [MaxLength(50)]
        public string LastDateOfWeek { get; set; }
        /// <summary>
        /// TicketDescription
        /// </summary>
       [MaxLength(int.MaxValue)]
        public string TicketDescription { get; set; }
        /// <summary>
        /// TicketID
        /// </summary>
        [MaxLength(50)]
        public string TicketId { get; set; }
        /// <summary>
        /// NonTicketActivityID
        /// </summary>
        [MaxLength(50)]
        public string NonTicketActivityId { get; set; }
        /// <summary>
        /// ProjectID
        /// </summary>
        [MaxLength(50)]
        public string ProjectId { get; set; }
        /// <summary>
        /// SuggestedActivityName
        /// </summary>
        [MaxLength(50)]
        public string SuggestedActivityName { get; set; }
        /// <summary>
        /// Language
        /// </summary>
        [MaxLength(50)]
        public string Language { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        /// <summary>
        /// SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// PortfolioID
        /// </summary>
        public long PortfolioId { get; set; }
        /// <summary>
        /// CauseCode
        /// </summary>
        [MaxLength(100)]
        public string CauseCode { get; set; }
    }

    /// <summary>
    /// EffortDetailsChartModel
    /// </summary>
    public class EffortDetailsChartModel
    {
        /// <summary>
        /// EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Month
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// FirstDateOfWeek
        /// </summary>
        public DateTime? FirstDateOfWeek { get; set; }
        /// <summary>
        /// LastDateOfWeek
        /// </summary>
        public DateTime? LastDateOfWeek { get; set; }
        /// <summary>
        /// CustomerID
        /// </summary>
        [MaxLength(50)]
        public string CustomerId { get; set; }
        /// <summary>
        /// Mode
        /// </summary>
        [MaxLength(50)]
        public string Mode { get; set; }
        /// <summary>
        /// IsCognizant
        /// </summary>
        public int? IsCognizant { get; set; }
        /// <summary>
        /// Gets or Sets First Week
        /// </summary>
        [MaxLength(500)]
        public string FirstWeek { get; set; }
        /// <summary>
        /// Gets or Sets Last Week
        /// </summary>
        [MaxLength(500)]
        public string LastWeek { get; set; }

    }

    public class ProjectSearchTickets
    {
        /// <summary>
        /// ProjectIDs
        /// </summary>
        public List<string> ProjectIds { get; set; }
    }
    /// <summary>
    /// User preference model
    /// </summary>
    public class PreferenceModel {
        /// <summary>
        /// Employee Id
        /// </summary>
        [MaxLength(100)]
        public string EmployeeID { get; set; }
        /// <summary>
        /// Account Id
        /// </summary>
        [MaxLength(100)]
        public string AccountID { get; set; }
        /// <summary>
        /// Previlege Id
        /// </summary>
        [MaxLength(100)]
        public string PrivilegeID { get; set; }
    }

}