using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{


    /// <summary>
    /// This class holds SpGetDebtAvoidResidual details
    /// </summary>
    public class GetDebtAvoidResidual
    {
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public int DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public int AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public int ResidualDebt { get; set; }
    }
}