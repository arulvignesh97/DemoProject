using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using CTS.Applens.WorkProfiler.Entities.Base;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL.Mainspring
{
    public class MainspringRepository : DBContext
    {

        /// <summary>
        /// This Method Is Used To GetMainspringProjectDetails
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<MainspringProjectModel> GetMainspringProjectDetails(int customerID, string employeeID)
        {

            List<MainspringProjectModel> lstMainspringProjectsModel = new List<MainspringProjectModel>();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                prms[1] = new SqlParameter("@CustomerID", customerID);

                DataTable dt = (new DBHelper()).GetTableFromSP("[MS].[GetMainspringProjectDetails]", prms, ConnectionString);
                if (dt != null)
                {
                    lstMainspringProjectsModel = dt.AsEnumerable().Select(x => new MainspringProjectModel
                    {
                        ProjectId = x["ProjectID"] != DBNull.Value ? x["ProjectID"].ToString() : string.Empty,
                        ProjectName = x["ProjectName"] != DBNull.Value ? x["ProjectName"].ToString() : string.Empty,
                        IsODCRestricted = x["IsODCRestricted"] != DBNull.Value ? x["IsODCRestricted"].
                        ToString() : string.Empty,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return lstMainspringProjectsModel;
        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureFiltermainspringavailability
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public MainspringProjectModel GetBaseMeasureFilterData(int ProjectID)
        {
            MainspringProjectModel model = new MainspringProjectModel();
            DataSet ds = GetBaseMeasureFilter(ProjectID);
            List<ServiceList> lstService = new List<ServiceList>();
            List<MetricList> lstMetric = new List<MetricList>();

            if (ds != null && ds.Tables.Count > 0)
            {

                model.FrequencyId = Convert.ToString(ds.Tables[0].Rows[0]["FrequencyID"]);
                model.FrequencyName = Convert.ToString(ds.Tables[0].Rows[0]["FrequencyName"]);

                model.RowIndex = Convert.ToString(ds.Tables[1].Rows[0]["RowIndex"]);
                model.ReportPeriodId = Convert.ToString(ds.Tables[1].Rows[0]["ReportPeriodID"]);
                model.ReportPeriod = Convert.ToString(ds.Tables[1].Rows[0]["ReportPeriod"]);

                lstService = ds.Tables[2].AsEnumerable().Select(x => new ServiceList()
                {
                    ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToString(x["ServiceID"]) : "",
                    ServiceName = x["MainspringServiceName"] != DBNull.Value ? Convert.ToString(x["MainspringServiceName"]) : "",
                }).ToList();
                model.Lstservice = lstService;

                lstMetric = ds.Tables[3].AsEnumerable().Select(x => new MetricList()
                {
                    MetricName = x["MetricName"] != DBNull.Value ? Convert.ToString(x["MetricName"]) : "",
                    MetricId = x["MetricID"] != DBNull.Value ? Convert.ToString(x["MetricID"]) : "",
                    UOMDataType = x["UOMDataType"] != DBNull.Value ? Convert.ToString(x["UOMDataType"]) : "",
                    UOMDESC = x["UOM"] != DBNull.Value ? Convert.ToString(x["UOM"]) : "",
                    ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToString(x["ServiceID"]) : "",
                }).ToList();
                model.LstMetrics = lstMetric;


                model.IsMainSpringConfigured = Convert.ToString(ds.Tables[4].Rows[0]["MainSpringConfigured"]);
                model.SupportCategoryCount = Convert.ToInt32(ds.Tables[4].Rows[0]["SUPPORTCATEGORYCount"]);
                model.IsODCRestricted = Convert.ToString(ds.Tables[4].Rows[0]["IsODCRestricted"]);

            }

            return model;
        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureLoadFactorProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <returns></returns>
        public bool GetBaseMeasureLoadFactorProject(string ProjectID, string MetricName)
        {
            bool isProjectSpecificBaseMeasure = false;
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@MetricName", MetricName);
            dtResult = (new DBHelper()).GetTableFromSP("[MS].[IsProjectSpecificLoadFactor]", prms, ConnectionString);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                isProjectSpecificBaseMeasure = true;
            }
            return isProjectSpecificBaseMeasure;
        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureValueLoadFactor
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <param name="ReportPeriodID"></param>
        /// <returns></returns>
        public string GetBaseMeasureValueLoadFactor(string ProjectID, string MetricName, int ReportPeriodID)
        {
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@MetricName", MetricName);
            prms[2] = new SqlParameter("@ReportPeriodID", ReportPeriodID);
            string BaseMeasureValue = "";
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[MS].[GetManualBaseMeasureValueLoadFactor]", prms, ConnectionString);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        BaseMeasureValue = (Convert.ToString(dt.Rows[0]["BaseMeasureValue"] != DBNull.Value
                            ? Convert.ToString(dt.Rows[0]["BaseMeasureValue"]) : ""));

                    }
                }
            }
            catch (Exception ex)
            {
                new ExceptionUtility().LogExceptionMessage(ex);
            }

            return BaseMeasureValue;

        }
        /// <summary>
        /// This Method Is Used To SaveLoadFactor
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <param name="ReportPeriodID"></param>
        /// <param name="LoadFactor"></param>
        /// <returns></returns>
        public string SaveLoadFactor(int ProjectID, string MetricName, int ReportPeriodID, int LoadFactor)
        {
            string result = string.Empty;
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            try
            {

                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@MetricName", MetricName);
                prms[2] = new SqlParameter("@ReportPeriodID", ReportPeriodID);
                prms[3] = new SqlParameter("@BaseMeasureValue", LoadFactor);
                dtResult = (new DBHelper()).GetTableFromSP("[MS].[SaveManualBaseMeasureValueLoadFactor]", prms, ConnectionString);
                if (dtResult != null)
                {
                    result = Convert.ToString(dtResult.Rows[0]["Result"]);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get Ticket Summary Filter Service List
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ServiceFilter"></param>
        /// <returns></returns>
        public List<MainspringServiceListModel> GetTicketSummaryFilterServiceList(int ProjectID, int ServiceFilter)
        {
            List<MainspringServiceListModel> lstResult = new List<MainspringServiceListModel>();
            DataTable dt = GetTicketSummaryBaseMeasureFilter(ProjectID, null, null, "services", ServiceFilter);
            if (dt != null && dt.Rows.Count > 0)
            {
                lstResult = dt.AsEnumerable().Select(row => new MainspringServiceListModel
                {
                    ServiceId = Convert.ToString(row["ServiceID"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),

                }).ToList();
            }
            return lstResult;
        }

        /// <summary>
        /// This Method is used to GetBaseMeasureFilter
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="RequiredFilterType"></param>
        /// <param name="ServiceFilter"></param>
        /// <returns></returns>
        public DataSet GetBaseMeasureFilter(int ProjectID)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            DataSet ds = (new DBHelper()).GetDatasetFromSP("[MS].[GetBaseMeasureFilters]", prms, ConnectionString);
            return ds;
        }

        /// <summary>
        /// This Method Is Used To GetTicketSummaryBaseMeasureFilter
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="RequiredFilterType"></param>
        /// <param name="ServiceFilter"></param>
        /// <returns></returns>
        private DataTable GetTicketSummaryBaseMeasureFilter(int ProjectID, int? FrequencyID, string ServiceIDs,
            string RequiredFilterType, int? ServiceFilter)
        {
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[5];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            if (FrequencyID != null)
            {
                prms[1] = new SqlParameter("@FrequencyID", FrequencyID);
            }
            else
            {
                prms[1] = new SqlParameter("@FrequencyID", DBNull.Value);
            }
            if (ServiceIDs != null && ServiceIDs.Trim() != "")
            {
                prms[2] = new SqlParameter("@ServiceIDs", ServiceIDs.Trim());
            }
            else
            {
                prms[2] = new SqlParameter("@ServiceIDs", DBNull.Value);
            }
            prms[3] = new SqlParameter("@RequiredFilterType", RequiredFilterType);
            prms[4] = new SqlParameter("@ServiceFilter", ServiceFilter);
            dtResult = (new DBHelper()).GetTableFromSP("[MS].[GetTicketSummaryBaseMeasureFilter]", prms, ConnectionString);

            return dtResult;

        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureProjectwiseSearchUserDefined
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="BaseMeasureSystemDefinedOrUserDefined"></param>
        /// <returns></returns>
        public List<MainspringUserDefinedBaseMeasureModel> GetBaseMeasureProjectwiseSearchUserDefined(int ProjectID,
            int? FrequencyID, string ServiceIDs, string MetricsIDs, int? ReportFrequencyID,
            string BaseMeasureSystemDefinedOrUserDefined)
        {
            List<MainspringUserDefinedBaseMeasureModel> lstResult = new List<MainspringUserDefinedBaseMeasureModel>();

            DataTable dt = GetBaseMeasureSearchResult(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                BaseMeasureSystemDefinedOrUserDefined, ReportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                lstResult = dt.AsEnumerable().Select(row => new MainspringUserDefinedBaseMeasureModel
                {
                    ServiceId = Convert.ToInt32(row["ServiceID"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),
                    BaseMeasureId = Convert.ToInt32(row["BaseMeasureID"]),
                    BaseMeasureName = Convert.ToString(row["BaseMeasureName"]),
                    UOMDesc = Convert.ToString(row["UOM_DESC"]),
                    UOMDataType = Convert.ToString(row["UOM_DataType"]),
                    BaseMeasureValue = Convert.ToString(row["BaseMeasureValue"]),
                    BaseMeasureTypeId = Convert.ToInt32(row["BaseMeasureTypeID"]),
                    MetricId = Convert.ToInt32(row["MetricID"]),
                    MetricName = Convert.ToString(row["MetricName"])

                }).ToList();
            }

            return lstResult;
        }

        /// <summary>
        /// This Method Is Used To GetTicketSummeryBaseMeasureODCList
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <returns></returns>
        public List<MainspringUserDefinedBaseMeasureModel> GetTicketSummeryBaseMeasureODCList(int ProjectID,
            int? FrequencyID, string ServiceIDs, int? ReportFrequencyID)
        {
            List<MainspringUserDefinedBaseMeasureModel> lstResult = new List<MainspringUserDefinedBaseMeasureModel>();

            DataTable dt = GetTicketSummeryBaseMeasureODC(ProjectID, FrequencyID, ServiceIDs, ReportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                lstResult = dt.AsEnumerable().Select(row => new MainspringUserDefinedBaseMeasureModel
                {

                    ServiceId = Convert.ToInt32(row["ServiceID"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),
                    TicketSummaryBaseId = Convert.ToInt32(row["TicketSummaryBaseID"]),
                    TicketSummaryBaseName = Convert.ToString(row["TicketSummaryBaseName"]),
                    PriorityId = Convert.ToString(row["PRIORITYID"]),
                    MainspringPriorityName = Convert.ToString(row["MainspringPriorityName"]),
                    SupportCategory = Convert.ToString(row["SUPPORTCATEGORY"]),
                    MainspringSupportCategoryName = Convert.ToString(row["MainspringSUPPORTCATEGORYName"]),
                    TicketSummaryValue = Convert.ToString(row["TicketSummaryValue"] == DBNull.Value ? "" :
                    row["TicketSummaryValue"])
                }).ToList();
            }
            return lstResult;
        }
        /// <summary>
        /// This Method Is Used To GetTicketSummeryBaseMeasureODC
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <returns></returns>
        private DataTable GetTicketSummeryBaseMeasureODC(int ProjectID, int? FrequencyID, string ServiceIDs,
            int? ReportFrequencyID)
        {
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            if (FrequencyID != null)
            {
                prms[1] = new SqlParameter("@FrequencyID", FrequencyID);
            }
            else
            {
                prms[1] = new SqlParameter("@FrequencyID", DBNull.Value);
            }
            if (ServiceIDs != null && ServiceIDs.Trim() != "")
            {
                prms[2] = new SqlParameter("@ServiceIDs", ServiceIDs.Trim());
            }
            else
            {
                prms[2] = new SqlParameter("@ServiceIDs", DBNull.Value);
            }

            if (ReportFrequencyID != null)
            {
                prms[3] = new SqlParameter("@ReportFrequencyID", ReportFrequencyID);
            }
            else
            {
                prms[3] = new SqlParameter("@ReportFrequencyID", DBNull.Value);
            }

            dtResult = (new DBHelper()).GetTableFromSP("[MS].[GetTicketSummaryBaseMeasureOdc]", prms, ConnectionString);

            return dtResult;
        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureProgress
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="BaseMeasureSystemDefinedOrUserDefined"></param>
        /// <returns></returns>
        public List<MainspringBaseMeasureProgressModel> GetBaseMeasureProgress(int ProjectID, int? FrequencyID,
            string ServiceIDs, string MetricsIDs, int? ReportFrequencyID, string BaseMeasureSystemDefinedOrUserDefined, string access)
        {
            List<MainspringBaseMeasureProgressModel> lstResult = new List<MainspringBaseMeasureProgressModel>();

            DataTable dt = GetBaseMeasureSearchResult(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                "progress", ReportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                lstResult = dt.AsEnumerable().Select(row => new MainspringBaseMeasureProgressModel
                {
                    ValuesAvailableCount = Convert.ToDecimal(row["ValuesAvailableCount"]),
                    ValuesTotalCount = Convert.ToDecimal(row["ValuesTotalCount"]),
                    ProgressPercentage = Convert.ToDecimal(row["ProgressPercentage"]),
                    BaseMeasureType = Convert.ToString(row["BaseMeasureType"])


                }).ToList();
            }
            if (new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"] == "true")
            {
                if (lstResult[0].ProgressPercentage == 100)
                {
                    List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                    existingAcitivities = new MyActivity().GetExistingActivitys(ProjectID, new AppSettings().AppsSttingsKeyValues["MainspringWorkItemCode"], access);
                    if (existingAcitivities != null && existingAcitivities.Count > 0 && !existingAcitivities[0].IsExpired)
                    {
                        UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                        expiryModel.WorkItemCode = new AppSettings().AppsSttingsKeyValues["MainspringWorkItemCode"];
                        expiryModel.SourceRecordId = ProjectID;
                        expiryModel.ModifiedBy = "System";
                        string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);

                    }
                }
            }

            return lstResult;
        }
        /// <summary>
        /// This Method Is Used To GetBaseMeasureSearchResult
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="RequiredFilterType"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <returns></returns>
        private DataTable GetBaseMeasureSearchResult(int ProjectID, int? FrequencyID, string ServiceIDs,
            string MetricsIDs, string RequiredFilterType, int? ReportFrequencyID)
        {
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            if (FrequencyID != null)
            {
                prms[1] = new SqlParameter("@FrequencyID", FrequencyID);
            }
            else
            {
                prms[1] = new SqlParameter("@FrequencyID", DBNull.Value);
            }
            if (ServiceIDs != null && ServiceIDs.Trim() != "")
            {
                prms[2] = new SqlParameter("@ServiceIDs", ServiceIDs.Trim());
            }
            else
            {
                prms[2] = new SqlParameter("@ServiceIDs", DBNull.Value);
            }
            if (MetricsIDs != null && MetricsIDs.Trim() != "")
            {
                prms[3] = new SqlParameter("@MetricsIDs", MetricsIDs.Trim());
            }
            else
            {
                prms[3] = new SqlParameter("@MetricsIDs", DBNull.Value);
            }
            prms[4] = new SqlParameter("@RequiredSearchType", RequiredFilterType);

            if (ReportFrequencyID != null)
            {
                prms[5] = new SqlParameter("@ReportFrequencyID", ReportFrequencyID);
            }
            else
            {
                prms[5] = new SqlParameter("@ReportFrequencyID", DBNull.Value);
            }

            dtResult = (new DBHelper()).GetTableFromSP("[MS].[GetProjectBaseMeasures]", prms, ConnectionString);

            return dtResult;

        }

        /// <summary>
        /// This Method Is Used To SaveBaseMeasureODC
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="lstBaseMeasureODC"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SaveBaseMeasure(int ProjectID, string UserID,
            List<SearchBaseMeasureParams> lstBaseMeasures)
        {
            bool result = false;
            try
            {
                DataTable dtResult = new DataTable();
                dtResult.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);

                if (UserID != null)
                {
                    prms[1] = new SqlParameter("@UserID", UserID);
                }
                else
                {
                    prms[1] = new SqlParameter("@UserID", DBNull.Value);
                }

                var objCollection = from o in lstBaseMeasures
                                    select new MainspringUDBaseMeasureModel
                                    {
                                        ServiceId = Convert.ToInt64(o.ServiceId),
                                        BaseMeasureId = Convert.ToInt64(o.BaseMeasureId),
                                        BaseMeasureValue = o.BaseMeasureValue == null ? string.Empty : o.BaseMeasureValue
                                    };
                prms[2] = new SqlParameter("@Basemeasuredata", objCollection.ToList().ToDT());
                prms[2].SqlDbType = SqlDbType.Structured;
                prms[2].TypeName = "MS.BaseMeasureData_TVP";
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[MS].[SaveProjectBaseMeasures]", prms, ConnectionString);
                result = Convert.ToBoolean(ds.Tables[0].Rows[0]["Result"]);
            }
            catch (Exception ex)
            {
                new ExceptionUtility().LogExceptionMessage(ex);
            }

            return result;

        }
        /// <summary>
        /// This Method Is Used To SaveTicketSummaryBaseMeasureODC
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="lstTicketSummaryBaseODC"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SaveTicketSummaryBaseMeasureODC(int ProjectID, int? FrequencyID, int? ReportFrequencyID,
            List<MainspringTicketSummaryBaseMeasureModel> lstTicketSummaryBaseODC, string UserId)
        {
            bool result = false;
            try
            {
                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                if (FrequencyID != null)
                {
                    prms[1] = new SqlParameter("@FrequencyID", FrequencyID);
                }
                else
                {
                    prms[1] = new SqlParameter("@FrequencyID", DBNull.Value);
                }

                if (ReportFrequencyID != null)
                {
                    prms[2] = new SqlParameter("@ReportFrequencyID", ReportFrequencyID);
                }
                else
                {
                    prms[2] = new SqlParameter("@ReportFrequencyID", DBNull.Value);
                }

                if (UserId != null)
                {
                    prms[3] = new SqlParameter("@UserID", UserId);
                }
                else
                {
                    prms[3] = new SqlParameter("@UserID", DBNull.Value);
                }
                var objCollection = from o in lstTicketSummaryBaseODC
                                    select new MainspringTicketSummaryBaseMeasureModel
                                    {
                                        ServiceId = o.ServiceId,
                                        TicketSummaryBaseMeasureId = o.TicketSummaryBaseMeasureId,
                                        MainspringPriorityId = o.MainspringPriorityId,
                                        MainspringSupportCategoryId = o.MainspringSupportCategoryId,
                                        TicketBaseMeasureValue = o.TicketBaseMeasureValue == null ? string.Empty : o.TicketBaseMeasureValue
                                    };
                prms[4] = new SqlParameter("@GridDetails", objCollection.ToList().ToDT());
                prms[4].SqlDbType = SqlDbType.Structured;
                prms[4].TypeName = "MS.TicketSummaryBaseMeasureOdc_TVP";

                (new DBHelper()).ExecuteNonQuery("MS.SaveTicketSummaryBaseMeasureODC", prms, ConnectionString);

                result = true;
            }
            catch (Exception ex)
            {
                new ExceptionUtility().LogExceptionMessage(ex);
            }

            return result;

        }
        /// <summary>
        /// BaseMeasureCollection
        /// </summary>
        public class BaseMeasureCollection : List<MainspringUserDefinedBaseMeasureModel>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("ServiceID", SqlDbType.Int),
                      new SqlMetaData("BaseMeasureID", SqlDbType.Int),
                      new SqlMetaData("BaseMeasureValue", SqlDbType.VarChar, 150));
                foreach (MainspringUserDefinedBaseMeasureModel obj in this)
                {
                    sqlRow.SetInt32(0, obj.ServiceId);
                    sqlRow.SetInt32(1, obj.BaseMeasureId);
                    sqlRow.SetString(2, obj.BaseMeasureValue != null ? obj.BaseMeasureValue : "");
                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// This Method Is Used To BaseMeasureODCCollection
        /// </summary>
        public class BaseMeasureODCCollection : List<MainspringUserDefinedBaseMeasureModel>,
            IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("ServiceID", SqlDbType.Int),
                      new SqlMetaData("BaseMeasureID", SqlDbType.Int),
                      new SqlMetaData("PRIORITYID", SqlDbType.VarChar, 25),
                      new SqlMetaData("SUPPORTCATEGORYID", SqlDbType.VarChar, 50),
                      new SqlMetaData("TECHNOLOGY", SqlDbType.VarChar, 50),
                      new SqlMetaData("BaseMeasureValue", SqlDbType.VarChar, 150));
                foreach (MainspringUserDefinedBaseMeasureModel obj in this)
                {
                    sqlRow.SetInt32(0, obj.ServiceId);
                    sqlRow.SetInt32(1, obj.BaseMeasureId);
                    if (obj.PriorityId != null)
                    {
                        sqlRow.SetString(2, obj.PriorityId);
                    }
                    else
                    {
                        sqlRow.SetString(2, "");
                    }

                    if (obj.SupportCategory != null)
                    {
                        sqlRow.SetString(3, obj.SupportCategory);
                    }
                    else
                    {
                        sqlRow.SetString(3, "");
                    }
                    if (obj.Technology != null)
                    {
                        sqlRow.SetString(4, obj.Technology);
                    }
                    else
                    {
                        sqlRow.SetString(4, "");
                    }

                    sqlRow.SetString(5, obj.BaseMeasureValue != null ? obj.BaseMeasureValue : "");
                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// TicketBaseMeasureCollection
        /// </summary>
        public class TicketBaseMeasureCollection : List<MainspringTicketSummaryBaseMeasureModel>,
            IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("ServiceID", SqlDbType.Int),
                      new SqlMetaData("TicketSummaryBaseMeasureID", SqlDbType.Int),
                      new SqlMetaData("MainspringPriorityID", SqlDbType.VarChar, 25),
                      new SqlMetaData("MainspringSUPPORTCATEGORYID", SqlDbType.VarChar, 50),
                      new SqlMetaData("TicketBaseMeasureValue", SqlDbType.VarChar, 150));
                foreach (MainspringTicketSummaryBaseMeasureModel obj in this)
                {
                    sqlRow.SetInt32(0, obj.ServiceId);
                    sqlRow.SetInt32(1, obj.TicketSummaryBaseMeasureId);
                    if (obj.MainspringPriorityId != null)
                    {
                        sqlRow.SetString(2, obj.MainspringPriorityId);
                    }
                    else
                    {
                        sqlRow.SetString(2, "");
                    }

                    if (obj.MainspringSupportCategoryId != null)
                    {
                        sqlRow.SetString(3, obj.MainspringSupportCategoryId);
                    }
                    else
                    {
                        sqlRow.SetString(3, "");
                    }

                    sqlRow.SetString(4, obj.TicketBaseMeasureValue != null ? obj.TicketBaseMeasureValue : "");
                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// Base Measure Report List Service Wise 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="BaseMeasureSystemDefinedOrUserDefined"></param>
        /// <returns></returns>
        public List<BaseMeasureReportModel> BaseMeasureReportList(int ProjectID, int? FrequencyID, string ServiceIDs,
            string MetricsIDs, int? ReportFrequencyID, string BaseMeasureSystemDefinedOrUserDefined)
        {
            List<BaseMeasureReportModel> reportList = new List<BaseMeasureReportModel>();

            DataTable dt = GetBaseMeasureServiceWiseReport(ProjectID, FrequencyID,
                BaseMeasureSystemDefinedOrUserDefined, ReportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                reportList = dt.AsEnumerable().Select(row => new BaseMeasureReportModel
                {
                    ProjectId = Convert.ToInt32(row["ProjectID"]),
                    MetricStartDate = Convert.ToString(row["MetricStartDate"]),
                    MetricEndDate = Convert.ToString(row["MetricEndDate"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),
                    MetricName = Convert.ToString(row["MetricName"]),
                    MetricTypeDesc = Convert.ToString(row["MetricTypeDesc"]),
                    MainspringPriorityName = Convert.ToString(row["MainspringPriorityName"]),
                    MainspringSupportCategoryName = Convert.ToString(row["MainspringSUPPORTCATEGORYName"]),
                    TechnologyLanguageNameShortDesc = Convert.ToString(row["TechnologyLanguageNameShortDESC"]),
                    BaseMeasureName = Convert.ToString(row["BaseMeasureName"]),
                    BaseMeasureValue = Convert.ToString(row["BaseMeasureValue"]),
                    ServiceId = Convert.ToInt32(row["ServiceID"]),
                    MetricId = Convert.ToInt32(row["MetricID"]),
                    BaseMeasureId = Convert.ToInt32(row["BaseMeasureID"])


                }).ToList();
            }

            return reportList;
        }


        /// <summary>
        /// Ticket Summary Base Measure Report List
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="frequencyID"></param>
        /// <param name="serviceIDs"></param>
        /// <param name="reportFrequencyID"></param>
        /// <param name="baseMeasureType"></param>
        /// <returns></returns>
        public List<BaseMeasureTicketReportModel> TicketSummaryBMReportList(int projectID, int? frequencyID,
            string serviceIDs, int? reportFrequencyID, string baseMeasureType)
        {
            List<BaseMeasureTicketReportModel> reportList = new List<BaseMeasureTicketReportModel>();

            DataTable dt = GetBaseMeasureServiceWiseReport(projectID, frequencyID, baseMeasureType,
                reportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                reportList = dt.AsEnumerable().Select(row => new BaseMeasureTicketReportModel
                {
                    ProjectId = Convert.ToString(row["ProjectID"]),
                    MetricStartDate = Convert.ToString(row["MetricStartDate"]),
                    MetricEndDate = Convert.ToString(row["MetricEndDate"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),
                    MainspringPriorityName = Convert.ToString(row["MainspringPriorityName"]),
                    MainspringSupportCategoryName = Convert.ToString(row["MainspringSUPPORTCATEGORYName"]),
                    TicketSummaryBaseName = Convert.ToString(row["TicketSummaryBaseName"]),
                    TicketSummaryValue = Convert.ToString(row["TicketSummaryValue"]),
                    ServiceId = Convert.ToString(row["ServiceID"]),
                    TicketSummaryBaseId = Convert.ToString(row["TicketSummaryBaseID"])

                }).ToList();
            }

            return reportList;
        }

        /// <summary>
        /// GetBaseMeasure Projectwise SearchUser DefinedList
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="ServiceIDs"></param>
        /// <param name="MetricsIDs"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <param name="BaseMeasureSystemDefinedOrUserDefined"></param>
        /// <returns></returns>

        public IList<SearchBaseMeasureParams> GetBaseMeasureProjectwiseSearch(int ProjectID,
     int? FrequencyID, string ServiceIDs, string MetricsIDs, int? ReportFrequencyID,
     string BaseMeasureSystemDefinedOrUserDefined)
        {
            IList<SearchBaseMeasureParams> lstResult = new List<SearchBaseMeasureParams>();

            DataTable dt = GetBaseMeasureSearchResult(ProjectID, FrequencyID, ServiceIDs, MetricsIDs,
                BaseMeasureSystemDefinedOrUserDefined, ReportFrequencyID);
            if (dt != null && dt.Rows.Count > 0)
            {
                lstResult = dt.AsEnumerable().Select(row => new SearchBaseMeasureParams
                {
                    ServiceId = Convert.ToInt32(row["ServiceID"]),
                    ServiceName = Convert.ToString(row["ServiceName"]),
                    BaseMeasureId = Convert.ToInt32(row["BaseMeasureID"]),
                    BaseMeasureName = Convert.ToString(row["BaseMeasureName"]),
                    UOMId = Convert.ToInt32(row["UOMID"]),
                    UOMDesc = Convert.ToString(row["UOMDESC"]),
                    UOMDataType = Convert.ToString(row["UOMDataType"]),
                    BaseMeasureValue = Convert.ToString(row["BaseMeasureValue"]),
                    BaseMeasureTypeId = Convert.ToInt32(row["BaseMeasureTypeID"]),
                    MetricId = Convert.ToInt32(row["MetricID"]),
                    MetricName = Convert.ToString(row["MetricName"]),
                    ServiceMetricBaseMeasureId = Convert.ToInt32(row["ServiceMetricBaseMeasureId"]),
                    ReportPeriodID = Convert.ToInt32(row["ReportingPeriod"])
                }).ToList();
            }

            return lstResult;
        }
        #region Private Methods

        /// <summary>
        /// Get Base Measure Report  ServiceWise
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FrequencyID"></param>
        /// <param name="RequiredFilterType"></param>
        /// <param name="ReportFrequencyID"></param>
        /// <returns></returns>
        private DataTable GetBaseMeasureServiceWiseReport(int ProjectID, int? FrequencyID, string RequiredFilterType,
            int? ReportFrequencyID)
        {
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            if (FrequencyID != null)
            {
                prms[1] = new SqlParameter("@FrequencyID", FrequencyID);
            }
            else
            {
                prms[1] = new SqlParameter("@FrequencyID", DBNull.Value);
            }
            if (ReportFrequencyID != null)
            {
                prms[2] = new SqlParameter("@ReportPeriod", ReportFrequencyID);
            }
            else
            {
                prms[2] = new SqlParameter("@ReportPeriod", DBNull.Value);
            }
            prms[3] = new SqlParameter("@RequiredSearchType", RequiredFilterType);

            dtResult = (new DBHelper()).GetTableFromSP("[MS].[GetBaseMeasureServiceWiseReport]", prms, ConnectionString);

            return dtResult;

        }

        #endregion
    }
}