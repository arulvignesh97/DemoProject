using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds DebtOverrideReview details
    /// </summary>
    public class DebtOverrideReview
        {
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }    
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets AssignedTo
        /// </summary>
        public string AssignedTo { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public int IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsApproved
        /// </summary>
        public int IsApproved { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }       
        /// <summary>
        /// Gets or sets DebtClassification
        /// </summary>
        public string DebtClassification { get; set; }
        /// <summary>
        /// Gets or sets NatureOfTheTicketName
        /// </summary>
        public string NatureOfTheTicketName { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public Int64 DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationMapID
        /// </summary>
        public Int64 DebtClassificationMapId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebt
        /// </summary>
        public string ResidualDebt { get; set; }      
        /// <summary>
        /// Gets or sets ResidualDebtMapID
        /// </summary>
        public Int64 ResidualDebtMapId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public Int64 AvoidableFlag { get; set; }
        /// <summary>
        /// Gets or Sets AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeMapID
        /// </summary>
        public Int64 ResolutionCodeMapId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public long ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets CauseID
        /// </summary>
        public long CauseId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeMapID
        /// </summary>
        public Int64 CauseCodeMapId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        /// <summary>
        /// Gets or sets FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        /// Gets or sets FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        /// Gets or sets FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        /// Gets or sets FlexField4
        /// </summary>
        public string FlexField4 { get; set; }
        /// <summary>
        /// Gets or sets FlexField1ProjectWise
        /// </summary>
        public string FlexField1ProjectWise { get; set; }
        /// <summary>
        /// Gets or sets FlexField2ProjectWise
        /// </summary>
        public string FlexField2ProjectWise { get; set; }
        /// <summary>
        /// Gets or sets FlexField3ProjectWise
        /// </summary>
        public string FlexField3ProjectWise { get; set; }
        /// <summary>
        /// Gets or sets FlexField4ProjectWise
        /// </summary>
        public string FlexField4ProjectWise { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField1Modifed
        /// </summary>
        public string IsFlexField1Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField2Modifed
        /// </summary>
        public string IsFlexField2Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField3Modifed
        /// </summary>
        public string IsFlexField3Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField4Modifed
        /// </summary>
        public string IsFlexField4Modified { get; set; }
        /// <summary>
        /// Gets or sets IsAHTagged
        /// </summary>
        public bool IsAHTagged { get; set; }
    }

    /// <summary>
    /// This class holds DebtOverrideReviewApprove details
    /// </summary>
    public class DebtOverrideReviewApprove
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        public string TicketID { get; set; }

        /// <summary>
        /// Gets or sets DebtClassificationMapID
        /// </summary>
        public int DebtClassificationMapID { get; set; }

        /// <summary>
        /// Gets or sets ResolutionCodeMapID
        /// </summary>
        public int ResolutionCodeMapID { get; set; }

        /// <summary>
        /// Gets or sets CauseCodeMapID
        /// </summary>
        public int CauseCodeMapID { get; set; }

        /// <summary>
        /// Gets or sets ResidualDebtMapID
        /// </summary>
        public int ResidualDebtMapID { get; set; }

        /// <summary>
        /// Gets or sets AvoidableFlag
        /// </summary>
        public int AvoidableFlag { get; set; }

        /// <summary>
        /// Gets or sets AssignedTo
        /// </summary>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        /// Gets or sets FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        /// Gets or sets FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        /// Gets or sets FlexField4
        /// </summary>
        public string FlexField4 { get; set; }

        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        public string EmployeeID { get; set; }
        
        /// <summary>
        /// Gets or sets IsFlexField1Modifed
        /// </summary>
        public string IsFlexField1Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField2Modifed
        /// </summary>
        public string IsFlexField2Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField3Modifed
        /// </summary>
        public string IsFlexField3Modified { get; set; }
        /// <summary>
        /// Gets or sets IsFlexField4Modifed
        /// </summary>
        public string IsFlexField4Modified { get; set; }
       
    }

    /// <summary>
    /// This class holds ResolutionModelDebt details
    /// </summary>
    public class ResolutionModelDebt
        {
        /// <summary>
        /// Gets or sets ResolutionID
        /// </summary>
        public string ResolutionId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCode
        /// </summary>
        public string ResolutionCode { get; set; }
        }
    /// <summary>
    /// This class holds DebtClassificationModelDebt details
    /// </summary>
    public class DebtClassificationModelDebt
        {
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public string DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationName
        /// </summary>
        public string DebtClassificationName { get; set; }
        }
    /// <summary>
    /// This class holds CauseModelDebt details
    /// </summary>
    public class CauseModelDebt
        {
        /// <summary>
        /// Gets or sets CauseID
        /// </summary>
        public string CauseId { get; set; }
        /// <summary>
        /// Gets or sets CauseCode
        /// </summary>
        public string CauseCode { get; set; }
        }
    /// <summary>
    /// This class holds ResidualModelDebt details
    /// </summary>
    public class ResidualModelDebt
    {
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public string ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtName
        /// </summary>
        public string ResidualDebtName { get; set; }
    }
    /// <summary>
    /// This class holds AvoidableModelFlag details
    /// </summary>
    public class AvoidableModelFlag
    {
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public string AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagName
        /// </summary>
        public string AvoidableFlagName { get; set; }
    }
    /// <summary>
    /// This class holds NatureOfTicket details
    /// </summary>
    public class NatureOfTicket
    {
        /// <summary>
        /// Gets or sets NatureOfTheTicketId
        /// </summary>
        public string NatureOfTheTicketId { get; set; }
        /// <summary>
        /// Gets or sets NatureOfTheTicketName
        /// </summary>
        public string NatureOfTheTicketName { get; set; }
    }
    /// <summary>
    /// This class holds ReasonForResidual details
    /// </summary>
    public class ReasonForResidual
    {
        /// <summary>
        /// Gets or sets ReasonResidualID
        /// </summary>
        public string ReasonResidualId { get; set; }
        /// <summary>
        /// Gets or sets ReasonResidualName
        /// </summary>
        public string ReasonResidualName { get; set; }
    }
    /// <summary>
    /// This class holds ProjectDataDictionary details
    /// </summary>
    public class ProjectDataDictionary
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public Int64 CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeID
        /// </summary>
        public Int64 ResolutionCodeId { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationID
        /// </summary>
        public Int64 DebtClassificationId { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagID
        /// </summary>
        public Int64 AvoidableFlagId { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtID
        /// </summary>
        public Int64 ResidualDebtId { get; set; }
        /// <summary>
        /// Gets or sets ReasonForResidual
        /// </summary>
        public Int64 ReasonForResidual { get; set; }
        /// <summary>
        /// Gets or sets ExpectedCompletionDate
        /// </summary>
        public string ExpectedCompletionDate { get; set; }
        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }
    }
    /// <summary>
    /// This class holds ProjectDataDictionaryDelete details
    /// </summary>
    public class ProjectDataDictionaryDelete
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets CauseCodeID
        /// </summary>
        public Int64 CauseCodeId { get; set; }
        /// <summary>
        /// Gets or sets ResolutionCodeID
        /// </summary>
        public Int64 ResolutionCodeId { get; set; }
    }
    /// <summary>
    /// This class holds TicketRole details
    /// </summary>
    public class TicketRole
    {
        /// <summary>
        /// Gets or sets RoleID
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// Gets or sets RoleName
        /// </summary>
        public string RoleName { get; set; }
    }
    /// <summary>
    /// This class holds MapFlexFields details
    /// </summary>
    public class MapFlexFields
    {
        /// <summary>
        /// Gets or sets ServiceColumn
        /// </summary>
        public string ServiceColumn { get; set; }
        /// <summary>
        /// Gets or sets ProjectColumn
        /// </summary>
        public string ProjectColumn { get; set; }
    }
}
