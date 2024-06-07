using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// DataDictionaryProjects
    /// </summary>
    public class DataDictionaryProjects
    {
        public List<Project> Project { get; set; }
        public string PortfolioName { get; set; }
    }
    /// <summary>
    /// User basic details
    /// </summary>
    public class UserBaseDetails {
        /// <summary>
        /// Employee ID 
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string employeeID { get; set; }
        /// <summary>
        /// Customer Id
        /// </summary>
        public Int32 customerID { get; set; }
        /// <summary>
        /// Project Id
        /// </summary>
        public Int64 projectID { get; set; }
    }
    /// <summary>
    /// This class holds DataDictionary details
    /// </summary>
    public class DataDictionary
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public int CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeID
        /// </summary>
        public int ResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public int DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public int AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public int ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public int CustomerId { get; set; }
    }
    /// <summary>
    /// DataDictionaryData
    /// </summary>
    public class DataDictionaryData
    {
        /// <summary>
        /// LstPortfolioDataPortfolioID
        /// </summary>
        public List<PortfolioData> LstPortfolioData { get; set; }
        /// <summary>
        /// LstApplicationData
        /// </summary>
        public List<ApplicationDataPortfolio> LstApplicationData { get; set; }
    }

    public class DownloadRequest {
        [MaxLength(int.MaxValue)] 
        public string Path { get; set; } 
       public bool IsMacroTemplate { get; set; }
    }
    /// <summary>
    /// get request parameters for search filters drop down values
    /// </summary>
    public class DataDictionarySearchFilter
    {
        /// <summary>
        /// Employee ID
        /// </summary>
        [Required]
        public string employeeID { get; set; }
        /// <summary>
        /// Customer Id
        /// </summary>
        [Required]
        public Int32 customerID { get; set; }
        /// <summary>
        /// Project Id
        /// </summary>
        [Required] 
        public Int64 projectID { get; set; }
        /// <summary>
        /// Mode
        /// </summary>
        [MaxLength(int.MaxValue)]  
        public string mode { get; set; }
        /// <summary>
        /// List of portfolio id's
        /// </summary>
        public List<PortfolioID> lstIDs { get; set; }
    }
    /// <summary>
    /// This class holds PortfolioData details
    /// </summary>
    public class PortfolioData
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
    /// ApplicationDataPortfolio
    /// </summary>
    public class ApplicationDataPortfolio
    {
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets PortfolioID
        /// </summary>
        public int PortfolioId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
    /// <summary>
    /// PortfolioID
    /// </summary>
    public class PortfolioID
    {
        /// <summary>
        /// Gets or sets PortFolioID
        /// </summary>
        public int PortFolioId { get; set; }
    }
    /// <summary>
    /// ApplicationIDs
    /// </summary>
    public class ApplicationIDs
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }
    }
    /// <summary>
    /// ApplicationData
    /// </summary>
    public class ApplicationData
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
    /// This class holds ResidualDetail details
    /// </summary>
    public class ResidualDetail
    {
        /// <summary>
        /// Gets or sets ExpectedDate
        /// </summary>
        public string ExpectedDate { get; set; }
        /// <summary>
        /// Gets or sets ReasonForResudial
        /// </summary>
        public string ReasonForResudial { get; set; }
    }
    /// <summary>
    /// This class hold parameters for AddApplicationDetails method
    /// </summary>
    public class AddApplicationDetails
    {
        /// <summary>
        ///  Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        ///  Gets or sets ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        ///  Gets or sets ReasonForResidualID
        /// </summary>
        public int ReasonForResidualId { get; set; }
        /// <summary>
        ///  Gets or sets AvoidableFlagID
        /// </summary>
        public int AvoidableFlagId { get; set; }
        /// <summary>
        ///  Gets or sets DebtClassificationID
        /// </summary>
        public int DebtClassificationId { get; set; }
        /// <summary>
        ///  Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        ///  Gets or sets ResidualDebtID
        /// </summary>
        public int ResidualDebtId { get; set; }
        /// <summary>
        ///  Gets or sets CauseCodeID
        /// </summary>
        public int CauseCodeId { get; set; }
        /// <summary>
        ///  Gets or sets ResolutionCodeID
        /// </summary>
        public int ResolutionCodeId { get; set; }
        /// <summary>
        ///  Gets or sets ExpectedCompletionDate
        /// </summary>
        [MaxLength(50)]
        public string ExpectedCompletionDate { get; set; }
        /// <summary>
        ///  Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        ///  Gets or sets EffectiveDate
        /// </summary>
        [MaxLength(50)]
        public string EffectiveDate { get; set; }
    }
    /// <summary>
    ///  This class hold parameters for AddReasonResidualAndCompDate method
    /// </summary>
    public class AddReasonResidualAndCompDate
    {
        /// <summary>
        /// Gets or sets EmpIds
        /// </summary>
        public int EmpId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationIds
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CauseIds
        /// </summary>
        public int CauseId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionIds
        /// </summary>
        public int ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets DebtclassiIds
        /// </summary>
        public int DebtclassiId { get; set; }
        /// <summary>
        /// Gets or sets AvoidIds
        /// </summary>
        public int AvoidId { get; set; }
        /// <summary>
        /// Gets or sets ResiIds
        /// </summary>
        public int ResiId { get; set; }
        /// <summary>
        /// Gets or sets Projectid
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets ReasonResiValueId
        /// </summary>
        public int ReasonResiValueId { get; set; }
        /// <summary>
        /// Gets or sets CompDateValue
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string CompDateValue { get; set; }
    }

    /// <summary>
    /// Add Reasonfor Residual
    /// </summary>
    public class AddReasonforResidual
    {
        /// <summary>
        /// Get or set ResidualText
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ResidualText { get; set; }

        /// <summary>
        /// Get or Set EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
    }
    /// <summary>
    /// Data Dictionary Details
    /// </summary>
    public class DataDictionaryDetails {
        /// <summary>
        /// Get/set Data Dictionary details
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string dataDictionaryDetails { get; set; }
        /// <summary>
        /// Employee Id
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string EmployeeID { get; set; }
    }
    /// <summary>
    /// SignOff Details
    /// </summary>
    public class SignOffDetails {
        /// <summary>
        /// Project ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// Application ID
        /// </summary>
        public int ApplicationID { get; set; }
        /// <summary>
        /// Effective Date
        /// </summary>
        [Required]
        public string EffectiveDate { get; set; }
        /// <summary>
        /// Employee ID
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string EmployeeID { get; set; }
    }
    public class Datadict
    {
        /// <summary>
        /// Project ID
        /// </summary>
        public int ProjectID { get; set; }        
    }
}