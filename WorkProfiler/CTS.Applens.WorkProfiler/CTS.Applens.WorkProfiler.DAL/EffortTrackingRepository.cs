using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using CTS.Applens.WorkProfiler.Common.Common;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// This class holds EffortTrackingRepository details
    /// </summary>
    public class EffortTrackingRepository : DBContext
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
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                prms[1] = new SqlParameter("@CustomerID", customerID);
                DataTable dtML = (new DBHelper()).GetTableFromSP("[AVL].[CL_GetPortfolioName]", prms, ConnectionString);
                if (dtML != null)
                {
                    if (dtML.Rows.Count > 0)
                    {
                        return dtML.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Portfolio";
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
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            List<EffortDetailsByDate> lstResult = new List<EffortDetailsByDate>();
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@EmployeeID", employeeID);
            prms[1] = new SqlParameter("@Customer", customerid);
            prms[2] = new SqlParameter("@TSSartDate", MonthStartDate);
            prms[3] = new SqlParameter("@TSEndDate", MonthEndDate);
            if (!string.IsNullOrEmpty(employeeID))
            {
                dtResult = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetEffortWeekWiseChart]", prms, ConnectionString);
            }
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                lstResult = dtResult.AsEnumerable().Select(row => new EffortDetailsByDate
                {
                    TicketedEffort = Convert.ToString(row["Effort"]),
                    NonTicketedEffort = Convert.ToString(row["NonEffort"]),
                    TimesheetDate = Convert.ToDateTime((row["TimesheetDate"]) == DBNull.Value ?
                    string.Empty : row["TimesheetDate"]),
                    Holiday = "",
                    NoEffort = "",
                    PartialEfforts = "",
                    FullEfforts = ""
                }).ToList();
            }
            return lstResult;
        }


        /// <summary>
        /// This Method Is Used To GetLstProjectPriority
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstPriorityModel> GetLstProjectPriority(int ProjectID)
        {
            List<LstPriorityModel> lstPriority = new List<LstPriorityModel>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);

                dtResult = (new DBHelper()).GetTableFromSP("GetPriority", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstPriority = dtResult.AsEnumerable().Select(row => new LstPriorityModel
                    {
                        PriorityId = Convert.ToString((row["PriorityID"]) == DBNull.Value ?
                        string.Empty : row["PriorityID"]),
                        PriorityName = Convert.ToString((row["PriorityName"]) == DBNull.Value ?
                        string.Empty : row["PriorityName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstPriority;

        }
        /// <summary>
        /// This Method Is Used To GetLstProjectCauseCode
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstCauseCode> GetLstProjectCauseCode(Int32 ProjectID, Int32 ApplicationID)
        {
            List<LstCauseCode> lstCause = new List<LstCauseCode>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@ApplicationID", ApplicationID);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetProjectCauseCode", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstCause = dtResult.AsEnumerable().Select(row => new LstCauseCode
                    {
                        CauseId = Convert.ToString((row["CauseID"]) == DBNull.Value ? string.Empty :
                        row["CauseID"]),
                        CauseName = Convert.ToString((row["CauseCode"]) == DBNull.Value ? string.Empty :
                        row["CauseCode"]),
                        CauseMapCount = Convert.ToInt32((row["MapCount"]) == DBNull.Value ? 0 :
                        row["MapCount"]),
                        CauseIsMapped = Convert.ToString((row["IsMapped"]) == DBNull.Value ?
                        string.Empty : row["IsMapped"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCause;
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
            GetDebtAvoidResidual objsp_GetDebtAvoidResidual = new GetDebtAvoidResidual();
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@ApplicationName", Application);
            prms[2] = new SqlParameter("@TicketDescription", TicketDescription);
            prms[4] = new SqlParameter("@Resolutioncode", ResolutionCode);
            prms[3] = new SqlParameter("@Causecode", CauseCode);
            prms[5] = new SqlParameter("@IsAutoClassified", IsAutoClassified);
            prms[6] = new SqlParameter("@IsDDAutoClassified", IsDDAutoClassified);
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("AVL_DebtGetAutoClassifiedDebtFilds", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {

                            objsp_GetDebtAvoidResidual.DebtClassification = Convert.ToInt32(((dt.Tables[0].
                                Rows[i]["DebtClassificationId"] != DBNull.Value) ? dt.Tables[0].
                                Rows[i]["DebtClassificationId"] : 0));
                            objsp_GetDebtAvoidResidual.ResidualDebt = Convert.ToInt32(((dt.Tables[0].
                                Rows[i]["ResidualFlagID"] != DBNull.Value) ? dt.Tables[0].
                                Rows[i]["ResidualFlagID"] : 0));
                            objsp_GetDebtAvoidResidual.AvoidableFlag = Convert.ToInt32(((dt.Tables[0].
                                Rows[i]["AvoidableFlagID"] != DBNull.Value) ? dt.Tables[0].
                                Rows[i]["AvoidableFlagID"] : 0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objsp_GetDebtAvoidResidual;
        }

        /// <summary>
        /// This Method Is Used To GetAutoClassifiedDetailsForDebt
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public DataTable GetAutoClassifiedDetailsForDebt(string ProjectID)
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectId", ProjectID);


            dt = (new DBHelper()).GetTableFromSP("AVL_debt_getautoclassifiedfieldforsharepathchange", prms, ConnectionString);
            return dt;
        }
        /// <summary>
        /// This Method Is Used To GetLstProjectResolutionCode
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstResolution> GetLstProjectResolutionCode(string ProjectID, string CauseCode)
        {
            List<LstResolution> lstResolution = new List<LstResolution>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@CauseCode", CauseCode);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetProjectResolutionCode", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstResolution = dtResult.AsEnumerable().Select(row => new LstResolution
                    {
                        IsMapped = Convert.ToString((row["IsMapped"]) == DBNull.Value ?
                        string.Empty : row["IsMapped"]),
                        ResolutionId = Convert.ToString((row["ResolutionID"]) == DBNull.Value ?
                        string.Empty : row["ResolutionID"]),
                        ResolutionName = Convert.ToString((row["ResolutionCode"]) == DBNull.Value ?
                        string.Empty : row["ResolutionCode"]),
                        MapCount = Convert.ToInt32((row["MapCount"]) == DBNull.Value ? 0 : row["MapCount"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResolution;

        }
        /// <summary>
        /// This Method Is Used To GetLstDebtClassification
        /// </summary>
        /// <returns></returns>
        public List<LstDebtClassification> GetLstDebtClassification(int SupportTypeID)
        {
            List<LstDebtClassification> lstDebt = new List<LstDebtClassification>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@SupportTypeID", SupportTypeID);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetDebtClassification", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstDebt = dtResult.AsEnumerable().Select(row => new LstDebtClassification
                    {
                        DebtClassificationId = Convert.ToString((row["DebtClassificationID"]) == DBNull.Value ?
                        string.Empty : row["DebtClassificationID"]),
                        DebtClassificationName = Convert.ToString((row["DebtClassificationName"]) == DBNull.Value ?
                        string.Empty : row["DebtClassificationName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDebt;

        }
        /// <summary>
        /// This Method Is Used To GetLstSeverity
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstSeverity> GetLstSeverity(int ProjectID)
        {
            List<LstSeverity> lstProjectSeverity = new List<LstSeverity>();
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("AVL.TK_GetProjectSeverity", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    lstProjectSeverity = dtResult.Tables[0].AsEnumerable().Select(row => new LstSeverity
                    {
                        SeverityId = Convert.ToString((row["SeverityID"]) == DBNull.Value ? string.Empty :
                        row["SeverityID"]),
                        SeverityName = Convert.ToString((row["SeverityName"]) == DBNull.Value ? string.Empty :
                        row["SeverityName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstProjectSeverity;

        }
        /// <summary>
        /// This Method Is Used To GetLstTicketSource
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<LstTicketSource> GetLstTicketSource(Int32 ProjectID)
        {
            List<LstTicketSource> lstTicketSource = new List<LstTicketSource>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetTicketSource", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstTicketSource = dtResult.AsEnumerable().Select(row => new LstTicketSource
                    {
                        TicketSourceId = Convert.ToString((row["TicketSourceID"]) == DBNull.Value ?
                        string.Empty : row["TicketSourceID"]),
                        TicketSourceName = Convert.ToString((row["TicketSourceName"]) == DBNull.Value ?
                        string.Empty : row["TicketSourceName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTicketSource;

        }
        /// <summary>
        /// This Method Is Used To GetLstReleaseType
        /// </summary>
        /// <returns></returns>
        public List<LstReleaseType> GetLstReleaseType()
        {
            List<LstReleaseType> lstRelease = new List<LstReleaseType>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {

                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetReleaseType", ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstRelease = dtResult.AsEnumerable().Select(row => new LstReleaseType
                    {
                        ReleaseTypeId = Convert.ToString((row["ReleaseTypeID"]) == DBNull.Value ?
                        string.Empty : row["ReleaseTypeID"]),
                        ReleaseTypeName = Convert.ToString((row["ReleaseTypeName"]) == DBNull.Value ?
                        string.Empty : row["ReleaseTypeName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstRelease;

        }
        /// <summary>
        /// This Method Is Used To GetLstBusinessImpact
        /// </summary>
        /// <returns></returns>
        public List<GetBusinessImpact> GetBusinessImpact()
        {
            List<GetBusinessImpact> lstBusinessImpact = new List<GetBusinessImpact>();
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                dtResult = (new DBHelper()).GetDatasetFromSP("AVL.TK_GetReleaseType", ConnectionString);
               
                if (dtResult.Tables[1].Rows != null && dtResult.Tables[1].Rows.Count > 0)
                {
                    lstBusinessImpact = dtResult.Tables[1].AsEnumerable().Select(row => new GetBusinessImpact
                    {
                        BusinessImpactId = Convert.ToInt16((row["BusinessImpactId"]) == DBNull.Value ?
                        string.Empty : row["BusinessImpactId"]),
                        BusinessImpactName = Convert.ToString((row["BusinessImpactName"]) == DBNull.Value ?
                        string.Empty : row["BusinessImpactName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstBusinessImpact;

        }
        /// <summary>
        /// This Method Is Used To GetLstKEDBUpdated
        /// </summary>
        /// <returns></returns>
        public List<LstKEDBUpdated> GetLstKEDBUpdated()
        {
            List<LstKEDBUpdated> lstKEDB = new List<LstKEDBUpdated>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {

                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetKEDBUpdated", ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstKEDB = dtResult.AsEnumerable().Select(row => new LstKEDBUpdated
                    {
                        KEDBUpdatedId = Convert.ToString((row["KEDBUpdatedID"]) == DBNull.Value ?
                        string.Empty : row["KEDBUpdatedID"]),
                        KEDBUpdatedName = Convert.ToString((row["KEDBUpdatedName"]) == DBNull.Value ?
                        string.Empty : row["KEDBUpdatedName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstKEDB;

        }
        /// <summary>
        /// This Method Is Used To GetLstTicketType
        /// </summary>
        /// <returns></returns>
        public List<LstTicketType> GetLstTicketType()
        {
            List<LstTicketType> lstTicketType = new List<LstTicketType>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {

                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetTicketType", ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstTicketType = dtResult.AsEnumerable().Select(row => new LstTicketType
                    {
                        TicTypeId = Convert.ToString((row["TicketTypeMappingID"]) == DBNull.Value ?
                        string.Empty : row["TicketTypeMappingID"]),
                        TicTypeName = Convert.ToString((row["TicketType"]) == DBNull.Value ?
                        string.Empty : row["TicketType"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTicketType;

        }
        /// <summary>
        /// This Method Is Used To GetDartStatus
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="StatusID">StatusID</param>
        /// <returns></returns>
        public List<PopupAttributeModel> GetDartStatus(int ProjectID, int StatusID)
        {
            List<PopupAttributeModel> lstDartStatus = new List<PopupAttributeModel>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@StatusID", StatusID);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetDartStatus", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstDartStatus = dtResult.AsEnumerable().Select(row => new PopupAttributeModel
                    {
                        DARTStatusId = Convert.ToString((row["DARTStatusID"]) == DBNull.Value ?
                        string.Empty : row["DARTStatusID"]),
                        DARTStatusName = Convert.ToString((row["DARTStatusName"]) == DBNull.Value ?
                        string.Empty : row["DARTStatusName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDartStatus;
        }
        /// <summary>
        /// This Method Is Used To GetkedbAvailable
        /// </summary>
        /// <returns>returns List<LstkedbAvailable></returns>
        public List<LstkedbAvailable> GetkedbAvailable()
        {
            List<LstkedbAvailable> lstKEDBAvailable = new List<LstkedbAvailable>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {

                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetKEDBAvailable", ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstKEDBAvailable = dtResult.AsEnumerable().Select(row => new LstkedbAvailable
                    {
                        Id = Convert.ToString((row["KEDBAvailableIndicatorID"]) == DBNull.Value ?
                        string.Empty : row["KEDBAvailableIndicatorID"]),
                        KEDBAvailableName = Convert.ToString((row["KEDBAvailableIndicatorName"]) ==
                        DBNull.Value ? string.Empty : row["KEDBAvailableIndicatorName"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstKEDBAvailable;

        }
        /// <summary>
        /// This Method Is Used To GetPopupAttributeData
        /// </summary>
        /// <param name="objPopupattributeget">objPopupattributeget</param>
        /// <returns></returns>
        public List<PopupAttributeModel> GetPopupAttributeData(Popupattributeget objPopupattributeget)
        {
            List<PopupAttributeModel> lstAttributes = new List<PopupAttributeModel>();
            PopupAttributeModel addtionalAttributeHolder = new PopupAttributeModel();
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@ProjectID", objPopupattributeget.ProjectId);
                prms[1] = new SqlParameter("@ServiceID", objPopupattributeget.ServiceId);
                prms[2] = new SqlParameter("@StatusID", objPopupattributeget.StatusId);
                prms[3] = new SqlParameter("@TicketID", objPopupattributeget.TicketId);
                prms[4] = new SqlParameter("@TicketTypeId", objPopupattributeget.TicketTypeId);
                prms[5] = new SqlParameter("@SupportTypeID", objPopupattributeget.SupportTypeId);
                dtResult = (new DBHelper()).GetTableFromSP("AVL.TK_GetPopupAttributeDetails", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    if (encryptionEnabled == "Enabled")
                    {
                        lstAttributes = dtResult.AsEnumerable().Select(row => new PopupAttributeModel
                        {
                            ApplicationName = Convert.ToString((row["ApplicationName"]) == DBNull.Value ?
                            string.Empty : row["ApplicationName"]),
                            TowerName = Convert.ToString((row["TowerName"]) == DBNull.Value ?
                            string.Empty : row["TowerName"]),
                            TowerId = Convert.ToInt16((row["TowerID"]) == DBNull.Value ?
                            string.Empty : row["TowerID"]),
                            TicketId = Convert.ToString((row["TicketID"]) == DBNull.Value ? string.Empty
                            : row["TicketID"]),
                            TicketOpenDate = Convert.ToDateTime((row["TicketOpenDate"]) == DBNull.Value ?
                            string.Empty : (row["TicketOpenDate"])),
                            TicketDescription =
                            Convert.ToString(string.IsNullOrEmpty(row["TicketDescription"].ToString()) ?
                            string.Empty :
                            aesMod.DecryptStringBytes((string)row["TicketDescription"],
                             AseKeyDetail.AesKeyConstVal)),
                            Priority = Convert.ToString((row["Priority"]) == DBNull.Value ? string.Empty :
                            row["Priority"]),
                            BusinessImpactId= Convert.ToInt32((row["BusinessImpactId"]) == DBNull.Value ? 0 :
                            row["BusinessImpactId"]),
                            ImpactComments= Convert.ToString((row["ImpactComments"]) == DBNull.Value ? string.Empty :
                            row["ImpactComments"]),
                            TicketType = Convert.ToString((row["TicketType"]) == DBNull.Value ? string.Empty :
                            row["TicketType"]),
                            CauseCode = Convert.ToString((row["CauseCode"]) == DBNull.Value ? string.Empty :
                            row["CauseCode"]),
                            ResolutionCode = Convert.ToString((row["ResolutionCode"]) == DBNull.Value ? string.Empty :
                            row["ResolutionCode"]),
                            DebtType = Convert.ToString((row["DebtType"]) == DBNull.Value ? string.Empty :
                            row["DebtType"]),
                            AvoidableFlag = Convert.ToString((row["AvoidableFlag"]) == DBNull.Value ? string.Empty :
                            row["AvoidableFlag"]),
                            ResidualDebt = Convert.ToString((row["ResidualDebt"]) == DBNull.Value ? string.Empty :
                            row["ResidualDebt"]),
                            Severity = Convert.ToString((row["Severity"]) == DBNull.Value ? string.Empty :
                            row["Severity"]),
                            AssignedTo = Convert.ToString((row["AssignedTo"]) == DBNull.Value ? string.Empty :
                            row["AssignedTo"]),
                            TicketSource = Convert.ToString((row["TicketSource"]) == DBNull.Value ? string.Empty :
                            row["TicketSource"]),
                            ReleaseType = Convert.ToString((row["ReleaseType"]) == DBNull.Value ? string.Empty :
                            row["ReleaseType"]),
                            EstimatedWorkSize = Convert.ToString((row["EstimatedWorkSize"]) == DBNull.Value ?
                            string.Empty : row["EstimatedWorkSize"]),
                            ActualEffort = Convert.ToDecimal((row["ActualEffort"]) == DBNull.Value ? "0.0" :
                            row["ActualEffort"]),
                            TicketCreatedDate = Convert.ToDateTime((row["TicketCreateDate"]) == DBNull.Value ?
                            string.Empty : (row["TicketCreateDate"])),
                            ActualStartDateTime = Convert.ToDateTime((row["ActualStartdateTime"]) == DBNull.Value ?
                            string.Empty : (row["ActualStartdateTime"])),
                            ActualEndtDateTime = Convert.ToDateTime((row["ActualEnddateTime"]) == DBNull.Value ?
                            string.Empty : (row["ActualEnddateTime"])),
                            ClosedDate = Convert.ToDateTime((row["Closeddate"]) == DBNull.Value ? string.Empty :
                            (row["Closeddate"])),
                            KEDBUpdated = Convert.ToString((row["KEDBUpdated"]) == DBNull.Value ? string.Empty :
                            row["KEDBUpdated"]),
                            KedbAvailable = Convert.ToString((row["KEDBAvailable"]) == DBNull.Value ? string.Empty :
                            row["KEDBAvailable"]),
                            KEDBPath = Convert.ToString((row["KEDBPath"]) == DBNull.Value ? string.Empty :
                            row["KEDBPath"]),
                            FlexField1 = Convert.ToString((row["FlexField1"]) == DBNull.Value ? string.Empty :
                            row["FlexField1"]),
                            FlexField2 = Convert.ToString((row["FlexField2"]) == DBNull.Value ? string.Empty :
                            row["FlexField2"]),
                            FlexField3 = Convert.ToString((row["FlexField3"]) == DBNull.Value ? string.Empty :
                            row["FlexField3"]),
                            FlexField4 = Convert.ToString((row["FlexField4"]) == DBNull.Value ? string.Empty :
                            row["FlexField4"]),
                            RCAId = Convert.ToString((row["RCAID"]) == DBNull.Value ? string.Empty : row["RCAID"]),
                            MetResponseSLA = Convert.ToString((row["MetResponseSLA"]) == DBNull.Value ? string.Empty :
                            row["MetResponseSLA"]),
                            MetResolution = Convert.ToString((row["MetResolution"]) == DBNull.Value ? string.Empty :
                            row["MetResolution"]),
                            OpenDateTime = Convert.ToDateTime((row["OpenDateTime"]) == DBNull.Value ? string.Empty :
                            (row["OpenDateTime"])),
                            CompletedDateTime = Convert.ToDateTime((row["CompletedDateTime"]) == DBNull.Value ?
                            string.Empty : (row["CompletedDateTime"])),
                            ReopenDateTime = Convert.ToDateTime((row["ReopenDateTime"]) == DBNull.Value ?
                            string.Empty : (row["ReopenDateTime"])),
                            ServiceName = Convert.ToString((row["ServiceName"]) == DBNull.Value ? string.Empty :
                            row["ServiceName"]),
                            StatusName = Convert.ToString((row["DARTStatusName"]) == DBNull.Value ? string.Empty :
                            row["DARTStatusName"]),
                            ServiceId = Convert.ToString((row["ServiceID"]) == DBNull.Value ? string.Empty :
                            row["ServiceID"]),
                            StatusId = Convert.ToString((row["DARTStatusID"]) == DBNull.Value ? string.Empty :
                            row["DARTStatusID"]),
                            ProjectId = Convert.ToString((row["ProjectID"]) == DBNull.Value ? string.Empty :
                            row["ProjectID"]),
                            ResolutionMethodName = Convert.ToString((row["ResolutionMethodName"]) == DBNull.Value ?
                            string.Empty : row["ResolutionMethodName"]),
                            TicketSourceId = Convert.ToString((row["TicketSourceMapID"]) == DBNull.Value ?
                            string.Empty : row["TicketSourceMapID"]),
                            CauseCodeId = Convert.ToString((row["CauseCodeMapID"]) == DBNull.Value ?
                            string.Empty : row["CauseCodeMapID"]),
                            ResolutionCodeId = Convert.ToString((row["ResolutionCodeMapID"]) == DBNull.Value ?
                            string.Empty : row["ResolutionCodeMapID"]),
                            ResidualDebtId = Convert.ToString((row["ResidualDebtMapID"]) == DBNull.Value ?
                            string.Empty : row["ResidualDebtMapID"]),
                            MetResolutionId = Convert.ToString((row["MetResolutionMapID"]) == DBNull.Value ?
                            string.Empty : row["MetResolutionMapID"]),
                            MetResponseSLAId = Convert.ToString((row["MetResponseSLAMapID"]) == DBNull.Value ?
                            string.Empty : row["MetResponseSLAMapID"]),
                            ResolutionMethodId = Convert.ToString((row["ResolutionMethodMapID"]) == DBNull.Value ?
                            string.Empty : row["ResolutionMethodMapID"]),
                            PriotityId = Convert.ToString((row["PriorityMapID"]) == DBNull.Value ? string.Empty :
                            row["PriorityMapID"]),
                            SeverityId = Convert.ToString((row["SeverityMapID"]) == DBNull.Value ? string.Empty :
                            row["SeverityMapID"]),
                            ApplicationId = Convert.ToString((row["ApplicationID"]) == DBNull.Value ? string.Empty :
                            row["ApplicationID"]),
                            KedbAvailableId = Convert.ToString((row["KEDBAvailableIndicatorMapID"]) == DBNull.Value ?
                            string.Empty : row["KEDBAvailableIndicatorMapID"]),
                            KedbUpdateId = Convert.ToString((row["KEDBUpdatedMapID"]) == DBNull.Value ? string.Empty
                            : row["KEDBUpdatedMapID"]),
                            DebtClassificationId = Convert.ToString((row["DebtClassificationMapID"]) == DBNull.Value ?
                            string.Empty : row["DebtClassificationMapID"]),
                            ReleaseTypeId = Convert.ToString((row["ReleaseTypeMapID"]) == DBNull.Value ? string.Empty
                            : row["ReleaseTypeMapID"]),
                            TicketTypeId = Convert.ToString((row["TicketTypeMapID"]) == DBNull.Value ? string.Empty :
                            row["TicketTypeMapID"]),
                            TicketStatusId = Convert.ToString((row["TicketStatusID"]) == DBNull.Value ?
                            string.Empty : row["TicketStatusID"]),
                            /* Addtional Columns*/
                            //BusinessImpact = Convert.ToString((row["BusinessImpact"]) == DBNull.Value ? 
                            //string.Empty : row["BusinessImpact"]),
                            Comments = Convert.ToString((row["Comments"]) == DBNull.Value ? string.Empty :
                            row["Comments"]),
                            //CSATScore = Convert.ToDecimal((row["CSATScore"]) == DBNull.Value ? string.Empty :
                            //row["CSATScore"]),
                            //JobProcessName = Convert.ToString((row["JobProcessName"]) == DBNull.Value ? 
                            //string.Empty : row["JobProcessName"]),
                            //OutageFlag = Convert.ToString((row["OutageFlag"]) == DBNull.Value ? 
                            //string.Empty : row["OutageFlag"]),
                            PlannedEffort = Convert.ToString((row["PlannedEffort"]) == DBNull.Value ?
                            string.Empty : row["PlannedEffort"]),
                            PlannedEndDate = Convert.ToDateTime((row["PlannedEndDate"]) == DBNull.Value ?
                            string.Empty : row["PlannedEndDate"]),
                            PlannedStartDate = Convert.ToDateTime((row["PlannedStartDate"]) == DBNull.Value ?
                            string.Empty : row["PlannedStartDate"]),
                            ReleaseDate = Convert.ToDateTime((row["ReleaseDate"]) == DBNull.Value ?
                            string.Empty : row["ReleaseDate"]),
                            TicketStatus = Convert.ToString((row["TicketStatus"]) == DBNull.Value ? string.Empty
                            : row["TicketStatus"]),
                            TicketSummary =
                            Convert.ToString(string.IsNullOrEmpty(row["TicketSummary"].ToString()) ? string.Empty :
                            aesMod.DecryptStringBytes((string)row["TicketSummary"],
                            AseKeyDetail.AesKeyConstVal)),
                            DebtClassificationMode = Convert.ToString((row["DebtClassificationMode"]) == DBNull.Value ?
                            string.Empty : row["DebtClassificationMode"]),
                            SupportTypeId = Convert.ToInt16((row["SupportTypeID"]) == DBNull.Value ? 0
                            : row["SupportTypeID"]),
                            OptionalAttributeType = (row["OptionalAttributeType"]
                                                    != DBNull.Value ?
                                                    Convert.ToInt16(row["OptionalAttributeType"]) : 0),
                            IsFlexField1Configured = (row["IsFlexField1Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField1Configured"])
                                                    : false),
                            IsFlexField2Configured = (row["IsFlexField2Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField2Configured"])
                                                    : false),
                            IsFlexField3Configured = (row["IsFlexField3Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField3Configured"])
                                                    : false),
                            IsFlexField4Configured = (row["IsFlexField4Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField4Configured"])
                                                    : false),
                            IsResolutionReConfigured = (row["IsResolutionReConfigured"]
                                                 != DBNull.Value ? Convert.ToBoolean(row["IsResolutionReConfigured"])
                                                    : false),
                            IsPartiallyAutomated = Convert.ToInt16((row["IsPartiallyAutomated"]
                                                 != DBNull.Value ? Convert.ToInt16(row["IsPartiallyAutomated"])
                                                    : 2)),
                            AutoClassificationType = Convert.ToInt16((row["AutoClassificationType"]
                                                 != DBNull.Value ? Convert.ToInt16(row["AutoClassificationType"])
                                                    : 0)),
                            AssignedUserID = Convert.ToString((row["AssignedUserID"]) == DBNull.Value ? string.Empty :
                            row["AssignedUserID"])
                        }).ToList();
                    }
                    else
                    {
                        lstAttributes = dtResult.AsEnumerable().Select(row => new PopupAttributeModel
                        {
                            ApplicationName = Convert.ToString((row["ApplicationName"]) == DBNull.Value ?
                            string.Empty : row["ApplicationName"]),
                            TowerName = Convert.ToString((row["TowerName"]) == DBNull.Value ?
                            string.Empty : row["TowerName"]),
                            TowerId = Convert.ToInt16((row["TowerID"]) == DBNull.Value ?
                            string.Empty : row["TowerID"]),
                            TicketId = Convert.ToString((row["TicketID"]) == DBNull.Value ? string.Empty :
                            row["TicketID"]),
                            TicketOpenDate = Convert.ToDateTime((row["TicketOpenDate"]) == DBNull.Value ?
                            string.Empty : (row["TicketOpenDate"])),
                            TicketDescription = Convert.ToString((row["TicketDescription"]) == DBNull.Value ?
                            string.Empty : row["TicketDescription"]),
                            Priority = Convert.ToString((row["Priority"]) == DBNull.Value ? string.Empty :
                            row["Priority"]),
                            TicketType = Convert.ToString((row["TicketType"]) == DBNull.Value ? string.Empty :
                            row["TicketType"]),
                            CauseCode = Convert.ToString((row["CauseCode"]) == DBNull.Value ? string.Empty :
                            row["CauseCode"]),
                            ResolutionCode = Convert.ToString((row["ResolutionCode"]) == DBNull.Value ?
                            string.Empty : row["ResolutionCode"]),
                            DebtType = Convert.ToString((row["DebtType"]) == DBNull.Value ?
                            string.Empty : row["DebtType"]),
                            AvoidableFlag = Convert.ToString((row["AvoidableFlag"]) == DBNull.Value ?
                            string.Empty : row["AvoidableFlag"]),
                            ResidualDebt = Convert.ToString((row["ResidualDebt"]) == DBNull.Value ?
                            string.Empty : row["ResidualDebt"]),
                            Severity = Convert.ToString((row["Severity"]) == DBNull.Value ?
                            string.Empty : row["Severity"]),
                            AssignedTo = Convert.ToString((row["AssignedTo"]) == DBNull.Value ?
                            string.Empty : row["AssignedTo"]),
                            TicketSource = Convert.ToString((row["TicketSource"]) == DBNull.Value ?
                            string.Empty : row["TicketSource"]),
                            ReleaseType = Convert.ToString((row["ReleaseType"]) == DBNull.Value ?
                            string.Empty : row["ReleaseType"]),
                            EstimatedWorkSize = Convert.ToString((row["EstimatedWorkSize"]) == DBNull.Value ?
                            string.Empty : row["EstimatedWorkSize"]),
                            ActualEffort = Convert.ToDecimal((row["ActualEffort"]) == DBNull.Value ? "0.0" :
                            row["ActualEffort"]),
                            TicketCreatedDate = Convert.ToDateTime((row["TicketCreateDate"]) == DBNull.Value ?
                            string.Empty : (row["TicketCreateDate"])),
                            ActualStartDateTime = Convert.ToDateTime((row["ActualStartdateTime"]) == DBNull.Value ?
                            string.Empty : (row["ActualStartdateTime"])),
                            ActualEndtDateTime = Convert.ToDateTime((row["ActualEnddateTime"]) == DBNull.Value ?
                            string.Empty : (row["ActualEnddateTime"])),
                            ClosedDate = Convert.ToDateTime((row["Closeddate"]) == DBNull.Value ? string.Empty :
                            (row["Closeddate"])),
                            KEDBUpdated = Convert.ToString((row["KEDBUpdated"]) == DBNull.Value ? string.Empty :
                            row["KEDBUpdated"]),
                            KedbAvailable = Convert.ToString((row["KEDBAvailable"]) == DBNull.Value ? string.Empty :
                            row["KEDBAvailable"]),
                            KEDBPath = Convert.ToString((row["KEDBPath"]) == DBNull.Value ? string.Empty :
                            row["KEDBPath"]),
                            FlexField1 = Convert.ToString((row["FlexField1"]) == DBNull.Value ? string.Empty :
                            row["FlexField1"]),
                            FlexField2 = Convert.ToString((row["FlexField2"]) == DBNull.Value ? string.Empty :
                            row["FlexField2"]),
                            FlexField3 = Convert.ToString((row["FlexField3"]) == DBNull.Value ? string.Empty :
                            row["FlexField3"]),
                            FlexField4 = Convert.ToString((row["FlexField4"]) == DBNull.Value ? string.Empty :
                            row["FlexField4"]),
                            RCAId = Convert.ToString((row["RCAID"]) == DBNull.Value ? string.Empty : row["RCAID"]),
                            MetResponseSLA = Convert.ToString((row["MetResponseSLA"]) == DBNull.Value ? string.Empty :
                            row["MetResponseSLA"]),
                            MetResolution = Convert.ToString((row["MetResolution"]) == DBNull.Value ? string.Empty :
                            row["MetResolution"]),
                            OpenDateTime = Convert.ToDateTime((row["OpenDateTime"]) == DBNull.Value ? string.Empty :
                            (row["OpenDateTime"])),
                            CompletedDateTime = Convert.ToDateTime((row["CompletedDateTime"]) == DBNull.Value ?
                            string.Empty : (row["CompletedDateTime"])),
                            ReopenDateTime = Convert.ToDateTime((row["ReopenDateTime"]) == DBNull.Value ? string.Empty :
                            (row["ReopenDateTime"])),
                            ServiceName = Convert.ToString((row["ServiceName"]) == DBNull.Value ? string.Empty :
                            row["ServiceName"]),
                            StatusName = Convert.ToString((row["DARTStatusName"]) == DBNull.Value ? string.Empty :
                            row["DARTStatusName"]),
                            ServiceId = Convert.ToString((row["ServiceID"]) == DBNull.Value ? string.Empty :
                            row["ServiceID"]),
                            StatusId = Convert.ToString((row["DARTStatusID"]) == DBNull.Value ? string.Empty :
                            row["DARTStatusID"]),
                            ProjectId = Convert.ToString((row["ProjectID"]) == DBNull.Value ? string.Empty :
                            row["ProjectID"]),
                            ResolutionMethodName = Convert.ToString((row["ResolutionMethodName"]) == DBNull.Value ?
                            string.Empty : row["ResolutionMethodName"]),
                            TicketSourceId = Convert.ToString((row["TicketSourceMapID"]) == DBNull.Value ?
                            string.Empty : row["TicketSourceMapID"]),
                            CauseCodeId = Convert.ToString((row["CauseCodeMapID"]) == DBNull.Value ?
                            string.Empty : row["CauseCodeMapID"]),
                            ResolutionCodeId = Convert.ToString((row["ResolutionCodeMapID"]) ==
                            DBNull.Value ? string.Empty : row["ResolutionCodeMapID"]),
                            ResidualDebtId = Convert.ToString((row["ResidualDebtMapID"]) ==
                            DBNull.Value ? string.Empty : row["ResidualDebtMapID"]),
                            MetResolutionId = Convert.ToString((row["MetResolutionMapID"]) == DBNull.Value ?
                            string.Empty : row["MetResolutionMapID"]),
                            MetResponseSLAId = Convert.ToString((row["MetResponseSLAMapID"]) == DBNull.Value ?
                            string.Empty : row["MetResponseSLAMapID"]),
                            ResolutionMethodId = Convert.ToString((row["ResolutionMethodMapID"]) == DBNull.Value ?
                            string.Empty : row["ResolutionMethodMapID"]),
                            PriotityId = Convert.ToString((row["PriorityMapID"]) == DBNull.Value ? string.Empty :
                            row["PriorityMapID"]),
                            SeverityId = Convert.ToString((row["SeverityMapID"]) == DBNull.Value ? string.Empty :
                            row["SeverityMapID"]),
                            ApplicationId = Convert.ToString((row["ApplicationID"]) == DBNull.Value ? string.Empty :
                            row["ApplicationID"]),
                            KedbAvailableId = Convert.ToString((row["KEDBAvailableIndicatorMapID"]) == DBNull.Value ?
                            string.Empty : row["KEDBAvailableIndicatorMapID"]),
                            KedbUpdateId = Convert.ToString((row["KEDBUpdatedMapID"]) == DBNull.Value ? string.Empty :
                            row["KEDBUpdatedMapID"]),
                            DebtClassificationId = Convert.ToString((row["DebtClassificationMapID"]) == DBNull.Value ?
                            string.Empty : row["DebtClassificationMapID"]),
                            ReleaseTypeId = Convert.ToString((row["ReleaseTypeMapID"]) == DBNull.Value ?
                            string.Empty : row["ReleaseTypeMapID"]),
                            TicketTypeId = Convert.ToString((row["TicketTypeMapID"]) == DBNull.Value ?
                            string.Empty : row["TicketTypeMapID"]),
                            TicketStatusId = Convert.ToString((row["TicketStatusID"]) == DBNull.Value ?
                            string.Empty : row["TicketStatusID"]),
                            /* Addtional Columns*/
                            Comments = Convert.ToString((row["Comments"]) == DBNull.Value ? string.Empty :
                            row["Comments"]),
                            PlannedEffort = Convert.ToString((row["PlannedEffort"]) == DBNull.Value ?
                            string.Empty : row["PlannedEffort"]),
                            PlannedEndDate = Convert.ToDateTime((row["PlannedEndDate"]) == DBNull.Value ?
                            string.Empty : row["PlannedEndDate"]),
                            PlannedStartDate = Convert.ToDateTime((row["PlannedStartDate"]) == DBNull.Value ?
                            string.Empty : row["PlannedStartDate"]),
                            ReleaseDate = Convert.ToDateTime((row["ReleaseDate"]) == DBNull.Value ?
                            string.Empty : row["ReleaseDate"]),
                            TicketStatus = Convert.ToString((row["TicketStatus"]) == DBNull.Value ?
                            string.Empty : row["TicketStatus"]),
                            TicketSummary = Convert.ToString((row["TicketSummary"]) == DBNull.Value ?
                            string.Empty : row["TicketSummary"]),
                            SupportTypeId = Convert.ToInt16((row["SupportTypeID"]) == DBNull.Value ? 0
                            : row["SupportTypeID"]),
                            OptionalAttributeType = (row["OptionalAttributeType"]
                                                    != DBNull.Value ?
                                                    Convert.ToInt16(row["OptionalAttributeType"]) : 0),
                            IsFlexField1Configured = (row["IsFlexField1Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField1Configured"])
                                                    : false),
                            IsFlexField2Configured = (row["IsFlexField2Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField2Configured"])
                                                    : false),
                            IsFlexField3Configured = (row["IsFlexField3Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField3Configured"])
                                                    : false),
                            IsFlexField4Configured = (row["IsFlexField4Configured"]
                                                    != DBNull.Value ? Convert.ToBoolean(row["IsFlexField4Configured"])
                                                    : false),
                            IsResolutionReConfigured = (row["IsResolutionReConfigured"]
                                                 != DBNull.Value ? Convert.ToBoolean(row["IsResolutionReConfigured"])
                                                    : false),
                            IsPartiallyAutomated = Convert.ToInt16((row["IsPartiallyAutomated"]
                                                 != DBNull.Value ? Convert.ToInt16(row["IsPartiallyAutomated"])
                                                    : 2)),
                            AutoClassificationType = Convert.ToInt16((row["AutoClassificationType"]
                                                 != DBNull.Value ? Convert.ToInt16(row["AutoClassificationType"])
                                                    : 0)),
                            DebtClassificationMode = Convert.ToString((row["DebtClassificationMode"]) == DBNull.Value ?
                            string.Empty : row["DebtClassificationMode"]),
                            AssignedUserID = Convert.ToString((row["AssignedUserID"]) == DBNull.Value ? string.Empty :
                            row["AssignedUserID"])
                        }).ToList();
                    }
                }

                addtionalAttributeHolder = GetAddtionalAttributeDetails();
                lstAttributes.ForEach(x =>
                {
                    x.LstEscalatedFlagCustomerModel = addtionalAttributeHolder.LstEscalatedFlagCustomerModel;
                    x.LstNatureoftheticketModel = addtionalAttributeHolder.LstNatureoftheticketModel;
                    x.LstOutageFlagModel = addtionalAttributeHolder.LstOutageFlagModel;
                    x.LstWarrantyIssueModel = addtionalAttributeHolder.LstWarrantyIssueModel;
                    x.LstAvoidableFlagModel = addtionalAttributeHolder.LstAvoidableFlagModel;
                    x.LstResidualDebtModel = addtionalAttributeHolder.LstResidualDebtModel;
                    x.LstMetSLA = addtionalAttributeHolder.LstMetSLA;
                }
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstAttributes;
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
            List<ConcatenateStrings> lstConcatStrings = new List<ConcatenateStrings>();
            try
            {
                AttributeCollections aC = new AttributeCollections();
                DataSet ds = new DataSet();
                ds.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[10];
                prms[0] = new SqlParameter("@IsAuditAvailable", 1);
                prms[1] = new SqlParameter("@UserID", insertAttributeSave.UserId);
                prms[2] = new SqlParameter("@ResidualDebtId", insertAttributeSave.ResidualDebtId);
                prms[3] = new SqlParameter("@AvoidableFlagId", insertAttributeSave.AvoidableFlagId);
                prms[4] = new SqlParameter("@IsAttributeUpdated", insertAttributeSave.IsAttributeUpdated);
                prms[5] = new SqlParameter("@TicketStatusID", insertAttributeSave.TicketStatusId);
                prms[6] = new SqlParameter("@Attribute", ListExtensions.ToDataTable<SaveAttributeModel>(insertAttributeSave.InsertAttributeList.ToList()));
                prms[6].SqlDbType = SqlDbType.Structured;
                prms[6].TypeName = "[AVL].[TVP_TicketAttributeDetails]";
                prms[7] = new SqlParameter("@IsTicketDescriptionUpdated",
                    insertAttributeSave.IsTicketDescriptionUpdated);
                prms[8] = new SqlParameter("@IsTicketSummaryUpdated", insertAttributeSave.IsTicketSummaryUpdated);
                prms[9] = new SqlParameter("@SupportTypeID", insertAttributeSave.SupportTypeId);
                DataSet dt = new DBHelper().GetDatasetFromSP("AVL.TK_SaveAttributeDetails", prms, ConnectionString);
                List<CustomerModel> objListCustomerModel = new List<CustomerModel>();
                objListCustomerModel = GetCustomer(insertAttributeSave.UserId);
                Translation translation = new Translation();
                var multilingualConfig = translation.GetProjectMultilinugalTranslateFields
                    (Convert.ToString(objListCustomerModel[0].CustomerId),
                    Convert.ToString(insertAttributeSave.InsertAttributeList[0].projectId));
                if (multilingualConfig.IsMultilingualEnable.Equals(1))
                {
                    translation.GetTickets(insertAttributeSave.InsertAttributeList[0].TicketID,
                        insertAttributeSave.SupportTypeId);
                    lstConcatStrings = translation.CheckIfProjectSubscriptionIsActive();
                }

                if (lstConcatStrings != null && lstConcatStrings.Count > 0 && lstConcatStrings.Any
                    (x => x.HasError == true))
                {
                    return "False";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Added successfully";
        }
        /// <summary>
        /// Collection of Attributes
        /// </summary>
        public class AttributeCollections : List<InsertAttributeModel>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                        new SqlMetaData("TicketID", SqlDbType.NVarChar, 50),
                        new SqlMetaData("Serviceid", SqlDbType.Int),
                        new SqlMetaData("projectId", SqlDbType.BigInt),
                        new SqlMetaData("Priority", SqlDbType.BigInt),
                        new SqlMetaData("Severity", SqlDbType.BigInt),
                        new SqlMetaData("Assignedto", SqlDbType.NVarChar, 100),
                        new SqlMetaData("ReleaseType", SqlDbType.BigInt),
                        new SqlMetaData("EstimatedWorkSize", SqlDbType.Decimal, 20, 2),
                        new SqlMetaData("Ticketcreatedate", SqlDbType.DateTime),

                        new SqlMetaData("ActualStartdateTime", SqlDbType.DateTime),
                        new SqlMetaData("ActualEnddateTime", SqlDbType.DateTime),
                        new SqlMetaData("ReopenDate", SqlDbType.DateTime),
                        new SqlMetaData("CloseDate", SqlDbType.DateTime),
                        new SqlMetaData("KEDBAvailableIndicator", SqlDbType.BigInt),
                        new SqlMetaData("KEDBUpdatedAdded", SqlDbType.BigInt),
                        new SqlMetaData("MetResponseSLA", SqlDbType.NVarChar, 100),
                        new SqlMetaData("MetResolution", SqlDbType.NVarChar, 100),
                        new SqlMetaData("TicketDescription", SqlDbType.NVarChar, SqlMetaData.Max),
                        new SqlMetaData("Application", SqlDbType.BigInt),
                        new SqlMetaData("KEDBPath", SqlDbType.NVarChar, 500),
                        new SqlMetaData("CompletedDateTime", SqlDbType.DateTime),
                        new SqlMetaData("ResolutionCode", SqlDbType.BigInt),
                        new SqlMetaData("DebtClassificationId", SqlDbType.BigInt),

                      new SqlMetaData("Resolutionmethod", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("CauseCode", SqlDbType.BigInt),
                      new SqlMetaData("TicketOpenDate", SqlDbType.DateTime),
                      new SqlMetaData("ActualEffort", SqlDbType.Decimal, 20, 2),
                      new SqlMetaData("Comments", SqlDbType.NVarChar, 500),
                      new SqlMetaData("PlannedEffort", SqlDbType.Decimal, 20, 2),
                      new SqlMetaData("PlannedEndDate", SqlDbType.DateTime),
                      new SqlMetaData("PlannedStartDate", SqlDbType.DateTime),
                      new SqlMetaData("RCAID", SqlDbType.NVarChar, 500),
                      new SqlMetaData("ReleaseDate", SqlDbType.DateTime),
                      new SqlMetaData("TicketSummary", SqlDbType.NVarChar, 500),
                      new SqlMetaData("AvoidableFlag", SqlDbType.Int),
                      new SqlMetaData("ResidualDebtId", SqlDbType.Int),
                      new SqlMetaData("TicketSource", SqlDbType.BigInt),
                      new SqlMetaData("FlexField1", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("FlexField2", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("FlexField3", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("FlexField4", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("IsPartiallyAutomated", SqlDbType.Int)
                      );

                foreach (InsertAttributeModel Type in this)
                {
                    sqlRow.SetString(0, Type.TicketId);
                    sqlRow.SetInt32(1, Type.ServiceId);
                    sqlRow.SetInt64(2, Type.ProjectId);
                    sqlRow.SetInt64(3, Type.Priority);
                    sqlRow.SetInt64(4, Type.Severity);
                    sqlRow.SetString(5, Type.AssignedTo != null ? Type.AssignedTo : "");
                    sqlRow.SetInt64(6, Type.ReleaseType);

                    sqlRow.SetDecimal(7, Convert.ToDecimal(Type.EstimatedWorkSize));
                    sqlRow.SetDateTime(8, Convert.ToDateTime(Type.TicketCreateDate));


                    sqlRow.SetDateTime(9, Convert.ToDateTime(Type.ActualStartDateTime));
                    sqlRow.SetDateTime(10, Convert.ToDateTime(Type.ActualEndDateTime));
                    sqlRow.SetDateTime(11, Convert.ToDateTime(Type.ReopenDate));

                    sqlRow.SetDateTime(12, Convert.ToDateTime(Type.CloseDate));

                    sqlRow.SetInt64(13, Type.KEDBAvailableIndicator);
                    sqlRow.SetInt64(14, Type.KEDBUpdated);
                    sqlRow.SetString(15, Type.MetResponseSLA != null ? Type.MetResponseSLA : "");

                    sqlRow.SetString(16, Type.MetResolution != null ? Type.MetResolution : "");
                    sqlRow.SetString(17, Type.TicketDescription != null ? Type.TicketDescription : "");


                    sqlRow.SetInt64(18, Type.Application);
                    sqlRow.SetString(19, Type.KEDBPath != null ? Type.KEDBPath : "");

                    sqlRow.SetDateTime(20, Convert.ToDateTime(Type.CompletedDateTime));
                    sqlRow.SetInt64(21, Type.ResolutionCode);
                    sqlRow.SetInt64(22, Type.DebtClassificationId);
                    sqlRow.SetString(23, Type.ResolutionMethod != null ? Type.ResolutionMethod : "");
                    sqlRow.SetInt64(24, Type.CauseCode);

                    sqlRow.SetDateTime(25, Convert.ToDateTime(Type.TicketOpenDate));
                    sqlRow.SetDecimal(26, Type.ActualEffort);
                    sqlRow.SetString(27, Type.Comments != null ? Type.Comments : "");
                    sqlRow.SetDecimal(28, Convert.ToDecimal(Type.PlannedEffort));
                    sqlRow.SetDateTime(29, Convert.ToDateTime(Type.PlannedEndDate));
                    sqlRow.SetDateTime(30, Convert.ToDateTime(Type.PlannedStartDateAndTime));

                    sqlRow.SetString(31, Type.RCAID != null ? Type.RCAID : "");

                    sqlRow.SetDateTime(32, Convert.ToDateTime(Type.ReleaseDate));
                    sqlRow.SetString(33, Type.TicketSummary != null ? Type.TicketSummary : "");
                    sqlRow.SetInt32(34, Type.AvoidalFlagId);
                    sqlRow.SetInt32(35, Type.ResidualDebtId);
                    sqlRow.SetInt64(36, Type.Source);
                    sqlRow.SetString(37, Type.FlexField1 != null ? Type.FlexField1 : "");
                    sqlRow.SetString(38, Type.FlexField2 != null ? Type.FlexField2 : "");
                    sqlRow.SetString(39, Type.FlexField3 != null ? Type.FlexField3 : "");
                    sqlRow.SetString(40, Type.FlexField4 != null ? Type.FlexField4 : "");
                    sqlRow.SetInt32(41, Type.IsPartiallyAutomated);
                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// This Method Is Used To AssignValuesToEntity
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="AC">AC</param>
        public void AssignValuesToEntity(List<InsertAttributeModel> list, AttributeCollections AC)
        {
            foreach (var objval in list)
            {
                AC.Add(new InsertAttributeModel
                {
                    TicketId = objval.TicketId,
                    ServiceId = objval.ServiceId,
                    ProjectId = objval.ProjectId,
                    Priority = objval.Priority,
                    Severity = objval.Severity,
                    AssignedTo = objval.AssignedTo == null ? string.Empty : objval.AssignedTo,
                    ReleaseType = objval.ReleaseType,
                    EstimatedWorkSize = objval.EstimatedWorkSize,
                    TicketCreateDate = objval.TicketCreateDate,
                    ActualStartDateTime = objval.ActualStartDateTime,
                    ActualEndDateTime = objval.ActualEndDateTime,
                    ReopenDate = objval.ReopenDate,
                    CloseDate = objval.CloseDate,
                    KEDBAvailableIndicator = objval.KEDBAvailableIndicator,
                    KEDBUpdated = objval.KEDBUpdated,
                    MetResponseSLA = objval.MetResponseSLA,
                    MetResolution = objval.MetResolution,
                    TicketDescription = objval.TicketDescription == null ? string.Empty : objval.TicketDescription,
                    Application = objval.Application,
                    KEDBPath = objval.KEDBPath == null ? string.Empty : objval.KEDBPath,
                    CompletedDateTime = objval.CompletedDateTime,
                    ResolutionCode = objval.ResolutionCode,
                    DebtClassificationId = objval.DebtClassificationId,
                    ResolutionMethod = objval.ResolutionMethod,
                    CauseCode = objval.CauseCode,
                    TicketOpenDate = objval.TicketOpenDate,
                    ActualEffort = objval.ActualEffort,
                    Comments = objval.Comments,
                    EscalatedFlagCustomer = objval.EscalatedFlagCustomer,
                    OnOff = objval.OnOff,
                    PlannedEffort = objval.PlannedEffort,
                    PlannedEndDate = objval.PlannedEndDate,
                    PlannedStartDateAndTime = objval.PlannedStartDateAndTime,
                    RCAID = objval.RCAID == null ? string.Empty : objval.RCAID,
                    ReleaseDate = objval.ReleaseDate,
                    TicketSummary = objval.TicketSummary,
                    AvoidalFlagId = objval.AvoidalFlagId,
                    ResidualDebtId = objval.ResidualDebtId,
                    Source = objval.Source,
                    FlexField1 = objval.FlexField1,
                    FlexField2 = objval.FlexField2,
                    FlexField3 = objval.FlexField3,
                    FlexField4 = objval.FlexField4,
                    IsPartiallyAutomated = objval.IsPartiallyAutomated
                });
            }
        }
        /// <summary>
        /// This Method Is Used To GetAddtionalAttributeDetails
        /// </summary>
        /// <returns>returns PopupAttributeModel</returns>
        public PopupAttributeModel GetAddtionalAttributeDetails()
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            PopupAttributeModel result = new PopupAttributeModel();
            SqlParameter[] prms = new SqlParameter[0];


            List<EscalatedFlagCustomerModel> lstEscalatedFlagCustomerModel = new List<EscalatedFlagCustomerModel>();
            List<NatureoftheticketModel> lstNatureoftheticketModel = new List<NatureoftheticketModel>();
            List<OutageFlagModel> lstOutageFlagModel = new List<OutageFlagModel>();
            List<AvoidableFlag> lstAvoidableFlagModel = new List<AvoidableFlag>();
            List<WarrantyIssueModel> lstWarrantyIssueModel = new List<WarrantyIssueModel>();
            List<ResidualDebt> lstResidualDebtModel = new List<ResidualDebt>();
            List<LstMetSLA> lstMetSLA = new List<LstMetSLA>();
            try
            {
                ds = (new DBHelper()).GetDatasetFromSP("[AVL].[GetAddtionalAttributeDetails]", prms, ConnectionString);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lstEscalatedFlagCustomerModel.Add(new EscalatedFlagCustomerModel
                            {
                                Id = Convert.ToInt32((ds.Tables[0].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[0].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[0].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[0].Rows[i]["Value"])
                            });
                        }
                        result.LstEscalatedFlagCustomerModel = lstEscalatedFlagCustomerModel;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lstNatureoftheticketModel.Add(new NatureoftheticketModel
                            {
                                Id = Convert.ToInt32((ds.Tables[1].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[1].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[1].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[1].Rows[i]["Value"])
                            });
                        }
                        result.LstNatureoftheticketModel = lstNatureoftheticketModel;
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            lstOutageFlagModel.Add(new OutageFlagModel
                            {
                                Id = Convert.ToInt32((ds.Tables[2].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[2].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[2].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[2].Rows[i]["Value"])
                            });
                        }
                        result.LstOutageFlagModel = lstOutageFlagModel;
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            lstWarrantyIssueModel.Add(new WarrantyIssueModel
                            {
                                Id = Convert.ToInt32((ds.Tables[3].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[3].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[3].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[3].Rows[i]["Value"])
                            });
                        }
                        result.LstWarrantyIssueModel = lstWarrantyIssueModel;
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            lstAvoidableFlagModel.Add(new AvoidableFlag
                            {
                                Id = Convert.ToInt32((ds.Tables[4].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[4].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[4].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[4].Rows[i]["Value"])
                            });
                        }
                        result.LstAvoidableFlagModel = lstAvoidableFlagModel;
                    }

                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                        {
                            lstResidualDebtModel.Add(new ResidualDebt
                            {
                                Id = Convert.ToInt32((ds.Tables[5].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[5].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[5].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[5].Rows[i]["Value"])
                            });
                        }
                        result.LstResidualDebtModel = lstResidualDebtModel;
                    }
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                        {
                            lstMetSLA.Add(new LstMetSLA
                            {
                                Id = Convert.ToInt32((ds.Tables[6].Rows[i]["Id"]) == DBNull.Value ? "0" :
                                ds.Tables[6].Rows[i]["Id"]),
                                Value = Convert.ToString((ds.Tables[6].Rows[i]["Value"]) == DBNull.Value ?
                                "Null" : ds.Tables[6].Rows[i]["Value"])
                            });
                        }
                        result.LstMetSLA = lstMetSLA;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// This Method Is Used To GetTicketInfoDetails
        /// </summary>
        /// <param name="ProjectId">ProjectId</param>
        /// <param name="TicketID">TicketID</param>
        /// <returns></returns>
        public string GetTicketInfoDetails(Int64 ProjectId, string TicketID, int supportType)
        {
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectId", ProjectId);
            prms[1] = new SqlParameter("@TicketID", TicketID);
            prms[2] = new SqlParameter("@supportTypeID", supportType);
            string isTicketIDPresent = "";
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("GetTicketInfoDetails", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    isTicketIDPresent = Convert.ToString(dt.Rows[0]["TicketID"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isTicketIDPresent;


        }
        /// <summary>
        /// This Method Is Used To GetRolePrivilageMenusForAppLens
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <param name="CustomerID">CustomerID</param>
        /// <returns></returns>
        public List<RolePrivilegeModel> GetRolePrivilageMenusForAppLens(string EmployeeID, Int64 CustomerID)
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            List<RolePrivilegeModel> objListRolePrivilegeModel = new List<RolePrivilegeModel>();
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[RolePrivilegeMenusForAppLens]", prms, ConnectionString);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            RolePrivilegeModel objRolePrivilegeModel = new RolePrivilegeModel();
                            objRolePrivilegeModel.PrivilegeId = Convert.ToInt32((ds.Tables[0].
                                Rows[i]["PrivilegeID"]) == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["PrivilegeID"]);
                            objRolePrivilegeModel.MenuName = (ds.Tables[0].Rows[i]["MenuName"]) ==
                                DBNull.Value ? "Null" : ds.Tables[0].Rows[i]["MenuName"].ToString();
                            objRolePrivilegeModel.MenuRole = (ds.Tables[0].Rows[i]["Role"]) ==
                                DBNull.Value ? "Null" : ds.Tables[0].Rows[i]["Role"].ToString();
                            objListRolePrivilegeModel.Add(objRolePrivilegeModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objListRolePrivilegeModel;
        }


        /// <summary>
        /// Getting values for copy
        /// </summary>
        /// <param name="CustomerID">CustomerID</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public List<CopyFields> GetDropDownValuesForCopy(long CustomerID, long ProjectID)
        {
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            List<CopyFields> application = new List<CopyFields>();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@CustomerID", CustomerID);
                prms[1] = new SqlParameter("@ProjectID", ProjectID);
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[GetHierarchyIL]", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    application = dtResult.Tables[0].AsEnumerable().Select(x => new CopyFields
                    {
                        ApplicationId = x["ApplicationID"] != DBNull.Value ? Convert.ToInt32(x["ApplicationID"]) : 0,
                        ApplicationName = x["ApplicationName"] != DBNull.Value ? Convert.
                        ToString(x["ApplicationName"]) : string.Empty,
                        LobId = x["LobID"] != DBNull.Value ? Convert.ToInt32(x["LobID"]) : 0,
                        LobName = x["LobName"] != DBNull.Value ? Convert.ToString(x["LobName"]) : string.Empty,
                        PortfolioId = x["PortfolioID"] != DBNull.Value ? Convert.ToInt32(x["PortfolioID"]) : 0,
                        PortfolioName = x["PortfolioName"] != DBNull.Value ? Convert.
                        ToString(x["PortfolioName"]) : string.Empty,
                        AppGroupId = x["AppgroupID"] != DBNull.Value ? Convert.ToInt32(x["AppgroupID"]) : 0,
                        AppGroupName = x["AppgroupName"] != DBNull.Value ? Convert.
                        ToString(x["AppgroupName"]) : string.Empty,

                    }).ToList();

                }

            }
            catch (Exception ex)
            {
            //    Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
              //      "Inside GetDropDownValuesApplication", Convert.ToInt32(CustomerID));
                throw ex;
            }
            return application;

        }
        /// <summary>
        /// This Method is used to GetHiddenFields
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFields(string EmployeeID)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);

            HiddenFieldsModel objHiddenFieldsModel = new HiddenFieldsModel();

            List<HiddenUserProjectID> lstProjectUserId = new List<HiddenUserProjectID>();
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_GetHiddenFields]", prms, ConnectionString);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            HiddenUserProjectID objHiddenUserProjectID = new HiddenUserProjectID();
                            objHiddenUserProjectID.UserId = Convert.ToInt32((ds.Tables[0].Rows[i]["UserID"])
                                == DBNull.Value ? "Null" : ds.Tables[0].Rows[i]["UserID"]);
                            objHiddenUserProjectID.ProjectName = (ds.Tables[0].Rows[i]["ProjectName"])
                                == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["ProjectName"].ToString();
                            objHiddenUserProjectID.ProjectId = Convert.ToInt32((ds.Tables[0].Rows[i]["ProjectID"])
                                == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["ProjectID"]);
                            objHiddenUserProjectID.UserTimeZone = (ds.Tables[0].Rows[i]["UserTimeZone"])
                                == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["UserTimeZone"].ToString();
                            objHiddenUserProjectID.CustomerTimeZone = (ds.Tables[0].Rows[i]["CustomerTimeZone"])
                                == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["CustomerTimeZone"].ToString();
                            lstProjectUserId.Add(objHiddenUserProjectID);
                        }
                        objHiddenFieldsModel.LstProjectUserID = lstProjectUserId;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            objHiddenFieldsModel.CustomerId = Convert.ToString((ds.Tables[1].Rows[i]["CustomerID"])
                                == DBNull.Value ? "" : ds.Tables[1].Rows[i]["CustomerID"]);
                            objHiddenFieldsModel.CustomerName = Convert.ToString((ds.Tables[1].Rows[i]["CustomerName"])
                                == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["CustomerName"]);
                            objHiddenFieldsModel.EmployeeId = Convert.ToString((ds.Tables[1].Rows[i]["EmployeeID"])
                                == DBNull.Value ? "" : ds.Tables[1].Rows[i]["EmployeeID"]);
                            objHiddenFieldsModel.EmployeeId = string.IsNullOrEmpty(objHiddenFieldsModel.EmployeeId) ?
                               string.Empty : objHiddenFieldsModel.EmployeeId.Trim();
                            objHiddenFieldsModel.IsEffortConfigured = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsEffortConfigured"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsEffortConfigured"]);
                            objHiddenFieldsModel.IsCognizant = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsCognizant"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsCognizant"]);
                            objHiddenFieldsModel.EmployeeName = Convert.ToString((ds.Tables[1].
                                Rows[i]["EmployeeName"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["EmployeeName"]);
                            objHiddenFieldsModel.EmployeeName = string.IsNullOrEmpty(objHiddenFieldsModel.EmployeeName) ?
                                string.Empty : objHiddenFieldsModel.EmployeeName.Trim();
                            objHiddenFieldsModel.IsDebtEngineEnabled = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsDebtEngineEnabled"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsDebtEngineEnabled"]);
                            objHiddenFieldsModel.IsDaily = Convert.ToInt32((ds.Tables[1].Rows[i]["IsDaily"])
                                == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["IsDaily"]);
                            objHiddenFieldsModel.RoleName = Convert.ToString((ds.Tables[1].Rows[i]["RoleName"]) ==
                                DBNull.Value ? "0" : ds.Tables[1].Rows[i]["RoleName"]);
                            objHiddenFieldsModel.LstProjectUserID = lstProjectUserId;
                        }
                    }
                    return objHiddenFieldsModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objHiddenFieldsModel;
        }
        /// <summary>
        /// This Method Is Used To GetHiddenFieldsForTM
        /// </summary>
        /// <param name="EmployeeID">EmployeeID</param>
        /// <param name="CustomerId">CustomerId</param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFieldsForTM(string EmployeeID, long CustomerId)
        {
            bool IsCG = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["IsCognizant"]);
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerId);
            prms[2] = new SqlParameter("@IsCG", IsCG);

            HiddenFieldsModel objHiddenFieldsModel = new HiddenFieldsModel();

            List<HiddenUserProjectID> lstProjectUserId = new List<HiddenUserProjectID>();
            List<HiddenScope> lstScope = new List<HiddenScope>();
            List<HiddenProjectPercentage> lstProjectPercentage = new List<HiddenProjectPercentage>();
            List<HcmSupervisorList> lsthcmSupervisors = new List<HcmSupervisorList>();
            try
            {

                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_GetHiddenFieldsForTM]", prms, ConnectionString);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            HiddenUserProjectID objHiddenUserProjectID = new HiddenUserProjectID();
                            objHiddenUserProjectID.UserId = Convert.ToInt32((ds.Tables[0].Rows[i]["UserID"]) ==
                                DBNull.Value ? "Null" : ds.Tables[0].Rows[i]["UserID"]);
                            objHiddenUserProjectID.ProjectName = (ds.Tables[0].Rows[i]["ProjectName"]) ==
                                DBNull.Value ? "0" : ds.Tables[0].Rows[i]["ProjectName"].ToString();
                            objHiddenUserProjectID.ProjectId = Convert.ToInt32((ds.Tables[0].Rows[i]["ProjectID"]) ==
                                DBNull.Value ? "0" : ds.Tables[0].Rows[i]["ProjectID"]);
                            objHiddenUserProjectID.UserTimeZone = (ds.Tables[0].Rows[i]["UserTimeZone"]) ==
                                DBNull.Value ? "0" : ds.Tables[0].Rows[i]["UserTimeZone"].ToString();
                            objHiddenUserProjectID.CustomerTimeZone = (ds.Tables[0].Rows[i]["CustomerTimeZone"]) ==
                                DBNull.Value ? "0" : ds.Tables[0].Rows[i]["CustomerTimeZone"].ToString();
                            objHiddenUserProjectID.IsApplensAsALM = Convert.ToBoolean((ds.Tables[0].Rows[i]["IsApplensAsALM"])
                             == DBNull.Value ? "false" : ds.Tables[0].Rows[i]["IsApplensAsALM"]);
                            lstProjectUserId.Add(objHiddenUserProjectID);
                            objHiddenUserProjectID.EsaProjectId = Convert.ToString((ds.Tables[0].Rows[i]["EsaProjectID"])
                                == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["EsaProjectID"]);
                            objHiddenUserProjectID.IsExempted = Convert.ToBoolean((ds.Tables[0].Rows[i]["IsExempted"])
                                == DBNull.Value ? "false" : ds.Tables[0].Rows[i]["IsExempted"]);

                    }
                        objHiddenFieldsModel.LstProjectUserID = lstProjectUserId;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            objHiddenFieldsModel.CustomerId = Convert.ToString((ds.Tables[1].Rows[i]["CustomerID"])
                                == DBNull.Value ? "" : ds.Tables[1].Rows[i]["CustomerID"]);
                            objHiddenFieldsModel.CustomerName = Convert.ToString((ds.Tables[1].Rows[i]["CustomerName"])
                                == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["CustomerName"]);
                            objHiddenFieldsModel.EmployeeId = Convert.ToString((ds.Tables[1].Rows[i]["EmployeeID"])
                                == DBNull.Value ? "" : ds.Tables[1].Rows[i]["EmployeeID"]);
                            objHiddenFieldsModel.IsEffortConfigured = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsEffortConfigured"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsEffortConfigured"]);
                            objHiddenFieldsModel.IsCognizant = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsCognizant"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsCognizant"]);
                            objHiddenFieldsModel.EmployeeName = Convert.ToString((ds.Tables[1].
                                Rows[i]["EmployeeName"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["EmployeeName"]);

                            objHiddenFieldsModel.IsDebtEngineEnabled = Convert.ToInt32((ds.Tables[1].
                                Rows[i]["IsDebtEngineEnabled"]) == DBNull.Value ? "0" : ds.Tables[1].
                                Rows[i]["IsDebtEngineEnabled"]);
                            objHiddenFieldsModel.IsDaily = Convert.ToInt32((ds.Tables[1].Rows[i]["IsDaily"])
                                == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["IsDaily"]);
                            objHiddenFieldsModel.RoleName = Convert.ToString((ds.Tables[1].Rows[i]["RoleName"])
                                == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["RoleName"]);
                            objHiddenFieldsModel.LstProjectUserID = lstProjectUserId;
                        }
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            HiddenScope objHiddenscope = new HiddenScope();
                            objHiddenscope.ProjectId = Convert.ToInt32((ds.Tables[2].Rows[i]["ProjectID"]) ==
                                DBNull.Value ? "0" : ds.Tables[2].Rows[i]["ProjectID"]);
                            objHiddenscope.Scope = Convert.ToInt32((ds.Tables[2].Rows[i]["Scope"]) ==
                                DBNull.Value ? "0" : ds.Tables[2].Rows[i]["Scope"]);
                            objHiddenscope.ScopeName = Convert.ToString((ds.Tables[2].Rows[i]["ScopeName"]) ==
                               DBNull.Value ? "0" : ds.Tables[2].Rows[i]["ScopeName"]);
                            lstScope.Add(objHiddenscope);
                        }
                        objHiddenFieldsModel.LstScope = lstScope;
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            HiddenProjectPercentage objHiddenprojectPercentage = new HiddenProjectPercentage();
                            objHiddenprojectPercentage.ProjectId = Convert.ToInt64((ds.Tables[3].Rows[i]["ProjectId"]) ==
                                DBNull.Value ? "0" : ds.Tables[3].Rows[i]["ProjectId"]);
                            objHiddenprojectPercentage.TileId = Convert.ToInt16((ds.Tables[3].Rows[i]["TileId"]) ==
                                DBNull.Value ? "0" : ds.Tables[3].Rows[i]["TileId"]);
                            objHiddenprojectPercentage.TileProgressPercentage = Convert.ToInt32((ds.Tables[3].Rows[i]["TileProgressPercentage"]) ==
                               DBNull.Value ? "0" : ds.Tables[3].Rows[i]["TileProgressPercentage"]);
                            lstProjectPercentage.Add(objHiddenprojectPercentage);
                        }
                        objHiddenFieldsModel.LstProjectPercentage = lstProjectPercentage;
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            HcmSupervisorList objhiddenhcmSupervisor = new HcmSupervisorList();
                            objhiddenhcmSupervisor.ProjectID = Convert.ToInt32((ds.Tables[4].Rows[i]["ProjectID"]) ==
                            DBNull.Value ? "0" : ds.Tables[4].Rows[i]["ProjectID"]);
                            objhiddenhcmSupervisor.CustomerID = Convert.ToInt32((ds.Tables[4].Rows[i]["CustomerID"]) ==
                          DBNull.Value ? "0" : ds.Tables[4].Rows[i]["CustomerID"]);
                            objhiddenhcmSupervisor.HcmSupervisorID = Convert.ToString((ds.Tables[4].Rows[i]["HcmSupervisorID"]) ==
                          DBNull.Value ? "0" : ds.Tables[4].Rows[i]["HcmSupervisorID"]);
                            lsthcmSupervisors.Add(objhiddenhcmSupervisor);
                        }
                        objHiddenFieldsModel.LstHCMSupervisorlist = lsthcmSupervisors;
                    }
                    return objHiddenFieldsModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objHiddenFieldsModel;
        }
        /// <summary>
        /// This Method Is Used To GetCustomer
        /// </summary>
        /// <param name="CogID">CogID</param>
        /// <returns></returns>
        public List<CustomerModel> GetCustomer(string CogID)
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            List<CustomerModel> objListCustomerModel = new List<CustomerModel>();
            try
            {
                if (CogID != null)
                {
                    SqlParameter[] prms = new SqlParameter[1];
                    prms[0] = new SqlParameter("@EmployeeID", CogID);
                    dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetCustomer]", prms, ConnectionString);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            CustomerModel objCustomerModel = new CustomerModel();
                            objCustomerModel.CustomerId = Convert.ToString((dt.Rows[i]["CustomerID"])
                                == DBNull.Value ? "0" : dt.Rows[i]["CustomerID"]);
                            objCustomerModel.CustomerName = Convert.ToString((dt.Rows[i]["CustomerName"])
                                == DBNull.Value ? "Null" : dt.Rows[i]["CustomerName"]);

                            objListCustomerModel.Add(objCustomerModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objListCustomerModel.OrderBy(cust => cust.CustomerName).ToList();
        }

        /// <summary>
        /// This Method Is Used To GetProjectCustomer
        /// </summary>
        /// <param name="CogID">CogID</param>
        /// <returns></returns>
        public List<long> GetProjectCustomer(string CogID, Int16 Mode)
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            List<long> lstProjCust = new List<long>();
            try
            {
                if (CogID != null)
                {
                    SqlParameter[] prms = new SqlParameter[2];
                    prms[0] = new SqlParameter("@EmployeeId", CogID);
                    prms[1] = new SqlParameter("@Mode", Mode);
                    dt = (new DBHelper()).GetTableFromSP("[AVL].[GetProjectCustomer]", prms, ConnectionString);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Mode == 1)
                        {
                            lstProjCust = (from row in dt.AsEnumerable() select Convert.ToInt64(row["CustomerId"])).ToList();
                        }
                        else
                        {
                            lstProjCust = (from row in dt.AsEnumerable() select Convert.ToInt64(row["ProjectId"])).ToList();
                        }
                        //lstProjCust = DataTableToList.ToDTList<long>(dt);
                        //lstProjCust = dt.AsEnumerable().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstProjCust;
        }
        /// <summary>
        /// This Method Is Used To GetSearchTicketsForET
        /// </summary>
        /// <param name="objChooseTicket">objChooseTicket</param>
        /// <returns></returns>
        public List<ChooseSearchTicketDetailsModel> GetSearchTicketsForET(ChooseTicket objChooseTicket)
        {
            List<ChooseSearchTicketDetailsModel> timesheetList = new List<ChooseSearchTicketDetailsModel>();
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[12];
                prms[0] = new SqlParameter("@ProjectID", objChooseTicket.ProjectId);
                prms[1] = new SqlParameter("@req", objChooseTicket.TicketIdDesc);
                prms[2] = new SqlParameter("@AssignedTo", objChooseTicket.AssignedTo);
                prms[3] = new SqlParameter("@applicationID", objChooseTicket.ApplicationId);
                prms[4] = new SqlParameter("@create_date_begin", string.IsNullOrEmpty(objChooseTicket.CreateDateBegin)
                    == true ? string.Empty : objChooseTicket.CreateDateBegin);
                prms[5] = new SqlParameter("@create_date_end", string.IsNullOrEmpty(objChooseTicket.CreateDateEnd)
                    == true ? string.Empty : objChooseTicket.CreateDateEnd);
                prms[6] = new SqlParameter("@close_date_begin", string.IsNullOrEmpty(objChooseTicket.CloseDateBegin)
                    == true ? string.Empty : objChooseTicket.CloseDateBegin);
                prms[7] = new SqlParameter("@close_date_end", string.IsNullOrEmpty(objChooseTicket.CloseDateEnd)
                    == true ? string.Empty : objChooseTicket.CloseDateEnd);
                prms[8] = new SqlParameter("@StatusIDs", objChooseTicket.StatusId);
                prms[9] = new SqlParameter("@IsDARTTicket", objChooseTicket.IsDARTTicket);
                prms[10] = new SqlParameter("@AssignmentGroupIds", objChooseTicket.AssignmentgroupIds);
                prms[11] = new SqlParameter("@TowerIDs", objChooseTicket.TowerId);
                dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetChooseTicketDetails]", prms, ConnectionString);

                string EncryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                AESEncryption aesMod = new AESEncryption();

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ChooseSearchTicketDetailsModel objTicketDetailsModel =
                            new ChooseSearchTicketDetailsModel();

                        objTicketDetailsModel.SupportTypeId = (dt.Rows[i]["SupportTypeID"]) == DBNull.Value ?
                            0 : Convert.ToInt16(dt.Rows[i]["SupportTypeID"]);
                        if (dt.Columns.Contains("SupportTypeName"))
                        {
                            objTicketDetailsModel.SupportTypeName = (dt.Rows[i]["SupportTypeName"]) == DBNull.Value ?
                                "" : Convert.ToString(dt.Rows[i]["SupportTypeName"]);
                        }

                        objTicketDetailsModel.TicketNumber = (dt.Rows[i]["req_no"]) == DBNull.Value ?
                            String.Empty : dt.Rows[i]["req_no"].ToString();

                        if (EncryptionEnabled == "Enabled")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TicketDescription"])))
                            {
                                string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Rows[i]
                                    ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                                string decTicketDesc = bytesDecrypted;
                                objTicketDetailsModel.TicketDescription = decTicketDesc;
                            }
                            else
                            {
                                objTicketDetailsModel.TicketDescription = Convert.
                                    ToString(dt.Rows[i]["TicketDescription"]);
                            }
                        }
                        else
                        {
                            objTicketDetailsModel.TicketDescription = Convert.
                                ToString(dt.Rows[i]["TicketDescription"]);
                        }
                        objTicketDetailsModel.AssigneeId = Convert.ToString(dt.Rows[i]["AssigneeID"]
                            == DBNull.Value ? "0" : dt.Rows[i]["AssigneeID"]);
                        objTicketDetailsModel.AssigneeName = Convert.ToString(dt.Rows[i]["AssigneeName"]
                            == DBNull.Value ? "0" : dt.Rows[i]["AssigneeName"]);
                        objTicketDetailsModel.AssignmentGroupId =
                            Convert.ToString(dt.Rows[i]["AssignmentGroupMapID"]
                            == DBNull.Value ? "0" : dt.Rows[i]["AssignmentGroupMapID"]);
                        objTicketDetailsModel.AssignmentGroupName =
                            Convert.ToString(dt.Rows[i]["AssignmentGroupName"]
                            == DBNull.Value ? "" : dt.Rows[i]["AssignmentGroupName"]);
                        objTicketDetailsModel.StatusId = Convert.ToInt32(dt.Rows[i]["StatusID"]
                            == DBNull.Value ? null : dt.Rows[i]["StatusID"]);
                        objTicketDetailsModel.DartStatusId = Convert.ToInt32(dt.Rows[i]["DartStatusID"]
                           == DBNull.Value ? null : dt.Rows[i]["DartStatusID"]);
                        objTicketDetailsModel.StatusName = Convert.ToString(dt.Rows[i]["StatusName"]
                            == DBNull.Value ? null : dt.Rows[i]["StatusName"]);
                        objTicketDetailsModel.ApplicationName = Convert.ToString(dt.Rows[i]["applicationName"]
                            == DBNull.Value ? null : dt.Rows[i]["applicationName"]);
                        objTicketDetailsModel.ApplicationId = Convert.ToString(dt.Rows[i]["ApplicationID"]
                            == DBNull.Value ? null : dt.Rows[i]["ApplicationID"]);
                        objTicketDetailsModel.TowerName = Convert.ToString(dt.Rows[i]["TowerName"]
                            == DBNull.Value ? null : dt.Rows[i]["TowerName"]);
                        objTicketDetailsModel.TowerId = Convert.ToInt16(dt.Rows[i]["TowerID"]
                            == DBNull.Value ? null : dt.Rows[i]["TowerID"]);
                        objTicketDetailsModel.IsDARTTicket = Convert.ToString(dt.Rows[i]["IsSDTicket"]
                            == DBNull.Value ? "0" : dt.Rows[i]["IsSDTicket"]);
                        objTicketDetailsModel.ServiceId = Convert.ToInt32(dt.Rows[i]["ServiceID"]
                            == DBNull.Value ? null : dt.Rows[i]["ServiceID"]);
                        objTicketDetailsModel.ServiceName = Convert.ToString(dt.Rows[i]["ServiceName"]
                            == DBNull.Value ? null : dt.Rows[i]["ServiceName"]);
                        objTicketDetailsModel.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryID"]
                            == DBNull.Value ? null : dt.Rows[i]["CategoryID"]);
                        objTicketDetailsModel.ActivityId = Convert.ToInt32(dt.Rows[i]["ActivityID"]
                            == DBNull.Value ? null : dt.Rows[i]["ActivityID"]);
                        objTicketDetailsModel.EffortTillDate = Convert.ToDecimal(dt.Rows[i]["EffortTillDate"]
                            == DBNull.Value ? "0" : (dt.Rows[i]["EffortTillDate"]));
                        objTicketDetailsModel.ITSMEffort = Convert.ToDecimal(dt.Rows[i]["ITSMEffort"]
                            == DBNull.Value ? "0" : (dt.Rows[i]["ITSMEffort"]));
                        objTicketDetailsModel.TicketCreateDate = dt.Rows[i]["TicketCreateDate"].ToString();
                        objTicketDetailsModel.IsAttributeUpdated =
                            Convert.ToBoolean(dt.Rows[i]["IsAttributeUpdated"]
                            == DBNull.Value ? "0" : (dt.Rows[i]["IsAttributeUpdated"]));
                        objTicketDetailsModel.TicketTypeId = Convert.ToInt32(dt.Rows[i]["TicketTypeID"]
                            == DBNull.Value ? null : dt.Rows[i]["TicketTypeID"]);
                        if (!string.IsNullOrEmpty(dt.Rows[i]["create_Date"].ToString()))
                        {
                            objTicketDetailsModel.OpenDateTime = (dt.Rows[i]["create_Date"].ToString());
                        }
                        else
                        {
                            objTicketDetailsModel.OpenDateTime = null;
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["resolved_date"].ToString()))
                        {
                            objTicketDetailsModel.Closeddate = (dt.Rows[i]["resolved_date"].ToString());
                        }
                        else
                        {
                            objTicketDetailsModel.Closeddate = null;
                        }
                        objTicketDetailsModel.ProjectId = Convert.ToInt32((dt.Rows[i]["ProjectID"])
                            == DBNull.Value ? "0" : dt.Rows[i]["ProjectID"]);
                        objTicketDetailsModel.IsMainSpringConfig = Convert.ToString(dt.
                            Rows[i]["IsMainSpringConfig"] == DBNull.Value ? null : dt.
                            Rows[i]["IsMainSpringConfig"]);
                        objTicketDetailsModel.IsDebtEnabled = Convert.ToString((dt.Rows[i]["IsDebtEnabled"])
                            == DBNull.Value ? "0" : dt.Rows[i]["IsDebtEnabled"]);
                        timesheetList.Add(objTicketDetailsModel);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return timesheetList;
        }
        /// <summary>
        /// Gets Choosed Work Item Details
        /// </summary>
        /// <param name="objChooseTicket"></param>
        /// <returns>List Work Item Details</returns>
        public ChooseWorkItem ChooseWorkItems(ChooseTicket objChooseTicket)
        {
            ChooseWorkItem objChooseWorkItem = new ChooseWorkItem();
            List<WorkItemDetails> WorkItemList = new List<WorkItemDetails>();
            try
            {
                DataSet dsWorkitemDetails = new DataSet();
                dsWorkitemDetails.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[9];
                prms[0] = new SqlParameter("@ProjectID", objChooseTicket.ProjectId);
                prms[1] = new SqlParameter("@WorkItemId", objChooseTicket.WorkItemId);
                prms[2] = new SqlParameter("@AssignedTo", objChooseTicket.AssigneeId);
                prms[3] = new SqlParameter("@ApplicationIDs", objChooseTicket.ApplicationId);
                prms[4] = new SqlParameter("@CreatedDateFrom", string.IsNullOrEmpty(objChooseTicket.CreatedDateFrom)
                    == true ? string.Empty : objChooseTicket.CreatedDateFrom);
                prms[5] = new SqlParameter("@CreatedDateTo", string.IsNullOrEmpty(objChooseTicket.CreatedDateTo)
                    == true ? string.Empty : objChooseTicket.CreatedDateTo);
                prms[6] = new SqlParameter("@StatusIDs", objChooseTicket.StatusId);
                prms[7] = new SqlParameter("@PageNo", Convert.ToInt32(objChooseTicket.PageNo));
                prms[8] = new SqlParameter("@PageSize", Convert.ToInt32(objChooseTicket.PageSize));
                dsWorkitemDetails = (new DBHelper()).GetDatasetFromSP("[AVL].[GetChooseWorkItemDetails]", prms, ConnectionString);

                if (dsWorkitemDetails != null)
                {
                    if (dsWorkitemDetails.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsWorkitemDetails.Tables[1].Rows.Count; i++)
                        {
                            WorkItemDetails objWorkItemDetails = new WorkItemDetails();
                            objWorkItemDetails.WorkItemId = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["WorkItemID"])
                                == DBNull.Value ? "0" : dsWorkitemDetails.Tables[1].Rows[i]["WorkItemID"]);
                            objWorkItemDetails.Description = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["Description"])
                                == DBNull.Value ? null : dsWorkitemDetails.Tables[1].Rows[i]["Description"]);
                            objWorkItemDetails.Application = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["Application"])
                                == DBNull.Value ? null : dsWorkitemDetails.Tables[1].Rows[i]["Application"]);
                            objWorkItemDetails.Assignee = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["Assignee"])
                                == DBNull.Value ? null : dsWorkitemDetails.Tables[1].Rows[i]["Assignee"]);
                            objWorkItemDetails.Status = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["Status"])
                                == DBNull.Value ? null : dsWorkitemDetails.Tables[1].Rows[i]["Status"]);
                            objWorkItemDetails.CreatedDate = Convert.ToString((dsWorkitemDetails.Tables[1].Rows[i]["CreatedDate"]));
                            WorkItemList.Add(objWorkItemDetails);
                        }
                        objChooseWorkItem.lstWorkItemDetails = WorkItemList;
                    }
                    if (dsWorkitemDetails.Tables[0].Rows.Count > 0)
                    {
                        objChooseWorkItem.PageSize = Convert.ToInt16(dsWorkitemDetails.Tables[0].Rows[0][0]);
                    }
                    //TotalRowCount
                    if (dsWorkitemDetails.Tables[2].Rows.Count > 0)
                    {
                        objChooseWorkItem.TotalRowCount = Convert.ToInt16(dsWorkitemDetails.Tables[2].Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objChooseWorkItem;
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
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                prms[1] = new SqlParameter("@CogID", employeeID);
                DataTable dtColumns = (new DBHelper()).GetTableFromSP("AVL.CheckIfMultilingualEnabled", prms, ConnectionString);
                if (dtColumns != null)
                {
                    if (dtColumns.Rows.Count > 0)
                    {
                        return Convert.ToBoolean(dtColumns.Rows[0][0]);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
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
            List<TicketDescriptionSummary> lstColumns = new List<TicketDescriptionSummary>();
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                prms[1] = new SqlParameter("@CogID", employeeID);
                prms[2] = new SqlParameter("@TicketID", ticketID);
                prms[3] = new SqlParameter("@SupportTypeID", SupportTypeID);
                DataTable dtSumDesc = (new DBHelper()).
                    GetTableFromSP("AVL.GetTicketSummaryDescriptionDetails_Multilingual_ByTicketID", prms, ConnectionString);
                if (dtSumDesc != null)
                {
                    if (dtSumDesc.Rows.Count > 0)
                    {
                        AESEncryption aesMod = new AESEncryption();
                        foreach (DataRow dr in dtSumDesc.Rows)
                        {
                            TicketDescriptionSummary tDesSum = new TicketDescriptionSummary();
                            tDesSum.TicketId = dr["TicketID"].ToString();
                            tDesSum.TicketSummary = Convert.ToString(string.IsNullOrEmpty
                                (dr["TicketSummary"].ToString()) ? string.Empty : aesMod.DecryptStringFromBytes
                                (Convert.FromBase64String((string)dr["TicketSummary"]),
                                AseKeyDetail.AesKeyConstVal));
                            tDesSum.TicketDescription = Convert.ToString(string.IsNullOrEmpty
                                (dr["TicketDescription"].ToString()) ? string.Empty :
                             aesMod.DecryptStringFromBytes(Convert.FromBase64String((string)dr["TicketDescription"]),
                             AseKeyDetail.AesKeyConstVal));
                            lstColumns.Add(tDesSum);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstColumns;
        }

        /// <summary>
        /// This Method Is Used To Get Ispartialautomated Details
        /// </summary>

        /// <returns></returns>
        public List<LstMetSLA> GetIspartialautomatedDetails()
        {
            List<LstMetSLA> lstispartial = new List<LstMetSLA>();
            SqlParameter[] prms = new SqlParameter[0];
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("AVL.[GetIsPartialDetails]", prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    lstispartial = dtResult.Tables[0].AsEnumerable().Select(row => new LstMetSLA
                    {
                        Id = Convert.ToInt32((row["Id"]) == DBNull.Value ? "0" :
                        row["Id"]),
                        Value = Convert.ToString((row["Value"]) == DBNull.Value ? string.Empty :
                        row["Value"])
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstispartial;

        }
        public bool IsTicketDescriptionOpted(int projectID)
        {
            Boolean result = false;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);

                DataTable dtColumns = (new DBHelper()).GetTableFromSP("dbo.TicketDescriptionOptedField", prms, ConnectionString);

                if (dtColumns != null)
                {
                    if (dtColumns.Rows.Count > 0)
                    {
                        result = Convert.ToBoolean(dtColumns.Rows[0][0]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return result;
        }
    }
}
