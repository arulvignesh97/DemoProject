using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds CustomerModel details
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets CustomerID 
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Gets or sets CustomerName 
        /// </summary>
        public string CustomerName { get; set; }
    }
    /// <summary>
    /// This class holds TimeZoneInfoByCustomer details
    /// </summary>
    public class TimeZoneInfoByCustomer
    {
        /// <summary>
        /// Gets or sets  UserTimeZoneId
        /// </summary>
        public string UserTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets  UserTimeZoneName
        /// </summary>
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets  TimeSheetDisplayMessage
        /// </summary>
        public string TimeSheetDisplayMessage { get; set; }
        /// <summary>
        /// Gets or sets  ProjectTimeZoneId
        /// </summary>
        public string ProjectTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets  ProjectTimeZoneName
        /// </summary>
        public string ProjectTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets  CurrentDate
        /// </summary>
        public string CurrentDate { get; set; }
    }
}
