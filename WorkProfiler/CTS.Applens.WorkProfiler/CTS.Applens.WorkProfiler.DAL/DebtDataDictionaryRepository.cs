using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Common;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// This class holds DebtDataDictionaryRepository informations
    /// </summary>
    public class DebtDataDictionaryRepository : DBContext
    {
        private static string strresult;
        CacheManager _cacheManager = new CacheManager();
        public static string StrResult
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
            List<DebtOverrideReview> listDebtOverRideList = new List<DebtOverrideReview>();

            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];

            AESEncryption aesMod = new AESEncryption();
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@StartDate", StartDate);
                prms[1] = new SqlParameter("@EndDate", EndDate);
                prms[2] = new SqlParameter("@CustomerID", CustomerID);
                prms[3] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[4] = new SqlParameter("@ProjectID", ProjectID);
                prms[5] = new SqlParameter("@ReviewStatus", ReviewStatus);
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("[dbo].[GetDebtReview]", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {

                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (var i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            DebtOverrideReview objresult = new DebtOverrideReview();
                            objresult.ServiceName = dt.Tables[0].Rows[i]["ServiceName"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["ServiceName"].ToString();
                            objresult.TicketId = dt.Tables[0].Rows[i]["TicketID"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["TicketID"].ToString();
                            objresult.EmployeeId = dt.Tables[0].Rows[i]["Assignee"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["Assignee"].ToString();
                            objresult.AssignedTo = dt.Tables[0].Rows[i]["AssignedTo"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["AssignedTo"].ToString();
                            objresult.Application = dt.Tables[0].Rows[i]["Application"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["Application"].ToString();
                            objresult.DebtClassification = dt.Tables[0].Rows[i]["DebtClassification"] == DBNull.Value ?
                                "0" : dt.Tables[0].Rows[i]["DebtClassification"].ToString();
                            objresult.ResidualDebt = dt.Tables[0].Rows[i]["ResidualDebt"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["ResidualDebt"].ToString();
                            objresult.AvoidableFlag = Convert.ToInt64(dt.Tables[0].Rows[i]["AvoidableFlag"], CultureInfo.CurrentCulture);
                            objresult.AvoidableFlagName = dt.Tables[0].Rows[i]["AvoidableFlagName"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["AvoidableFlagName"].ToString();
                            objresult.ResolutionCode = dt.Tables[0].Rows[i]["ResolutionCode"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["ResolutionCode"].ToString();
                            objresult.CauseCode = dt.Tables[0].Rows[i]["CauseCode"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["CauseCode"].ToString();
                            objresult.DebtClassificationId = Convert.ToInt64(dt.Tables[0].
                                Rows[i]["DebtClassificationID"], CultureInfo.CurrentCulture);
                            objresult.CauseId = Convert.ToInt64(dt.Tables[0].Rows[i]["CauseID"], CultureInfo.CurrentCulture);
                            objresult.ResolutionId = Convert.ToInt64(dt.Tables[0].Rows[i]["ResolutionID"], CultureInfo.CurrentCulture);
                            objresult.DebtClassificationMapId = Convert.ToInt64(dt.Tables[0].
                                Rows[i]["DebtClassificationMapID"], CultureInfo.CurrentCulture);
                            objresult.CauseCodeMapId = Convert.ToInt64(dt.Tables[0].Rows[i]["CauseCodeMapID"], CultureInfo.CurrentCulture);
                            objresult.ResidualDebtMapId = Convert.ToInt64(dt.Tables[0].Rows[i]["ResidualDebtMapID"], CultureInfo.CurrentCulture);
                            objresult.ResolutionCodeMapId = Convert.ToInt64(dt.Tables[0].Rows[i]["ResolutionCodeMapID"], CultureInfo.CurrentCulture);
                            if (encryptionEnabled == "Enabled")
                            {

                                if (!string.IsNullOrEmpty(Convert.ToString(dt.Tables[0].Rows[i]["TicketDescription"], CultureInfo.CurrentCulture)))
                                {
                                    try
                                    {
                                        string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Tables[0].Rows[i]
                                            ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                                        string decTicketDesc = bytesDecrypted;
                                        objresult.TicketDescription = decTicketDesc;
                                    }
                                    catch (Exception)
                                    {
                                        objresult.TicketDescription = Convert.ToString(dt.Tables[0].
                                            Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                                    }
                                }
                                else
                                {
                                    objresult.TicketDescription = Convert.ToString(dt.Tables[0].
                                        Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                                }
                            }
                            else
                            {
                                objresult.TicketDescription = dt.Tables[0].Rows[i]["TicketDescription"] == DBNull.Value ?
                                    "0" : dt.Tables[0].Rows[i]["TicketDescription"].ToString();
                            }
                            objresult.CustomerId = Convert.ToInt64(dt.Tables[0].Rows[i]["CustomerID"], CultureInfo.CurrentCulture);
                            objresult.ProjectId = Convert.ToInt64(dt.Tables[0].Rows[i]["ProjectID"], CultureInfo.CurrentCulture);
                            objresult.IsCognizant = Convert.ToInt32(dt.Tables[0].Rows[i]["IsCognizant"], CultureInfo.CurrentCulture);
                            objresult.TicketType = dt.Tables[0].Rows[i]["TicketType"] == DBNull.Value ? "0" :
                                dt.Tables[0].Rows[i]["TicketType"].ToString();
                            objresult.IsApproved = dt.Tables[0].Rows[i]["IsApproved"] == DBNull.Value ? 0 :
                                Convert.ToInt16(dt.Tables[0].Rows[i]["IsApproved"], CultureInfo.CurrentCulture);
                            objresult.NatureOfTheTicketName = dt.Tables[0].Rows[i]["NatureoftheTicket"] == DBNull.Value ?
                                "0" : dt.Tables[0].Rows[i]["NatureoftheTicket"].ToString();
                            objresult.FlexField1ProjectWise = dt.Tables[0].Rows[i]["FlexField1ProjectWise"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["FlexField1ProjectWise"].ToString();
                            objresult.FlexField2ProjectWise = dt.Tables[0].Rows[i]["FlexField2ProjectWise"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["FlexField2ProjectWise"].ToString();
                            objresult.FlexField3ProjectWise = dt.Tables[0].Rows[i]["FlexField3ProjectWise"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["FlexField3ProjectWise"].ToString();
                            objresult.FlexField4ProjectWise = dt.Tables[0].Rows[i]["FlexField4ProjectWise"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["FlexField4ProjectWise"].ToString();

                            objresult.FlexField1 = dt.Tables[0].Rows[i]["FlexField1Value"] == DBNull.Value ?
                                string.Empty : dt.Tables[0].Rows[i]["FlexField1Value"].ToString();
                            objresult.FlexField2 = dt.Tables[0].Rows[i]["FlexField2Value"] == DBNull.Value ?
                                string.Empty : dt.Tables[0].Rows[i]["FlexField2Value"].ToString();
                            objresult.FlexField3 = dt.Tables[0].Rows[i]["FlexField3Value"] == DBNull.Value ?
                                string.Empty : dt.Tables[0].Rows[i]["FlexField3Value"].ToString();
                            objresult.FlexField4 = dt.Tables[0].Rows[i]["FlexField4Value"] == DBNull.Value ?
                                string.Empty : dt.Tables[0].Rows[i]["FlexField4Value"].ToString();

                            objresult.IsAHTagged = (dt.Tables[0].Rows[i]["IsAHTagged"] != DBNull.Value) ?
                                      Convert.ToBoolean(dt.Tables[0].Rows[i]["IsAHTagged"], CultureInfo.CurrentCulture) : false;

                            listDebtOverRideList.Add(objresult);
                        }

                    }
                    else
                    {
                        if (new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"] == "true")
                        {
                            if (ReviewStatus == 2)
                            {
                                List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                                existingAcitivities = new MyActivity().GetExistingActivitys(ProjectID, new AppSettings().AppsSttingsKeyValues["DebtReviewWorkItemCode"], access);
                                if (existingAcitivities != null && existingAcitivities.Count > 0)
                                {
                                    if (existingAcitivities[0].ActivityTo == EmployeeID && !existingAcitivities[0].IsExpired)
                                    {
                                        UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                                        expiryModel.WorkItemCode = new AppSettings().AppsSttingsKeyValues["DebtReviewWorkItemCode"];
                                        expiryModel.SourceRecordId = ProjectID;
                                        expiryModel.ModifiedBy = "System";
                                        string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return listDebtOverRideList;
        }
        /// <summary>
        /// This Method Is Used To GetDebtClassificationmodel
        /// </summary>
        /// <returns></returns>
        public List<DebtClassificationModelDebt> GetDebtClassificationmodel()
        {

            List<DebtClassificationModelDebt> debtClassification = new List<DebtClassificationModelDebt>();
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[DebtReview_GetDebtClassificationDetails]", ConnectionString).
                    Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            DebtClassificationModelDebt objresult = new DebtClassificationModelDebt();
                            objresult.DebtClassificationId = dt.Tables[0].Rows[i]["DebtClassificationID"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["DebtClassificationID"].ToString();
                            objresult.DebtClassificationName = dt.Tables[0].Rows[i]["DebtClassificationName"].
                                ToString();
                            debtClassification.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return debtClassification;
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
            {
                List<TicketRole> ticketRole = new List<TicketRole>();
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[1] = new SqlParameter("@CustomerID", CustomerID);
                prms[2] = new SqlParameter("@ProjectID", ProjectID);
                try
                {
                    DataTable dt = new DBHelper().GetTableFromSP("GetTicketRoles", prms, ConnectionString);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                TicketRole objresult = new TicketRole();
                                objresult.RoleId = dt.Rows[i]["RoleID"] == DBNull.Value ? "0" : dt.Rows[i]["RoleID"]
                                    .ToString();
                                objresult.RoleName = dt.Rows[i]["RoleName"].ToString();
                                ticketRole.Add(objresult);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return ticketRole;
            }
        }
        /// <summary>
        /// This Method Is Used To GetCauseCode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<CauseModelDebt> GetCauseCode(int ProjectID)
        {
            List<CauseModelDebt> causeCodeDebt = new List<CauseModelDebt>();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@projectId", ProjectID);
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[ReMap_GetCauseCodeDetails]", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            CauseModelDebt objresult = new CauseModelDebt();
                            objresult.CauseId = dt.Tables[0].Rows[i]["CauseID"] == DBNull.Value ? "0" : dt.Tables[0].
                                Rows[i]["CauseID"].ToString();
                            objresult.CauseCode = dt.Tables[0].Rows[i]["CauseCode"].ToString();
                            causeCodeDebt.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return causeCodeDebt;
        }
        /// <summary>
        /// This Method Is Used To GetReasonForResidualByprojectid
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ReasonForResidual> GetReasonForResidualByprojectid(int ProjectID)
        {

            List<ReasonForResidual> reasonForResidual = new List<ReasonForResidual>();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@projectID", ProjectID);
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[Datadictionary_GetReasonForResidualBYProjectId]",
                    prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            ReasonForResidual objresult = new ReasonForResidual();
                            objresult.ReasonResidualId = dt.Tables[0].Rows[i]["ReasonResidualID"] == DBNull.Value ?
                                "0" : dt.Tables[0].Rows[i]["ReasonResidualID"].ToString();
                            objresult.ReasonResidualName = dt.Tables[0].Rows[i]["ReasonResidualName"].ToString();
                            reasonForResidual.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reasonForResidual;
        }
        /// <summary>
        /// This Method Is Used To GetResolutionCode
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ResolutionModelDebt> GetResolutionCode(int ProjectID)
        {

            List<ResolutionModelDebt> resolutionCode = new List<ResolutionModelDebt>();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@projectId", ProjectID);
            try
            {
                DataTable dt = new DBHelper().GetTableFromSP("[AVL].[ReMap_GetResolutionCodeDetails]", prms, ConnectionString);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ResolutionModelDebt objresult = new ResolutionModelDebt();
                            objresult.ResolutionId = dt.Rows[i]["ResolutionID"] == DBNull.Value ? "0" :
                                dt.Rows[i]["ResolutionID"].ToString();
                            objresult.ResolutionCode = dt.Rows[i]["ResolutionCode"].ToString();
                            resolutionCode.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resolutionCode;
        }

        /// <summary>
        /// This Method Is Used To GetApplicationDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ApplicationModel> GetApplicationDetails(int ProjectID, string access)
        {

            List<ApplicationModel> applicationdetails = new List<ApplicationModel>();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@projectid", ProjectID);
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[Effort_GetApplicationDetails]", prms, ConnectionString).Copy());
                if (dt != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            ApplicationModel objresult = new ApplicationModel();
                            objresult.ApplicationId = Convert.ToInt32((dt.Tables[0].Rows[i]["ApplicationID"]) ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["ApplicationID"], CultureInfo.CurrentCulture);
                            objresult.ApplicationName = Convert.ToString((dt.Tables[0].Rows[i]["ApplicationName"]) ==
                                DBNull.Value ? "Null" : dt.Tables[0].Rows[i]["ApplicationName"], CultureInfo.CurrentCulture);
                            objresult.ConflictPatternsCount = dt.Tables[0].Rows[0]["ConflictPatternsCount"] != DBNull.Value ? Convert.ToInt32(dt.
                                                       Tables[0].Rows[0]["ConflictPatternsCount"], CultureInfo.CurrentCulture) : 0;
                            objresult.EsaProjectId = Convert.ToString((dt.Tables[0].Rows[i]["EsaProjectId"]) ==
                                DBNull.Value ? "Null" : dt.Tables[0].Rows[i]["EsaProjectId"], CultureInfo.CurrentCulture);
                            applicationdetails.Add(objresult);
                        }
                    }
                }
                if (new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"] == "true")
                {
                    if (applicationdetails.Count > 0)
                    {
                        if (applicationdetails[0].ConflictPatternsCount == 0)
                        {
                            List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                            existingAcitivities = new MyActivity().GetExistingActivitys(ProjectID, new AppSettings().AppsSttingsKeyValues["ConflictPatternWorkItemCode"], access);
                            if (existingAcitivities != null && existingAcitivities.Count > 0)
                            {
                                if (!existingAcitivities[0].IsExpired)
                                {
                                    UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                                    expiryModel.WorkItemCode = new AppSettings().AppsSttingsKeyValues["ConflictPatternWorkItemCode"];
                                    expiryModel.SourceRecordId = ProjectID;
                                    expiryModel.ModifiedBy = "System";
                                    string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return applicationdetails;
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
            List<ResidualDetail> residualModelDebt = new List<ResidualDetail>();
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@ApplicationID", ApplicationID);
                prms[2] = new SqlParameter("@RowID", RowID);

                DataTable dt = new DBHelper().GetTableFromSP("GetResidualDetail", prms, ConnectionString);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ResidualDetail objresult = new ResidualDetail();
                            objresult.ReasonForResudial = dt.Rows[i]["ReasonForResidual"] == DBNull.Value ? "0" :
                                dt.Rows[i]["ReasonForResidual"].ToString();
                            objresult.ExpectedDate = dt.Rows[i]["ExpectedCompletionDate"].ToString();
                            residualModelDebt.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return residualModelDebt;
        }
        /// <summary>
        /// This Method Is Used To ProjectDebtDetailsdate
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string ProjectDebtDetailsdate(int ProjectID)
        {
            string date = string.Empty;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("ProjectDebtDetailsdate", prms, ConnectionString);
                if (dt != null)
                {

                    if (dt.Rows.Count > 0)
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            date = dt.Rows[i]["IsDDAutoClassifiedDate"] != DBNull.Value ? Convert.ToString(dt.
                                Rows[i]["IsDDAutoClassifiedDate"], CultureInfo.CurrentCulture) : string.Empty;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return date;
        }
        /// <summary>
        /// This Method Is Used To GetGridData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ApplicationIds"></param>
        /// <returns></returns>
        public List<Griddata> GetGridData(int ProjectID, List<ApplicationIDs> ApplicationIds)
        {
            List<Griddata> gridDataList = new List<Griddata>();
            try
            {
                DataTable dtApps = new DataTable();
                dtApps.Locale = CultureInfo.InvariantCulture;
                DBHelper db = new DBHelper();
                dtApps = db.ToDataTable(ApplicationIds);
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@ApplicationIDs", dtApps);
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = "[AVL].[IDList]";
                DataTable dt = new DBHelper().GetTableFromSP("[AVL].[GetDataDictionaryPatternsByProjectID]", prms, ConnectionString);
                if (dt != null)
                {

                    if (dt.Rows.Count > 0)
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {

                            Griddata objresult = new Griddata();
                            objresult.Id = dt.Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ID"], CultureInfo.CurrentCulture) : 0;
                            objresult.ProjectId = dt.Rows[i]["ProjectID"] != DBNull.Value ? Convert.ToInt32(dt.
                                Rows[i]["ProjectID"], CultureInfo.CurrentCulture) : 0;
                            objresult.ProjectName = dt.Rows[i]["ProjectName"] != DBNull.Value ? Convert.ToString(dt.
                                Rows[i]["ProjectName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ApplicationId = dt.Rows[i]["ApplicationID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["ApplicationID"], CultureInfo.CurrentCulture) : 0;
                            objresult.ApplicationName = dt.Rows[i]["ApplicationName"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["ApplicationName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ResolutionId = dt.Rows[i]["ResolutionCodeID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["ResolutionCodeID"], CultureInfo.CurrentCulture) : 0;
                            objresult.ResolutionName = dt.Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["ResolutionCode"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.DebtId = dt.Rows[i]["DebtClassificationID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["DebtClassificationID"], CultureInfo.CurrentCulture) : 0;
                            objresult.DebtName = dt.Rows[i]["DebtClassificationName"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["DebtClassificationName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.CauseId = dt.Rows[i]["CauseCodeID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["CauseCodeID"], CultureInfo.CurrentCulture) : 0;
                            objresult.CauseName = dt.Rows[i]["CauseCode"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["CauseCode"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.AvoidableFlagId = dt.Rows[i]["AvoidableFlagID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["AvoidableFlagID"], CultureInfo.CurrentCulture) : 0;
                            objresult.AvoidableFlagName = dt.Rows[i]["AvoidableFlagName"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["AvoidableFlagName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ResidualDebtId = dt.Rows[i]["ResidualDebtID"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["ResidualDebtID"], CultureInfo.CurrentCulture) : 0;
                            objresult.ResidualDebtName = dt.Rows[i]["ResidualDebtName"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["ResidualDebtName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ResidualId = dt.Rows[i]["ReasonForResidual"] != DBNull.Value ? Convert.
                                ToInt32(dt.Rows[i]["ReasonForResidual"], CultureInfo.CurrentCulture) : 0;
                            objresult.ResidualName = dt.Rows[i]["ReasonResidualName"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["ReasonResidualName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ExpectedCompletiondate = dt.Rows[i]["ExpectedCompletionDate"] != DBNull.Value ?
                                Convert.ToString(dt.Rows[i]["ExpectedCompletionDate"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.CreatedBy = dt.Rows[i]["CreatedBy"] != DBNull.Value ?
                               Convert.ToString(dt.Rows[i]["CreatedBy"], CultureInfo.CurrentCulture) : string.Empty;

                            gridDataList.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return gridDataList;
        }
        /// <summary>
        /// This Method Is Used To GetResidualDebt
        /// </summary>
        /// <returns></returns>
        public List<ResidualModelDebt> GetResidualDebt()
        {

            List<ResidualModelDebt> residualModelDebt = new List<ResidualModelDebt>();
            try
            {
                DataTable dt = new DBHelper().GetTableFromSP("[AVL].[DebtReview_GetResidualDebtDetails]", ConnectionString);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ResidualModelDebt objresult = new ResidualModelDebt();
                            objresult.ResidualDebtId = dt.Rows[i]["ResidualDebtID"] == DBNull.Value ? "0" :
                                dt.Rows[i]["ResidualDebtID"].ToString();
                            objresult.ResidualDebtName = dt.Rows[i]["ResidualDebtName"].ToString();
                            residualModelDebt.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return residualModelDebt;
        }
        /// <summary>
        /// This Method Is Used To GetAvoidableFlag
        /// </summary>
        /// <returns></returns>
        public List<AvoidableModelFlag> GetAvoidableFlag()
        {

            List<AvoidableModelFlag> avoidableModelFlag = new List<AvoidableModelFlag>();
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[DebtReview_GetAvoidableFlagDetails]", ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            AvoidableModelFlag objresult = new AvoidableModelFlag();
                            objresult.AvoidableFlagId = dt.Tables[0].Rows[i]["AvoidableFlagID"] == DBNull.Value ?
                                "0" : dt.Tables[0].Rows[i]["AvoidableFlagID"].ToString();
                            objresult.AvoidableFlagName = dt.Tables[0].Rows[i]["AvoidableFlagName"].ToString();
                            avoidableModelFlag.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return avoidableModelFlag;
        }
        /// <summary>
        /// This Method Is Used To GetNatureOfTicket
        /// </summary>
        /// <returns></returns>
        public List<NatureOfTicket> GetNatureOfTicket()
        {

            List<NatureOfTicket> natureofticketModelFlag = new List<NatureOfTicket>();
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add(new DBHelper().GetTableFromSP("[AVL].[DebtReview_GetNatureOfTicket]", ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                        {
                            NatureOfTicket objresult = new NatureOfTicket();
                            objresult.NatureOfTheTicketId = dt.Tables[0].Rows[i]["NatureOfTheTicketId"] ==
                                DBNull.Value ? "0" : dt.Tables[0].Rows[i]["NatureOfTheTicketId"].ToString();
                            objresult.NatureOfTheTicketName = dt.Tables[0].Rows[i]["NatureOfTheTicketName"].
                                ToString();
                            natureofticketModelFlag.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return natureofticketModelFlag;
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
            int result = 0;
            SqlParameter[] prms = new SqlParameter[2];

            try
            {
                var objCollection = from o in dataDictionaryDetails
                                    select new
                                    {
                                        ID = o.Id,
                                        ProjectID = o.ProjectId,
                                        ApplicationID = o.ApplicationId,
                                        CauseCodeID = o.CauseCodeId,
                                        ResolutionCodeID = o.ResolutionCodeId
                                    };

                prms[0] = new SqlParameter("@DataDetails_delete", objCollection.ToList().ToDT());
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "[dbo].[TVP_ProjectDataDictionary_Delete]";
                prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP("DeleteDataDictionaryByID", prms, ConnectionString);
                if (ds != null)
                {
                    //CCAP FIX
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// AddReasonforResidual
        /// </summary>
        /// <param name="objAddReason"></param>
        /// <returns></returns>
        public int AddReasonforResidual(AddReasonforResidual objAddReason)
        {
            int result = 0;
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ResidualText", objAddReason.ResidualText);
            prms[1] = new SqlParameter("@EmployeeID", objAddReason.EmployeeId);
            prms[2] = new SqlParameter("@projectID", objAddReason.ProjectId);
            try
            {
                (new DBHelper()).ExecuteNonQueryReturn("dbo.[DataDictionary_AddReasonforResidual]", prms, ConnectionString);
                result = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
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
            DataDictionaryData objDataDictionaryData = new DataDictionaryData();
            List<PortfolioData> lstPortfolioData = new List<PortfolioData>();
            List<ApplicationDataPortfolio> lstApplicationData = new List<ApplicationDataPortfolio>();
            try
            {
                DataSet ds = new DataSet();
                ds.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                prms[1] = new SqlParameter("@CustomerID", customerID);
                prms[2] = new SqlParameter("@ProjectID", projectID);
                ds = (new DBHelper()).GetDatasetFromSP("[AVL].[GetDDSearchFilterValues]", prms, ConnectionString);

                DataTable dtPortfolio = new DataTable();
                dtPortfolio.Locale = CultureInfo.InvariantCulture;
                DataTable dtApplication = new DataTable();
                dtApplication.Locale = CultureInfo.InvariantCulture;
                dtPortfolio = ds.Tables[0];
                dtApplication = ds.Tables[1];
                objDataDictionaryData.LstPortfolioData = dtPortfolio.AsEnumerable().Select(x => new PortfolioData
                {
                    PortfolioId = x["BusinessClusterMapID"] != DBNull.Value ? Convert.
                    ToInt32(x["BusinessClusterMapID"], CultureInfo.CurrentCulture) : 0,
                    PortfolioName = x["BusinessClusterBaseName"] != DBNull.Value ? x["BusinessClusterBaseName"].
                    ToString() : string.Empty
                }).ToList();
                objDataDictionaryData.LstApplicationData = dtApplication.AsEnumerable().Select(x =>
                new ApplicationDataPortfolio
                {
                    ApplicationId = x["ApplicationID"] != DBNull.Value ? Convert.ToInt32(x["ApplicationID"], CultureInfo.CurrentCulture) : 0,
                    PortfolioId = x["SubBusinessClusterMapID"] != DBNull.Value ? Convert.
                    ToInt32(x["SubBusinessClusterMapID"], CultureInfo.CurrentCulture) : 0,
                    ApplicationName = x["ApplicationName"] != DBNull.Value ? x["ApplicationName"].
                    ToString() : string.Empty
                }).ToList();
                if (mode == "GetApplications" && portfolioIds.Count > 0)
                {
                    objDataDictionaryData.LstApplicationData = (from lstApplication in objDataDictionaryData.
                                                                LstApplicationData
                                                                join lstPortfolio in portfolioIds on lstApplication.
                                                                PortfolioId equals lstPortfolio.PortFolioId
                                                                select lstApplication).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDataDictionaryData;
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
            SqlParameter[] prms = new SqlParameter[8];
            string result = "";
            prms[0] = new SqlParameter("@DebtClassificationID", debtClassficationID);
            prms[1] = new SqlParameter("@TicketID", ticketID);
            prms[2] = new SqlParameter("@ResolutionID", debtResolutionID);
            prms[3] = new SqlParameter("@CauseID", causeCodeID);
            prms[4] = new SqlParameter("@ResidualDebtID", resiDebt);
            prms[5] = new SqlParameter("@AvoidableFlag", avdFlag);
            prms[6] = new SqlParameter("@ReasonResiID", reasonResiID);
            prms[7] = new SqlParameter("@ExCompDate", exComDate);


            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("UpdateDebtReview", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    result = Convert.ToString(dt.Tables[0].Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// This Method Is Used To AddApplicationDetails
        /// </summary>
        /// <param name="objAddApplicationDetails"></param>
        /// <returns></returns>
        public string AddApplicationDetails(AddApplicationDetails objAddApplicationDetails)
        {

            SqlParameter[] prms = new SqlParameter[12];
            string result = "";
            prms[0] = new SqlParameter("@customerID", objAddApplicationDetails.CustomerId);
            prms[1] = new SqlParameter("@applicationID", objAddApplicationDetails.ApplicationId);
            prms[2] = new SqlParameter("@reasonForResidualID", objAddApplicationDetails.ReasonForResidualId);
            prms[3] = new SqlParameter("@avoidableFlagID", objAddApplicationDetails.AvoidableFlagId);
            prms[4] = new SqlParameter("@debtClassificationID", objAddApplicationDetails.DebtClassificationId);
            prms[5] = new SqlParameter("@projectID", objAddApplicationDetails.ProjectId);
            prms[6] = new SqlParameter("@residualDebtID", objAddApplicationDetails.ResidualDebtId);
            prms[7] = new SqlParameter("@causeCodeID", objAddApplicationDetails.CauseCodeId);
            prms[8] = new SqlParameter("@resolutionCodeID", objAddApplicationDetails.ResolutionCodeId);
            prms[9] = new SqlParameter("@employeeID", objAddApplicationDetails.EmployeeId);
            prms[10] = new SqlParameter("@expectedCompletionDate", !string.IsNullOrEmpty(objAddApplicationDetails.
                ExpectedCompletionDate) ? (objAddApplicationDetails.ExpectedCompletionDate) : String.Empty);
            prms[11] = new SqlParameter("@EffectiveDate", !string.IsNullOrEmpty(objAddApplicationDetails.
                EffectiveDate) ? Convert.ToDateTime(objAddApplicationDetails.EffectiveDate, CultureInfo.CurrentCulture) : Convert.
                ToDateTime("1/1/1900", CultureInfo.CurrentCulture));

            try
            {

                DataTable dt = new DBHelper().GetTableFromSP("AVL.DD_AddDataDictionary", prms, ConnectionString);
                if (dt != null)
                {
                    result = Convert.ToString(dt.Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// This Method Is Used To AddReasonResidualAndCompDate
        /// </summary>
        /// <param name="objAddReasonResidualAndCompDate"></param>
        /// <returns></returns>
        public string AddReasonResidualAndCompDate(AddReasonResidualAndCompDate objAddReasonResidualAndCompDate)
        {
            SqlParameter[] prms = new SqlParameter[11];
            string result = "";
            prms[0] = new SqlParameter("@empIds", objAddReasonResidualAndCompDate.EmpId);
            prms[1] = new SqlParameter("@applicationIds", objAddReasonResidualAndCompDate.ApplicationId);
            prms[2] = new SqlParameter("@causeIds", objAddReasonResidualAndCompDate.CauseId);
            prms[3] = new SqlParameter("@resolutionIds", objAddReasonResidualAndCompDate.ResolutionId);
            prms[4] = new SqlParameter("@debtclassiIds", objAddReasonResidualAndCompDate.DebtclassiId);
            prms[5] = new SqlParameter("@avoidIds", objAddReasonResidualAndCompDate.AvoidId);
            prms[6] = new SqlParameter("@resiIds", objAddReasonResidualAndCompDate.ResiId);
            prms[7] = new SqlParameter("@Projectid", objAddReasonResidualAndCompDate.ProjectId);
            prms[8] = new SqlParameter("@employeeID", objAddReasonResidualAndCompDate.EmployeeId);
            prms[9] = new SqlParameter("@reasonResiValueId", objAddReasonResidualAndCompDate.ReasonResiValueId);
            prms[10] = new SqlParameter("@compDateValue", !string.IsNullOrEmpty(objAddReasonResidualAndCompDate.
                CompDateValue) ? objAddReasonResidualAndCompDate.CompDateValue : String.Empty);
            try
            {

                DataTable dt = new DBHelper().GetTableFromSP("[AVL].[DD_AddReasonResidualAndCompDate]", prms, ConnectionString);
                if (dt != null)
                {
                    result = Convert.ToString(dt.Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// This Method Is Used To SaveDataDictionarybyProject
        /// </summary>
        /// <param name="DataDetails"></param>
        /// <returns></returns>
        public bool SaveDataDictionarybyProject(List<ProjectDataDictionary> DataDetails)
        {
            SqlParameter[] prms = new SqlParameter[1];
            bool result = false;
            try
            {
                var objCollection = from o in DataDetails
                                    select new
                                    {
                                        ID = o.Id,
                                        ProjectID = o.ProjectId,
                                        ApplicationID = o.ApplicationId,
                                        CauseCodeID = o.CauseCodeId,
                                        ResolutionCodeID = o.ResolutionCodeId,
                                        DebtClassificationID = o.DebtClassificationId,
                                        AvoidableFlagID = o.AvoidableFlagId,
                                        ResidualDebtID = o.ResidualDebtId,
                                        ReasonForResidual = o.ReasonForResidual,
                                        ExpectedCompletionDate = o.ExpectedCompletionDate,
                                        CreatedBy = o.CreatedBy,
                                        ModifiedBy = o.ModifiedBy
                                    };

                prms[0] = new SqlParameter("@DataDetails", objCollection.ToList().ToDT());
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "[dbo].[TVP_ProjectDataDictionary]";
                DataSet ds = (new DBHelper()).GetDatasetFromSP("Debt_SaveDatadictionaryDetails", prms, ConnectionString);
                if (ds != null)
                {
                    result = Convert.ToBoolean(ds.Tables[0].Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// This Method Is Used To SaveDataDictionaryByID
        /// </summary>
        /// <param name="dataDictionaryDetails"></param>
        /// <returns></returns>
        public bool SaveDataDictionaryByID(List<ProjectDataDictionary> dataDictionaryDetails)
        {
            SqlParameter[] prms = new SqlParameter[1];
            bool result = false;
            try
            {
                var objCollection = from o in dataDictionaryDetails
                                    select new
                                    {
                                        ID = o.Id,
                                        ProjectID = o.ProjectId,
                                        ApplicationID = o.ApplicationId,
                                        CauseCodeID = o.CauseCodeId,
                                        ResolutionCodeID = o.ResolutionCodeId,
                                        DebtClassificationID = o.DebtClassificationId,
                                        AvoidableFlagID = o.AvoidableFlagId,
                                        ResidualDebtID = o.ResidualDebtId,
                                        ReasonForResidual = o.ReasonForResidual,
                                        ExpectedCompletionDate = o.ExpectedCompletionDate,
                                        CreatedBy = o.CreatedBy,
                                        ModifiedBy = o.ModifiedBy
                                    };
                prms[0] = new SqlParameter("@DataDetails", objCollection.ToList().ToDT());
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "[dbo].[TVP_ProjectDataDictionary]";
                DataSet ds = (new DBHelper()).GetDatasetFromSP("Debt_SaveDatadictionaryDetails", prms, ConnectionString);
                if (ds != null)
                {
                    result = Convert.ToBoolean(ds.Tables[0].Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// DebtDataDictionaryCollection
        /// </summary>
        public class DebtDataDictionaryCollection : List<ProjectDataDictionary>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("ID", SqlDbType.BigInt),
                      new SqlMetaData("ProjectID", SqlDbType.BigInt),
                      new SqlMetaData("ApplicationID", SqlDbType.BigInt),
                      new SqlMetaData("CauseCodeID", SqlDbType.BigInt),
                      new SqlMetaData("ResolutionCodeID", SqlDbType.BigInt),
                      new SqlMetaData("DebtClassificationID", SqlDbType.BigInt),
                      new SqlMetaData("AvoidableFlagID", SqlDbType.BigInt),
                      new SqlMetaData("ResidualDebtID", SqlDbType.BigInt),
                      new SqlMetaData("ReasonForResidual", SqlDbType.BigInt),
                      new SqlMetaData("ExpectedCompletionDate", SqlDbType.VarChar, 100),
                      new SqlMetaData("CreatedBy", SqlDbType.VarChar,100),
                      new SqlMetaData("ModifiedBy", SqlDbType.VarChar,100)
                      );

                foreach (ProjectDataDictionary obj in this)
                {

                    sqlRow.SetInt64(0, obj.Id);
                    sqlRow.SetInt64(1, obj.ProjectId);
                    sqlRow.SetInt64(2, obj.ApplicationId);
                    sqlRow.SetInt64(3, obj.CauseCodeId);
                    sqlRow.SetInt64(4, obj.ResolutionCodeId);
                    sqlRow.SetInt64(5, obj.DebtClassificationId);
                    sqlRow.SetInt64(6, obj.AvoidableFlagId);
                    sqlRow.SetInt64(7, obj.ResidualDebtId);
                    sqlRow.SetInt64(8, obj.ReasonForResidual);
                    sqlRow.SetString(9, Convert.ToString(obj.ExpectedCompletionDate, CultureInfo.CurrentCulture));
                    sqlRow.SetString(10, obj.CreatedBy);
                    sqlRow.SetString(11, obj.ModifiedBy);


                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// DebtDataDictionaryCollectionDelete
        /// </summary>
        public class DebtDataDictionaryCollectionDelete : List<ProjectDataDictionaryDelete>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                    new SqlMetaData("ID", SqlDbType.BigInt),
                      new SqlMetaData("ProjectID", SqlDbType.BigInt),
                      new SqlMetaData("ApplicationID", SqlDbType.BigInt),
                      new SqlMetaData("CauseCodeID", SqlDbType.BigInt),
                      new SqlMetaData("ResolutionCodeID", SqlDbType.BigInt)
                      );

                foreach (ProjectDataDictionaryDelete obj in this)
                {
                    sqlRow.SetInt64(0, obj.Id);
                    sqlRow.SetInt64(1, obj.ProjectId);
                    sqlRow.SetInt64(2, obj.ApplicationId);
                    sqlRow.SetInt64(3, obj.CauseCodeId);
                    sqlRow.SetInt64(4, obj.ResolutionCodeId);

                    yield return sqlRow;
                }
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
            SqlParameter[] prms = new SqlParameter[3];
            TicketUploadRepository uploadRepository = new TicketUploadRepository();
            List<TicketDescriptionSummary> ticketDescriptionSummary = new List<TicketDescriptionSummary>();
            List<TicketSupportTypeMapping> ticketSupportMapping = new List<TicketSupportTypeMapping>();
            bool result = false;
            try
            {
                bool isMultilingualEnabled = uploadRepository.CheckIfMultilingualEnabled(Convert.ToString(ProjectID, CultureInfo.CurrentCulture),
                    EmployeeID);
                if (isMultilingualEnabled)
                {
                    ticketSupportMapping = TicketDetails.Select(r => new TicketSupportTypeMapping
                    { TicketId = r.TicketId, SupportType = 1 }).ToList();
                    ticketDescriptionSummary = uploadRepository.GetTicketValues(ticketSupportMapping,
                        Convert.ToString(ProjectID, CultureInfo.CurrentCulture), EmployeeID);
                    TicketDetails = IsCheckFlexFieldsModified(TicketDetails.ToDataTable(),
                        ticketDescriptionSummary).ToListof<DebtOverrideReview>();
                }

                var objCollection = from o in TicketDetails
                                    select new DebtOverrideReviewApprove
                                    {
                                        TicketID = o.TicketId,
                                        DebtClassificationMapID = Convert.ToInt32(o.DebtClassificationMapId),
                                        ResolutionCodeMapID = Convert.ToInt32(o.ResolutionCodeMapId),
                                        CauseCodeMapID = Convert.ToInt32(o.CauseCodeMapId),
                                        ResidualDebtMapID = Convert.ToInt32(o.ResidualDebtMapId),
                                        AvoidableFlag = Convert.ToInt32(o.AvoidableFlag),
                                        AssignedTo = o.AssignedTo,
                                        FlexField1 = o.FlexField1,
                                        FlexField2 = o.FlexField2,
                                        FlexField3 = o.FlexField3,
                                        FlexField4 = o.FlexField4,
                                        EmployeeID = o.EmployeeId,
                                        IsFlexField1Modified = !string.IsNullOrEmpty(o.IsFlexField1Modified) ? o.IsFlexField1Modified : string.Empty,
                                        IsFlexField2Modified = !string.IsNullOrEmpty(o.IsFlexField2Modified) ? o.IsFlexField2Modified : string.Empty,
                                        IsFlexField3Modified = !string.IsNullOrEmpty(o.IsFlexField3Modified) ? o.IsFlexField3Modified : string.Empty,
                                        IsFlexField4Modified = !string.IsNullOrEmpty(o.IsFlexField4Modified) ? o.IsFlexField4Modified : string.Empty
                                    };

                prms[0] = new SqlParameter("@ticketDetails", objCollection.ToList().ToDT());
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "UpdateApproveTicketList";
                prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[2] = new SqlParameter("@ProjectID", ProjectID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP("UpdateApproveTicketsByTicketID", prms, ConnectionString);
                if (ds != null)
                {
                    result = Convert.ToBoolean(ds.Tables[0].Rows[0]["Result"], CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// DebtApproveTicketsCollection
        /// </summary>
        public class DebtApproveTicketsCollection : List<DebtOverrideReview>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("TicketID", SqlDbType.VarChar, 100),
                      new SqlMetaData("DebtClassificationMapID", SqlDbType.BigInt),
                      new SqlMetaData("ResolutionCodeMapID", SqlDbType.BigInt),
                      new SqlMetaData("CauseCodeMapID", SqlDbType.BigInt),
                      new SqlMetaData("ResidualDebtMapID", SqlDbType.BigInt),
                      new SqlMetaData("AvoidableFlag", SqlDbType.BigInt),
                      new SqlMetaData("AssignedTo", SqlDbType.VarChar, 100),
                      new SqlMetaData("FlexField1", SqlDbType.NVarChar, 500),
                      new SqlMetaData("FlexField2", SqlDbType.NVarChar, 500),
                      new SqlMetaData("FlexField3", SqlDbType.NVarChar, 500),
                      new SqlMetaData("FlexField4", SqlDbType.NVarChar, 500),
                      new SqlMetaData("EmployeeID", SqlDbType.NVarChar, 50),
                      new SqlMetaData("IsFlexField1Modified", SqlDbType.NVarChar, 20),
                      new SqlMetaData("IsFlexField2Modified", SqlDbType.NVarChar, 20),
                      new SqlMetaData("IsFlexField3Modified", SqlDbType.NVarChar, 20),
                      new SqlMetaData("IsFlexField4Modified", SqlDbType.NVarChar, 20));
                foreach (DebtOverrideReview obj in this)
                {
                    sqlRow.SetString(0, obj.TicketId);
                    sqlRow.SetInt64(1, obj.DebtClassificationMapId);
                    sqlRow.SetInt64(2, obj.ResolutionCodeMapId);
                    sqlRow.SetInt64(3, obj.CauseCodeMapId);
                    sqlRow.SetInt64(4, obj.ResidualDebtMapId);
                    sqlRow.SetInt64(5, obj.AvoidableFlag);
                    sqlRow.SetString(6, obj.AssignedTo);
                    sqlRow.SetString(7, obj.FlexField1);
                    sqlRow.SetString(8, obj.FlexField2);
                    sqlRow.SetString(9, obj.FlexField3);
                    sqlRow.SetString(10, obj.FlexField4);
                    sqlRow.SetString(11, obj.EmployeeId);
                    sqlRow.SetString(12, obj.IsFlexField1Modified);
                    sqlRow.SetString(13, obj.IsFlexField2Modified);
                    sqlRow.SetString(14, obj.IsFlexField3Modified);
                    sqlRow.SetString(15, obj.IsFlexField4Modified);
                    yield return sqlRow;
                }
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
            string newpth = string.Empty;
            string orgpath = string.Empty;
            AESEncryption aesMod = new AESEncryption();
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            byte[] aesKey = new byte[32];
            if (encryptionEnabled == "Enabled")
            {
                aesKey = _cacheManager.GetOrCreate<byte[]>("aesKeyconst", null, CacheDuration.Long);
            }

            try
            {
                string sourcepath = string.Empty;
                string foldername = string.Empty;
                string currentSheets = string.Empty;
                if (IsCognizant == 1)
                {
                    sourcepath = new ApplicationConstants().ExcelDebtReviewTemplatePath;
                    foldername = new ApplicationConstants().DownloadExcelTemp;
                    currentSheets = "DataOverRideAndReview";
                }
                else
                {
                    sourcepath = new ApplicationConstants().ExcelDebtReviewCustomerTemplatePath;

                    foldername = new ApplicationConstants().DownloadExcelTemp;
                    currentSheets = "DebtReviewCustomer";

                }
                string strExtension = Path.GetExtension(sourcepath);
                string orginalfile = Path.GetDirectoryName(sourcepath) + "\\";
                string filename = Path.GetFileName(sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(sourcepath);
                string struserID = Convert.ToString(CustomerID, CultureInfo.CurrentCulture);
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss", CultureInfo.CurrentCulture);
                var ext = strExtension;
                orgpath = foldername + string.Concat(fleInfo.Name.Split('.')[0], "_", struserID, "_",
                    strTimeStamp, ext);
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile);
                if (directoryInfo.Exists)
                {
                    newpth = directoryInfo + string.Concat(fleInfo.Name.Split('.')[0], "_", struserID, "_",
                        strTimeStamp, ext);
                    if (File.Exists(newpth))
                    {
                        File.Delete(newpth);
                        fleInfo.CopyTo(newpth, true);
                    }
                    else
                    {
                        fleInfo.CopyTo(newpth, true);
                    }
                }

                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@StartDate", StartDate);
                prms[1] = new SqlParameter("@EndDate", EndDate);
                prms[2] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[3] = new SqlParameter("@CustomerID", CustomerID);
                prms[4] = new SqlParameter("@ProjectID", ProjectID);
                prms[5] = new SqlParameter("@ReviewStatus", ReviewStatus);
                DataSet ds = new DBHelper().GetDatasetFromSP("GetDebtReview_Download", prms, ConnectionString);//_30Nov


                DataTable dtDataReview = ds.Tables[0];

                DataTable dsNatureTicket = new DBHelper().GetTableFromSP("GetDebtReview_Download", prms, ConnectionString);

                bool UpdateFlexField1ProjectWiseFlag = false;
                bool UpdateFlexField2ProjectWiseFlag = false;
                bool UpdateFlexField3ProjectWiseFlag = false;
                bool UpdateFlexField4ProjectWiseFlag = false;

                for (int c = 0; c < dsNatureTicket.Columns.Count; c++)
                {

                    if (dsNatureTicket.Columns[c].ToString() == "FlexField1ProjectWise")
                    {
                        UpdateFlexField1ProjectWiseFlag = dsNatureTicket.Rows[1][dsNatureTicket.Columns[c].
                            ToString()].ToString() != "0" ? true : false;

                    }
                    if (dsNatureTicket.Columns[c].ToString() == "FlexField2ProjectWise")
                    {
                        UpdateFlexField2ProjectWiseFlag = dsNatureTicket.Rows[1][dsNatureTicket.Columns[c].
                            ToString()].ToString() != "0" ? true : false;

                    }
                    if (dsNatureTicket.Columns[c].ToString() == "FlexField3ProjectWise")
                    {
                        UpdateFlexField3ProjectWiseFlag = dsNatureTicket.Rows[1][dsNatureTicket.Columns[c].
                            ToString()].ToString() != "0" ? true : false;

                    }
                    if (dsNatureTicket.Columns[c].ToString() == "FlexField4ProjectWise")
                    {
                        UpdateFlexField4ProjectWiseFlag = dsNatureTicket.Rows[1][dsNatureTicket.Columns[c].
                            ToString()].ToString() != "0" ? true : false;
                    }
                }

                for (int i = 0; i < dsNatureTicket.Rows.Count; i++)
                {
                    for (int j = 0; j < dsNatureTicket.Columns.Count; j++)
                    {
                        if (dsNatureTicket.Columns[j].ColumnName == "FlexField1ProjectWise")
                        {
                            dsNatureTicket.Rows[i][j] = UpdateFlexField1ProjectWiseFlag ? "1" : "0";
                        }
                        else if (dsNatureTicket.Columns[j].ColumnName == "FlexField2ProjectWise")
                        {
                            dsNatureTicket.Rows[i][j] = UpdateFlexField2ProjectWiseFlag ? "1" : "0";
                        }
                        else if (dsNatureTicket.Columns[j].ColumnName == "FlexField3ProjectWise")
                        {
                            dsNatureTicket.Rows[i][j] = UpdateFlexField3ProjectWiseFlag ? "1" : "0";
                        }
                        else if (dsNatureTicket.Columns[j].ColumnName == "FlexField4ProjectWise")
                        {
                            dsNatureTicket.Rows[i][j] = UpdateFlexField4ProjectWiseFlag ? "1" : "0";
                        }
                        else
                        {
                            dsNatureTicket.Rows[i][j] = string.Empty;
                        }
                    }
                }

                new OpenXMLOperations().ToExcelSheetByDataTable(dsNatureTicket, null, newpth, "Enablement", null);

                SqlParameter[] prmsCC = new SqlParameter[1];
                prmsCC[0] = new SqlParameter("@ProjectID", ProjectID);
                DataSet dsCauseCode = new DBHelper().GetDatasetFromSP("[AVL].[ReMap_GetCauseCodeDetails]",
                    prmsCC, ConnectionString);
                DataTable dtcausecodeName = dsCauseCode.Tables[0];
                new OpenXMLOperations().ToExcelSheetByDataTable(dtcausecodeName, null, newpth, "CauseCode",
                    null);

                SqlParameter[] prmsRC = new SqlParameter[1];
                prmsRC[0] = new SqlParameter("@ProjectID", ProjectID);
                DataSet dsResolutionCode = new DBHelper().
                    GetDatasetFromSP("[AVL].[ReMap_GetResolutionCodeDetails]", prmsRC, ConnectionString);
                DataTable dtResolutionCodeName = dsResolutionCode.Tables[0];
                new OpenXMLOperations().ToExcelSheetByDataTable(dtResolutionCodeName, null, newpth, "ResolutionCode",
                    null);
                DataTable dsDebtClassification = new DBHelper().
                    GetTableFromSP("[AVL].[DebtReview_GetDebtClassificationDetails]", ConnectionString);
                new OpenXMLOperations().ToExcelSheetByDataTable(dsDebtClassification, null, newpth,
                    "DebtClassification", null);
                DataTable dsAvoidableFlag = new DBHelper().
                    GetTableFromSP("[AVL].[DebtReview_GetAvoidableFlagDetails]", ConnectionString);
                new OpenXMLOperations().ToExcelSheetByDataTable(dsAvoidableFlag, null, newpth,
                    "AvoidableFlag", null);
                DataTable dsResidualDebt = new DBHelper().
                    GetTableFromSP("[AVL].[DebtReview_GetResidualDebtDetails]", ConnectionString);
                new OpenXMLOperations().ToExcelSheetByDataTable(dsResidualDebt, null, newpth,
                    "ResidualDebt", null);

                if (dtDataReview.Rows.Count > 0)

                {
                    Int64 User = Convert.ToInt64(dtDataReview.Rows[1]["IsCognizant"], CultureInfo.CurrentCulture);
                    DataColumnCollection columns = dtDataReview.Columns;
                    if (User == 0 && columns.Contains("TicketDescription"))
                    {
                        for (int c = 1; c < dtDataReview.Rows.Count; c++)
                        {

                            dtDataReview.Rows[c]["TicketDescription"] = Convert.ToString(string.IsNullOrEmpty(dtDataReview.Rows[c]["TicketDescription"].
                                 ToString()) ? string.Empty :
                                 encryptionEnabled.ToLower(CultureInfo.CurrentCulture) == "enabled" ? aesMod.DecryptStringBytes((string)
                                 dtDataReview.Rows[c]["TicketDescription"], aesKey) : dtDataReview.Rows[c]["TicketDescription"], CultureInfo.CurrentCulture);
                        }
                    }
                    else if (User == 1 && columns.Contains("TicketDescription"))
                    {
                        dtDataReview.Columns.Remove("TicketDescription");
                    }
                    else
                    {
                        //mandatory else
                    }
                    if (User == 1 && columns.Contains("ServiceName"))
                    {
                        dtDataReview.Columns.Remove("TicketType");
                    }
                    else if (User == 0 && columns.Contains("TicketType"))
                    {
                        dtDataReview.Columns.Remove("ServiceName");
                    }
                    else
                    {
                        //mandatory else
                    }
                    if (columns.Contains("AvoidableFlag"))
                    {
                        dtDataReview.Columns.Remove("AvoidableFlag");
                    }
                    if (columns.Contains("IsApproved"))
                    {
                        dtDataReview.Columns.Remove("IsApproved");
                    }
                    if (columns.Contains("AssignedTo"))
                    {
                        dtDataReview.Columns.Remove("AssignedTo");
                    }
                    if (User == 1 && columns.Contains("TicketDescription"))
                    {
                        dtDataReview.Columns.Remove("TicketDescription");
                    }
                    if (columns.Contains("Closeddate"))
                    {
                        dtDataReview.Columns.Remove("Closeddate");
                    }
                    if (columns.Contains("DebtClassificationID"))
                    {
                        dtDataReview.Columns.Remove("DebtClassificationID");
                    }

                    if (columns.Contains("ResolutionCodeMapID"))
                    {
                        dtDataReview.Columns.Remove("ResolutionCodeMapID");
                    }
                    if (columns.Contains("CauseCodeMapID"))
                    {
                        dtDataReview.Columns.Remove("CauseCodeMapID");
                    }
                    if (columns.Contains("DebtClassificationMapID"))
                    {
                        dtDataReview.Columns.Remove("DebtClassificationMapID");
                    }

                    if (columns.Contains("ResidualDebtMapID"))
                    {
                        dtDataReview.Columns.Remove("ResidualDebtMapID");
                    }
                    if (columns.Contains("ResidualDebtID"))
                    {
                        dtDataReview.Columns.Remove("ResidualDebtID");
                    }
                    if (columns.Contains("ResolutionID"))
                    {
                        dtDataReview.Columns.Remove("ResolutionID");
                    }
                    if (columns.Contains("CauseID"))
                    {
                        dtDataReview.Columns.Remove("CauseID");
                    }
                    if (columns.Contains("CustomerID"))
                    {
                        dtDataReview.Columns.Remove("CustomerID");
                    }
                    if (columns.Contains("FlexField1ProjectWise"))
                    {
                        dtDataReview.Columns.Remove("FlexField1ProjectWise");
                    }
                    if (columns.Contains("FlexField2ProjectWise"))
                    {
                        dtDataReview.Columns.Remove("FlexField2ProjectWise");
                    }
                    if (columns.Contains("FlexField3ProjectWise"))
                    {
                        dtDataReview.Columns.Remove("FlexField3ProjectWise");
                    }
                    if (columns.Contains("FlexField4ProjectWise"))
                    {
                        dtDataReview.Columns.Remove("FlexField4ProjectWise");
                    }

                    DataTable CountForExcelDt = new DataTable();
                    CountForExcelDt.Locale = CultureInfo.InvariantCulture;
                    CountForExcelDt.Columns.Add("countforexcel");
                    DataRow toInsert = CountForExcelDt.NewRow();
                    toInsert[0] = dtDataReview.Rows.Count + 1;
                    CountForExcelDt.Rows.InsertAt(toInsert, 0);
                    new OpenXMLOperations().ToExcelSheetByDataTable(CountForExcelDt, null, newpth, "countforexcel",
                        null);
                    string[] HiddenColumns = { "projectid", "iscognizant" };
                    new OpenXMLOperations().ToExcelSheetByDataTable(dtDataReview, null, newpth, currentSheets,
                        HiddenColumns);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orgpath;
        }
        /// <summary>
        /// Download function for Data Dictionary
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>

        public string ExportToExcelForDataDictionary(Int64 ProjectID)
        {
            string newpth = string.Empty;
            string orgpath = string.Empty;
            try
            {
                string sourcepath = string.Empty;
                sourcepath = new ApplicationConstants().ExcelDataDictionaryTemplatePath;
                string strExtension = Path.GetExtension(sourcepath);

                string foldername = new ApplicationConstants().DownloadExcelTemp;
                string orginalfile = Path.GetDirectoryName(sourcepath) + "\\";
                string filename = Path.GetFileName(sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(sourcepath);
                string struserID = Convert.ToString(ProjectID, CultureInfo.CurrentCulture);
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss", CultureInfo.CurrentCulture);
                var ext = strExtension;
                orgpath = foldername + string.Concat(fleInfo.Name.Split('.')[0], "_", struserID, "_",
                    strTimeStamp, ext);
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile);
                if (directoryInfo.Exists)
                {
                    newpth = directoryInfo + string.Concat(fleInfo.Name.Split('.')[0], "_", struserID, "_",
                        strTimeStamp, ext);
                    if (File.Exists(newpth))
                    {
                        File.Delete(newpth);
                        fleInfo.CopyTo(newpth, true);
                    }
                    else
                    {
                        fleInfo.CopyTo(newpth, true);
                    }
                }
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                DataSet dsDataDictionary = new DBHelper().GetDatasetFromSP("[AVL].[DD_GetDownloadData]",
                    prms, ConnectionString);
                DataTable dtDataDictionary = dsDataDictionary.Tables[0];
                DataTable dtApplication = dsDataDictionary.Tables[1];
                DataTable dtcausecodeName = dsDataDictionary.Tables[2];
                DataTable dtResolutionCodeName = dsDataDictionary.Tables[3];
                DataTable dtDebtClassification = dsDataDictionary.Tables[4];
                DataTable dtAvoidableFlag = dsDataDictionary.Tables[5];
                DataTable dtResidualDebt = dsDataDictionary.Tables[6];
                DataTable dtReasonResidual = dsDataDictionary.Tables[7];
                OpenXMLOperations oxml = new OpenXMLOperations();
                oxml.DataTableToExcel(dtDataDictionary, null, newpth, "DataDictionary", null, 4);
                oxml.DataTableToExcel(dtApplication, null, newpth, "Applications", null, 4);
                oxml.DataTableToExcel(dtcausecodeName, null, newpth, "CauseCode", null, 4);
                oxml.DataTableToExcel(dtResolutionCodeName, null, newpth, "ResolutionCode", null, 4);
                oxml.DataTableToExcel(dtDebtClassification, null, newpth, "DebtClassification", null, 4);
                oxml.DataTableToExcel(dtAvoidableFlag, null, newpth, "AvoidableFlag", null, 4);
                oxml.DataTableToExcel(dtResidualDebt, null, newpth, "ResidualDebt", null, 4);
                oxml.DataTableToExcel(dtReasonResidual, null, newpth, "ReasonforResidual", null, 4);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return orgpath;
        }
        /// <summary>
        /// Get the last uploaded errorred patterns for a project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ErrorLogPopUp> GetDDErrorLogData(Int64 ProjectID)
        {
            List<ErrorLogPopUp> objErrorLogData = new List<ErrorLogPopUp>();
            try
            {

                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectId", ProjectID);

                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[DD_GetErrorLogData]", prms, ConnectionString);
                if (dt != null)
                {

                    if (dt.Rows.Count > 0)
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            ErrorLogPopUp objresult = new ErrorLogPopUp();


                            objresult.ApplicationName = dt.Rows[i]["ApplicationName"] != DBNull.Value ?
                                Convert.ToString(dt.Rows[i]["ApplicationName"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.CauseCode = dt.Rows[i]["CauseCode"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["CauseCode"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ResolutionCode = dt.Rows[i]["ResolutionCode"] != DBNull.Value ?
                                Convert.ToString(dt.Rows[i]["ResolutionCode"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.DebtClassification = dt.Rows[i]["DebtClassification"] != DBNull.
                                Value ? Convert.ToString(dt.Rows[i]["DebtClassification"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.AvoidableFlag = dt.Rows[i]["AvoidableFlag"] != DBNull.Value ?
                                Convert.ToString(dt.Rows[i]["AvoidableFlag"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ResidualDebt = dt.Rows[i]["ResidualDebt"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["ResidualDebt"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ReasonForResidual = dt.Rows[i]["ReasonForResidual"] != DBNull.Value ?
                                Convert.ToString(dt.Rows[i]["ReasonForResidual"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.ExpectedCompletionDate = dt.Rows[i]["ExpectedCompletionDate"] != DBNull.
                                Value ? Convert.ToString(dt.Rows[i]["ExpectedCompletionDate"], CultureInfo.CurrentCulture) : string.Empty;
                            objresult.Remarks = dt.Rows[i]["Remarks"] != DBNull.Value ? Convert.
                                ToString(dt.Rows[i]["Remarks"], CultureInfo.CurrentCulture) : string.Empty;
                            objErrorLogData.Add(objresult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objErrorLogData;
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
            catch (Exception ex)
            {
                throw ex;
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
            string strPath = filepath;
            bool s = File.Exists(strPath);
            string result = "";
            try
            {
                TicketUploadRepository uploadRepository = new TicketUploadRepository();
                List<TicketDescriptionSummary> ticketDescriptionSummary = new List<TicketDescriptionSummary>();
                List<TicketSupportTypeMapping> ticketSupportMapping = new List<TicketSupportTypeMapping>();
                DataTable dt = ExcelToDataSet(filepath, ProjectID, IsCognizant, out result);
                if (result != "")
                {
                    return result;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    bool isMultilingualEnabled = uploadRepository.CheckIfMultilingualEnabled
                        (Convert.ToString(ProjectID,CultureInfo.InvariantCulture), EmployeeID);
                    if (isMultilingualEnabled)
                    {
                        ticketSupportMapping = dt.DefaultView.ToTable(true, "TicketID").AsEnumerable()
                                               .Select(r => new TicketSupportTypeMapping
                                               { TicketId = r.Field<string>("TicketID"), SupportType = 1 })
                                               .ToList();
                        ticketDescriptionSummary = uploadRepository.GetTicketValues(ticketSupportMapping,
                            Convert.ToString(ProjectID, CultureInfo.InvariantCulture), EmployeeID);
                        dt = IsCheckFlexFieldsModified(dt, ticketDescriptionSummary);
                    }
                    dt.TableName = "tblDebtReviewUpload";

                    StrResult = UploadDebtReview(dt, Convert.ToString(ProjectID, CultureInfo.InvariantCulture), EmployeeID);
                    return StrResult;
                }
            }
            catch (Exception)
            {
                StrResult = "Invalid Template. Please download the latest template and validate before uploading..";
                return StrResult;
            }
            return StrResult;
        }

        /// <summary>
        /// Retrieve Ticket details
        /// </summary>
        /// <param name="dtBulkUpload"></param>
        /// <param name="ticketDescriptionSummary"></param>
        /// <returns></returns>
        private DataTable IsCheckFlexFieldsModified(DataTable dtBulkUpload,
            List<TicketDescriptionSummary> ticketDescriptionSummary)
        {
            for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
            {
                dtBulkUpload.Rows[i]["IsFlexField1Modified"] = (!dtBulkUpload.Columns.Contains("FlexField1")
                    || string.IsNullOrEmpty(Convert.ToString(dtBulkUpload.Rows[i]["FlexField1"], CultureInfo.InvariantCulture)) ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["TicketID"].ToString()
                       && (sd.FlexField1.Trim().Equals(dtBulkUpload.Rows[i]["FlexField1"].ToString().Trim())))))
                       ) == true ? "0" : "1";

                dtBulkUpload.Rows[i]["IsFlexField2Modified"] = (!dtBulkUpload.Columns.Contains("FlexField2")
                    || string.IsNullOrEmpty(Convert.ToString(dtBulkUpload.Rows[i]["FlexField2"], CultureInfo.InvariantCulture)) ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["TicketID"].ToString()
                       && (sd.FlexField2.Trim().Equals(dtBulkUpload.Rows[i]["FlexField2"].ToString().Trim())))))
                       ) == true ? "0" : "1";

                dtBulkUpload.Rows[i]["IsFlexField3Modified"] = (!dtBulkUpload.Columns.Contains("FlexField3")
                    || string.IsNullOrEmpty(Convert.ToString(dtBulkUpload.Rows[i]["FlexField3"], CultureInfo.InvariantCulture)) ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["TicketID"].ToString()
                       && (sd.FlexField3.Trim().Equals(dtBulkUpload.Rows[i]["FlexField3"].ToString().Trim())))))
                       ) == true ? "0" : "1";

                dtBulkUpload.Rows[i]["IsFlexField4Modified"] = (!dtBulkUpload.Columns.Contains("FlexField4")
                    || string.IsNullOrEmpty(Convert.ToString(dtBulkUpload.Rows[i]["FlexField4"], CultureInfo.InvariantCulture)) ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["TicketID"].ToString()
                       && (sd.FlexField4.Trim().Equals(dtBulkUpload.Rows[i]["FlexField4"].ToString().Trim())))))
                       ) == true ? "0" : "1";
            }
            return dtBulkUpload;
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
            Int64 DDUploadID = 0;
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[2] = new SqlParameter("@Filename", Filename);
                dt = (new DBHelper()).GetTableFromSP("AVL.DD_AddExcelDetailsByProject", prms, ConnectionString);
                if (dt != null)
                {
                    DDUploadID = Convert.ToInt64(dt.Rows[0]["DDUploadID"], CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DDUploadID;
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
            string strPath = filepath + filename;
            string path = filepath;
            string result = "";
            bool s = File.Exists(strPath);
            try
            {
                DataTable dt = ExcelToDataSetDataDictionary(filepath, ProjectID, EmployeeID, out result);
                if (result != "")
                {
                    return result;
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.TableName = "tblDataDictionaryUpload";
                    StrResult = UploadDataDictionaryByProject(dt, Convert.ToString(ProjectID, CultureInfo.InvariantCulture), EmployeeID,
                        DDUploadID);
                    return StrResult;
                }
            }
            catch (Exception)
            {
                StrResult = new ApplicationConstants().InvalidTemplateMessage;
                return StrResult;
            }
            return StrResult;
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
            DataTable dtDataDictionary = new DataTable();
            dtDataDictionary.Locale = CultureInfo.InvariantCulture;
            try
            {
                bool flgxlsm = Path.GetExtension(filename).Contains("xlsm");
                bool flgcsv = Path.GetExtension(filename).Contains("csv");
                bool flgxls = Path.GetExtension(filename).Contains("xls");

                string strCon = string.Empty;
                string strCom = string.Empty;
                string strCom1 = string.Empty;
                string sWSName = String.Empty;
                string sql = String.Empty;
                int diffcount = 0;
                List<string> diff;
                dtDataDictionary = new DataTable();
                dtDataDictionary.Locale = CultureInfo.InvariantCulture;
                OpenXMLOperations oxml = new OpenXMLOperations();
                string[] arr = { "Expected Completion Date(MM/DD/YYYY)" };
                dtDataDictionary = oxml.ToDataTableBySheetName(filename, "DataDictionary", 1, 2, arr);

                if (dtDataDictionary == null)
                {
                    result = new ApplicationConstants().InvalidUploadTemplateMessage;
                    return dtDataDictionary;
                }
                else if (dtDataDictionary.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtDataDictionary.Columns)
                    {
                        string sColumnName = dc.ColumnName;
                        dtDataDictionary.Columns[sColumnName].ColumnName = dtDataDictionary.
                            Columns[sColumnName].ColumnName.TrimStart().TrimEnd();
                    }

                    for (int i = dtDataDictionary.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dtDataDictionary.Rows[i];
                        if (string.IsNullOrEmpty(Convert.ToString(dr[0], CultureInfo.InvariantCulture)) && string.IsNullOrEmpty
                            (Convert.ToString(dr[1])) && string.IsNullOrEmpty(Convert.ToString(dr[2], CultureInfo.InvariantCulture))
                            && string.IsNullOrEmpty(Convert.ToString(dr[3], CultureInfo.InvariantCulture)) && string.IsNullOrEmpty
                            (Convert.ToString(dr[4])) && string.IsNullOrEmpty(Convert.ToString(dr[5], CultureInfo.InvariantCulture))
                            && string.IsNullOrEmpty(Convert.ToString(dr[6], CultureInfo.InvariantCulture)) && string.IsNullOrEmpty
                            (Convert.ToString(dr[7], CultureInfo.InvariantCulture)))
                        {
                            dr.Delete();
                        }
                    }

                    SqlParameter[] prms = new SqlParameter[0];
                    DataTable dtspcolumnnames = new DBHelper().GetTableFromSP("AVL.GetDDExcelColumnNames",
                        prms, ConnectionString);
                    for (int j = 0; j < dtspcolumnnames.Rows.Count; j++)
                    {
                        if (!dtDataDictionary.Columns.Contains(dtspcolumnnames.Rows[0]["Column name"].
                            ToString()))
                        {

                            diffcount = diffcount + 1;

                        }
                    }
                    var dtColumnNames = (from DataColumn x in dtDataDictionary.Columns
                                         select x.ColumnName.ToUpper(CultureInfo.InvariantCulture)).ToArray();

                    var spcolnames = dtspcolumnnames.Rows.OfType<DataRow>().Select(k => k[0].ToString().
                    ToUpper(CultureInfo.InvariantCulture)).ToArray();

                    IEnumerable<string> set1 = dtColumnNames;
                    IEnumerable<string> set2 = spcolnames;
                    if (set2.Count() > set1.Count())
                    {
                        diff = set2.Except(set1).ToList();
                    }
                    else
                    {
                        diff = set1.Except(set2).ToList();
                    }

                    if (diffcount > 0 || diff.Count > 0)
                    {
                        result = new ApplicationConstants().InvalidTemplateMessage;
                        return dtDataDictionary;
                    }

                    else if (dtDataDictionary != null && dtDataDictionary.Rows.Count > 0)
                    {
                        result = "";
                        return dtDataDictionary;
                    }
                    else
                    {
                        result = new ApplicationConstants().NoDataMessage;
                        return dtDataDictionary;
                    }
                }
                else
                {
                    //mandatory else
                }
            }
            catch (Exception)
            {
                result = new ApplicationConstants().InvalidMessage;
                return dtDataDictionary;
            }
            result = "";
            return dtDataDictionary;
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
            DataTable dtDebtReview = new DataTable();
            dtDebtReview.Locale = CultureInfo.InvariantCulture;
            try
            {
                bool flgxlsm = Path.GetExtension(filename).Contains("xlsm");
                bool flgcsv = Path.GetExtension(filename).Contains("csv");
                bool flgxls = Path.GetExtension(filename).Contains("xls");

                string strCon = string.Empty;
                string strCom = string.Empty;
                string strCom1 = string.Empty;
                string sWSName = String.Empty;
                string sql = String.Empty;
                int Rowcnt = 0;
                int flag = 0;

                dtDebtReview = new OpenXMLOperations().ToDataTableBySheetName(filename, (IsCognizant != 1 ?
                    "DebtReviewCustomer" : "DataOverRideAndReview"), 0, 1);
                var filteredRows = dtDebtReview.Rows.Cast<DataRow>().Where(
                    row => row.ItemArray.Any(field => !(field is System.DBNull)));
                Rowcnt = filteredRows.Count();

                var causeCodeList = GetCauseCode(Convert.ToInt32(ProjectID));
                var resoluationCodeList = GetResolutionCode(Convert.ToInt32(ProjectID));
                var debtCategory = GetDebtClassificationmodel();
                var avoidableFlag = GetAvoidableFlag();
                var residual = GetResidualDebt();
                var natureOfTicket = GetNatureOfTicket();

                if (Rowcnt < 2001 && Rowcnt > 0)
                {
                    if (dtDebtReview != null && dtDebtReview.Rows.Count > 0 && Rowcnt > 0)
                    {
                        foreach (DataColumn dc in dtDebtReview.Columns)
                        {
                            string sColumnName = dc.ColumnName;
                            dtDebtReview.Columns[sColumnName].ColumnName = dtDebtReview.Columns[sColumnName].
                                ColumnName.TrimStart().TrimEnd();
                        }

                        int debtReviewCount = dtDebtReview.Rows.Count - 1;
                        for (int i = debtReviewCount; i >= 0; i--)
                        {
                            DataRow dr = dtDebtReview.Rows[i];
                            if (string.IsNullOrEmpty(dr[0].ToString()) && string.IsNullOrEmpty(dr[1].ToString()))
                            {
                                dr.Delete();
                            }
                        }
                        StringBuilder ticketId = new StringBuilder();
                        StringBuilder data = new StringBuilder();
                        StringBuilder message = new StringBuilder();
                        List<MapFlexFields> objMapFlexFieldsList = new List<MapFlexFields>();
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@ProjectID", ProjectID);
                        var healPatternList = new DBHelper().
                            GetTableFromSP("[AVL].[DebtReview_GetHealPatternByProjectId]", prms, ConnectionString);
                        if (healPatternList != null && healPatternList.Rows.Count > 0)
                        {
                            int rowCount = healPatternList.Rows.Count;
                            for (int i = 0; i < rowCount; i++)
                            {
                                MapFlexFields objMapFlexFields = new MapFlexFields();
                                objMapFlexFields.ServiceColumn = Convert.ToString((healPatternList.
                                    Rows[i]["ServiceColumn"]) == DBNull.Value ? "0" : healPatternList.
                                    Rows[i]["ServiceColumn"], CultureInfo.InvariantCulture);
                                objMapFlexFields.ProjectColumn = Convert.ToString((healPatternList.
                                    Rows[i]["ColumnName"]) == DBNull.Value ? "0" : healPatternList.
                                    Rows[i]["ColumnName"], CultureInfo.InvariantCulture);
                                objMapFlexFieldsList.Add(objMapFlexFields);
                            }
                        }
                        DataColumnCollection columnName = dtDebtReview.Columns;
                        if (columnName.Contains("FlexField1"))
                        {
                            dtDebtReview.Columns.Remove("FlexField1");
                        }
                        if (columnName.Contains("FlexField2"))
                        {
                            dtDebtReview.Columns.Remove("FlexField2");
                        }
                        if (columnName.Contains("FlexField3"))
                        {
                            dtDebtReview.Columns.Remove("FlexField3");
                        }
                        if (columnName.Contains("FlexField4"))
                        {
                            dtDebtReview.Columns.Remove("FlexField4");
                        }

                        foreach (DataRow wdr in dtDebtReview.Rows)
                        {
                            foreach (DataColumn col in dtDebtReview.Columns)
                            {
                                data.Append(wdr[col].ToString().Trim().ToUpper(CultureInfo.InvariantCulture));
                                if (col.ColumnName == "TicketID")
                                {
                                    ticketId = data;
                                }
                                if (data.ToString() != "" || objMapFlexFieldsList.Where(s => s.ProjectColumn ==
                                col.ColumnName).FirstOrDefault() != null || col.ColumnName.Contains("Flex"))
                                {
                                    wdr[col] = HttpUtility.HtmlEncode(wdr[col].ToString().Trim());
                                    wdr[col] = HttpUtility.HtmlDecode(wdr[col].ToString().Trim());
                                    if (col.ColumnName == "Cause Code")
                                    {
                                        if (!causeCodeList.Exists(s => s.CauseCode.ToUpper(CultureInfo.InvariantCulture) == data.ToString()))
                                        {
                                            message.Append("Please enter valid Cause Code for the ticket ").
                                                Append(ticketId);
                                            flag = 1;
                                        }
                                    }
                                    else if (col.ColumnName == "Resolution Code")
                                    {
                                        if (!resoluationCodeList.Exists(s => s.ResolutionCode.ToUpper(CultureInfo.InvariantCulture) ==
                                        data.ToString()))
                                        {
                                            message.Append("Please enter valid Resolution Code for the ticket ").
                                                Append(ticketId);
                                            flag = 1;
                                        }
                                    }

                                    else if (col.ColumnName == "Debt Category")
                                    {
                                        if (!debtCategory.Exists(s => s.DebtClassificationName.ToUpper(CultureInfo.InvariantCulture) == data.
                                        ToString()))
                                        {
                                            message.Append("Please enter valid Debt Category for the ticket ").
                                                Append(ticketId);
                                            flag = 1;
                                        }
                                    }

                                    else if (col.ColumnName == "Avoidable Flag")
                                    {
                                        if (!avoidableFlag.Exists(s => s.AvoidableFlagName.ToUpper(CultureInfo.InvariantCulture) == data.
                                        ToString()))
                                        {
                                            message.Append("Please enter valid Avoidable Flag for the ticket ").
                                                Append(ticketId);
                                            flag = 1;
                                        }
                                    }
                                    else if (col.ColumnName == "ResidualDebt")
                                    {
                                        if (!residual.Exists(s => s.ResidualDebtName.ToUpper(CultureInfo.InvariantCulture) == data.ToString()))
                                        {
                                            message.Append("Please enter valid Residual for the ticket ").
                                                Append(ticketId);
                                            flag = 1;
                                        }
                                    }

                                    else if (string.IsNullOrEmpty(data.ToString()))
                                    {
                                        message.Append("Please enter the value for the ticket ").Append(ticketId);
                                        flag = 1;
                                    }
                                    else
                                    {
                                        //mandatory else
                                    }
                                }
                                else
                                {
                                    message.Append("Please enter data for the ticket ").Append(ticketId);
                                    flag = 1;
                                }

                                if (flag == 1)
                                {
                                    result = message.ToString();
                                    return dtDebtReview;
                                }
                                ticketId = new StringBuilder();
                                data = new StringBuilder();
                                message = new StringBuilder();
                            }
                        }

                        DataColumnCollection columns = dtDebtReview.Columns;
                        int fieldListCount = objMapFlexFieldsList.Count();
                        for (int j = 0; j < fieldListCount; j++)
                        {
                            if (columns.Contains(objMapFlexFieldsList[j].ProjectColumn))
                            {
                                dtDebtReview.Columns[objMapFlexFieldsList[j].ProjectColumn].ColumnName =
                                    objMapFlexFieldsList[j].ServiceColumn.Trim();
                            }
                        }

                        foreach (DataRow dr in dtDebtReview.Rows)
                        {
                            dr["ProjectID"] = ProjectID;
                            dr["IsCognizant"] = IsCognizant;
                        }

                        if (dtDebtReview != null && dtDebtReview.Rows.Count > 0)
                        {
                            dtDebtReview.Columns.Add(new DataColumn("IsFlexField1Modified", typeof(string)));
                            dtDebtReview.Columns.Add(new DataColumn("IsFlexField2Modified", typeof(string)));
                            dtDebtReview.Columns.Add(new DataColumn("IsFlexField3Modified", typeof(string)));
                            dtDebtReview.Columns.Add(new DataColumn("IsFlexField4Modified", typeof(string)));
                            dtDebtReview.AcceptChanges();
                            result = "";
                            return dtDebtReview;
                        }
                        else
                        {
                            result = "The file has no data to upload.";
                            return dtDebtReview;
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = "Invalid Template";
                return dtDebtReview;
            }
            result = "";
            return dtDebtReview;
        }

        /// <summary>
        /// To upload data dictionary values UploadDataDictionaryByProject
        /// </summary>
        /// <param name="dtExcelBytes"></param>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private string UploadDataDictionaryByProject(DataTable dtExcelBytes, string ProjectID, string EmployeeID,
            Int64 DDUploadID)
        {
            string IsUpload = "";
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@DataDictionaryDetailsUpload";
                parameter.SqlDbType = System.Data.SqlDbType.Structured;
                parameter.Value = dtExcelBytes;
                parameter.TypeName = "[AVL].[TVP_DataDictionaryDetailsUpload]";
                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt64(ProjectID, CultureInfo.InvariantCulture));
                prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[2] = parameter;

                prms[3] = new SqlParameter("@DDUploadID", Convert.ToInt64(DDUploadID));
                DataSet ds = new DBHelper().GetDatasetFromSP("[AVL].[DataDictionaryExcelUpload]", prms, ConnectionString);
                IsUpload = "Y";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsUpload;

        }
        /// <summary>
        /// This Method Is Used To InsertDatatable
        /// </summary>
        /// <param name="dtExcelBytes"></param>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private string InsertDatatable(DataTable dtExcelBytes, string ProjectID, string EmployeeID)
        {
            try
            {
                if (dtExcelBytes == null || dtExcelBytes.Rows.Count == 0)
                {
                    return StrResult;
                }

                try
                {
                    SqlConnectionStringBuilder connectionBuilder = new DBHelper().GetSqlConnectionStringBuilder(ConnectionString);
                    connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                    StrResult = "";
                    using (SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString))
                    {
                        connection.Open();

                        SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);

                        for (int y = 0; y < dtExcelBytes.Columns.Count; y++)
                        {

                            bulkcopy.ColumnMappings.Add(dtExcelBytes.Columns[y].ColumnName, dtExcelBytes.Columns[y].
                                ColumnName);

                        }

                        bulkcopy.DestinationTableName = "[dbo].[DebtReviewTemp]";
                        bulkcopy.BulkCopyTimeout = 100000;
                        bulkcopy.WriteToServer(dtExcelBytes);

                    }

                }
                catch (InvalidOperationException exInvOp)
                {
                    DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                    objDebtFieldsApprovalRepository.ErrorLOG(exInvOp.Message, "Ticket upload Bulk insert not done",
                        Convert.ToInt64(ProjectID, CultureInfo.InvariantCulture));
                    if (exInvOp.InnerException.Message == "String or binary data would be truncated.")
                    {
                        StrResult = "Data length is exceeding the maximum limit. Please refer the Help" +
                            " file for data limits";
                    }
                    else
                    {
                        StrResult = "Data Type mismatch. Please refer the Help file for data DataType";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StrResult;
        }
        /// <summary>
        /// This Method Is Used To UploadDebtReview
        /// </summary>
        /// <param name="dtExcelBytes"></param>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private string UploadDebtReview(DataTable dtExcelBytes, string ProjectID, string EmployeeID)
        {
            int prjID;
            bool blProjectId = int.TryParse(ProjectID, out prjID);
            try
            {
                InsertDatatable(dtExcelBytes, ProjectID, EmployeeID);
                if (dtExcelBytes.Rows.Count > 0)
                {
                    SqlParameter[] prms = new SqlParameter[2];
                    SqlParameter parameter = new SqlParameter();
                    prms[0] = new SqlParameter("@ProjectID", ProjectID);
                    prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                    DataSet ds = new DBHelper().GetDatasetFromSP("[AVL].[UploadAndUpdateDebtReview]", prms, ConnectionString);
                }
                else
                {
                    return "Invalid Template. Please download the latest template and validate before uploading..";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return StrResult;
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

            SqlParameter[] prms = new SqlParameter[4];
            string result = "";
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@ApplicationID", ApplicationID);
            prms[2] = new SqlParameter("@EffectiveDate", EffectiveDate);
            prms[3] = new SqlParameter("@EmployeeID", EmployeeID);



            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("Debt_DataDictionarySignOffDate", prms, ConnectionString);
                if (dt != null)
                {
                    result = Convert.ToString(dt.Rows[0]["Result"], CultureInfo.InvariantCulture);
                }
                if (new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"] == "true")
                {
                    if (result == "True")
                    {

                        List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                        existingAcitivities = new MyActivity().GetExistingActivitys(ProjectID, new AppSettings().AppsSttingsKeyValues["AutoDDWorkItemCode"], access);
                        List<ExistingAcitivityDetailsModel> existingAcitivity = new List<ExistingAcitivityDetailsModel>();
                        existingAcitivity = new MyActivity().GetExistingActivitys(ProjectID, new AppSettings().AppsSttingsKeyValues["PPDDWorkItemCode"], access);
                        if (existingAcitivities != null && existingAcitivities.Count > 0)
                        {
                            if (!existingAcitivities[0].IsExpired)
                            {
                                UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                                expiryModel.WorkItemCode = new AppSettings().AppsSttingsKeyValues["AutoDDWorkItemCode"];
                                expiryModel.SourceRecordId = ProjectID;
                                expiryModel.ModifiedBy = "System";
                                string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);
                            }
                        }
                        if (existingAcitivity.Count > 0)
                        {
                            if (!existingAcitivity[0].IsExpired)
                            {
                                UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                                expiryModel.WorkItemCode = new AppSettings().AppsSttingsKeyValues["PPDDWorkItemCode"];
                                expiryModel.SourceRecordId = ProjectID;
                                expiryModel.ModifiedBy = "System";
                                string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);
                            }
                        }
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
        /// Gets the drop down values for Project and dynamic name for AppGroup
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>

        public DataDictionaryProjects GetDropDownValuesProjectPortfolio(string employeeID, int customerID)
        {
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            DataDictionaryProjects ddlist = new DataDictionaryProjects();
            List<Project> project = new List<Project>();
            string PortfolioName = "";
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@CustomerID", customerID);
                prms[1] = new SqlParameter("@EmployeeID", employeeID);
                dsResult = (new DBHelper()).GetDatasetFromSP("[AVL].[DD_GetDataDictionaryProjectPortfolio]", prms, ConnectionString);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count > 0)
                    {
                        project = dsResult.Tables[0].AsEnumerable().Select(x => new Project
                        {
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt32(x["ProjectID"], CultureInfo.InvariantCulture) : 0,
                            ProjectName = x["ProjectName"] != DBNull.Value ? Convert.ToString(x["ProjectName"], CultureInfo.InvariantCulture) :
                            string.Empty,
                            IsDDAutoClassified = x["IsDDAutoClassified"] != DBNull.Value ? Convert.
                            ToString(x["IsDDAutoClassified"], CultureInfo.InvariantCulture) : string.Empty,
                            IsManual = x["IsManual"] != DBNull.Value ? Convert.ToString(x["IsManual"], CultureInfo.InvariantCulture) :
                            string.Empty,
                            IsTicketApprovalNeeded = x["IsTicketApprovalNeeded"] != DBNull.Value ? Convert.
                            ToString(x["IsTicketApprovalNeeded"], CultureInfo.InvariantCulture) : string.Empty,
                        }).ToList();
                        ddlist.Project = project;
                    }
                    if (dsResult.Tables.Count > 1)
                    {
                        if (dsResult.Tables[1] != null && dsResult.Tables[1].Rows.Count > 0)
                        {
                            PortfolioName = Convert.ToString(dsResult.Tables[1].Rows[0]["PortfolioName"], CultureInfo.InvariantCulture);
                        }
                        ddlist.PortfolioName = PortfolioName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ddlist;
        }


        /// <summary>
        /// This Method Is Used To GetConflictPatterns
        /// </summary>
        /// <param name="ProjectId"></param>

        /// <returns></returns>
        public List<ConflictPatterns> GetConflictPatterns(int ProjectId)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            List<ConflictPatterns> lstconflictResult = new List<ConflictPatterns>();
            string sPName = "[AVL].[GetConflictPatternDetails]";
            try
            {

                SqlParameter[] conflictprms = new SqlParameter[1];
                conflictprms[0] = new SqlParameter("@ProjectId", ProjectId);

                DataSet dsconflicts = (new DBHelper()).GetDatasetFromSP(sPName, conflictprms, ConnectionString);
                if (dsconflicts != null)
                {
                    if (dsconflicts.Tables[0] != null && dsconflicts.Tables[0].Rows.Count > 0)
                    {

                        lstconflictResult = dsconflicts.Tables[0].AsEnumerable().Select(row => new ConflictPatterns
                        {
                            ApplicationName = Convert.ToString(row["ApplicationName"], CultureInfo.InvariantCulture),
                            CauseCodeName = Convert.ToString(row["CauseCode"], CultureInfo.InvariantCulture),
                            ResolutionName = Convert.ToString(row["ResolutionCode"], CultureInfo.InvariantCulture),
                            DebtClassficationName = Convert.ToString(row["DebtCategory"], CultureInfo.InvariantCulture),
                            AvoidableFlag = Convert.ToString(row["AvoidableFlag"], CultureInfo.InvariantCulture),
                            ResidualFlag = Convert.ToString(row["ResidualFlag"], CultureInfo.InvariantCulture),
                            TicketCount = Convert.ToInt32(row["TicketCount"], CultureInfo.InvariantCulture),
                            Period = Convert.ToString(row["Period"], CultureInfo.InvariantCulture),
                            ExistingPattern = row["ExistingPattern"] != DBNull.Value ? Convert.
                            ToString(row["ExistingPattern"], CultureInfo.InvariantCulture) : string.Empty,

                        }).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstconflictResult;
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
                DataTable dtconflits = ListExtensions.ToDataTable<ConflictPatterns>(lstconflict);
                dtconflits.Columns["ApplicationName"].ColumnName = ApplicationConstants.ApplicationName;
                dtconflits.Columns["CauseCodeName"].ColumnName = ApplicationConstants.CauseCodeName;
                dtconflits.Columns["ResolutionName"].ColumnName = ApplicationConstants.ResolutionName;
                dtconflits.Columns["DebtClassficationName"].ColumnName = ApplicationConstants.DebtClassficationName;
                dtconflits.Columns["AvoidableFlag"].ColumnName = ApplicationConstants.AvoidableFlagName;
                dtconflits.Columns["TicketCount"].ColumnName = ApplicationConstants.TicketCount;
                dtconflits.Columns["ResidualFlag"].ColumnName = ApplicationConstants.ResidualFlag;
                dtconflits.Columns["Period"].ColumnName = ApplicationConstants.Period;
                dtconflits.Columns["ExistingPattern"].ColumnName = ApplicationConstants.ExistingPattern;
                new OpenXMLFunctions().ExportDataSet(dtconflits, DestinationTemplateFileName);

                string dirctoryName = Path.GetDirectoryName(DestinationTemplateFileName);
                string fName = Path.GetFileNameWithoutExtension(DestinationTemplateFileName);
                string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                validatePath = RemoveLastIndexCharacter(validatePath);
                return validatePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveLastIndexCharacter(string path)
        {
            if (path.Length > 0)
            {
                int place = path.LastIndexOf(@"\");
                string result = path.Remove(place, (@"\").Length);
                return result;
            }
            else
            {
                return "";
            }
        }
    }

}
