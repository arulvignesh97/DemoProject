using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class TaskDetailsList holds Task List details 
    /// </summary>
    public class TaskDetailsList
    {
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets TaskID
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// Gets or sets TaskName
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// Gets or sets URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Gets or sets TaskDetails
        /// </summary>
        public string TaskDetails { get; set; }
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets RefreshedTime
        /// </summary>
        public string RefreshedTime { get; set; }
        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets CreatedTime
        /// </summary>
        public string CreatedTime { get; set; }
        /// <summary>
        /// Gets or sets ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets ModifiedTime
        /// </summary>
        public string ModifiedTime { get; set; }
        /// <summary>
        /// Gets or sets TaskType
        /// </summary>
        public string TaskType { get; set; }
        /// <summary>
        /// Gets or sets ExpiryDate
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// Gets or sets Duedate
        /// </summary>
        public string DueDate { get; set; }
        /// <summary>
        /// Gets or sets Read
        /// </summary>
        public string Read { get; set; }
        /// <summary>
        /// Gets or sets ExpiryAfterRead
        /// </summary>
        public string ExpiryAfterRead { get; set; }
        /// <summary>
        /// Gets or sets AccountID
        /// </summary>
        public long AccountId { get; set; }
    }
}