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
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    public class InitialLearningRepository : DBContext
    {
        public static readonly string Enabled = InitialLearningConstants.EncryptionEnabledIL;
        public static readonly string DefaultTickDesc = InitialLearningConstants.DefaultTicketDesc;
        public static readonly string Flag = InitialLearningConstants.Flag;

        /// <summary>
        /// Function to get the Configured Values for the project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        public MLDetails GetTopFiltersOnLoad(Int32 ProjectID, int SupportTypeID)
        {
            MLDetails objMLDetails = new MLDetails();
            try
            {
                DataSet dsResult = new DataSet();
                dsResult.Locale = CultureInfo.InvariantCulture;
                MLDetailsParam objMLDetailsParam = new MLDetailsParam();
                objMLDetailsParam.ProjectId = ProjectID;
                objMLDetailsParam.SupportTypeId = SupportTypeID;
                dsResult = new DBHelper().GetDatasetFromSP("AVL.ML_GetTopFilters",
                                                             DBHelper.CreatePara(objMLDetailsParam), ConnectionString);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        objMLDetails.StartDate = Convert.ToString(dsResult.Tables[0].Rows[0]["StartDate"]);
                        objMLDetails.EndDate = Convert.ToString(dsResult.Tables[0].Rows[0]["EndDate"]);
                        objMLDetails.IsAutoClassified = Convert.ToString(dsResult.Tables[0].Rows[0]
                            ["IsAutoClassified"]);
                        objMLDetails.OptionalFieldId = Convert.ToInt16(dsResult.Tables[0].Rows[0]["OptionalFieldID"]);
                        objMLDetails.IsMLSignoff = Convert.ToString(dsResult.Tables[0].Rows[0]["IsMLSignOff"]);
                    }
                    if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                    {
                        var OptionalFieldsInfo = dsResult.Tables[1].ToList<ProjOptionalFields>();
                        objMLDetails.LstOptionalFields = OptionalFieldsInfo.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objMLDetails;
        }



        /// <summary>
        /// Get Sampling Details
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<DebtSamplingModel> GetInfraDebtSamplingData(int projectID)
        {

            List<DebtSamplingModel> debtSamplingList = new List<DebtSamplingModel>();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", projectID);

            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();

            DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[InfraMLGetDebtSamplingDetails]", prms, ConnectionString);
            if (dt != null)
            {

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DebtSamplingModel ticketDetails = new DebtSamplingModel();
                        ticketDetails.TicketId = ((dt.Rows[i]["TicketID"] != DBNull.Value) ? dt.Rows[i]["TicketID"].
                            ToString() : string.Empty);

                        if (string.Compare(encryptionEnabled, Enabled) == 0)
                        {

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TicketDescription"])))
                            {
                                string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Rows[i]
                                    ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                                string decTicketDesc = bytesDecrypted;
                                ticketDetails.TicketDescription = decTicketDesc;
                            }

                            else
                            {
                                ticketDetails.TicketDescription = Convert.ToString(dt.Rows[i]["TicketDescription"]);
                            }
                        }
                        else
                        {
                            ticketDetails.TicketDescription = Convert.ToString(dt.Rows[i]["TicketDescription"]);
                        }

                        if (string.Compare(ticketDetails.TicketDescription, DefaultTickDesc) == 0)
                        {
                            ticketDetails.TicketDescription = string.Empty;
                        }
                        ticketDetails.PresenceOfOptional = (Convert.ToBoolean(dt.Rows[i]["PresenceOfOptioanl"] !=
                            DBNull.Value ? Convert.ToInt16(dt.Rows[i]["PresenceOfOptioanl"]) : 0));
                        if (ticketDetails.PresenceOfOptional)
                        {
                            ticketDetails.OptionalField = (Convert.ToInt16(dt.Rows[i]["OptionalFieldID"] !=
                                DBNull.Value ? Convert.ToInt16(dt.Rows[i]["OptionalFieldID"]) : 0));
                            if (ticketDetails.OptionalField == 2)
                            {
                                if (string.Compare(encryptionEnabled, Enabled) == 0)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AdditionalText"])))
                                    {
                                        string bytesDecrypted = aesMod.DecryptStringBytes
                                            ((string)dt.Rows[i]["AdditionalText"], AseKeyDetail.AesKeyConstVal);
                                        string decTicketDesc = bytesDecrypted;
                                        ticketDetails.AdditionalText = decTicketDesc;
                                    }

                                    else
                                    {
                                        ticketDetails.AdditionalText = Convert.ToString(dt.Rows[i]["AdditionalText"]);
                                    }
                                }
                                else
                                {
                                    ticketDetails.AdditionalText = Convert.ToString(dt.Rows[i]["AdditionalText"]);
                                }

                            }
                            else
                            {
                                ticketDetails.AdditionalText = Convert.ToString(dt.Rows[i]["AdditionalText"]);
                            }
                            ticketDetails.ResBaseWorkPattern = Convert.ToString(dt.Rows[i]["Res_Base_WorkPattern"]);
                            ticketDetails.ResSubWorkPattern = Convert.ToString(dt.Rows[i]["Res_Sub_WorkPattern"]);
                        }
                        ticketDetails.TowerName = dt.Rows[i]["TowerName"].ToString();
                        ticketDetails.TowerId = (Convert.ToInt32(dt.Rows[i]["TowerID"] != DBNull.Value ?
                            Convert.ToInt32(dt.Rows[i]["TowerID"]) : 0));
                        ticketDetails.DescBaseWorkPattern = Convert.ToString(dt.Rows[i]["TicketDescriptionPattern"]);
                        ticketDetails.DescSubWorkPattern = Convert.ToString(dt.Rows[i]["TicketDescriptionSubPattern"]);
                        ticketDetails.DebtClassificationId = (Convert.ToInt32(dt.Rows[i]["DebtClassificationID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["DebtClassificationID"]) : 0));
                        ticketDetails.DebtClassificationName = (Convert.ToString(dt.Rows[i]["DebtClassificationName"]
                            != DBNull.Value ? Convert.ToString(dt.Rows[i]["DebtClassificationName"]) : string.Empty));

                        ticketDetails.AvoidableFlagId = (Convert.ToInt32(dt.Rows[i]["AvoidableFlagID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["AvoidableFlagID"]) : 0));
                        ticketDetails.AvoidableFlagName = (Convert.ToString(dt.Rows[i]["AvoidableFlagName"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["AvoidableFlagName"]) : string.Empty));

                        ticketDetails.ResidualDebtId = (Convert.ToInt32(dt.Rows[i]["ResidualDebtID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ResidualDebtId"]) : 0));
                        ticketDetails.CauseCodeId = (Convert.ToInt32(dt.Rows[i]["CauseCodeID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["CauseCodeID"]) : 0));
                        ticketDetails.CauseCode = (Convert.ToString(dt.Rows[i]["CauseCode"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[i]["CauseCode"]) : string.Empty));
                        ticketDetails.ResolutionCodeId = (Convert.ToInt32(dt.Rows[i]["ResolutionCodeID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ResolutionCodeID"]) : 0));
                        ticketDetails.ResolutionCode = (Convert.ToString(dt.Rows[i]["ResolutionCode"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["ResolutionCode"]) : string.Empty));
                        ticketDetails.ResidualDebtId = (Convert.ToInt32(dt.Rows[i]["ResidualDebtID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ResidualDebtId"]) : 0));
                        ticketDetails.CauseCodeId = (Convert.ToInt32(dt.Rows[i]["CauseCodeID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["CauseCodeID"]) : 0));
                        ticketDetails.CauseCode = (Convert.ToString(dt.Rows[i]["CauseCode"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[i]["CauseCode"]) : ""));
                        ticketDetails.ResolutionCodeId = (Convert.ToInt32(dt.Rows[i]["ResolutionCodeID"] !=
                            DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ResolutionCodeID"]) : 0));
                        ticketDetails.ResolutionCode = (Convert.ToString(dt.Rows[i]["ResolutionCode"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["ResolutionCode"]) : ""));
                        debtSamplingList.Add(ticketDetails);
                    }
                }
            }
            return debtSamplingList;
        }
        /// <summary>
        /// to update sampled tickets
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateInfraSampledTicketsFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            DebtFieldsApprovalRepository debtapproval = new DebtFieldsApprovalRepository();
            string HiveFilepath = string.Empty;
            FilePath objFilePath = new FilePath();
            DataTable dtSampleddata = new DataTable();
            dtSampleddata.Locale = CultureInfo.InvariantCulture;
            objFilePath = debtapproval.GetInfraHivePathnameByJobId(MLJobId);

            HiveFilepath = objFilePath.OutputPath;
            string downloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];


            HiveFilepath = Utility.DownloadFile(HiveFilepath, downloadPath, 2);


            dtSampleddata = Utility.GetDataTabletFromCSVFile(HiveFilepath);

            //Code to get the file data from csv to table
            List<DebtSampledTicketsSaveModel> lstDebtSampledTicketsSaveModel =
                new List<DebtSampledTicketsSaveModel>();
            try
            {
                if (dtSampleddata != null)
                {
                    for (int i = 0; i < dtSampleddata.Rows.Count; i++)
                    {
                        DebtSampledTicketsSaveModel DebtMLPatternSaveModel = new DebtSampledTicketsSaveModel();
                        DebtMLPatternSaveModel.ESAProjectId = (Convert.ToString(dtSampleddata.Rows[i]["ESAProjectID"]
                            != DBNull.Value ? Convert.ToString(dtSampleddata.Rows[i]["ESAProjectID"]) :
                            string.Empty));
                        DebtMLPatternSaveModel.TicketId = (Convert.ToString(dtSampleddata.Rows[i]["TicketID"] !=
                            DBNull.Value ? Convert.ToString(dtSampleddata.Rows[i]["TicketID"]) : string.Empty));
                        if (dtSampleddata.Columns.Contains("TicketDescription"))
                        {
                            DebtMLPatternSaveModel.TicketDescription = (Convert.ToString(dtSampleddata.
                                Rows[i]["TicketDescription"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["TicketDescription"]) : string.Empty));
                        }

                        if (dtSampleddata.Columns.Contains("AdditionalText"))
                        {
                            DebtMLPatternSaveModel.AdditionalText = (Convert.ToString(dtSampleddata.
                                Rows[i]["AdditionalText"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["AdditionalText"]) : string.Empty));
                        }

                        DebtMLPatternSaveModel.TowerName = (Convert.ToString(dtSampleddata.
                            Rows[i]["Tower"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["Tower"]) : string.Empty));

                        if (dtSampleddata.Columns.Contains("DebtClassification"))
                        {
                            DebtMLPatternSaveModel.DebtClassification = (Convert.ToString(dtSampleddata.
                                Rows[i]["DebtClassification"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["DebtClassification"]) : string.Empty));
                        }
                        if (dtSampleddata.Columns.Contains("AvoidableFlag"))
                        {
                            DebtMLPatternSaveModel.AvoidableFlag = (Convert.ToString(dtSampleddata.
                                Rows[i]["AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["AvoidableFlag"]) : string.Empty));
                        }
                        if (dtSampleddata.Columns.Contains("ResidualDebt"))
                        {
                            DebtMLPatternSaveModel.ResidualDebt = (Convert.ToString(dtSampleddata.
                                Rows[i]["ResidualDebt"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["ResidualDebt"]) : string.Empty));
                        }
                        DebtMLPatternSaveModel.CauseCode = (Convert.ToString(dtSampleddata.Rows[i]["CauseCode"] !=
                            DBNull.Value ? Convert.ToString(dtSampleddata.Rows[i]["CauseCode"]) : string.Empty));
                        DebtMLPatternSaveModel.ResolutionCode = (Convert.ToString(dtSampleddata.
                            Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ResolutionCode"]) : string.Empty));
                        DebtMLPatternSaveModel.MLDebtClassification = (Convert.ToString(dtSampleddata.
                            Rows[i]["ML_DebtClassification"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ML_DebtClassification"]) : string.Empty));
                        DebtMLPatternSaveModel.DescBaseWorkPattern = (Convert.ToString(dtSampleddata.
                            Rows[i]["Desc_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["Desc_Base_WorkPattern"]) : string.Empty));
                        DebtMLPatternSaveModel.DescSubWorkPattern = (Convert.ToString(dtSampleddata.
                            Rows[i]["Desc_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["Desc_Sub_WorkPattern"]) : string.Empty));
                        if (dtSampleddata.Columns.Contains("Res_Base_WorkPattern"))
                        {
                            DebtMLPatternSaveModel.ResBaseWorkPattern = (Convert.ToString(dtSampleddata.
                                Rows[i]["Res_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["Res_Base_WorkPattern"]) : string.Empty));
                        }
                        if (dtSampleddata.Columns.Contains("Res_Sub_WorkPattern"))
                        {
                            DebtMLPatternSaveModel.ResSubWorkPattern = (Convert.ToString(dtSampleddata.
                                Rows[i]["Res_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["Res_Sub_WorkPattern"]) : string.Empty));
                        }
                        if (dtSampleddata.Columns.Contains("ResolutionCode"))
                        {
                            DebtMLPatternSaveModel.MLResolutionCode = (Convert.ToString(dtSampleddata.
                                Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                                Rows[i]["ResolutionCode"]) : string.Empty));
                        }
                        DebtMLPatternSaveModel.MLAvoidableFlag = (Convert.ToString(dtSampleddata.
                            Rows[i]["ML_AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ML_AvoidableFlag"]) : string.Empty));
                        DebtMLPatternSaveModel.MLResidualDebt = (Convert.ToString(dtSampleddata.
                            Rows[i]["ML_ResidualDebt"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ML_ResidualDebt"]) : string.Empty));

                        lstDebtSampledTicketsSaveModel.Add(DebtMLPatternSaveModel);

                    }
                }

                AESEncryption aesMod = new AESEncryption();
                var objCollection = from i in lstDebtSampledTicketsSaveModel
                                    select new
                                    {
                                        ESAProjectID = i.ESAProjectId,
                                        TicketID = i.TicketId,
                                        TicketDescription =
                                                string.IsNullOrEmpty(i.TicketDescription) ? string.Empty :
                                                Convert.ToBase64String(aesMod.EncryptStringAsBytes(i.
                                                TicketDescription, AseKeyDetail.AesKeyConstVal)),
                                        AdditionalText =
                                                string.IsNullOrEmpty(i.AdditionalText) ? string.Empty :
                                                Convert.ToBase64String(aesMod.EncryptStringAsBytes(i.
                                                AdditionalText, AseKeyDetail.AesKeyConstVal)),
                                        TowerName = i.TowerName,
                                        DebtClassification = i.DebtClassification,
                                        AvoidableFlag = i.AvoidableFlag,
                                        ResidualDebt = i.ResidualDebt,
                                        CauseCode = i.CauseCode,
                                        ResolutionCode = i.ResolutionCode,
                                        MLDebtClassification = i.MLDebtClassification,
                                        MLAvoidableFlag = i.MLAvoidableFlag,
                                        MLResidualDebt = i.MLResidualDebt,
                                        MLCauseCode = i.MLCauseCode,
                                        DescBaseWorkPattern = i.DescBaseWorkPattern,
                                        DescSubWorkPattern = i.DescSubWorkPattern,
                                        ResBaseWorkPattern = i.ResBaseWorkPattern,
                                        ResSubWorkPattern = i.ResSubWorkPattern
                                    };


                //updating the list to main pattern table
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@TVP_lstDebtSampleTickets", objCollection.ToList().ToDT());
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = "[AVL].[TVPDebtSampledTicketsInfra]";
                prms[2] = new SqlParameter("@UserID", UserID);
                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[MLSaveSampledTicketsFromAlgorithmInfra]",
                    prms, ConnectionString);
                debtapproval.AddTask(ProjectId, UserID, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return HiveFilepath;
        }

        /// <summary>
        /// Class DebtSampledTicketsCollection
        /// </summary>
        public class DebtInfraSampledTicketsCollection : List<DebtSampledTicketsSaveModel>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow1 = new SqlDataRecord(
                     new SqlMetaData("ESAProjectID", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("TicketID", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("TicketDescription", SqlDbType.VarChar, SqlMetaData.Max),
                           new SqlMetaData("AdditionalText", SqlDbType.VarChar, 1000),
                      new SqlMetaData("TowerName", SqlDbType.VarChar, SqlMetaData.Max),

                      new SqlMetaData("DebtClassification", SqlDbType.VarChar, 500),
                      new SqlMetaData("AvoidableFlag", SqlDbType.VarChar, 500),
                      new SqlMetaData("ResidualDebt", SqlDbType.VarChar, 500),
                      new SqlMetaData("CauseCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ResolutionCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("MLDebtClassification", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLAvoidableFlag", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLResidualDebt", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLCauseCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("DescBaseWorkPattern", SqlDbType.VarChar, 1000),
                      new SqlMetaData("DescSubWorkPattern", SqlDbType.VarChar, 1000),
                      new SqlMetaData("ResBaseWorkPattern", SqlDbType.VarChar, 1000),
                      new SqlMetaData("ResSubWorkPattern", SqlDbType.VarChar, 1000)
                      );

                foreach (DebtSampledTicketsSaveModel obj in this)
                {
                    sqlRow1.SetString(0, Convert.ToString(obj.ESAProjectId) != null ? Convert.
                        ToString(obj.ESAProjectId) : string.Empty);
                    sqlRow1.SetString(1, Convert.ToString(obj.TicketId) != null ? Convert.ToString(obj.TicketId)
                        : string.Empty);
                    sqlRow1.SetString(2, Convert.ToString(obj.TicketDescription) != null ? Convert.
                        ToString(obj.TicketDescription) : string.Empty);
                    sqlRow1.SetString(3, Convert.ToString(obj.AdditionalText) != null ? Convert.
                        ToString(obj.AdditionalText) : string.Empty);
                    sqlRow1.SetString(4, Convert.ToString(obj.TowerName) != null ? Convert.
                        ToString(obj.TowerName) : string.Empty);
                    sqlRow1.SetString(5, Convert.ToString(obj.DebtClassification) != null ? Convert.
                        ToString(obj.DebtClassification) : string.Empty);
                    sqlRow1.SetString(6, Convert.ToString(obj.AvoidableFlag) != null ? Convert.
                        ToString(obj.AvoidableFlag) : string.Empty);
                    sqlRow1.SetString(7, Convert.ToString(obj.ResidualDebt) != null ? Convert.
                        ToString(obj.ResidualDebt) : string.Empty);
                    sqlRow1.SetString(8, Convert.ToString(obj.CauseCode) != null ? Convert.
                        ToString(obj.CauseCode) : string.Empty);
                    sqlRow1.SetString(9, Convert.ToString(obj.ResolutionCode) != null ? Convert.
                        ToString(obj.ResolutionCode) : string.Empty);
                    sqlRow1.SetString(10, Convert.ToString(obj.MLDebtClassification) != null ? Convert.
                        ToString(obj.MLDebtClassification) : string.Empty);
                    sqlRow1.SetString(11, Convert.ToString(obj.MLAvoidableFlag) != null ? Convert.
                        ToString(obj.MLAvoidableFlag) : string.Empty);
                    sqlRow1.SetString(12, Convert.ToString(obj.MLResidualDebt) != null ? Convert.
                        ToString(obj.MLResidualDebt) : string.Empty);
                    sqlRow1.SetString(13, Convert.ToString(obj.MLCauseCode) != null ? Convert.
                        ToString(obj.MLCauseCode) : string.Empty);
                    sqlRow1.SetString(14, Convert.ToString(obj.DescBaseWorkPattern) != null ? Convert.
                        ToString(obj.DescBaseWorkPattern) : string.Empty);
                    sqlRow1.SetString(15, Convert.ToString(obj.DescSubWorkPattern) != null ? Convert.
                        ToString(obj.DescSubWorkPattern) : string.Empty);
                    sqlRow1.SetString(16, Convert.ToString(obj.ResBaseWorkPattern) != null ? Convert.
                        ToString(obj.ResBaseWorkPattern) : string.Empty);
                    sqlRow1.SetString(17, Convert.ToString(obj.ResSubWorkPattern) != null ? Convert.
                        ToString(obj.ResSubWorkPattern) : string.Empty);


                    yield return sqlRow1;
                }
            }
        }

        /// <summary>
        /// SaveDebtSamplingDetails
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="lstDebtSampling">lstDebtSampling</param>
        /// <returns></returns>
        public string SaveDebtSamplingDetails(string UserId, string ProjectID,
    List<GetDebtSamplingDetails> lstDebtSampling)
        {
            AESEncryption aesMod = new AESEncryption();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);

            var objCollection = from i in lstDebtSampling
                                select new
                                {
                                    TicketId = i.TicketId,
                                    TicketDescription = string.IsNullOrEmpty(i.TicketDescription) ?
                                                        string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                        IsNullOrEmpty(i.TicketDescription) ? string.Empty :
                                                        i.TicketDescription, AseKeyDetail.AesKeyConstVal)),
                                    AdditionalText = i.OptionalField == 2 ? (string.
                                                        IsNullOrEmpty(i.AdditionalText) ? string.Empty :
                                                        Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                        IsNullOrEmpty(i.AdditionalText) ? string.Empty :
                                                        i.AdditionalText, AseKeyDetail.AesKeyConstVal))) :
                                                        i.AdditionalText,
                                    DebtClassificationId = i.DebtClassificationId,
                                    AvoidableFlagId = i.AvoidableFlagId,
                                    ResidualDebtId = i.ResidualDebtId,
                                    CauseCode = i.CauseCode,
                                    ResolutionCode = i.ResolutionCode,
                                    DescBaseWorkPattern = i.DescBaseWorkPattern,
                                    DescSubWorkPattern = i.DescSubWorkPattern,
                                    ResBaseWorkPattern = i.ResBaseWorkPattern,
                                    ResSubWorkPattern = i.ResSubWorkPattern,
                                    TowerID = Convert.ToInt64(i.TowerId),
                                    ApplicationID = i.ApplicationId
                                };

            prms[1] = new SqlParameter("@TVP_SavelstDebtTickets", objCollection.ToList().ToDT());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = InitialLearningConstants.TypeSaveDebtSampledTickets;
            prms[2] = new SqlParameter("@UserId", UserId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[MLSaveSamplingDetailsInfra]", prms, ConnectionString);
                if (ds != null)
                {
                    //CCAP FIX
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Flag;


        }

        /// <summary>
        /// This Method is used to DebtSamplingCollection : List<GetDebtSamplingDetails>, IEnumerable<SqlDataRecord>
        /// </summary>
        public class DebtSamplingCollection : List<GetDebtSamplingDetails>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                       new SqlMetaData("TicketId", SqlDbType.VarChar, 1000),
                       new SqlMetaData("TicketDescription", SqlDbType.NVarChar, SqlMetaData.Max),
                       new SqlMetaData("AdditionalText", SqlDbType.VarChar, 1000),

                       new SqlMetaData("DebtClassificationId", SqlDbType.VarChar, 1000),
                       new SqlMetaData("AvoidableFlagId", SqlDbType.VarChar, 1000),
                       new SqlMetaData("ResidualDebtId", SqlDbType.VarChar, 1000),
                       new SqlMetaData("CauseCode", SqlDbType.VarChar, 1000),
                       new SqlMetaData("ResolutionCode", SqlDbType.VarChar, 1000),
                        new SqlMetaData("DescBaseWorkPattern", SqlDbType.VarChar, 1000),
                        new SqlMetaData("DescSubWorkPattern", SqlDbType.VarChar, 1000),
                        new SqlMetaData("ResBaseWorkPattern", SqlDbType.VarChar, 1000),
                        new SqlMetaData("ResSubWorkPattern", SqlDbType.VarChar, 1000),
                          new SqlMetaData("TowerID", SqlDbType.BigInt),
                          new SqlMetaData("ApplicationID", SqlDbType.VarChar, 1000)



                        );
                foreach (GetDebtSamplingDetails obj in this)
                {
                    sqlRow.SetString(0, obj.TicketId);
                    sqlRow.SetString(1, obj.TicketDescription != null ? obj.TicketDescription : string.Empty);
                    //made null since TVP is reused
                    sqlRow.SetString(2, obj.AdditionalText != null ? obj.AdditionalText : string.Empty);
                    sqlRow.SetString(3, obj.DebtClassificationId != null ? obj.DebtClassificationId : string.Empty);
                    sqlRow.SetString(4, obj.AvoidableFlagId != null ? obj.AvoidableFlagId : string.Empty);
                    sqlRow.SetString(5, obj.ResidualDebtId != null ? obj.ResidualDebtId : string.Empty);
                    sqlRow.SetString(6, obj.CauseCode != null ? obj.CauseCode : string.Empty);
                    sqlRow.SetString(7, obj.ResolutionCode != null ? obj.ResolutionCode : string.Empty);


                    sqlRow.SetString(8, obj.DescBaseWorkPattern != null ? obj.DescBaseWorkPattern : string.Empty);
                    sqlRow.SetString(9, obj.DescSubWorkPattern != null ? obj.DescSubWorkPattern : string.Empty);
                    sqlRow.SetString(10, obj.ResBaseWorkPattern != null ? obj.ResBaseWorkPattern : string.Empty);
                    sqlRow.SetString(11, obj.ResSubWorkPattern != null ? obj.ResSubWorkPattern : string.Empty);
                    sqlRow.SetInt64(12, obj.TowerId != null ? obj.TowerId : 0);
                    sqlRow.SetString(13, String.IsNullOrEmpty(obj.ApplicationId) == false ? obj.ApplicationId : "0");


                    yield return sqlRow;
                }
            }
        }

        /// <summary>
        /// Submit Sampling details
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtSampling">List of sampling details</param>
        /// <returns></returns>
        public string SubmitDebtSamplingDetails(string UserId, int ProjectID,
            List<GetDebtSamplingDetails> lstDebtSampling)
        {
            AESEncryption aesMod = new AESEncryption();
            string CriteriaMet = string.Empty;
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);

            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            var objCollection = from i in lstDebtSampling
                                select new
                                {
                                    TicketId = i.TicketId,
                                    TicketDescription = string.IsNullOrEmpty(i.TicketDescription) ?
                                                        string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                        IsNullOrEmpty(i.TicketDescription) ? string.Empty :
                                                        i.TicketDescription, AseKeyDetail.AesKeyConstVal)),
                                    AdditionalText = i.OptionalField == 2 ? (string.
                                                        IsNullOrEmpty(i.AdditionalText) ? string.Empty :
                                                        Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                        IsNullOrEmpty(i.AdditionalText) ? string.Empty :
                                                        i.AdditionalText, AseKeyDetail.AesKeyConstVal))) :
                                                        i.AdditionalText,
                                    DebtClassificationId = i.DebtClassificationId,
                                    AvoidableFlagId = i.AvoidableFlagId,
                                    ResidualDebtId = i.ResidualDebtId,
                                    CauseCode = i.CauseCode,
                                    ResolutionCode = i.ResolutionCode,
                                    DescBaseWorkPattern = i.DescBaseWorkPattern,
                                    DescSubWorkPattern = i.DescSubWorkPattern,
                                    ResBaseWorkPattern = i.ResBaseWorkPattern,
                                    ResSubWorkPattern = i.ResSubWorkPattern,
                                    TowerID = Convert.ToInt64(i.TowerId),
                                    ApplicationID = i.ApplicationId
                                };


            prms[1] = new SqlParameter("@TVP_lstDebtTickets", objCollection.ToList().ToDT());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = InitialLearningConstants.TypeSaveDebtSampledTickets;
            prms[2] = new SqlParameter("@UserId", UserId);
            try
            {
                ds = (new DBHelper()).GetDatasetFromSP("[AVL].[MLSubmitSamplingDetailsInfra]", prms, ConnectionString);
                if (ds != null)
                {
                    CriteriaMet = ds.Tables[0].Rows[0]["CriteriaMet"].ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return CriteriaMet;
        }


    }
}
