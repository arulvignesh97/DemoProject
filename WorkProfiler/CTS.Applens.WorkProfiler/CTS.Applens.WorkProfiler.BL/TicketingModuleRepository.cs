using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.Work_Items;
using System;
using System.Collections.Generic;
using System.Data;
using TicketingModule = CTS.Applens.WorkProfiler.DAL.TicketingModuleRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    public class TicketingModuleRepository
    {
        //main function to retrive fields
        /// <summary>
        /// This Method Is Used To GetWeeklyTicketDetails
        /// </summary>
        /// <param name="CustomerID">This parameter holds CustomerID value</param>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <param name="FirstDateOfWeek">This parameter holds FirstDateOfWeek value</param>
        /// <param name="LastDateOfWeek">This parameter holds LastDateOfWeek value</param>
        /// <returns></returns>
        public TimeSheetModel GetWeeklyTicketDetails(string CustomerID, string EmployeeID, string FirstDateOfWeek,
            string LastDateOfWeek, string Mode, List<TicketIDSupport> TicketList, string Tickets, string ProjectID,
            int? isCognizant)
        {
            try
            {
                return new TicketingModule().GetWeeklyTicketDetails(CustomerID, EmployeeID, FirstDateOfWeek, LastDateOfWeek, Mode, 
                                                                             TicketList, Tickets, ProjectID, isCognizant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to create Data Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns>Methos returns table</returns>

        public DataTable ToDataTable<T>(List<T> items)
        {
            try
            {
                return new TicketingModule().ToDataTable<T>(items);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketIdByCustomerID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GetTicketIdByCustomerID(Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetTicketIdByCustomerID(CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To BindTimesheetInfo
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public TimeSheetModel BindTimesheetInfo(DataSet ds)
        {
            try
            {
                return new TicketingModule().BindTimesheetInfo(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To BindTimesSheetInfoFromDataSet
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public TimeSheetModel BindTimesSheetInfoFromDataSet(DataSet ds, DataSet dsMaster,
            DataSet dsServiceBenchMark, DataSet dsADMDetails)
        {
            try
            {
                return new TicketingModule().BindTimesSheetInfoFromDataSet(ds, dsMaster, dsServiceBenchMark, dsADMDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method is Used to SaveNonTicket
        /// </summary>
        /// <param name="CognizantID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="LastDateOfWeek"></param>
        /// <param name="TicketID"></param>
        /// <param name="Remarks"></param>
        /// <param name="NonTicketActivity"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public TimeSheetModel SaveNonTicket(BaseInformationModel objNonTicketModel)
        {
            try
            {
                return new TicketingModule().SaveNonTicket(objNonTicketModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To DeleteTicket
        /// </summary>
        /// <param name="objDeleteTicket"></param>
        /// <returns></returns>
        //-----------------Delete Ticket-------------------------
        public bool DeleteTicket(DeleteTicket objDeleteTicket)
        {
            try
            {
                return new TicketingModule().DeleteTicket(objDeleteTicket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetEffortDetailsDataByMonth
        /// </summary>
        /// <param name="objNonTicketModel"></param>
        /// <returns></returns>
        public EffortDetailsData GetEffortDetailsDataByMonth(string EmployeeID, string CustomerID, string FromDate,
            string ToDate)
        {
            try
            {
                return new TicketingModule().GetEffortDetailsDataByMonth(EmployeeID, CustomerID, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This Method Is Used To GetDetailsAddTicket
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public AddTicketNewDetails GetDetailsAddTicket(int ProjectID, string UserID, int supportTypeID)
        {
            try
            {
                return new TicketingModule().GetDetailsAddTicket(ProjectID, UserID, supportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetStatusPriorityTicketTypeDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ProjectNewDetails GetStatusPriorityTicketTypeDetails(int ProjectID)
        {
            try
            {
                return new TicketingModule().GetStatusPriorityTicketTypeDetails(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To CauseCodeResolutionCode
        /// </summary>
        /// <param name="objCauseCodeResolutionCode"></param>
        /// <param name="isAutoClassified"></param>
        /// <param name="isDDAutoClassified"></param>
        /// <returns></returns>
        public GetDebtAvoidResidual CauseCodeResolutionCode(CauseCodeResolutionCode objCauseCodeResolutionCode,
            string isAutoClassified, string isDDAutoClassified)

        {
            try
            {
                return new TicketingModule().CauseCodeResolutionCode(objCauseCodeResolutionCode, isAutoClassified, isDDAutoClassified);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetAutoClassifiedDetailsForDebt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetAutoClassifiedDetailsForDebt(string ProjectID)
        {
            try
            {
                return new TicketingModule().GetAutoClassifiedDetailsForDebt(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTranslatedfieldsforMLAPI
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="add_text"></param>
        /// <param name="TimeTickerID"></param>
        /// <returns></returns>
        public DataTable GetTranslatedfieldsforMLAPI(int ProjectID, string add_text, string TimeTickerID)
        {
            try
            {
                return new TicketingModule().GetTranslatedfieldsforMLAPI(ProjectID, add_text, TimeTickerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get addtext for Debt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetaddtextforDebt(string ProjectID, int SupportTypeID)
        {
            try
            {
                return new TicketingModule().GetaddtextforDebt(ProjectID, SupportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetApplicationDetailForEmployeeid
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<ApplicationProjectModel> GetProjectDetailForEmployeeid(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetProjectDetailForEmployeeid(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetApplicationDetailForEmployeeid
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<ApplicationProjectModel> GetApplicationDetailForEmployeeid(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetApplicationDetailForEmployeeid(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetInvalidSuggestedActivities
        /// </summary>
        /// <returns></returns>
        public List<NameIDModel> GetInvalidSuggestedActivities()
        {
            try
            {
                return new TicketingModule().GetInvalidSuggestedActivities();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To AddTicketDetails
        /// </summary>
        /// <param name="objAddTicketDetails"></param>
        /// <returns></returns>
        public TimeSheetModel AddTicketDetails(AddTicketDetails objAddTicketDetails)
        {
            try
            {
                return new TicketingModule().AddTicketDetails(objAddTicketDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetNonTicketDetailsToPopup
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public NonTicketPopupDetails GetNonTicketDetailsToPopup(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetNonTicketDetailsToPopup(EmployeeID, CustomerID)
;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetHiddenFieldsForTM
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFieldsForTM(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetHiddenFieldsForTM(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method is used to SaveLanguageForUserID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="Language"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SaveLanguageForUserID(BaseInformationModel objBasicDetails)
        //string EmployeeID, string Language, string UserID)
        {
            try
            {
                return new TicketingModule().SaveLanguageForUserID(objBasicDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method is used to GetLanguageForUserlD
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public LanguageModel GetLanguageForUserlD(string EmployeeID)
        {
            try
            {
                return new TicketingModule().GetLanguageForUserlD(EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to save data
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Flag"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public bool SaveData(List<SaveTicketingModuleTicketDetails> Data, int Flag, string EmployeeID, string IsDaily, string access)
        {
            try
            {
                return new TicketingModule().SaveData(Data, Flag, EmployeeID, IsDaily, access);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetCountErroredTickets
        /// </summary>
        /// <param name="Employeeid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public bool GetCountErroredTickets(string Employeeid, int customerid)
        {
            try
            {
                return new TicketingModule().GetCountErroredTickets(Employeeid, customerid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To SaveErrorLogTicketData
        /// </summary>
        /// <param name="errorLogTicketData"></param>
        /// <param name="projectId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int SaveErrorLogTicketData(List<ErrorLogCorrection> errorLogTicketData, int projectId,
            string employeeId, string customerId, int SupportTypeID)
        {
            try
            {
                return new TicketingModule().SaveErrorLogTicketData(errorLogTicketData, projectId, employeeId, customerId, SupportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetDebtEnabledFields
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DebtEnabledFields GetDebtEnabledFields(int ProjectID)
        {
            try
            {
                return new TicketingModule().GetDebtEnabledFields(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetErrorLogTicketData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>S
        public List<ErrorLogCorrection> GetErrorLogTicketData(int ProjectID, string EmployeeID, int SupportTypeID)
        {
            try
            {
                return new TicketingModule().GetErrorLogTicketData(ProjectID, EmployeeID, SupportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To UpdateIsAttributeByTicketID
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="TicketID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="StatusID"></param>
        /// <param name="IsCognizant"></param>
        /// <param name="TicketTypeID"></param>
        /// <returns></returns>
        public string UpdateIsAttributeByTicketID(OverallTicketDetails updateIsAttribute)
        {
            try
            {
                return new TicketingModule().UpdateIsAttributeByTicketID(updateIsAttribute);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketAttributeDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="DARTStatusID"></param>
        /// <param name="TicketStatusID"></param>
        /// <param name="IsAttributeUpdated"></param>
        /// <param name="IsCognizant"></param>
        /// <param name="TicketTypeID"></param>
        /// <param name="SupportTypeId"></param>
        /// <returns></returns>
        public List<TicketAttributesModel> GetTicketAttributeDetails(TicketAttributesModel ticketAttributesModel)
        {
            try
            {
                return new TicketingModule().GetTicketAttributeDetails(ticketAttributesModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetApplicationsByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ApplicationsModel> GetApplicationsByProject(Int64 ProjectID)
        {
            try
            {
                return new TicketingModule().GetApplicationsByProject(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To get Assignment Group by project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>


        public List<AssignmentGroupModel> GetAssignmentGroupByProjectID(Int64 ProjectID, string UserID)
        {
            try
            {
                return new TicketingModule().GetAssignmentGroupByProjectID(ProjectID, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Tower details list based on project ID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<TowerDetailsModel> GetTowerDetailsByProjectID(Int64 ProjectID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetTowerDetailsByProjectID(ProjectID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetProjectsByCustomer
        /// </summary>
        /// <param name="CustomerID"></parama
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<ProjectsModel> GetProjectsByCustomer(string CustomerID, string EmployeeID)
        {
            try
            {
                return new TicketingModule().GetProjectsByCustomer(CustomerID, EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// This Method Is Used To GetTicketStatusByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<StatusDetails> GetTicketStatusByProject(Int64 ProjectID)
        {
            try
            {
                return new TicketingModule().GetTicketStatusByProject(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTimeZoneInformationByCustomer
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public TimeZoneInfoByCustomer GetTimeZoneInformationByCustomer(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new TicketingModule().GetTimeZoneInformationByCustomer(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetProjectName
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetProjectName(string ProjectID)
        {
            try
            {
                return new TicketingModule().GetProjectName(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetProjectNameESAID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<GetProjectNameEsaID> GetProjectNameESAID(string ProjectID)
        {
            try
            {
                return new TicketingModule().GetProjectNameESAID(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetRoles
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="userId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<Roles> GetRoles(string mode, string userId, int customerId)
        {
            try
            {
                return new TicketingModule().GetRoles(mode, userId, customerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To MandatoryHours
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public decimal MandatoryHours(int CustomerId, string EmployeeID)
        {
            try
            {
                return new TicketingModule().MandatoryHours(CustomerId, EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To InsertRuleIDForTickerDetails
        /// </summary>
        /// <param name="TimeTickerID"></param>
        /// <param name="Ruleid"></param>
        /// <param name="UserID"></param>
        public void InsertRuleIDForTickerDetails(string TimeTickerID, string Ruleid, string UserID, string projectId, int SupportTypeID, string clusterDesc, string clusterReso)
        {
            try
            {
                new TicketingModule().InsertRuleIDForTickerDetails(TimeTickerID, Ruleid, UserID, projectId, SupportTypeID, clusterDesc, clusterReso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To APIForAutoClassification
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TowerID"></param>
        /// <param name="SupportTypeID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="CauseCode"></param>
        /// <param name="ResolutionCode"></param>
        /// <param name="TicketDescription"></param>
        /// <param name="add_text"></param>
        /// <param name="TimeTickerID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public Responseclass APIForAutoClassification(string ProjectID, int TowerID, int SupportTypeID,
            int ApplicationID, int CauseCode, int ResolutionCode, string TicketDescription,
            string add_text, string TimeTickerID, string UserID, int AutoClassificationtype)
        {
            try
            {
                return new TicketingModule().APIForAutoClassification(ProjectID, TowerID, SupportTypeID, ApplicationID, CauseCode, ResolutionCode, 
                    TicketDescription, add_text, TimeTickerID, UserID, AutoClassificationtype);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetAssigneNameByProjectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="assigneName"></param>
        /// <returns></returns>
        public List<AssigneModel> GetAssigneNameByProjectID(int projectID, string assigneName)
        {
            try
            {
                return new TicketingModule().GetAssigneNameByProjectID(projectID, assigneName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get Language for drop down.
        /// <param name="ModuleName"></param>
        /// </summary>
        /// <returns>Language list</returns>
        public List<LanguageModel> GetLanguageForDropdown(string ModuleName)
        {
            try
            {
                return new TicketingModule().GetLanguageForDropdown(ModuleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method is used to GetUserApplicaitionDetails
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public UserMasterDetails GetUserApplicaitionDetails(string EmployeeID, int CustomerID)
        {
            try
            {
                return new TicketingModule().GetUserApplicaitionDetails(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetServiceDetails
        /// </summary>
        /// <param name="objTicketDetails"></param>
        /// <returns></returns>
        public int GetServiceDetails(AddTicketDetails objTicketDetails)
        {
            try
            {
                return new TicketingModule().GetServiceDetails(objTicketDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// Method to Get Default Landing Page Details.
        /// </summary>
        /// <returns>Default Page list</returns>
        public List<DefaultLandingPage> GetDefaultLandingPageDetails(string EmployeeID, string CustomerID)
        {
            try
            {
                return new TicketingModule().GetDefaultLandingPageDetails(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public List<ProjectDetails> GetProjectDetailsforDefaultLanding(string CustomerID, string EmployeeID)
        {
            try
            {
                return new TicketingModule().GetProjectDetailsforDefaultLanding(CustomerID, EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public List<LeadDetails> GetProjectLeadDetails(string ProjectID, string EmployeeID)
        {
            try
            {
                return new TicketingModule().GetProjectLeadDetails(ProjectID, EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public bool SaveDefaultLandingPageDetails(string EmployeeID, string AccountID, string PrivilegeID)
        {
            try
            {
                return new TicketingModule().SaveDefaultLandingPageDetails(EmployeeID, AccountID, PrivilegeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public Int64 GetCustomerwiseDefaultPage(string EmployeeID, string CustomerID)
        {
            try
            {
                return new TicketingModule().GetCustomerwiseDefaultPage(EmployeeID, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Nullable<bool> GetAppEnableFlag(Int64 customerID)
        {
            try
            {
                return new TicketingModule().GetAppEnableFlag(customerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method updates status and service for Work items
        /// </summary>
        /// <param name="TimeTickerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="TicketID"></param>
        /// <param name="StatusID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="ServiceID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>result</returns>
        public bool UpdateWorkItemServiceandStatus(OverallTicketDetails overallTicketDetails)
        {
            try
            {
                return new TicketingModule().UpdateWorkItemServiceandStatus(overallTicketDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks whether WorkItem already exists or not
        /// </summary>
        /// <param name="workItemDetails"></param>
        /// <returns>lstValidationmsg</returns>
        public List<ValidationMessages> CheckWorkItemrepo(List<CheckDuplicate> workItemDetails)
        {
            try
            {
                return new TicketingModule().CheckWorkItemrepo(workItemDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks if sprint name already exists
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns>lstValidationmsg</returns>
        public List<ValidationMessages> CheckSprintName(List<CheckDuplicate> sprintDetails)
        {
            try
            {
                return new TicketingModule().CheckSprintName(sprintDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Addworkitem(List<AddWorkItemSave> workitem)
        {
            try
            {
                return new TicketingModule().Addworkitem(workitem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves Sprint Details
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns>result</returns>
        public bool SaveSprintDetails(List<SavesprintDetails> sprintDetails)
        {
            try
            {
                return new TicketingModule().SaveSprintDetails(sprintDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets ADM Project Details
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>lstProject</returns>
        public List<ADMProjectsModel> GetADMProjectDetails(List<ADMProjectInput> aDMProjectInputs)
        {
            try
            {
                return new TicketingModule().GetADMProjectDetails(aDMProjectInputs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets DropDown Values For WorkItem
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns>MasterDataModel</returns>
        public MasterDataModel GetDropDownValuesForWorkItem(Int64 ProjectId, DateTime StartDate, DateTime EndDate,string access)
        {
            try
            {
                return new TicketingModule().GetDropDownValuesForWorkItem(ProjectId, StartDate, EndDate, access);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets DropDown Values For Sprint
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns>MasterDataModel</returns>
        public MasterDataModel GetDropDownValuesSprint(Int64 ProjectId, DateTime StartDate, DateTime EndDate,string access)
        {
            try
            {
                return new TicketingModule().GetDropDownValuesSprint(ProjectId, StartDate, EndDate, access);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NewAlgoColumnList GetAlgoKeyAndColumn(int projectId, int supportTypeId)
        {
            try
            {
                return new TicketingModule().GetAlgoKeyAndColumn(projectId, supportTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ResponseClassNewAlgo NewAlgoClassification(string jsonParams, string userId, int projectId, int supportTypeId, string timeTickerId)
        {
            try
            {
                var response = new TicketingModule().NewAlgoClassification(jsonParams, userId, projectId, supportTypeId, timeTickerId);
                return response;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
