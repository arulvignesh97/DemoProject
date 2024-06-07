using CTS.Applens.Framework;

// <copyright file="SearchTicketRepository.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.DAL
{
    using Common;
    using CTS.Applens.WorkProfiler.Entities;
    using CTS.Applens.WorkProfiler.Entities.Base;
    using Models.SearchTicket;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Linq;
    using System.Globalization;

    /// <summary>
    /// The Class SearchTicketRepository holds methods to interact with database for 
    /// Search Tickets related functionalities.
    /// </summary>
    public class SearchTicketRepository : DBContext
    {
        /// <summary>
        /// Method to get all active Ticket Types mapped to project.
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns>List of active Ticket Types</returns>
        public List<TicketTypes> GetTicketTypes(SearchTicketTypeParameter objSearchTicketType)
        {
            List<TicketTypes> lstTicketTypes = new List<TicketTypes>();
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@ProjectID", objSearchTicketType.ProjectId);
            sqlParams[1] = new SqlParameter("@IsCognizant", objSearchTicketType.IsCognizant);
            DataTable dtTicketTypes = new DBHelper()
                .GetTableFromSP("AVL.USP_GetTicketTypeByProjectID", sqlParams, ConnectionString);

            if (dtTicketTypes != null && dtTicketTypes.Rows.Count > 0)
            {
                lstTicketTypes = dtTicketTypes.AsEnumerable().Select(row =>
                         new TicketTypes
                         {
                             TicketTypeId = row["TicketTypeID"] == DBNull.Value ?
                                (Int32?)null : row.Field<Int32>("TicketTypeID"),
                             TicketTypeName = row.Field<string>("TicketTypeName"),
                             SupportTypeId = row.Field<Int32>("SupportTypeID")
                         }).ToList();

            }

            return lstTicketTypes;
        }


        /// <summary>
        /// Method to get all active Ticket Statuses mapped to project.
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns>List of all active Ticket Statuses</returns>
        public List<TicketStatus> GetTicketStatus(ProjectIDs searchTicketParameters)
        {
            List<TicketStatus> lstTicketStatus = new List<TicketStatus>();
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@ProjectID", searchTicketParameters.ProjectId);
            DataTable dtTicketStatus = new DBHelper()
                .GetTableFromSP("AVL.USP_GetTicketStatusByProjectID", sqlParams, ConnectionString);

            if (dtTicketStatus != null && dtTicketStatus.Rows.Count > 0)
            {
                lstTicketStatus = dtTicketStatus.AsEnumerable().Select(row =>
                         new TicketStatus
                         {
                             StatusId = row.Field<int>("StatusID"),
                             StatusName = row.Field<string>("StatusName")
                         }).ToList();
            }

            return lstTicketStatus;
        }

        public string DownloadSearchTicket(SearchTicketParameters searchTicketParameters, byte[] aesKey, string encryptionEnabled, string TableName, string SheetName)
        {
            DataTable dt = CreateSearchTicketExcel(searchTicketParameters, aesKey, encryptionEnabled);
            DataSet _DSProjectMapping = new DataSet();
            _DSProjectMapping.Locale = CultureInfo.InvariantCulture;
            _DSProjectMapping.Tables.Add(dt);
            _DSProjectMapping.Tables[0].TableName = TableName + "-" + searchTicketParameters.CustomerName;
            String excelFileName = SheetName;

            string filename = new DownloadExcel().ToExcelSheetByDataSet(_DSProjectMapping, excelFileName);
            return filename;
        }

        public DataTable CreateSearchTicketExcel(SearchTicketParameters searchTicketParameters, byte[] aesKey, string encryptionEnabled)
        {
            List<SearchTicketDetails> lstTicketDetails = GetSearchTickets(searchTicketParameters, aesKey, encryptionEnabled);
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Clear();
            
            dt.Columns.Add("Application Name");
            dt.Columns.Add("Tower Name");
            dt.Columns.Add("Project Name");
            dt.Columns.Add("Hierarchy 1");
            dt.Columns.Add("Hierarchy 2");
            dt.Columns.Add("Hierarchy 3");
            dt.Columns.Add("Hierarchy 4");
            dt.Columns.Add("Hierarchy 5");
            dt.Columns.Add("Hierarchy 6");
            dt.Columns.Add("Ticket ID");
            dt.Columns.Add("Ticket Description");
            dt.Columns.Add("Service");
            dt.Columns.Add("Service Group");
            dt.Columns.Add("Ticket Type");
            dt.Columns.Add("Priority");
            dt.Columns.Add("Status");
            dt.Columns.Add("AppLens Status");
            dt.Columns.Add("Cause Code");
            dt.Columns.Add("Resolution Code");
            dt.Columns.Add("Debt Classification");
            dt.Columns.Add("Avoidable Flag");
            dt.Columns.Add("Residual Debt");
            dt.Columns.Add("Effort Till Date");
            dt.Columns.Add("Insertion Mode");
            dt.Columns.Add("Open Date");
            dt.Columns.Add("Closed Date");
            dt.Columns.Add("Assignee");
            dt.Columns.Add("External Login ID");
            dt.Columns.Add("Reopen Date");
            dt.Columns.Add("Ticket Source");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Data Entry Complete");
            dt.Columns.Add("Release Type");
            dt.Columns.Add("Planned Effort");
            dt.Columns.Add("Estimated WorkSize");
            dt.Columns.Add("Actual WorkSize");
            dt.Columns.Add("Planned Start Date");
            dt.Columns.Add("Planned End Date");
            dt.Columns.Add("Rejected Time Stamp");
            dt.Columns.Add("KEDB Available Indicator");
            dt.Columns.Add("KEDB Updated");
            dt.Columns.Add("Elevate Flag Internal");
            dt.Columns.Add("RCA ID");
            dt.Columns.Add("MetResponse SLA");
            dt.Columns.Add("Met Acknowledgement SLA");
            dt.Columns.Add("Met Resolution");
            dt.Columns.Add("Actual Start Date Time");
            dt.Columns.Add("Actual End Date Time");
            dt.Columns.Add("Actual Duration");
            dt.Columns.Add("Nature Of The Ticket");
            dt.Columns.Add("Comments");
            dt.Columns.Add("Repeated Incident");
            dt.Columns.Add("Related Tickets");
            dt.Columns.Add("KEDBPath");
            dt.Columns.Add("Ticket CreatedBy");
            dt.Columns.Add("Escalated Flag Customer");
            dt.Columns.Add("Approved By");
            dt.Columns.Add("Started DateTime");
            dt.Columns.Add("WIP DateTime");
            dt.Columns.Add("OnHold DateTime");
            dt.Columns.Add("Completed DateTime");
            dt.Columns.Add("Cancelled DateTime");
            dt.Columns.Add("Outage Duration");
            dt.Columns.Add("Resolution Remarks");
            dt.Columns.Add("Flex Field1");
            dt.Columns.Add("Flex Field2");
            dt.Columns.Add("Flex Field3");
            dt.Columns.Add("Flex Field4");
            dt.Columns.Add("Category");
            dt.Columns.Add("Type");

            foreach (var obj in lstTicketDetails)
            {
                DataRow NewRow = dt.NewRow();

                NewRow["Application Name"] = obj.ApplicationName;
                NewRow["Tower Name"] = obj.TowerName;
                NewRow["Project Name"] = obj.ProjectName;
                NewRow["Hierarchy 1"] = obj.Hierarchy1;
                NewRow["Hierarchy 2"] = obj.Hierarchy2;
                NewRow["Hierarchy 3"] = obj.Hierarchy3;
                NewRow["Hierarchy 4"] = obj.Hierarchy4;
                NewRow["Hierarchy 5"] = obj.Hierarchy5;
                NewRow["Hierarchy 6"] = obj.Hierarchy6;
                NewRow["Ticket ID"] = obj.TicketId;
                NewRow["Ticket Description"] = obj.TicketDescription;
                NewRow["Service"] = obj.Service;
                NewRow["Service Group"] = obj.ServiceGroup;
                NewRow["Ticket Type"] = obj.TicketType;
                NewRow["Priority"] = obj.Priority;
                NewRow["Status"] = obj.Status;
                NewRow["AppLens Status"] = obj.AppLensStatus;
                NewRow["Cause Code"] = obj.CauseCode;
                NewRow["Resolution Code"] = obj.ResolutionCode;
                NewRow["Debt Classification"] = obj.DebtClassification;
                NewRow["Avoidable Flag"] = obj.AvoidableFlag;
                NewRow["Residual Debt"] = obj.ResidualDebt;
                NewRow["Effort Till Date"] = obj.EffortTillDate;
                NewRow["Insertion Mode"] = obj.InsertionMode;
                NewRow["Open Date"] = obj.OpenDate;
                NewRow["Closed Date"] = obj.ClosedDate;
                NewRow["Assignee"] = obj.Assignee;
                NewRow["External Login ID"] = obj.ClientUserId;
                NewRow["Reopen Date"] = obj.ReopenDate;
                NewRow["Ticket Source"] = obj.Source;
                NewRow["Severity"] = obj.Severity;
                NewRow["Data Entry Complete"] = obj.DataEntryComplete;
                NewRow["Release Type"] = obj.ReleaseType;
                NewRow["Planned Effort"] = obj.PlannedEffort;
                NewRow["Estimated WorkSize"] = obj.EstimatedWorkSize;
                NewRow["Actual WorkSize"] = obj.ActualWorkSize;
                NewRow["Planned Start Date"] = obj.PlannedStartDate;
                NewRow["Planned End Date"] = obj.PlannedEndDate;
                NewRow["Rejected Time Stamp"] = obj.RejectedTimeStamp;
                NewRow["KEDB Available Indicator"] = obj.KEDBAvailableIndicator;
                NewRow["KEDB Updated"] = obj.KEDBUpdated;
                NewRow["Elevate Flag Internal"] = obj.ElevateFlagInternal;
                NewRow["RCA ID"] = obj.RCAID;
                NewRow["MetResponse SLA"] = obj.MetResponseSLA;
                NewRow["Met Acknowledgement SLA"] = obj.MetAcknowledgementSLA;
                NewRow["Met Resolution"] = obj.MetResolution;
                NewRow["Actual Start Date Time"] = obj.ActualStartDateTime;
                NewRow["Actual End Date Time"] = obj.ActualEndDateTime;
                NewRow["Actual Duration"] = obj.ActualDuration;
                NewRow["Nature Of The Ticket"] = obj.NatureOfTheTicket;
                NewRow["Comments"] = obj.Comments;
                NewRow["Repeated Incident"] = obj.RepeatedIncident;
                NewRow["Related Tickets"] = obj.RelatedTickets;
                NewRow["KEDBPath"] = obj.KEDBPath;
                NewRow["Ticket CreatedBy"] = obj.TicketCreatedBy;
                NewRow["Escalated Flag Customer"] = obj.EscalatedFlagCustomer;
                NewRow["Approved By"] = obj.ApprovedBy;
                NewRow["Started DateTime"] = obj.StartedDateTime;
                NewRow["WIP DateTime"] = obj.WIPDateTime;
                NewRow["OnHold DateTime"] = obj.OnHoldDateTime;
                NewRow["Completed DateTime"] = obj.CompletedDateTime;
                NewRow["Cancelled DateTime"] = obj.CancelledDateTime;
                NewRow["Outage Duration"] = obj.OutageDuration;
                NewRow["Resolution Remarks"] = obj.ResolutionRemarks;
                NewRow["Flex Field1"] = obj.FlexField1;
                NewRow["Flex Field2"] = obj.FlexField2;
                NewRow["Flex Field3"] = obj.FlexField3;
                NewRow["Flex Field4"] = obj.FlexField4;
                NewRow["Category"] = obj.Category;
                NewRow["Type"] = obj.Type;

                dt.Rows.Add(NewRow);
            }
            //Ticket Description
           

            if (!searchTicketParameters.InfraEnabled)
            {
                //application
                if (dt.Columns.Contains("Tower Name"))
                {
                    dt.Columns.Remove("Tower Name");
                }
            }
            else
            {
                //Infra
                if (dt.Columns.Contains("Application Name"))
                {
                    dt.Columns.Remove("Application Name");
                }
                if (dt.Columns.Contains("Service"))
                {
                    dt.Columns.Remove("Service");
                }
                if (dt.Columns.Contains("Service Group"))
                {
                    dt.Columns.Remove("Service Group");
                }
            }
            if (searchTicketParameters.IsCognizant == 1)
            {
                if (dt.Columns.Contains("Hierarchy 4"))
                {
                    dt.Columns.Remove("Hierarchy 4");
                }
                if (dt.Columns.Contains("Hierarchy 5"))
                {
                    dt.Columns.Remove("Hierarchy 5");
                }
                if (dt.Columns.Contains("Hierarchy 6"))
                {
                    dt.Columns.Remove("Hierarchy 6");
                }
                if (dt.Columns.Contains("Ticket Description"))
                {
                    dt.Columns.Remove("Ticket Description");
                }
            }
            else
            {
               
                if (dt.Columns.Contains("Project Name"))
                {
                    dt.Columns.Remove("Project Name");
                }
                if (dt.Columns.Contains("Service"))
                {
                    dt.Columns.Remove("Service");
                }
                if (dt.Columns.Contains("Service Group"))
                {
                    dt.Columns.Remove("Service Group");
                }
            }
            if (searchTicketParameters.HeiraricyNum == 3)
            {
                if (dt.Columns.Contains("Hierarchy 4"))
                {
                    dt.Columns.Remove("Hierarchy 4");
                }
                if (dt.Columns.Contains("Hierarchy 5"))
                {
                    dt.Columns.Remove("Hierarchy 5");
                }
                if (dt.Columns.Contains("Hierarchy 6"))
                {
                    dt.Columns.Remove("Hierarchy 6");
                }
            }
            else if (searchTicketParameters.HeiraricyNum == 4)
            {
                if (dt.Columns.Contains("Hierarchy 5"))
                {
                    dt.Columns.Remove("Hierarchy 5");
                }
                if (dt.Columns.Contains("Hierarchy 6"))
                {
                    dt.Columns.Remove("Hierarchy 6");
                }
            }
            else if (searchTicketParameters.HeiraricyNum == 5)
            {
                if (dt.Columns.Contains("Hierarchy 6"))
                {
                    dt.Columns.Remove("Hierarchy 6");
                }
            }
            else
            {
                //mandatory else
            }
            return dt;
        }

        /// <summary>
        /// Method to get Search Ticket Details based on search parameters.
        /// </summary>
        /// <param name="searchTicketParameters">Search Ticket Parameters</param>
        /// <returns>List of Search Ticket Details.</returns>
        public List<SearchTicketDetails> GetSearchTickets(
            SearchTicketParameters searchTicketParameters, byte[] aesKey, string encryptionEnabled)
        {
            List<SearchTicketDetails> lstTicketDetails = new List<SearchTicketDetails>();
            SqlParameter[] sqlParams = new SqlParameter[19];
            AESEncryption aesMod = new AESEncryption();

            sqlParams[0] = new SqlParameter("@ProjectIDs", searchTicketParameters.ProjectIds);
            sqlParams[1] = new SqlParameter("@StartDate", searchTicketParameters.StartDate);
            sqlParams[2] = new SqlParameter("@EndDate", searchTicketParameters.EndDate);
            sqlParams[3] = new SqlParameter("@IsFilterByOpenDate",
                searchTicketParameters.IsFilterByOpenDate);

            sqlParams[4] = new SqlParameter("@Hierarchy1IDs",
                searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy1Id != null ?
                searchTicketParameters.Hierarchy1Id.Replace("H1-", string.Empty) : searchTicketParameters.Hierarchy1Id);

            sqlParams[5] = new SqlParameter("@Hierarchy2IDs",
                searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy2Id != null ?
                searchTicketParameters.Hierarchy2Id.Replace("H2-", string.Empty) : searchTicketParameters.Hierarchy2Id);

            sqlParams[6] = new SqlParameter("@Hierarchy3IDs",
                searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy3Id != null ?
                searchTicketParameters.Hierarchy3Id.Replace("H3-", string.Empty) : searchTicketParameters.Hierarchy3Id);

            sqlParams[7] = new SqlParameter("@Hierarchy4IDs",
                 searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy4Id != null ?
                searchTicketParameters.Hierarchy4Id.Replace("H4-", string.Empty) : searchTicketParameters.Hierarchy4Id);

            sqlParams[8] = new SqlParameter("@Hierarchy5IDs",
                searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy5Id != null ?
                searchTicketParameters.Hierarchy5Id.Replace("H5-", string.Empty) : searchTicketParameters.Hierarchy5Id);

            sqlParams[9] = new SqlParameter("@Hierarchy6IDs",
                searchTicketParameters.InfraEnabled && searchTicketParameters.Hierarchy6Id != null ?
                searchTicketParameters.Hierarchy6Id.Replace("H6-", string.Empty) : searchTicketParameters.Hierarchy6Id);

            sqlParams[10] = new SqlParameter("@ApplicationIDs",
                searchTicketParameters.ApplicationId);
            sqlParams[11] = new SqlParameter("@TicketStatusIDs",
                searchTicketParameters.TicketStatusId);
            sqlParams[12] = new SqlParameter("@TicketSourceIDs",
                searchTicketParameters.TicketSourceId);
            sqlParams[13] = new SqlParameter("@TicketTypeIDs",
                searchTicketParameters.TicketTypeId);
            sqlParams[14] = new SqlParameter("@TicketingData",
                searchTicketParameters.TicketingData);
            sqlParams[15] = new SqlParameter("@DataEntryCompletion",
                searchTicketParameters.DataEntryCompletion);
            sqlParams[16] = new SqlParameter("@CustomerID",
                searchTicketParameters.CustomerId);
            sqlParams[17] = new SqlParameter("@IsCognizant",
                searchTicketParameters.IsCognizant);
            sqlParams[18] = new SqlParameter("@IsInfra",
                searchTicketParameters.InfraEnabled);

            DataTable dtTicketDetails = new DBHelper()
                .GetTableFromSP("[AVL].[Effort_GetSearchTickets]", sqlParams, ConnectionString);

            if (dtTicketDetails != null && dtTicketDetails.Rows.Count > 0)
            {
                lstTicketDetails = dtTicketDetails.AsEnumerable().Select(row =>
                         new SearchTicketDetails
                         {
                             ProjectName = Convert.ToString((row["ProjectName"]) == DBNull.Value ?
                                string.Empty : row["ProjectName"]),
                             TicketId = Convert.ToString((row["TicketID"]) == DBNull.Value ?
                                string.Empty : row["TicketID"]),
                             ApplicationName = Convert.ToString((row["ApplicationName"])
                                == DBNull.Value ? string.Empty : row["ApplicationName"]),
                             Hierarchy1 = Convert.ToString((row["Hierarchy1"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy1"]),
                             Hierarchy2 = Convert.ToString((row["Hierarchy2"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy2"]),
                             Hierarchy3 = Convert.ToString((row["Hierarchy3"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy3"]),
                             Hierarchy4 = Convert.ToString((row["Hierarchy4"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy4"]),
                             Hierarchy5 = Convert.ToString((row["Hierarchy5"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy5"]),
                             Hierarchy6 = Convert.ToString((row["Hierarchy6"]) == DBNull.Value ?
                                string.Empty : row["Hierarchy6"]),
                             TicketDescription = Convert.ToString(string.IsNullOrEmpty(row["TicketDescription"].
                             ToString()) ? string.Empty :
                             encryptionEnabled.ToLower() == "enabled" ? aesMod.DecryptStringBytes((string)
                             row["TicketDescription"], aesKey) : row["TicketDescription"].ToString()),
                             Service = Convert.ToString((row["Service"]) == DBNull.Value ?
                                string.Empty : row["Service"]),
                             ServiceGroup = Convert.ToString((row["ServiceGroup"])
                                == DBNull.Value ? string.Empty : row["ServiceGroup"]),
                             DataEntryComplete = Convert.ToString((row["DataEntryComplete"])
                                == DBNull.Value ? string.Empty : row["DataEntryComplete"]),
                             TicketType = Convert.ToString((row["TicketType"]) == DBNull.Value ?
                                string.Empty : row["TicketType"]),
                             CauseCode = Convert.ToString((row["CauseCode"]) == DBNull.Value ?
                                string.Empty : row["CauseCode"]),
                             ResolutionCode = Convert.ToString((row["ResolutionCode"])
                                == DBNull.Value ? string.Empty : row["ResolutionCode"]),
                             AvoidableFlag = Convert.ToString((row["AvoidableFlag"])
                                == DBNull.Value ? string.Empty : row["AvoidableFlag"]),
                             Priority = Convert.ToString((row["Priority"]) == DBNull.Value ?
                                string.Empty : row["Priority"]),
                             Status = Convert.ToString((row["Status"]) == DBNull.Value ?
                                string.Empty : row["Status"]),
                             AppLensStatus = Convert.ToString((row["AppLensStatus"])
                                == DBNull.Value ? string.Empty : row["AppLensStatus"]),
                             EffortTillDate = Convert.ToString((row["EffortTillDate"])
                                == DBNull.Value ? string.Empty : row["EffortTillDate"]),
                             InsertionMode = Convert.ToString((row["InsertionMode"])
                                == DBNull.Value ? string.Empty : row["InsertionMode"]),
                             OpenDate = Convert.ToString((row["OpenDate"]) == DBNull.Value ?
                                string.Empty : row["OpenDate"]),
                             ClosedDate = Convert.ToString((row["ClosedDate"]) == DBNull.Value ?
                                string.Empty : row["ClosedDate"]),
                             Assignee = Convert.ToString((row["Assignee"]) == DBNull.Value ?
                                string.Empty : row["Assignee"]),
                             ReopenDate = Convert.ToString((row["ReopenDate"]) == DBNull.Value ?
                                string.Empty : row["ReopenDate"]),
                             Source = Convert.ToString((row["Source"]) == DBNull.Value ?
                                string.Empty : row["Source"]),
                             Severity = Convert.ToString((row["Severity"]) == DBNull.Value ?
                                string.Empty : row["Severity"]),
                             DebtClassification = Convert.ToString((row["DebtClassification"])
                                == DBNull.Value ? string.Empty : row["DebtClassification"]),
                             ReleaseType = Convert.ToString((row["ReleaseType"]) == DBNull.Value ?
                                string.Empty : row["ReleaseType"]),
                             PlannedEffort = Convert.ToString((row["PlannedEffort"])
                                == DBNull.Value ? string.Empty : row["PlannedEffort"]),
                             EstimatedWorkSize = Convert.ToString((row["EstimatedWorkSize"])
                                == DBNull.Value ? string.Empty : row["EstimatedWorkSize"]),
                             ActualWorkSize = Convert.ToString((row["ActualWorkSize"])
                                == DBNull.Value ? string.Empty : row["ActualWorkSize"]),
                             PlannedStartDate = Convert.ToString((row["PlannedStartDate"])
                                == DBNull.Value ? string.Empty : row["PlannedStartDate"]),
                             PlannedEndDate = Convert.ToString((row["PlannedEndDate"])
                                == DBNull.Value ? string.Empty : row["PlannedEndDate"]),
                             NewStatusDateTime = Convert.ToString((row["NewStatusDateTime"])
                                == DBNull.Value ? string.Empty : row["NewStatusDateTime"]),
                             RejectedTimeStamp = Convert.ToString((row["RejectedTimeStamp"])
                                == DBNull.Value ? string.Empty : row["RejectedTimeStamp"]),
                             KEDBAvailableIndicator = Convert.ToString((row["KEDBAvailableIndicator"])
                                == DBNull.Value ? string.Empty : row["KEDBAvailableIndicator"]),
                             KEDBUpdated = Convert.ToString((row["KEDBUpdated"]) == DBNull.Value ?
                                string.Empty : row["KEDBUpdated"]),
                             ElevateFlagInternal = Convert.ToString((row["ElevateFlagInternal"])
                                == DBNull.Value ? string.Empty : row["ElevateFlagInternal"]),
                             RCAID = Convert.ToString((row["RCAID"]) == DBNull.Value ?
                                string.Empty : row["RCAID"]),
                             MetResponseSLA = Convert.ToString((row["MetResponseSLA"])
                                == DBNull.Value ? string.Empty : row["MetResponseSLA"]),
                             MetAcknowledgementSLA = Convert.ToString((row["MetAcknowledgementSLA"])
                                == DBNull.Value ? string.Empty : row["MetAcknowledgementSLA"]),
                             MetResolution = Convert.ToString((row["MetResolution"]) == DBNull.Value ?
                                string.Empty : row["MetResolution"]),
                             ActualStartDateTime = Convert.ToString((row["ActualStartDateTime"])
                                == DBNull.Value ? string.Empty : row["Hierarchy4"]),
                             ActualEndDateTime = Convert.ToString((row["ActualEndDateTime"])
                                == DBNull.Value ? string.Empty : row["ActualEndDateTime"]),
                             ActualDuration = Convert.ToString((row["ActualDuration"])
                                == DBNull.Value ? string.Empty : row["ActualDuration"]),
                             NatureOfTheTicket = Convert.ToString((row["NatureOfTheTicket"])
                                == DBNull.Value ? string.Empty : row["NatureOfTheTicket"]),
                             Comments = Convert.ToString((row["Comments"]) == DBNull.Value ?
                                string.Empty : row["Comments"]),
                             RepeatedIncident = Convert.ToString((row["RepeatedIncident"])
                                == DBNull.Value ? string.Empty : row["RepeatedIncident"]),
                             RelatedTickets = Convert.ToString((row["RelatedTickets"])
                                == DBNull.Value ? string.Empty : row["RelatedTickets"]),
                             KEDBPath = Convert.ToString((row["KEDBPath"]) == DBNull.Value ?
                                string.Empty : row["KEDBPath"]),
                             TicketCreatedBy = Convert.ToString((row["TicketCreatedBy"])
                                == DBNull.Value ? string.Empty : row["TicketCreatedBy"]),
                             ApprovedBy = Convert.ToString((row["ApprovedBy"]) == DBNull.Value ?
                                string.Empty : row["ApprovedBy"]),
                             StartedDateTime = Convert.ToString((row["StartedDateTime"])
                                == DBNull.Value ? string.Empty : row["StartedDateTime"]),
                             WIPDateTime = Convert.ToString((row["WIPDateTime"]) == DBNull.Value ?
                                string.Empty : row["WIPDateTime"]),
                             OnHoldDateTime = Convert.ToString((row["OnHoldDateTime"])
                                == DBNull.Value ? string.Empty : row["OnHoldDateTime"]),
                             CompletedDateTime = Convert.ToString((row["CompletedDateTime"])
                                == DBNull.Value ? string.Empty : row["CompletedDateTime"]),
                             CancelledDateTime = Convert.ToString((row["CancelledDateTime"])
                                == DBNull.Value ? string.Empty : row["CancelledDateTime"]),
                             OutageDuration = Convert.ToString((row["OutageDuration"])
                                == DBNull.Value ? string.Empty : row["OutageDuration"]),
                             ResidualDebt = Convert.ToString((row["ResidualDebt"])
                                == DBNull.Value ? string.Empty : row["ResidualDebt"]),
                             ResolutionRemarks = Convert.ToString((row["ResolutionRemarks"])
                                == DBNull.Value ? string.Empty : row["ResolutionRemarks"]),
                             EscalatedFlagCustomer = Convert.ToString((row["EscalatedFlagCustomer"])
                                == DBNull.Value ? string.Empty : row["EscalatedFlagCustomer"]),
                             FlexField1 = Convert.ToString((row["FlexField1"])
                                == DBNull.Value ? string.Empty : row["FlexField1"]),
                             FlexField2 = Convert.ToString((row["FlexField2"])
                                == DBNull.Value ? string.Empty : row["FlexField2"]),
                             FlexField3 = Convert.ToString((row["FlexField3"])
                                == DBNull.Value ? string.Empty : row["FlexField3"]),
                             FlexField4 = Convert.ToString((row["FlexField4"])
                                == DBNull.Value ? string.Empty : row["FlexField4"]),
                             Category = Convert.ToString((row["Category"])
                                == DBNull.Value ? string.Empty : row["Category"]),
                             Type = Convert.ToString((row["Type"])
                                == DBNull.Value ? string.Empty : row["Type"]),
                             TowerName = Convert.ToString((row["TowerName"])
                                == DBNull.Value ? string.Empty : row["TowerName"]),
                             ClientUserId = Convert.ToString((row["ClientUserID"])
                                == DBNull.Value ? string.Empty : row["ClientUserID"]),
                         }).ToList();
            }
            return lstTicketDetails;
        }

        /// <summary>
        /// Method to get the Business Cluster Data.        
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="associateID">Associate ID</param>
        /// <returns>Business Cluster Data</returns>
        public ProjectBusinessCluster GetBusinessClusterData(string customerID, string associateID)
        {
            ProjectBusinessCluster projCluster = new ProjectBusinessCluster();
            List<Project> lstProjects = new List<Project>();
            List<BusinessCluster> lstBusinessCluster = new List<BusinessCluster>();

            SqlParameter[] prmsObj = new SqlParameter[2];
            prmsObj[0] = new SqlParameter("@CustomerID", customerID);
            prmsObj[1] = new SqlParameter("@AssociateID", associateID);
            DataSet dsBusinessCluster = new DBHelper()
                .GetDatasetFromSP("AVL.Effort_GetSearchTicketProjectApplicationHierarchyFilter",
                prmsObj, ConnectionString);
            DataTable dtProjectDetails = dsBusinessCluster.Tables[0];
            DataTable dtBusinessCluster = dsBusinessCluster.Tables[1];
            DataTable dtSubBusinessCluster = dsBusinessCluster.Tables[2];

            if (dtBusinessCluster != null && dtBusinessCluster.Rows.Count > 0)
            {
                lstBusinessCluster = dtBusinessCluster.AsEnumerable().Select(row =>
                new BusinessCluster
                {
                    BusinessClusterId = Convert.ToInt32(row["BusinessClusterID"]),
                    BusinessClusterName = row["BusinessClusterName"].ToString(),
                    RowNumber = Convert.ToInt32(row["RowNumber"]),
                    IsInfra = Convert.ToBoolean(row["IsInfra"]),
                    SubBusinessClusterList = this.GetSubClusterDetails(
                        Convert.ToInt32(row["BusinessClusterID"]), dtSubBusinessCluster,
                        Convert.ToInt32(row["RowNumber"]), Convert.ToBoolean(row["IsInfra"])),

                }).ToList();
            }

            if (dtProjectDetails != null && dtProjectDetails.Rows.Count > 0)
            {
                lstProjects = dtProjectDetails.AsEnumerable().Select(row => new Project
                {
                    ProjectId = Convert.ToInt32(row["ProjectId"]),
                    ProjectName = Convert.ToString(row["ProjectName"]),
                    SupportTypeId = Convert.ToInt32(row["SupportTypeId"])

                }).ToList();
            }
            projCluster.Projects = lstProjects;
            projCluster.BusinessCluster = lstBusinessCluster;
            return projCluster;
        }

        /// <summary>
        /// Method to the Sub Cluster Details.
        /// </summary>
        /// <param name="businessClusterID">Business Cluster ID</param>
        /// <param name="dtResult">Business Cluster Details</param>
        /// <param name="rowNumber">Row Number</param>
        /// <returns>List of Sub Cluster Details</returns>
        private List<SubBusinessCluster> GetSubClusterDetails(int businessClusterID,
            DataTable dtResult, int rowNumber, bool infraEnabled)
        {
            List<SubBusinessCluster> lstSubClusterData = new List<SubBusinessCluster>();

            lstSubClusterData.AddRange(dtResult.AsEnumerable().
                    Where(dr => (dr.Field<Int64>("RANK1") == rowNumber) && (dr.Field<Int32>("IsInfra") == Convert.ToInt32(infraEnabled))).Select(row =>
                    new SubBusinessCluster
                    {
                        BusinessClusterMapId = Convert.ToString(row["SubClusterID"]),
                        BusinessClusterBaseName = Convert.ToString(row["BusinessClusterBaseName"]),
                        RowNumber = Convert.ToInt32(row["RANK1"]),
                        SubClusterId = Convert.ToString(row["SubClusterID"]),
                        ParentBusinessClusterMapId =
                            Convert.ToString(row["ParentBusinessClusterMapID"]),
                        ProjectId = Convert.ToInt32(row["ProjectID"]),
                        IsInfra = Convert.ToBoolean(row["IsInfra"]),
                        ApplicationList = this.GetApplicationDetails(rowNumber, dtResult, Convert.ToBoolean(row["IsInfra"])),
                    }).ToList());

            return lstSubClusterData;
        }

        /// <summary>
        /// Method to get the Application Details.
        /// </summary>
        /// <param name="rowNumber">Row Number</param>
        /// <param name="dtResult">Business Cluster Details</param>
        /// <returns>List of Application Details</returns>
        private List<Application> GetApplicationDetails(int rowNumber, DataTable dtResult, bool isInfra)
        {
            List<Application> lstApplication = new List<Application>();
            lstApplication = dtResult.AsEnumerable()
                .Where(dr => dr.Field<Int64>("RANK1") == rowNumber + 1
                    && dr.Field<Int64>("BusinessClusterID") == 7 && (dr.Field<Int32>("IsInfra") == Convert.ToInt32(isInfra)))
                .Select(row => new Application
                {
                    ParentBusinessClusterId = Convert.ToString(row["ParentBusinessClusterMapID"]),
                    ApplicationId = Convert.ToString(row["SubClusterID"]),
                    ApplicationName = Convert.ToString(row["BusinessClusterBaseName"]),
                    ProjectId = Convert.ToInt32(row["ProjectID"]),
                    IsInfra = Convert.ToBoolean(row["IsInfra"])
                }).ToList();
            return lstApplication;
        }
    }
}

