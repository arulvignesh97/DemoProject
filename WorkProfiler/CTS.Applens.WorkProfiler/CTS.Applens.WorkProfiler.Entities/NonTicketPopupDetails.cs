using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds NonTicketPopupDetails details
    /// </summary>
    public class NonTicketPopupDetails
    {
        /// <summary>
        /// This is a Method Declaration of LstAppProj !
        /// </summary>
        public List<ApplicationProjectModel> LstAppProj { get; set; }
        /// <summary>
        /// This is a Method Declaration of lstNonTicketActivity !
        /// </summary>
        private  List<NonTicketedActivityModel> lstNonTicketActivity = new List<NonTicketedActivityModel>();
        /// <summary>
        /// Gets or sets LstNonTicketActivity
        /// </summary>
        public List<NonTicketedActivityModel> LstNonTicketActivity
        {
            get
            {
                return lstNonTicketActivity;
            }
            set
            {
                lstNonTicketActivity = value ;
            }
        }
        /// <summary>
        /// Gets or Sets the Excluded Words
        /// </summary>
        public List<NameIDModel> LstExcludedWords { get; set; }
    }
    /// <summary>
    /// This class holds AddTicketNewDetais details
    /// </summary>
    public class AddTicketNewDetails
    {
        /// <summary>
        /// This is a Method Declaration of lstAssignmentGroup !
        /// </summary>
        private  List<TMAssignmentGroupModel> lstAssignmentGroup = new List<TMAssignmentGroupModel>();
        /// <summary>
        /// Gets or sets LstAssignmentGroup
        /// </summary>
        public List<TMAssignmentGroupModel> LstAssignmentGroup
        {
            get { return lstAssignmentGroup; }
            set {   lstAssignmentGroup = value ; }
        }
        /// <summary>
        /// This is a Method Declaration of tmsupporttype
        /// </summary>
        private  TMASupportTypeModel tmSupporttype = new TMASupportTypeModel();
        /// <summary>
        /// Gets or sets tmsupporttype
        /// </summary>
        public TMASupportTypeModel TmSupporttype
        {
            get { return tmSupporttype; }
            set { tmSupporttype = value; }
        }
        /// <summary>
        /// This is a Method Declaration of LstTower !
        /// </summary>
        private  List<TMTowerModel> lstTower = new List<TMTowerModel>();
        /// <summary>
        /// Gets or sets lstTower
        /// </summary>
        public List<TMTowerModel> LstTower
        {
            get { return lstTower; }
            set { lstTower = value; }
        }
        /// <summary>
        /// This is a Method Declaration of tickettypemodel by support type
        /// </summary>
        private  List<TicketTypeNewModel> lstTicketTypeBysupportType = new List<TicketTypeNewModel>();
        /// <summary>
        /// Gets or Sets lstTicketTypeBysupportType
        /// </summary>
        public List<TicketTypeNewModel> LstTicketTypeBysupportType
        {
            get { return lstTicketTypeBysupportType; }
            set { lstTicketTypeBysupportType = value; }
        }

        /// <summary>
        /// This is a Method Declaration of tickettypemodel by support type
        /// </summary>
        private  List<AssignmentTowerMapModel> lstAssignmentTowerMapModel = new List<AssignmentTowerMapModel>();
        /// <summary>
        /// Gets or Sets lstTicketTypeBysupportType
        /// </summary>
        public List<AssignmentTowerMapModel> LstAssignmentTowerMapModel
        {
            get { return lstAssignmentTowerMapModel; }
            set { lstAssignmentTowerMapModel = value; }
        }

    }
    /// <summary>
    /// This class holds ProjectNewDetails details
    /// </summary>
    public class ProjectNewDetails
    {
        /// <summary>
        /// This is a Method Declaration of lststatusListForAdd !
        /// </summary>
        private  List<StatusNewModel> lstStatusListForAdd = new List<StatusNewModel>();
        /// <summary>
        /// Gets or sets LstStatusListForAdd
        /// </summary>
        public List<StatusNewModel> LstStatusListForAdd
        {
            get
            {
                return lstStatusListForAdd;
            }

            set
            {
                lstStatusListForAdd = value;
            }
        }
        /// <summary>
        /// This is a Method Declaration of lstpriorityListForAdd !
        /// </summary>
        private  List<PriorityNewModel> lstPriorityListForAdd = new List<PriorityNewModel>();
        /// <summary>
        /// Gets or sets LstPriorityListForAdd
        /// </summary>
        public List<PriorityNewModel> LstPriorityListForAdd
        {
            get
            {
                return lstPriorityListForAdd;
            }

            set
            {
                 lstPriorityListForAdd = value;
            }
        }
        /// <summary>
        /// This is a Method Declaration of lstticketTypeListForAdd !
        /// </summary>
        private  List<TicketTypeNewModel> lstTicketTypeListForAdd = new List<TicketTypeNewModel>();
        /// <summary>
        /// Gets or sets LstTicketTypeListForAdd
        /// </summary>
        public List<TicketTypeNewModel> LstTicketTypeListForAdd
        {
            get
            {
                return lstTicketTypeListForAdd;
            }

            set
            {
                 lstTicketTypeListForAdd = value;
            }
        }
        /// <summary>
        /// LstTowerDetails
        /// </summary>
        public List<InfraTowerDetails> LstTowerDetails { get; set; }
        /// <summary>
        /// LstAssignmentGroupDetails
        /// </summary>
        public List<AssignmentGroupDetails> LstAssignmentGroupDetails { get; set; }
    }
    /// <summary>
    /// This class holds AssignemntGroupModel details
    /// </summary>
    public class TMAssignmentGroupModel
    {
        public int AssignmentGroupId { get; set; }

        public string AssignmentGroupName { get; set; }
    }
    /// <summary>
    /// This class holds Tower model details
    /// </summary>
    public class TMTowerModel
    {
        public int TowerId { get; set; }

        public string TowerName { get; set; }
    }
    /// <summary>
    /// This class holds Support Type Model details
    /// </summary>
    public class TMASupportTypeModel
    {
        public int SupportTypeId { get; set; }
    }

    /// <summary>
    /// This class holds StatusNewModel details
    /// </summary>
    public class StatusNewModel
    {
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public Int64 StatusId { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or sets TicketStatusID
        /// </summary>
        public Int64 TicketStatusId { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultTicketStatus
        /// </summary>
        public char IsDefaultTicketStatus { get; set; }

    }
    /// <summary>
    /// This class holds PriorityNewModel details
    /// </summary>
    public class PriorityNewModel
    {
        /// <summary>
        /// Gets or sets PriorityID
        /// </summary>
        public Int64 PriorityId { get; set; }
        /// <summary>
        /// Gets or sets PriorityName
        /// </summary>
        public string PriorityName { get; set; }
        /// <summary>
        /// Gets or sets PrioritymasID
        /// </summary>
        public Int64 PriorityMasId { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultPrority
        /// </summary>
        public char IsDefaultPrority { get; set; }

    }
    /// <summary>
    /// This class holds TicketTypeNewModel details
    /// </summary>
    public class TicketTypeNewModel
    {
        /// <summary>
        /// Gets or sets TicketTypeID
        /// </summary>
        public Int64 TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets TicketType
        /// </summary>
        public string TicketType { get; set; }
        /// <summary>
        /// Gets or sets TicketTypemasID
        /// </summary>
        public Int64 TicketTypemasId { get; set; }
        /// <summary>
        /// Gets or sets IsDefaultTicketType
        /// </summary>
        public char IsDefaultTicketType { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
    }
    /// <summary>
    /// This class holds AddTicket Assignment Tower Mapping details
    /// </summary>
    public class AssignmentTowerMapModel
    {
        /// <summary>
        /// Gets or sets AssignmentGroupMapId
        /// </summary>
        public Int64 AssignmentGroupMapId { get; set; }
        /// <summary>
        /// Gets or sets TowerId
        /// </summary>
        public Int64 TowerId { get; set; }
    }
    /// <summary>
    /// This class holds ApplicationProjectModel details
    /// </summary>
    public class ApplicationProjectModel
    {
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneId
        /// </summary>
        public string UserTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string UserTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZoneName
        /// </summary>
        public string ProjectTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZoneName
        /// </summary>
        public string ProjectTimeZoneName { get; set; }
    }
    /// <summary>
    /// This class holds NonTicketedActivityModel details
    /// </summary>
    public class NonTicketedActivityModel
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Gets or sets NonTicketedActivity
        /// </summary>
        public string NonTicketedActivity { get; set; }
    }
    /// <summary>
    /// This class holds AddTicketDetails details
    /// </summary>
    public class AddTicketDetails
    {
        /// <summary>
        /// Gets or sets TicketID
        /// </summary>
        [MaxLength(50)]
        public string TicketId { get; set; }
        /// <summary>
        /// Gets or sets ApplicationID
        /// </summary>
        public Int64 ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// Gets or sets TicketDescription
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Gets or sets DartStatusID
        /// </summary>
        public int DartStatusId { get; set; }
        /// <summary>
        /// Gets or sets PriorityMapId
        /// </summary>
        public int PriorityMapId { get; set; }
        /// <summary>
        /// Gets or sets TicketTypeMapID
        /// </summary>
        public int TicketTypeMapId { get; set; }
        /// <summary>
        /// Gets or sets IsSDTicket
        /// </summary>
        public int IsSDTicket { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public int IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets FirstDayofWeek
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FirstDayofWeek { get; set; }
        /// <summary>
        /// Gets or sets LastDayofWeek
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string LastDayofWeek { get; set; }
        /// <summary>
        /// Gets or sets OpenDate
        /// </summary>
        public DateTime OpenDate { get; set; }
        /// <summary>
        /// Gets or sets OpenDateUI
        /// </summary>
        [MaxLength(200)]
        public string OpenDateUI { get; set; }
        /// <summary>
        /// Gets or sets UserTimeZone
        /// </summary>
        [MaxLength(400)]
        public string UserTimeZone { get; set; }
        /// <summary>
        /// Gets or sets ProjectTimeZone
        /// </summary>
        [MaxLength(400)]
        public string ProjectTimeZone { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int TowerID { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int AssignmentGroupId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(400)]
        public string AssignmentGroup { get; set; }


    }
    /// <summary>
    /// InfraTowerDetails- Class to hold the Tower Dropdown in Errorlog Correction module
    /// </summary>
    public class InfraTowerDetails
    {
        /// <summary>
        /// TowerID
        /// </summary>
        public Int64 TowerId { get; set; }
        /// <summary>
        /// TowerName
        /// </summary>
        public string Tower { get; set; }
    }
    /// <summary>
    /// AssignmentGroupDetails- Class to hold the AssignmentGroup Dropdown in Errorlog Correction module
    /// </summary>
    public class AssignmentGroupDetails
    {
        /// <summary>
        /// AssignmentGroupID
        /// </summary>
        public Int64 AssignmentGroupId { get; set; }
        /// <summary>
        /// AssignmentGroupName
        /// </summary>
        public string AssignmentGroupName { get; set; }
    }
    /// <summary>
    /// ProjectMaster- Class to get the ProjectMaster Details
    /// </summary>
    public class UserProjectMaster
    {
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// ProjectManagerID
        /// </summary>
        public string ProjectManagerId { get; set; }
        /// <summary>
        /// TSApproverID
        /// </summary>
        public string TSApproverId { get; set; }
        /// <summary>
        /// TSApproverName
        /// </summary>
        public string TSApproverName { get; set; }

        public string AdminId { get; set; }

    }

    /// <summary>
    /// UserCustomerDetails- Class to get the Customer Details
    /// </summary>
    public class UserCustomerDetails
    {
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// EmployeeIDName
        /// </summary>
        public string EmployeeIdName { get; set; }
    }

    /// <summary>
    /// LoginMasterDetails- Class to get the LoginMaster Details
    /// </summary>
    public class LoginMasterDetails
    {
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// EmployeeIDName
        /// </summary>
        public string EmployeeIdName { get; set; }
        /// <summary>
        /// UserRoleMappingID
        /// </summary>
        public string UserRoleMappingId { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// AccessLevelSourceID
        /// </summary>
        public string AccessLevelSourceId { get; set; }
        /// <summary>
        /// AccessLevelID
        /// </summary>
        public string AccessLevelId { get; set; }
    }
    public class UserMasterDetails
    {
        public List<UserProjectMaster> ListUserProjectMaster { get; set; }
        public List<UserCustomerDetails> ListCustomerDetails { get; set; }
        public List<LoginMasterDetails> ListLoginMasterDetails { get; set; }
        public string EmployeeId { get; set; }
    }

}
