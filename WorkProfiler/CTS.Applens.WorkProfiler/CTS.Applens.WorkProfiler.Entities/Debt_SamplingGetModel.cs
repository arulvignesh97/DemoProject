using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds DebtSamplingGetModel details
    /// </summary>
    public class DebtSamplingGetModel
    {
        private List<DebtSamplingValues> lstCauseCodeModel = new List<DebtSamplingValues>();
        /// <summary>
        /// Gets or sets lstCauseCodeModel 
        /// </summary>
        public List<DebtSamplingValues> LstCauseCodeModel
        {

                get
                {
                    return lstCauseCodeModel;
                }
                set
                {
                lstCauseCodeModel = value;
                }

        }

        private List<DebtSamplingValues> lstresolutionCodeModel = new List<DebtSamplingValues>();
        /// <summary>
        /// Gets or sets  lstResolutionCodeModel
        /// </summary>
        public List<DebtSamplingValues> LstResolutionCodeModel
        {
            get
            {
                return lstresolutionCodeModel;
            }
            set
            {
                lstresolutionCodeModel = value;
            }
        }

        private List<DebtSamplingValues> lstDebtClassificationModel = new List<DebtSamplingValues>();
        /// <summary>
        /// Gets or sets  lstDebtClassificationModel
        /// </summary>
        public List<DebtSamplingValues> LstDebtClassificationModel

        {
            get
            {
                return lstDebtClassificationModel;
            }
            set
            {
                lstDebtClassificationModel = value;
            }
        }

        private List<DebtSamplingValues> lstAvoidableFlagModel = new List<DebtSamplingValues>();
        /// <summary>
        /// Gets or sets lstAvoidableFlagModel 
        /// </summary>
        public List<DebtSamplingValues> LstAvoidableFlagModel
        {
            get
            {
                return lstAvoidableFlagModel;
            }
            set
            {
                lstAvoidableFlagModel = value;
            }
        }
        private List<DebtSamplingValues> lstResidualDebtModel = new List<DebtSamplingValues>();
        /// <summary>
        /// Gets or sets  lstResidualDebtModel
        /// </summary>
        public List<DebtSamplingValues> LstResidualDebtModel
        {
            get
            {
                return lstResidualDebtModel;
            }
            set
            {
                lstResidualDebtModel = value;
            }
        }      
        /// <summary>
        /// Gets or sets  TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets  TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets  AdditionalText
        /// </summary>
        public string AdditionalText { get; set; }
        /// <summary>
        /// Gets or sets  PresenceOfOptional
        /// </summary>
        public bool PresenceOfOptional { get; set; }
        /// <summary>
        /// Gets or sets  OptionalField
        /// </summary>
        public int OptionalField { get; set; }
        /// <summary>
        /// Gets or sets  ApplicationID
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
        /// Gets or sets  CauseCodeID
        /// </summary>
        public int? CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets  CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets  ResolutionCodeID
        /// </summary>
        public int? ResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets  ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets  DebtClassificationID
        /// </summary>
        public int? DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets  DebtClassificationName
        /// </summary>
        public string DebtClassificationName { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID 
        /// </summary>
        public int? AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets  AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets  ResidualDebtID
        /// </summary>
        public int? ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets  ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }
        /// <summary>
        /// Gets or sets DescBaseWorkPattern 
        /// </summary>
        public string DescBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets  DescSubWorkPattern
        /// </summary>
        public string DescSubWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets  ResBaseWorkPattern
        /// </summary>
        public string ResBaseWorkPattern { get; set; }
        /// <summary>
        /// Gets or sets  ResSubWorkPattern
        /// </summary>
        public string ResSubWorkPattern { get; set; }

    }
    /// <summary>
    /// This class holds DebtSamplingValues details
    /// </summary>
    public class DebtSamplingValues
    {
        /// <summary>
        /// Gets or sets  AttributeType
        /// </summary>
        public string AttributeType { get; set; }
        /// <summary>
        /// Gets or sets  AttributeTypeId
        /// </summary>
        public int AttributeTypeId { get; set; }
        /// <summary>
        /// Gets or sets AttributeTypeValue
        /// </summary>
        public string AttributeTypeValue { get; set; }
    }
}
