using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// Class is used to get ALM Configured Details
    /// </summary>
    public class ALMConfiguredDetails
    {
        /// <summary>
        /// Gets or sets the ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        ///  Gets or sets the IsALMToolConfigured
        /// </summary>
        public bool IsALMToolConfigured { get; set; }
        /// <summary>
        ///  Gets or sets the ScopeId
        /// </summary>
        public int ScopeId { get; set; }
    }
}
