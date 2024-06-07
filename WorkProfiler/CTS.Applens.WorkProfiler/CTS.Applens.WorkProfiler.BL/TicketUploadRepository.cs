using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using TicketUpload = CTS.Applens.WorkProfiler.DAL.TicketUploadRepository;
namespace CTS.Applens.WorkProfiler.Repository
{

    /// <summary>
    /// TicketUploadRepository
    /// </summary>
    public class TicketUploadRepository
    {

        /// <summary>
        /// This Method Is Used To DownloadTicketDumpTemplate
        /// </summary>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <returns>Method returns Column mapping</returns>
        public string DownloadTicketDumpTemplate(string EmployeeID, string ProjectID)
        {
            try
            {
                return new TicketUpload().DownloadTicketDumpTemplate(EmployeeID, ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            try
            {
                new TicketUpload().EnableTrustedHosts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To ProcessFileforTicketUpload
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ExcelExportPath"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="EmployeeName"></param>
        /// <param name="CustomerId"></param>
        /// <param name="ProjectID"></param>
        /// <param name="flgUpload"></param>
        /// <returns></returns>
        public string ProcessFileforTicketUpload(string filename, string ExcelExportPath, string EmployeeID,
            string EmployeeName, string CustomerId, string ProjectID, string flgUpload, string IsCognizant,
            string accountname, List<HcmSupervisorList> supervisorLists,string esaprojectid,string access, bool allowEncrypt)
        {
            try
            {
                return new TicketUpload().ProcessFileforTicketUpload(filename, ExcelExportPath, EmployeeID, EmployeeName,
                                                                                CustomerId, ProjectID, flgUpload, IsCognizant,accountname,supervisorLists,esaprojectid,access, allowEncrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="emailDetail"></param>
        /// <returns></returns>
        public void SendEmail(EmailDetail emailDetail)
        {
            try
            {
                 new TicketUpload().SendEmail(emailDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddTask
        /// </summary>
        /// <param name="projectID"></param>
        /// /// <param name="employeeID"></param>
        /// /// <param name="Option"></param>
        /// <returns></returns>
        public void AddTask(string projectID, string CogID, string CustomerID, int Option)
        {
            try
            {
                new TicketUpload().AddTask(projectID, CogID, CustomerID, Option);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To IsFieldExists
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public bool IsFieldExists(DataTable dt, string FieldName)
        {
            bool isFieldExists = false;
            for (int I = 0; I < dt.Columns.Count; I++)
            {
                if (dt.Columns[I].ColumnName == FieldName)
                {
                    isFieldExists = true;
                }
            }
            return isFieldExists;
        }

        /// <summary>
        /// This Method Is Used To ExcelToDataSet
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="EmployeeName"></param>
        /// <returns></returns>
        public DataTable ExcelToDataSet(string filename, string EmployeeName)
        {
            try
            {
                return new TicketUpload().ExcelToDataSet(filename, EmployeeName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To DataTableConvertion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable DataTableConvertion<TSource>(IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// This Method Is Used To InsertTicketDumpUpload
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="FileName"></param>
        /// <param name="Ticket"></param>
        /// <param name="TicketUploadTrackID"></param>
        /// <returns></returns>
        public string InsertTicketDumpUpload(string EmployeeID, string ProjectID, string FileName,
            List<TicketDetail> Ticket, Int64 TicketUploadTrackID, string accountname, List<HcmSupervisorList> supervisorLists, string esaprojectid, string access, bool allowEncrypt)
        {
            try
            {
                return new TicketUpload().InsertTicketDumpUpload(EmployeeID, ProjectID, FileName, Ticket, TicketUploadTrackID, accountname,supervisorLists,esaprojectid,access, allowEncrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks If Multilingual is enabled
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <returns>bool</returns>
        public bool CheckIfMultilingualEnabled(string projectID, string employeeID)
        {
            try
            {
                return new TicketUpload().CheckIfMultilingualEnabled(projectID, employeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets Ticket Values
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <returns>List<TicketDescriptionSummary></returns>
        public List<TicketDescriptionSummary> GetTicketValues(List<TicketSupportTypeMapping> lstColumnMapping,
            string projectID, string employeeID)
        {
            try
            {
                return new TicketUpload().GetTicketValues(lstColumnMapping, projectID, employeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetUserProjectDetail
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<UserDetails> GetUserProjectDetail(string EmployeeID, int CustomerID,string MenuRole)
        {
            try
            {
                return new TicketUpload().GetUserProjectDetail(EmployeeID, CustomerID, MenuRole);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetUserProjectDetail
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public int GetOnboardingPercentageDetails(Int64 ProjectID,string EmployeeID)
        {
            try
            {
                return new TicketUpload().GetOnboardingPercentageDetails(ProjectID, EmployeeID);
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
                return new TicketUpload().GetAutoClassifiedDetailsForDebt(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To get EsaProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public Int32 EffortUploadEsaProjectID(int ProjectID)
        {
            try
            {
                return new TicketUpload().EffortUploadEsaProjectID(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To ChekcITSM
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public Int32 ChekcITSM(string CustomerID, string ProjectID)
        {
            try
            {
                return new TicketUpload().ChekcITSM(CustomerID, ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To CheckIsManualOrAuto
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public string CheckIsManualOrAuto(string projectID)
        {
            try
            {
                return new TicketUpload().CheckIsManualOrAuto(projectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To CheckMandatecolumns
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public string CheckMandatecolumns(string projectID)
        {
            try
            {
                return new TicketUpload().CheckMandatecolumns(projectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used to InsertTicketUploadTrackDetails
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="LineNumber"></param>
        /// <param name="position"></param>
        /// <param name="Message"></param>
        /// <param name="TicketUploadTrackID"></param>
        public void InsertTicketUploadTrackDetails(string MethodName, string LineNumber,
            string position, string Message, Int64 TicketUploadTrackID)
        {
            try
            {
                new TicketUpload().InsertTicketUploadTrackDetails(MethodName, LineNumber, position, Message, TicketUploadTrackID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckIsTicketDescriptionOpted(string projectID)
        {
            try
            {
                return new TicketUpload().CheckIsTicketDescriptionOpted(projectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TicketDetail> UpdateWorkPatternFields(string projectID, DataTable dtBulkUpload, List<TicketDetail> ticket)
        {
            try
            {
                return new TicketUpload().UpdateWorkPatternFields(projectID, dtBulkUpload, ticket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public WorkPatternColumns GetWorkPatternColumns(string projectID)
        {
            try
            {
                return new TicketUpload().GetWorkPatternColumns(projectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetUserProjectDetail
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ConfigList GetUploadConfigDetails(long ProjectId, string EmployeeId)
        {
            try
            {
                return new TicketUpload().GetUploadConfigDetails(ProjectId, EmployeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ProcessFileforEffortUpload(string FileName, string FilePath, string IsCognizant, 
            Int32 ProjectID, string IsEffortTrackActivityWise, string IsDaily, string IsApp,string EmployeeID)
        {
            try
            {
                return new TicketUpload().ProcessFileforEffortUpload(FileName, FilePath, IsCognizant, ProjectID, IsEffortTrackActivityWise, IsDaily, IsApp, EmployeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region MyRegion
        public List<AssociateLensModel> GetHealticketdetails(string fromDate,
       string toDate)
        {
            return new TicketUpload().GetHealticketdetails(fromDate, toDate);
        }
        public List<AssociateLensModel> GetAutomationticketdetails(string fromDate,
        string toDate)
        {
            return new TicketUpload().GetAutomationticketdetails(fromDate, toDate);
        }
        #endregion
        /// <summary>
        /// Method to download the debt unclassified
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string DownloadDebtTemplate(string EsaProjectID, string ProjectID, string ProjectName, string ClosedDateFrom, string ClosedDateTo,
            string AppTowerId, string ispureApp, string userID)
        {
            string resolutionModel = string.Empty;
            var DAL = new TicketUpload();
            resolutionModel = DAL.DownloadDebtTemplate(EsaProjectID,
                ProjectID, ProjectName, ClosedDateFrom, ClosedDateTo, AppTowerId, ispureApp, userID);
            return resolutionModel;
        }

        /// <summary>
        /// File upload for the debt uncalssified ticket
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string ProcessFileUploadforDebt(string fileName, string filePath, string projectId, string ispureApp,string UserId)
        {
            string resolutionModel = string.Empty;
            try
            {
                var DAL = new TicketUpload();
                resolutionModel = DAL.ProcessFileUploadforDebt
                    (fileName, filePath, projectId, ispureApp, UserId);
                return resolutionModel;
            }
            catch
            {
                throw;
            }
        }
        public List<HcmSupervisorList> GetSupervisorAndEmployeeList(string projectId)
        {
            try
            {
                var DAL = new TicketUpload();
                var supervisorList = DAL.GetSupervisorAndEmployeeList(projectId);
                return supervisorList;
            }
            catch
            {
                throw;
            }
        }

    }
}

