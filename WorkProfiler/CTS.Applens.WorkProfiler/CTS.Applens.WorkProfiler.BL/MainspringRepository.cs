using CTS.Applens.WorkProfiler.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using MainSpring = CTS.Applens.WorkProfiler.DAL.Mainspring.MainspringRepository;
namespace CTS.Applens.WorkProfiler.Repository.Mainspring
{
    public class MainspringRepository
    {
        /// <summary>
        /// This Method Is Used To GetMainspringProjectDetails
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<MainspringProjectModel> GetMainspringProjectDetails(int customerID, string employeeID)
        {
            try
            {
                return new MainSpring().GetMainspringProjectDetails(customerID, employeeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetBaseMeasureFiltermainspringavailability
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public MainspringProjectModel GetBaseMeasureFiltermainspringavailability(int ProjectID)
        {
            try
            {
                return new MainSpring().GetBaseMeasureFilterData(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetBaseMeasureLoadFactorProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="MetricName"></param>
        /// <returns></returns>
        public bool GetBaseMeasureLoadFactorProject(string ProjectID, string MetricName)
        {
            try
            {
                return new MainSpring().GetBaseMeasureLoadFactorProject(ProjectID, MetricName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().GetBaseMeasureValueLoadFactor(ProjectID, MetricName, ReportPeriodID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().SaveLoadFactor(ProjectID, MetricName, ReportPeriodID, LoadFactor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        /// <summary>
        /// Get Ticket Summary Filter Service List
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ServiceFilter"></param>
        /// <returns></returns>
        public List<MainspringServiceListModel> GetTicketSummaryFilterServiceList(int ProjectID, int ServiceFilter)
        {
            try
            {
                return new MainSpring().GetTicketSummaryFilterServiceList(ProjectID, ServiceFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().GetTicketSummeryBaseMeasureODCList(ProjectID, FrequencyID,
                    ServiceIDs, ReportFrequencyID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().GetBaseMeasureProgress(ProjectID, FrequencyID,
                    ServiceIDs, MetricsIDs, ReportFrequencyID, BaseMeasureSystemDefinedOrUserDefined, access);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public bool SaveBaseMeasure(int ProjectID,string UserID,
           List<SearchBaseMeasureParams> lstBaseMeasures)
        {
            try
            {
                return new MainSpring().SaveBaseMeasure(ProjectID,UserID,lstBaseMeasures);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().SaveTicketSummaryBaseMeasureODC(ProjectID, FrequencyID,
                    ReportFrequencyID, lstTicketSummaryBaseODC, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().BaseMeasureReportList(ProjectID, FrequencyID,
                    ServiceIDs, MetricsIDs, ReportFrequencyID, BaseMeasureSystemDefinedOrUserDefined);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return new MainSpring().TicketSummaryBMReportList(projectID, frequencyID,
                    serviceIDs, reportFrequencyID, baseMeasureType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}