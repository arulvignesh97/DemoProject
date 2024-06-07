
namespace CTS.Applens.WorkProfiler.DAL
{
    using Common;
    using CTS.Applens.WorkProfiler.DAL.BaseDetails;
    using CTS.Applens.WorkProfiler.Entities;
    using CTS.Applens.WorkProfiler.Entities.Base;
    using CTS.Applens.Framework;
    using Models.ServiceClassification;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Linq;
    using System.Globalization;

    public class ServiceClassificationRepository : DBContext
    {
        public List<ServiceClassificationDictionaryDetails> GetDictionaryDetails()
        {
            SqlParameter[] prms1 = new SqlParameter[0];
            DataSet dtResultDictionary = new DataSet();
            dtResultDictionary.Locale = CultureInfo.InvariantCulture;
            List<ServiceClassificationDictionaryDetails> dictioanryDetails = new List<ServiceClassificationDictionaryDetails>();
            try
            {
                dtResultDictionary.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[GetDictionaryValuesForServiceClassification]", prms1,ConnectionString).Copy());
                if (dtResultDictionary != null && dtResultDictionary.Tables[0].Rows.Count > 0)
                {
                    dictioanryDetails = dtResultDictionary.Tables[0].AsEnumerable().Select(row => new ServiceClassificationDictionaryDetails
                    {
                        RuleId = (Int64)row["RULE_ID"],
                        Priority = (Int64)row["PRIORITY"],
                        WorkPattern = Convert.ToString((row["WORK_PATTERN"]) == DBNull.Value ?
                           string.Empty : row["WORK_PATTERN"]).ToLower(),
                        CauseCode = Convert.ToString((row["CAUSE_CODE"]) == DBNull.Value ? string.Empty :
                       row["CAUSE_CODE"]),
                        ResolutionCode = Convert.ToString((row["RESOLUTION_CODE"]) == DBNull.Value ? string.Empty :
                       row["RESOLUTION_CODE"]),
                        ServiceName = Convert.ToString((row["SERVICE_NAME"]) == DBNull.Value ? string.Empty :
                       row["SERVICE_NAME"])
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dictioanryDetails;
        }

        public List<ServiceTicketDetails> GetTicketDetails()
        {
            List<ServiceTicketDetails> tickketDetails = new List<ServiceTicketDetails>();
            SqlParameter[] prms = new SqlParameter[0];            
            DataSet dtResult = new DataSet();
            dtResult.Locale = CultureInfo.InvariantCulture;
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabledAPI"];
            AESEncryption aesMod = new AESEncryption();
            try
            {
                dtResult.Tables.Add((new DBHelper()).GetTableFromSP("[AVL].[GetTicketDetailsForServiceClassification]", prms,ConnectionString).Copy());
                if (dtResult != null && dtResult.Tables[0].Rows.Count > 0)
                {
                    if (encryptionEnabled == "Enabled")
                    {
                        tickketDetails = dtResult.Tables[0].AsEnumerable().Select(row => new ServiceTicketDetails
                        {
                            TimeTickerId = (Int64)row["TimeTickerID"],
                            TicketId = Convert.ToString((row["TicketID"]) == DBNull.Value ? string.Empty : row["TicketID"]),
                            TicketDescription =
                            Convert.ToString(string.IsNullOrEmpty(row["TicketDescription"].ToString()) ?
                            string.Empty :
                            aesMod.DecryptStringBytes((string)row["TicketDescription"],
                              Convert.FromBase64String(AseKeyDetail.AesKeyconstAPIval))).ToLower(),
                            CauseCode = Convert.ToString((row["CauseCode"]) == DBNull.Value ? string.Empty :
                        row["CauseCode"]).ToLower(),
                            ResolutionCode = Convert.ToString((row["ResolutionCode"]) == DBNull.Value ? string.Empty :
                        row["ResolutionCode"]).ToLower()
                        }).ToList();
                    }
                    else
                    {
                        tickketDetails = dtResult.Tables[0].AsEnumerable().Select(row => new ServiceTicketDetails
                        {
                            TimeTickerId = (Int64)row["TimeTickerID"],
                            TicketId = Convert.ToString((row["TicketID"]) == DBNull.Value ? string.Empty : row["TicketID"]),

                            TicketDescription = Convert.ToString((row["TicketDescription"]) == DBNull.Value ?
                           string.Empty : row["TicketDescription"]).ToLower(),

                            CauseCode = Convert.ToString((row["CauseCode"]) == DBNull.Value ? string.Empty :
                       row["CauseCode"]).ToLower(),
                            ResolutionCode = Convert.ToString((row["ResolutionCode"]) == DBNull.Value ? string.Empty :
                       row["ResolutionCode"]).ToLower()
                        }).ToList();
                    }
                }
               

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return tickketDetails;
        }

        public void ServiceNameUpdation(List<UpdateServiceName> updServiceName,List<ServiceAutoClassifiedTicketDetails> deleteTempTable)
        {
            DataTable dtServiceName = new DataTable();
            dtServiceName.Locale = CultureInfo.InvariantCulture;
            DataTable dtTimeTickerID = new DataTable();
            dtTimeTickerID.Locale = CultureInfo.InvariantCulture;
            DBHelper db = new DBHelper();
            dtServiceName = db.ToDataTableSingleRow<UpdateServiceName>(updServiceName);
            dtTimeTickerID = db.ToDataTableSingleRow<ServiceAutoClassifiedTicketDetails>(deleteTempTable);
            SqlParameter[] prms = new SqlParameter[2];          
            prms[0] = new SqlParameter("@BulkServiceName", dtServiceName);
            prms[0].SqlDbType = SqlDbType.Structured;
            prms[0].TypeName = "[AVL].[BulkServiceName]";
            prms[1] = new SqlParameter("@BulkTimeTickerID", dtTimeTickerID);
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = "[AVL].[DeleteServiceClassificationTempTable]";
            (new DBHelper()).ExecuteNonQueryReturn("[AVL].[UpdateServiceNameForBulkData]", prms,ConnectionString);
           
        }

        public List<UpdateServiceName> ServiceAutoClassificationAPI(List<ServiceTicketDetails> tickketDetails)
        {
            List<UpdateServiceName> servicaNameUpdate = new List<UpdateServiceName>();
            try
            {
                List<ServiceClassificationDictionaryDetails> dictioanryDetails = new List<ServiceClassificationDictionaryDetails>();
                ServiceClassificationRepository objService = new ServiceClassificationRepository();
                dictioanryDetails = objService.GetDictionaryDetails();



                servicaNameUpdate = (from tDetails in tickketDetails
                                     from dicDetails in dictioanryDetails
                                     where (tDetails.TicketDescription.ToLower(CultureInfo.CurrentCulture).Contains(dicDetails.WorkPattern.ToLower(CultureInfo.CurrentCulture)))
                                     select new UpdateServiceName
                                     {
                                         TimeTickerId = tDetails.TimeTickerId,
                                         ServiceName = dicDetails.ServiceName
                                     }).ToList();
                tickketDetails = tickketDetails.Where(excludeTicketDetails => !servicaNameUpdate.Select(serUpdate => serUpdate.TimeTickerId).
                Contains(excludeTicketDetails.TimeTickerId)).ToList();



                var splitDictDetails = (from dDetails in dictioanryDetails
                                        select new
                                        {
                                            RuleID = dDetails.RuleId,
                                            WorkPattern = dDetails.WorkPattern.Split(' ').ToList(),
                                            ServiceName = dDetails.ServiceName
                                        }).ToList();



                var splitDescDetails = (from descDetails in tickketDetails
                                        select new
                                        {
                                            TimeTickerID = descDetails.TimeTickerId,
                                            TicketDescription = descDetails.TicketDescription.Split(' ').ToList()
                                        }).ToList();



                var random = splitDescDetails.Select(des => splitDictDetails.Select(dic => new
                {
                    des.TimeTickerID,
                    ServiceName = (des.TicketDescription.Intersect(dic.WorkPattern).Count() == dic.WorkPattern.Count() ? dic.ServiceName : string.Empty)
                }).Where(match => !string.IsNullOrEmpty(match.ServiceName) && match.ServiceName != "")).ToList();



                var randomResult = random.SelectMany(finalMatch => finalMatch.Where(serName => !string.IsNullOrEmpty(serName.ServiceName))).ToList();



                List<UpdateServiceName> servicaNameUpdateRandom;
                servicaNameUpdateRandom = randomResult.Select(r => new UpdateServiceName
                {
                    TimeTickerId = r.TimeTickerID,
                    ServiceName = r.ServiceName
                }).ToList();



                servicaNameUpdate.AddRange(servicaNameUpdateRandom);



                tickketDetails = tickketDetails.Where(excludeTicketDetails => !servicaNameUpdate.Select(serUpdate => serUpdate.TimeTickerId).
                Contains(excludeTicketDetails.TimeTickerId)).ToList();



                List<UpdateServiceName> servicaNameUpdateCCRC;
                servicaNameUpdateCCRC = (from tickDetails in tickketDetails
                                         from ddDetails in dictioanryDetails
                                         where (tickDetails.CauseCode.ToLower(CultureInfo.CurrentCulture).Contains(ddDetails.CauseCode.ToLower(CultureInfo.CurrentCulture))
                                         && tickDetails.ResolutionCode.ToLower(CultureInfo.CurrentCulture).Contains(ddDetails.ResolutionCode.ToLower(CultureInfo.CurrentCulture)))
                                         select new UpdateServiceName
                                         {
                                             TimeTickerId = tickDetails.TimeTickerId,
                                             ServiceName = ddDetails.ServiceName
                                         }).ToList();
                List<UpdateServiceName> distinctCCRC;
                distinctCCRC = servicaNameUpdateCCRC.GroupBy(x => x.TimeTickerId).Select(y => y.First()).ToList();
                servicaNameUpdate.AddRange(distinctCCRC);
                return servicaNameUpdate;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}

