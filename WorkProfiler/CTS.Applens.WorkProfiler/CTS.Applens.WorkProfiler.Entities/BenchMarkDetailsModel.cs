using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    ///  This class holds BenchMarkDetailsModel details
    /// </summary>
    public class BenchMarkDetailsModel
    {
        /// <summary>
        /// Gets or Sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or Sets BenchMarkLevel
        /// </summary>
        public int? BenchMarkLevel { get; set; }
        /// <summary>
        /// Gets or Sets BenchMarkValue
        /// </summary>
        public decimal BenchMarkValue { get; set; }
    }
}
