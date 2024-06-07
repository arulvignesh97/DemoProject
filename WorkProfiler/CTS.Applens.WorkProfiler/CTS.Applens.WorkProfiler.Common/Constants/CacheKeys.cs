using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Class to hold keys for Caching
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Key to hold the associate details
        /// </summary>
        public static readonly string AssociteDetails                 = "WP_Associate_{0}";

        /// <summary>
        /// Key to hold the project machine learning status
        /// </summary>
        public static readonly string ProjectMLStatus                 = "MLStatus_{0}";

        /// <summary>
        /// Key to hold the project ticket description mapping status
        /// </summary>
        public static readonly string ProjectTicketDescStatus         = "TicketDescSts_{0}";
        public static readonly string HiddenFieldsModel = "objHiddenFieldsModel{0}"; 
        public static readonly string TimeZoneInfoByEmployeeID = "TimeZoneInfoByEmployeeID_{0}"; 
        public static readonly string UserWiseCustomer = "UserWiseCustomer_{0}"; 
        public static readonly string CustomerId = "CustomerId_{0}"; 
        public static readonly string UserId = "UserId_{0}";
        public static readonly string AllowEncrypt = "AllowEncrypt_{0}";
    }
}
