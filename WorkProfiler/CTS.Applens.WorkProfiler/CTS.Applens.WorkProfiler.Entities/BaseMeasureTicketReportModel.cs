using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds BaseMeasureTicketReportModel details 
    /// </summary>
    public class BaseMeasureTicketReportModel
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
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
        /// Gets or sets MainspringPriorityName
        /// </summary>
        public string MainspringPriorityName { get; set; }
        /// <summary>
        /// Gets or sets MainspringSUPPORTCATEGORYName
        /// </summary>
        public string MainspringSupportCategoryName { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryBaseName
        /// </summary>
        public string TicketSummaryBaseName { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryBaseID
        /// </summary>
        public string TicketSummaryBaseId { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryValue
        /// </summary>
        public string TicketSummaryValue { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public string ServiceId { get; set; }
    }
}
