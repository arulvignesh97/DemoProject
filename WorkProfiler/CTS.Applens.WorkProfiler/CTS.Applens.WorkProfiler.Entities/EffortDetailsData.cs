using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds EffortDetailsData details
    /// </summary>
    public class EffortDetailsData
    {
        /// <summary>
        /// Gets or sets ClosedTickets
        /// </summary>
        public string ClosedTicket { get; set; }
        /// <summary>
        /// Gets or sets TicketedEffort
        /// </summary>
        public string TicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets NonTicketedEffort
        /// </summary>
        public string NonTicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets ClosedWorkItems
        /// </summary>
        public string ClosedWorkItem { get; set; }
        /// <summary>
        /// Gets or sets WorkItemEffort
        /// </summary>
        public string WorkItemEffort { get; set; }
        /// <summary>
        /// Gets or sets TotalEfforts
        /// </summary>
        public string TotalEffort { get; set; }
    }
}
