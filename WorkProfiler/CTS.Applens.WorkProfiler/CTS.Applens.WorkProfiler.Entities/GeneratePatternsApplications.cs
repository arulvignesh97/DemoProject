using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// Used to generate patterns functionality
    /// </summary>
    public class GeneratePatternsApplications
    {
        /// <summary>
        /// Get or set ApplicationID
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// Get or set ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Get or set PortfolioID
        /// </summary>
        public int PortfolioId { get; set; }
        /// <summary>
        /// Get or set PortfolioName
        /// </summary>
        public string PortfolioName { get; set; }
        /// <summary>
        /// Get or set AppGroupID
        /// </summary>
        public int AppGroupId { get; set; }
        /// <summary>
        /// Get or set AppGroupName
        /// </summary>
        public string AppGroupName { get; set; }
    }
}
