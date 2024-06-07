using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DebtDataDictionary = CTS.Applens.WorkProfiler.DAL.DebtDataDictionaryRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// This class holds DebtDataDictionaryRepository informations
    /// </summary>
    public class DebtDataDictionaryRepository
    {
        private string strresult;
        public string StrResult
        {
            get
            {
                return strresult;
            }

            set
            {
                strresult = value;
            }
        }

        /// <summary>
        /// This Method Is Used To GetDebtOverrideReview
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ReviewStatus"></param>
        /// <returns></returns>
        public List<DebtOverrideReview> GetDebtOverrideReview(DateTime StartDate, DateTime EndDate, Int64 CustomerID,
            string EmployeeID, Int64 ProjectID, int ReviewStatus, string access)
        {
            try
            {
                return new DebtDataDictionary().GetDebtOverrideReview(StartDate, EndDate, CustomerID, EmployeeID, ProjectID, ReviewStatus, access);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetDebtClassificationmodel
        /// </summary>
        /// <returns></returns>
        public List<DebtClassificationModelDebt> GetDebtClassificationmodel()
        {
            try
            {
                return new DebtDataDictionary().GetDebtClassificationmodel();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketRoles
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<TicketRole> GetTicketRoles(string EmployeeID, Int64 CustomerID, Int64 ProjectID)
        {
            try
            {
                return new DebtDataDictionary().GetTicketRoles(EmployeeID, CustomerID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
                     
        /// <summary>
        /// This Method Is Used To GetCauseCode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<CauseModelDebt> GetCauseCode(int ProjectID)
        {
            try
            {
                return new DebtDataDictionary().GetCauseCode(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetReasonForResidualByprojectid
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ReasonForResidual> GetReasonForResidualByprojectid(int ProjectID)
        {
            try
            {
                return new DebtDataDictionary().GetReasonForResidualByprojectid(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetResolutionCode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ResolutionModelDebt> GetResolutionCode(int ProjectID)
        {
            try
            {
                return new DebtDataDictionary().GetResolutionCode(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetApplicationDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ApplicationModel> GetApplicationDetails(int ProjectID, string access)
        {
            try
            {
                return new DebtDataDictionary().GetApplicationDetails(ProjectID, access);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetResidualDetail
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="RowID"></param>
        /// <returns></returns>
        public List<ResidualDetail> GetResidualDetail(int ProjectID, int ApplicationID, int RowID)
        {
            try
            {
                return new DebtDataDictionary().GetResidualDetail(ProjectID, ApplicationID, RowID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To ProjectDebtDetailsdate
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string ProjectDebtDetailsdate(int ProjectID)
        {
            try
            {
                return new DebtDataDictionary().ProjectDebtDetailsdate(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetGridData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationIds"></param>
        /// <returns></returns>
        public List<Griddata> GetGridData(int ProjectID, List<ApplicationIDs> ApplicationIds)
        {
            try
            {
                return new DebtDataDictionary().GetGridData(ProjectID, ApplicationIds);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetResidualDebt
        /// </summary>
        /// <returns></returns>
        public List<ResidualModelDebt> GetResidualDebt()
        {
            try
            {
                return new DebtDataDictionary().GetResidualDebt();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetAvoidableFlag
        /// </summary>
        /// <returns></returns>
        public List<AvoidableModelFlag> GetAvoidableFlag()
        {
            try
            {
                return new DebtDataDictionary().GetAvoidableFlag();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To GetNatureOfTicket
        /// </summary>
        /// <returns></returns>
        public List<NatureOfTicket> GetNatureOfTicket()
        {
            try
            {
                return new DebtDataDictionary().GetNatureOfTicket();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To DeleteDataDictionaryByID
        /// </summary>
        /// <param name="dataDictionaryDetails"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public int DeleteDataDictionaryByID(List<ProjectDataDictionaryDelete> dataDictionaryDetails,
            string EmployeeID)
        {
            try
            {
                return new DebtDataDictionary().DeleteDataDictionaryByID(dataDictionaryDetails, EmployeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// AddReasonforResidual
        /// </summary>
        /// <param name="objAddReason"></param>
        /// <returns></returns>
        public int AddReasonforResidual(AddReasonforResidual objAddReason)
        {
            try
            {
                return new DebtDataDictionary().AddReasonforResidual(objAddReason);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the drop down for AppGroup and Application
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="customerID"></param>
        /// <param name="projectID"></param>
        /// <param name="mode"></param>
        /// <param name="portfolioIds"></param>
        /// <returns></returns>
        public DataDictionaryData GetDropDownValuesDataDictionary(string employeeID, Int64 customerID,
            Int64 projectID,
            string mode, List<PortfolioID> portfolioIds)
        {
            try
            {
                return new DebtDataDictionary().GetDropDownValuesDataDictionary(employeeID, customerID, projectID, mode, portfolioIds);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To EditDebtReview
        /// </summary>
        /// <param name="debtClassficationID"></param>
        /// <param name="ticketID"></param>
        /// <param name="debtResolutionID"></param>
        /// <param name="causeCodeID"></param>
        /// <param name="resiDebt"></param>
        /// <param name="avdFlag"></param>
        /// <param name="reasonResiID"></param>
        /// <param name="exComDate"></param>
        /// <returns></returns>
        public string EditDebtReview(long debtClassficationID, string ticketID, long debtResolutionID,
            long causeCodeID, long resiDebt, int avdFlag, long reasonResiID, string exComDate)
        {
            try
            {
                return new DebtDataDictionary().EditDebtReview(debtClassficationID, ticketID, debtResolutionID,
                        causeCodeID, resiDebt, avdFlag, reasonResiID, exComDate);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To AddApplicationDetails
        /// </summary>
        /// <param name="objAddApplicationDetails"></param>
        /// <returns></returns>
        public string AddApplicationDetails(AddApplicationDetails objAddApplicationDetails)
        {
            try
            {
                return new DebtDataDictionary().AddApplicationDetails(objAddApplicationDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To AddReasonResidualAndCompDate
        /// </summary>
        /// <param name="objAddReasonResidualAndCompDate"></param>
        /// <returns></returns>
        public string AddReasonResidualAndCompDate(AddReasonResidualAndCompDate objAddReasonResidualAndCompDate)
        {
            try
            {
                return new DebtDataDictionary().AddReasonResidualAndCompDate(objAddReasonResidualAndCompDate);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To SaveDataDictionarybyProject
        /// </summary>
        /// <param name="DataDetails"></param>
        /// <returns></returns>
        public bool SaveDataDictionarybyProject(List<ProjectDataDictionary> DataDetails)
        {
            try
            {
                return new DebtDataDictionary().SaveDataDictionarybyProject(DataDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method Is Used To SaveDataDictionaryByID
        /// </summary>
        /// <param name="dataDictionaryDetails"></param>
        /// <returns></returns>
        public bool SaveDataDictionaryByID(List<ProjectDataDictionary> dataDictionaryDetails)
        {
            try
            {
                return new DebtDataDictionary().SaveDataDictionaryByID(dataDictionaryDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To ApproveTicketsByTicketId
        /// </summary>
        /// <param name="TicketDetails"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public bool ApproveTicketsByTicketId(List<DebtOverrideReview> TicketDetails, string EmployeeID,
            Int64 ProjectID)
        {
            try
            {
                return new DebtDataDictionary().ApproveTicketsByTicketId(TicketDetails, EmployeeID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// This Method Is Used To ExportToExcelForDebtReview
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsCognizant"></param>
        /// <param name="ReviewStatus"></param>
        /// <returns></returns>
        public string ExportToExcelForDebtReview(DateTime StartDate, DateTime EndDate, string EmployeeID,
            Int64 CustomerID, Int64 ProjectID, int IsCognizant, int ReviewStatus)
        {
            try
            {
                return new DebtDataDictionary().ExportToExcelForDebtReview(StartDate, EndDate, EmployeeID, CustomerID, ProjectID, IsCognizant, ReviewStatus);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Download function for Data Dictionary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>

        public string ExportToExcelForDataDictionary(Int64 ProjectID)
        {
            try
            {
                return new DebtDataDictionary().ExportToExcelForDataDictionary(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get the last uploaded errorred patterns for a project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ErrorLogPopUp> GetDDErrorLogData(Int64 ProjectID)
        {
            try
            {
                return new DebtDataDictionary().GetDDErrorLogData(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To 
        /// </summary>
        /// <param name="orgpath"></param>
        /// <returns></returns>
        public string GetFileName(string orgpath)
        {
            string fileName = string.Empty;
            try
            {
                fileName = Path.GetFileName(orgpath);
            }
            catch (Exception)
            {
                throw;
            }
            return fileName;
        }

        /// <summary>
        /// This Method Is Used To ProcessFileUploadForDebtReview
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filepath"></param>
        /// <param name="flgUpload"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsCognizant"></param>
        /// <param name="ReviewStatus"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string ProcessFileUploadForDebtReview(string filename, string filepath, string flgUpload, DateTime
            StartDate, DateTime EndDate, Int64 ProjectID, int IsCognizant, int ReviewStatus, string EmployeeID)
        {
            try
            {
                return new DebtDataDictionary().ProcessFileUploadForDebtReview(filename, filepath, flgUpload, StartDate, EndDate, ProjectID, IsCognizant, ReviewStatus, EmployeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        /// <summary>
        /// Inserts an entry to excel upload details and retrives the identity records.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public Int64 InsertDataDictionalExcelDetailsByProject(Int64 ProjectID, string EmployeeID, string Filename)
        {
            try
            {
                return new DebtDataDictionary().InsertDataDictionalExcelDetailsByProject(ProjectID, EmployeeID, Filename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function is called during the upload functionality of data dictionary
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filepath"></param>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string ProcessFileUploadDataDictionary(string filename, string filepath, Int64 ProjectID,
            string EmployeeID, Int64 DDUploadID)
        {
            try
            {
                return new DebtDataDictionary().ProcessFileUploadDataDictionary(filename, filepath, ProjectID, EmployeeID, DDUploadID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convert excel data to data set during uplaod
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public DataTable ExcelToDataSetDataDictionary(string filename, Int64 ProjectID, string EmployeeID,
            out string result)
        {
            try
            {
                return new DebtDataDictionary().ExcelToDataSetDataDictionary(filename, ProjectID, EmployeeID, out result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To ExcelToDataSet
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsCognizant"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public DataTable ExcelToDataSet(string filename, Int64 ProjectID, int IsCognizant, out string result)
        {
            try
            {
                return new DebtDataDictionary().ExcelToDataSet(filename, ProjectID, IsCognizant, out result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// This Method Is Used To UpdateSignOffDate
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationID"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string UpdateSignOffDate(int ProjectID, int ApplicationID, DateTime EffectiveDate, string EmployeeID, string access)
        {
            try
            {
                return new DebtDataDictionary().UpdateSignOffDate(ProjectID, ApplicationID, EffectiveDate, EmployeeID, access);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the drop down values for Project and dynamic name for AppGroup
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>

        public DataDictionaryProjects GetDropDownValuesProjectPortfolio(string employeeID, int customerID)
        {
            try
            {
                return new DebtDataDictionary().GetDropDownValuesProjectPortfolio(employeeID, customerID);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This Method Is Used To GetConflictPatterns
        /// </summary>
        /// <param name="ProjectId"></param>

        /// <returns></returns>
        public List<ConflictPatterns> GetConflictPatterns(int ProjectId)
        {
            try
            {
                return new DebtDataDictionary().GetConflictPatterns(ProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Is Used To GetConflictpatternDetailsForDownload
        /// </summary>
        /// <param name="lstconflict"></param>
        /// <param name="DestinationTemplateFileName"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public string GetConflictpatternDetailsForDownload(List<ConflictPatterns> lstconflict, 
            string DestinationTemplateFileName)
        {
            try
            {
                return new DebtDataDictionary().GetConflictpatternDetailsForDownload(lstconflict, DestinationTemplateFileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
