using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models.Work_Items
{
    public class MasterDataModel
    {
        public MasterDataModel()
        {
            ListWorkItemTypeData = new List<WorkTypeModel>();
            ListApplicationData = new List<ApplicationDataModel>();
            ListPriorityData = new List<PriorityModel>();
            ListStatusData = new List<StatusModel>();
            ListMandateAttributes = new List<MandateAttributes>();
            ListEpic = new List<Epic>();
            ListUserStory = new List<Userstory>();
            ListSprint= new List<SprintModel>();
            ListPOD = new List<PODDetailsModel>();
            ListBugPhase = new List<BugPhaseModel>();


        }
        /// <summary>
        /// Gets or Sets listApplicationData
        /// </summary>
        public List<ApplicationDataModel> ListApplicationData { get; set; }
        /// <summary>
        /// Gets or Sets listPriorityData
        /// </summary>
        public List<PriorityModel> ListPriorityData { get; set; }
        /// <summary>
        /// Gets or Sets listWorkItemTypeData
        /// </summary>
        public List<WorkTypeModel> ListWorkItemTypeData { get; set; }
        /// <summary>
        /// Gets or Sets listStatusData
        /// </summary>
        public List<StatusModel> ListStatusData { get; set; }
        /// <summary>
        /// Gets or Sets listMandateAttributes
        /// </summary>
        public List<MandateAttributes> ListMandateAttributes { get; set; }
        /// <summary>
        /// Gets or Sets listEpic
        /// </summary>
        public List<Epic> ListEpic { get; set; }
        /// <summary>
        /// Gets or Sets listUserStory
        /// </summary>
        public List<Userstory> ListUserStory { get; set; }
        /// <summary>
        /// Gets or Sets listSprint
        /// </summary>
        public List<SprintModel> ListSprint { get; set; }
        /// <summary>
        /// Gets or Sets listPOD
        /// </summary>
        public List<PODDetailsModel> ListPOD { get; set; }
        /// <summary>
        /// Gets or Sets Bug Phase
        /// </summary>
        public List<BugPhaseModel> ListBugPhase { get; set; }
    }
    public class ApplicationDataModel
    {
        /// <summary>
        /// Gets or Sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or Sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or Sets ExecutionMethod
        /// </summary>
        public Int64 ExecutionMethod { get; set; }
    }

    public class PriorityModel
    {
        /// <summary>
        /// Gets or Sets PriorityId
        /// </summary>
        public long PriorityId { get; set; }
        /// <summary>
        /// Gets or Sets PriorityMapId
        /// </summary>
        public long PriorityMapId { get; set; }
        /// <summary>
        /// Gets or Sets PriorityName
        /// </summary>
        public string PriorityName { get; set; }
    }


    public class WorkTypeModel
    {
        /// <summary>
        /// Gets or Sets WorkTypeId
        /// </summary>
        public long WorkTypeId { get; set; }
        /// <summary>
        /// Gets or Sets WorkTypeMapId
        /// </summary>
        public long WorkTypeMapId { get; set; }
        /// <summary>
        /// Gets or Sets MasWorkTypeName
        /// </summary>
        public string MasWorkTypeName { get; set; }
        /// <summary>
        /// Gets or Sets WorkTypeName
        /// </summary>
        public string WorkTypeName { get; set; }
        /// <summary>
        /// Gets or Sets IsEffortTracking
        /// </summary>
        public bool? IsEffortTracking { get; set; }
    }

    public class StatusModel
    {
        /// <summary>
        /// Gets or Sets StatusId
        /// </summary>
        public long StatusId { get; set; }
        /// <summary>
        /// Gets or Sets 
        /// </summary>
        public long StatusMapId { get; set; }
        /// <summary>
        /// Gets or Sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemDetailsId
        /// </summary>
        public long WorkItemDetailsId { get; set; }
        /// <summary>
        /// Gets or Sets WorkTypeMapId
        /// </summary>
        public long WorkTypeMapId { get; set; }
    }

    public class Epic
    {
        /// <summary>
        /// Gets or Sets WorkItemDetailsId
        /// </summary>
        public long WorkItemDetailsId { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemId
        /// </summary>
        public string WorkItemId { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemTitle
        /// </summary>
        public string WorkItemTitle { get; set; }
        /// <summary>
        /// Gets or Sets StatusMapId
        /// </summary>
        public long StatusMapId { get; set; }
        /// <summary>
        /// Gets or Sets StatusId
        /// </summary>
        public long StatusId { get; set; }
    }
    public class Userstory
    {
        /// <summary>
        /// Gets or Sets WorkItemDetailsId
        /// </summary>
        public long WorkItemDetailsId { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemId
        /// </summary>
        public string WorkItemId { get; set; }
        /// <summary>
        /// Gets or Sets WorkItemTitle
        /// </summary>
        public string WorkItemTitle { get; set; }
        /// <summary>
        /// Gets or Sets StatusMapId
        /// </summary>
        public long StatusMapId { get; set; }
        /// <summary>
        /// Gets or Sets StatusId
        /// </summary>
        public long StatusId { get; set; }
    }
    public class SprintModel
    {
        /// <summary>
        /// Gets or Sets SprintDetailsId
        /// </summary>
        public long SprintDetailsId { get; set; }
        /// <summary>
        /// Gets or Sets SprintName
        /// </summary>
        public string SprintName { get; set; }
        /// <summary>
        /// Gets or Sets SprintStartDate
        /// </summary>
        public DateTime? SprintStartDate { get; set; }
        /// <summary>
        /// Gets or Sets SprintEndDate
        /// </summary>
        public DateTime? SprintEndDate { get; set; }
    }
    public class PODDetailsModel
    {
        /// <summary>
        /// Gets or Sets PODDetailId
        /// </summary>
        public Int64 PODDetailID { get; set; }
        /// <summary>
        /// Gets or Sets PODName
        /// </summary>
        public string PODName { get; set; }
    }
    public class BugPhaseModel
    {
        /// <summary>
        /// Gets or Sets Bug Phase Model
        /// </summary>
        public BugPhaseModel()
        {
            BugPhaseType = new List<BugTypeModel>();
        }
        /// <summary>
        /// Gets or Sets BugPhaseId
        /// </summary>
        public long BugPhaseID { get; set; }
        /// <summary>
        /// Gets or Sets BugPhaseName
        /// </summary>
        public string BugPhaseName { get; set; }
        /// <summary>
        /// Gets or Sets BugPhaseType
        /// </summary>
        public List<BugTypeModel> BugPhaseType { get; set; }

    }
    public class BugTypeModel
    {
        /// <summary>
        /// Gets or Sets BugTypeID
        /// </summary>
        public int BugTypeID { get; set; }
        /// <summary>
        /// Gets or Sets BugTypeName
        /// </summary>
        public string BugTypeName { get; set; }
        /// <summary>
        /// Gets or Sets BugPhaseTypeMapId
        /// </summary>
        public int BugPhaseTypeMapId { get; set; }
    }

}
