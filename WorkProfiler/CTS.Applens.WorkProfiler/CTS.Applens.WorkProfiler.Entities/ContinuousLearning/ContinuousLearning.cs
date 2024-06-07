using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models.ContinuousLearning
{
    /// <summary>
    /// This class holds base continuous learning base model
    /// </summary>
    public class ContinuousLearningBaseModel
    {
        /// <summary>
        /// Gets or sets RolePrevilageMenus
        /// </summary>
        public List<RolePrivilegeModel> RolePrevilageMenus { get; set; }


        /// <summary>
        /// Gets or sets HiddenFields
        /// </summary>
        public HiddenFieldsModel HiddenFields { get; set; }

    }

    /// <summary>
    /// This class holds ContinuousLearningList details
    /// </summary>
    public class ContinuousLearningList
    {
        /// <summary>
        /// Gets or sets project
        /// </summary>
        public List<Project> Project { get; set; }
        /// <summary>
        /// Gets or sets portfolio
        /// </summary>
        public List<Portfolio> Portfolio { get; set; }
    }
    /// <summary>
    /// This class holds CLExcel details
    /// </summary>
    public class CLExcel
    {
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets TicketPattern
        /// </summary>
        public string TicketPattern { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtCategory
        /// </summary>
        public string DebtCategory { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public string AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualFlag
        /// </summary>
        public string ResidualFlag { get; set; }
        /// <summary>
        /// Gets or sets Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets ReasonForResidual
        /// </summary>
        public string ReasonForResidual { get; set; }
        /// <summary>
        /// Gets or sets DateCompletion
        /// </summary>
        public string DateCompletion { get; set; }
        /// <summary>
        /// Gets or sets Approved
        /// </summary>
        public string Approved { get; set; }
        /// <summary>
        /// Gets or sets Muted
        /// </summary>
        public string Muted { get; set; }
    }
    /// <summary>
    /// This class holds CustomerCognizant details
    /// </summary>
    public class CustomerCognizant
    {
        /// <summary>
        /// Gets or sets isCognizant
        /// </summary>
        public bool IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets CustomerName
        /// </summary>
        public string CustomerName { get; set; }
    }
    /// <summary>
    /// This class holds CLConfig details
    /// </summary>
    public class CLConfig
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets Month
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Gets or sets Day
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// Gets or sets HasValue
        /// </summary>
        public bool HasValue { get; set; } = false;
    }
    /// <summary>
    /// This class holds BusinessUnit details
    /// </summary>
    public class BusinessUnit
    {
        /// <summary>
        /// Gets or sets BUID
        /// </summary>
        public int BUID { get; set; }
        /// <summary>
        /// Gets or sets BusinessUnitName
        /// </summary>
        public string BusinessUnitName { get; set; }
    }
    /// <summary>
    /// This class holds Account details
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets AccountID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Gets or sets AccountName
        /// </summary>
        public string AccountName { get; set; }
    }
    /// <summary>
    /// This class holds Project details
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets projectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets projectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets SuppoetTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets IsDDAutoClassified
        /// </summary>
        public string IsDDAutoClassified { get; set; }
        /// <summary>
        /// Gets or sets IsManual
        /// </summary>
        public string IsManual { get; set; }
        /// <summary>
        /// Gets or sets IsTicketApprovalNeeded
        /// </summary>
        public string IsTicketApprovalNeeded { get; set; }
    }
    /// <summary>
    /// This class holds Portfolio details
    /// </summary>
    public class Portfolio
    {
        /// <summary>
        /// Gets or sets portfolioID
        /// </summary>
        public int PortfolioId { get; set; }
        /// <summary>
        /// Gets or sets portfolioName
        /// </summary>
        public string PortfolioName { get; set; }
    }
    /// <summary>
    /// This class holds Application details
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Gets or sets applicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets applicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
    /// <summary>
    /// This class is used for copy functionality
    /// </summary>

    public class CLPatterns
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets DebtID
        /// </summary>
        public int DebtId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public int AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets ResidualID
        /// </summary>
        public int ResidualId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public int CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets ApprovedOrMuted
        /// </summary>
        public int ApprovedOrMuted { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets IsCLSignOff
        /// </summary>
        public bool IsCLSignOff { get; set; }
        /// <summary>
        /// Gets or sets OldContID
        /// </summary>
        public Int32 OldContId { get; set; }
        /// <summary>
        /// Gets or sets NewContID
        /// </summary>
        public Int32 NewContId { get; set; }
        /// <summary>
        /// Gets or sets IsDebtChanged
        /// </summary>
        public int IsDebtChanged { get; set; }

    }
    /// <summary>
    /// This class holds CLPatternsSignOff details
    /// </summary>
    public class CLPatternsSignOff
    {
        /// <summary>
        /// Gets or sets patterns
        /// </summary>
        public List<CLPatterns> Patterns { get; set; }
        /// <summary>
        /// Gets or sets IsCLSignOff
        /// </summary>
        public bool IsCLSignOff { get; set; } = false;
        /// <summary>
        /// Gets or sets EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; } = DateTime.Today;
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [MaxLength(50)]
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets IsSave
        /// </summary>
        public bool IsSave { get; set; }
        
    }
    /// <summary>
    /// This class holds CLpatternContiSignOff details
    /// </summary>
    public class CLpatternContiSignOff
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets patterns
        /// </summary>
        public List<CLPatterns> Patterns { get; set; }
        /// <summary>
        /// Gets or sets IsCLSignOff
        /// </summary>
        public bool IsCLSignOff { get; set; }
        /// <summary>
        /// Gets or sets AppID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
    }
    /// <summary>
    /// This class holds LearningEnrichmentDate details
    /// </summary>
    public class LearningEnrichmentDate
    {
        /// <summary>
        /// Gets or sets FromDate
        /// </summary>
        public string FromDate { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets ToDate
        /// </summary>
        public string ToDate { get; set; } = string.Empty;
    }
    /// <summary>
    /// This class holds JobDetails details
    /// </summary>
    public class JobDetails
    {
        /// <summary>
        /// Gets or sets JobDate
        /// </summary>
        public string JobDate { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets JobStatus
        /// </summary>
        public string JobStatus { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets isCLEnabled
        /// </summary>
        public string IsCLEnabled { get; set; }
    }
    /// <summary>
    /// This class holds CLDetails details
    /// </summary>
    public class CLDetails
    {
        /// <summary>
        /// Gets or sets CLSignoff
        /// </summary>
        public int CLSignoff { get; set; }
        /// <summary>
        /// Gets or sets IsCLAutoClassified
        /// </summary>
        public string IsCLAutoClassified { get; set; }
        /// <summary>
        /// Gets or sets CLAutoclassificationDatestring
        /// </summary>
        public string CLAutoclassificationDatestring { get; set; }

    }
   public class ExcelDownload
    {
        [MaxLength(int.MaxValue)]
        public string FileName { get; set; }
    }
}