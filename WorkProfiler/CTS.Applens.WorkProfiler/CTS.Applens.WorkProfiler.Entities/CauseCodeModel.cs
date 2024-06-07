using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds CauseCodeModel details
    /// </summary>
    public class CauseCodeModel
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets CognizantID
        /// </summary>
        public int CognizantId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        //[Required(ErrorMessage = "Cause Code is Required", AllowEmptyStrings = false)]
        [StringLength(50)]
        [Display(Name = "Cause Code")]
        [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets CauseID
        /// </summary>
        public System.Nullable<int> CauseId { get; set; }
        /// <summary>
        /// Gets or sets CauseStatus
        /// </summary>
        public string CauseStatus { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultCauseStatus
        /// </summary>
        public string IsDefaultCauseStatus { get; set; }
        /// <summary>
        /// Gets or sets IsDeleted
        /// </summary>
        public string IsDeleted { get; set; }
    }
    /// <summary>
    /// This class holds ResolutionCodeModel details
    /// </summary>
    public class ResolutionCodeModel
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets CognizantID
        /// </summary>
        public int CognizantId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        //[Required(ErrorMessage = "Resolution Code is Required", AllowEmptyStrings = false)]
        [StringLength(50)]
        [Display(Name = "Resolution Code")]
        [RegularExpression(@"[^<>]*", ErrorMessage = "Invalid entry")]
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public System.Nullable<int> ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionStatus
        /// </summary>
        public string ResolutionStatus { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultResolutionStatus
        /// </summary>
        public string IsDefaultResolutionStatus { get; set; }
        /// <summary>
        /// Gets or sets IsDeleted
        /// </summary>
        public string IsDeleted { get; set; }
    }
    /// <summary>
    /// This class holds DebtClassificationModel details
    /// </summary>
    public class DebtClassificationModel
    {
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public int DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationName
        /// </summary>
        public string DebtClassificationName { get; set; }
    }
    /// <summary>
    /// This class holds AvoidableFlagModel details
    /// </summary>
    public class AvoidableFlagModel
    {
        /// <summary>
        /// Gets or sets AvoidableFlagId
        /// </summary>
        public int AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
    }
    /// <summary>
    /// This class holds ResidualDebtModel details
    /// </summary>
    public class ResidualDebtModel
    {
        /// <summary>
        /// Gets or sets ResidualDebtId
        /// </summary>
        public int ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtName
        /// </summary>
        public string ResidualDebtName { get; set; }
    }
}
