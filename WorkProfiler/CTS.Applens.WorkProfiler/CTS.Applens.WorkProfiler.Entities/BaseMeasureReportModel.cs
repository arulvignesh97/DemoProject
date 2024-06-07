using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds BaseMeasureReportModel details
    /// </summary>
    public class BaseMeasureReportModel
    {
        /// <summary>
        /// Gets or sets MetricStartDate
        /// </summary>
        public string MetricStartDate { get; set; }
        /// <summary>
        /// Gets or sets MetricEndDate
        /// </summary>
        public string MetricEndDate { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets MetricName
        /// </summary>
        public string MetricName { get; set; }
        /// <summary>
        /// Gets or sets MetricTypeDesc
        /// </summary>
        public string MetricTypeDesc { get; set; }
        /// <summary>
        /// Gets or sets MainspringPriorityName
        /// </summary>
        public string MainspringPriorityName { get; set; }
        /// <summary>
        /// Gets or sets MainspringSUPPORTCATEGORYName
        /// </summary>
        public string MainspringSupportCategoryName { get; set; }
        /// <summary>
        /// Gets or sets TechnologyLanguageNameShortDESC
        /// </summary>
        public string TechnologyLanguageNameShortDesc { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureName
        /// </summary>
        public string BaseMeasureName { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureValue
        /// </summary>
        public string BaseMeasureValue { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets MetricID
        /// </summary>
        public int MetricId { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureID
        /// </summary>
        public int BaseMeasureId { get; set; }
    }
}
