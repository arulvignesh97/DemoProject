using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Common;
using CTS.Applens.WorkProfiler.Common.Extensions;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ManagedDataSets;
using CTS.Applens.WorkProfiler.Models.ServiceClassification;
using CTS.Applens.WorkProfiler.Models.Work_Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Net.Http.Headers;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.DAL
{
    public class TicketingModuleRepository : DBContext
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
            TimeSheetModel objTimeSheetModel = new TimeSheetModel();

            try
            {
                if (CustomerID != null)
                {
                    DataSet ds = new DataSet();
                    ds.Locale = CultureInfo.InvariantCulture;
                    DataSet dsMaster = new DataSet();
                    dsMaster.Locale = CultureInfo.InvariantCulture;
                    DataSet dsServiceBenchMark = new DataSet();
                    dsServiceBenchMark.Locale = CultureInfo.InvariantCulture;
                    DataSet dsADMTimesheetDetails = new DataSet();
                    dsADMTimesheetDetails.Locale = CultureInfo.InvariantCulture;
                    if (Mode == "ChooseTicket")
                    {
                        DataTable dtTicketlist = ToDataTable(TicketList);
                        SqlParameter[] prmsCT = new SqlParameter[6];
                        prmsCT[0] = new SqlParameter("@TicketList", dtTicketlist);
                        prmsCT[0].SqlDbType = SqlDbType.Structured;
                        prmsCT[0].TypeName = "Tvp_TicketIDSupportTypeDetails";
                        prmsCT[1] = new SqlParameter("@ProjectID", ProjectID);
                        prmsCT[2] = new SqlParameter("@EmployeeID", EmployeeID);
                        prmsCT[3] = new SqlParameter("@FirstDateOfWeek", DateTime.ParseExact(FirstDateOfWeek,
                            "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        prmsCT[4] = new SqlParameter("@LastDateOfWeek", DateTime.ParseExact(LastDateOfWeek,
                            "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        prmsCT[5] = new SqlParameter("@CustomerID", CustomerID);
                        ds = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_GetTicketbasedonTicketNumberWeekly]",
                            prmsCT, ConnectionString);
                    }
                    else
                    {
                        SqlParameter[] prms = new SqlParameter[4];
                        prms[0] = new SqlParameter("@CustomerID", CustomerID);
                        prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                        prms[2] = new SqlParameter("@FirstDateOfWeek", DateTime.ParseExact(FirstDateOfWeek,
                            "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        prms[3] = new SqlParameter("@LastDateOfWeek", DateTime.ParseExact(LastDateOfWeek,
                            "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));

                        ds = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_GetWeeklyTicketDetails]", prms, ConnectionString);

                    }
                    dsMaster = GetMasterValuesForTimeSheet(CustomerID, EmployeeID, FirstDateOfWeek, LastDateOfWeek);
                    if (isCognizant == 1)
                    {
                        dsServiceBenchMark = GetServiceBenchMarkForTimeSheet(CustomerID, EmployeeID);
                    }
                    dsADMTimesheetDetails = GetADMDetails(Convert.ToInt64(CustomerID), EmployeeID);
                    objTimeSheetModel = BindTimesSheetInfoFromDataSet(ds, dsMaster, dsServiceBenchMark,
                        dsADMTimesheetDetails);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeSheetModel;
        }

        /// <summary>
        /// Method to create Data Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns>Methos returns table</returns>

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// This Method Is Used To GetTicketIdByCustomerID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GetTicketIdByCustomerID(Int64 CustomerID)
        {

            string ticketid = "";
            string tktid = "";
            int ticketidnew = 0;
            int index = -1;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@CustomerID", CustomerID);
            DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetTicketIdByAccount]", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ticketid = Convert.ToString((dt.Rows[i]["TicketID"]) == DBNull.Value ? "0" :
                        dt.Rows[i]["TicketID"]);
                }
            }
            return ticketid;
        }
        /// <summary>
        /// This Method Is Used To BindTimesheetInfo
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public TimeSheetModel BindTimesheetInfo(DataSet ds)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            TimeSheetModel objTimeSheetModel = new TimeSheetModel();
            DataTable dtWeekDays = new DataTable();
            dtWeekDays.Locale = CultureInfo.InvariantCulture;
            DataTable dtTimeSheetDetails = new DataTable();
            dtTimeSheetDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTicketDetails = new DataTable();
            dtTicketDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtServiceActivityDetails = new DataTable();
            dtServiceActivityDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTicketTypeDetails = new DataTable();
            dtTicketTypeDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtStatusDetails = new DataTable();
            dtStatusDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtCustomerDetails = new DataTable();
            dtCustomerDetails.Locale = CultureInfo.InvariantCulture;

            WeekDays weekdays = new WeekDays();
            OverallTicketDetails objOverallTicketDetails = new OverallTicketDetails();
            TimeSheetDetails objTimeSheetDetails = new TimeSheetDetails();
            CustomerDetails objCustomerDetails = new CustomerDetails();
            try
            {
                dtTicketDetails = ds.Tables[0];
                dtTimeSheetDetails = ds.Tables[1];
                dtWeekDays = ds.Tables[2];
                dtServiceActivityDetails = ds.Tables[3];
                dtTicketTypeDetails = ds.Tables[4];
                dtStatusDetails = ds.Tables[5];
                dtCustomerDetails = ds.Tables[6];

                if (dtWeekDays != null && dtWeekDays.Rows.Count > 0)
                {
                    objTimeSheetModel.LstWeekDays = new List<WeekDays>();

                    objTimeSheetModel.LstWeekDays = dtWeekDays.AsEnumerable().Select(x => new WeekDays
                    {
                        Date = x["DATETODAY"] != DBNull.Value ? Convert.ToString(x["DATETODAY"]) : "",
                        Day = x["NAME"] != DBNull.Value ? x["NAME"].ToString() : string.Empty,
                        DisplayDate = x["DisplayDate"] != DBNull.Value ? Convert.ToString(x["DisplayDate"]) : "",
                        FreezeStatus = x["FreezeStatus"] != DBNull.Value ? Convert.ToString(x["FreezeStatus"]) : "",
                        StatusId = x["StatusID"] != DBNull.Value ? Convert.ToInt32(x["StatusID"]) : 0
                    }).ToList();



                    if (dtServiceActivityDetails != null && dtServiceActivityDetails.Rows.Count > 0 &&
                        dtServiceActivityDetails.Columns.Contains("CustomerID"))
                    {
                        objTimeSheetModel.LstServiceActivityDetails = dtServiceActivityDetails.AsEnumerable().
                            Select(x => new ServiceActivityDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                                ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt64(x["ServiceID"]) : 0,
                                ServiceName = x["ServiceName"] != DBNull.Value ? x["ServiceName"].
                            ToString() : string.Empty,
                                ServiceTypeId = x["ServiceTypeID"] != DBNull.Value ? Convert.
                            ToInt64(x["ServiceTypeID"]) : 0,
                                ActivityId = x["ActivityID"] != DBNull.Value ? Convert.ToInt64(x["ActivityID"]) : 0,
                                ActivityName = x["ActivityName"] != DBNull.Value ? x["ActivityName"].
                            ToString() : string.Empty,
                                ServiceLevelId = x["ServiceLevelID"] != DBNull.Value ? Convert.
                            ToInt32(x["ServiceLevelID"]) : 0,
                            }).ToList();
                    }

                    if (dtServiceActivityDetails != null && dtServiceActivityDetails.Rows.Count > 0 &&
                        dtServiceActivityDetails.Columns.Contains("CustomerID"))
                    {
                        objTimeSheetModel.LstTicketTypeDetails = dtTicketTypeDetails.AsEnumerable().
                            Select(x => new TicketTypeDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                                AVMTicketType = x["AVMTicketType"] != DBNull.Value ? Convert.
                            ToInt64(x["AVMTicketType"]) : 0,
                                TicketTypeId = x["TicketTypeID"] != DBNull.Value ? Convert.
                            ToInt64(x["TicketTypeID"]) : 0,
                                TicketTypeMappingId = x["TicketTypeMappingID"] != DBNull.Value ? Convert.
                            ToInt64(x["TicketTypeMappingID"]) : 0,
                                TicketTypeName = x["TicketTypeName"] != DBNull.Value ? x["TicketTypeName"].
                            ToString() : string.Empty,
                                TicketType = x["TicketType"] != DBNull.Value ? x["TicketType"].
                            ToString() : string.Empty,
                            }).ToList();
                    }
                    if (dtStatusDetails != null && dtStatusDetails.Rows.Count > 0 && dtStatusDetails.
                        Columns.Contains("CustomerID"))
                    {
                        objTimeSheetModel.LstStatusDetails = dtStatusDetails.AsEnumerable().
                            Select(x => new StatusDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,

                                DARTStatusId = x["DARTStatusID"] != DBNull.Value ? Convert.
                            ToInt64(x["DARTStatusID"]) : 0,
                                DARTStatusName = x["DARTStatusName"] != DBNull.Value ? x["DARTStatusName"].
                            ToString() : string.Empty,
                                TicketStatusId = x["TicketStatus_ID"] != DBNull.Value ? Convert.
                            ToInt64(x["TicketStatus_ID"]) : 0,
                                StatusName = x["StatusName"] != DBNull.Value ? x["StatusName"].
                            ToString() : string.Empty,

                            }).ToList();

                    }
                    objTimeSheetModel.LstOverallTicketDetails = dtTicketDetails.AsEnumerable().
                        Select(x => new OverallTicketDetails
                        {
                            TimeTickerId = x["TimeTickerID"] != DBNull.Value ? Convert.ToInt64(x["TimeTickerID"]) : 0,
                            TicketId = x["TicketID"] != DBNull.Value ? x["TicketID"].ToString() : string.Empty,
                            ApplicationId = x["ApplicationID"] != DBNull.Value ? Convert.ToInt64(x["ApplicationID"])
                            : 0,
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                            AssignedTo = x["AssignedTo"] != DBNull.Value ? x["AssignedTo"].ToString() : string.Empty,
                            EffortTillDate = x["EffortTillDate"] != DBNull.Value ? x["EffortTillDate"].
                        ToString() : string.Empty,
                            ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt64(x["ServiceID"]) : 0,
                            ActivityId = x["ActivityID"] != DBNull.Value ? Convert.ToInt64(x["ActivityID"]) : 0,
                            TicketDescription = x["TicketDescription"] != DBNull.Value ? x["TicketDescription"].
                        ToString() : string.Empty,
                            IsDeleted = x["IsDeleted"] != DBNull.Value ? Convert.ToInt32(x["IsDeleted"]) : 0,
                            TicketStatusMapId = x["TicketStatusMapID"] != DBNull.Value ? Convert.ToInt64
                            (x["TicketStatusMapID"]) : 0,
                            TicketTypeMapId = x["TicketTypeMapID"] != DBNull.Value ?
                            Convert.ToInt64(x["TicketTypeMapID"]) : 0,
                            IsSDTicket = x["IsSDTicket"] != DBNull.Value ? x["IsSDTicket"].ToString() : string.Empty,
                            DARTStatusId = x["DARTStatusID"] != DBNull.Value ? Convert.ToInt64(x["DARTStatusID"]) : 0,
                            ITSMEffort = x["ITSMEffort"] != DBNull.Value ? x["ITSMEffort"].ToString() : string.Empty,
                            IsNonTicket = x["IsNonTicket"] != DBNull.Value ? x["IsNonTicket"].ToString() :
                            string.Empty,
                            IsCognizant = x["IsCustomer"] != DBNull.Value ? x["IsCustomer"].ToString() : string.Empty,
                            IsEffortTracked = x["IsEfforTracked"] != DBNull.Value ? x["IsEfforTracked"].
                        ToString() : string.Empty,
                            IsDebtEnabled = x["IsDebtEnabled"] != DBNull.Value ? x["IsDebtEnabled"].
                        ToString() : string.Empty,
                            IsMainspringConfigured = x["IsMainspringConfigured"] != DBNull.Value ?
                        x["IsMainspringConfigured"].ToString() : string.Empty,
                            ISTicket = x["ISTicket"] != DBNull.Value ? x["ISTicket"].ToString() : string.Empty,
                            IsAttributeUpdated = x["IsAttributeUpdated"] != DBNull.Value ? Convert.
                        ToInt32(x["IsAttributeUpdated"]) : 0,
                            TimeSheetDetailId1 = x["1TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["1TimeSheetDetailId"]) : 0,
                            Effort1 = x["1"] != DBNull.Value ? x["1"].ToString() : string.Empty,
                            TimeSheetDetailId2 = x["2TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["2TimeSheetDetailId"]) : 0,
                            Effort2 = x["2"] != DBNull.Value ? x["2"].ToString() : string.Empty,
                            TimeSheetDetailId3 = x["3TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["3TimeSheetDetailId"]) : 0,
                            Effort3 = x["3"] != DBNull.Value ? x["3"].ToString() : string.Empty,
                            TimeSheetDetailId4 = x["4TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["4TimeSheetDetailId"]) : 0,
                            Effort4 = x["4"] != DBNull.Value ? x["4"].ToString() : string.Empty,
                            TimeSheetDetailId5 = x["5TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["5TimeSheetDetailId"]) : 0,
                            Effort5 = x["5"] != DBNull.Value ? x["5"].ToString() : string.Empty,
                            TimeSheetDetailId6 = x["6TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["6TimeSheetDetailId"]) : 0,
                            Effort6 = x["6"] != DBNull.Value ? x["6"].ToString() : string.Empty,
                            TimeSheetDetailId7 = x["7TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["7TimeSheetDetailId"]) : 0,
                            Effort7 = x["7"] != DBNull.Value ? x["7"].ToString() : string.Empty,
                            TowerId = x["TowerID"] != DBNull.Value ? Convert.ToInt16(x["TowerID"]) : 0,
                            SupportTypeId
                            = x["SupportTypeID"] != DBNull.Value ? Convert.ToInt16(x["SupportTypeID"]) : 0,
                            SuggestedActivityName = x["SuggestedActivityName"] != DBNull.Value
                           ? x["SuggestedActivityName"].ToString() : string.Empty,

                        }).ToList();


                    if (dtTimeSheetDetails.Columns.Contains("SNO"))
                    {
                        objTimeSheetModel.LstTimeSheetDetails = dtTimeSheetDetails.AsEnumerable().
                            Select(x => new TimeSheetDetails
                            {
                                No = x["SNO"] != DBNull.Value ? Convert.ToInt32(x["SNO"]) : 0,
                                TimeSheetId = x["TimeSheetId"] != DBNull.Value ? Convert.ToInt64(x["TimeSheetId"]) : 0,
                                TimeSheetDate = x["TimeSheetDate"] != DBNull.Value ? Convert.
                            ToString(x["TimeSheetDate"]) : "",
                                TimeSheetDetailId = x["TimeSheetDetailId"] != DBNull.Value ? Convert.
                            ToInt64(x["TimeSheetDetailId"]) : 0,
                                FreezeStatus = x["FreezStatus"] != DBNull.Value ? Convert.
                            ToBoolean(x["FreezStatus"]) : false,
                            }).ToList();
                    }
                    if (dtTimeSheetDetails.Columns.Contains("SNO"))
                    {
                        objTimeSheetModel.LstTimeSheetDetails = dtTimeSheetDetails.AsEnumerable().
                            Select(x => new TimeSheetDetails
                            {
                                No = x["SNO"] != DBNull.Value ? Convert.ToInt64(x["SNO"]) : 0,
                                TimeSheetId = x["TimeSheetId"] != DBNull.Value ? Convert.
                            ToInt64(x["TimeSheetId"]) : 0,
                                TimeSheetDate = x["TimeSheetDate"] != DBNull.Value ? Convert.
                            ToString(x["TimeSheetDate"]) : "",
                                TimeSheetDetailId = x["TimeSheetDetailId"] != DBNull.Value ?
                            Convert.ToInt64(x["TimeSheetDetailId"]) : 0,
                                FreezeStatus = x["FreezStatus"] != DBNull.Value ?
                            Convert.ToBoolean(x["FreezStatus"]) : false,
                            }).ToList();
                    }

                    objTimeSheetModel.LstCustomerDetails = dtCustomerDetails.AsEnumerable().
                        Select(x => new CustomerDetails
                        {
                            CustomerId = x["CustomerId"] != DBNull.Value ? Convert.
                        ToInt64(x["CustomerId"]) : 0,
                            IsCognizant = x["IsCustomer"] != DBNull.Value ? Convert.
                        ToInt32(x["IsCustomer"]) : 0,
                            IsEffortTracked = x["IsEffortTracked"] != DBNull.Value ?
                        Convert.ToInt32(x["IsEffortTracked"]) : 0,
                            IsDaily = x["IsDaily"] != DBNull.Value ? Convert.ToInt32(x["IsDaily"]) : 0
                        }).ToList();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeSheetModel;


        }
        /// <summary>
        /// This Method Is Used To BindTimesSheetInfoFromDataSet
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public TimeSheetModel BindTimesSheetInfoFromDataSet(DataSet ds, DataSet dsMaster,
            DataSet dsServiceBenchMark, DataSet dsADMDetails)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();

            TimeSheetModel objTimeSheetModel = new TimeSheetModel();
            DataTable dtWeekDays = new DataTable();
            dtWeekDays.Locale = CultureInfo.InvariantCulture;
            DataTable dtTimeSheetDetails = new DataTable();
            dtTimeSheetDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTicketDetails = new DataTable();
            dtTicketDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtServiceActivityDetails = new DataTable();
            dtServiceActivityDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTicketTypeDetails = new DataTable();
            dtTicketTypeDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtStatusDetails = new DataTable();
            dtStatusDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtCustomerDetails = new DataTable();
            dtCustomerDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTicketTypeServiceDetails = new DataTable();
            dtTicketTypeServiceDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtUserLevelDetails = new DataTable();
            dtUserLevelDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtTowerDetails = new DataTable();
            dtTowerDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtApplicableServices = new DataTable();
            dtApplicableServices.Locale = CultureInfo.InvariantCulture;
            DataTable dtBUBenchMark = new DataTable();
            dtBUBenchMark.Locale = CultureInfo.InvariantCulture;
            DataTable dtOrgBenchMark = new DataTable();
            dtOrgBenchMark.Locale = CultureInfo.InvariantCulture;
            DataTable dtADProjectStatusDetails = new DataTable();
            dtADProjectStatusDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtADMasterStatusDetails = new DataTable();
            dtADMasterStatusDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtALMConfiguredDetails = new DataTable();
            dtALMConfiguredDetails.Locale = CultureInfo.InvariantCulture;
            DataTable dtScopeDetails = new DataTable();
            dtScopeDetails.Locale = CultureInfo.InvariantCulture;
            WeekDays weekdays = new WeekDays();
            OverallTicketDetails objOverallTicketDetails = new OverallTicketDetails();
            TimeSheetDetails objTimeSheetDetails = new TimeSheetDetails();
            CustomerDetails objCustomerDetails = new CustomerDetails();
            TicketTypeServiceDetails objTicketTypeServiceDetails = new TicketTypeServiceDetails();
            UserLevelDetails objUserLevelDetails = new UserLevelDetails();
            TaskDetails objTowerDetails = new TaskDetails();
            List<BenchMarkDetailsModel> LstBUBenchMarkDetails = new List<BenchMarkDetailsModel>();
            List<BenchMarkDetailsModel> LstOrgBenchMarkDetails = new List<BenchMarkDetailsModel>();
            StatusDetails objStatusDetails = new StatusDetails();
            List<ALMConfiguredDetails> LstALMConfiguredDetails = new List<ALMConfiguredDetails>();
            List<int> LstScopeIds = new List<int>();
            try
            {
                dtTicketDetails = ds.Tables[0];
                dtTimeSheetDetails = ds.Tables[1];
                dtWeekDays = ds.Tables[2];
                if (dsMaster != null && dsMaster.Tables.Count > 0)
                {
                    dtServiceActivityDetails = dsMaster.Tables[0];
                    dtTicketTypeDetails = dsMaster.Tables[1];
                    dtStatusDetails = dsMaster.Tables[2];
                    dtCustomerDetails = dsMaster.Tables[3];
                    dtTicketTypeServiceDetails = dsMaster.Tables[4];
                    dtUserLevelDetails = dsMaster.Tables[5];
                    dtTowerDetails = dsMaster.Tables[6];

                }
                if (dsADMDetails != null && dsADMDetails.Tables.Count > 0)
                {
                    if (dsADMDetails.Tables[0].Rows.Count > 0)
                    {
                        dtADProjectStatusDetails = dsADMDetails.Tables[0];
                        dtADMasterStatusDetails = dsADMDetails.Tables[1];
                        objTimeSheetModel.LstADMStatusDetails = dtADProjectStatusDetails.AsEnumerable().Select(
                        x => new StatusDetails
                        {
                            CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                            DARTStatusId = x["StatusId"] != DBNull.Value ? Convert.ToInt64(x["StatusId"]) : 0,
                            TicketStatusId = x["StatusMapId"] != DBNull.Value ? Convert.
                            ToInt64(x["StatusMapId"]) : 0,
                            StatusName = x["ProjectStatusName"] != DBNull.Value ? x["ProjectStatusName"].ToString() : string.Empty
                        }).ToList();

                    }
                    if (dsADMDetails.Tables[1] != null && dsADMDetails.Tables[1].Rows.Count > 0)
                    {
                        dtADMasterStatusDetails = dsADMDetails.Tables[1];
                        objTimeSheetModel.LstADMMasterStatusDetails = dtADMasterStatusDetails.AsEnumerable().Select(
                        x => new NameIDModel
                        {
                            Id = x["ID"] != DBNull.Value ? Convert.ToInt64(x["ID"]) : 0,
                            Name = x["Name"] != DBNull.Value ? x["Name"].ToString() : string.Empty
                        }).ToList();

                    }
                    if (dsADMDetails.Tables[2] != null && dsADMDetails.Tables[2].Rows.Count > 0)
                    {
                        dtALMConfiguredDetails = dsADMDetails.Tables[2];
                        objTimeSheetModel.LstALMConfiguredDetails = dtALMConfiguredDetails.AsEnumerable().Select(
                        x => new ALMConfiguredDetails
                        {
                            ProjectId = x["ProjectId"] != DBNull.Value ? Convert.ToInt64(x["ProjectId"]) : 0,
                            IsALMToolConfigured = x["IsApplensAsALM"] != DBNull.Value ?
                                    Convert.ToBoolean(x["IsApplensAsALM"]) : false,
                        }).ToList();
                    }
                }
                if (dsServiceBenchMark != null && dsServiceBenchMark.Tables.Count > 0)
                {
                    dtApplicableServices = dsServiceBenchMark.Tables[0];
                    dtOrgBenchMark = dsServiceBenchMark.Tables[1];
                    dtBUBenchMark = dsServiceBenchMark.Tables[2];

                    objTimeSheetModel.LstApplicableServices = dtApplicableServices.AsEnumerable()
                                                       .Select(r => r.Field<int>("ServiceID"))
                                                       .ToList();
                    objTimeSheetModel.LstBUBenchMarkDetails = dtBUBenchMark.AsEnumerable().Select(
                                                x => new BenchMarkDetailsModel
                                                {
                                                    ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt16(x["ServiceID"]) : 0,
                                                    BenchMarkLevel = x["BenchMarkLevel"] != DBNull.Value ? Convert.ToInt16(x["BenchMarkLevel"]) : 0,
                                                    BenchMarkValue = x["BenchMarkValue"] != DBNull.Value ? Convert.ToDecimal(x["BenchMarkValue"]) : 0,

                                                }).ToList();
                    objTimeSheetModel.LstOrgBenchMarkDetails = dtOrgBenchMark.AsEnumerable().Select(x =>
                                        new BenchMarkDetailsModel
                                        {
                                            ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt16(x["ServiceID"]) : 0,
                                            BenchMarkLevel = x["BenchMarkLevel"] != DBNull.Value ? Convert.ToInt16(x["BenchMarkLevel"]) : 0,
                                            BenchMarkValue = x["BenchMarkValue"] != DBNull.Value ? Convert.ToDecimal(x["BenchMarkValue"]) : 0,

                                        }).ToList();
                }
                if (dtWeekDays != null && dtWeekDays.Rows.Count > 0)
                {
                    objTimeSheetModel.LstWeekDays = new List<WeekDays>();

                    objTimeSheetModel.LstWeekDays = dtWeekDays.AsEnumerable().Select(x => new WeekDays
                    {
                        Date = x["DATETODAY"] != DBNull.Value ? Convert.ToString(x["DATETODAY"]) : "",
                        Day = x["NAME"] != DBNull.Value ? x["NAME"].ToString() : string.Empty,
                        DisplayDate = x["DisplayDate"] != DBNull.Value ? Convert.ToString(x["DisplayDate"]) : "",
                        FreezeStatus = x["FreezeStatus"] != DBNull.Value ? Convert.ToString(x["FreezeStatus"]) : "",
                        StatusId = x["StatusID"] != DBNull.Value ? Convert.ToInt64(x["StatusID"]) : 0,
                    }).ToList();

                    objTimeSheetModel.LstServiceActivityDetails = dtServiceActivityDetails.AsEnumerable().
                        Select(x => new ServiceActivityDetails
                        {
                            CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                            ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt64(x["ServiceID"]) : 0,
                            ServiceName = x["ServiceName"] != DBNull.Value ? x["ServiceName"].ToString() :
                        string.Empty,
                            ServiceTypeId = x["ServiceTypeID"] != DBNull.Value ? Convert.ToInt64(x["ServiceTypeID"]) : 0,
                            ActivityId = x["ActivityID"] != DBNull.Value ? Convert.ToInt64(x["ActivityID"]) : 0,
                            ActivityName = x["ActivityName"] != DBNull.Value ? x["ActivityName"].ToString() :
                        string.Empty,
                            ServiceLevelId = x["ServiceLevelID"] != DBNull.Value ? Convert.ToInt32(x["ServiceLevelID"]) : 0,
                            ScopeId = x["ScopeID"] != DBNull.Value ? Convert.ToInt32(x["ScopeID"]) : 0,
                        }).ToList();
                    objTimeSheetModel.LstTicketTypeDetails = dtTicketTypeDetails.AsEnumerable().Select(x =>
                    new TicketTypeDetails
                    {
                        CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                        ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                        AVMTicketType = x["AVMTicketType"] != DBNull.Value ? Convert.ToInt64(x["AVMTicketType"]) : 0,
                        TicketTypeId = x["TicketTypeID"] != DBNull.Value ? Convert.ToInt64(x["TicketTypeID"]) : 0,
                        TicketTypeMappingId = x["TicketTypeMappingID"] != DBNull.Value ? Convert.
                        ToInt64(x["TicketTypeMappingID"]) : 0,
                        TicketTypeName = x["TicketTypeName"] != DBNull.Value ? x["TicketTypeName"].ToString() :
                        string.Empty,
                        TicketType = x["TicketType"] != DBNull.Value ? x["TicketType"].ToString() : string.Empty,
                    }).ToList();
                    objTimeSheetModel.LstStatusDetails = dtStatusDetails.AsEnumerable().
                        Select(x => new StatusDetails
                        {
                            CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,

                            DARTStatusId = x["DARTStatusID"] != DBNull.Value ? Convert.ToInt64(x["DARTStatusID"]) : 0,
                            DARTStatusName = x["DARTStatusName"] != DBNull.Value ? x["DARTStatusName"].
                        ToString() : string.Empty,
                            TicketStatusId = x["TicketStatus_ID"] != DBNull.Value ? Convert.
                        ToInt64(x["TicketStatus_ID"]) : 0,
                            StatusName = x["StatusName"] != DBNull.Value ? x["StatusName"].ToString() : string.Empty
                        }).ToList();

                    objTimeSheetModel.LstOverallTicketDetails = dtTicketDetails.AsEnumerable().
                        Select(x => new OverallTicketDetails
                        {
                            TimeTickerId = x["TimeTickerID"] != DBNull.Value ? Convert.ToInt64(x["TimeTickerID"]) : 0,
                            TicketId = x["TicketID"] != DBNull.Value ? x["TicketID"].ToString() : string.Empty,

                            ApplicationId = x["ApplicationID"] != DBNull.Value ? Convert.ToInt64(x["ApplicationID"]) : 0,
                            TowerId = x["TowerID"] != DBNull.Value ? Convert.ToInt16(x["TowerID"]) : 0,
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                            AssignedTo = x["AssignedTo"] != DBNull.Value ? x["AssignedTo"].ToString() : string.Empty,
                            EffortTillDate = x["EffortTillDate"] != DBNull.Value ?
                        x["EffortTillDate"].ToString() : string.Empty,
                            ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt64(x["ServiceID"]) : 0,
                            ActivityId = x["ActivityID"] != DBNull.Value ? Convert.ToInt64(x["ActivityID"]) : 0,
                            TicketDescription = x["TicketDescription"] != DBNull.Value ?
                        Convert.ToInt32(x["IsNonTicket"]) != 1
                        && !string.IsNullOrEmpty(x["TicketDescription"].ToString()) &&
                        encryptionEnabled.ToLower() == "enabled" && (x["Type"] != DBNull.Value ? Convert.ToString(x["Type"]) : "T") != "W" ? aesMod.DecryptStringBytes((string)x["TicketDescription"],
                                                                    AseKeyDetail.AesKeyConstVal) :
                        Convert.ToString(x["TicketDescription"]) : string.Empty,
                            IsDeleted = x["IsDeleted"] != DBNull.Value ? Convert.ToInt32(x["IsDeleted"]) : 0,
                            TicketStatusMapId = x["TicketStatusMapID"] != DBNull.Value ? Convert.
                        ToInt64(x["TicketStatusMapID"]) : 0,
                            TicketTypeMapId = x["TicketTypeMapID"] != DBNull.Value ? Convert.
                        ToInt64(x["TicketTypeMapID"]) : 0,
                            IsSDTicket = x["IsSDTicket"] != DBNull.Value ? x["IsSDTicket"].
                        ToString() : string.Empty,
                            DARTStatusId = x["DARTStatusID"] != DBNull.Value ? Convert.
                        ToInt64(x["DARTStatusID"]) : 0,
                            ITSMEffort = x["ITSMEffort"] != DBNull.Value ? x["ITSMEffort"].ToString() : string.Empty,
                            IsNonTicket = x["IsNonTicket"] != DBNull.Value ? x["IsNonTicket"].ToString() : string.Empty,
                            IsCognizant = x["IsCustomer"] != DBNull.Value ? x["IsCustomer"].ToString() : string.Empty,
                            IsEffortTracked = x["IsEfforTracked"] != DBNull.Value ? x["IsEfforTracked"].
                        ToString() : string.Empty,
                            IsDebtEnabled = x["IsDebtEnabled"] != DBNull.Value ? x["IsDebtEnabled"].
                        ToString() : string.Empty,
                            IsMainspringConfigured = x["IsMainspringConfigured"] != DBNull.Value ?
                        x["IsMainspringConfigured"].ToString() : string.Empty,
                            ProjectTimeZoneName = x["ProjectTimeZoneName"] != DBNull.Value ?
                        x["ProjectTimeZoneName"].ToString() : string.Empty,
                            UserTimeZoneName = x["UserTimeZoneName"] != DBNull.Value ? x["UserTimeZoneName"].
                        ToString() : string.Empty,
                            ISTicket = x["ISTicket"] != DBNull.Value ? x["ISTicket"].ToString() : string.Empty,
                            IsAHTicket = x["IsAHTicket"] != DBNull.Value ? x["IsAHTicket"].ToString() : string.Empty,
                            IsAttributeUpdated = x["IsAttributeUpdated"] != DBNull.Value ? Convert.
                        ToInt32(x["IsAttributeUpdated"]) : 0,
                            TimeSheetDetailId1 = x["1TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["1TimeSheetDetailId"]) : 0,
                            Effort1 = x["1"] != DBNull.Value ? x["1"].ToString() : string.Empty,
                            TimeSheetDetailId2 = x["2TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["2TimeSheetDetailId"]) : 0,
                            Effort2 = x["2"] != DBNull.Value ? x["2"].ToString() : string.Empty,

                            TimeSheetDetailId3 = x["3TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["3TimeSheetDetailId"]) : 0,
                            Effort3 = x["3"] != DBNull.Value ? x["3"].ToString() : string.Empty,

                            TimeSheetDetailId4 = x["4TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["4TimeSheetDetailId"]) : 0,
                            Effort4 = x["4"] != DBNull.Value ? x["4"].ToString() : string.Empty,


                            TimeSheetDetailId5 = x["5TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["5TimeSheetDetailId"]) : 0,
                            Effort5 = x["5"] != DBNull.Value ? x["5"].ToString() : string.Empty,


                            TimeSheetDetailId6 = x["6TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["6TimeSheetDetailId"]) : 0,
                            Effort6 = x["6"] != DBNull.Value ? x["6"].ToString() : string.Empty,

                            TimeSheetDetailId7 = x["7TimeSheetDetailId"] != DBNull.Value ? Convert.
                        ToInt64(x["7TimeSheetDetailId"]) : 0,
                            Effort7 = x["7"] != DBNull.Value ? x["7"].ToString() : string.Empty,
                            SupportTypeId = x["SupportTypeID"] != DBNull.Value ? Convert.ToInt16(x["SupportTypeID"]) : 0,
                            GracePeriod = x["GracePeriod"] != DBNull.Value ? Convert.ToInt16(x["GracePeriod"]) : 0,

                            ClosedDate = x["ClosedDate"] != DBNull.Value ?
                                    Convert.ToDateTime(x["ClosedDate"]) : (DateTime?)null,
                            IsAHTagged = x["IsAHTagged"] != DBNull.Value ?
                                    Convert.ToBoolean(x["IsAHTagged"]) : false,
                            CompletedDate = x["CompletedDate"] != DBNull.Value ?
                                    Convert.ToDateTime(x["CompletedDate"]) : (DateTime?)null,
                            IsGracePeriodMet = (x["DARTStatusID"] != DBNull.Value ?
                                (Convert.ToInt16(x["DARTStatusID"]) == 8 && x["ClosedDate"]
                                != DBNull.Value ?
                                (DateTimeOffset.Now.DateTime >
                                Convert.ToDateTime(x["ClosedDate"]).AddDays(Convert.ToInt16(x["GracePeriod"]))
                                ? true : false) :
                                (Convert.ToInt16(x["DARTStatusID"]) == 9 && x["CompletedDate"] != DBNull.Value ?
                               (DateTimeOffset.Now.DateTime >
                               Convert.ToDateTime(x["CompletedDate"]).AddDays(Convert.ToInt16(x["GracePeriod"]))
                                ? true
                                : false)
                                : false))
                                : false),
                            Type = x["Type"] != DBNull.Value ? Convert.ToString(x["Type"]) : "T",
                            OpenDateNTime = x["OpenDateNTime"] != DBNull.Value ?
                                    Convert.ToDateTime(x["OpenDateNTime"]) : (DateTime?)null

                        }).ToList();
                    if (dtTimeSheetDetails.Rows.Count > 0)
                    {
                        objTimeSheetModel.LstTimeSheetDetails = dtTimeSheetDetails.AsEnumerable().
                            Select(x => new TimeSheetDetails
                            {
                                No = x["SNO"] != DBNull.Value ? Convert.ToInt64(x["SNO"]) : 0,
                                TimeSheetId = x["TimeSheetId"] != DBNull.Value ? Convert.
                            ToInt64(x["TimeSheetId"]) : 0,
                                TimeSheetDate = x["TimeSheetDate"] != DBNull.Value ? Convert.
                            ToString(x["TimeSheetDate"]) : "",
                                TimeSheetDetailId = x["TimeSheetDetailId"] != DBNull.Value ?
                            Convert.ToInt64(x["TimeSheetDetailId"]) : 0,
                                FreezeStatus = x["FreezStatus"] != DBNull.Value ? Convert.
                            ToBoolean(x["FreezStatus"]) : false,
                            }).ToList();
                    }



                    objTimeSheetModel.LstCustomerDetails = dtCustomerDetails.AsEnumerable().
                        Select(x => new CustomerDetails
                        {
                            CustomerId = x["CustomerId"] != DBNull.Value ? Convert.
                        ToInt64(x["CustomerId"]) : 0,
                            IsCognizant = x["IsCustomer"] != DBNull.Value ? Convert.
                        ToInt32(x["IsCustomer"]) : 0,
                            IsEffortTracked = x["IsEffortTracked"] != DBNull.Value ? Convert.
                        ToInt32(x["IsEffortTracked"]) : 0,
                            IsDaily = x["IsDaily"] != DBNull.Value ? Convert.
                        ToInt32(x["IsDaily"]) : 0,
                        }).ToList();
                    if (dtTicketTypeServiceDetails != null)
                    {
                        objTimeSheetModel.LstTicketTypeServiceDetails = dtTicketTypeServiceDetails.
                            AsEnumerable().Select(x => new TicketTypeServiceDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                                ServiceId = x["ServiceID"] != DBNull.Value ? Convert.ToInt64(x["ServiceID"]) : 0,
                                TicketTypeMappingId = x["TicketTypeMappingID"] != DBNull.Value ? Convert.
                            ToInt64(x["TicketTypeMappingID"]) : 0,
                                ServiceTypeId = x["ServiceTypeID"] != DBNull.Value ? Convert.
                            ToInt64(x["ServiceTypeID"]) : 0,
                            }).ToList();
                    }

                    if (dtUserLevelDetails != null)
                    {
                        objTimeSheetModel.LstUserLevelDetails = dtUserLevelDetails.AsEnumerable().
                            Select(x => new UserLevelDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                                ServiceLevelId = x["ServiceLevelID"] != DBNull.Value ? Convert.
                            ToInt32(x["ServiceLevelID"]) : 0
                            }).ToList();
                    }
                    if (dtTowerDetails != null)
                    {
                        objTimeSheetModel.LstTaskDetails = dtTowerDetails.AsEnumerable().
                            Select(x => new TaskDetails
                            {
                                CustomerId = x["CustomerID"] != DBNull.Value ? Convert.ToInt64(x["CustomerID"]) : 0,
                                ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                                InfraTowerTransactionId = x["InfraTowerTransactionID"] != DBNull.Value ?
                                                            Convert.ToInt64(x["InfraTowerTransactionID"]) : 0,
                                TowerName = x["TowerName"] != DBNull.Value ? x["TowerName"].ToString() : string.Empty,
                                InfraTransactionTaskId = x["InfraTransactionTaskID"] != DBNull.Value ? Convert.ToInt64(x["InfraTransactionTaskID"]) : 0,
                                InfraTaskName = x["InfraTaskName"] != DBNull.Value ? x["InfraTaskName"].ToString() : string.Empty,
                                ServiceLevelId = x["ServiceLevelID"] != DBNull.Value ? Convert.ToInt16(x["ServiceLevelID"]) : 0,
                            }).ToList();
                    }
                    if (objTimeSheetModel.LstTimeSheetDetails != null)
                    {
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate1 = objTimeSheetModel.
                                                                LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId == x.
                                                                TimeSheetDetailId1)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate2 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId ==
                                                                x.TimeSheetDetailId2)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());

                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate3 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId ==
                                                                x.TimeSheetDetailId3)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate4 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId ==
                                                                x.TimeSheetDetailId4)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate5 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId ==
                                                                x.TimeSheetDetailId5)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate6 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId ==
                                                                x.TimeSheetDetailId6)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetDate7 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId7)).
                                                                Select(b => b.TimeSheetDate).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                    (x => x.TimeSheetDate1 =
                                                                    objTimeSheetModel.LstTimeSheetDetails.
                                                                    Where(a => (a.TimeSheetDetailId
                                                                    == x.TimeSheetDetailId1)).
                                                                    Select(b => b.TimeSheetDate).
                                                                    FirstOrDefault());

                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId1 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId1)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId2 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId2)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());

                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId3 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId3)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetID4 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId4)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId5 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId5)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId6 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId6)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.TimeSheetId7 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId7)).
                                                                Select(b => b.TimeSheetId).
                                                                FirstOrDefault());

                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus1 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId1)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus2 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId2)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus3 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId3)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus4 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId4)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus5 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId5)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus6 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId6)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());
                        objTimeSheetModel.LstOverallTicketDetails.ForEach
                                                                (x => x.FreezeStatus7 =
                                                                objTimeSheetModel.LstTimeSheetDetails.
                                                                Where(a => (a.TimeSheetDetailId
                                                                == x.TimeSheetDetailId7)).
                                                                Select(b => b.FreezeStatus).
                                                                FirstOrDefault());

                    }
                    objTimeSheetModel.LstOverallTicketDetails.ForEach
                                    (x => x.LstTicketTypeModel = objTimeSheetModel.LstTicketTypeDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).
                                    Select(b => new TicketTypeDetails
                                    {
                                        TicketTypeMappingId =
                                        b.TicketTypeMappingId,
                                        TicketType = b.TicketType,
                                        ProjectId = b.ProjectId,
                                        AVMTicketType = b.AVMTicketType
                                    }).ToList());

                    objTimeSheetModel.LstOverallTicketDetails.ForEach
                                    (x => x.LstUserLevelDetails = objTimeSheetModel.LstUserLevelDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).
                                    Select(b => new UserLevelDetails
                                    {
                                        CustomerId = b.CustomerId,
                                        ProjectId = b.ProjectId,
                                        ServiceLevelId = b.ServiceLevelId
                                    }).ToList());

                    objTimeSheetModel.LstOverallTicketDetails.ForEach
                                    (x => x.LstTicketTypeServiceDetails =
                                    objTimeSheetModel.LstTicketTypeServiceDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).
                                    Select(b => new TicketTypeServiceDetails
                                    {
                                        CustomerId = b.CustomerId,
                                        ServiceId = b.ServiceId,
                                        ProjectId = b.ProjectId,
                                        TicketTypeMappingId = b.TicketTypeMappingId,
                                        ServiceTypeId = b.ServiceTypeId
                                    }).ToList());

                    objTimeSheetModel.LstOverallTicketDetails.ForEach(x => x.LstTicketTypeModel.
                    ForEach(a => a.IsSelected
                    = objTimeSheetModel.LstTicketTypeDetails.Where(c => (c.ProjectId == x.ProjectId) &&
                    (a.ProjectId == x.ProjectId) && (a.TicketTypeMappingId == c.TicketTypeMappingId) &&
                    c.TicketTypeMappingId == x.TicketTypeMapId).Count() > 0 ? true : false));
                    //ALM Tool Configured Flag
                    if (objTimeSheetModel.LstALMConfiguredDetails != null && objTimeSheetModel.LstALMConfiguredDetails.Count > 0)
                    {
                        objTimeSheetModel.LstOverallTicketDetails.ForEach(x =>
                        x.IsALMToolConfigured = (objTimeSheetModel.LstALMConfiguredDetails.Where(c =>
                        c.ProjectId == x.ProjectId)).Select(c => c.IsALMToolConfigured).
                                                                    FirstOrDefault());
                    }
                    //Status For App
                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "T").ToList().ForEach
                                    (x => x.LstStatusDetails = objTimeSheetModel.LstStatusDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).
                                    Select(b => new StatusDetails
                                    {
                                        TicketStatusId = b.TicketStatusId,
                                        StatusName = b.StatusName,
                                        ProjectId = b.ProjectId,
                                        DARTStatusId = b.DARTStatusId,
                                        DARTStatusName = b.DARTStatusName
                                    }).ToList());
                    if (objTimeSheetModel.LstADMStatusDetails != null && objTimeSheetModel.LstADMStatusDetails.Count > 0)
                    {
                        objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "W"
                                        && y.IsALMToolConfigured == false).ToList()
                                        .ForEach
                                    (x => x.LstStatusDetails = objTimeSheetModel.LstADMStatusDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).
                                    Select(b => new StatusDetails
                                    {
                                        TicketStatusId = b.TicketStatusId,
                                        StatusName = b.StatusName,
                                        ProjectId = b.ProjectId,
                                        DARTStatusId = b.DARTStatusId,
                                        DARTStatusName = b.DARTStatusName
                                    }).ToList());
                    }
                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "W"
                                        && y.IsALMToolConfigured == true).ToList().ForEach
                                  (x => x.LstStatusDetails = objTimeSheetModel.LstADMMasterStatusDetails.
                                  Select(b => new StatusDetails
                                  {
                                      TicketStatusId = b.Id,
                                      StatusName = b.Name,
                                      DARTStatusId = b.Id,
                                      DARTStatusName = b.Name
                                  }).ToList());
                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "T").ToList().ForEach(x => x.LstStatusDetails.ForEach(a => a.IsSelected
                        = objTimeSheetModel.LstStatusDetails.Where(c => (c.ProjectId == x.ProjectId) &&
                        (a.ProjectId == x.ProjectId)
                        && (a.TicketStatusId == c.TicketStatusId)
                        && c.TicketStatusId == x.TicketStatusMapId).Count() > 0 ? true : false));

                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "W" && y.IsALMToolConfigured == true).ToList()
                        .ForEach(x => x.LstStatusDetails.ForEach(a => a.IsSelected
                      = objTimeSheetModel.LstADMMasterStatusDetails.Where(c => (a.TicketStatusId == c.Id)
                      && c.Id == x.TicketStatusMapId).Count() > 0 ? true : false));

                    if (objTimeSheetModel.LstADMStatusDetails != null && objTimeSheetModel.LstADMStatusDetails.Count > 0)
                    {
                        objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "W" && y.IsALMToolConfigured == false).ToList().ForEach(x => x.LstStatusDetails.ForEach(a => a.IsSelected
                          = objTimeSheetModel.LstADMStatusDetails.Where(c => (c.ProjectId == x.ProjectId) &&
                          (a.ProjectId == x.ProjectId)
                          && (a.TicketStatusId == c.TicketStatusId)
                          && c.TicketStatusId == x.TicketStatusMapId).Count() > 0 ? true : false));
                    }

                    var objServiceDetails = objTimeSheetModel.LstServiceActivityDetails.Select(v =>
                    new { v.ServiceId, v.ProjectId, v.ServiceName, v.ServiceLevelId, v.ScopeId })
                    .Distinct().ToList();


                    List<ServiceDetails> lstServiceDetails = new List<ServiceDetails>();
                    foreach (var item in objServiceDetails)
                    {
                        ServiceDetails s = new ServiceDetails
                        {
                            IsSelected = false,
                            ProjectId = item.ProjectId,
                            ServiceId = item.ServiceId,
                            ServiceName = item.ServiceName,
                            ServiceLevelId = item.ServiceLevelId,
                            ScopeId = item.ScopeId
                        };
                        lstServiceDetails.Add(s);
                    }
                    objTimeSheetModel.LstServiceDetails = lstServiceDetails;
                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type == "W").ToList().
                    ForEach
                    (x => x.LstServiceModel = objTimeSheetModel.LstServiceDetails.
                    Where(a => (a.ProjectId == x.ProjectId) && a.ScopeId != 2).ToList().
                    Select(b => new ServiceDetails
                    {
                        ServiceId = b.ServiceId,
                        ServiceName = b.ServiceName,
                        ProjectId = b.ProjectId,
                        ServiceLevelId =
                    b.ServiceLevelId,
                        ScopeId = b.ScopeId
                    }).ToList());

                    objTimeSheetModel.LstOverallTicketDetails.Where(y => y.Type != "W").ToList().ForEach
                                    (x => x.LstServiceModel = objTimeSheetModel.LstServiceDetails.
                                    Where(a => (a.ProjectId == x.ProjectId)).ToList().
                                    Select(b => new ServiceDetails
                                    {
                                        ServiceId = b.ServiceId,
                                        ServiceName = b.ServiceName,
                                        ProjectId = b.ProjectId,
                                        ServiceLevelId =
                                        b.ServiceLevelId,
                                        ScopeId = b.ScopeId
                                    }).ToList());


                    objTimeSheetModel.LstOverallTicketDetails.ForEach(x => x.LstServiceModel.ForEach(a => a.IsSelected
                    = objTimeSheetModel.LstServiceDetails.Where(c => c.ProjectId == x.ProjectId &&
                    (a.ProjectId == x.ProjectId) && c.ServiceId == a.ServiceId && c.ServiceId == x.ServiceId).
                    Count() > 0 ? true : false));

                    List<ActivityDetails> lstActivityDetails = new List<ActivityDetails>();
                    objTimeSheetModel.LstOverallTicketDetails.ForEach
                                        (x => x.LstActivityModel = objTimeSheetModel.LstServiceActivityDetails.
                                        Where(a => (a.ProjectId == x.ProjectId)).
                                        Select(b => new ActivityDetails
                                        {
                                            ServiceId = b.ServiceId,
                                            ActivityId = b.ActivityId,
                                            ActivityName = b.ActivityName,
                                            ProjectId
                                            = b.ProjectId
                                        }).ToList());
                    objTimeSheetModel.LstOverallTicketDetails.ForEach(x => x.LstActivityModel.ForEach(a =>
                    a.IsSelected
                    = objTimeSheetModel.LstServiceActivityDetails.Where(c => c.ProjectId == x.ProjectId &&
                    (a.ProjectId == x.ProjectId) && (c.ServiceId == a.ServiceId && c.ServiceId == x.ServiceId)
                    && (c.ActivityId == a.ActivityId && c.ActivityId == x.ActivityId)).Count() > 0 ? true : false));

                    objTimeSheetModel.LstOverallTicketDetails.ForEach(x => x.LstActivityModel.ForEach(a =>
                    a.DisplayActivity
                        = objTimeSheetModel.LstOverallTicketDetails.Where(c => c.ServiceId == a.ServiceId &&
                        c.ProjectId == a.ProjectId).Count() > 0 ? "block" : "none"));


                    List<TaskDetails> lstTowerDetails = new List<TaskDetails>();
                    objTimeSheetModel.LstOverallTicketDetails.ForEach
                                        (x => x.LstTaskModel = objTimeSheetModel.LstTaskDetails.
                                        Where(a => (a.ProjectId == x.ProjectId) && a.InfraTowerTransactionId == x.TowerId).
                                        Select(b => new TaskDetails
                                        {
                                            InfraTowerTransactionId = b.InfraTowerTransactionId,
                                            TowerName = b.TowerName,
                                            InfraTransactionTaskId = b.InfraTransactionTaskId,
                                            InfraTaskName = b.InfraTaskName,
                                            ProjectId = b.ProjectId,
                                            ServiceLevelId = b.ServiceLevelId,
                                            CustomerId = b.CustomerId
                                        }).ToList());
                    objTimeSheetModel.LstOverallTicketDetails.ForEach(x => x.LstTaskModel.ForEach(a =>
                    a.IsSelected
                    = objTimeSheetModel.LstTaskDetails.Where(c => c.ProjectId == x.ProjectId &&
                    (a.ProjectId == x.ProjectId) && (c.InfraTowerTransactionId == a.InfraTowerTransactionId
                    && c.InfraTowerTransactionId == x.TowerId)
                    && (c.InfraTransactionTaskId == a.InfraTransactionTaskId && c.InfraTransactionTaskId == x.ActivityId)).Count() > 0 ? true : false));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeSheetModel;
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
            TimeSheetModel objTimeSheetModel = new TimeSheetModel();
            try
            {
                DataSet dtResult = new DataSet();
                dtResult.Locale = CultureInfo.InvariantCulture;
                List<EffortDetailsData> lstResult = new List<EffortDetailsData>();
                SqlParameter[] prms = new SqlParameter[9];
                prms[0] = new SqlParameter("@EmployeeID", objNonTicketModel.EmployeeId);
                prms[1] = new SqlParameter("@CustomerID", objNonTicketModel.CustomerId);
                prms[2] = new SqlParameter("@FirstDateOfWeek", objNonTicketModel.FirstDateOfWeek);
                prms[3] = new SqlParameter("@TicketID", objNonTicketModel.TicketId);
                prms[4] = new SqlParameter("@Remarks", !string.IsNullOrEmpty(objNonTicketModel.TicketDescription)
                                            ? objNonTicketModel.TicketDescription : "");
                prms[5] = new SqlParameter("@NonTicketActivity", !string.IsNullOrEmpty(objNonTicketModel.NonTicketActivityId)
                               ? objNonTicketModel.NonTicketActivityId : "");
                prms[6] = new SqlParameter("@LastDateOfWeek", objNonTicketModel.LastDateOfWeek);
                prms[7] = new SqlParameter("@ProjectID", objNonTicketModel.ProjectId);
                prms[8] = new SqlParameter("@SuggestedActivityName", !string.IsNullOrEmpty(objNonTicketModel.SuggestedActivityName)
                                                 ? objNonTicketModel.SuggestedActivityName : "");
                dtResult = (new DBHelper()).GetDatasetFromSP("AVL_SaveNonTicketDetails", prms, ConnectionString);
                objTimeSheetModel = BindTimesheetInfo(dtResult);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeSheetModel;


        }

        /// <summary>
        /// This Method Is Used To DeleteTicket
        /// </summary>
        /// <param name="objDeleteTicket"></param>
        /// <returns></returns>
        //-----------------Delete Ticket-------------------------
        public bool DeleteTicket(DeleteTicket objDeleteTicket)
        {
            TimeSheetModel objTimeSheetModel = new TimeSheetModel();
            try
            {
                DataSet dtResult = new DataSet();
                dtResult.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[13];
                prms[0] = new SqlParameter("@EmployeeID", objDeleteTicket.EmployeeId);
                prms[1] = new SqlParameter("@CustomerID", objDeleteTicket.CustomerId);
                prms[2] = new SqlParameter("@ProjectID", objDeleteTicket.ProjectId);
                prms[3] = new SqlParameter("@TicketID", objDeleteTicket.TicketId);
                prms[4] = new SqlParameter("@ServiceID", string.IsNullOrEmpty(objDeleteTicket.ServiceId) == true ? "0"
                    : objDeleteTicket.ServiceId);
                prms[5] = new SqlParameter("@ActivityID", objDeleteTicket.ActivityId);
                prms[6] = new SqlParameter("@TimeTickerID", objDeleteTicket.TimeTickerId);
                prms[7] = new SqlParameter("@FirstDateOfWeek", objDeleteTicket.StartDate);
                prms[8] = new SqlParameter("@LastDateOfWeek", objDeleteTicket.EndDate);
                prms[9] = new SqlParameter("@SubmitterID", objDeleteTicket.SubmitterId);
                prms[10] = new SqlParameter("@Hours", objDeleteTicket.TxtHours);
                prms[11] = new SqlParameter("@TickSupportTypeID", objDeleteTicket.TickSupportTypeId);
                prms[12] = new SqlParameter("@Type", objDeleteTicket.Type);

                dtResult = (new DBHelper()).GetDatasetFromSP("[AVL].[DeleteTicketfrmGrid]", prms, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// GetEffortDetailsDataByMonth
        /// </summary>
        /// <param name="objNonTicketModel"></param>
        /// <returns></returns>
        public EffortDetailsData GetEffortDetailsDataByMonth(string EmployeeID, string CustomerID, string FromDate,
            string ToDate)
        {
            EffortDetailsData objEffortDetailsByDate = new EffortDetailsData();
            try
            {

                DataTable dtResult = new DataTable();
                dtResult.Locale = CultureInfo.InvariantCulture;
                List<EffortDetailsData> lstResult = new List<EffortDetailsData>();
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@CognizantID", EmployeeID);
                prms[1] = new SqlParameter("@CustomerID", CustomerID);
                prms[2] = new SqlParameter("@FromDate", DateTime.ParseExact(FromDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture));
                prms[3] = new SqlParameter("@ToDate", DateTime.ParseExact(ToDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture));


                dtResult = (new DBHelper()).GetTableFromSP("[dbo].[Effort_GetMonthlyEffortByCustomer]", prms, ConnectionString);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    objEffortDetailsByDate.ClosedTicket = Convert.ToString(dtResult.Rows[0]["ClosedTickets"]);
                    objEffortDetailsByDate.TicketedEffort = Convert.ToString(dtResult.Rows[0]["TicketedEffort"]);
                    objEffortDetailsByDate.NonTicketedEffort = Convert.ToString(dtResult.Rows[0]["NonTicketedEffort"]);
                    objEffortDetailsByDate.ClosedWorkItem = Convert.ToString(dtResult.Rows[0]["ClosedWorkItems"]);
                    objEffortDetailsByDate.WorkItemEffort = Convert.ToString(dtResult.Rows[0]["WorkItemEffort"]);
                    objEffortDetailsByDate.TotalEffort = Convert.ToString(dtResult.Rows[0]["TotalEfforts"]);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objEffortDetailsByDate;
        }


        /// <summary>
        /// This Method Is Used To GetDetailsAddTicket
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public AddTicketNewDetails GetDetailsAddTicket(int ProjectID, string UserID, int supportTypeID)
        {
            AddTicketNewDetails prjDet = new AddTicketNewDetails();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@UserID", UserID);
            prms[2] = new SqlParameter("@supportTypeID", supportTypeID);
            try
            {
                DataSet dsList = (new DBHelper()).GetDatasetFromSP("[AVL].[GetDetailsAddTicket]", prms, ConnectionString);
                if (dsList != null)
                {
                    if (dsList.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsList.Tables[0].Rows[0]["SupportTypeId"]) != 4)
                    {
                        prjDet.TmSupporttype.SupportTypeId = Convert.ToInt32(dsList.Tables[0].Rows[0]
                                ["SupportTypeId"]);

                    }

                    if (dsList.Tables[1].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[1].Rows.Count; h++)
                        {
                            prjDet.LstAssignmentGroup.Add(new TMAssignmentGroupModel
                            {
                                AssignmentGroupId = Convert.ToInt32(dsList.Tables[1].Rows[h]["AssignmentGroupMapID"]),
                                AssignmentGroupName = dsList.Tables[1].Rows[h]["AssignmentGroupName"].ToString()
                            });
                        }


                    }



                    if (dsList.Tables[2].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[2].Rows.Count; h++)
                        {
                            prjDet.LstTower.Add(new TMTowerModel
                            {
                                TowerId = Convert.ToInt32(dsList.Tables[2].Rows[h]["TowerID"]),
                                TowerName = dsList.Tables[2].Rows[h]["TowerName"].ToString()
                            });
                        }
                    }

                    if (dsList.Tables[3].Rows.Count > 0)
                    {


                        for (int h = 0; h < dsList.Tables[3].Rows.Count; h++)
                        {

                            prjDet.LstTicketTypeBysupportType.Add(
                               new TicketTypeNewModel
                               {
                                   TicketTypeId = Convert.ToInt64(dsList.Tables[3].Rows[h]["TicketTypeMappingID"]),
                                   TicketType = dsList.Tables[3].Rows[h]["TicketType"].ToString()
                            ,
                                   TicketTypemasId = Convert.ToInt64(dsList.Tables[3].Rows[h]["AVMTicketType"] !=
                                DBNull.Value ? dsList.Tables[3].Rows[h]["AVMTicketType"] : "0"),
                                   IsDefaultTicketType = Convert.ToChar(dsList.Tables[3].Rows[h]["IsDefaultTicketType"]
                                != DBNull.Value ? dsList.Tables[3].Rows[h]["IsDefaultTicketType"] : 'N'),
                                   SupportTypeId = Convert.ToInt32(dsList.Tables[3].Rows[h]["SupportTypeID"] !=
                                DBNull.Value ? dsList.Tables[3].Rows[h]["SupportTypeID"] : "0")
                               }

                                );
                        }
                    }

                    if (dsList.Tables[4].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[4].Rows.Count; h++)
                        {
                            prjDet.LstAssignmentTowerMapModel.Add(new AssignmentTowerMapModel
                            {
                                AssignmentGroupMapId = Convert.ToInt64(dsList.Tables[4].Rows[h]["AssignmentGroupMapId"]),
                                TowerId = Convert.ToInt64(dsList.Tables[4].Rows[h]["TowerId"])
                            });
                        }


                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prjDet;


        }
        /// <summary>
        /// This Method Is Used To GetStatusPriorityTicketTypeDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ProjectNewDetails GetStatusPriorityTicketTypeDetails(int ProjectID)
        {

            ProjectNewDetails prjDet = new ProjectNewDetails();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            try
            {
                DataSet dsList = (new DBHelper()).GetDatasetFromSP("AVL_GetDetailsByProjectid", prms, ConnectionString);
                if (dsList != null)
                {
                    if (dsList.Tables[0].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[0].Rows.Count; h++)
                        {
                            prjDet.LstPriorityListForAdd.Add(new PriorityNewModel
                            {
                                PriorityId = Convert.ToInt64(dsList.Tables[0].Rows[h]["PriorityIDMapID"]),
                                PriorityName = dsList.Tables[0].Rows[h]["PriorityName"].ToString()
                            ,
                                PriorityMasId = Convert.ToInt64(dsList.Tables[0].Rows[h]["PriorityID"] !=
                                DBNull.Value ? dsList.Tables[0].Rows[h]["PriorityID"] : "0"),
                                IsDefaultPrority = Convert.ToChar(dsList.Tables[0].Rows[h]["IsDefaultPriority"] !=
                                DBNull.Value ? dsList.Tables[0].Rows[h]["IsDefaultPriority"] : 'N')
                            });
                        }


                    }


                    if (dsList.Tables[1].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[1].Rows.Count; h++)
                        {
                            prjDet.LstTicketTypeListForAdd.Add(new TicketTypeNewModel
                            {
                                TicketTypeId = Convert.ToInt64(dsList.Tables[1].Rows[h]["TicketTypeMappingID"]),
                                TicketType = dsList.Tables[1].Rows[h]["TicketType"].ToString()
                            ,
                                TicketTypemasId = Convert.ToInt64(dsList.Tables[1].Rows[h]["AVMTicketType"] !=
                                DBNull.Value ? dsList.Tables[1].Rows[h]["AVMTicketType"] : "0"),
                                IsDefaultTicketType = Convert.ToChar(dsList.Tables[1].Rows[h]["IsDefaultTicketType"]
                                != DBNull.Value ? dsList.Tables[1].Rows[h]["IsDefaultTicketType"] : 'N'),
                                SupportTypeId = Convert.ToInt32(dsList.Tables[1].Rows[h]["SupportTypeID"] !=
                                DBNull.Value ? dsList.Tables[1].Rows[h]["SupportTypeID"] : "0"),
                            });
                        }


                    }



                    if (dsList.Tables[2].Rows.Count > 0)
                    {
                        for (int h = 0; h < dsList.Tables[2].Rows.Count; h++)
                        {
                            prjDet.LstStatusListForAdd.Add(new StatusNewModel
                            {
                                StatusId = Convert.ToInt64(dsList.Tables[2].Rows[h]["StatusID"]),
                                StatusName = dsList.Tables[2].Rows[h]["StatusName"].ToString()
                            ,
                                TicketStatusId = Convert.ToInt64(dsList.Tables[2].Rows[h]["TicketStatus_ID"] !=
                                DBNull.Value ? dsList.Tables[2].Rows[h]["TicketStatus_ID"] : 0),
                                IsDefaultTicketStatus = Convert.ToChar(dsList.Tables[2].
                                Rows[h]["IsDefaultTicketStatus"] != DBNull.Value ? dsList.Tables[2].
                                Rows[h]["IsDefaultTicketStatus"] : 'N')
                            });
                        }


                    }
                    if (dsList.Tables.Count > 3 && dsList.Tables[3].Rows.Count > 0)
                    {
                        prjDet.LstTowerDetails = new List<InfraTowerDetails>();
                        for (int h = 0; h < dsList.Tables[3].Rows.Count; h++)
                        {
                            prjDet.LstTowerDetails.Add(new InfraTowerDetails
                            {
                                TowerId = Convert.ToInt64(dsList.Tables[3].Rows[h]["TowerID"]),
                                Tower = dsList.Tables[3].Rows[h]["Tower"].ToString()
                            });
                        }
                    }
                    if (dsList.Tables.Count > 4 && dsList.Tables[4].Rows.Count > 0)
                    {
                        prjDet.LstAssignmentGroupDetails = new List<AssignmentGroupDetails>();
                        for (int h = 0; h < dsList.Tables[4].Rows.Count; h++)
                        {
                            prjDet.LstAssignmentGroupDetails.Add(new AssignmentGroupDetails
                            {
                                AssignmentGroupId = Convert.ToInt64(dsList.Tables[4].Rows[h]["AssignmentGroupID"]),
                                AssignmentGroupName = dsList.Tables[4].Rows[h]["AssignmentGroupName"].ToString()
                            });
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prjDet;

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
            GetDebtAvoidResidual objsp_GetDebtAvoidResidual = new GetDebtAvoidResidual();
            SqlParameter[] prms = new SqlParameter[11];
            prms[0] = new SqlParameter("@ProjectID", objCauseCodeResolutionCode.ProjectId);
            prms[1] = new SqlParameter("@ApplicationName", objCauseCodeResolutionCode.Application);
            prms[2] = new SqlParameter("@TicketDescription", objCauseCodeResolutionCode.TicketDescription);
            prms[3] = new SqlParameter("@Causecode", objCauseCodeResolutionCode.CauseCode);
            prms[4] = new SqlParameter("@Resolutioncode", objCauseCodeResolutionCode.ResolutionCode);
            prms[5] = new SqlParameter("@IsAutoClassified", isAutoClassified);
            prms[6] = new SqlParameter("@IsDDAutoClassified", isDDAutoClassified);
            prms[7] = new SqlParameter("@ServiceID", objCauseCodeResolutionCode.ServiceId);
            prms[8] = new SqlParameter("@TicketTypeID", objCauseCodeResolutionCode.TicketTypeId);
            prms[9] = new SqlParameter("@TimeTickerID", objCauseCodeResolutionCode.TimeTickerId);
            prms[10] = new SqlParameter("@UserID", objCauseCodeResolutionCode.UserId);

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[dbo].[AVL_DebtGetAutoClassifiedDebtFilds_DD]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        objsp_GetDebtAvoidResidual.DebtClassification = Convert.ToInt32(((dt.
                            Rows[i]["DebtClassificationId"] != DBNull.Value) ? dt.
                            Rows[i]["DebtClassificationId"] : 0));
                        objsp_GetDebtAvoidResidual.ResidualDebt = Convert.ToInt32(((dt.
                            Rows[i]["ResidualFlagID"] != DBNull.Value) ? dt.Rows[i]["ResidualFlagID"] : 0));
                        objsp_GetDebtAvoidResidual.AvoidableFlag = Convert.ToInt32(((dt.
                            Rows[i]["AvoidableFlagID"] != DBNull.Value) ? dt.Rows[i]["AvoidableFlagID"] : 0));
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
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetAutoClassifiedDetailsForDebt(string ProjectID)
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectId", ProjectID);


            dt = (new DBHelper()).GetTableFromSP("[dbo].[debt_getautoclassifiedfieldforsharepathchange]", prms, ConnectionString);
            return dt;
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
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@projectID", ProjectID);
            prms[1] = new SqlParameter("@add_text", add_text);
            prms[2] = new SqlParameter("@TimeTickerID", TimeTickerID);

            dt = (new DBHelper()).GetTableFromSP("[AVL].[GetTranslatedfieldsforMLAPI]", prms, ConnectionString);
            return dt;
        }

        /// <summary>
        /// Get addtext for Debt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetaddtextforDebt(string ProjectID, int SupportTypeID)
        {
            string add_text = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[2];

                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt32(ProjectID));
                prms[1] = new SqlParameter("@SupportTypeID", SupportTypeID);

                dt = (new DBHelper()).GetTableFromSP("[dbo].[ML_Getadd_textforDebt]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    add_text = dt.Rows[0]["OptionalFields"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return add_text;
        }

        /// <summary>
        /// This Method Is Used To GetApplicationDetailForEmployeeid
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<ApplicationProjectModel> GetProjectDetailForEmployeeid(string EmployeeID, Int64 CustomerID)
        {

            List<ApplicationProjectModel> lstAppProject = new List<ApplicationProjectModel>();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("GetApplicationDetailForEmp", prms, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        lstAppProject.Add(new ApplicationProjectModel
                        {
                            ApplicationId = Convert.ToInt64(dt.Rows[i]["ApplicationID"]),
                            ApplicationName = dt.Rows[i]["ApplicationName"].ToString(),
                            ProjectId = Convert.ToInt64(dt.Rows[i]["ProjectID"]),
                            ProjectName = dt.Rows[i]["ProjectName"].ToString(),
                            UserTimeZoneId = Convert.ToString(dt.Rows[i]["UserTimeZoneId"]),
                            UserTimeZoneName = Convert.ToString(dt.Rows[i]["UserTimeZoneName"]),
                            ProjectTimeZoneId = Convert.ToString(dt.Rows[i]["ProjectTimeZoneId"]),
                            ProjectTimeZoneName = Convert.ToString(dt.Rows[i]["ProjectTimeZoneName"])
                        });
                    }
                    lstAppProject = lstAppProject.OrderBy(x => x.ApplicationName).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstAppProject;
        }
        /// <summary>
        /// This Method Is Used To GetApplicationDetailForEmployeeid
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<ApplicationProjectModel> GetApplicationDetailForEmployeeid(string EmployeeID, Int64 CustomerID)
        {

            List<ApplicationProjectModel> lstAppProject = new List<ApplicationProjectModel>();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("GetApplicationDetailForEmp", prms, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        lstAppProject.Add(new ApplicationProjectModel
                        {
                            ApplicationId = Convert.ToInt64(dt.Rows[i]["ApplicationID"]),
                            ApplicationName = dt.Rows[i]["ApplicationName"].ToString(),
                            ProjectId = Convert.ToInt64(dt.Rows[i]["ProjectID"]),
                            ProjectName = dt.Rows[i]["ProjectName"].ToString(),
                            UserTimeZoneId = Convert.ToString(dt.Rows[i]["UserTimeZoneId"]),
                            UserTimeZoneName = Convert.ToString(dt.Rows[i]["UserTimeZoneName"]),
                            ProjectTimeZoneId = Convert.ToString(dt.Rows[i]["ProjectTimeZoneId"]),
                            ProjectTimeZoneName = Convert.ToString(dt.Rows[i]["ProjectTimeZoneName"])
                        });
                    }
                    lstAppProject = lstAppProject.OrderBy(x => x.ApplicationName).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstAppProject;
        }
        /// <summary>
        /// This Method Is Used To GetInvalidSuggestedActivities
        /// </summary>
        /// <returns></returns>
        public List<NameIDModel> GetInvalidSuggestedActivities()
        {
            List<NameIDModel> lstInvalidSuggestedActivities = new List<NameIDModel>();
            SqlParameter[] prmsSuggestedActivities = new SqlParameter[0];
            try
            {
                DataTable dtSuggestedActivities =
                    (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetInvalidSuggestedActivities]",
                                prmsSuggestedActivities, ConnectionString);
                if (dtSuggestedActivities != null && dtSuggestedActivities.Rows.Count > 0)
                {
                    lstInvalidSuggestedActivities =
                        DataTableEntensions.ToList<NameIDModel>(dtSuggestedActivities).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstInvalidSuggestedActivities;
        }
        /// <summary>
        /// This Method Is Used To AddTicketDetails
        /// </summary>
        /// <param name="objAddTicketDetails"></param>
        /// <returns></returns>
        public TimeSheetModel AddTicketDetails(AddTicketDetails objAddTicketDetails)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            TimeSheetModel objTimeSheetModel = new TimeSheetModel();
            AESEncryption aesMod = new AESEncryption();
            byte[] bytesEncrypted;
            string encTicketDesc;
            Translation translation = new Translation();
            List<ConcatenateStrings> lstTicket = new List<ConcatenateStrings>();
            string translatedTicketDescription = null;
            var multilingualConfig = translation.GetProjectMultilinugalTranslateFields(Convert.ToString
                (objAddTicketDetails.CustomerId), Convert.ToString(objAddTicketDetails.ProjectId));
            if (!string.IsNullOrEmpty(objAddTicketDetails.TicketDescription) && multilingualConfig.IsMultilingualEnable
                .Equals(1) && multilingualConfig.ListTranslateFields.Any(translateField => translateField.ColumnName
                .Equals(ApplicationConstants.LanguageTranslateDescriptionColumn)))
            {
                var lang = multilingualConfig.ListTranslateLanguage.Where
                    (c => c.IsSelected.Equals(true)).Select(i => i.LanguageCode).ToArray();
                lstTicket.Add(new ConcatenateStrings
                {
                    TimeTickerId = 0,
                    OriginalColumn = "TicketDescription",
                    TranslatedColumn = "TranslatedTicketDescription",
                    Text = objAddTicketDetails.TicketDescription.Trim(),
                    TranslatedText = null,
                    TextLength = objAddTicketDetails.TicketDescription.Length,
                    SupportType = objAddTicketDetails.SupportTypeId
                });
                var translateResult = translation.PostToMSTranslatorList(lstTicket,
                    ApplicationConstants.TranslateDestinationLanguageCode, lang, multilingualConfig.SubscriptionKey);
                translateResult.Wait();
                if (!translateResult.Result.FirstOrDefault().HasError)
                {
                    translatedTicketDescription = translateResult.Result.FirstOrDefault().TranslatedText;
                }
                else
                {
                    return objTimeSheetModel;
                }
            }
            if (objAddTicketDetails.TicketDescription != "" && encryptionEnabled.ToLower() == "enabled")
            {
                bytesEncrypted = aesMod.EncryptStringAsBytes(objAddTicketDetails.TicketDescription,
                    AseKeyDetail.AesKeyConstVal);
                encTicketDesc = Convert.ToBase64String(bytesEncrypted);
                if (!string.IsNullOrEmpty(translatedTicketDescription))
                {
                    bytesEncrypted = aesMod.EncryptStringAsBytes(translatedTicketDescription,
                        AseKeyDetail.AesKeyConstVal);
                    translatedTicketDescription = Convert.ToBase64String(bytesEncrypted);
                }
            }
            else
            {
                encTicketDesc = objAddTicketDetails.TicketDescription;
            }
            if (objAddTicketDetails.AssignmentGroupId == 0)
            {
                objAddTicketDetails.AssignmentGroup = "";
            }
            try
            {
                SqlParameter[] prms = new SqlParameter[21];
                prms[0] = new SqlParameter("@TicketID", objAddTicketDetails.TicketId);
                prms[1] = new SqlParameter("@EmployeeID", objAddTicketDetails.EmployeeId);
                prms[2] = new SqlParameter("@FirstDateOfWeek", DateTime.ParseExact(objAddTicketDetails.FirstDayofWeek,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prms[3] = new SqlParameter("@LastDateOfWeek", DateTime.ParseExact(objAddTicketDetails.LastDayofWeek,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prms[4] = new SqlParameter("@IsCognizant", objAddTicketDetails.IsCognizant);
                prms[5] = new SqlParameter("@UserID", objAddTicketDetails.UserId);
                prms[6] = new SqlParameter("@IsSDTicket", objAddTicketDetails.IsSDTicket);
                prms[7] = new SqlParameter("@DartStatusID", objAddTicketDetails.DartStatusId);
                prms[8] = new SqlParameter("@TicketStatus", objAddTicketDetails.StatusId);
                prms[9] = new SqlParameter("@CustomerID", objAddTicketDetails.CustomerId);
                prms[10] = new SqlParameter("@ProjectID", objAddTicketDetails.ProjectId);
                prms[11] = new SqlParameter("@TicketDescription", encTicketDesc);
                prms[12] = new SqlParameter("@OpenDate", objAddTicketDetails.OpenDate);
                prms[13] = new SqlParameter("@PriorityID", objAddTicketDetails.PriorityMapId);
                prms[14] = new SqlParameter("@TicketTypeID", objAddTicketDetails.TicketTypeMapId);
                prms[15] = new SqlParameter("@ApplicationID", objAddTicketDetails.ApplicationId);
                prms[16] = new SqlParameter("@mticketdescription", translatedTicketDescription);
                prms[17] = new SqlParameter("@SupportTypeID", objAddTicketDetails.SupportTypeId);
                prms[18] = new SqlParameter("@TowerID", objAddTicketDetails.TowerID);
                prms[19] = new SqlParameter("@AssignmentGroupID", objAddTicketDetails.AssignmentGroupId);
                prms[20] = new SqlParameter("@AssignmentGroup", objAddTicketDetails.AssignmentGroup);
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[SaveTicketDetails]", prms, ConnectionString);
                DataSet dsMaster = new DataSet();
                dsMaster.Locale = CultureInfo.InvariantCulture;
                DataSet dsADMTimesheetDetails = new DataSet();
                dsADMTimesheetDetails.Locale = CultureInfo.InvariantCulture;
                dsMaster = GetMasterValuesForTimeSheet(Convert.ToString(objAddTicketDetails.CustomerId),
                    objAddTicketDetails.EmployeeId, objAddTicketDetails.FirstDayofWeek,
                    objAddTicketDetails.LastDayofWeek);
                dsADMTimesheetDetails = GetADMDetails(objAddTicketDetails.CustomerId,
                                        objAddTicketDetails.EmployeeId);

                objTimeSheetModel = BindTimesSheetInfoFromDataSet(ds, dsMaster, null,
                    dsADMTimesheetDetails);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeSheetModel;
        }

        /// <summary>
        /// This Method Is used to get the Service Bench Mark for Timesheet
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private DataSet GetADMDetails(Int64 CustomerID, string EmployeeID)
        {
            DataSet dsADMDetails = new DataSet();
            dsADMDetails.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prmsADMDetails = new SqlParameter[2];
                prmsADMDetails[0] = new SqlParameter("@CustomerID", CustomerID);
                prmsADMDetails[1] = new SqlParameter("@EmployeeID", EmployeeID);
                dsADMDetails = (new DBHelper()).GetDatasetFromSP
                                    ("[AVL].[Effort_GetADMDetailsForTimesheet]",
                                                                prmsADMDetails, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dsADMDetails;
        }
        /// <summary>
        /// This Method Is used to get the Service Bench Mark for Timesheet
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        private DataSet GetServiceBenchMarkForTimeSheet(string CustomerID, string EmployeeID)
        {
            DataSet dsServiceBenchMark = new DataSet();
            dsServiceBenchMark.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prmsServiceBenchMark = new SqlParameter[2];
                prmsServiceBenchMark[0] = new SqlParameter("@CustomerID", CustomerID);
                prmsServiceBenchMark[1] = new SqlParameter("@EmployeeID", EmployeeID);
                dsServiceBenchMark = (new DBHelper()).GetDatasetFromSP
                                    ("[AVL].[Effort_GetServiceBenchMarkDetailsForTimesheet]",
                                                                prmsServiceBenchMark, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dsServiceBenchMark;
        }
        /// <summary>
        /// Get master details for timesheet
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstDateOfWeek"></param>
        /// <param name="LastDateOfWeek"></param>
        /// <returns></returns>

        private DataSet GetMasterValuesForTimeSheet(string CustomerID, string EmployeeID,
            string FirstDateOfWeek, string LastDateOfWeek)
        {
            DataSet dsMaster = new DataSet();
            dsMaster.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prmsMaster = new SqlParameter[4];
                prmsMaster[0] = new SqlParameter("@CustomerID", CustomerID);
                prmsMaster[1] = new SqlParameter("@EmployeeID", EmployeeID);
                prmsMaster[2] = new SqlParameter("@FirstDateOfWeek", DateTime.ParseExact(FirstDateOfWeek,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prmsMaster[3] = new SqlParameter("@LastDateOfWeek", DateTime.ParseExact(LastDateOfWeek,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));

                dsMaster = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_GetMasterDetailsForTimesheet]",
                                                                prmsMaster, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dsMaster;

        }

        /// <summary>
        /// This Method Is Used To GetNonTicketDetailsToPopup
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public NonTicketPopupDetails GetNonTicketDetailsToPopup(string EmployeeID, Int64 CustomerID)
        {

            NonTicketPopupDetails nonTicketObject = new NonTicketPopupDetails();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);

            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("AVL_GetTicketCategoryList", prms, ConnectionString);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        nonTicketObject.LstNonTicketActivity.Add(new NonTicketedActivityModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[i]["ID"]),
                            NonTicketedActivity = ds.Tables[0].Rows[i]["NonTicketedActivity"].ToString()
                        });
                    }
                }


            }

            catch (Exception ex)
            {
                throw ex;

            }

            return nonTicketObject;
        }
        /// <summary>
        /// This Method Is Used To GetHiddenFieldsForTM
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public HiddenFieldsModel GetHiddenFieldsForTM(string EmployeeID, Int64 CustomerID)
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);

            HiddenFieldsModel objHiddenFieldsModel = new HiddenFieldsModel();
            List<HiddenScope> lstScope = new List<HiddenScope>();
            List<HiddenUserProjectID> lstProjectUserId = new List<HiddenUserProjectID>();
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
                            objHiddenUserProjectID.IsApplensAsALM = Convert.ToBoolean((ds.Tables[0].Rows[i]["IsApplensAsALM"])
                               == DBNull.Value ? "false" : ds.Tables[0].Rows[i]["IsApplensAsALM"]);
                            objHiddenUserProjectID.EsaProjectId = Convert.ToString((ds.Tables[0].Rows[i]["EsaProjectID"])
                          == DBNull.Value ? "0" : ds.Tables[0].Rows[i]["EsaProjectID"]);
                            lstProjectUserId.Add(objHiddenUserProjectID);
                        }
                        objHiddenFieldsModel.LstProjectUserID = lstProjectUserId;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (Convert.ToInt64(ds.Tables[1].Rows[i]["CustomerID"]) == CustomerID)
                            {
                                objHiddenFieldsModel.CustomerId = Convert.ToString((ds.Tables[1].Rows[i]["CustomerID"])
                                    == DBNull.Value ? "" : ds.Tables[1].Rows[i]["CustomerID"]);
                                objHiddenFieldsModel.CustomerName = Convert.ToString((ds.Tables[1].
                                    Rows[i]["CustomerName"]) == DBNull.Value ? "0" : ds.Tables[1].
                                    Rows[i]["CustomerName"]);
                                objHiddenFieldsModel.CustomerTimeZoneId = Convert.ToString((ds.Tables[1].
                                    Rows[i]["CustomerTimeZoneID"]) == DBNull.Value ? "" : ds.Tables[1].
                                    Rows[i]["CustomerTimeZoneID"]);
                                objHiddenFieldsModel.CustomerTimeZoneName = Convert.ToString((ds.Tables[1].
                                    Rows[i]["CustomerTimeZoneName"]) == DBNull.Value ? "" : ds.Tables[1]
                                    .Rows[i]["CustomerTimeZoneName"]);
                                objHiddenFieldsModel.EmployeeId = Convert.ToString((ds.Tables[1].
                                    Rows[i]["EmployeeID"]) == DBNull.Value ? "" : ds.Tables[1].Rows[i]["EmployeeID"]);
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
                                objHiddenFieldsModel.IsDaily = Convert.ToInt32((ds.Tables[1].
                                    Rows[i]["IsDaily"]) == DBNull.Value ? "0" : ds.Tables[1].
                                    Rows[i]["IsDaily"]);

                                objHiddenFieldsModel.RoleName = Convert.ToString((ds.Tables[1].
                                    Rows[i]["RoleName"]) == DBNull.Value ? "0" : ds.Tables[1].Rows[i]["RoleName"]);
                            }
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
        /// This method is used to SaveLanguageForUserID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="Language"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SaveLanguageForUserID(BaseInformationModel objBasicDetails)
        //string EmployeeID, string Language, string UserID)
        {
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@EmployeeID", objBasicDetails.EmployeeId);
            prms[1] = new SqlParameter("@Language", objBasicDetails.Language);
            prms[2] = new SqlParameter("@UserID", objBasicDetails.UserId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[SaveLanguageByEmployee]", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Y";
        }
        /// <summary>
        /// This method is used to GetLanguageForUserlD
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public LanguageModel GetLanguageForUserlD(string EmployeeID)
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@ModuleName", "TM");

            LanguageModel objLanguageModel = new LanguageModel();

            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[GetLanguageByEmployee]", prms, ConnectionString);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            objLanguageModel.EmployeeId = Convert.ToString((ds.Tables[0].Rows[i]["EmployeeID"]) ==
                                DBNull.Value ? "Null" : ds.Tables[0].Rows[i]["EmployeeID"]);
                            objLanguageModel.Language = (ds.Tables[0].Rows[i]["Language"]) == DBNull.Value ? "0" :
                                ds.Tables[0].Rows[i]["Language"].ToString();
                        }
                    }

                    return objLanguageModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objLanguageModel;
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
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();

            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            string sPName = "[AVL].[SaveTicketingModuleData]";
            datasetResultSetObject.Tables.Add(ListExtensions.ToDataTable<SaveTicketingModuleTimeSheetDetails>
                (Data.SelectMany(x => x.TimesheetDetails).ToList()).Copy());
            if (datasetResultSetObject.Tables[0] != null && datasetResultSetObject.Tables[0].Rows.Count > 0)
            {
                datasetResultSetObject.Tables[0].TableName = "SaveTicketingModuleTimesheetDetails";
                var result = (from u in Data
                              select new SaveTicketDetails
                              {
                                  TicketId = u.TicketId,
                                  TicketDescription = u.TicketDescription,
                                  ServiceId = u.ServiceId,
                                  ActivityId = u.ActivityId,
                                  TicketType = u.TicketType,
                                  TicketStatus = u.TicketStatus,
                                  ITSMEffort = u.ITSMEffort,
                                  TotalEffort = u.TotalEffort,
                                  ProjectId = u.ProjectId,
                                  TimeTickerId = u.TimeTickerId,
                                  DARTStatusId = u.DARTStatusId,
                                  ApplicationId = u.ApplicationId,
                                  UserId = u.UserId,
                                  Type = u.Type

                                  //CustomerID=u.CustomerID
                              }).ToList();

                datasetResultSetObject.Tables.Add(ListExtensions.ToDataTable<SaveTicketDetails>(result));
            }
            if (datasetResultSetObject.Tables[1] != null && datasetResultSetObject.Tables[1].Rows.Count > 0)
            {
                datasetResultSetObject.Tables[1].TableName = "SaveTicketDetails";
            }

            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@SaveTimesheetDetails", datasetResultSetObject.Tables[0]);
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "AVL.SaveTimesheetDetails";
                prms[1] = new SqlParameter("@SaveTicketDetails", datasetResultSetObject.Tables[1]);
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = "AVL.SaveTicketDetails_TS";
                prms[2] = new SqlParameter("@Flag", Flag);
                prms[3] = new SqlParameter("@EmployeeID", EmployeeID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP(sPName, prms, ConnectionString);
                if (new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"] == "true")
                {
                    if (Flag == 2)
                    {
                        int SourceRecordId = 0;
                        if (datasetResultSetObject.Tables[0].Rows.Count > 0)
                        {
                            SourceRecordId = Convert.ToInt32(datasetResultSetObject.Tables[0].Rows[0]["CustomerId"]);
                        }

                        List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                        existingAcitivities = new MyActivity().GetExistingActivitys(SourceRecordId, new AppSettings().AppsSttingsKeyValues["DefaulterWorkItemCode"], access);
                        ExpiringActivitycall(existingAcitivities, datasetResultSetObject, EmployeeID, IsDaily, access);

                        List<ExistingAcitivityDetailsModel> existingActTimesheetUnfreeze = new List<ExistingAcitivityDetailsModel>();
                        existingActTimesheetUnfreeze = new MyActivity().GetExistingActivitys(SourceRecordId, new AppSettings().AppsSttingsKeyValues["TimesheetUnfreezeCode"], access);
                        ExpiringActivitycall(existingActTimesheetUnfreeze, datasetResultSetObject, EmployeeID, IsDaily, access);

                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
                return false;
            }

        }

        private void ExpiringActivitycall(List<ExistingAcitivityDetailsModel> existingAcitivities, DataSet datasetResultSetObject, string EmployeeID, string IsDaily, string access)
        {
            if (existingAcitivities != null && existingAcitivities.Count > 0)
            {
                if (IsDaily == "0")
                {
                    for (int i = 0; i < existingAcitivities.Count; i++)
                    {
                        if (!existingAcitivities[i].IsExpired && existingAcitivities[i].ActivityTo == EmployeeID
                        && DateTime.Parse(existingAcitivities[i].RequestorJson.Split(',')[0]).ToString
                        ("M/d/yyyy", CultureInfo.InvariantCulture) ==DateTime.Parse( datasetResultSetObject.Tables[0].Rows[0]["TimesheetDate"].ToString().Split(' ')[0]).ToString("M/d/yyyy"))
                        {
                            ActivityBasedExpiryModel expiryModel = new ActivityBasedExpiryModel();
                            expiryModel.ActivityID = existingAcitivities[i].ActivityID;
                            expiryModel.ActivityTo = EmployeeID;
                            expiryModel.ModifiedBy = "System";
                            string st = new MyActivity().ExpireBasedOnActivity(expiryModel, access);
                        }
                    }
                }
                else
                {


                    for (int u = 0; u < datasetResultSetObject.Tables[0].Rows.Count; u++)
                    {
                        var date = datasetResultSetObject.Tables[0].Rows[u]["TimesheetDate"].ToString().Split(' ')[0].ToList();
                    }

                    for (int i = 0; i < existingAcitivities.Count; i++)
                    {
                        for (int j = 0; j < datasetResultSetObject.Tables[0].Rows.Count; j++)
                        {
                            if (!existingAcitivities[i].IsExpired && existingAcitivities[i].ActivityTo == EmployeeID
                            && DateTime.Parse(existingAcitivities[i].RequestorJson).ToString("M/d/yyyy",
                            CultureInfo.InvariantCulture) == DateTime.Parse(datasetResultSetObject.Tables[0].Rows[j]["TimesheetDate"].ToString().Split(' ')[0]).ToString("M/d/yyyy"))
                            {
                                ActivityBasedExpiryModel expiryModel = new ActivityBasedExpiryModel();
                                expiryModel.ActivityID = existingAcitivities[i].ActivityID;
                                expiryModel.ActivityTo = EmployeeID;
                                expiryModel.ModifiedBy = "System";
                                string st = new MyActivity().ExpireBasedOnActivity(expiryModel, access);
                            }
                        }
                    }
                }
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
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            string sPName = "[AVL].[GetErroredTicketsCount]";
            try
            {
                SqlParameter[] prms = new SqlParameter[2];

                prms[0] = new SqlParameter("@employeeid", Employeeid);
                prms[1] = new SqlParameter("@customerid", customerid);
                DataSet ds = (new DBHelper()).GetDatasetFromSP(sPName, prms, ConnectionString);
                DataTable dt = ds.Tables[0];
                Int64 count = Convert.ToInt64(dt.Rows[0]["Count"]);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
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
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            string sPName = "[AVL].[SaveErroredTicketsData]";
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            Translation translation = new Translation();
            TicketUploadRepository upload = new TicketUploadRepository();
            List<ConcatenateStrings> lstTicket = new List<ConcatenateStrings>();
            var multilingualConfig = translation.GetProjectMultilinugalTranslateFields
                (customerId, projectId.ToString());
            if (multilingualConfig.IsMultilingualEnable.Equals(1) && multilingualConfig.ListTranslateFields.Any
                (translateField => translateField.ColumnName.
                Equals(ApplicationConstants.LanguageTranslateDescriptionColumn)))
            {
                var lang = multilingualConfig.ListTranslateLanguage.Where(c => c.IsSelected.Equals(true))
                    .Select(i => i.LanguageCode).ToArray();
                List<TicketDescriptionSummary> lstticketDescription = new List<TicketDescriptionSummary>();
                List<TicketSupportTypeMapping> lstTicketId = new List<TicketSupportTypeMapping>();
                lstTicketId = errorLogTicketData.Select(ticket => new TicketSupportTypeMapping
                { TicketId = ticket.TicketId, SupportType = SupportTypeID }).ToList();
                lstticketDescription = upload.GetTicketValues(lstTicketId, projectId.ToString(), employeeId);
                foreach (var errorTicket in errorLogTicketData)
                {
                    errorTicket.IsTicketDescriptionModified = string.IsNullOrEmpty(errorTicket.TicketDescription)
                        || (lstticketDescription.Any(sd => (sd.TicketId == errorTicket.TicketId)
                        && (sd.TicketDescription.Trim().Equals(errorTicket.TicketDescription.Trim()))))
                        ? false : true;
                    if (errorTicket.IsTicketDescriptionModified)
                    {
                        lstTicket.Add(new ConcatenateStrings
                        {
                            TimeTickerId = 0,
                            OriginalColumn = ApplicationConstants.TranslateTicketDescriptionColumn,
                            TranslatedColumn = ApplicationConstants.TranslatedTicketDescriptionColumn,
                            Text = errorTicket.TicketDescription.Trim(),
                            TranslatedText = null,
                            TextLength = errorTicket.TicketDescription.Length,
                            ErrorCol = errorTicket.TicketId,
                            SupportType = SupportTypeID
                        });
                    }
                }

                var translateResult = translation.PostToMSTranslatorList(lstTicket,
                    ApplicationConstants.TranslateDestinationLanguageCode, lang, multilingualConfig.SubscriptionKey);
                translateResult.Wait();
                if (!translateResult.Result.Any(item => item.HasError == true))
                {
                    ConcatenateStrings translateTicket = new ConcatenateStrings();
                    foreach (var errorTicket in errorLogTicketData)
                    {
                        translateTicket = translateResult.Result.Where(o => o.ErrorCol == errorTicket.TicketId)
                            .FirstOrDefault();
                        if (translateTicket != null)
                        {
                            errorTicket.MTicketDescription = translateTicket.TranslatedText;
                        }
                    }
                }
                else
                {
                    return (int)ApplicationEnum.ErrorTicketsResult.TranslationFailure;
                }
            }
            List<ErrorLogCorrection> result = new List<ErrorLogCorrection>();

            if (encryptionEnabled == "Enabled")
            {
                result = (from u in errorLogTicketData
                          select new ErrorLogCorrection
                          {
                              TicketId = u.TicketId,
                              TicketDescription = u.TicketDescription,
                              Application = u.Application.TrimStart(),
                              ApplicationId = u.ApplicationId,
                              TicketType = u.TicketType.TrimStart(),
                              TicketTypeId = u.TicketTypeId,
                              Priority = u.Priority.TrimStart(),
                              PriorityId = u.PriorityId,
                              Status = u.Status.TrimStart(),
                              StatusId = u.StatusId,
                              ProjectId = u.ProjectId,
                              SeverityId = u.SeverityId,
                              CauseCodeId = u.CauseCodeId,
                              ResolutionId = u.ResolutionId,
                              Severity = u.Severity.TrimStart(),
                              CauseCode = u.CauseCode.TrimStart(),
                              ResolutionCode = u.ResolutionCode.TrimStart(),
                              DebtClassificationId = u.DebtClassificationId,
                              AvoidableFlagId = u.AvoidableFlagId,
                              ResidualDebtId = u.DebtClassificationId,
                              DebtClassification = u.DebtClassification.TrimStart(),
                              AvoidableFlag = u.AvoidableFlag.TrimStart(),
                              ResidualDebt = u.ResidualDebt.TrimStart(),
                              OpenDate = u.OpenDate,
                              AssignmentGroupId = u.AssignmentGroupId,
                              TowerId = u.TowerId,
                              MTicketDescription = u.MTicketDescription,
                              IsTicketDescriptionModified = u.IsTicketDescriptionModified,
                              IsPartiallyAutomated = u.IsPartiallyAutomated
                          }).ToList();
                byte[] bytesEncrypted;
                string encTicketDesc;
                foreach (var TD in result)
                {
                    if (!string.IsNullOrEmpty(TD.TicketDescription))
                    {
                        bytesEncrypted = aesMod.EncryptStringAsBytes(TD.TicketDescription,
                            AseKeyDetail.AesKeyConstVal);
                        TD.TicketDescription = Convert.ToBase64String(bytesEncrypted);
                    }

                    if (!string.IsNullOrEmpty(TD.MTicketDescription))
                    {
                        bytesEncrypted = aesMod.EncryptStringAsBytes(TD.MTicketDescription,
                            AseKeyDetail.AesKeyConstVal);
                        TD.MTicketDescription = Convert.ToBase64String(bytesEncrypted);
                    }
                }

            }
            else
            {
                result = (from u in errorLogTicketData
                          select new ErrorLogCorrection
                          {
                              TicketId = u.TicketId,
                              TicketDescription = u.TicketDescription.TrimStart(),
                              Application = u.Application.TrimStart(),
                              ApplicationId = u.ApplicationId,
                              TicketType = u.TicketType.TrimStart(),
                              TicketTypeId = u.TicketTypeId,
                              Priority = u.Priority.TrimStart(),
                              PriorityId = u.PriorityId,
                              Status = u.Status.TrimStart(),
                              StatusId = u.StatusId,
                              ProjectId = u.ProjectId,
                              SeverityId = u.SeverityId,
                              CauseCodeId = u.CauseCodeId,
                              ResolutionId = u.ResolutionId,
                              Severity = u.Severity.TrimStart(),
                              CauseCode = u.CauseCode.TrimStart(),
                              ResolutionCode = u.ResolutionCode.TrimStart(),
                              DebtClassificationId = u.DebtClassificationId,
                              AvoidableFlagId = u.AvoidableFlagId,
                              ResidualDebtId = u.DebtClassificationId,
                              DebtClassification = u.DebtClassification.TrimStart(),
                              AvoidableFlag = u.AvoidableFlag.TrimStart(),
                              ResidualDebt = u.ResidualDebt.TrimStart(),
                              OpenDate = u.OpenDate,
                              AssignmentGroupId = u.AssignmentGroupId,
                              TowerId = u.TowerId,
                              MTicketDescription = u.MTicketDescription,
                              IsTicketDescriptionModified = u.IsTicketDescriptionModified,
                              IsPartiallyAutomated = u.IsPartiallyAutomated
                          }).ToList();
            }

            datasetResultSetObject.Tables.Add(ListExtensions.ToDataTable<ErrorLogCorrection>(result));
            datasetResultSetObject.Tables[0].TableName = "ErrorLogCorrection";
            DataTable t = datasetResultSetObject.Tables[0];
            t.Columns.Remove("Assignee");
            t.Columns.Remove("IsManual");
            t.Columns.Remove("EmployeeID");
            t.Columns.Remove("EmployeeName");
            t.Columns.Remove("ExternalLoginID");
            t.Columns.Remove("OpenDate2");
            t.Columns.Remove("IsDeleted");
            DataTable newTable = t.DefaultView.ToTable(false,
                    "TicketID",
                    "TicketDescription",
                    "ApplicationID",
                    "Application",
                    "TicketTypeID",
                    "TicketType",
                     "Priority",
                      "PriorityID",
                     "StatusID",
                       "Status",
                      "severityID",
                         "Severity",
                        "CauseCodeID",
                      "Causecode",
                  "ResolutionID",
                      "ResolutionCode",
                    "DebtClassificationId",
            "DebtClassification",
             "AvoidableFlagID",
               "AvoidableFlag",
                                    "ResidualDebtID",
                                  "ResidualDebt",
                                 "OpenDate",
                                    "ProjectID",
                                    "AssignmentGroupID",
                                    "TowerID",
                                    "MTicketDescription",
                                    "IsTicketDescriptionModified",
                                    "IsPartiallyAutomated");



            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@SaveErrorLogTicketDetails", newTable);
                prms[0].SqlDbType = SqlDbType.Structured;
                //Existing Prod Issue.--Fixed
                prms[0].TypeName = "[AVL].[SaveErrorLogTicketDetails]";
                prms[1] = new SqlParameter("@ProjectID", projectId);
                prms[2] = new SqlParameter("@EmployeeID", employeeId);
                prms[3] = new SqlParameter("@SupportTypeID", SupportTypeID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP(sPName, prms, ConnectionString);
                return (int)ApplicationEnum.ErrorTicketsResult.Success;
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
            DebtEnabledFields dbEnabledFields = new DebtEnabledFields();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[GetDebtConfiguration]", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                dbEnabledFields.IsDebtEnabled = dt.Rows[0][0] != DBNull.Value ? Convert.ToString(dt.
                    Rows[0][0]) == "Y" ? true : false : false;
                if (dbEnabledFields.IsDebtEnabled == false)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        string ColumnValue = dt.Rows[i]["ServiceDartColumn"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[i]["ServiceDartColumn"]) : string.Empty;
                        if (ColumnValue.ToUpper() == "RESOLUTION CODE")
                        {
                            dbEnabledFields.ResolutionCode = true;
                        }
                        else if (ColumnValue.ToUpper() == "CAUSE CODE")
                        {
                            dbEnabledFields.Causecode = true;
                        }
                        else if (ColumnValue.ToUpper() == "DEBT CLASSIFICATION")
                        {
                            dbEnabledFields.DebtClassification = true;
                        }
                        else if (ColumnValue.ToUpper() == "AVOIDABLE FLAG")
                        {
                            dbEnabledFields.AvoidableFlag = true;
                        }
                        else if (ColumnValue.ToUpper() == "RESIDUAL DEBT")
                        {
                            dbEnabledFields.ResidualDebt = true;
                        }
                        else
                        {
                            //mandatory else
                        }

                    }
                }
            }
            return dbEnabledFields;
        }
        /// <summary>
        /// This Method Is Used To GetErrorLogTicketData
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>S
        public List<ErrorLogCorrection> GetErrorLogTicketData(int ProjectID, string EmployeeID, int SupportTypeID)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            List<ErrorLogCorrection> objErrorLogData = new List<ErrorLogCorrection>();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@projectid", ProjectID);
            prms[1] = new SqlParameter("@employeeid", EmployeeID);
            prms[2] = new SqlParameter("@SupportTypeID", SupportTypeID);
            DataTable dt = (new DBHelper()).GetTableFromSP("[dbo].[sp_GetErroredTickets]", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    ErrorLogCorrection objresult = new ErrorLogCorrection();
                    objresult.TicketId = dt.Rows[i]["Ticket ID"] != DBNull.Value ?
                        Convert.ToString(dt.Rows[i]["Ticket ID"]) : string.Empty;
                    if (encryptionEnabled == "Enabled")
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Ticket Description"])))
                        {


                            string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Rows[i]
                                ["Ticket Description"], AseKeyDetail.AesKeyConstVal);
                            string decTicketDesc = bytesDecrypted;
                            objresult.TicketDescription = decTicketDesc;
                        }
                        else
                        {
                            objresult.TicketDescription = dt.Rows[i]["Ticket Description"] !=
                                DBNull.Value ? Convert.ToString(dt.Rows[i]["Ticket Description"]) : string.Empty;
                        }
                    }
                    else
                    {
                        objresult.TicketDescription = dt.Rows[i]["Ticket Description"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[i]["Ticket Description"]) : string.Empty;
                    }

                    objresult.ApplicationId = dt.Rows[i]["ApplicationID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["ApplicationID"]) : 0;
                    objresult.Application = dt.Rows[i]["Application"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Application"]) : string.Empty;
                    objresult.TicketTypeId = dt.Rows[i]["TicketTypeID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["TicketTypeID"]) : 0;
                    objresult.TicketType = dt.Rows[i]["Ticket Type"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Ticket Type"]) : string.Empty;
                    objresult.ResolutionId = dt.Rows[i]["ResolutionID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["ResolutionID"]) : 0;
                    objresult.ResolutionCode = dt.Rows[i]["Resolution Code"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Resolution Code"]) : string.Empty;
                    objresult.DebtClassificationId = dt.Rows[i]["DebtClassificationId"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["DebtClassificationId"]) : 0;
                    objresult.DebtClassification = dt.Rows[i]["Debt Classification"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Debt Classification"]) : string.Empty;
                    objresult.CauseCodeId = dt.Rows[i]["CauseCodeID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["CauseCodeID"]) : 0;
                    objresult.CauseCode = dt.Rows[i]["Cause code"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Cause code"]) : string.Empty;
                    objresult.AvoidableFlagId = dt.Rows[i]["AvoidableFlagID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["AvoidableFlagID"]) : 0;
                    objresult.AvoidableFlag = dt.Rows[i]["Avoidable Flag"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Avoidable Flag"]) : string.Empty;
                    objresult.ResidualDebtId = dt.Rows[i]["ResidualDebtID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["ResidualDebtID"]) : 0;
                    objresult.ResidualDebt = dt.Rows[i]["Residual Debt"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Residual Debt"]) : string.Empty;
                    objresult.SeverityId = dt.Rows[i]["severityID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["severityID"]) : 0;
                    objresult.Severity = dt.Rows[i]["Severity"] != DBNull.Value ? Convert
                        .ToString(dt.Rows[i]["Severity"]) : string.Empty;
                    objresult.PriorityId = dt.Rows[i]["PriorityID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["PriorityID"]) : 0;
                    objresult.Priority = dt.Rows[i]["Priority"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Priority"]) : string.Empty;
                    objresult.StatusId = dt.Rows[i]["StatusID"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["StatusID"]) : 0;
                    objresult.Status = dt.Rows[i]["Status"] != DBNull.Value ? Convert.
                        ToString(dt.Rows[i]["Status"]) : string.Empty;
                    if (dt.Rows[i]["Open Date"] != DBNull.Value)
                    {
                        objresult.OpenDate = Convert.ToDateTime(dt.Rows[i]["Open Date"]);
                        objresult.OpenDate2 = objresult.OpenDate.ToString("MM/dd/yyyy HH:mm:ss");
                    }
                    objresult.TowerId = dt.Rows[i]["TowerID"] != DBNull.Value ? Convert.
                     ToInt32(dt.Rows[i]["TowerID"]) : 0;
                    objresult.Tower = dt.Rows[i]["TowerName"] != DBNull.Value ? Convert.
                      ToString(dt.Rows[i]["TowerName"]) : string.Empty;
                    objresult.AssignmentGroupId = dt.Rows[i]["Assignment Group ID"] != DBNull.Value ? Convert.
                      ToInt32(dt.Rows[i]["Assignment Group ID"]) : 0;
                    objresult.AssignmentGroup = dt.Rows[i]["Assignment Group"] != DBNull.Value ? Convert.
                      ToString(dt.Rows[i]["Assignment Group"]) : string.Empty;
                    objresult.IsAHTicket = dt.Rows[i]["IsAHTicket"] != DBNull.Value ? Convert.
                      ToBoolean(dt.Rows[i]["IsAHTicket"]) : false;
                    objresult.IsPartiallyAutomated = dt.Rows[i]["IsPartiallyAutomated"] != DBNull.Value ? Convert.
                        ToInt32(dt.Rows[i]["IsPartiallyAutomated"]) : 0;
                    objErrorLogData.Add(objresult);
                }
            }
            return objErrorLogData;
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

            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            string isAttributeUpdated = "";
            try
            {
                if (updateIsAttribute.SupportTypeId == 1)
                {
                    SqlParameter[] prms = new SqlParameter[5];
                    prms[0] = new SqlParameter("@ProjectId", updateIsAttribute.ProjectId);
                    prms[1] = new SqlParameter("@TicketID", updateIsAttribute.TicketId);
                    prms[2] = new SqlParameter("@ServiceID", updateIsAttribute.ServiceId);
                    prms[3] = new SqlParameter("@TicketStatusID", updateIsAttribute.DARTStatusId);
                    prms[4] = new SqlParameter("@TicketTypeID", updateIsAttribute.TicketTypeMapId);

                    if (updateIsAttribute.IsCognizant == "1")
                    {
                        ds = (new DBHelper()).GetDatasetFromSP("[AVL].[TK_UpdateIsAttributeUpdatedCognizant]", prms, ConnectionString);
                    }
                    else
                    {
                        ds = (new DBHelper()).GetDatasetFromSP("[AVL].[TK_UpdateIsAttributeUpdatedCustomer]", prms, ConnectionString);
                    }
                }
                else
                {
                    SqlParameter[] prms = new SqlParameter[3];
                    prms[0] = new SqlParameter("@ProjectId", updateIsAttribute.ProjectId);
                    prms[1] = new SqlParameter("@TicketID", updateIsAttribute.TicketId);
                    prms[2] = new SqlParameter("@TicketStatusID", updateIsAttribute.DARTStatusId);
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[TK_UpdateIsAttributeInfra]", prms, ConnectionString);
                }
                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        isAttributeUpdated = Convert.ToString(ds.Tables[1].Rows[0]["IsAttributeUpdated"]);
                    }
                    else if (ds.Tables[0].Rows.Count > 0)
                    {
                        isAttributeUpdated = Convert.ToString(ds.Tables[0].Rows[0]["IsAttributeUpdated"]);
                    }
                    else
                    {
                        //Mandatory else block
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            return isAttributeUpdated;
        }
        /// <summary>
        /// This Method Is Used To GetTicketAttributeDetails
        /// </summary>
        /// <param name="objTicketAttributes"></param>
        /// <returns></returns>
        public List<TicketAttributesModel> GetTicketAttributeDetails(TicketAttributesModel objTicketAttributes)
        {
            List<TicketAttributesModel> attributeDetails = new List<TicketAttributesModel>();
            DataTable dtResult = new DataTable();
            dtResult.Locale = CultureInfo.InvariantCulture;
            if (objTicketAttributes.SupportTypeId == 1)
            {
                try
                {
                    SqlParameter[] prms = new SqlParameter[5];
                    prms[0] = new SqlParameter("@ProjectId", objTicketAttributes.ProjectId);
                    prms[1] = new SqlParameter("@serviceid", objTicketAttributes.ServiceId);
                    prms[2] = new SqlParameter("@DARTStatusID", objTicketAttributes.DARTStatusId);
                    prms[3] = new SqlParameter("@TicketStatusID", objTicketAttributes.TicketStatusID);
                    prms[4] = new SqlParameter("@TicketTypeID", objTicketAttributes.TicketTypeID);

                    if (objTicketAttributes.IsCognizant == "1")
                    {
                        dtResult = (new DBHelper()).GetTableFromSP("[AVL].[TK_GetTicketAttributeCognizant]", prms, ConnectionString);
                    }
                    else
                    {
                        dtResult = (new DBHelper()).GetTableFromSP("[AVL].[TK_GetTicketAttributeCustomer]", prms, ConnectionString);
                    }
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtResult.Rows.Count; i++)
                        {
                            TicketAttributesModel attrmodel = new TicketAttributesModel();
                            attrmodel.ServiceId = Convert.ToInt64(dtResult.Rows[i]["ServiceID"].ToString());
                            attrmodel.AttributeName = dtResult.Rows[i]["AttributeName"].ToString();
                            attrmodel.ColumnMappingName = dtResult.Rows[i]["ColumnMappingName"].ToString();
                            attrmodel.ProjectStatusId = dtResult.Rows[i]["ProjectStatusID"].ToString();
                            attrmodel.DARTStatusId = Convert.ToInt32(dtResult.Rows[i]["DARTStatusID"].ToString());
                            attrmodel.AttributeType = dtResult.Rows[i]["AttributeType"].ToString();
                            attributeDetails.Add(attrmodel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {

                    SqlParameter[] prms = new SqlParameter[3];
                    prms[0] = new SqlParameter("@ProjectId", objTicketAttributes.ProjectId);
                    prms[1] = new SqlParameter("@DARTStatusID", objTicketAttributes.DARTStatusId);
                    prms[2] = new SqlParameter("@TicketStatusID", objTicketAttributes.TicketStatusID);

                    dtResult = (new DBHelper()).GetTableFromSP("[AVL].[TK_GetInfraTicketAttributes]", prms, ConnectionString);

                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtResult.Rows.Count; i++)
                        {
                            TicketAttributesModel attrmodel = new TicketAttributesModel();
                            attrmodel.AttributeName = dtResult.Rows[i]["AttributeName"].ToString();
                            attrmodel.ColumnMappingName = dtResult.Rows[i]["ColumnMappingName"].ToString();
                            attrmodel.AttributeType = dtResult.Rows[i]["AttributeType"].ToString();

                            attributeDetails.Add(attrmodel);
                        }

                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return attributeDetails;

        }

        /// <summary>
        /// This Method Is Used To GetApplicationsByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<ApplicationsModel> GetApplicationsByProject(Int64 ProjectID)
        {

            SqlParameter[] prms = new SqlParameter[1];

            prms[0] = new SqlParameter("@ProjectID", ProjectID);


            List<ApplicationsModel> lstApplicationsModel = new List<ApplicationsModel>();
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetApplicationDetailsByProjectID]",
                    prms, ConnectionString);
                if (dt != null)
                {
                    lstApplicationsModel = dt.AsEnumerable().Select(x => new ApplicationsModel
                    {
                        ApplicationId = x["ApplicationID"] != DBNull.Value ? Convert.ToInt64(x["ApplicationID"]) : 0,
                        ApplicationName = x["ApplicationName"] != DBNull.Value ? x["ApplicationName"].ToString() :
                        string.Empty,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstApplicationsModel;
        }
        /// <summary>
        /// To get Assignment Group by project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>


        public List<AssignmentGroupModel> GetAssignmentGroupByProjectID(Int64 ProjectID, string UserID)
        {

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@UserID", UserID);
            List<AssignmentGroupModel> lstAssignmentGroup = new List<AssignmentGroupModel>();
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[GetAssignmentGroupByProjectID]", prms, ConnectionString);
                if (dt != null)
                {
                    lstAssignmentGroup = dt.AsEnumerable().Select(x => new AssignmentGroupModel
                    {
                        AssignmentGroupMapId = x["AssignmentGroupMapID"] != DBNull.Value ?
                        Convert.ToInt64(x["AssignmentGroupMapID"]) : 0,
                        AssignmentGroupName = x["AssignmentGroupName"] != DBNull.Value ?
                        x["AssignmentGroupName"].ToString() : string.Empty,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstAssignmentGroup;
        }

        /// <summary>
        /// Get Tower details list based on project ID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<TowerDetailsModel> GetTowerDetailsByProjectID(Int64 ProjectID, Int64 CustomerID)
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            List<TowerDetailsModel> lstTowerDetails = new List<TowerDetailsModel>();
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("AVL.GetTowerByProjectID",
                    prms, ConnectionString);
                if (dt != null)
                {
                    lstTowerDetails = dt.AsEnumerable().Select(x => new TowerDetailsModel
                    {
                        TowerId = x["TowerID"] != DBNull.Value ? Convert.ToInt16(x["TowerID"]) : 0,
                        TowerName = x["TowerName"] != DBNull.Value ? x["TowerName"].ToString() :
                        string.Empty,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTowerDetails;
        }
        /// <summary>
        /// This Method Is Used To GetProjectsByCustomer
        /// </summary>
        /// <param name="CustomerID"></parama
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<ProjectsModel> GetProjectsByCustomer(string CustomerID, string EmployeeID)
        {

            SqlParameter[] prms = new SqlParameter[2];

            prms[0] = new SqlParameter("@CustomerID", CustomerID);
            prms[1] = new SqlParameter("@EmployeeID", EmployeeID);

            List<ProjectsModel> lstProjectModel = new List<ProjectsModel>();
            try
            {
                if (CustomerID != null)
                {
                    DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetProjectDetailsByAccountID]", prms, ConnectionString);
                    if (dt != null)
                    {
                        lstProjectModel = dt.AsEnumerable().Select(x => new ProjectsModel
                        {
                            ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"]) : 0,
                            ProjectName = x["ProjectName"] != DBNull.Value ? x["ProjectName"].ToString() : string.Empty,
                            UserTimeZoneName = x["UserTimeZoneName"] != DBNull.Value ? x["UserTimeZoneName"].ToString() :
                            string.Empty,
                            ProjectTimeZoneName = x["ProjectTimeZoneName"] != DBNull.Value ? x["ProjectTimeZoneName"].
                            ToString() : string.Empty,
                            ProjectTimeZoneId = x["ProjectTimeZoneID"] != DBNull.Value ? Convert.ToInt64(x["ProjectID"])
                            : 0,
                            UserTimeZoneId = x["UserTimeZoneID"] != DBNull.Value ? Convert.ToInt64(x["UserTimeZoneID"])
                            : 0,
                            SupportTypeId = x["SupportTypeId"] != DBNull.Value ? Convert.ToInt32(x["SupportTypeId"])
                            : 0,
                        }).ToList();

                        lstProjectModel = lstProjectModel.OrderBy(x => x.ProjectName).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstProjectModel;
        }
        /// This Method Is Used To GetTicketStatusByProject
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<StatusDetails> GetTicketStatusByProject(Int64 ProjectID)
        {
            List<StatusDetails> lsTicketStatusDetails = new List<StatusDetails>();

            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[1];

                prms[0] = new SqlParameter("@ProjectID", ProjectID);

                dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetProjectBasedTicketStatusDetails]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lsTicketStatusDetails = dt.AsEnumerable().Select(x => new StatusDetails
                    {
                        TicketStatusId = x["StatusID"] != DBNull.Value ? Convert.ToInt64(x["StatusID"]) : 0,
                        StatusName = x["StatusName"] != DBNull.Value ? x["StatusName"].ToString() : string.Empty,
                        Type = x["Type"] != DBNull.Value ? Convert.ToChar(x["Type"]) : 'T'
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lsTicketStatusDetails;
        }
        /// <summary>
        /// This Method Is Used To GetTimeZoneInformationByCustomer
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public TimeZoneInfoByCustomer GetTimeZoneInformationByCustomer(string EmployeeID, Int64 CustomerID)
        {
            TimeZoneInfoByCustomer objTimeZoneInfoByCustomer = new TimeZoneInfoByCustomer();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[1] = new SqlParameter("@CustomerID", CustomerID);

                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[Effort_GetTimeZoneInformationByCustomer]",
                    prms, ConnectionString);
                objTimeZoneInfoByCustomer.UserTimeZoneId = Convert.ToString((dt.Rows[0]["UserTimeZoneId"])
                    == DBNull.Value ? DBNull.Value : dt.Rows[0]["UserTimeZoneId"]);
                objTimeZoneInfoByCustomer.UserTimeZoneName = Convert.ToString((dt.Rows[0]["UserTimeZoneName"])
                    == DBNull.Value ? DBNull.Value : dt.Rows[0]["UserTimeZoneName"]);
                objTimeZoneInfoByCustomer.ProjectTimeZoneId = Convert.ToString((dt.Rows[0]["ProjectTimeZoneId"])
                    == DBNull.Value ? DBNull.Value : dt.Rows[0]["ProjectTimeZoneId"]);
                objTimeZoneInfoByCustomer.ProjectTimeZoneName = Convert.ToString((dt.Rows[0]["ProjectTimeZoneName"])
                    == DBNull.Value ? DBNull.Value : dt.Rows[0]["ProjectTimeZoneName"]);
                objTimeZoneInfoByCustomer.TimeSheetDisplayMessage = Convert.ToString((dt.
                    Rows[0]["TimeSheetDisplayMessage"]) == DBNull.Value ? DBNull.Value : dt.
                    Rows[0]["TimeSheetDisplayMessage"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTimeZoneInfoByCustomer;
        }
        /// <summary>
        /// This Method Is Used To GetProjectName
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public string GetProjectName(string ProjectID)
        {
            string projectName = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[1];

                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt32(ProjectID));

                dt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    projectName = dt.Rows[0]["ProjectName"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return projectName;
        }
        /// <summary>
        /// This Method Is Used To GetProjectNameESAID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<GetProjectNameEsaID> GetProjectNameESAID(string ProjectID)
        {
            List<GetProjectNameEsaID> projectDetails = new List<GetProjectNameEsaID>();
            string projectName = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[1];

                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt32(ProjectID));

                dt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GetProjectNameEsaID detail = new GetProjectNameEsaID();
                    detail.ProjectName = dt.Rows[0]["ProjectName"].ToString();
                    detail.ESAId = dt.Rows[0]["EsaProjectID"].ToString();
                    projectDetails.Add(detail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return projectDetails;
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
                List<Roles> rolesList = new List<Roles>();
                SqlParameter[] prmsObj = new SqlParameter[3];
                prmsObj[0] = new SqlParameter("@Mode", mode);
                prmsObj[1] = new SqlParameter("@UserId", userId);
                prmsObj[2] = new SqlParameter("@CustomerId", customerId);
                var dtRoles = (new DBHelper()).GetTableFromSP("GetRoles", prmsObj, ConnectionString);
                if (dtRoles != null)
                {
                    Roles objRoles;
                    if (dtRoles.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtRoles.Rows.Count; i++)
                        {
                            objRoles = new Roles();
                            objRoles.RoleId
                                = Convert.ToInt32((dtRoles.Rows[i]["RoleId"]));
                            objRoles.RoleName
                                = Convert.ToString((dtRoles.Rows[i]["RoleName"]));
                            objRoles.Priority
                                = Convert.ToInt32((dtRoles.Rows[i]["Priority"]));
                            rolesList.Add(objRoles);
                        }
                    }
                }
                return rolesList;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
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
            decimal mandatoryhours = 0;
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@CustomerID", Convert.ToInt32(CustomerId));
                prms[1] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));

                dt = (new DBHelper()).GetTableFromSP("[AVL].[User_MandatoryHours]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["MandatoryHours"].ToString() != "")
                {
                    mandatoryhours = Convert.ToDecimal(dt.Rows[0]["MandatoryHours"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mandatoryhours;
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
                SqlParameter[] prm = new SqlParameter[9];
                prm[0] = new SqlParameter("@TimeTickerID", TimeTickerID);
                prm[1] = new SqlParameter("@Ruleid", Ruleid);
                prm[2] = new SqlParameter("@UserID", UserID);
                prm[3] = new SqlParameter("@SupportTypeID", SupportTypeID);
                prm[4] = new SqlParameter("@lwruleid", null);
                prm[5] = new SqlParameter("@lwrulelevel", null);
                prm[6] = new SqlParameter("@ProjectID", projectId);
                prm[7] = new SqlParameter("@clusterDesc", clusterDesc);
                prm[8] = new SqlParameter("@clusterReso", clusterReso);

                (new DBHelper()).ExecuteNonQuery("[AVL].[InsertRuleIDforticketdetail]", prm, ConnectionString);
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
            Utility.ErrorLOG("Autoclassification-1", "AC_Methodstart", 0, UserID);
            Responseclass responseObject = new Responseclass();
            try
            {
                var clientApiURL = "";
                if (SupportTypeID == 1)
                {
                    clientApiURL = new AppSettings().AppsSttingsKeyValues["ClientApiURL"];
                }
                else
                {
                    clientApiURL = new AppSettings().AppsSttingsKeyValues["ClientApiURLInfra"];
                }
                string apiurlmethod = "";

                if (SupportTypeID == 1)
                {
                    apiurlmethod = AutoClassificationtype == 1 ? ApplicationConstants.ApplicationAPIURL : ApplicationConstants.ApplicationCcRcAPIURL;
                }
                else
                {
                    apiurlmethod = AutoClassificationtype == 1 ? ApplicationConstants.InfraAPIURL : ApplicationConstants.InfraCcRcAPIURL;
                }

                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    client.BaseAddress = new Uri(clientApiURL);
                    Utility.ErrorLOG("Autoclassification-2", "AC_Going to start API", 0, UserID);
                    Utility.ErrorLOG("Autoclassification-3", "API Request Information", 0, UserID);

                    if (SupportTypeID == 1)
                    {
                        switch (AutoClassificationtype)
                        {
                            case 1:
                                {
                                    var requestClass = new Requestclass
                                    {
                                        Projectid = ProjectID,
                                        Appid = ApplicationID,
                                        Causeid = CauseCode,
                                        Resolutionid = ResolutionCode,
                                        Desc_text = TicketDescription,
                                        Add_text = add_text
                                    };

                                    this.CaptureLogDetails(UserID, "Requestclass", requestClass.ToString());
                                    string url = Path.Combine(clientApiURL, apiurlmethod);
                                    var dataAsString = JsonConvert.SerializeObject(requestClass);
                                    var content = new StringContent(dataAsString);
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                                    postTask.Wait();
                                    responseObject = postTask.Result.Content.ReadAsJsonAsync<Responseclass>().Result;
                                    break;
                                }
                            case 2:
                                {
                                    var requestClass = new RequestTypeclass
                                    {
                                        Projectid = ProjectID,
                                        Appid = ApplicationID,
                                        Desc_text = TicketDescription,
                                        Add_text = add_text
                                    };

                                    this.CaptureLogDetails(UserID, "RequestTypeclass", requestClass.ToString());
                                    string url = Path.Combine(clientApiURL, apiurlmethod);
                                    var dataAsString = JsonConvert.SerializeObject(requestClass);
                                    var content = new StringContent(dataAsString);
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                                    postTask.Wait();
                                    responseObject = postTask.Result.Content.ReadAsJsonAsync<Responseclass>().Result;
                                    break;
                                }
                            default:
                                //CCAP FIX
                                break;

                        }
                    }
                    else if (SupportTypeID == 2)
                    {
                        switch (AutoClassificationtype)
                        {
                            case 1:
                                {
                                    var requestClass = new RequestclassInfra
                                    {
                                        Projectid = ProjectID,
                                        Towerid = TowerID,
                                        Causeid = CauseCode,
                                        Resolutionid = ResolutionCode,
                                        Desc_text = TicketDescription,
                                        Add_text = add_text,
                                    };

                                    this.CaptureLogDetails(UserID, "RequestclassInfra", requestClass.ToString());
                                    var dataAsString = JsonConvert.SerializeObject(requestClass);
                                    var content = new StringContent(dataAsString);
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var postTask = client.PostAsync(new SanitizeString(apiurlmethod).Value, content);
                                    postTask.Wait();
                                    responseObject = postTask.Result.Content.ReadAsJsonAsync<Responseclass>().Result;
                                    break;
                                }
                            case 2:
                                {
                                    var requestClass = new RequestTypeclassInfra
                                    {
                                        Projectid = ProjectID,
                                        Towerid = TowerID,
                                        Desc_text = TicketDescription,
                                        Add_text = add_text
                                    };

                                    this.CaptureLogDetails(UserID, "RequestTypeclassInfra", requestClass.ToString());
                                    var dataAsString = JsonConvert.SerializeObject(requestClass);
                                    var content = new StringContent(dataAsString);
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var postTask = client.PostAsync(new SanitizeString(apiurlmethod).Value, content);
                                    postTask.Wait();
                                    responseObject = postTask.Result.Content.ReadAsJsonAsync<Responseclass>().Result;
                                    break;
                                }
                            default:
                                //CCAP FIX
                                break;
                        }
                    }
                    else
                    {
                        //mandatory else
                    }
                }

                string Logrequest = SupportTypeID == 1 ? "application/json={" + "Projectid:" + ProjectID + "," +
                    "AppID:" + ApplicationID + "," + "Causeid:" + CauseCode + "," + "Resolutionid:" + ResolutionCode +
                    "," + "Desc_text:" + "''" + "," + "Add_text:" + "''" + "}"
                    : "application/json={" + "Projectid:" + ProjectID + "Towerid:" + TowerID + "," + "Causeid:" +
                    CauseCode + "," + "Resolutionid:" + ResolutionCode + "," + "Desc_text:" + "''" + "," + "Add_text:" + "''" + "}";

                Utility.ErrorLOG("Autoclassification-5", "AC_API successfully called", 0, UserID);
                Utility.ErrorLOG("Autoclassification-6", responseObject.ToString(), 0, UserID);

                SqlParameter[] prm = new SqlParameter[5];
                prm[0] = new SqlParameter("@ProjectID", ProjectID);
                prm[1] = new SqlParameter("@UserID", UserID);
                prm[2] = new SqlParameter("@request", Logrequest);
                prm[3] = new SqlParameter("@responce", responseObject.ToString());
                prm[4] = new SqlParameter("@Mode", "AddTicket");

                (new DBHelper()).ExecuteNonQuery("[dbo].[CaptureAPIRequestResponce]", prm, ConnectionString);
                bool x1 = responseObject.Debt == "" || responseObject.Debt == null;
                bool x2 = responseObject.Avoidable == "" || responseObject.Avoidable == null;
                bool x3 = responseObject.Residual == "" || responseObject.Residual == null;

                if ((x1) && (x2) && (x3))
                {
                    responseObject = null;
                }
                else
                {
                    TicketingModuleRepository objAutoClassificationMode = new TicketingModuleRepository();
                    SqlParameter[] prms = new SqlParameter[9];
                    prms[0] = new SqlParameter("@ProjectID", ProjectID);
                    prms[1] = new SqlParameter("@TimeTickerID", TimeTickerID);
                    prms[2] = new SqlParameter("@UserID", UserID);
                    prms[3] = new SqlParameter("@debt", responseObject.Debt);
                    prms[4] = new SqlParameter("@avoidable", responseObject.Avoidable);
                    prms[5] = new SqlParameter("@residual", responseObject.Residual);
                    prms[6] = new SqlParameter("@SupportTypeID", SupportTypeID);
                    prms[7] = new SqlParameter("@causecode", !string.IsNullOrEmpty(responseObject.CauseCode) ?
                                                                                responseObject.CauseCode : null);
                    prms[8] = new SqlParameter("@resolutioncode", !string.IsNullOrEmpty(responseObject.ResolutionCode) ?
                                                            responseObject.ResolutionCode : null);
                    (new DBHelper()).ExecuteNonQuery("[dbo].[InsertDebtClassificationModeDetails_ML]", prms, ConnectionString);
                }

                if (TimeTickerID != "" && responseObject != null &&
                    (responseObject.Ruleid != "" || responseObject.Lw_ruleid != ""))
                {
                    SqlParameter[] prms = new SqlParameter[9];
                    prms[0] = new SqlParameter("@TimeTickerID", Convert.ToInt64(TimeTickerID));
                    prms[1] = new SqlParameter("@Ruleid", string.IsNullOrEmpty(responseObject.Ruleid) ? "0" :
                        responseObject.Ruleid);
                    prms[2] = new SqlParameter("@UserId", UserID);
                    prms[3] = new SqlParameter("@SupportTypeID", (SupportTypeID != null && SupportTypeID > 0) ?
                        SupportTypeID : 4);
                    prms[4] = new SqlParameter("@lwruleid", string.IsNullOrEmpty(responseObject.Lw_ruleid) ?
                        null : responseObject.Lw_ruleid);
                    prms[5] = new SqlParameter("@lwrulelevel", string.IsNullOrEmpty(responseObject.Lw_rulelevel)
                        ? null : responseObject.Lw_rulelevel);
                    prms[6] = new SqlParameter("@ProjectID", ProjectID);
                    prms[7] = new SqlParameter("@clusterDesc", null);
                    prms[8] = new SqlParameter("@clusterReso", null);
                    (new DBHelper()).ExecuteNonQuery("[AVL].[InsertRuleIDforticketdetail]", prms, ConnectionString);
                }

            }
            catch (Exception ex)
            {
                Utility.ErrorLOG("Autoclassification-Error", "Error Occured", 0, UserID);
                Utility.ErrorLOG("Autoclassification-Error", ex.Message, 0, UserID);
                Utility.ErrorLOG("Autoclassification-Error", ex.InnerException.Message, 0, UserID);
                throw ex;
            }
            return responseObject;
        }


        public ResponseClassNewAlgo NewAlgoClassification(string jsonParams, string UserID, int projectId, int supportTypeId, string timeTickerId)
        {
            StringBuilder classificationErrorMessage = new StringBuilder();
            classificationErrorMessage = classificationErrorMessage.Append("Param name : ").Append("Requestclass")
                    .Append(" - Value : ").Append(jsonParams);

            Utility.ErrorLOG("NewAlgoAutoClassification-1", "NA_AC_Methodstart", projectId, UserID);

            ResponseClassNewAlgo responseObject = new ResponseClassNewAlgo();
            try
            {
                var clientApiURL = "";
                if (supportTypeId == 1)
                {
                    clientApiURL = new AppSettings().AppsSttingsKeyValues["NewAlgoApiURLApp"];
                }
                else
                {
                    clientApiURL = new AppSettings().AppsSttingsKeyValues["NewAlgoApiURLInfra"];
                }

                HttpClientHandler authtHandlerClient = new HttpClientHandler()
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
               
                {
                    client.BaseAddress = new Uri(clientApiURL);

                    Utility.ErrorLOG("NewAlgoAutoClassification-2", "NA_AC_Going to start API", projectId, UserID);
                    Utility.ErrorLOG("NewAlgoAutoClassification-3", "NA_API Request Information", projectId, UserID);
                    Utility.ErrorLOG("NewAlgoAutoclassification-4", classificationErrorMessage.ToString(), projectId, UserID);

                    var content = new StringContent(jsonParams);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(clientApiURL).Value, content);
                    postTask.Wait();
                    responseObject = postTask.Result.Content.ReadAsJsonAsync<ResponseClassNewAlgo>().Result;
                }

                Utility.ErrorLOG("NewAlgoAutoClassification-5", "NA_AC_API successfully called", projectId, UserID);
                Utility.ErrorLOG("NewAlgoAutoClassification-6", JsonConvert.SerializeObject(responseObject), projectId, UserID);

                SqlParameter[] prm = new SqlParameter[5];
                prm[0] = new SqlParameter("@ProjectID", projectId);
                prm[1] = new SqlParameter("@UserID", UserID);
                prm[2] = new SqlParameter("@request", jsonParams);
                prm[3] = new SqlParameter("@responce", JsonConvert.SerializeObject(responseObject));
                prm[4] = new SqlParameter("@Mode", "AddTicket");

                (new DBHelper()).ExecuteNonQuery("[dbo].[CaptureAPIRequestResponce]", prm, ConnectionString);

                if (responseObject.AvoidableFlagID == "" && responseObject.ClusterID_Desc == "" && responseObject.ClusterID_Resolution == ""
                    && responseObject.DebtClassificationID == "")
                {
                    responseObject = responseObject.ResidualDebtID == "" ? null : responseObject;
                }
                else if (responseObject.ClusterID_Desc != "0" || responseObject.ClusterID_Resolution != "0")
                {
                    TicketingModuleRepository objAutoClassificationMode = new TicketingModuleRepository();
                    SqlParameter[] prms = new SqlParameter[9];
                    prms[0] = new SqlParameter("@ProjectID", projectId);
                    prms[1] = new SqlParameter("@TimeTickerID", timeTickerId);
                    prms[2] = new SqlParameter("@UserID", UserID);
                    prms[3] = new SqlParameter("@debt", responseObject.DebtClassificationID);
                    prms[4] = new SqlParameter("@avoidable", responseObject.AvoidableFlagID);
                    prms[5] = new SqlParameter("@residual", responseObject.ResidualDebtID);
                    prms[6] = new SqlParameter("@SupportTypeID", supportTypeId);
                    prms[7] = new SqlParameter("@causecode", null);
                    prms[8] = new SqlParameter("@resolutioncode", null);
                    (new DBHelper()).ExecuteNonQuery("[dbo].[InsertDebtClassificationModeDetails_ML]", prms, ConnectionString);
                }
                else
                {
                    //CCAP Fix
                }
                if (timeTickerId != "" && responseObject != null &&
                    (responseObject.ClusterID_Resolution != "" || responseObject.ClusterID_Desc != ""))
                {
                    SqlParameter[] prms = new SqlParameter[9];
                    prms[0] = new SqlParameter("@TimeTickerID", Convert.ToInt64(timeTickerId));
                    prms[1] = new SqlParameter("@Ruleid", null);
                    prms[2] = new SqlParameter("@UserId", UserID);
                    prms[3] = new SqlParameter("@SupportTypeID", (supportTypeId > 0) ? supportTypeId : 4);
                    prms[4] = new SqlParameter("@lwruleid", null);
                    prms[5] = new SqlParameter("@lwrulelevel", null);
                    prms[6] = new SqlParameter("@ProjectID", projectId);
                    prms[7] = new SqlParameter("@clusterDesc", string.IsNullOrEmpty(responseObject.ClusterID_Desc) ? null : responseObject.ClusterID_Desc);
                    prms[8] = new SqlParameter("@clusterReso", string.IsNullOrEmpty(responseObject.ClusterID_Resolution) ? null : responseObject.ClusterID_Resolution);
                    (new DBHelper()).ExecuteNonQuery("[AVL].[InsertRuleIDforticketdetail]", prms, ConnectionString);
                }

            }
            catch (Exception ex)
            {
                Utility.ErrorLOG("NewAlgoAutoClassification-Error", "Error Occured", projectId, UserID);
                Utility.ErrorLOG("NewAlgoAutoClassification-Error", ex.Message, projectId, UserID);
                Utility.ErrorLOG("NewAlgoAutoClassification-Error", ex.InnerException.Message, projectId, UserID);
                throw ex;
            }
            return responseObject;
        }

        /// <summary>
        /// This Method Is Used To GetAssigneNameByProjectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="assigneName"></param>
        /// <returns></returns>
        public List<AssigneModel> GetAssigneNameByProjectID(int projectID, string assigneName)
        {
            List<AssigneModel> assigneList = new List<AssigneModel>();
            try
            {
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt32(projectID));
                prms[1] = new SqlParameter("@AssigneName", Convert.ToString(assigneName));

                dt = (new DBHelper()).GetTableFromSP("AVL.GetAssigneNamesByProjectID", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    assigneList = dt.AsEnumerable().Select(x => new AssigneModel
                    {
                        EmployeeId = x["EmployeeID"] != DBNull.Value ? Convert.
                        ToString(x["EmployeeID"]) : "",
                        ProjectId = x["ProjectID"] != DBNull.Value ? Convert.ToInt32(x["ProjectID"]) : 0,
                        UserId = x["UserID"] != DBNull.Value ? Convert.ToInt64(x["UserID"]) : 0,
                        AssigneName = x["EmployeeName"] != DBNull.Value ? Convert.
                        ToString(x["EmployeeName"]) : "",
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return assigneList;
        }

        /// <summary>
        /// Method to get Language for drop down.
        /// <param name="ModuleName"></param>
        /// </summary>
        /// <returns>Language list</returns>
        public List<LanguageModel> GetLanguageForDropdown(string ModuleName)
        {
            List<LanguageModel> lstLanguage = new List<LanguageModel>();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ModuleName", ModuleName);
            DataTable dt = (new DBHelper()).GetTableFromSP("AVL.GetLanguage", prms, ConnectionString);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LanguageModel objLanguageModel = new LanguageModel();
                        objLanguageModel.Language = dt.Rows[i]["LanguageName"].ToString();
                        objLanguageModel.LanguageNameInEnglish = dt.Rows[i]["LanguageNameInEnglish"].ToString();
                        objLanguageModel.LanguageCode = dt.Rows[i]["LanguageCode"].ToString();
                        lstLanguage.Add(objLanguageModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstLanguage;
        }
        /// <summary>
        /// This Method is used to GetUserApplicaitionDetails
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public UserMasterDetails GetUserApplicaitionDetails(string EmployeeID, int CustomerID)
        {
            UserMasterDetails lstUserMasterDetails = new UserMasterDetails();
            List<UserProjectMaster> lstUserProjectMaster = new List<UserProjectMaster>();
            List<UserCustomerDetails> lstCustomerDetails = new List<UserCustomerDetails>();
            List<LoginMasterDetails> lstLoginMasterDetails = new List<LoginMasterDetails>();
            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[1] = new SqlParameter("@CustomerID", Convert.ToInt32(CustomerID));

                dt = (new DBHelper()).GetNoiseTableFromSP("GetEmployeeInfo", prms, ConnectionString);
                if (dt != null && dt.Tables.Count > 0)
                {
                    lstUserProjectMaster = dt.Tables[0].AsEnumerable().Select(x => new UserProjectMaster
                    {
                        ProjectManagerId = x["ProjectManagerID"] != DBNull.Value ? Convert.ToString(x["ProjectManagerID"]) : "",
                        ProjectName = x["ProjectName"] != DBNull.Value ? Convert.ToString(x["ProjectName"]) : "",
                        TSApproverId = x["TSApproverID"] != DBNull.Value ? Convert.ToString(x["TSApproverID"]) : "",
                        AdminId = x["EmployeeIDs"] != DBNull.Value ? Convert.ToString(x["EmployeeIDs"]) : ""
                    }).ToList();
                }
                lstUserMasterDetails.ListUserProjectMaster = lstUserProjectMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstUserMasterDetails;
        }
        /// <summary>
        /// GetServiceDetails
        /// </summary>
        /// <param name="objTicketDetails"></param>
        /// <returns></returns>
        public int GetServiceDetails(AddTicketDetails objTicketDetails)
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            int ServiceID = 0;
            string ServiceName = "";
            try
            {
                ServiceTicketDetails tDetail = new ServiceTicketDetails();
                tDetail.TimeTickerId = 0;
                tDetail.TicketId = objTicketDetails.TicketId;
                tDetail.TicketDescription = objTicketDetails.TicketDescription;
                tDetail.CauseCode = string.Empty;
                tDetail.ResolutionCode = string.Empty;
                List<ServiceTicketDetails> lstTicketDetails = new List<ServiceTicketDetails>();
                lstTicketDetails.Add(tDetail);

                ServiceClassificationRepository SCR = new ServiceClassificationRepository();
                List<UpdateServiceName> updServiceName = SCR.ServiceAutoClassificationAPI(lstTicketDetails);
                if (updServiceName != null && updServiceName.Count > 0)
                {
                    ServiceName = updServiceName.FirstOrDefault().ServiceName.ToString(CultureInfo.CurrentCulture);
                    SqlParameter[] prms = new SqlParameter[4];
                    prms[0] = new SqlParameter("@ProjectID", objTicketDetails.ProjectId);
                    prms[1] = new SqlParameter("@ServiceName", ServiceName);
                    prms[2] = new SqlParameter("@TicketID", objTicketDetails.TicketId);
                    prms[3] = new SqlParameter("@TicketTypeMapID", objTicketDetails.TicketTypeMapId);
                    try
                    {
                        ds = (new DBHelper()).GetDatasetFromSP("[AVL].[UpdateSingleTicketServiceName]", prms, ConnectionString);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {

                            ServiceID = Convert.ToInt16(ds.Tables[0].Rows[0]["ServiceID"].ToString());

                        }
                    }
                    catch (Exception ex)
                    {
                        return 0;
                        throw new CustomException(ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceID;
        }



        /// <summary> 
        /// Method to Get Default Landing Page Details.
        /// </summary>
        /// <returns>Default Page list</returns>
        public List<DefaultLandingPage> GetDefaultLandingPageDetails(string EmployeeID, string CustomerID)
        {
            List<DefaultLandingPage> lstDefaultPages = new List<DefaultLandingPage>();
            SqlParameter[] prms = new SqlParameter[2];
            try
            {
                prms[0] = new SqlParameter("@CustomerID", Convert.ToInt64(CustomerID));
                prms[1] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));
                DataTable dtDefaultPages = (new DBHelper()).GetTableFromSP("AVL.GetDefaultLandingPageDetails", prms, ConnectionString);

                if (dtDefaultPages != null && dtDefaultPages.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDefaultPages.Rows.Count; i++)
                    {
                        DefaultLandingPage objDisplayModel = new DefaultLandingPage();
                        objDisplayModel.PrivilegeId = Convert.ToInt64(dtDefaultPages.Rows[i]["PrivilegeID"]);
                        objDisplayModel.DisplayName = dtDefaultPages.Rows[i]["DisplayName"].ToString();
                        lstDefaultPages.Add(objDisplayModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDefaultPages;
        }
        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public List<ProjectDetails> GetProjectDetailsforDefaultLanding(string CustomerID, string EmployeeID)
        {
            List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
            DataTable dtProjectDetails = new DataTable();
            dtProjectDetails.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[2];
            try
            {
                prms[0] = new SqlParameter("@CustomerID", Convert.ToInt64(CustomerID));
                prms[1] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));

                dtProjectDetails = (new DBHelper()).GetTableFromSP("[AVL].[GetProjectDetailsforDefaultLanding]", prms, ConnectionString);

                if (dtProjectDetails != null && dtProjectDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtProjectDetails.Rows.Count; i++)
                    {
                        ProjectDetails objProjectDetails = new ProjectDetails();
                        objProjectDetails.ProjectId = Convert.ToInt64(dtProjectDetails.Rows[i]["ProjectID"]);
                        objProjectDetails.ProjectName = dtProjectDetails.Rows[i]["ProjectName"].ToString();
                        lstProjectDetails.Add(objProjectDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstProjectDetails;
        }

        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public List<LeadDetails> GetProjectLeadDetails(string ProjectID, string EmployeeID)
        {
            List<LeadDetails> lstLeadDetails = new List<LeadDetails>();
            DataTable dtLeadDetails = new DataTable();
            dtLeadDetails.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", Convert.ToInt64(ProjectID));
            prms[1] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));

            dtLeadDetails = (new DBHelper()).GetTableFromSP("[AVL].[GetEmployeeLeadDetails]", prms, ConnectionString);

            try
            {
                if (dtLeadDetails != null && dtLeadDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLeadDetails.Rows.Count; i++)
                    {
                        LeadDetails objLeadDetails = new LeadDetails();
                        objLeadDetails.ProjectTSApprover = dtLeadDetails.Rows[i]["ProjectTSApprover"].ToString();
                        objLeadDetails.ProjectManager = dtLeadDetails.Rows[i]["ProjectManager"].ToString();
                        objLeadDetails.ProjectAdmin = dtLeadDetails.Rows[i]["ProjectAdmin"].ToString();
                        lstLeadDetails.Add(objLeadDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstLeadDetails;
        }
        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public bool SaveDefaultLandingPageDetails(string EmployeeID, string AccountID, string PrivilegeID)
        {
            bool result;
            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));
                prms[1] = new SqlParameter("@AccountID", Convert.ToInt64(AccountID));
                prms[2] = new SqlParameter("@PrivilegeID", Convert.ToInt64(PrivilegeID));
                DataSet dsSavedDetails = (new DBHelper()).GetDatasetFromSP("[AVL].[SaveDefaultLandingPageDetails]", prms, ConnectionString);
                DataTable dt = dsSavedDetails.Tables[0];
                result = Convert.ToBoolean(dt.Rows[0]["Result"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary> 
        /// Method to Get Project Details for Default Landing.
        /// </summary>
        /// <returns>Project list</returns>
        public Int64 GetCustomerwiseDefaultPage(string EmployeeID, string CustomerID)
        {
            Int64 Defaulter = 2;
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@EmployeeID", Convert.ToString(EmployeeID));
                prms[1] = new SqlParameter("@CustomerID", Convert.ToInt64(CustomerID));
                DataSet dtDefaultpages = (new DBHelper()).GetDatasetFromSP("[AVL].[GetCustomerwiseDefaultPage]", prms, ConnectionString);
                DataTable dt = dtDefaultpages.Tables[0];
                Defaulter = Convert.ToInt64(dt.Rows[0]["PrivilegeID"]);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Defaulter;
        }
        public Nullable<bool> GetAppEnableFlag(Int64 customerID)
        {
            Nullable<bool> isAppEnable = false;

            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;

            try
            {
                SqlParameter[] prms = new SqlParameter[1];

                prms[0] = new SqlParameter("@CustomerID", customerID);

                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[dbo].[GetAppEditableByCustomer]",
                    prms, ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    isAppEnable = Convert.ToBoolean(dtResult.Tables[0].Rows[0][0]);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAppEnable;
        }
        /// <summary>
        /// This method updates status and service for Work items
        /// </summary>
        /// <param name="updateWIParams"></param>
        /// <returns>result</returns>
        public bool UpdateWorkItemServiceandStatus(OverallTicketDetails updateWIParams)
        {
            bool result = false;
            DataTable dtworkitems = new DataTable();
            dtworkitems.Locale = CultureInfo.InvariantCulture;
            try
            {

                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@TimeTickerID", updateWIParams.TimeTickerId);
                prms[1] = new SqlParameter("@ProjectID", updateWIParams.ProjectId);
                prms[2] = new SqlParameter("@TicketID", updateWIParams.TicketId);
                prms[3] = new SqlParameter("@StatusID", updateWIParams.DARTStatusId);
                prms[4] = new SqlParameter("@ServiceID", Convert.ToInt32(updateWIParams.ServiceId));
                prms[5] = new SqlParameter("@EmployeeID", updateWIParams.EmployeeId);
                dtworkitems = (new DBHelper()).GetTableFromSP("[AVL].[UpdateWorkItemStatusandService]", prms, ConnectionString);
                result = Convert.ToBoolean(dtworkitems.Rows[0]["Result"]);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return result;

        }
        /// <summary>
        /// Checks whether WorkItem already exists or not
        /// </summary>
        /// <param name="workItemDetails"></param>
        /// <returns>lstValidationmsg</returns>
        public List<ValidationMessages> CheckWorkItemrepo(List<CheckDuplicate> workItemDetails)
        {
            List<ValidationMessages> lstValidationmsg = new List<ValidationMessages>();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"], "api/WorkProfiler/CheckWokitemID");
                    var dataAsString = JsonConvert.SerializeObject(workItemDetails);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                    postTask.Wait();
                    lstValidationmsg = postTask.Result.Content.ReadAsJsonAsync<List<ValidationMessages>>().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstValidationmsg;

        }

        /// <summary>
        /// Checks if sprint name already exists
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns>lstValidationmsg</returns>
        public List<ValidationMessages> CheckSprintName(List<CheckDuplicate> sprintDetails)
        {
            List<ValidationMessages> lstValidationmsg = new List<ValidationMessages>();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"], "api/WorkProfiler/CheckSprintName");
                    var dataAsString = JsonConvert.SerializeObject(sprintDetails);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                    postTask.Wait();
                    lstValidationmsg = postTask.Result.Content.ReadAsJsonAsync<List<ValidationMessages>>().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstValidationmsg;
        }

        public bool Addworkitem(List<AddWorkItemSave> workitem)
        {
            bool Issucess = false;
            List<AddWorkItem> addworkitem = new List<AddWorkItem>();
            AddWorkItem add = new AddWorkItem();
            if (workitem != null)
            {
                add.ProjectId = workitem[0].ProjectId;
                add.WorkItemTypeId = workitem[0].WorkItemTypeId;
                add.WorkItemId = workitem[0].WorkItemId;
                add.ApplicationId = workitem[0].ApplicationId;
                add.EpicId = workitem[0].EpicId;
                add.SprintId = workitem[0].SprintId;
                add.UserStoryId = workitem[0].UserStoryId;
                add.Assignee = workitem[0].Assignee;
                add.SprintStartDate = Convert.ToDateTime(workitem[0].SprintStartDate);
                add.SprintEndDate = Convert.ToDateTime(workitem[0].SprintEndDate);
                add.PlannedStartDate = Convert.ToDateTime(workitem[0].PlannedStartDate);
                add.PlannedEndDate = Convert.ToDateTime(workitem[0].PlannedEndDate);
                add.StatusId = workitem[0].StatusId;
                add.PriorityId = workitem[0].PriorityId;
                add.WorkItemTitle = workitem[0].WorkItemTitle;
                add.Description = workitem[0].Description;
                add.PlannedEstimate = workitem[0].PlannedEstimate;
                add.EstimationPoints = workitem[0].EstimationPoints;
                add.IsMileStoneMet = workitem[0].IsMileStoneMet;
                add.BugPhaseTypeMapId = workitem[0].BugPhaseTypeMapId;
                add.ReMapSprintDetailsId = workitem[0].ReMapSprintDetailsId;
                addworkitem.Add(add);
            }
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"], "api/WorkProfiler/AddWorkItem");
                    var dataAsString = JsonConvert.SerializeObject(addworkitem);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                    postTask.Wait();
                    var result = postTask.Result;
                    Issucess = result.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Issucess;
        }
        /// <summary>
        /// Saves Sprint Details
        /// </summary>
        /// <param name="sprintDetails"></param>
        /// <returns>result</returns>
        public bool SaveSprintDetails(List<SavesprintDetails> sprintDetails)
        {
            bool Issuccess = false;
            try
            {
                List<SprintDetails> sprint = new List<SprintDetails>();
                SprintDetails sprintdata = new SprintDetails();
                if (sprintDetails != null)
                {
                    sprintdata.ProjectId = sprintDetails[0].ProjectId;
                    sprintdata.SprintName = sprintDetails[0].SprintName;
                    sprintdata.SprintDescription = sprintDetails[0].SprintDescription;
                    sprintdata.StartDate = Convert.ToDateTime(sprintDetails[0].StartDate);
                    sprintdata.EndDate = Convert.ToDateTime(sprintDetails[0].EndDate);
                    sprintdata.UserId = sprintDetails[0].UserId;
                    sprintdata.StatusMapId = sprintDetails[0].StatusMapId;
                    sprintdata.PODDetailId = sprintDetails[0].PODDetailId;
                    sprint.Add(sprintdata);
                }
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"], "api/WorkProfiler/SaveSprintData");
                    var dataAsString = JsonConvert.SerializeObject(sprint);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                    postTask.Wait();
                    var result = postTask.Result;
                    Issuccess = result.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Issuccess;
        }
        /// <summary>
        /// Gets ADM Project Details
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns>lstProject</returns>
        public List<ADMProjectsModel> GetADMProjectDetails(List<ADMProjectInput> aDMProjectInputs)
        {
            List<ADMProjectsModel> lstAdmProjects = new List<ADMProjectsModel>();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"], "api/WorkProfiler/GetProjectDetails");
                    var dataAsString = JsonConvert.SerializeObject(aDMProjectInputs);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var postTask = client.PostAsync(new SanitizeString(url).Value, content);
                    postTask.Wait();
                    lstAdmProjects = postTask.Result.Content.ReadAsJsonAsync<List<ADMProjectsModel>>().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstAdmProjects;
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
            MasterDataModel objMasterDataModel = new MasterDataModel();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {

                    List<MandateAttributes> objMandatoryAttributes = new List<MandateAttributes>();
                    client.BaseAddress = new Uri(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"]);
                    bool KeycloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.InvariantCulture);
                    KeyCloakTokenHelper.SetTokenOnHeader(client, access, KeycloakEnabled);
                    HttpResponseMessage response = client.GetAsync(string.Format("api/MasterData/GetMasterData/{0}/{1}", ProjectId, false)).Result;
                    objMasterDataModel = response.Content.ReadAsJsonAsync<MasterDataModel>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListWorkItemTypeData != null && objMasterDataModel.ListWorkItemTypeData.Count > 0)
                    {
                        objMasterDataModel.ListWorkItemTypeData = objMasterDataModel.ListWorkItemTypeData.Where(x => x.IsEffortTracking == true).ToList();
                    }
                    HttpResponseMessage response1 = client.GetAsync($"api/WorkProfiler/GetMandateAttributes").Result;
                    objMandatoryAttributes = response1.Content.ReadAsJsonAsync<List<MandateAttributes>>().Result;
                    objMasterDataModel.ListMandateAttributes = objMandatoryAttributes;
                    HttpResponseMessage responseEpic = client.GetAsync(string.Format("api/ProductBacklog/GetEpicDetails/{0}/{1}", ProjectId, false)).Result;
                    objMasterDataModel.ListEpic = responseEpic.Content.ReadAsJsonAsync<List<Epic>>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListEpic != null && objMasterDataModel.ListEpic.Count > 0)
                    {
                        objMasterDataModel.ListEpic = objMasterDataModel.ListEpic.Where(x => x.StatusId != ApplicationConstants.Cancelled && x.StatusId != ApplicationConstants.Completed).ToList();
                        objMasterDataModel.ListEpic = objMasterDataModel.ListEpic.OrderBy(x => x.WorkItemTitle).ToList();
                    }
                    HttpResponseMessage responseUserStory = client.GetAsync(string.Format("api/ProductBacklog/GetUserStoryLists/{0}/{1}", ProjectId, false)).Result;
                    objMasterDataModel.ListUserStory = responseUserStory.Content.ReadAsJsonAsync<List<Userstory>>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListUserStory != null && objMasterDataModel.ListUserStory.Count > 0)
                    {
                        objMasterDataModel.ListUserStory = objMasterDataModel.ListUserStory.Where(
                            x => x.StatusId != ApplicationConstants.Cancelled && x.StatusId != ApplicationConstants.Completed).ToList();
                        objMasterDataModel.ListUserStory = objMasterDataModel.ListUserStory.OrderBy(x => x.WorkItemTitle).ToList();
                    }
                    HttpResponseMessage responseSprint = client.GetAsync(string.Format("api/SprintBacklog/GetSprintName/{0}/{1}", ProjectId, false)).Result;
                    objMasterDataModel.ListSprint = responseSprint.Content.ReadAsJsonAsync<List<SprintModel>>().Result;
                    HttpResponseMessage responsePOD = client.GetAsync(string.Format("api/SprintBacklog/GetPodDetails/{0}", ProjectId)).Result;
                    objMasterDataModel.ListPOD = responsePOD.Content.ReadAsJsonAsync<List<PODDetailsModel>>().Result;

                    if (objMasterDataModel != null && objMasterDataModel.ListMandateAttributes != null && objMasterDataModel.ListMandateAttributes.Count() > 0)
                    {
                        objMasterDataModel.ListMandateAttributes = objMasterDataModel.ListMandateAttributes.OrderBy(x => x.AttributeName).ToList();
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListPriorityData != null && objMasterDataModel.ListPriorityData.Count() > 0)
                    {
                        objMasterDataModel.ListPriorityData = objMasterDataModel.ListPriorityData.OrderBy(x => x.PriorityName).ToList();
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListStatusData != null && objMasterDataModel.ListStatusData.Count() > 0)
                    {
                        objMasterDataModel.ListStatusData = objMasterDataModel.ListStatusData.OrderBy(x => x.StatusName).ToList();
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListWorkItemTypeData != null && objMasterDataModel.ListWorkItemTypeData.Count() > 0)
                    {
                        objMasterDataModel.ListWorkItemTypeData = objMasterDataModel.ListWorkItemTypeData.OrderBy(x => x.WorkTypeName).ToList();
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListSprint != null && objMasterDataModel.ListSprint.Any())
                    {
                        objMasterDataModel.ListSprint.ToList<SprintModel>().ForEach(x =>
                        {
                            if (x.SprintStartDate != null && x.SprintEndDate != null)
                            {
                                x.SprintStartDate = x.SprintStartDate;
                                x.SprintEndDate = x.SprintEndDate;
                            }
                        });
                        if (StartDate != null && EndDate != null)
                        {
                            objMasterDataModel.ListSprint = objMasterDataModel.ListSprint.Where(x => x.SprintStartDate != null && x.SprintEndDate != null &&
                                    (x.SprintStartDate >= StartDate && x.SprintStartDate <= EndDate
                                    || x.SprintEndDate >= StartDate && x.SprintEndDate <= EndDate
                                    || (x.SprintStartDate <= StartDate && x.SprintStartDate <= EndDate
                                    && x.SprintEndDate >= EndDate)
                                   )).ToList<SprintModel>();
                        }
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListSprint != null && objMasterDataModel.ListSprint.Count() > 0)
                    {
                        objMasterDataModel.ListSprint = objMasterDataModel.ListSprint.OrderBy(x => x.SprintName).ToList();
                    }
                    HttpResponseMessage responseApplication = client.GetAsync(string.Format("api/WorkProfiler/GetADMApplicationDetails/{0}", ProjectId)).Result;
                    objMasterDataModel.ListApplicationData = responseApplication.Content.ReadAsJsonAsync<List<ApplicationDataModel>>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListEpic != null && objMasterDataModel.ListApplicationData.Count() > 0)
                    {
                        objMasterDataModel.ListApplicationData = objMasterDataModel.ListApplicationData.OrderBy(x => x.ApplicationName).ToList();
                    }
                    HttpResponseMessage responsebugPhase = client.GetAsync(string.Format("api/SprintBacklog/GetBugPhaseAndType")).Result;
                    objMasterDataModel.ListBugPhase = responsebugPhase.Content.ReadAsJsonAsync<List<BugPhaseModel>>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListBugPhase != null && objMasterDataModel.ListBugPhase.Count > 0)
                    {
                        objMasterDataModel.ListBugPhase = objMasterDataModel.ListBugPhase.OrderBy(x => x.BugPhaseID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objMasterDataModel;
        }

        /// <summary>
        /// Gets DropDown Values For Sprint
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns>MasterDataModel</returns>
        public MasterDataModel GetDropDownValuesSprint(Int64 ProjectId, DateTime StartDate, DateTime EndDate, string access)
        {
            MasterDataModel objMasterDataModel = new MasterDataModel();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    client.BaseAddress = new Uri(new AppSettings().AppsSttingsKeyValues["ADMWebApiURL"]);
                    bool KeycloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.InvariantCulture);
                    KeyCloakTokenHelper.SetTokenOnHeader(client, access, KeycloakEnabled);
                    HttpResponseMessage responseSprint = client.GetAsync(string.Format("api/SprintBacklog/GetSprintName/{0}/{1}", ProjectId, false)).Result;
                    objMasterDataModel.ListSprint = responseSprint.Content.ReadAsJsonAsync<List<SprintModel>>().Result;
                    if (objMasterDataModel != null && objMasterDataModel.ListSprint != null && objMasterDataModel.ListSprint.Any())
                    {
                        objMasterDataModel.ListSprint.ToList<SprintModel>().ForEach(x =>
                        {
                            if (x.SprintStartDate != null && x.SprintEndDate != null)
                            {
                                x.SprintStartDate = x.SprintStartDate;
                                x.SprintEndDate = x.SprintEndDate;
                            }
                        });
                        if (StartDate != null && EndDate != null)
                        {

                            objMasterDataModel.ListSprint = objMasterDataModel.ListSprint.Where(x => x.SprintStartDate != null && x.SprintEndDate != null &&
                                    (x.SprintStartDate >= StartDate && x.SprintStartDate <= EndDate
                                    || x.SprintEndDate >= StartDate && x.SprintEndDate <= EndDate
                                    || (x.SprintStartDate <= StartDate && x.SprintStartDate <= EndDate
                                    && x.SprintEndDate >= EndDate)
                                   )).ToList<SprintModel>();
                        }
                    }
                    if (objMasterDataModel != null && objMasterDataModel.ListSprint != null && objMasterDataModel.ListSprint.Count() > 0)
                    {
                        objMasterDataModel.ListSprint = objMasterDataModel.ListSprint.OrderBy(x => x.SprintName).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objMasterDataModel;
        }

        /// <summary>
        /// Method to capture log
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="itemName">Request Name</param>
        /// <param name="logContent">Content to log</param>
        private void CaptureLogDetails(string userId, string itemName, string logContent)
        {
            StringBuilder classificationErrorMessage = new StringBuilder();
            classificationErrorMessage = classificationErrorMessage.Append("Param name : ").Append(itemName)
                    .Append(" - Value : ").Append(logContent);
            Utility.ErrorLOG("Autoclassification-4", classificationErrorMessage.ToString(), 0, userId);
            classificationErrorMessage.Clear();
        }

        public NewAlgoColumnList GetAlgoKeyAndColumn(int projectId, int supportTypeId)
        {
            NewAlgoColumnList result = new NewAlgoColumnList();

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@ProjectID", projectId);
            prm[1] = new SqlParameter("@SupportTypeId", supportTypeId);

            DataSet dtalgoKey = (new DBHelper()).GetDatasetFromSP("[ML].[GetAlgoKey]", prm, ConnectionString);
            if (dtalgoKey.Tables.Count > 0)
            {
                if (dtalgoKey.Tables[0].Rows.Count > 0)
                {
                    result.AlgoKey = dtalgoKey.Tables[0].Rows[0]["AlgorithmKey"].ToString();
                    result.TransactionId = Convert.ToInt32(dtalgoKey.Tables[0].Rows[0]["TransactionId"]);
                }
                else
                {
                    result.AlgoKey = "AL001";
                }
                if (dtalgoKey.Tables[1].Rows.Count > 0)
                {
                    DataTable dtColumnList = dtalgoKey.Tables[1];
                    result.ColumnList = dtColumnList.AsEnumerable().Where(x => x["TK_TicketDetailColumn"] != DBNull.Value)
                        .Select(x => x["TK_TicketDetailColumn"]).Cast<string>().ToList();
                }

            }

            return result;
        }
    }
}
