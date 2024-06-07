using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds DebtSamplingModel details
    /// </summary>
    public class DebtSamplingModel
    {  
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets AdditionalText
        /// </summary>
        public string AdditionalText { get; set; }
        /// <summary>
        /// Gets or sets DescBaseWorkPattern
        /// </summary>
        public string DescBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets DescSubWorkPattern
        /// </summary>
        public string DescSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResBaseWorkPattern
        /// </summary>
        public string ResBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResSubWorkPattern
        /// </summary>
        public string ResSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets PresenceOfOptional
        /// </summary>
        public bool PresenceOfOptional { get; set; }
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public int OptionalField { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public int? ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets the TowerID
        /// </summary>
        public int TowerId { get; set; }
        /// <summary>
        /// Gets or Sets the TowerName
        /// </summary>
        public string TowerName { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public int? CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeID
        /// </summary>
        public int? ResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public int? DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationName
        /// </summary>
        public string DebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public int? AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public int? ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }        
    }
    
    /// <summary>
    /// This class holds GetDebtSamplingDetails details
    /// </summary>
    public class GetDebtSamplingDetails
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets AdditionalText
        /// </summary>
        public string AdditionalText { get; set; }
        /// <summary>
        /// Gets or sets PresenceOfOptional
        /// </summary>
        public bool PresenceOfOptional { get; set; }

        public int OptionalField { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationId
        /// </summary>
        public string DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagId
        /// </summary>
        public string AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtId
        /// </summary>
        public string ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets DescBaseWorkPattern
        /// </summary>
        public string DescBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets DescSubWorkPattern
        /// </summary>
        public string DescSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResBaseWorkPattern
        /// </summary>
        public string ResBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ResSubWorkPattern
        /// </summary>
        public string ResSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or Sets TowerID
        /// </summary>
        public Int32 TowerId { get; set; }
    }

}
