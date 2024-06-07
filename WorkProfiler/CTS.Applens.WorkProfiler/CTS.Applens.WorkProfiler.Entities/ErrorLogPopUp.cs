using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// ErrorLogPopUp
    /// </summary>
    public class ErrorLogPopUp
    {

        public string ApplicationName { get; set; }
        public string CauseCode { get; set; }
        public string ResolutionCode { get; set; }
        public string DebtClassification { get; set; }
        public string AvoidableFlag { get; set; }
        public string ResidualDebt { get; set; }
        public string ReasonForResidual { get; set; }
        public string ExpectedCompletionDate { get; set; }
        public string Remarks { get; set; }

    }
}