using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.SqlServer.Server;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using CTS.Applens.WorkProfiler.Common;
using System.IO;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.Text;
using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using CTS.Applens.Framework;
using System.Configuration;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// ContinuousLearningRepository
    /// </summary>
    public class ContinuousLearningRepository : DBContext, IContinuousLearningRepository
    {
        /// <summary>
        /// This Method is used to LearningEnrichmentDate
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>

        public LearningEnrichmentDate GetLearningEnrichmentDates(int ProjectId)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            LearningEnrichmentDate objLearningEnrichmentDate = new LearningEnrichmentDate();
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@projectID", ProjectId);

                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_GetEnrichmentDate]",
                    prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    objLearningEnrichmentDate.FromDate = dtResult.Tables[0].Rows[0]["FromDate"] != DBNull.Value ?
                        Convert.ToDateTime(dtResult.Tables[0].Rows[0]["FromDate"],
                        CultureInfo.InvariantCulture).ToString("dd-MM-yyyy",CultureInfo.InvariantCulture) :
                        string.Empty;
                    objLearningEnrichmentDate.ToDate = dtResult.Tables[0].Rows[0]["ToDate"] != DBNull.Value ? Convert.
                        ToDateTime(dtResult.Tables[0].Rows[0]["ToDate"],
                        CultureInfo.InvariantCulture).ToString("dd-MM-yyyy",CultureInfo.InvariantCulture) : 
                        string.Empty;

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return objLearningEnrichmentDate;
        }




        /// <summary>
        ///  This Method is used to ToShowCLConfig
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public CLConfig ToShowCLConfig(int projectID, string UserID)
        {
            CLConfig config = new CLConfig();
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@projectID", projectID);
                prms[1] = new SqlParameter("@userID", UserID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_CheckCLJob]", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {

                    config.Month = dtResult.Tables[0].Rows[0]["Month"] != DBNull.Value ?
                        Convert.ToInt32(dtResult.Tables[0].Rows[0]["Month"],CultureInfo.InvariantCulture) : 1;
                    config.Day = dtResult.Tables[0].Rows[0]["Day"] != DBNull.Value ?
                        Convert.ToInt32(dtResult.Tables[0].Rows[0]["Day"],CultureInfo.InvariantCulture) : 1;
                    config.HasValue = true;
                }
                else
                {
                    config.Month = 1;
                    config.Day = 1;
                    config.HasValue = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return config;
        }
        /// <summary>
        ///  This Method is used to SaveConfigValues
        /// </summary>
        /// <param name="clconfig"></param>
        /// <returns></returns>
        public string SaveConfigValues(CLConfig clconfig)
        {
            int result = 0;
            try
            {
                DateTime dtClDate = new DateTime();
                dtClDate = DateTimeOffset.Now.DateTime;
                dtClDate = dtClDate.AddMonths(clconfig.Month);
                string format = "yyyy-MM-dd";
                dtClDate = new DateTime(dtClDate.Year, dtClDate.Month, clconfig.Day);
                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@projectID", clconfig.ProjectId);
                prms[1] = new SqlParameter("@userID", clconfig.UserId);
                prms[2] = new SqlParameter("@month", clconfig.Month);
                prms[3] = new SqlParameter("@day", clconfig.Day);
                prms[4] = new SqlParameter("@date", dtClDate.ToString(format,CultureInfo.InvariantCulture));
                result = (new DBHelper()).ExecuteNonQueryReturn("[AVL].[CL_SaveCLConfig]", prms, ConnectionString);
                if (result > 0)
                {

                    return "Continuous Learning is next configured to run on " + dtClDate.ToShortDateString() + " .";
                }
                else
                {
                    return "Error Occured in saving config values for project !!!Please try again later";
                }
            }
            catch (Exception ex)
            {
                throw ex;

                return string.Empty;
            }
        }
        /// <summary>
        /// This Method is used to CheckIfAllPatternsAreSignedOff
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public bool CheckIfAllPatternsAreSignedOff(long ProjectID)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            bool returnValue = false;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CheckIfAllPatternsAreSignedOff]",
                    prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt64(dtResult.Tables[0].Rows[0]["COUNT"],
                        CultureInfo.InvariantCulture) > 0)
                    {
                        returnValue = false;
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        /// <summary>
        /// This Method is used to GetDropDownValuesBU
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<BusinessUnit> GetDropDownValuesBU(string employeeID)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            List<BusinessUnit> businessUnit = new List<BusinessUnit>();
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_GetDropDownValuesBU]", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    businessUnit = dtResult.Tables[0].AsEnumerable().Select(x => new BusinessUnit
                    {
                        BUID = x["BUID"] != DBNull.Value ? Convert.ToInt32(x["BUID"],
                        CultureInfo.InvariantCulture) : 0,
                        BusinessUnitName = x["BUName"] != DBNull.Value ? Convert.ToString(x["BUName"],
                        CultureInfo.InvariantCulture) : string.Empty,
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return businessUnit;
        }
        /// <summary>
        /// This Method is used to ToDataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() ==
                    typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
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
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcel(List<CLExcel> excel)
        {
            StringBuilder newpth = new StringBuilder();
            StringBuilder orgpath = new StringBuilder();
            string downloadedFile = string.Empty;
            DataSet mLDatatable = new DataSet();
            mLDatatable.Locale = CultureInfo.InvariantCulture;
            try
            {
                string sourcepath = "";
                StringBuilder orginalfile = new StringBuilder();
                sourcepath = new ApplicationConstants().ExcelCLTemplatePath;
                string strExtension = Path.GetExtension(sourcepath);
                string foldername = new ApplicationConstants().DownloadExcelTemp;
                orginalfile.Append(Path.GetDirectoryName(sourcepath)).Append("\\");
                string filename = Path.GetFileName(sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(sourcepath);
                var ext = strExtension;
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss",CultureInfo.InvariantCulture);
                orgpath.Append(foldername).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext));
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile.ToString());
                if (directoryInfo.Exists)
                {
                    newpth.Append(directoryInfo).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp,
                        ext));
                    if (File.Exists(newpth.ToString()))
                    {
                        File.Delete(newpth.ToString());
                        fleInfo.CopyTo(newpth.ToString(), true);
                    }
                    else
                    {
                        fleInfo.CopyTo(newpth.ToString(), true);
                    }
                }
                mLDatatable.Tables.Add(ToDataTable(excel).Copy());

                new OpenXMLOperations().ToExcelSheetByDataTable(mLDatatable.Tables[0], null, newpth.ToString(),
                    "ContinuousLearningReview", null);
                downloadedFile = string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return downloadedFile;

        }
        /// <summary>
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcelTEST(List<TimeSheetDataDaily> excel)
        {
            StringBuilder newpth = new StringBuilder();
            StringBuilder orgpath = new StringBuilder();
            string downloadedFile = string.Empty;
            DataTable mLDatatable = new DataTable();
            mLDatatable.Locale = CultureInfo.InvariantCulture;
            try
            {
                string sourcepath = "";
                StringBuilder orginalfile = new StringBuilder();
                sourcepath = new ApplicationConstants().ExcelAUTemplatePath;
                string strExtension = Path.GetExtension(sourcepath);
                string foldername = new ApplicationConstants().DownloadExcelTemp;
                orginalfile.Append(Path.GetDirectoryName(sourcepath)).Append("\\");
                string filename = Path.GetFileName(sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(sourcepath);
                var ext = strExtension;
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss",CultureInfo.InvariantCulture);
                orgpath.Append(foldername).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext));
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile.ToString());
                if (directoryInfo.Exists)
                {
                    newpth.Append(directoryInfo).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp,
                        ext));
                    if (File.Exists(newpth.ToString()))
                    {
                        File.Delete(newpth.ToString());
                        fleInfo.CopyTo(newpth.ToString(), true);
                    }
                    else
                    {
                        fleInfo.CopyTo(newpth.ToString(), true);
                    }
                }
                mLDatatable = ToDataTable(excel).Copy();
                mLDatatable.Columns.Remove("TimesheetStatusId");
                mLDatatable.Columns.Remove("TimesheetId");
                mLDatatable.Columns.Remove("ProjectId");
                mLDatatable.Columns.Remove("IsFreezed");
                mLDatatable.Columns.Remove("IsApprove");
                mLDatatable.Columns.Remove("IsUnfreeze");

                new OpenXMLOperations().ToExcelSheetByDataTable(mLDatatable, null, newpth.ToString(), "countforexcel",
                       null);
                downloadedFile = string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return downloadedFile;

        }

        /// <summary>
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcelWeekly(List<TimeSheetData> excel)
        {
            string downloadedFile = string.Empty;
            if (excel != null && excel[0] != null && excel[0].DailyTimeSheetData != null)
            {
                var da = excel.SelectMany(b => b.DailyTimeSheetData).ToList();
                List<TimeSheetDataExcel> final = new List<TimeSheetDataExcel>();
                List<DateHead> DateHead1 = new List<DateHead>();
                List<string> DateHead = new List<string>();
                int j = 0;
                int k = 6;
                for (int i = 0; i < excel.Count; i++)
                {
                    TimeSheetDataExcel te = new TimeSheetDataExcel();

                    te.EmployeeId = excel[i].EmployeeId;
                    te.EmployeeName = excel[i].EmployeeName;
                    te.IsFreezed = excel[i].IsFreezed;
                    te.RejectionCommects = excel[i].RejectionCommects;
                    te.TimeSheetStatus = excel[i].TimeSheetStatus;
                    te.TotalHours = excel[i].TotalHours;
                    int p = 0;
                    foreach (var obj in da.Skip(j).Take(7))
                    {
                        if (p == 0)
                        {
                            te.TotalHours1 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 1)
                        {
                            te.TotalHours2 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 2)
                        {
                            te.TotalHours3 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 3)
                        {
                            te.TotalHours4 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 4)
                        {
                            te.TotalHours5 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 5)
                        {
                            te.TotalHours6 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        if (p == 6)
                        {
                            te.TotalHours7 = obj.TotalHours;
                            DateHead.Add(obj.TimeSheetDate.Date.ToString(CultureInfo.CurrentCulture));
                        }
                        p++;

                    }

                    final.Add(te);
                    j = j + 7;
                    k = k + 7;
                }
                StringBuilder newpth = new StringBuilder();

                StringBuilder orgpath = new StringBuilder();

                DataTable mLDatatable = new DataTable();
                mLDatatable.Locale = CultureInfo.InvariantCulture;
                try
                {
                    string sourcepath = "";
                    StringBuilder orginalfile = new StringBuilder();
                    sourcepath = new ApplicationConstants().ExcelAUTemplatePathWeekly;
                    string strExtension = Path.GetExtension(sourcepath);
                    string foldername = new ApplicationConstants().DownloadExcelTemp;
                    orginalfile.Append(Path.GetDirectoryName(sourcepath)).Append("\\");
                    string filename = Path.GetFileName(sourcepath);
                    DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                    FileInfo fleInfo = new FileInfo(sourcepath);
                    var ext = strExtension;
                    string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss",CultureInfo.InvariantCulture);
                    orgpath.Append(foldername).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext));
                    DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile.ToString());
                    if (directoryInfo.Exists)
                    {
                        newpth.Append(directoryInfo).Append(string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp,
                            ext));
                        if (File.Exists(newpth.ToString()))
                        {
                            File.Delete(newpth.ToString());
                            fleInfo.CopyTo(newpth.ToString(), true);
                        }
                        else
                        {
                            fleInfo.CopyTo(newpth.ToString(), true);
                        }
                    }
                    mLDatatable = ToDataTable(final).Copy();
                    mLDatatable.Columns.Remove("RejectionCommects");
                    mLDatatable.Columns.Remove("IsFreezed");
                    new OpenXMLOperations().ToExcelSheetByDataTable(mLDatatable, null, newpth.ToString(), "Weekly",
                           DateHead.ToArray());
                    downloadedFile = string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return downloadedFile;

        }
        /// <summary>
        /// This Method is used to SaveCLPatterns
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public int SaveCLPatterns(CLPatternsSignOff pattern)
        {
            int result = 0;
            bool isCLPatternsPresent = (pattern != null && pattern.Patterns != null && pattern.Patterns.Count > 0);
            try
            {
                SqlParameter[] prms = new SqlParameter
                    [isCLPatternsPresent ? 6 : 5];

                prms[0] = new SqlParameter("@ProjectID", pattern.ProjectId);
                prms[1] = new SqlParameter("@EmployeeID", pattern.EmployeeId);
                prms[2] = new SqlParameter("@IsCLSignOff", pattern.IsCLSignOff);
                prms[3] = new SqlParameter("@EffectiveDate", pattern.EffectiveDate);
                prms[4] = new SqlParameter("@IsSave", pattern.IsSave);

                if (isCLPatternsPresent)
                {
                    var objCollection = from o in pattern.Patterns
                                        select new
                                        {
                                            ID = o.Id,
                                            DebtID = o.DebtId,
                                            AvoidableFlagID = o.AvoidableFlagId,
                                            ResidualID = o.ResidualId,
                                            CauseCodeID = o.CauseCodeId,
                                            ApprovedOrMuted = o.ApprovedOrMuted,
                                            EmployeeID = o.EmployeeId,
                                            IsCLSignOff = o.IsCLSignOff,
                                            OldContID = o.OldContId,
                                            NewContID = o.NewContId,
                                            IsDebtChanged = o.IsDebtChanged
                                        };

                    prms[5] = new SqlParameter("@CLPatterns", objCollection.ToList().ToDT());
                    prms[5].SqlDbType = SqlDbType.Structured;
                }

                (new DBHelper()).ExecuteNonQueryReturn("[AVL].[CL_SaveCLPatterns]", prms, ConnectionString);
                result = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        /// <summary>
        /// This Method is used to CLSavePatternProcessedCollection
        /// </summary>
        public class CLSavePatternProcessedCollection : List<CLPatterns>, IEnumerable<SqlDataRecord>
        {/// <summary>
         /// GetEnumerator
         /// </summary>
         /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("ID", SqlDbType.BigInt),
                      new SqlMetaData("DebtID", SqlDbType.Int),
                      new SqlMetaData("AvoidableFlagID", SqlDbType.Int),
                      new SqlMetaData("ResidualID", SqlDbType.Int),
                      new SqlMetaData("CauseCodeID", SqlDbType.Int),
                      new SqlMetaData("ApprovedOrMuted", SqlDbType.Int),
                      new SqlMetaData("EmployeeID", SqlDbType.NVarChar, 1000),
                      new SqlMetaData("IsCLSignOff", SqlDbType.Bit),
                      new SqlMetaData("OldContID", SqlDbType.Int),
                      new SqlMetaData("NewContID", SqlDbType.Int),
                      new SqlMetaData("IsDebtChanged", SqlDbType.Bit)
             );

                foreach (CLPatterns obj in this)
                {
                    sqlRow.SetInt64(0, obj.Id);
                    sqlRow.SetInt32(1, obj.DebtId);
                    sqlRow.SetInt32(2, obj.AvoidableFlagId);
                    sqlRow.SetInt32(3, obj.ResidualId);
                    sqlRow.SetInt32(4, obj.CauseCodeId);
                    sqlRow.SetInt32(5, obj.ApprovedOrMuted);
                    sqlRow.SetString(6, obj.EmployeeId);
                    sqlRow.SetBoolean(7, obj.IsCLSignOff);
                    sqlRow.SetInt32(8, obj.OldContId);
                    sqlRow.SetInt32(9, obj.NewContId);
                    sqlRow.SetBoolean(10, obj.IsDebtChanged == 1 ? true : false);

                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// This Method is used to GetDropDownValuesAccount
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="BUID"></param>
        /// <returns></returns>
        public List<Account> GetDropDownValuesAccount(string employeeID, int BUID)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            List<Account> account = new List<Account>();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@BUID", BUID);
                prms[1] = new SqlParameter("@EmployeeID", employeeID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_GetDropDownValuesAccount]",
                    prms, ConnectionString).Copy());

                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    account = dtResult.Tables[0].AsEnumerable().Select(x => new Account
                    {
                        AccountId = x["CustomerID"] != DBNull.Value ? Convert.ToInt32(x["CustomerID"],
                        CultureInfo.InvariantCulture) : 0,
                        AccountName = x["CustomerName"] != DBNull.Value ? Convert.ToString(x["CustomerName"], 
                        CultureInfo.InvariantCulture) :
                        string.Empty,
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return account;
        }
        /// <summary>
        /// This method is used to GetDropDownValuesApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="portfolioID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<Application> GetDropDownValuesApplication(long projectID, long portfolioID, long CustomerID)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            List<Application> application = new List<Application>();
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@projectID", projectID);
                prms[1] = new SqlParameter("@portfolioID", portfolioID);
                prms[2] = new SqlParameter("@CustomerID", CustomerID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_GetDropDownValuesApplication]",
                    prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    application = dtResult.Tables[0].AsEnumerable().Select(x => new Application
                    {
                        ApplicationId = x["applicationID"] != DBNull.Value ? Convert.ToInt32(x["applicationID"],
                        CultureInfo.InvariantCulture) : 0,
                        ApplicationName = x["applicationName"] != DBNull.Value ? Convert.
                        ToString(x["applicationName"],CultureInfo.InvariantCulture) : string.Empty,
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return application;
        }
        /// <summary>
        /// This method is used to CheckCognizantCustomer
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public CustomerCognizant CheckCognizantCustomer(string employeeID)
        {
            CustomerCognizant custCog = new CustomerCognizant();
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                DataSet dtResult = new DataSet();
                dtResult.Locale = CultureInfo.InvariantCulture;
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_CheckCustomerCognizant]",
                    prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    custCog.CustomerId = dtResult.Tables[0].Rows[0]["CustomerID"] != DBNull.Value ?
                        Convert.ToInt32(dtResult.Tables[0].Rows[0]["CustomerID"], CultureInfo.InvariantCulture) : 0;
                    custCog.IsCognizant = dtResult.Tables[0].Rows[0]["IsCognizant"] != DBNull.Value ?
                        Convert.ToBoolean(dtResult.Tables[0].Rows[0]["IsCognizant"], CultureInfo.InvariantCulture) : false;
                    custCog.CustomerName = dtResult.Tables[0].Rows[0]["CustomerName"] != DBNull.Value ?
                        Convert.ToString(dtResult.Tables[0].Rows[0]["CustomerName"], CultureInfo.InvariantCulture) : string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return custCog;
        }
        /// <summary>
        /// This method is used to GetDropDownValuesProjectPortfolio
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsPortfolio"></param>
        /// <returns></returns>
        public ContinuousLearningList GetDropDownValuesProjectPortfolio(string employeeID, long CustomerID,
            long ProjectID, int IsPortfolio)
        {
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            ContinuousLearningList cllist = new ContinuousLearningList();
            List<Project> project = new List<Project>();
            List<Portfolio> portfolio = new List<Portfolio>();
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@CustomerID", CustomerID);
                prms[1] = new SqlParameter("@EmployeeID", employeeID);
                prms[2] = new SqlParameter("@ProjectID", ProjectID);
                prms[3] = new SqlParameter("@IsPortfolio", IsPortfolio);
                dsResult = (new DBHelper()).GetDatasetFromSP("[AVL].[CL_GetDropDownValuesProjectPortfolio]", prms, ConnectionString);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (IsPortfolio == 0)
                        {
                            project = dsResult.Tables[0].AsEnumerable().Select(x => new Project
                            {
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt32(x["ProjectID"],
                                CultureInfo.InvariantCulture) : 0,
                                ProjectName = x["ProjectName"] != DBNull.Value ? Convert.ToString(x["ProjectName"],
                                CultureInfo.InvariantCulture)
                                : string.Empty,
                                SupportTypeId = x["SupportTypeId"] != DBNull.Value ?
                                Convert.ToInt32(x["SupportTypeId"],CultureInfo.InvariantCulture) : 0
                            }).ToList();
                        }
                        else
                        {
                            portfolio = dsResult.Tables[0].AsEnumerable().Select(x => new Portfolio
                            {
                                PortfolioId = x["BusinessClusterMapID"] != DBNull.Value ? Convert.
                                ToInt32(x["BusinessClusterMapID"],CultureInfo.InvariantCulture) : 0,
                                PortfolioName = x["BusinessClusterBaseName"] != DBNull.Value ? Convert.
                                ToString(x["BusinessClusterBaseName"],CultureInfo.InvariantCulture) : string.Empty,
                            }).ToList();

                        }
                    }
                }
                cllist.Project = project;
                cllist.Portfolio = portfolio;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cllist;
        }
        /// <summary>
        /// This method is used to GetDebtMLPatternValidationReportContinuous
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <returns></returns>
        public GetDebtPatternValidation GetDebtMLPatternValidationReportContinuous(int ProjectId)
        {

            List<SpDebtMLPatternValidationModel> debtMLPatternValidationList =
                new List<SpDebtMLPatternValidationModel>();
            GetDebtPatternValidation DebtPatternValidation = new GetDebtPatternValidation();
            var JobMessage = "";
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);

                DataSet dtMLPatternValModel = new DataSet();
                dtMLPatternValModel.Locale = CultureInfo.InvariantCulture;
                dtMLPatternValModel = (new DBHelper()).GetDatasetFromSP("[AVL].[CL_GetPatternValidation]", prms, ConnectionString);

                if (dtMLPatternValModel != null && dtMLPatternValModel.Tables[0].Rows.Count > 0)
                {
                    debtMLPatternValidationList = dtMLPatternValModel.Tables[0].AsEnumerable().Select(row =>
                    new SpDebtMLPatternValidationModel
                    {
                        Id = row["ID"] != null ? Convert.ToInt32(row["ID"],CultureInfo.InvariantCulture) : 0,
                        InitialLearningId = row["ContLearningID"] != DBNull.Value ? Convert.
                                ToInt32(row["ContLearningID"],CultureInfo.InvariantCulture) : 0,
                        ApplicationId = row["ApplicationID"] != DBNull.Value ?
                        Convert.ToInt32(row["ApplicationID"],CultureInfo.InvariantCulture) : 0,
                        ApplicationName = (Convert.ToString(row["ApplicationName"] != DBNull.Value ? Convert.
                        ToString(row["ApplicationName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        ApplicationTypeId = row["ApplicationTypeID"] != DBNull.Value ? Convert.
                        ToInt32(row["ApplicationTypeID"],CultureInfo.InvariantCulture) : 0,
                        ApplicationTypeName = (Convert.ToString(row["ApplicationTypeName"] != DBNull.Value ?
                        Convert.ToString(row["ApplicationTypeName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        TechnologyId = row["TechnologyID"] != DBNull.Value ?
                        Convert.ToInt32(row["TechnologyID"],CultureInfo.InvariantCulture) : 0,
                        TechnologyName = (Convert.ToString(row["TechnologyName"] != DBNull.Value ? Convert.
                        ToString(row["TechnologyName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                        ToString(row["TicketPattern"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLDebtClassificationId = row["MLDebtClassificationID"] != DBNull.Value ? Convert.
                        ToInt32(row["MLDebtClassificationID"],CultureInfo.InvariantCulture) : 0,
                        MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] != DBNull.
                                Value ? Convert.ToString(row["MLDebtClassificationName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLResidualFlagId = row["MLResidualFlagID"] != DBNull.Value ? Convert.
                        ToInt32(row["MLResidualFlagID"],CultureInfo.InvariantCulture) : 0,
                        MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] != DBNull.Value ?
                                Convert.ToString(row["MLResidualFlagName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLAvoidableFlagId = row["MLAvoidableFlagID"] != DBNull.Value ? Convert.
                        ToInt32(row["MLAvoidableFlagID"],CultureInfo.InvariantCulture) : 0,
                        MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] != DBNull.Value ?
                                Convert.ToString(row["MLAvoidableFlagName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLCauseCodeId = row["MLCauseCodeID"] != DBNull.Value ? Convert.ToInt32(row["MLCauseCodeID"],
                        CultureInfo.InvariantCulture) : 0,
                        MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ? Convert.
                        ToString(row["MLCauseCodeName"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                        ToString(row["MLAccuracy"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        TicketOccurence = (Convert.ToInt32(row["TicketOccurence"] != DBNull.Value ? Convert.
                        ToInt32(row["TicketOccurence"],CultureInfo.InvariantCulture) : 0)),
                        MLResolutionCodeId = row["MLResolutionCodeID"] != DBNull.Value ? Convert.
                                ToInt32(row["MLResolutionCodeID"],CultureInfo.InvariantCulture) : 0,
                        MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ?
                                Convert.ToString(row["MLResolutionCodeName"],CultureInfo.InvariantCulture) : "", CultureInfo.CurrentCulture)),
                        IsApprovedOrMute = row["IsApprovedOrMute"] != DBNull.Value ? Convert.
                        ToInt32(row["IsApprovedOrMute"],CultureInfo.InvariantCulture) : 0,
                        SubPattern = (Convert.ToString(row["TicketSubPattern"] != DBNull.Value ? Convert.
                                ToString(row["TicketSubPattern"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        AdditionalTextPattern = (Convert.ToString(row["AdditionalPattern"] != DBNull.Value ?
                        Convert.ToString(row["AdditionalPattern"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        AdditionalTextsubPattern = (Convert.ToString(row["AdditionalSubPattern"] != DBNull.Value ?
                        Convert.ToString(row["AdditionalSubPattern"],CultureInfo.InvariantCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        IsCLSignoff = row["IsCLSignOff"] != DBNull.Value ? Convert.ToBoolean(row["IsCLSignOff"], CultureInfo.CurrentCulture) :
                        false,
                        PatternsOrigin = (Convert.ToString(row["PatternsOrigin"] != DBNull.Value ?
                        Convert.ToString(row["PatternsOrigin"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        IsDefaultRuleSelected = row["IsDefaultRuleSelected"] != DBNull.Value ? Convert.
                        ToInt32(row["IsDefaultRuleSelected"], CultureInfo.CurrentCulture) : 0,
                        IsApprovedPatternsConflict = (Convert.ToString(row["IsApprovedPatternsConflict"] !=
                        DBNull.Value ? Convert.ToString(row["IsApprovedPatternsConflict"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        ApprovedFlag = (Convert.ToString(row["ApprovedFlag"] != DBNull.Value ?
                                Convert.ToString(row["ApprovedFlag"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                    }).ToList();
                    DebtPatternValidation.DebtMLPatternValidationModel = debtMLPatternValidationList;
                }
                if (dtMLPatternValModel != null && dtMLPatternValModel.Tables[1].Rows.Count > 0)
                {
                    DebtPatternValidation.JobStatusMessage =
                        dtMLPatternValModel.Tables[1].Rows[0]["JobStatusMessage"].ToString();
                    if (dtMLPatternValModel.Tables[1].Columns.Contains("JobFromDate"))
                    {
                        DebtPatternValidation.JobFromDate =
                            dtMLPatternValModel.Tables[1].Rows[0]["JobFromDate"].ToString();
                    }
                    if (dtMLPatternValModel.Tables[1].Columns.Contains("JobToDate"))
                    {
                        DebtPatternValidation.JobToDate =
                            dtMLPatternValModel.Tables[1].Rows[0]["JobToDate"].ToString();
                    }
                }
                if (dtMLPatternValModel != null && dtMLPatternValModel.Tables[0].Rows.Count > 0)
                {
                    JobMessage = DebtPatternValidation.JobStatusMessage.Replace
                        (ApplicationConstants.RevisitNextWeek, "");
                    JobMessage = string.Format("{0} {1} to {2}.", JobMessage,
                        DebtPatternValidation.JobFromDate, DebtPatternValidation.JobToDate,CultureInfo.CurrentCulture);
                    DebtPatternValidation.JobStatusMessage = JobMessage;
                }
                else
                {
                    JobMessage = DebtPatternValidation.JobStatusMessage.Replace
                        (ApplicationConstants.RevisitNextWeek, "");
                    if (DebtPatternValidation.JobFromDate != null &&
                        DebtPatternValidation.JobFromDate != string.Empty)
                    {
                        JobMessage = string.Format("{0} {1} to {2}. {3} ", JobMessage,
                            DebtPatternValidation.JobFromDate, DebtPatternValidation.JobToDate,
                            ApplicationConstants.RevisitNextWeek,CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        JobMessage = string.Format("{0}", JobMessage,CultureInfo.CurrentCulture);
                        DebtPatternValidation.JobStatusMessage = JobMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DebtPatternValidation;
        }

        /// <summary>
        /// This method is used to VerifyJobStatus
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public JobDetails VerifyJobStatus(int ProjectID)
        {
            JobDetails jobDetails = new JobDetails();
            try
            {

                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                DataSet dtResult = new DataSet();
                dtResult.Locale = CultureInfo.InvariantCulture;
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("ML_VerifyJobStatus_CL", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    jobDetails.JobDate = dtResult.Tables[0].Rows[0]["JobDate"].ToString();
                    jobDetails.JobStatus = dtResult.Tables[0].Rows[0]["StatusForJob"].ToString();
                    jobDetails.IsCLEnabled = dtResult.Tables[0].Rows[0]["isCLEnabled"].ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobDetails;

        }
        /// <summary>
        /// This method is used to GetCLDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<CLDetails> GetCLDetails(int ProjectID)
        {
            List<CLDetails> lstGetMLDetailsOnLoad = new List<CLDetails>();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);

            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_GetCLDetailsOnLoad", prms, ConnectionString);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lstGetMLDetailsOnLoad.Add(new CLDetails
                    {
                        CLSignoff = ds.Tables[0].Rows.Count > 0 ? (Convert.ToInt32(ds.Tables[0].
                        Rows[0]["IsCLSignOff"] != DBNull.Value ? ds.Tables[0].Rows[0]["IsCLSignOff"] : 0, CultureInfo.CurrentCulture)) : 0,
                        CLAutoclassificationDatestring = ds.Tables[0].Rows.Count > 0 ? (Convert.ToString(ds.Tables[0].
                        Rows[0]["CLAutoclassificationDate"] != DBNull.Value ? Convert.ToString(ds.Tables[0].
                        Rows[0]["CLAutoclassificationDate"], CultureInfo.CurrentCulture) : "", CultureInfo.CurrentCulture)) : "",
                        IsCLAutoClassified = ds.Tables[0].Rows.Count > 0 ? (Convert.ToString(ds.Tables[0].
                        Rows[0]["IsCLAutoClassified"] != DBNull.Value ? Convert.ToString(ds.Tables[0].
                        Rows[0]["IsCLAutoClassified"], CultureInfo.CurrentCulture) : "N", CultureInfo.CurrentCulture)) : "N"

                    });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstGetMLDetailsOnLoad;

        }
        /// <summary>
        /// GetDebtMLPatternOccurenceReportContinuous
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternOccurenceReportContinuous(int projectID,
            int PatternApplicationID, int CauseCodeId, int ResolutionCodeId, string TicketPattern,
            string TicketSubPattern, string AddiPattern, string AddiSubPattern)
        {
            List<SpDebtMLPatternValidationModel> debtMLPatternValidationList =
                new List<SpDebtMLPatternValidationModel>();
            DataSet dtDebtMLPattern = new DataSet();
            dtDebtMLPattern.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[8];
                prms[0] = new SqlParameter("@projectID", projectID);
                prms[1] = new SqlParameter("@AppID", PatternApplicationID);
                prms[2] = new SqlParameter("@causeCodeId", CauseCodeId);
                prms[3] = new SqlParameter("@ResolutionCodeID", ResolutionCodeId);
                prms[4] = new SqlParameter("@TicketPattern", TicketPattern);
                prms[5] = new SqlParameter("@TicketSubPattern", TicketSubPattern);
                prms[6] = new SqlParameter("@AddiPattern", AddiPattern);
                prms[7] = new SqlParameter("@AddiSubPattern", AddiSubPattern);

                dtDebtMLPattern.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[CL_GetPatternOccurence]", prms, ConnectionString).Copy());
                if (dtDebtMLPattern.Tables[0] != null)
                {
                    debtMLPatternValidationList = dtDebtMLPattern.Tables[0].AsEnumerable().Select(row =>
                    new SpDebtMLPatternValidationModel
                    {
                        Id = row["ID"] != null ? Convert.ToInt32(row["ID"], CultureInfo.CurrentCulture) : 0,
                        InitialLearningId = (row["ContLearningID"] != null ? Convert.ToInt32(row["ContLearningID"], CultureInfo.CurrentCulture) : 0),
                        MLDebtClassificationName = (Convert.ToString(row["DebtClassificationName"] != DBNull.Value ?
                        Convert.ToString(row["DebtClassificationName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLResidualFlagName = (Convert.ToString(row["ResidualDebtName"] != DBNull.Value ? Convert.
                        ToString(row["ResidualDebtName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLAvoidableFlagName = (Convert.ToString(row["AvoidableFlagName"] != DBNull.Value ? Convert.
                        ToString(row["AvoidableFlagName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        TicketOccurence = row["TicketOccurence"] != null ? Convert.ToInt32(row["TicketOccurence"], CultureInfo.CurrentCulture) : 0,
                        MLAccuracy = (Convert.ToString(row["MLAccuracy"] !=
                        DBNull.Value ? Convert.ToString(row["MLAccuracy"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLDebtClassificationId = (row["MLDebtClassificationID"] != null ?
                        Convert.ToInt32(row["MLDebtClassificationID"], CultureInfo.CurrentCulture) : 0),
                        MLResidualFlagId = (row["MLResidualFlagID"] != null ?
                        Convert.ToInt32(row["MLResidualFlagID"], CultureInfo.CurrentCulture) : 0),
                        MLAvoidableFlagId = (row["MLAvoidableFlagID"] != null ?
                        Convert.ToInt32(row["MLAvoidableFlagID"], CultureInfo.CurrentCulture) : 0),
                        TicketPattern = (Convert.ToString(row["TicketPattern"] !=
                        DBNull.Value ? Convert.ToString(row["TicketPattern"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        SubPattern = (Convert.ToString(row["TicketSubPattern"] !=
                        DBNull.Value ? Convert.ToString(row["TicketSubPattern"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        AdditionalTextPattern = (Convert.ToString(row["AdditionalPattern"] !=
                        DBNull.Value ? Convert.ToString(row["AdditionalPattern"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        AdditionalTextsubPattern = (Convert.ToString(row["AdditionalSubPattern"] !=
                        DBNull.Value ? Convert.ToString(row["AdditionalSubPattern"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        ApplicationId = (row["ApplicationID"] != null ? Convert.ToInt32(row["ApplicationID"], CultureInfo.CurrentCulture) : 0),
                        MLResolutionCodeId = (row["MLResolutionCodeID"] != null ?
                        Convert.ToInt32(row["MLResolutionCodeID"], CultureInfo.CurrentCulture) : 0),
                        MLCauseCodeId = (row["MLCauseCodeID"] != null ? Convert.ToInt32(row["MLCauseCodeID"],CultureInfo.CurrentCulture) : 0),
                        MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ?
                                Convert.ToString(row["MLCauseCodeName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ?
                                Convert.ToString(row["MLResolutionCodeName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                        ApplicationName = (Convert.ToString(row["ApplicationName"] != DBNull.Value ? Convert.
                        ToString(row["ApplicationName"], CultureInfo.CurrentCulture) : string.Empty, CultureInfo.CurrentCulture)),
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
               // Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                  //  "Inside GetDebtMLPatternOccurenceReportContinuous", Convert.ToInt32(projectID));
            throw ex;
            }

            return debtMLPatternValidationList;
        }

    }
}
