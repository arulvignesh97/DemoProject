using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities
{
    public class MyActivitySourceDto
    {
        [Required]
        [MaxLength(4000)]
        public string ActivityDescription { get; set; }
        [Required]
        [MaxLength(15)]
        public string WorkItemCode { get; set; }

        [Required]
        public long SourceRecordID { get; set; }
        [Required]
        [MaxLength(50)]
        public string ActivityTo { get; set; }
        [Required]
        public DateTime RaisedOnDate { get; set; }
        [MaxLength(4000)]
        public string RequestorJson { get; set; }
        [MaxLength(4000)]
        public string ActivityInfo { get; set; }
        [MaxLength(4000)]
        [Required]
        public string Navigation { get; set; }
        [MaxLength(4000)]
        public string MailContent { get; set; }
        [MaxLength(255)]
        public string MailTo { get; set; }
        [MaxLength(200)]
        public string RaisedByName { get; set; }
        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

    }

    public class ActivityConfigurationsModel
    {
        public string WorkItemCode { get; set; }
        public string WorkItemName { get; set; }
        public int GracePeriod { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityTypeCode { get; set; }
        public string PostNavigationApiURL { get; set; }
        public bool IsWebApp { get; set; }
        public bool IsMail { get; set; }


    }

    public class UpdateActivityToExpiryModel
    {
        [Required]
        public long SourceRecordId { get; set; }
        [Required]
        //[MaxLength(15, ErrorMessage = "WorkItemCode should not exceed 15chars")]
        [MaxLength(15)]
        public string WorkItemCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

    }
    /// <summary>
    /// ActivityBasedExpiryModel
    /// </summary>
    public class ActivityBasedExpiryModel
    {
        [Required]
        public long ActivityID { get; set; }
        [Required]
        [MaxLength(50)]
        public string ActivityTo { get; set; }
        [Required]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

    }

    public class ExistingAcitivityDetailsModel
    {
        public long ActivityID { get; set; }
        public string ActivityDescription { get; set; }
        public long SourceRecordID { get; set; }
        public string ActivityTo { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime DueDate { get; set; }
        public string FormatedDueDate { get { return this.DueDate.ToString("MM-dd-yyyy"); } }
        public Boolean IsExpired { get; set; }
        public Boolean IsViewed { get; set; }
        public long WorkItemID { get; set; }
        public string RequestorJson { get; set; }
        public string ActivityInfo { get; set; }
        public string Comments { get; set; }
        public string ApprovalStatus { get; set; }
        public string Navigation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormatedCreatedDate { get { return this.CreatedDate.ToString("MM-dd-yyyy"); } }
        public string WorkItemName { get; set; }
        public int GracePeriod { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityTypeCode { get; set; }
        public string DueDateColor { get; set; }

    }
}
