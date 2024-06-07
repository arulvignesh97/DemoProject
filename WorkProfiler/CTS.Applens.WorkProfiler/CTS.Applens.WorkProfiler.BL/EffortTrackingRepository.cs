
using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using EffortTracking = CTS.Applens.WorkProfiler.DAL.EffortTrackingRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// This class holds EffortTrackingRepository details
    /// </summary>
    public class EffortTrackingRepository
    {
        /// <summary>
        /// This Method Is Used To GetPortfolioName
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="customerID"></param>
        /// <returns>string</returns>
        public string GetPortfolioName(string employeeID, long customerID)
        {
            try
            {
                return new EffortTracking().GetPortfolioName(employeeID, customerID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetEffortWeekWiseList
        /// </summary>
        /// <param name="employeeID">employeeID</param>
        /// <param name="customerid">customerid</param>
        /// <param name="MonthStartDate">MonthStartDate</param>
        /// <param name="MonthEndDate">MonthEndDate</param>
        /// <returns></returns>
        public List<EffortDetailsByDate> GetEffortWeekWiseList(string employeeID, string customerid,
            DateTime MonthStartDate, DateTime MonthEndDate)
        {
            try
            {
                return new EffortTracking().GetEffortWeekWiseList(employeeID, customerid, MonthStartDate, MonthEndDate);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This Method Is Used To GetLstProjectPriority
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstPriorityModel> GetLstProjectPriority(int ProjectID)
        {
            try
            {
                return new EffortTracking().GetLstProjectPriority(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstProjectCauseCode
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstCauseCode> GetLstProjectCauseCode(Int32 ProjectID, Int32 ApplicationID)
        {
            try
            {
                return new EffortTracking().GetLstProjectCauseCode(ProjectID, ApplicationID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To CauseCodeResolutionCode
        /// </summary>
        /// <param name="CauseCode">CauseCode</param>
        /// <param name="TicketDescription">TicketDescription</param>
        /// <param name="ResolutionCode">ResolutionCode</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="Application">Application</param>
        /// <param name="IsAutoClassified">IsAutoClassified</param>
        /// <param name="IsDDAutoClassified">IsDDAutoClassified</param>
        /// <returns></returns>
        public GetDebtAvoidResidual CauseCodeResolutionCode(int CauseCode, string TicketDescription,
            int ResolutionCode, string ProjectID, string Application, string IsAutoClassified,
            string IsDDAutoClassified)
        {
            try
            {
                return new EffortTracking().CauseCodeResolutionCode(CauseCode, TicketDescription, ResolutionCode, 
                                                                        ProjectID, Application, IsAutoClassified, IsDDAutoClassified);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetAutoClassifiedDetailsForDebt
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public DataTable GetAutoClassifiedDetailsForDebt(string ProjectID)
        {
            try
            {
                return new EffortTracking().GetAutoClassifiedDetailsForDebt(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetLstProjectResolutionCode
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstResolution> GetLstProjectResolutionCode(string ProjectID, string CauseCode)
        {
            try
            {
                return new EffortTracking().GetLstProjectResolutionCode(ProjectID, CauseCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstDebtClassification
        /// </summary>
        /// <returns></returns>
        public List<LstDebtClassification> GetLstDebtClassification(int SupportTypeID)
        {
            try
            {
                return new EffortTracking().GetLstDebtClassification(SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstSeverity
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstSeverity> GetLstSeverity(int ProjectID)
        {
            try
            {
                return new EffortTracking().GetLstSeverity(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstTicketSource
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstTicketSource> GetLstTicketSource(Int32 ProjectID)
        {
            try
            {
                return new EffortTracking().GetLstTicketSource(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstReleaseType
        /// </summary>
        /// <returns></returns>
        public List<LstReleaseType> GetLstReleaseType()
        {
            try
            {
                return new EffortTracking().GetLstReleaseType();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstBusinessImpact
        /// </summary>
        /// <returns></returns>
        public List<GetBusinessImpact> GetBusinessImpact()
        {
            try
            {
                return new EffortTracking().GetBusinessImpact();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This Method Is Used To GetLstKEDBUpdated
        /// </summary>
        /// <returns></returns>
        public List<LstKEDBUpdated> GetLstKEDBUpdated()
        {
            try
            {
                return new EffortTracking().GetLstKEDBUpdated();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetLstTicketType
        /// </summary>
        /// <returns></returns>
        public List<LstTicketType> GetLstTicketType()
        {
            try
            {
                return new EffortTracking().GetLstTicketType();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetDartStatus
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="StatusID">StatusID</param>
        /// <returns></returns>
        public List<PopupAttributeModel> GetDartStatus(int ProjectID, int StatusID)
        {
            try
            {
                return new EffortTracking().GetDartStatus(ProjectID, StatusID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetkedbAvailable
        /// </summary>
        /// <returns>returns List<LstkedbAvailable></returns>
        public List<LstkedbAvailable> GetkedbAvailable()
        {
            try
            {
                return new EffortTracking().GetkedbAvailable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetPopupAttributeData
        /// </summary>
        /// <param name="objPopupattributeget">objPopupattributeget</param>
        /// <returns></returns>
        public List<PopupAttributeModel> GetPopupAttributeData(Popupattributeget objPopupattributeget)
        {
            try
            {
                return new EffortTracking().GetPopupAttributeData(objPopupattributeget);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To InsertAttributeDetails
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="UserID">UserID</param>
        /// <param name="ResidualDebtId">ResidualDebtId</param>
        /// <param name="AvoidalFlagId">AvoidalFlagId</param>
        /// <param name="IsAttributeUpdated">IsAttributeUpdated</param>
        /// <param name="TicketStatusID">TicketStatusID</param>
        /// <returns></returns>
        public string InsertAttributeDetails(InsertAttributeSaveModel insertAttributeSave)
        {
            try
            {
                return new EffortTracking().InsertAttributeDetails(insertAttributeSave);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// This Method Is Used To GetAddtionalAttributeDetails
        /// </summary>
        /// <returns>returns PopupAttributeModel</returns>
        public PopupAttributeModel GetAddtionalAttributeDetails()
        {
            try
            {
                return new EffortTracking().GetAddtionalAttributeDetails();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetTicketInfoDetails
        /// </summary>
        /// <param name="ProjectId">ProjectId</param>
        /// <param name="TicketID">TicketID</param>
        /// <returns></returns>
        public string GetTicketInfoDetails(Int64 ProjectId, string TicketID, int supportType)
        {
            try
            {
                return new EffortTracking().GetTicketInfoDetails(ProjectId, TicketID, supportType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetRolePrivilageMenusForAppLens
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <param name="CustomerID">CustomerID</param>
        /// <returns></returns>
        public List<RolePrivilegeModel> GetRolePrivilageMenusForAppLens(string EmployeeID, Int64 CustomerID)
        {
            try
            {
                return new EffortTracking().GetRolePrivilageMenusForAppLens(EmployeeID, CustomerID);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Getting values for copy
        /// </summary>
        /// <param name="CustomerID">CustomerID</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<CopyFields> GetDropDownValuesForCopy(long CustomerID, long ProjectID)
        {
            try
            {
                return new EffortTracking().GetDropDownValuesForCopy(CustomerID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method is used to GetHiddenFields
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFields(string EmployeeID)
        {
            try
            {
                return new EffortTracking().GetHiddenFields(EmployeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetHiddenFieldsForTM
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <param name="CustomerId">CustomerId</param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFieldsForTM(string EmployeeID, long CustomerId)
        {
            try
            {
                return new EffortTracking().GetHiddenFieldsForTM(EmployeeID, CustomerId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetCustomer
        /// </summary>
        /// <param name="CogID">CogID</param>
        /// <returns></returns>
        public List<CustomerModel> GetCustomer(string CogID)
        {
            try
            {
                return new EffortTracking().GetCustomer(CogID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<long> GetProjectCustomer(string CogID, Int16 Mode)
        {
            try
            {
                return new EffortTracking().GetProjectCustomer(CogID, Mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetSearchTicketsForET
        /// </summary>
        /// <param name="objChooseTicket">objChooseTicket</param>
        /// <returns></returns>
        public List<ChooseSearchTicketDetailsModel> GetSearchTicketsForET(ChooseTicket objChooseTicket)
        {
            try
            {
                return new EffortTracking().GetSearchTicketsForET(objChooseTicket);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets Choosed Work Item Details
        /// </summary>
        /// <param name="objChooseTicket"></param>
        /// <returns>List Work Item Details</returns>
        public ChooseWorkItem ChooseWorkItems(ChooseTicket objChooseTicket)
        {
            try
            {
                return new EffortTracking().ChooseWorkItems(objChooseTicket);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks If Multilingual is enabled
        /// </summary>
        /// <param name="projectID">projectID</param>
        /// <param name="employeeID">employeeID</param>
        /// <returns>bool</returns>
        public bool CheckIfMultilingualEnabled(string projectID, string employeeID)
        {
            try
            {
                return new EffortTracking().CheckIfMultilingualEnabled(projectID, employeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets Ticket Values
        /// </summary>
        /// <param name="ticketID">ticketID</param>
        /// <param name="projectID">projectID</param>
        /// <param name="employeeID">employeeID</param>
        /// <returns>List<TicketDescriptionSummary></returns>
        public List<TicketDescriptionSummary> GetTicketValues(string ticketID, string projectID,
            string employeeID, int SupportTypeID)
        {
            try
            {
                return new EffortTracking().GetTicketValues(ticketID, projectID, employeeID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To Get Ispartialautomated Details
        /// </summary>

        /// <returns></returns>
        public List<LstMetSLA> GetIspartialautomatedDetails()
        {
            try
            {
                return new EffortTracking().GetIspartialautomatedDetails();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsTicketDescriptionOpted(int projectID)
        {
            try
            {
                return new EffortTracking().IsTicketDescriptionOpted(projectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
