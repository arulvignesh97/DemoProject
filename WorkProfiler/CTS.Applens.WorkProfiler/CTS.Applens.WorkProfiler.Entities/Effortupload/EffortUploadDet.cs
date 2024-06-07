using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models.EffortUpload
{
    public class EffortUploadDet
    {
        public Int32 ProjectID { get; set; }
        public string TicketID { get; set; }
        public string TrackID { get; set; }
        public string ServiceName { get; set; }
        public string ActivityName { get; set; }
        public string TicketType { get; set; }
        public string CognizantID { get; set; }

        public bool IsCognizant { get; set; }

        public bool IsEffortTrackActivityWise { get; set; }
        public decimal Hours { get; set; }
        public string HoursCheck { get; set; }
        public DateTime TimeSheetDate { get; set; }
        public string TimeSheetDateCheck { get; set; }

        public string Remarks { get; set; }
        public bool IsDaily { get; set; }

        public string SuggestedActivity { get; set;}

        public string SuggestedRemarks { get; set; }
        public string Type { get; set; }


    }

    public class EffortUploadSuccessCount
    {
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public string EffortErrorLogFile { get; set; }
        public string Status { get; set; }
        public string TrackID { get; set; }

        public Int32 ProjectID { get; set; }

    }
    public class EffortUploadResultGrid
    {
        public List<EffortUploadDet> LstErrorLogDetails { get; set; }
        public EffortUploadSuccessCount EffortResultCount { get; set; }
        public string Status { get; set; }
    }
   
    public class effdup
    {
        public string TicketID { get; set; }
        public string TicketType { get; set; }
        public string ServiceName { get; set; }
        public string ActivityName { get; set; }
        public string CognizantID { get; set; }
        public DateTime TimeSheetDate { get; set; }
        public string Remarks { get; set; }
        public string TimeSheetDateCheck { get; set; }
        public string Type { get; set; }
    }

    public class ErrorLogUploadDet
    {
        public Int32 ProjectID { get; set; }
        public string TicketID { get; set; }
        public string ServiceName { get; set; }
        public string ActivityName { get; set; }
        public string TicketType { get; set; }
        public bool IsCognizant { get; set; }

        public DateTime TimeSheetDate { get; set; }

        public string Remarks { get; set; }

    }

    public class ErrorExcell
    {
        public string Type { get; set; }
        public string TicketID { get; set; }
        public string TimeSheetDate { get; set; }
        public string CognizantID { get; set; }
        public string ActivityName { get; set; }
        public string Remarks { get; set; }
        public string TicketType { get; set; }
        

    }
    public class ErrorExcellCust
    {
        public string TicketID { get; set; }
        public string Remarks { get; set; }

        public string TimeSheetDate { get; set; }
        public string CognizantID { get; set; }
        public string ActivityName { get; set; }
        public string TicketType { get; set; }


    }
    public class ErrorExcellInfra
    {
        public string TicketID { get; set; }
        public string TimeSheetDate { get; set; }        
        public string CognizantID { get; set; }
        public string ActivityName { get; set; }
        public string Remarks { get; set; }


    }

    public class OpportunityModel
    {
        public int ApplensOpportunityId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public string ReleasePlanName { get; set; }
        public bool IsDeleted { get; set; }
        public string TicketType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ISpaceOpportunityId { get; set; }
        public DateTime? TriggeredDate { get; set; }
        public DateTime? ISpaceJobDate { get; set; }
        public string ISpaceStatus { get; set; }
        public int? ISpaceJobStatus { get; set; }
        public string EsaProjectID { get; set; }

    }
    public class HealAutomationTicketModel
    {


        public int ApplensIdeaId { get; set; }
        public int ProjectPatternMapID { get; set; }
        public string HealingTicketID { get; set; }
        public string TicketType { get; set; }
        public int? DARTStatusID { get; set; }
        public string Assignee { get; set; }
        public int? ApplicationID { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? PriorityID { get; set; }
        public int? IsManual { get; set; }
        public string IsPushed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? IsMappedToProblemTicket { get; set; }
        public decimal? PlannedEffort { get; set; }
        public int? HealTypeId { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int? ApplensOpportunityId { get; set; }
        public string TicketDescription { get; set; }
        public string SolutionType { get; set; }
        public string SolutionTypeName { get; set; }
        public bool IsDormant { get; set; }
        public DateTime? DormantCreatedDate { get; set; }
        public bool MarkAsDormant { get; set; }
        public DateTime? MarkAsDormantDate { get; set; }
        public string MarkAsDormantComments { get; set; }
        public string MarkAsDormantBy { get; set; }
        public string ReasonForRepetition { get; set; }
        public string ReasonForCancellation { get; set; }
        public decimal? ActualEffortReduction { get; set; }
        public decimal? PlannedEffortReduction { get; set; }
        public decimal? Scope { get; set; }
        public string ImplementationStatus { get; set; }
        public decimal? SavingsHardDollarActualCognizant { get; set; }
        public decimal? SavingsHardDollarActualCustomer { get; set; }
        public decimal? SavingsHardDollarPlannedCognizant { get; set; }
        public decimal? SavingsHardDollarPlannedCustomer { get; set; }
        public decimal? SavingsSoftDollarActualCognizant { get; set; }
        public decimal? SavingsSoftDollarActualCustomer { get; set; }
        public decimal? SavingsSoftDollarPlannedCognizant { get; set; }
        public decimal? SavingsSoftDollarPlannedCustomer { get; set; }
        public bool? IsMandatory { get; set; }
        public decimal? IncidentReductionMonth { get; set; }
        public decimal? EffortReductionMonth { get; set; }
        public int? ISpaceIdeaId { get; set; }
        public DateTime? TriggeredDate { get; set; }
        public DateTime? ISpaceJobDate { get; set; }
        public int? ProjectID { get; set; }
        public string EsaProjectID { get; set; }



    }
    public class OpportunityDetail
    {
        public List<OpportunityModel> Opportunity { get; set; }
        public List<HealAutomationTicketModel> Idea { get; set; }


    }
    public class IspaceIdea
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int? ISpaceIdeaId { get; set; }
        public int? ApplensTicketId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class IspaceOpportunity
    {

        public int Id { get; set; }
        public string Status { get; set; }
        public int? ISpaceOpportunityId { get; set; }
        public int? ApplensOpportunityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class lstIspaceOpportunity {

        public List<IspaceOpportunity> lstOpportunity { get; set; }

    }
    public class ISpaceOpportunityModel
    {
        public int Id
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public int ISpaceOpportunityId
        {
            get; set;
        }
        public int ApplensOpportunityId
        {
            get; set;
        }
        public DateTime? CreatedDate
        {
            get; set;
        }
        public DateTime? ModifiedDate
        {
            get; set;
        }

    }
    public class ISpaceIdeaModel
    {

        public int Id
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public int ISpaceIdeaId
        {
            get; set;
        }
        public int ApplensTicketId
        {
            get; set;
        }
        public DateTime? CreatedDate
        {
            get; set;
        }
        public DateTime? ModifiedDate
        {
            get; set;
        }


    }
    public class IspaceOpportunityDetail
    {
        public List<ISpaceOpportunityModel> Opportunity { get; set; }
        public List<ISpaceIdeaModel> Idea { get; set; }


    }
    public class inputparam
    {
        [MaxLength(int.MaxValue)]
        public string Path { get; set; }
        public Int32 ProjectID { get; set; }
        public bool IsCognizant { get; set; }
        public bool IsEffortTrackActivityWise { get; set; }
        public bool IsDaily { get; set; }
        public Int64 TrackID { get; set; }
    }
}