using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Common;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// DebtFieldsApprovalRepository
    /// </summary>
    public class DebtFieldsApprovalRepository : DBContext
    {

        public static readonly string Flag = InitialLearningConstants.Flag;
        public static readonly string Enabled = InitialLearningConstants.EncryptionEnabledIL;
        public static readonly string DefaultTickDesc = InitialLearningConstants.DefaultTicketDesc;
        public static readonly string DefaultTickSummary = InitialLearningConstants.DefaultTicketSummary;
        public static readonly string DescWordFile = InitialLearningConstants.NoiseListDescFile;
        public static readonly string ResWordFile = InitialLearningConstants.NoiseListResFile;
        public static readonly string FileStatNotFound = InitialLearningConstants.FileStatusNotFound;
        public static readonly string FileStatSuccess = InitialLearningConstants.FileStatusSuccess;
        public static readonly bool FlagTrue = new InitialLearningConstants().FlagTrue;
        public static readonly bool FlagFalse = new InitialLearningConstants().FlagFalse;
        public static readonly string MlDataExtraction = InitialLearningConstants.MLDataExtractionFileName;
        public static readonly string TypeDebtTickets = InitialLearningConstants.TypeDebtTickets;
        public static readonly string TypeDebtTicketsInfra = InitialLearningConstants.TypeDebtTicketsInfra;
        public static readonly string TypeMLTicketDescWordList = InitialLearningConstants.TypeMLTicketDescWordList;
        public static readonly string TypeMLOptionalWordList = InitialLearningConstants.TypeMLOptionalWordList;

        /// <summary>
        /// Initial stage
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="Datefrom">Date from</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="UserID">User ID</param>
        /// <param name="IsSMTicket">IsSMTicket</param>
        /// <param name="IsDARTTicket">IsDARTTicket</param>
        /// <param name="OptFieldProjID">OptField ProjID</param>
        /// <returns></returns>
        public List<GetDebtTicketsForValidation> GetDebtValidateTicketsForML(int ProjectID, DateTime Datefrom,
            DateTime DateTo, string UserID, int OptFieldProjID, int SupportTypeID)
        {
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            string EncryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            string SaveILSaveML = InitialLearningConstants.SaveInitialLearningChoicesSaveML;
            AESEncryption aesMod = new AESEncryption();
            string IsSMTicket = "1";
            string IsDARTTicket = "1";
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            List<GetDebtTicketsForValidation> lstDebtTicketsForValidation = new List<GetDebtTicketsForValidation>();
            int OptionalfieldId;
            if (SupportTypeID == 1)
            {
                SqlParameter[] prms1 = new SqlParameter[8];
                prms1[0] = new SqlParameter("@ProjectID", ProjectID);
                prms1[1] = new SqlParameter("@StartDate", Datefrom);
                prms1[2] = new SqlParameter("@EndDate", DateTo);
                prms1[3] = new SqlParameter("@UserID", UserID);
                prms1[4] = new SqlParameter("@IsSMTicket", IsSMTicket);
                prms1[5] = new SqlParameter("@IsDARTTicket", IsDARTTicket);
                prms1[6] = new SqlParameter("@OptFieldsForProj", OptFieldProjID);
                prms1[7] = new SqlParameter("@choice", SaveILSaveML);
                (new DBHelper()).ExecuteNonQuery("ML_SaveInitialLearningState", prms1, ConnectionString);
            }
            else
            {
                SqlParameter[] prmsInfra = new SqlParameter[6];
                prmsInfra[0] = new SqlParameter("@ProjectID", ProjectID);
                prmsInfra[1] = new SqlParameter("@StartDate", Datefrom);
                prmsInfra[2] = new SqlParameter("@EndDate", DateTo);
                prmsInfra[3] = new SqlParameter("@UserID", UserID);
                prmsInfra[4] = new SqlParameter("@OptFieldsForProj", OptFieldProjID);
                prmsInfra[5] = new SqlParameter("@choice", SaveILSaveML);
                (new DBHelper()).ExecuteNonQuery("[AVL].[ML_SaveInitialLearningStateInfra]", prmsInfra, ConnectionString);
            }
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@DateFrom", Datefrom);
            prms[2] = new SqlParameter("@DateTo", DateTo);
            prms[3] = new SqlParameter("@UserID", UserID);
            if (SupportTypeID == 1)
            {

                dt = (new DBHelper()).GetTableFromSP("ML_GetlistofprojectsforInitialML", prms, ConnectionString);
            }
            else
            {
                dt = (new DBHelper()).GetTableFromSP("[AVL].[ML_InsertValidTicketsInfra]", prms, ConnectionString);
            }
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    OptionalfieldId = Convert.ToInt16(dt.Rows[0]["OptionalFieldId"], CultureInfo.CurrentCulture);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetDebtTicketsForValidation TicketDetails = new GetDebtTicketsForValidation();
                        TicketDetails.TicketId = ((dt.Rows[i]["TicketID"] != DBNull.Value) ? dt.Rows[i]["TicketID"]
                            .ToString() : string.Empty);

                        if (string.Compare(EncryptionEnabled, Enabled) == 0)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TicketDescription"], CultureInfo.CurrentCulture)))
                            {

                                string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Rows[i]
                                    ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                                string decTicketDesc = bytesDecrypted;
                                TicketDetails.TicketDescription = decTicketDesc;
                            }
                            else
                            {
                                TicketDetails.TicketDescription = Convert.ToString(dt.Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                            }
                        }
                        else
                        {
                            TicketDetails.TicketDescription = Convert.ToString(dt.Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                        }
                        if (string.Compare(TicketDetails.TicketDescription, DefaultTickDesc) == 0)
                        {
                            TicketDetails.TicketDescription = string.Empty;
                        }
                        if (SupportTypeID == 2)
                        {
                            TicketDetails.TowerId = (Convert.ToInt32(dt.Rows[i]["TowerID"] !=
                                                     DBNull.Value ? Convert.ToInt32(dt.Rows[i]["TowerID"], CultureInfo.CurrentCulture) : 0));
                        }
                        else
                        {
                            TicketDetails.ApplicationId = (Convert.ToString(dt.Rows[i]["ApplicationID"] !=
                                DBNull.Value ? Convert.ToString(dt.Rows[i]["ApplicationID"], CultureInfo.InvariantCulture) : string.Empty));
                        }
                        TicketDetails.CauseCode = (Convert.ToString(dt.Rows[i]["CauseCodeID"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["CauseCodeID"], CultureInfo.InvariantCulture) : string.Empty));
                        TicketDetails.ResolutionCode = (Convert.ToString(dt.Rows[i]["ResolutionCodeID"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["ResolutionCodeID"], CultureInfo.InvariantCulture) : string.Empty));
                        TicketDetails.DebtClassificationId = (Convert.ToString(dt.Rows[i]["DebtClassificationID"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["DebtClassificationID"], CultureInfo.InvariantCulture) : string.Empty));
                        TicketDetails.AvoidableFlagId = (Convert.ToString(dt.Rows[i]["AvoidableFlagID"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["AvoidableFlagID"],CultureInfo.InvariantCulture) : string.Empty));
                        TicketDetails.ResidualDebtId = (Convert.ToString(dt.Rows[i]["ResidualDebtID"] !=
                            DBNull.Value ? Convert.ToString(dt.Rows[i]["ResidualDebtID"], CultureInfo.InvariantCulture) : string.Empty));
                        TicketDetails.OptionalField = Convert.ToString(dt.Rows[i]["OptionalFieldProj"], CultureInfo.InvariantCulture);
                        lstDebtTicketsForValidation.Add(TicketDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDebtTicketsForValidation;
        }
        /// <summary>
        /// Updating Icon Details
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <param name="TicketConsidered"></param>
        /// <param name="TicketAnalysed"></param>
        /// <param name="SamplingCount"></param>
        /// <param name="PatternCount"></param>
        /// <param name="ApprovedCount"></param>
        /// <param name="MuteCount"></param>
        /// <param name="userid"></param>
        public void UpdateILCountDetails(string Choose, int ProjectId, int TicketConsidered, int TicketAnalysed,
            int SamplingCount, int PatternCount, int ApprovedCount, int MuteCount, string userid, int SupportID)
        {
            try
            {
                SqlParameter[] prms1 = new SqlParameter[9];
                prms1[0] = new SqlParameter("@ProjectID", ProjectId);
                prms1[1] = new SqlParameter("@TicketConsidered", TicketConsidered);
                prms1[2] = new SqlParameter("@TicketAnalysed", TicketAnalysed);
                prms1[3] = new SqlParameter("@SamplingCount", SamplingCount);
                prms1[4] = new SqlParameter("@PatternCount", PatternCount);
                prms1[5] = new SqlParameter("@ApprovedCount", ApprovedCount);
                prms1[6] = new SqlParameter("@MuteCount", MuteCount);
                prms1[7] = new SqlParameter("@choice", Choose);
                prms1[8] = new SqlParameter("@userid", userid);
                if (SupportID == 1)
                {
                    (new DBHelper()).ExecuteNonQuery("[dbo].[ML_UpdateILCountDetailsByChoice]", prms1, ConnectionString);
                }
                else
                {
                    (new DBHelper()).ExecuteNonQuery("[AVL].[MLInfraUpdateILCountDetailsByChoice]", prms1, ConnectionString);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// To get noise data and save to db
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="NoiseEliminationJobId">NoiseElimination JobId</param>
        /// <returns></returns>
        public NoiseElimination GetNoiseEliminationData(int projectID, string NoiseEliminationJobId, int SupportTypeID)
        {
            NoiseElimination noiseEliminationDetails = new NoiseElimination();
            noiseEliminationDetails = CheckIfNoiseOutputFileGenerated(projectID, NoiseEliminationJobId, SupportTypeID);
            if (noiseEliminationDetails != null && noiseEliminationDetails.LstNoiseTicketDescription.Count > 0)
            {
                SaveNoiseEliminationDetails(noiseEliminationDetails, projectID, 1, "System");
            }
            return noiseEliminationDetails;

        }
        /// <summary>
        /// To skip optional field upload
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string UpdateOptUpload(Int64 ProjectID, int SupportTypeID)
        {
            string CriteriaMet = string.Empty;

            try
            {
                SqlParameter[] prms1 = new SqlParameter[2];
                prms1[0] = new SqlParameter("@ProjectID", ProjectID);
                prms1[1] = new SqlParameter("@SupportTypeID", SupportTypeID);

                DataTable dt = (new DBHelper()).GetTableFromSP("ML_UpdateOptionalFieldUpload ", prms1, ConnectionString);
                if (dt != null)
                {
                    CriteriaMet = dt.Rows[0]["CriteriaMet"].ToString();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return CriteriaMet;

        }

        /// <summary>
        /// This Method is used to UpdateNoiseSkipAndContinue
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string UpdateNoiseSkipAndContinue(Int64 ProjectID, string EmployeeID)
        {
            string CriteriaMet = string.Empty;

            try
            {
                SqlParameter[] prms1 = new SqlParameter[2];
                prms1[0] = new SqlParameter("@ProjectID", ProjectID);
                prms1[1] = new SqlParameter("@UserID", EmployeeID);
                DataTable dt = (new DBHelper()).GetTableFromSP("ML_UpdateNoiseEliminationSkipOrCont", prms1, ConnectionString);
                if (dt != null)
                {
                    CriteriaMet = dt.Rows[0]["CriteriaMet"].ToString();
                }


            }

            catch (Exception ex)
            {
                throw ex;
            }
            return CriteriaMet;

        }
        /// <summary>
        /// To get criteria for project
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtTickets">List of valid tickets</param>
        /// <param name="UserId">User Id</param>
        /// <param name="DateFrom">Date From</param>
        /// <param name="DateTo">Date To</param>
        /// <returns></returns>
        public string SaveDebtTicketDetailsAfterProcessing(Int32 ProjectID,
            List<GetDebtTicketsForValidation> lstDebtTickets, string UserId, string DateFrom, string DateTo,
            int SupportTypeID)
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            AESEncryption aesMod = new AESEncryption();
            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
            string CriteriaMet = string.Empty;

            try
            {
                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                
                if (SupportTypeID == 2)
                {
                   
                    var objCollection = from i in lstDebtTickets
                                        select new
                                        {

                                            TicketId = i.TicketId,
                                            TicketDescription = string.IsNullOrEmpty(i.TicketDescription.ToString(CultureInfo.CurrentCulture)) ?
                                                                   string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                                   IsNullOrEmpty(i.TicketDescription.ToString(CultureInfo.CurrentCulture)) ? string.Empty :
                                                                   i.TicketDescription.ToString(CultureInfo.CurrentCulture), AseKeyDetail.AesKeyConstVal)),
                                            TowerID = (i.TowerId == null ? 0 : Convert.ToInt64(i.TowerId, CultureInfo.CurrentCulture)),
                                            DebtClassificationId = string.IsNullOrEmpty(i.DebtClassificationId) ? 0 : Convert.ToInt32(i.DebtClassificationId, CultureInfo.CurrentCulture),
                                            AvoidableFlagId = string.IsNullOrEmpty(i.AvoidableFlagId) ? 0 : Convert.ToInt32(i.AvoidableFlagId, CultureInfo.CurrentCulture),
                                            CauseCode = string.IsNullOrEmpty(i.CauseCode) ? 0 : Convert.ToInt64(i.CauseCode, CultureInfo.CurrentCulture),
                                            ResolutionCode = string.IsNullOrEmpty(i.ResolutionCode) ? 0 : Convert.ToInt64(i.ResolutionCode, CultureInfo.CurrentCulture),
                                            ResidualDebtId = string.IsNullOrEmpty(i.ResidualDebtId) ? 0 : Convert.ToInt32(i.ResidualDebtId, CultureInfo.CurrentCulture),
                                            OptionalField = i.OptionalField
                                        };

                    prms[1] = new SqlParameter("@TVP_lstDebtTickets", objCollection.ToList().ToDT());
                    prms[1].SqlDbType = SqlDbType.Structured;
                    prms[1].TypeName = TypeDebtTicketsInfra;
                    prms[2] = new SqlParameter("@UserId", UserId);
                    prms[3] = new SqlParameter("@DateFrom", DateFrom);
                    prms[4] = new SqlParameter("@DateTo", DateTo);
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveProcessedTicketDetailsInfra]", prms, ConnectionString);
                }
                else
                {
                    var objCollection = from i in lstDebtTickets
                                        select new
                                        {

                                            TicketId = i.TicketId,
                                            TicketDescription = string.IsNullOrEmpty(i.TicketDescription.ToString(CultureInfo.CurrentCulture)) ?
                                                                   string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                                                   IsNullOrEmpty(i.TicketDescription.ToString(CultureInfo.CurrentCulture)) ? string.Empty :
                                                                   i.TicketDescription.ToString(CultureInfo.CurrentCulture), AseKeyDetail.AesKeyConstVal)),
                                            ApplicationID = i.ApplicationId,
                                            DebtClassificationId = i.DebtClassificationId,
                                            AvoidableFlagId = i.AvoidableFlagId,
                                            CauseCode = i.CauseCode,
                                            ResolutionCode = i.ResolutionCode,
                                            ResidualDebtId = i.ResidualDebtId,
                                            OptionalField = i.OptionalField
                                        };

                    prms[1] = new SqlParameter("@TVP_lstDebtTickets", objCollection.ToList().ToDT());
                    prms[1].SqlDbType = SqlDbType.Structured;
                    prms[1].TypeName = TypeDebtTickets;
                    prms[2] = new SqlParameter("@UserId", UserId);
                    prms[3] = new SqlParameter("@DateFrom", DateFrom);
                    prms[4] = new SqlParameter("@DateTo", DateTo);
                    ds = (new DBHelper()).GetDatasetFromSP("ML_SaveProcessedTicketDetails", prms, ConnectionString);
                }
                if (ds != null && ds.Tables.Count > 0)
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

        /// <summary>
        /// Tickets details for ML
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="UserID">Employee ID</param>
        /// <returns></returns>

        public string MLDatSetBindingForCSVCreation(int ProjectID, string UserID, Int32 SupportType)
        {
            string jobSuccess = string.Empty;
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                if (SupportType == 2)
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_FinalTicketDetailsforcallingMLInfra", prms, ConnectionString);
                }
                else
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_FinalTicketDetailsforcallingML", prms, ConnectionString);

                }

                string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                AESEncryption aesMod = new AESEncryption();

                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (string.Compare(encryptionEnabled, Enabled) == 0)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dsResult.Tables[0].
                                Rows[i]["TicketDescription"], CultureInfo.CurrentCulture)))
                            {

                                string bytesDecrypted = aesMod.DecryptStringBytes((string)dsResult.Tables[0].Rows[i]
                                    ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                                string decTicketDesc = bytesDecrypted;
                                dsResult.Tables[0].Rows[i]["TicketDescription"] = decTicketDesc;
                            }
                            else
                            {
                                dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.
                                    ToString(dsResult.Tables[0].Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                            }
                        }
                        else
                        {
                            dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.
                                ToString(dsResult.Tables[0].Rows[i]["TicketDescription"], CultureInfo.CurrentCulture);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                jobSuccess = SubmitMLJob(dsResult, ProjectID, UserID, SupportType);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jobSuccess;
        }
        /// <summary>
        /// Error Log insert
        /// </summary>
        /// <param name="ErrorMessage">Error Message</param>
        /// <param name="step">step</param>
        /// <param name="ProjectID">Project ID</param>
        public void ErrorLOG(string ErrorMessage, string step, Int64 ProjectID)
        {
            ErrorLOG(ErrorMessage, step, ProjectID, null);
        }
        public void ErrorLOG(string ErrorMessage, string step, Int64 ProjectID, string UserID)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@step", step);
                prms[2] = new SqlParameter("@ErrorMessage", ErrorMessage);
                prms[3] = new SqlParameter("@UserID", UserID);
                (new DBHelper()).ExecuteNonQuery("ML_InsertErrorLog", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To get Master values for sampling screen
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<DebtSamplingValues> GetDebtSamplingDataValues(int projectID, int SupportTypeID)
        {

            List<DebtSamplingValues> debtApprovalList = new List<DebtSamplingValues>();

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", projectID);
            prms[1] = new SqlParameter("@SupportType", SupportTypeID);

            DataTable dt = (new DBHelper()).GetTableFromSP("ML_MasterValuesForPatternValidation", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                debtApprovalList = dt.AsEnumerable().Select(row => new DebtSamplingValues
                {
                    AttributeType = Convert.ToString(row["AttributeType"], CultureInfo.CurrentCulture),
                    AttributeTypeId = Convert.ToInt32(row["AttributeTypeId"], CultureInfo.CurrentCulture),
                    AttributeTypeValue = Convert.ToString(row["AttributeTypeValue"], CultureInfo.CurrentCulture)

                }).ToList();
            }
            return debtApprovalList;
        }
        /// <summary>
        /// save sampling details
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtSampling">Sampling details</param>
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
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_SaveSamplingDetails", prms, ConnectionString);
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
        /// To get details for invoking ML Job
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="UserID">Employee ID</param>
        /// <returns></returns>
        public string MLDatSetBindingAfterSamplingForCSVCreation(int projectID, string UserID, Int32 SupportId)
        {
            string jobSuccess = string.Empty;
            SqlParameter[] prms = new SqlParameter[1];
            DataSet dsTickets = new DataSet();
            dsTickets.Locale = CultureInfo.InvariantCulture;

            prms[0] = new SqlParameter("@ProjectID", projectID);
            try
            {
                if (SupportId == 1)
                {
                    dsTickets = (new DBHelper()).GetDatasetFromSP("ML_GetSamplingDetailsForML", prms, ConnectionString);
                }
                else
                {
                    dsTickets = (new DBHelper()).GetDatasetFromSP("[AVL].[InfraGetSamplingDetailsForML]", prms, ConnectionString);
                }
                int optionalfielid;
                bool presenceOfOptional;
                if (dsTickets.Tables[3].Rows.Count > 0)
                {
                    optionalfielid = Convert.ToInt16(dsTickets.Tables[3].Rows[0]["OptionalFieldID"], CultureInfo.CurrentCulture);
                    presenceOfOptional = Convert.ToBoolean(dsTickets.Tables[3].Rows[0]["PresenceOfOptional"], CultureInfo.CurrentCulture);
                }

                else
                {
                    optionalfielid = 0;
                    presenceOfOptional = false;

                }


                jobSuccess = SubmitMLJob(dsTickets, projectID, UserID, SupportId);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return jobSuccess;
        }
        /// <summary>
        /// This Method is used to UpdateSamplingSubmitFlag
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        public string UpdateSamplingSubmitFlag(int ProjectId, string AssociateID, Int32 SupportId)
        {
            string IsMLUpdated = string.Empty;
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@UserID", AssociateID);
                prms[2] = new SqlParameter("@choice", "SamplingSubmit");
                prms[3] = new SqlParameter("@SupportId", SupportId);

                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_SaveInitialLearningStateMLSubmit", prms, ConnectionString);
                IsMLUpdated = Flag;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return IsMLUpdated;
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
                                    TicketDescription = string.IsNullOrEmpty(i.TicketDescription) ? string.Empty :
                                            Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.IsNullOrEmpty(i.
                                            TicketDescription) ? string.Empty : i.TicketDescription,
                                            AseKeyDetail.AesKeyConstVal)),
                                    AdditionalText = i.OptionalField == 2 ? (string.IsNullOrEmpty(i.
                                            AdditionalText) ? string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(string.
                                            IsNullOrEmpty(i.AdditionalText) ? string.Empty : i.
                                            AdditionalText, AseKeyDetail.AesKeyConstVal))) : i.
                                            AdditionalText,
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
                                    ApplicationId = i.ApplicationId
                                };
            prms[1] = new SqlParameter("@TVP_lstDebtTickets", objCollection.ToList().ToDT());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = InitialLearningConstants.TypeSaveDebtSampledTickets;
            prms[2] = new SqlParameter("@UserId", UserId);
            try
            {
                ds = (new DBHelper()).GetDatasetFromSP("ML_SubmitSamplingDetails", prms, ConnectionString);
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
        /// <summary>
        /// To get Project details
        /// </summary>
        /// <param name="EmployeeID">Employee ID</param>
        /// <param name="CustomerID">Customer ID</param>
        /// <returns></returns>
        public List<GetProjectDetailsById> GetProjectDetailsByEmployeeID(string EmployeeID, int CustomerID)
        {
            List<GetProjectDetailsById> mLProjectDetails = new List<GetProjectDetailsById>();

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            DataTable dt = (new DBHelper()).GetTableFromSP("ML_GetAVMInitialLearningProjectDetails", prms, ConnectionString);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetProjectDetailsById ProjectDetails = new GetProjectDetailsById();
                    ProjectDetails.ProjectId = dt.Rows[i]["ProjectID"].ToString();
                    ProjectDetails.ProjectName = dt.Rows[i]["ProjectName"].ToString();
                    ProjectDetails.SupportTypeId = Convert.ToInt16(dt.Rows[i]["SupportTypeId"], CultureInfo.CurrentCulture);

                    mLProjectDetails.Add(ProjectDetails);
                }
            }
            return mLProjectDetails;
        }
        /// <summary>
        /// Submitting ML job
        /// </summary>
        /// <param name="dsResult">Dataset contain ticket details</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="UserID">Employee ID</param>
        /// <returns></returns>
        private string SubmitMLJob(DataSet dsResult, int ProjectID, string UserID, Int32 SupportTypeID)
        {

            string jobSuccess = string.Empty;
            string filePath = string.Empty;
            string filePathDesc = string.Empty;
            string filePathRes = string.Empty;
            GetMLJobDetails MLJobDetails = new GetMLJobDetails();
            bool PresenceOfOptionalField = false;
            if (dsResult.Tables[3].Rows.Count > 0)
            {
                PresenceOfOptionalField = Convert.ToBoolean(dsResult.Tables[3].Rows[0]["PresenceOfOptional"].ToString(),CultureInfo.CurrentCulture);
            }

            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0 && dsResult.Tables[1] != null)
                    {

                        string timeStamp = string.Format("{0:yyyy-MM-dd-HHmmss}", DateTimeOffset.Now.DateTime,CultureInfo.CurrentCulture);
                        string path = new AppSettings().AppsSttingsKeyValues["MLTempPath"];
                        string mlInputFile = InitialLearningConstants.MLInputFile;
                        filePath = string.Format(mlInputFile, path, timeStamp, CultureInfo.CurrentCulture);
                        filePathDesc = string.Format(DescWordFile, path, timeStamp, CultureInfo.CurrentCulture);
                        filePathRes = string.Format(ResWordFile, path, timeStamp, CultureInfo.CurrentCulture);
                        Utility.DataTableToCSV(dsResult.Tables[0], filePath);
                        Utility.DataTableToCSV(dsResult.Tables[1], filePathDesc);
                        if (PresenceOfOptionalField)
                        {
                            Utility.DataTableToCSV(dsResult.Tables[2], filePathRes);
                        }

                    }

                    if (dsResult.Tables[3].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Locale = CultureInfo.InvariantCulture;
                        dt = dsResult.Tables[3];
                        string bUName;
                        string accountName;
                        string projectName;
                        string esaProjectID;
                        string initialLearningId;

                        bUName = (Convert.ToString(dt.Rows[0]["BUName"] != DBNull.Value ? Convert.ToString(dt.
                            Rows[0]["BUName"], CultureInfo.CurrentCulture) : string.Empty));
                        accountName = (Convert.ToString(dt.Rows[0]["AccountName"] != DBNull.Value ? Convert.
                            ToString(dt.Rows[0]["AccountName"], CultureInfo.CurrentCulture) : string.Empty));
                        projectName = (Convert.ToString(dt.Rows[0]["ProjectName"] != DBNull.Value ? Convert.
                            ToString(dt.Rows[0]["ProjectName"],CultureInfo.CurrentCulture) : string.Empty));
                        esaProjectID = (Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"] != DBNull.
                            Value ? Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"], CultureInfo.CurrentCulture) : string.Empty));
                        initialLearningId = (Convert.ToString(dt.Rows[0]["InitialLearningId"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[0]["InitialLearningId"], CultureInfo.CurrentCulture) : string.Empty));


                        if (PresenceOfOptionalField)

                        {
                            MLJobDetails = Utility.SumbmitClassificationMapReduceJob(esaProjectID,
                                initialLearningId, filePath, filePathDesc, filePathRes,
                                UserID, ProjectID, SupportTypeID);
                        }
                        else
                        {

                            MLJobDetails = Utility.SumbmitClassificationMapReduceJob(esaProjectID,
                                initialLearningId, filePath, filePathDesc, string.Empty,
                                UserID, ProjectID, SupportTypeID);

                        }

                        jobSuccess = InsertMLJobId(ProjectID, initialLearningId, MLJobDetails.MLJobId,
                            MLJobDetails.FileName, MLJobDetails.DataPath, "ML", "Sent", UserID, SupportTypeID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobSuccess;
        }
        /// <summary>
        /// Get Sampling Details
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>

        public List<DebtSamplingModel> GetDebtSamplingData(int projectID)
        {

            List<DebtSamplingModel> debtSamplingList = new List<DebtSamplingModel>();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", projectID);

            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();

            DataTable dt = (new DBHelper()).GetTableFromSP("ML_GetDebtSamplingDetails", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DebtSamplingModel ticketDetails = new DebtSamplingModel();
                    ticketDetails.TicketId = ((dt.Rows[i]["TicketID"] != DBNull.Value) ?
                        dt.Rows[i]["TicketID"].
                        ToString() : string.Empty);

                    if (string.Compare(encryptionEnabled, Enabled) == 0)
                    {

                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TicketDescription"], CultureInfo.CurrentCulture)))
                        {
                            string bytesDecrypted = aesMod.DecryptStringBytes((string)dt.Rows[i]
                                ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                            string decTicketDesc = bytesDecrypted;
                            ticketDetails.TicketDescription = decTicketDesc;
                        }

                        else
                        {
                            ticketDetails.TicketDescription = Convert.ToString(dt.Rows[i]
                                ["TicketDescription"], CultureInfo.CurrentCulture);
                        }
                    }
                    else
                    {
                        ticketDetails.TicketDescription = Convert.ToString(dt.Rows[i]
                            ["TicketDescription"]);
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
                                        ((string)dt.Rows[i]["AdditionalText"],
                                        AseKeyDetail.AesKeyConstVal);
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
                    ticketDetails.ApplicationName = dt.Rows[i]["ApplicationName"].ToString();
                    ticketDetails.ApplicationId = (Convert.ToInt32(dt.Rows[i]["ApplicationID"] != DBNull.Value ?
                        Convert.ToInt32(dt.Rows[i]["ApplicationID"]) : 0));
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
            return debtSamplingList;
        }

        /// <summary>
        /// Update Noise Elimination Flag
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <returns></returns>

        public List<GetMLDetails> UpdateNoiseEliminationFlag(Int64 ProjectId, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {

            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            string saveILNoiseUpd = InitialLearningConstants.SaveInitialLearningChoicesNoiseUpd;
            List<GetMLDetails> lstGetSamplingSent = new List<GetMLDetails>();
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@UserID", AssociateID);
                prms[2] = new SqlParameter("@StartDate", StartDate);
                prms[3] = new SqlParameter("@EndDate", EndDate);
                prms[4] = new SqlParameter("@choice", saveILNoiseUpd);
                prms[5] = new SqlParameter("@OptFieldsForProj", OptionalFieldProj);

                DataSet ds;
                if (SupportTypeID == 1)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("ML_SaveInitialLearningState", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveInitialLearningStateInfra]", prms, ConnectionString);
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lstGetSamplingSent.Add(new GetMLDetails
                    {
                        StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString(),
                        EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString(),
                        MLStatus = ds.Tables[0].Rows[0]["MLStatus"].ToString(),
                        NoiseEliminationSent = ds.Tables[0].Rows[0]["NoiseEliminationSent"].ToString(),
                        IsDartTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDartTicket"].ToString()),
                        IsSDTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSDTicket"].ToString()),
                        SamplingInProgressStatus = ds.Tables[0].Rows[0]["SamplingInProgressStatus"].ToString(),
                        SamplingSentOrReceivedStatus = ds.Tables[0].Rows[0]["SamplingSentOrReceivedStatus"].ToString(),
                        IsRegenerated = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRegenerated"].ToString()),
                        RegStartDate = ds.Tables[0].Rows[0]["FromDate"].ToString(),
                        RegEndDate = ds.Tables[0].Rows[0]["Todate"].ToString()
                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstGetSamplingSent;
        }
        /// <summary>
        /// Update Sampling Flag
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <returns></returns>
        public List<GetMLDetails> UpdateSamplingFlag(int ProjectId, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, Int32 SupportTypeID)
        {
            string saveILSamplingUpd = InitialLearningConstants.SaveInitialLearningChoicesSampleUpd;
            List<GetMLDetails> lstGetSamplingSent = new List<GetMLDetails>();
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@UserID", AssociateID);
                prms[2] = new SqlParameter("@StartDate", StartDate);
                prms[3] = new SqlParameter("@EndDate", EndDate);
                prms[4] = new SqlParameter("@choice", saveILSamplingUpd);
                prms[5] = new SqlParameter("@OptFieldsForProj", OptionalFieldProj);

                if (SupportTypeID == 1)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("ML_SaveInitialLearningState", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveInitialLearningStateInfra]", prms, ConnectionString);
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    lstGetSamplingSent.Add(new GetMLDetails
                    {
                        StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString(),
                        EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString(),
                        MLStatus = ds.Tables[0].Rows[0]["MLStatus"].ToString(),
                        IsDartTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDartTicket"].ToString()),
                        IsSDTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSDTicket"].ToString()),
                        SamplingInProgressStatus = ds.Tables[0].Rows[0]["SamplingInProgressStatus"].ToString(),
                        SamplingSentOrReceivedStatus = ds.Tables[0].Rows[0]["SamplingSentOrReceivedStatus"].ToString(),
                        IsRegenerated = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRegenerated"].ToString()),
                        RegStartDate = ds.Tables[0].Rows[0]["FromDate"].ToString(),
                        RegEndDate = ds.Tables[0].Rows[0]["Todate"].ToString()

                    });



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstGetSamplingSent;
        }
        /// <summary>
        /// Insert ML Jobid 
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="initialLearningId">initialLearning Id</param>
        /// <param name="MLJobId">ML JobId </param>
        /// <param name="FileName">File Name</param>
        /// <param name="DataPath">Data Path</param>
        /// <param name="JobType">Job Type</param>
        /// <param name="JobMessage">Job Message</param>
        /// <param name="UserID">Employee id</param>
        /// <returns></returns>
        public string InsertMLJobId(int ProjectID, string initialLearningId, string MLJobId, string FileName,
            string DataPath, string JobType, string JobMessage, string UserID, int SupportId)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[8];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@initialLearningId", initialLearningId);
                prms[2] = new SqlParameter("@MLJobId", MLJobId);
                prms[3] = new SqlParameter("@JobType", JobType);
                prms[4] = new SqlParameter("@JobMessage", JobMessage);
                prms[5] = new SqlParameter("@UserID", UserID);
                prms[6] = new SqlParameter("@FileName", FileName);
                prms[7] = new SqlParameter("@DataPath", DataPath);

                if (SupportId == 1)
                {
                    (new DBHelper()).ExecuteNonQuery("ML_InsertMLJobId", prms, ConnectionString);
                }
                else
                {
                    (new DBHelper()).ExecuteNonQuery("[AVL].[InfraInsertMLJobId]", prms, ConnectionString);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Flag;
        }
        /// <summary>
        /// To get hivepath  based on mljobid
        /// </summary>
        /// <param name="MLJobId"></param>
        /// <returns></returns>
        public FilePath GetHivePathnameByJobId(string MLJobId)
        {
            FilePath objFilePath = new FilePath();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@MLJobId", MLJobId);

            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_GetFileNameByJobId", prms, ConnectionString);
                if (ds != null)
                {
                    objFilePath.OutputPath = ds.Tables[0].Rows[0]["HiveDataPath"].ToString();
                    objFilePath.ErrorPath = ds.Tables[0].Rows[0]["FileErrorPath"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objFilePath;
        }

        /// <summary>
        /// To get infra hivepath based on mljobid
        /// </summary>
        /// <param name="MLJobId"></param>
        /// <returns></returns>
        public FilePath GetInfraHivePathnameByJobId(string MLJobId)
        {
            FilePath objFilePath = new FilePath();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@MLJobId", MLJobId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[MLInfraGetFileNameByJobId]", prms, ConnectionString);
                if (ds != null)
                {
                    objFilePath.OutputPath = ds.Tables[0].Rows[0]["HiveDataPath"].ToString();
                    objFilePath.ErrorPath = ds.Tables[0].Rows[0]["FileErrorPath"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objFilePath;
        }
        /// <summary>
        /// get hive path details for  noise elimination
        /// </summary>
        /// <param name="NoiseEliminationJobId">Noise Elimination JobId</param>
        /// <returns></returns>
        public FilePathNoiseEl GetHivePathnameByJobIdForNoiseEl(string NoiseEliminationJobId)
        {
            FilePathNoiseEl objFilePath = new FilePathNoiseEl();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@NoiseEliminationJobId", NoiseEliminationJobId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_GetNoiseOutPutpath", prms, ConnectionString);
                if (ds != null)
                {
                    objFilePath.OutputPathDesc = ds.Tables[0].Rows[0]["HiveDataPathDesc"].ToString();
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["PresenceOfOptField"].ToString()))
                    {

                        objFilePath.OutputPathOpt = ds.Tables[0].Rows[0]["HiveDataPathOpt"].ToString();
                    }

                    objFilePath.PresenceOfOptField = Convert.ToBoolean(ds.Tables[0].Rows[0]["PresenceOfOptField"].
                        ToString());
                    objFilePath.ErrorPath = ds.Tables[0].Rows[0]["FileErrorPath"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objFilePath;
        }
        /// <summary>
        /// update error status
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="errorData">error Data</param>
        /// <param name="JobStatus">Job Status</param>
        /// <returns></returns>
        public string UpdateErrorStatus(int ProjectID, string MLJobId, string errorData, int JobStatus,
            int SupportTypeID)
        {
            SqlParameter[] prms = new SqlParameter[5];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@MLJobId", MLJobId);
            prms[2] = new SqlParameter("@errorData", errorData);
            prms[3] = new SqlParameter("@JobStatus", JobStatus);
            prms[4] = new SqlParameter("@SupportTypeID", SupportTypeID);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_UpdateErrorStatus", prms, ConnectionString);
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
        /// To save recieved ML Pattern
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateMLPatternFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            string hiveFilepath = string.Empty;
            FilePath objFilePath = new FilePath();
            DataTable dtMLdata = new DataTable();
            dtMLdata.Locale = CultureInfo.InvariantCulture;
            objFilePath = GetHivePathnameByJobId(MLJobId);
            hiveFilepath = objFilePath.OutputPath;
            string downloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];


            hiveFilepath = Utility.DownloadFile(hiveFilepath, downloadPath, 1);


            dtMLdata = Utility.GetDataTabletFromCSVFile(hiveFilepath);
            //Code to get the file data from csv to table
            List<DebtMLPatternSaveModel> lstDebtMLPatternSaveModel = new List<DebtMLPatternSaveModel>();
            try
            {
                if (dtMLdata != null)
                {
                    for (int i = 0; i < dtMLdata.Rows.Count; i++)
                    {
                        DebtMLPatternSaveModel objDebtMLPatternSaveModel = new DebtMLPatternSaveModel();
                        objDebtMLPatternSaveModel.ApplicationName = (Convert.ToString(dtMLdata.
                            Rows[i]["ApplicationName"] != DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]
                            ["ApplicationName"]) : string.Empty));
                        objDebtMLPatternSaveModel.ApplicationType = (Convert.ToString(dtMLdata.
                            Rows[i]["ApplicationType"] != DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]
                            ["ApplicationType"]) : string.Empty));
                        objDebtMLPatternSaveModel.Technology = (Convert.ToString(dtMLdata.Rows[i]["Technology"]
                            != DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]["Technology"]) : string.Empty));
                        if (dtMLdata.Columns.Contains("DebtClassification"))
                        {
                            objDebtMLPatternSaveModel.DebtClassification = (Convert.ToString(dtMLdata.
                                Rows[i]["DebtClassification"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["DebtClassification"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("AvoidableFlag"))
                        {
                            objDebtMLPatternSaveModel.AvoidableFlag = (Convert.ToString(dtMLdata.
                                Rows[i]["AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["AvoidableFlag"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("ResidualDebt"))
                        {
                            objDebtMLPatternSaveModel.ResidualDebt = (Convert.ToString(dtMLdata.
                                Rows[i]["ResidualDebt"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["ResidualDebt"]) : string.Empty));
                        }

                        objDebtMLPatternSaveModel.CauseCode = (Convert.ToString(dtMLdata.Rows[i]["CauseCode"] !=
                            DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]["CauseCode"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResolutionCode = (Convert.ToString(dtMLdata.
                            Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ResolutionCode"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Base_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.DescSubWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Sub_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResBaseWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Res_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Res_Base_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResSubWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Res_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Res_Sub_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLDebtClassification = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_DebtClassification"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_DebtClassification"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLAvoidableFlag = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_AvoidableFlag"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLResidualDebt = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_ResidualDebt"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_ResidualDebt"]) : string.Empty));
                        if (dtMLdata.Columns.Contains("CauseCode"))
                        {
                            objDebtMLPatternSaveModel.MLCauseCode = (Convert.ToString(dtMLdata.
                                Rows[i]["CauseCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["CauseCode"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("ResolutionCode"))
                        {
                            objDebtMLPatternSaveModel.MLResolutionCode = (Convert.ToString(dtMLdata.
                                Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["ResolutionCode"]) : string.Empty));
                        }
                        objDebtMLPatternSaveModel.MLRuleAccuracy = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_RuleLevelAccuracy"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_RuleLevelAccuracy"]) : string.Empty));
                        objDebtMLPatternSaveModel.SMEApproval = (Convert.ToString(dtMLdata.
                            Rows[i]["SME_Approval_Flag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["SME_Approval_Flag"]) : string.Empty));
                        objDebtMLPatternSaveModel.TicketOccurence = (Convert.ToInt32(dtMLdata.
                            Rows[i]["TicketOccurence"] != DBNull.Value ? Convert.ToInt32(dtMLdata.
                            Rows[i]["TicketOccurence"]) : 0));
                        if (dtMLdata.Columns.Contains("Classified_by"))
                        {
                            objDebtMLPatternSaveModel.Classifiedby = (Convert.ToString(dtMLdata.
                                Rows[i]["Classified_by"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["Classified_by"]) : string.Empty));
                        }
                        lstDebtMLPatternSaveModel.Add(objDebtMLPatternSaveModel);

                    }
                }

                var objCollection = from i in lstDebtMLPatternSaveModel
                                    select new
                                    {
                                        ApplicationName = i.ApplicationName,
                                        TowerName = i.ApplicationType,
                                        ApplicationType = "",
                                        Technology = i.Technology,
                                        DebtClassification = i.DebtClassification,
                                        AvoidableFlag = i.AvoidableFlag,
                                        ResidualDebt = i.ResidualDebt,
                                        CauseCode = i.CauseCode,
                                        ResolutionCode = i.ResolutionCode,
                                        MLDebtClassification = i.MLDebtClassification,
                                        MLAvoidableFlag = i.MLAvoidableFlag,
                                        MLResidualDebt = i.MLResidualDebt,
                                        MLCauseCode = i.MLCauseCode,
                                        MLWorkPattern = i.MLWorkPattern,
                                        DescSubPattern = i.DescSubWorkPattern,
                                        ResBasePattern = i.ResBaseWorkPattern,
                                        ResSubPattern = i.ResSubWorkPattern,
                                        MLRuleAccuracy = i.MLRuleAccuracy,
                                        SMEApproval = i.SMEApproval,
                                        MLResolutionCode = i.MLResolutionCode,
                                        TicketOccurence = i.TicketOccurence,
                                        Classifiedby = i.Classifiedby
                                    };


                //updating the list to main pattern table
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@TVP_lstDebtPattern", objCollection.ToList().ToDT());
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = InitialLearningConstants.TypeDebtMLPattern;
                prms[2] = new SqlParameter("@UserID", UserID);
                DataSet dt = (new DBHelper()).GetDatasetFromSP("ML_SaveMLPatternFromAlgorithm", prms, ConnectionString);
                AddTask(ProjectId, UserID, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return hiveFilepath;
        }



        /// <summary>
        /// To save recieved ML Pattern
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateMLPatternFromCSVInfra(int ProjectId, string MLJobId, string UserID)
        {
            string hiveFilepath = string.Empty;
            FilePath objFilePath = new FilePath();
            DataTable dtMLdata = new DataTable();
            dtMLdata.Locale = CultureInfo.InvariantCulture;
            objFilePath = GetInfraHivePathnameByJobId(MLJobId);
            hiveFilepath = objFilePath.OutputPath;
            string downloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];

            //siva
            hiveFilepath = Utility.DownloadFile(hiveFilepath, downloadPath, 2);


            dtMLdata = Utility.GetDataTabletFromCSVFile(hiveFilepath);
            //Code to get the file data from csv to table
            List<DebtMLPatternSaveModel> lstDebtMLPatternSaveModel = new List<DebtMLPatternSaveModel>();
            try
            {
                if (dtMLdata != null)
                {
                    for (int i = 0; i < dtMLdata.Rows.Count; i++)
                    {
                        DebtMLPatternSaveModel objDebtMLPatternSaveModel = new DebtMLPatternSaveModel();
                        objDebtMLPatternSaveModel.TowerName = (Convert.ToString(dtMLdata.
                            Rows[i]["Tower"] != DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]
                            ["Tower"]) : string.Empty));

                        if (dtMLdata.Columns.Contains("DebtClassification"))
                        {
                            objDebtMLPatternSaveModel.DebtClassification = (Convert.ToString(dtMLdata.
                                Rows[i]["DebtClassification"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["DebtClassification"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("AvoidableFlag"))
                        {
                            objDebtMLPatternSaveModel.AvoidableFlag = (Convert.ToString(dtMLdata.
                                Rows[i]["AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["AvoidableFlag"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("ResidualDebt"))
                        {
                            objDebtMLPatternSaveModel.ResidualDebt = (Convert.ToString(dtMLdata.
                                Rows[i]["ResidualDebt"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["ResidualDebt"]) : string.Empty));
                        }

                        objDebtMLPatternSaveModel.CauseCode = (Convert.ToString(dtMLdata.Rows[i]["CauseCode"] !=
                            DBNull.Value ? Convert.ToString(dtMLdata.Rows[i]["CauseCode"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResolutionCode = (Convert.ToString(dtMLdata.
                            Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ResolutionCode"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Base_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.DescSubWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Desc_Sub_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResBaseWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Res_Base_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Res_Base_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.ResSubWorkPattern = (Convert.ToString(dtMLdata.
                            Rows[i]["Res_Sub_WorkPattern"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["Res_Sub_WorkPattern"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLDebtClassification = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_DebtClassification"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_DebtClassification"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLAvoidableFlag = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_AvoidableFlag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_AvoidableFlag"]) : string.Empty));
                        objDebtMLPatternSaveModel.MLResidualDebt = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_ResidualDebt"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_ResidualDebt"]) : string.Empty));
                        if (dtMLdata.Columns.Contains("CauseCode"))
                        {
                            objDebtMLPatternSaveModel.MLCauseCode = (Convert.ToString(dtMLdata.
                                Rows[i]["CauseCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["CauseCode"]) : string.Empty));
                        }
                        if (dtMLdata.Columns.Contains("ResolutionCode"))
                        {
                            objDebtMLPatternSaveModel.MLResolutionCode = (Convert.ToString(dtMLdata.
                                Rows[i]["ResolutionCode"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["ResolutionCode"]) : string.Empty));
                        }
                        objDebtMLPatternSaveModel.MLRuleAccuracy = (Convert.ToString(dtMLdata.
                            Rows[i]["ML_RuleLevelAccuracy"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["ML_RuleLevelAccuracy"]) : string.Empty));
                        objDebtMLPatternSaveModel.SMEApproval = (Convert.ToString(dtMLdata.
                            Rows[i]["SME_Approval_Flag"] != DBNull.Value ? Convert.ToString(dtMLdata.
                            Rows[i]["SME_Approval_Flag"]) : string.Empty));
                        objDebtMLPatternSaveModel.TicketOccurence = (Convert.ToInt32(dtMLdata.
                            Rows[i]["TicketOccurence"] != DBNull.Value ? Convert.ToInt32(dtMLdata.
                            Rows[i]["TicketOccurence"]) : 0));
                        if (dtMLdata.Columns.Contains("Classified_by"))
                        {
                            objDebtMLPatternSaveModel.Classifiedby = (Convert.ToString(dtMLdata.
                                Rows[i]["Classified_by"] != DBNull.Value ? Convert.ToString(dtMLdata.
                                Rows[i]["Classified_by"]) : string.Empty));
                        }
                        lstDebtMLPatternSaveModel.Add(objDebtMLPatternSaveModel);

                    }
                }
                var objCollection = from i in lstDebtMLPatternSaveModel
                                    select new
                                    {
                                        ApplicationName = i.ApplicationName,
                                        TowerName = i.TowerName,
                                        ApplicationType = i.ApplicationType,
                                        Technology = i.Technology,
                                        DebtClassification = i.DebtClassification,
                                        AvoidableFlag = i.AvoidableFlag,
                                        ResidualDebt = i.ResidualDebt,
                                        CauseCode = i.CauseCode,
                                        ResolutionCode = i.ResolutionCode,
                                        MLDebtClassification = i.MLDebtClassification,
                                        MLAvoidableFlag = i.MLAvoidableFlag,
                                        MLResidualDebt = i.MLResidualDebt,
                                        MLCauseCode = i.MLCauseCode,
                                        MLWorkPattern = i.MLWorkPattern,
                                        DescSubPattern = i.DescSubWorkPattern,
                                        ResBasePattern = i.ResBaseWorkPattern,
                                        ResSubPattern = i.ResSubWorkPattern,
                                        MLRuleAccuracy = i.MLRuleAccuracy,
                                        SMEApproval = i.SMEApproval,
                                        MLResolutionCode = i.MLResolutionCode,
                                        TicketOccurence = i.TicketOccurence,
                                        Classifiedby = i.Classifiedby

                                    };
                //updating the list to main pattern table
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", ProjectId);
                prms[1] = new SqlParameter("@TVP_lstDebtPatternInfra", objCollection.ToList().ToDT());
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = InitialLearningConstants.TypeDebtMLPattern;
                prms[2] = new SqlParameter("@UserID", UserID);
                DataSet dt = (new DBHelper()).GetDatasetFromSP("ML_SaveMLPatternFromAlgorithmInfra", prms, ConnectionString);
                AddTask(ProjectId, UserID, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return hiveFilepath;
        }

        /// <summary>
        /// To get ML Details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public List<GetMLDetails> GetMLDetailsOnLoad(Int32 ProjectID, int SupportTypeID)
        {
            List<GetMLDetails> lstGetMLDetailsOnLoad = new List<GetMLDetails>();
            string mlDetOnloadAfterProcess = InitialLearningConstants.MLDetailsChoiceAfterProcess;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@Step", mlDetOnloadAfterProcess);
            DataSet ds;
            try
            {
                if (SupportTypeID == 2)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_GetMLDetailsOnLoadInfra]", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("ML_GetMLDetailsOnLoad", prms, ConnectionString);
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lstGetMLDetailsOnLoad.Add(new GetMLDetails
                    {
                        StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString(),
                        EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString(),
                        MLStatus = ds.Tables[0].Rows[0]["MLStatus"].ToString(),
                        MLSentBy = ds.Tables[0].Rows[0]["MLSentBy"].ToString(),
                        MLSentDate = ds.Tables[0].Rows[0]["MLSentDate"].ToString(),
                        SamplingSentBy = ds.Tables[0].Rows[0]["SamplingSentBy"].ToString(),
                        SamplingSentDate = ds.Tables[0].Rows[0]["SamplingSentDate"].ToString(),
                        NoiseSentDate = ds.Tables[0].Rows[0]["NoiseSentDate"].ToString(),
                        DataValidationDate = ds.Tables[0].Rows[0]["DataValidationDate"].ToString(),
                        MlReceiveddate = ds.Tables[0].Rows[0]["MlReceiveddate"].ToString(),
                        IsDartTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDartTicket"].ToString()),
                        NoiseEliminationSent = ds.Tables[0].Rows[0]["IsNoiseEliminationSentorReceived"].ToString(),
                        IsSDTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSDTicket"].ToString()),
                        SamplingInProgressStatus = ds.Tables[0].Rows[0]["SamplingInProgressStatus"].ToString(),
                        SamplingSentOrReceivedStatus = ds.Tables[0].Rows[0]["SamplingSentOrReceivedStatus"].
                        ToString(),
                        MLSignoff = ds.Tables[2].Rows.Count > 0 ? (Convert.ToInt32(ds.Tables[2].
                        Rows[0]["IsMLSignOff"] != DBNull.Value ? ds.Tables[2].Rows[0]["IsMLSignOff"] : 0)) : 0,
                        ErrorMessage = ds.Tables[0].Rows[0]["ErrorMessage"].ToString(),
                        OptionalFieldId = Convert.ToInt16(ds.Tables[0].Rows[0]["OptionalFieldID"]),
                        AutoclassificationDatestring = ds.Tables[2].Rows.Count > 0 ? (Convert.
                        ToString(ds.Tables[2].Rows[0]["AutoclassificationDate"] != DBNull.Value ? Convert.
                        ToString(ds.Tables[2].Rows[0]["AutoclassificationDate"]) : string.Empty)) : string.Empty,
                        IsAutoClassified = ds.Tables[2].Rows.Count > 0 ? (Convert.ToString(ds.Tables[2].
                        Rows[0]["IsAutoClassified"] != DBNull.Value ? Convert.ToString(ds.Tables[2].
                        Rows[0]["IsAutoClassified"]) : "N")) : "N",
                        IsRegMLsignOff = ds.Tables[2].Rows.Count > 0 ? (Convert.ToInt32(ds.Tables[2].
                        Rows[0]["IsRegMLsignOff"] != DBNull.Value ? ds.Tables[2].Rows[0]["IsRegMLsignOff"] : 0)) : 0,
                        IsRegenerated = ds.Tables[0].Rows.Count > 0 ? (Convert.ToBoolean(ds.Tables[0].
                        Rows[0]["IsRegenerated"] != DBNull.Value ? ds.Tables[0].
                        Rows[0]["IsRegenerated"] : false)) : false,
                        RegStartDate = ds.Tables[0].Rows[0]["RegStartDate"].ToString(),
                        RegEndDate = ds.Tables[0].Rows[0]["RegEndDate"].ToString(),
                        RegenerateCount = ds.Tables[2].Rows.Count > 0 ? (Convert.ToInt32(ds.Tables[2].
                        Rows[0]["RegenerateCount"] != DBNull.Value ? ds.Tables[2].Rows[0]["RegenerateCount"] : 0)) : 0
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
        /// To check the project is Auto Classified
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>

        public string CheckProjectAutoClassified(int ProjectID)
        {
            string isAutoClassified = string.Empty;
            string mlDetOnloadAfterProcess = InitialLearningConstants.MLDetailsChoiceAfterProcess;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@Step", mlDetOnloadAfterProcess);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_GetMLDetailsOnLoad", prms, ConnectionString);
                if (ds != null && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {

                    isAutoClassified = ds.Tables[2].Rows.Count > 0 ? (Convert.ToString(ds.Tables[2].
                        Rows[0]["IsAutoClassified"] != DBNull.Value ? Convert.ToString(ds.Tables[2].
                        Rows[0]["IsAutoClassified"]) : "N")) : "N";

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAutoClassified;
        }
        /// <summary>
        /// To check if noise file is generated
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="NoiseEliminationJobId">Noise Elimination Job Id </param>
        /// <returns></returns>
        public NoiseElimination CheckIfNoiseOutputFileGenerated(int ProjectID, string NoiseEliminationJobId,
            int SupportTypeID)
        {
            NoiseElimination objNoiseElimination = new NoiseElimination();

            DataTable dtnoiseticket = new DataTable();
            dtnoiseticket.Locale = CultureInfo.InvariantCulture;
            DataTable dtnoiseresolution = new DataTable();
            dtnoiseresolution.Locale = CultureInfo.InvariantCulture;
            List<NoiseEliminationTicketDescription> lstticketdescription =
                new List<NoiseEliminationTicketDescription>();
            List<NoiseEliminationResolutionRemarks> lstResolution =
                new List<NoiseEliminationResolutionRemarks>();
            string output = string.Empty;
            string HiveFilepathdesc = string.Empty;
            string HiveFilepathopt = string.Empty;
            string ErrorPath = string.Empty;
            bool PresenceOfOptField;
            FilePathNoiseEl objFilePath = new FilePathNoiseEl();
            DataTable dtErrordata = new DataTable();
            dtErrordata.Locale = CultureInfo.InvariantCulture;
            objFilePath = GetHivePathnameByJobIdForNoiseEl(NoiseEliminationJobId);
            HiveFilepathdesc = objFilePath.OutputPathDesc;
            HiveFilepathopt = objFilePath.OutputPathOpt;
            PresenceOfOptField = objFilePath.PresenceOfOptField;
            ErrorPath = objFilePath.ErrorPath;
            string errorData = string.Empty;
            string DownloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];

            //Checking for Error Output File
            string FileErrorOutputPresent = Utility.CheckIfFileExists(ErrorPath, DownloadPath, SupportTypeID);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtErrordata = Utility.GetDataTabletFromCSVFile(FileErrorOutputPresent);
                try
                {
                    if (dtErrordata != null)
                    {
                        errorData = Convert.ToString(dtErrordata.Rows[0][1]);
                        if (string.Compare(errorData, FileStatSuccess) != 0)
                        {
                            output = UpdateErrorStatus(ProjectID, NoiseEliminationJobId, errorData, 2,
                                SupportTypeID);
                        }
                        else if (string.Compare(errorData, FileStatSuccess) == 0)
                        {
                            output = UpdateErrorStatus(ProjectID, NoiseEliminationJobId, errorData, 1,
                                SupportTypeID);
                        }
                        else
                        {
                            //mandatory else
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //Checking for Output File
            string FileOutputPresentdesc = Utility.CheckIfFileExists(HiveFilepathdesc, DownloadPath, SupportTypeID);
            string FileOutputPresentOpt = Utility.CheckIfFileExists(HiveFilepathopt, DownloadPath, SupportTypeID);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtnoiseticket = Utility.GetDataTabletFromCSVFile(FileOutputPresentdesc);
                if (PresenceOfOptField)
                {

                    dtnoiseresolution = Utility.GetDataTabletFromCSVFile(FileOutputPresentOpt);
                }


            }


            if (dtnoiseticket != null)
            {
                for (int i = 0; i < dtnoiseticket.Rows.Count; i++)
                {
                    NoiseEliminationTicketDescription objnoise = new NoiseEliminationTicketDescription();
                    objnoise.Keywords = (Convert.ToString(dtnoiseticket.Rows[i]["Word"] != DBNull.Value ?
                        Convert.ToString(dtnoiseticket.Rows[i]["Word"]) : string.Empty));
                    objnoise.Frequency = (Convert.ToString(dtnoiseticket.Rows[i]["Frequency"] != DBNull.Value ?
                        Convert.ToString(dtnoiseticket.Rows[i]["Frequency"]) : string.Empty));
                    objnoise.IsActive = true;
                    lstticketdescription.Add(objnoise);
                }
                objNoiseElimination.LstNoiseTicketDescription = lstticketdescription;
            }
            if (dtnoiseresolution != null)
            {
                for (int i = 0; i < dtnoiseresolution.Rows.Count; i++)
                {
                    NoiseEliminationResolutionRemarks objnoiseResolution = new NoiseEliminationResolutionRemarks();
                    objnoiseResolution.Keywords = (Convert.ToString(dtnoiseresolution.Rows[i]["Word"] !=
                        DBNull.Value ? Convert.ToString(dtnoiseresolution.Rows[i]["Word"]) : string.Empty));
                    objnoiseResolution.Frequency = (Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"] !=
                        DBNull.Value ? Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"]) : string.Empty));
                    objnoiseResolution.IsActive = true;
                    lstResolution.Add(objnoiseResolution);
                }
                objNoiseElimination.LstNoiseResolution = lstResolution;
            }
            return objNoiseElimination;
        }
        /// <summary>
        /// To get the ml job id
        /// </summary>
        /// <param name="Projectid">Project id</param>
        /// <returns></returns>
        private string getMLJobID(Int32 Projectid)
        {
            SqlParameter[] prms = new SqlParameter[1];
            string MLJobID = string.Empty;
            prms[0] = new SqlParameter("@ProjectID", Projectid);
            try
            {

                DataTable dt = (new DBHelper()).GetTableFromSP("ML_GetMLJob", prms, ConnectionString);
                if (dt != null)
                {
                    MLJobID = dt.Rows[0]["JobIdFromML"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return MLJobID;

        }

        /// <summary>
        /// For downloading ML Base details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string UpdatePatternId(Int32 ProjectID, int SupportTypeId)

        {
            DataTable dtBaseData = new DataTable();
            dtBaseData.Locale = CultureInfo.InvariantCulture;
            string filepath = String.Empty;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            try
            {
                if (SupportTypeId == 1)

                {

                    dtBaseData = (new DBHelper()).GetTableFromSP("[dbo].[ML_GetBaseDetails]", prms, ConnectionString);
                }
                else
                {
                    dtBaseData = (new DBHelper()).GetTableFromSP("[dbo].[ML_GetBaseDetailsInfra]", prms, ConnectionString);
                }
                if (dtBaseData.Rows.Count == 0)
                {

                    var ProcessingDetails = GetProcessingRequiredOnLoad(ProjectID, SupportTypeId);
                    InitialLearningRepository debtRepository = new InitialLearningRepository();
                    string HiveFilepath = string.Empty;
                    string ErrorPath = string.Empty;
                    string mlOutputFile = InitialLearningConstants.MLOutputFile;
                    string mlBaseFile = InitialLearningConstants.MLBaseFile;
                    FilePath objFilePath = new FilePath();
                    if (SupportTypeId == 1)
                    {
                        objFilePath = GetHivePathnameByJobId(ProcessingDetails.MLJobId);
                    }
                    else
                    {
                        //siva
                        objFilePath = GetInfraHivePathnameByJobId(ProcessingDetails.MLJobId);
                    }
                    HiveFilepath = objFilePath.OutputPath;
                    ErrorPath = objFilePath.ErrorPath;
                    //Anna
                    string DownloadPath = new ApplicationConstants().DownloadExcelTemp;

                    //Checking for Error Output File
                    //siva
                    string FileErrorOutputPresent = Utility.CheckIfFileExists(ErrorPath, DownloadPath,
                        SupportTypeId);
                    dtBaseData = SaveMLBase(HiveFilepath, mlBaseFile, mlOutputFile, FileErrorOutputPresent,
                        DownloadPath, SupportTypeId, ProjectID);
                }
                filepath = ExportToExcelForMLBaseDetails(dtBaseData, SupportTypeId);
            }

            catch (Exception ex)
            {
                throw ex;
            }


            return filepath;
        }

        /// <summary>
        /// To check whether ML File is generated
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="JobType">Job Type</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string CheckIfMLFileGenerated(int ProjectID, string MLJobId, string JobType, string UserID,
            int SupportTypeID)
        {
            InitialLearningRepository debtRepository = new InitialLearningRepository();
            string Output = string.Empty;
            string HiveFilepath = string.Empty;
            string ErrorPath = string.Empty;
            string mlOutputFile = InitialLearningConstants.MLOutputFile;
            string mlBaseFile = InitialLearningConstants.MLBaseFile;
            FilePath objFilePath = new FilePath();
            DataTable dtMLdata = new DataTable();
            dtMLdata.Locale = CultureInfo.InvariantCulture;
            DataTable dtErrordata = new DataTable();
            dtErrordata.Locale = CultureInfo.InvariantCulture;
            if (SupportTypeID == 1)
            {
                objFilePath = GetHivePathnameByJobId(MLJobId);
            }
            else
            {
                //siva
                objFilePath = GetInfraHivePathnameByJobId(MLJobId);
            }
            HiveFilepath = objFilePath.OutputPath;
            ErrorPath = objFilePath.ErrorPath;
            string errorData = string.Empty;
            string DownloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];

            //Checking for Error Output File
            //siva
            string FileErrorOutputPresent = Utility.CheckIfFileExists(ErrorPath, DownloadPath, SupportTypeID);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtErrordata = Utility.GetDataTabletFromCSVFile(FileErrorOutputPresent);
                try
                {
                    if (dtErrordata != null)
                    {
                        errorData = Convert.ToString(dtErrordata.Rows[0][1]);
                        if (string.Compare(errorData, FileStatSuccess) != 0)
                        {
                            Output = UpdateErrorStatus(ProjectID, MLJobId, errorData, 2, SupportTypeID);
                        }
                        else if (string.Compare(errorData, FileStatSuccess) == 0)
                        {
                            Output = UpdateErrorStatus(ProjectID, MLJobId, errorData, 1, SupportTypeID);
                        }
                        else
                        {
                            //mandatory else
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            //Checking for Output File
            //siva
            string FileOutputPresent = Utility.CheckIfFileExists(HiveFilepath, DownloadPath, SupportTypeID);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtMLdata = Utility.GetDataTabletFromCSVFile(FileOutputPresent);
                try
                {
                    if (string.Compare(JobType, InitialLearningConstants.JobTypeML) == 0)
                    {
                        if (SupportTypeID == 1)
                        {
                            UpdateMLPatternFromCSV(ProjectID, MLJobId, UserID);
                        }
                        else
                        {
                            UpdateMLPatternFromCSVInfra(ProjectID, MLJobId, UserID);
                        }
                    }
                    else
                    {
                        if (SupportTypeID == 1)
                        {
                            UpdateSampledTicketsFromCSV(ProjectID, MLJobId, UserID);
                        }
                        else
                        {
                            debtRepository.UpdateInfraSampledTicketsFromCSV(ProjectID, MLJobId, UserID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            DataTable dtbasedatanew = SaveMLBase(HiveFilepath, mlBaseFile, mlOutputFile, FileErrorOutputPresent,
                DownloadPath, SupportTypeID, ProjectID);

            return HiveFilepath;
        }
        public DataTable SaveMLBase(string HiveFilepath, string mlBaseFile, string mlOutputFile,
            string FileErrorOutputPresent, string DownloadPath, int SupportTypeID, Int64 ProjectID)
        {
            string FileMLBase = HiveFilepath.Replace(mlOutputFile, mlBaseFile);
            DataTable dtBaseData = new DataTable();
            dtBaseData.Locale = CultureInfo.InvariantCulture;
            string FileOutputPresentMLBase = Utility.CheckIfFileExists(FileMLBase, DownloadPath, SupportTypeID);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {

                try
                {
                    dtBaseData = Utility.GetDataTabletFromCSVFile(FileOutputPresentMLBase);
                    if (dtBaseData.Columns.Contains("DepartmentName"))
                    {
                        dtBaseData.Columns.Remove("DepartmentName");
                    }
                    if (dtBaseData.Columns.Contains("AccountName"))
                    {

                        dtBaseData.Columns.Remove("AccountName");
                    }
                    if (dtBaseData.Columns.Contains("EsaProjectID"))
                    {

                        dtBaseData.Columns.Remove("EsaProjectID");
                    }
                    if (dtBaseData.Columns.Contains("TicketDescription"))
                    {

                        dtBaseData.Columns.Remove("TicketDescription");
                    }
                    if (dtBaseData.Columns.Contains("desc_mod_text"))
                    {

                        dtBaseData.Columns.Remove("desc_mod_text");
                    }
                    if (dtBaseData.Columns.Contains("res_mod_text"))
                    {

                        dtBaseData.Columns.Remove("res_mod_text");
                    }
                    if (dtBaseData.Columns.Contains("ApplicationTypename"))
                    {

                        dtBaseData.Columns.Remove("ApplicationTypename");
                    }
                    if (dtBaseData.Columns.Contains("TechnologyName"))
                    {

                        dtBaseData.Columns.Remove("TechnologyName");
                    }
                    if (dtBaseData.Columns.Contains("desc_trigram"))
                    {

                        dtBaseData.Columns.Remove("desc_trigram");
                    }
                    if (dtBaseData.Columns.Contains("desc_unigram"))
                    {

                        dtBaseData.Columns.Remove("desc_unigram");
                    }
                    if (dtBaseData.Columns.Contains("desc_bigram"))
                    {

                        dtBaseData.Columns.Remove("desc_bigram");
                    }
                    if (dtBaseData.Columns.Contains("ML_WorkPattern_desc"))
                    {

                        dtBaseData.Columns.Remove("ML_WorkPattern_desc");
                    }
                    if (dtBaseData.Columns.Contains("res_unigram"))
                    {

                        dtBaseData.Columns.Remove("res_unigram");
                    }
                    if (dtBaseData.Columns.Contains("res_bigram"))
                    {

                        dtBaseData.Columns.Remove("res_bigram");
                    }
                    if (dtBaseData.Columns.Contains("res_trigram"))
                    {

                        dtBaseData.Columns.Remove("res_trigram");
                    }
                    if (dtBaseData.Columns.Contains("ML_WorkPattern_res"))
                    {

                        dtBaseData.Columns.Remove("ML_WorkPattern_res");
                    }
                    if (dtBaseData.Columns.Contains("AdditionalText"))
                    {

                        dtBaseData.Columns.Remove("AdditionalText");
                    }
                    if (dtBaseData.Columns.Contains("TowerHierarchy1"))
                    {

                        dtBaseData.Columns.Remove("TowerHierarchy1");
                    }
                    if (dtBaseData.Columns.Contains("TowerHierarchy2"))
                    {

                        dtBaseData.Columns.Remove("TowerHierarchy2");
                    }


                    foreach (DataRow row in dtBaseData.Rows)
                    {
                        foreach (DataColumn column in dtBaseData.Columns)
                        {
                            if (string.Compare(column.ColumnName, "Desc_Base_WorkPattern") == 0 ||
                                string.Compare(column.ColumnName, "Desc_Sub_WorkPattern") == 0 ||
                                string.Compare(column.ColumnName, "Res_Base_WorkPattern") == 0 ||
                                string.Compare(column.ColumnName, "Res_Sub_WorkPattern") == 0)
                            {
                                if (string.Compare(row[column].ToString(), "0") == 0)
                                {
                                    row.SetField(column, string.Empty);
                                }
                            }
                        }
                    }

                    dtBaseData.Columns[7].ColumnName = "TicketDescriptionPattern";
                    dtBaseData.Columns[8].ColumnName = "TicketDescriptionSubPattern";
                    dtBaseData.Columns[9].ColumnName = "OptionalFieldpattern";
                    dtBaseData.Columns[10].ColumnName = "OptionalFieldSubPattern";
                    dtBaseData.AcceptChanges();

                    SqlParameter[] prms = new SqlParameter[2];
                    prms[0] = new SqlParameter("@ProjectID", ProjectID);
                    prms[1] = new SqlParameter("@MLBaseDetails", dtBaseData);
                    if (SupportTypeID == 1)
                    {
                        (new DBHelper()).ExecuteNonQuery("[dbo].[ML_SaveBaseDetails]", prms, ConnectionString);
                    }
                    else
                    {
                        (new DBHelper()).ExecuteNonQuery("[dbo].[ML_SaveInfraBaseDetails]", prms, ConnectionString);
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dtBaseData;
        }
        /// <summary>
        /// to update sampled tickets
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateSampledTicketsFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            string HiveFilepath = string.Empty;
            FilePath objFilePath = new FilePath();
            DataTable dtSampleddata = new DataTable();
            dtSampleddata.Locale = CultureInfo.InvariantCulture;
            objFilePath = GetHivePathnameByJobId(MLJobId);
            HiveFilepath = objFilePath.OutputPath;
            string downloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];


            HiveFilepath = Utility.DownloadFile(HiveFilepath, downloadPath, 1);


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
                            != DBNull.Value ? Convert.ToString(dtSampleddata.Rows[i]["ESAProjectID"]) : string.Empty));
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

                        DebtMLPatternSaveModel.ApplicationName = (Convert.ToString(dtSampleddata.
                            Rows[i]["ApplicationName"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ApplicationName"]) : string.Empty));
                        DebtMLPatternSaveModel.ApplicationType = (Convert.ToString(dtSampleddata.
                            Rows[i]["ApplicationTypename"] != DBNull.Value ? Convert.ToString(dtSampleddata.
                            Rows[i]["ApplicationTypename"]) : string.Empty));

                        DebtMLPatternSaveModel.Technology = (Convert.ToString(dtSampleddata.Rows[i]["TechnologyName"]
                            != DBNull.Value ? Convert.ToString(dtSampleddata.Rows[i]["TechnologyName"]) :
                            string.Empty));
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
                                        ApplicationName = i.ApplicationName,
                                        ApplicationType = i.ApplicationType,
                                        Technology = i.Technology,
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
                prms[1].TypeName = InitialLearningConstants.TypeDebtSampledTickets;
                prms[2] = new SqlParameter("@UserID", UserID);
                DataTable dt = (new DBHelper()).GetTableFromSP("ML_SaveSampledTicketsFromAlgorithm", prms, ConnectionString);
                AddTask(ProjectId, UserID, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return HiveFilepath;
        }
        /// <summary>
        /// AddTask
        /// </summary>
        /// <param name="projectID"></param>
        /// /// <param name="employeeID"></param>
        /// /// <param name="Option"></param>
        /// <returns></returns>
        public void AddTask(long projectID, string employeeID, int Option)
        {
            try
            {
                SqlParameter[] prmsTasks = new SqlParameter[3];
                prmsTasks[0] = new SqlParameter("@ProjectID", projectID);
                prmsTasks[1] = new SqlParameter("@UserID", employeeID);
                prmsTasks[2] = new SqlParameter("@Option", Option);
                DataTable prjTasks = new DataTable();
                prjTasks.Locale = CultureInfo.InvariantCulture;
                prjTasks = (new DBHelper()).GetTableFromSP("AVL.MyTask_InitialLearning_ContinuousLearning",
                    prmsTasks, ConnectionString);
                if (prjTasks != null && prjTasks.Rows.Count > 0)
                {
                    List<TaskDetailsList> taskList = new List<TaskDetailsList>();
                    taskList = prjTasks.AsEnumerable().Select(dailyrow => new TaskDetailsList
                    {
                        UserId = dailyrow["UserID"] == null ? string.Empty : dailyrow["UserID"].ToString(),
                        AccountId = Convert.ToInt64(dailyrow["AccountID"]),
                        Application = dailyrow["Application"].ToString(),
                        CreatedBy = employeeID,
                        CreatedTime = DateTimeOffset.Now.DateTime.ToString(),
                        DueDate = string.Empty,
                        ExpiryAfterRead = dailyrow["ExpiryAfterRead"].ToString(),
                        ExpiryDate = string.Empty,
                        Read = dailyrow["Read"].ToString(),
                        RefreshedTime = DateTimeOffset.Now.DateTime.ToString(),
                        Status = dailyrow["Status"].ToString(),
                        TaskDetails = dailyrow["TaskDetails"].ToString(),
                        TaskId = Convert.ToInt16(dailyrow["TaskID"]),
                        TaskName = dailyrow["TaskName"].ToString(),
                        TaskType = dailyrow["TaskType"].ToString(),
                        URL = dailyrow["TaskURL"].ToString(),
                        ModifiedBy = employeeID,
                        ModifiedTime = DateTimeOffset.Now.DateTime.ToString()


                    }).ToList();

                    MyTaskRepository taskRep = new MyTaskRepository();
                    JArray taskdetailsObj = JArray.FromObject(taskList);

                }

            }
            catch (Exception ex)
            {
               // MyTaskRepository taskRep = new MyTaskRepository();
               // taskRep.ErrorLOG(string.Concat(ex.Message, " Stack trace: ", ex.StackTrace),
                //    "Reached My task Initial Learning Sampling", Convert.ToInt64(projectID));
                
            throw ex;
            }
        }

        /// <summary>
        /// get processin details of project on load
        /// </summary>
        /// <param name="ProjectID">Project id</param>
        /// <returns></returns>
        public MLSamplingProcess GetProcessingRequiredOnLoad(int ProjectID, int SupportTypeID)
        {
            MLSamplingProcess objMLSamplingProcess = new MLSamplingProcess();
            string mlDetOnLoadBeforeProcess = InitialLearningConstants.MLDetailsChoiceBeforeProcess;
            DataSet ds;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@Step", mlDetOnLoadBeforeProcess);
            try
            {
                if (SupportTypeID == 2)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_GetMLDetailsOnLoadInfra]", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("ML_GetMLDetailsOnLoad", prms, ConnectionString);
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    objMLSamplingProcess.IsMLProcessingRequired = Convert.ToString(ds.Tables[0].
                        Rows[0]["IsMLProcessingRequired"]);
                    objMLSamplingProcess.IsSamplingProcessingRequired = Convert.ToString(ds.Tables[0].
                        Rows[0]["IsSamplingProcessingRequired"]);
                    objMLSamplingProcess.IsMLSent = Convert.ToString(ds.Tables[0].Rows[0]["IsMLSent"]);
                    objMLSamplingProcess.IsSamplingSent = Convert.ToString(ds.Tables[0].Rows[0]["IsSamplingSent"]);
                    objMLSamplingProcess.MLJobId = Convert.ToString(ds.Tables[0].Rows[0]["MLJobId"]);
                    objMLSamplingProcess.NoiseEliminationJobId = ds.Tables[0].Rows[0]["NoiseEliminationJobId"].
                        ToString();
                    objMLSamplingProcess.IsNoiseEliminationSent = ds.Tables[0].Rows[0]["NoiseEliminationSent"].
                        ToString();
                    objMLSamplingProcess.SamplingJobId = Convert.ToString(ds.Tables[0].Rows[0]["SamplingJobId"]);
                    objMLSamplingProcess.ErrorMessage = Convert.ToString(ds.Tables[0].Rows[0]["ErrorMessage"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objMLSamplingProcess;
        }

        //update ml Flag

        /// <summary>
        ///  update ml Flag
        /// </summary>
        /// <param name="StartDate">Min value of the date</param>
        /// <param name="EndDate">End date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <param name="ProjectID">ID OF THE PROJECT</param>
        /// <param name="UserID">ID of the Employee</param>
        /// <returns></returns>
        public List<GetMLDetails> MLUpdateInitialLearningStateDetails(int ProjectID, string UserID,
            DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            string saveILMLUpd = InitialLearningConstants.SaveInitialLearningChoicesMLUpd;
            List<GetMLDetails> lstGetMLSent = new List<GetMLDetails>();
            DataSet ds;
            try
            {
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@UserID", UserID);
                prms[2] = new SqlParameter("@StartDate", StartDate);
                prms[3] = new SqlParameter("@EndDate", EndDate);
                prms[4] = new SqlParameter("@choice", saveILMLUpd);
                prms[5] = new SqlParameter("@OptFieldsForProj", OptionalFieldProj);

                if (SupportTypeID == 1)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("ML_SaveInitialLearningState", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveInitialLearningStateInfra]", prms, ConnectionString);
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lstGetMLSent.Add(new GetMLDetails
                    {
                        StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString(),
                        EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString(),
                        MLStatus = ds.Tables[0].Rows[0]["MLStatus"].ToString(),
                        IsDartTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDartTicket"].ToString()),
                        IsSDTicket = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSDTicket"].ToString()),
                        SamplingInProgressStatus = ds.Tables[0].Rows[0]["SamplingInProgressStatus"].ToString(),
                        SamplingSentOrReceivedStatus = ds.Tables[0].Rows[0]["SamplingSentOrReceivedStatus"].
                        ToString(),
                        IsRegenerated = ds.Tables[0].Rows.Count > 0 ? (Convert.ToBoolean(ds.Tables[0].
                        Rows[0]["IsRegenerated"] != DBNull.Value ? ds.Tables[0].Rows[0]["IsRegenerated"] :
                        false)) : false,
                        RegStartDate = ds.Tables[0].Rows[0]["FromDate"].ToString(),
                        RegEndDate = ds.Tables[0].Rows[0]["Todate"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstGetMLSent;
        }
        /// <summary>
        /// ML Base details download
        /// </summary>
        /// <param name="dtBaseData">base data for ml pattern</param>
        /// <returns></returns>
        public string ExportToExcelForMLBaseDetails(DataTable dtBaseData, int SupportTypeId)
        {

            string newpth = string.Empty;
            string orgpath = string.Empty;
            string mlBaseFileName = InitialLearningConstants.MlBaseExcelName;
            try
            {
                string Sourcepath = string.Empty;
                if (SupportTypeId == 1)
                {
                    Sourcepath = new ApplicationConstants().ExcelMLTemplateBasePath;
                }
                else
                {
                    Sourcepath = new ApplicationConstants().ExcelMLInfraTemplateBasePath;
                }
                string strExtension = Path.GetExtension(Sourcepath);
                string foldername = new ApplicationConstants().DownloadExcelTemp;
                string orginalfile = string.Concat(Path.GetDirectoryName(Sourcepath), "\\");
                string filename = Path.GetFileName(Sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(Sourcepath);

                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss");
                var ext = strExtension;
                orgpath = string.Concat(foldername, string.Concat(fleInfo.Name.Split('.')[0], "_", strTimeStamp, ext));
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile);
                if (directoryInfo.Exists)
                {
                    newpth = string.Concat(directoryInfo, string.Concat(fleInfo.Name.Split('.')[0], "_",
                        strTimeStamp, ext));
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


                new OpenXMLOperations().ToExcelSheetByDataTable(dtBaseData, null, newpth, mlBaseFileName, null, 2);


            }

            catch (Exception ex)
            {
                InsertCheck(ex.Message);
                throw ex;
            }
            return orgpath;
        }

        /// <summary>
        /// download ticket details 
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string ExportToExcelForML(int ProjectID, int SupportTypeID)
        {

            string newpth = string.Empty;

            string orgpath = string.Empty;
            try
            {
                string Sourcepath = string.Empty;
                Sourcepath = new ApplicationConstants().ExcelMLTemplatePath;
                string strExtension = Path.GetExtension(Sourcepath);
                string foldername = new ApplicationConstants().DownloadExcelTemp;
                string orginalfile = string.Concat(Path.GetDirectoryName(Sourcepath), "\\");
                string filename = Path.GetFileName(Sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(Sourcepath);
                DataTable MLDatatable = new DataTable();
                MLDatatable.Locale = CultureInfo.InvariantCulture;
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss");
                var ext = strExtension;
                orgpath = string.Concat(foldername, string.Concat(fleInfo.Name.Split('.')[0], "_"
                    , strTimeStamp, ext));
                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile);
                if (directoryInfo.Exists)
                {
                    newpth = string.Concat(directoryInfo, string.Concat(fleInfo.Name.Split('.')[0], "_",
                        strTimeStamp, ext));
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

                if (SupportTypeID == 1)
                {
                    MLDatatable = (new DBHelper()).GetTableFromSP("ML_GetTicketDetailsForDownloadExcel", prms, ConnectionString);
                }
                else
                {
                    MLDatatable = (new DBHelper()).GetTableFromSP("[AVL].[ML_GetTicketDetailsForDownloadExcelInfra]"
                        , prms, ConnectionString);
                }
                new OpenXMLOperations().ToExcelSheetByDataTable(MLDatatable, null, newpth, MlDataExtraction, null, 2);


            }

            catch (Exception ex)
            {
                InsertCheck(ex.Message);
                throw ex;
            }
            return orgpath;
        }

        /// <summary>
        /// This Method is used to InsertCheck
        /// </summary>
        /// <param name="ErrorMessage"></param>
        public void InsertCheck(string ErrorMessage)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@Reg", ErrorMessage);
            (new DBHelper()).ExecuteNonQuery("InsertCheck", prms, ConnectionString);
        }

        /// <summary>
        /// This Method is used to ExcelTodataset
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DataSet ExcelToDataSet(string filename)
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            string sWSName = String.Empty;
            try
            {

                sWSName = MlDataExtraction;
                ds.Tables.Add(new OpenXMLOperations().ToDataTableBySheetName(filename, sWSName, 0, 1).Copy());
            }


            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        /// <summary>
        /// This Method is used to GetListTicketMasterDebtFieldsFrom
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<TicketMasterModel> GetListTicketMasterDebtFieldsFrom(string UserID, Int64 ProjectID)
        {
            List<TicketMasterModel> lstTicketMasterModel = new List<TicketMasterModel>();
            string auditChoiceFrom = InitialLearningConstants.AuditChoiceFrom;
            DataTable dtTicketDetails = new DataTable();
            dtTicketDetails.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[3];

                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@UserID", UserID);
                prms[2] = new SqlParameter("@Type", auditChoiceFrom);
                dtTicketDetails = (new DBHelper()).GetTableFromSP("Audit_GetListTicketMasterDetailsForInitialLearning",
                    prms, ConnectionString);
                if (dtTicketDetails != null)
                {
                    if (dtTicketDetails.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTicketDetails.Rows.Count; i++)
                        {

                            TicketMasterModel objTicketMasterModel = new TicketMasterModel();
                            objTicketMasterModel.ProjectId = Convert.ToString(dtTicketDetails.Rows[i]["ProjectID"]);
                            objTicketMasterModel.TicketId = Convert.ToString(dtTicketDetails.Rows[i]["TicketID"]);
                            objTicketMasterModel.DebtClassification = Convert.ToString(dtTicketDetails.
                                Rows[i]["DebtClassification"]);
                            objTicketMasterModel.AvoidableFlag = Convert.ToString(dtTicketDetails.
                                Rows[i]["AvoidableFlag"]);
                            objTicketMasterModel.ResidualDebt = Convert.ToString(dtTicketDetails.
                                Rows[i]["ResidualDebt"]);
                            objTicketMasterModel.CauseCode = Convert.ToString(dtTicketDetails.Rows[i]["CauseCode"]);
                            objTicketMasterModel.ResolutionCode = Convert.ToString(dtTicketDetails.
                                Rows[i]["ResolutionCode"]);
                            lstTicketMasterModel.Add(objTicketMasterModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTicketMasterModel;
        }

        /// <summary>
        ///  To save for upload
        /// </summary>
        ///<param name="filename" name of the file></param>
        /// <param name="filePath" path of the file></param>
        /// <param name="AssociateID" ID of Employee></param>
        /// <param name="ProjectID" ID of the project></param>
        ///<param name="OptfieldProj" Optional field for the project></param>
        /// <returns></returns>

        public string ProcessFileUpload(string filename, string filePath, string AssociateID,
            Int32 ProjectID, Int16 OptfieldProj, int SupportTypeID)
        {
            List<TicketMasterModel> lstTicketMasterFrom = new List<TicketMasterModel>();
            lstTicketMasterFrom = GetListTicketMasterDebtFieldsFrom(AssociateID, ProjectID);
            DataSet dsResult;
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();

            string CriteriaMet = string.Empty;
            string strPath = string.Concat(filePath, filename);
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@UserID", AssociateID);
            List<GetDebtFieldsForUploadExcel> objDebtUploadExcelTicketsCollection =
                new List<GetDebtFieldsForUploadExcel>();

            DataSet ds = ExcelToDataSet(strPath);
            if (ds.Tables[0] != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {
                    EffortTrackingRepository effortTracking = new EffortTrackingRepository();
                    bool isMultilingualEnabled = effortTracking.CheckIfMultilingualEnabled
                    (Convert.ToString(ProjectID), AssociateID);
                    List<TicketDescriptionSummary> ticketDescriptionSummary = new List<TicketDescriptionSummary>();
                    string isTicketDescriptionUpdated = "0";
                    string isTicketSummaryUpdated = "0";
                    if (isMultilingualEnabled)
                    {
                        ticketDescriptionSummary = effortTracking.GetTicketValues(Convert.ToString(ds.Tables[0].
                            Rows[i]["Ticket ID"]),
                            Convert.ToString(ProjectID), AssociateID, 1);
                        isTicketDescriptionUpdated = !string.IsNullOrEmpty(ticketDescriptionSummary[0].
                            TicketDescription) && !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]
                                                    ["Ticket Description"]).Trim()) ? ticketDescriptionSummary[0].
                            TicketDescription.Trim().Equals(
                                ds.Tables[0].Rows[i]["Ticket Description"].ToString().Trim()) == true ? "0" : "1" :
                            !string.IsNullOrEmpty(Convert.ToString(
                                ds.Tables[0].Rows[i]["Ticket Description"]).Trim()) ? "1" : "0";

                        if (OptfieldProj == 2)
                        {
                            isTicketSummaryUpdated = !string.IsNullOrEmpty(ticketDescriptionSummary[0].TicketSummary)
                            && !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Ticket Summary"]).Trim()) ?
                            ticketDescriptionSummary[0].TicketSummary.Trim().Equals(ds.Tables[0].Rows[i]
                                ["Ticket Summary"].ToString().Trim()) == true ? "0" : "1" :
                                !string.IsNullOrEmpty(
                                    Convert.ToString(ds.Tables[0].Rows[i]["Ticket Summary"]).Trim()) ? "1" : "0";
                        }
                    }
                    if (string.Compare(encryptionEnabled, Enabled) == 0)
                    {
                        objDebtUploadExcelTicketsCollection.Add(new GetDebtFieldsForUploadExcel
                        {

                            TicketId = Convert.ToString(ds.Tables[0].Rows[i]["Ticket ID"]),
                            TicketDescription = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Ticket Description"].
                            ToString()) ? string.Empty : ((ds.Tables[0].Rows[i]["Ticket Description"].
                            ToString() == "***") ? "***" : (Convert.ToBase64String(aesMod.
                            EncryptStringAsBytes(ds.Tables[0].Rows[i]["Ticket Description"].
                            ToString(), AseKeyDetail.AesKeyConstVal)))),
                            ApplicationName = SupportTypeID == 1 && SupportTypeID != 0 ?
                            ds.Tables[0].Rows[i]["Application Name"].ToString() : "",
                            DebtClassificationValue = ds.Tables[0].Rows[i]["Debt Classification"].ToString(),
                            AvoidableFlagValue = ds.Tables[0].Rows[i]["Avoidable Flag"].ToString(),
                            ResidualDebtValue = ds.Tables[0].Rows[i]["Residual Debt"].ToString(),
                            CauseCodeValue = ds.Tables[0].Rows[i]["Cause Code"].ToString(),
                            ResolutionCodeValue = ds.Tables[0].Rows[i]["Resolution Code"].ToString(),

                            OptionalFieldProj = OptfieldProj == 2 ? string.IsNullOrEmpty(ds.Tables[0].
                            Rows[i]["Ticket Summary"].ToString()) ? string.Empty : ((ds.Tables[0].
                            Rows[i]["Ticket Summary"].ToString() == "***") ? "***" : (Convert.
                            ToBase64String(aesMod.EncryptStringAsBytes(ds.Tables[0].Rows[i]["Ticket Summary"].
                            ToString(), AseKeyDetail.AesKeyConstVal)))) : OptfieldProj == 1 ? ds.Tables[0].
                            Rows[i]["Resolution Remarks"].ToString() : OptfieldProj == 3 ?
                            ds.Tables[0].Rows[i]["Comments"].ToString() : string.Empty,
                            IsTicketDescriptionUpdated = isTicketDescriptionUpdated,
                            IsTicketSummaryUpdated = isTicketSummaryUpdated

                        });
                    }
                    else
                    {
                        objDebtUploadExcelTicketsCollection.Add(new GetDebtFieldsForUploadExcel
                        {

                            TicketId = Convert.ToString(ds.Tables[0].Rows[i]["Ticket ID"]),
                            TicketDescription = Convert.ToString(ds.Tables[0].Rows[i]["Ticket Description"]),
                            ApplicationName = SupportTypeID == 1 && SupportTypeID != 0 ?
                            ds.Tables[0].Rows[i]["Application Name"].ToString() : "",
                            DebtClassificationValue = ds.Tables[0].Rows[i]["Debt Classification"].ToString(),
                            AvoidableFlagValue = ds.Tables[0].Rows[i]["Avoidable Flag"].ToString(),
                            ResidualDebtValue = ds.Tables[0].Rows[i]["Residual Debt"].ToString(),
                            CauseCodeValue = ds.Tables[0].Rows[i]["Cause Code"].ToString(),
                            ResolutionCodeValue = ds.Tables[0].Rows[i]["Resolution Code "].ToString(),

                            OptionalFieldProj = OptfieldProj == 2 ? ds.Tables[0].Rows[i]["Ticket Summary"].
                            ToString() : OptfieldProj == 1 ? ds.Tables[0].Rows[i]["Resolution Remarks"].ToString() :
                            OptfieldProj == 3 ?
                            ds.Tables[0].Rows[i]["Comments"].ToString() : string.Empty,
                            IsTicketDescriptionUpdated = isTicketDescriptionUpdated,
                            IsTicketSummaryUpdated = isTicketSummaryUpdated
                        });
                    }

                }

            }

            var objCollection = from i in objDebtUploadExcelTicketsCollection
                                select new
                                {
                                    TicketId = i.TicketId,
                                    TicketDescription = i.TicketDescription,
                                    ApplicationName = i.ApplicationName,
                                    DebtClassificationValue = i.DebtClassificationValue,
                                    AvoidableFlagValue = i.AvoidableFlagValue,
                                    CauseCodeValue = i.CauseCodeValue,
                                    ResolutionCodeValue = i.ResolutionCodeValue,
                                    ResidualDebtValue = i.ResidualDebtValue,
                                    OptionalFieldProj = i.OptionalFieldProj,
                                    IsTicketDescriptionUpdated = i.IsTicketDescriptionUpdated,
                                    IsTicketSummaryUpdated = i.IsTicketSummaryUpdated
                                };

            prms[2] = new SqlParameter("@TVP_lstDebtExcelUploadTickets", objCollection.ToList().ToDT());
            prms[2].SqlDbType = SqlDbType.Structured;
            prms[2].TypeName = InitialLearningConstants.TypeDebtUploadTickets;
            prms[3] = new SqlParameter("@OptionalFieldId", OptfieldProj);
            try
            {
                if (SupportTypeID == 1)
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_SaveExcelUploadDetailsML", prms, ConnectionString);
                }
                else
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveExcelUploadDetailsInfra]", prms, ConnectionString);
                }
                if (dsResult != null)
                {
                    CriteriaMet = dsResult.Tables[0].Rows[0]["CriteriaMet"].ToString();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CriteriaMet;
        }
        /// <summary>
        ///  To get  the master fields for debt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>

        public List<TicketMasterModel> GetListTicketMasterDebtFieldsTo(string UserID, Int64 ProjectID)
        {
            List<TicketMasterModel> lstTicketMasterModel = new List<TicketMasterModel>();
            string auditChoiceTo = InitialLearningConstants.AuditChoiceTo;
            DataTable dtTicketDetails = new DataTable();
            dtTicketDetails.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[3];

                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@UserID", UserID);
                prms[2] = new SqlParameter("@Type", auditChoiceTo);
                dtTicketDetails = (new DBHelper()).GetTableFromSP("Audit_GetListTicketMasterDetailsForInitialLearning",
                    prms, ConnectionString);
                if (dtTicketDetails != null)
                {
                    if (dtTicketDetails.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTicketDetails.Rows.Count; i++)
                        {

                            TicketMasterModel objTicketMasterModel = new TicketMasterModel();
                            objTicketMasterModel.ProjectId = Convert.ToString(dtTicketDetails.Rows[i]["ProjectID"]);
                            objTicketMasterModel.TicketId = Convert.ToString(dtTicketDetails.Rows[i]["TicketID"]);
                            objTicketMasterModel.DebtClassification = Convert.ToString(dtTicketDetails.
                                Rows[i]["DebtClassification"]);
                            objTicketMasterModel.AvoidableFlag = Convert.ToString(dtTicketDetails.
                                Rows[i]["AvoidableFlag"]);
                            objTicketMasterModel.ResidualDebt = Convert.ToString(dtTicketDetails.
                                Rows[i]["ResidualDebt"]);
                            objTicketMasterModel.CauseCode = Convert.ToString(dtTicketDetails.Rows[i]["CauseCode"]);
                            objTicketMasterModel.ResolutionCode = Convert.ToString(dtTicketDetails.
                                Rows[i]["ResolutionCode"]);
                            lstTicketMasterModel.Add(objTicketMasterModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTicketMasterModel;
        }

        /// <summary>
        ///  To Signoff the ML patterns
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SignOffDebtPatternValidation(int ProjectID, DateTime Datefrom, string UserID, int SupportTypeID)
        {


            try
            {
                {
                    SqlParameter[] prms = new SqlParameter[4];
                    prms[0] = new SqlParameter("@ProjectID", ProjectID);
                    prms[1] = new SqlParameter("@Datefrom", Datefrom);
                    prms[2] = new SqlParameter("@UserID", UserID);
                    prms[3] = new SqlParameter("@SupportTypeID", SupportTypeID);
                    (new DBHelper()).ExecuteNonQuery("ML_DebtPatternValidationSignOff", prms, ConnectionString);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Flag;

        }

        string jobSuccess;
        /// <summary>
        ///  To get the list of optional field based on projectid
        /// </summary>      
        /// <param name="ProjectID"></param>
        /// <returns></returns>


        public List<ProjOptionalFields> GetOptionalFieldsOnLoad(int projectID)
        {
            List<ProjOptionalFields> lstOptFields = new List<ProjOptionalFields>();

            DataTable dsResult = new DataTable();
            dsResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);

                dsResult = (new DBHelper()).GetTableFromSP("AVL.ML_GetOptionalFieldsListOnLoad", prms, ConnectionString);

                if (dsResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dsResult.Rows.Count; i++)
                    {
                        lstOptFields.Add(new ProjOptionalFields
                        {
                            Id = Convert.ToInt16(dsResult.
                            Rows[i]["ID"].ToString()),
                            OptFieldName = dsResult.Rows[i]["OptionalFields"].
                            ToString()
                        });

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstOptFields;

        }

        /// <summary>
        ///  To get the details for sampling
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SamplingDatSetBindingForCSVCreation(MLDetailsParam MLUserdetails)
        {

            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", MLUserdetails.ProjectId);

                if (MLUserdetails.SupportTypeId == 1)
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_FinalTicketDetailsforcallingML", prms, ConnectionString);
                }
                else
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("[AVL].[InfraFinalTicketDetailsforcallingML]", prms, ConnectionString);
                }
                int optionalfielid;
                bool PresenceOfOptional;

                if (dsResult.Tables[3].Rows.Count > 0)
                {
                    optionalfielid = Convert.ToInt16(dsResult.Tables[3].Rows[0]["OptionalFieldID"]);
                    PresenceOfOptional = Convert.ToBoolean(dsResult.Tables[3].Rows[0]["PresenceOfOptional"]);
                }
                else
                {
                    optionalfielid = 0;
                    PresenceOfOptional = false;
                }
                string EncryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                AESEncryption aesMod = new AESEncryption();

                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    if (string.Compare(EncryptionEnabled, Enabled) == 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dsResult.Tables[0].
                            Rows[i]["TicketDescription"])))
                        {

                            string bytesDecrypted = aesMod.DecryptStringBytes((string)dsResult.Tables[0].Rows[i]
                                ["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                            string decTicketDesc = bytesDecrypted;
                            dsResult.Tables[0].Rows[i]["TicketDescription"] = decTicketDesc;
                        }
                        else
                        {
                            dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.ToString(dsResult.
                                Tables[0].Rows[i]["TicketDescription"]);
                        }
                    }
                    else
                    {
                        dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.ToString(dsResult.
                            Tables[0].Rows[i]["TicketDescription"]);
                    }

                    dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.ToString(dsResult.
                        Tables[0].Rows[i]["TicketDescription"]);
                    if (PresenceOfOptional)
                    {
                        if (optionalfielid == 2)
                        {
                            if (string.Compare(EncryptionEnabled, Enabled) == 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dsResult.Tables[0].
                                    Rows[i]["AdditionalText"])))
                                {

                                    string bytesDecrypted = aesMod.DecryptStringBytes((string)dsResult.Tables[0].
                                        Rows[i]["AdditionalText"], AseKeyDetail.AesKeyConstVal);
                                    string decTicketDesc = bytesDecrypted;
                                    dsResult.Tables[0].Rows[i]["AdditionalText"] = decTicketDesc;
                                }
                                else
                                {
                                    dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                                        Tables[0].Rows[i]["AdditionalText"]);
                                }
                            }
                            else
                            {
                                dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                                    Tables[0].Rows[i]["AdditionalText"]);
                            }

                        }

                        dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                            Tables[0].Rows[i]["AdditionalText"]);
                    }
                }
                jobSuccess = SubmitSamplingJob(dsResult, MLUserdetails.ProjectId, MLUserdetails.AssociateId, MLUserdetails.SupportTypeId);

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return jobSuccess;

        }
        /// <summary>
        ///  To submit for sampling
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>

        private string SubmitSamplingJob(DataSet dsResult, Int64 ProjectID, string UserID, int SupportTypeId)
        {
            string jobSuccess = string.Empty;
            string filePath = string.Empty;
            string filePathDesc = string.Empty;
            string filePathRes = string.Empty;
            bool PresenceOfOptionalField = false;
            try
            {
                if (dsResult.Tables[3].Rows.Count > 0)
                {
                    PresenceOfOptionalField = Convert.ToBoolean(dsResult.Tables[3].Rows[0]["PresenceOfOptional"].
                        ToString());
                }
                GetMLJobDetails MLJobDetails = new GetMLJobDetails();

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0 && dsResult.Tables[1] != null)
                    {
                        string timeStamp = string.Format("{0:yyyy-MM-dd-HHmmss}", DateTimeOffset.Now.DateTime);
                        string path = new ApplicationConstants().DownloadExcelTemp;
                        string samplingInputFile = InitialLearningConstants.SamplingInputFile;
                        filePath = string.Format(samplingInputFile, path, timeStamp);
                        filePathDesc = string.Format(DescWordFile, path, timeStamp);
                        filePathRes = string.Format(ResWordFile, path, timeStamp);
                        Utility.DataTableToCSV(dsResult.Tables[0], filePath);
                        Utility.DataTableToCSV(dsResult.Tables[1], filePathDesc);
                        if (PresenceOfOptionalField)
                        {
                            Utility.DataTableToCSV(dsResult.Tables[2], filePathRes);
                        }
                    }
                    if (dsResult.Tables[3].Rows.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Locale = CultureInfo.InvariantCulture;
                        dt = dsResult.Tables[3];
                        string bUName;
                        string accountName;
                        string projectName;
                        string initialLearningId;
                        string esaProjectID;

                        bUName = (Convert.ToString(dt.Rows[0]["BUName"] != DBNull.Value ? Convert.ToString(dt.
                            Rows[0]["BUName"]) : string.Empty));
                        accountName = (Convert.ToString(dt.Rows[0]["AccountName"] != DBNull.Value ? Convert.
                            ToString(dt.Rows[0]["AccountName"]) : string.Empty));
                        projectName = (Convert.ToString(dt.Rows[0]["ProjectName"] != DBNull.Value ? Convert.
                            ToString(dt.Rows[0]["ProjectName"]) : string.Empty));
                        esaProjectID = (Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"] != DBNull.Value
                            ? Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"]) : string.Empty));
                        initialLearningId = (Convert.ToString(dt.Rows[0]["InitialLearningId"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[0]["InitialLearningId"]) : string.Empty));

                        if (PresenceOfOptionalField)
                        {
                            MLJobDetails = Utility.SumbmitSamplingMapReduceJob(esaProjectID, initialLearningId,
                                filePath, filePathDesc, filePathRes,
                                UserID, Convert.ToInt64(ProjectID), SupportTypeId);
                        }
                        else
                        {
                            MLJobDetails = Utility.SumbmitSamplingMapReduceJob(esaProjectID, initialLearningId,
                                filePath, filePathDesc, string.Empty,
                                UserID, Convert.ToInt64(ProjectID), SupportTypeId);
                        }
                        jobSuccess = InsertMLJobId(Convert.ToInt32(ProjectID), initialLearningId, MLJobDetails.MLJobId,
                            MLJobDetails.FileName, MLJobDetails.DataPath, "Sampling", "Sent", UserID, SupportTypeId);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLOG(ex.Message, "SubmitSamplingJob",
                    Convert.ToInt32(UserID));
            }
            return jobSuccess;
        }

        /// <summary>
        ///  To submit for noise elimination
        /// </summary>
        /// <param name="dsResult">dataset of valid ticket details</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="UserID">Cog ID</param>
        /// <returns></returns>
        public string SubmitNoiseEliminationJob(DataSet dsResult, int ProjectID, string UserID, int SupportTypeID)
        {
            Utility.ErrorLOGInfra("5 .1. in  SubmitNoiseEliminationJob start ",
                  "UserID is" + UserID, 0);

            string jobSuccess = string.Empty;
            string filePath = string.Empty;
            GetMLJobDetails MLJobDetails = new GetMLJobDetails();

            Utility.ErrorLOGInfra("0. in SubmitNoiseEliminationJob  SupportTypeID =" + SupportTypeID,
               "SubmitNoiseEliminationJob", 0);
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        string timeStamp = string.Format("{0:yyyy-MM-dd-HHmmss}", DateTimeOffset.Now.DateTime);
                        string path = new ApplicationConstants().DownloadExcelTemp;
                        string noiseInput = InitialLearningConstants.NoiseInputFile;
                        filePath = string.Format(noiseInput, path, timeStamp);
                        Utility.DataTableToCSV(dsResult.Tables[0], filePath);
                    }
                    if (dsResult.Tables[1].Rows.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Locale = CultureInfo.InvariantCulture;
                        dt = dsResult.Tables[1];
                        string bUName;
                        string accountName;
                        string projectName;
                        string initialLearningId;
                        string esaProjectID;

                        bUName = (Convert.ToString(dt.Rows[0]["BUName"] != DBNull.Value ? Convert.
                            ToString(dt.Rows[0]["BUName"]) : string.Empty));
                        accountName = (Convert.ToString(dt.Rows[0]["AccountName"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[0]["AccountName"]) : string.Empty));
                        projectName = (Convert.ToString(dt.Rows[0]["ProjectName"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[0]["ProjectName"]) : string.Empty));
                        esaProjectID = (Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"]
                            != DBNull.Value ? Convert.ToString(dsResult.Tables[0].Rows[0]["EsaProjectID"]) :
                            string.Empty));
                        initialLearningId = (Convert.ToString(dt.Rows[0]["InitialLearningId"] != DBNull.Value ?
                            Convert.ToString(dt.Rows[0]["InitialLearningId"]) : string.Empty));

                        System.Text.StringBuilder account_Name = new System.Text.StringBuilder();
                        Utility.ErrorLOGInfra("0. in before SubmitNoiseEliminationJob  initialLearningId =" + initialLearningId,
                   "SubmitNoiseEliminationJob", 0);
                        MLJobDetails = Utility.SubmitNoiseEliminationMapReduceJob(bUName, account_Name.ToString(),
                            projectName, esaProjectID, initialLearningId, filePath, UserID, ProjectID, SupportTypeID);
                        jobSuccess = InsertMLJobId(ProjectID, initialLearningId, MLJobDetails.MLJobId,
                            MLJobDetails.FileName, MLJobDetails.DataPath, "NoiseEl", "Sent", UserID, SupportTypeID);
                        Utility.ErrorLOGInfra("0. in job success SubmitNoiseEliminationJob  jobSuccess =" + jobSuccess,
                   "SubmitNoiseEliminationJob", 0);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLOG(ex.Message, "SubmitNoiseEliminationJob",
                    Convert.ToInt32(UserID));
            }
            return jobSuccess;


        }

        /// <summary>
        ///  To get the ticket details for noise elimination
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <returns></returns>
        public string GetTicketsForNoiseElimination(string ProjectID, string AssociateID, Int32 SupportTypeID)
        {
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;

            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", ProjectID);
                prms[1] = new SqlParameter("@UserID", AssociateID);

                if (SupportTypeID == 2)
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_GetTicketDetailsForInfraNoiseElimination", prms, ConnectionString);
                }
                else
                {
                    dsResult = (new DBHelper()).GetDatasetFromSP("ML_GetTicketDetailsForNoiseElimination", prms, ConnectionString);
                }
                int optionalfielid;
                bool PresenceOfOptional;

                if (dsResult.Tables[1].Rows.Count > 0)
                {
                    optionalfielid = Convert.ToInt16(dsResult.Tables[1].Rows[0]["OptionalFieldID"]);
                    PresenceOfOptional = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["PresenceOfOptional"]);
                }
                else
                {
                    optionalfielid = 0;
                    PresenceOfOptional = false;
                }
                string EncryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                AESEncryption aesMod = new AESEncryption();

                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    if (string.Compare(EncryptionEnabled, Enabled) == 0)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dsResult.Tables[0].
                            Rows[i]["TicketDescription"])))
                        {

                            string bytesDecrypted = aesMod.DecryptStringBytes((string)dsResult.Tables[0]
                                .Rows[i]["TicketDescription"], AseKeyDetail.AesKeyConstVal);
                            string decTicketDesc = bytesDecrypted;
                            dsResult.Tables[0].Rows[i]["TicketDescription"] = decTicketDesc;
                        }
                        else
                        {
                            dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.
                                ToString(dsResult.Tables[0].Rows[i]["TicketDescription"]);
                        }
                    }
                    else
                    {
                        dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.ToString(dsResult.
                            Tables[0].Rows[i]["TicketDescription"]);
                    }

                    dsResult.Tables[0].Rows[i]["TicketDescription"] = Convert.ToString(dsResult.
                        Tables[0].Rows[i]["TicketDescription"]);
                    if (PresenceOfOptional)
                    {
                        if (optionalfielid == 2)
                        {
                            if (string.Compare(EncryptionEnabled, Enabled) == 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dsResult.Tables[0].
                                    Rows[i]["AdditionalText"])))
                                {

                                    string bytesDecrypted = aesMod.DecryptStringBytes((string)dsResult.Tables[0]
                                        .Rows[i]["AdditionalText"], AseKeyDetail.AesKeyConstVal);
                                    string decTicketDesc = bytesDecrypted;
                                    dsResult.Tables[0].Rows[i]["AdditionalText"] = decTicketDesc;
                                }
                                else
                                {
                                    dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                                        Tables[0].Rows[i]["AdditionalText"]);
                                }
                            }
                            else
                            {
                                dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                                    Tables[0].Rows[i]["AdditionalText"]);
                            }

                        }

                        dsResult.Tables[0].Rows[i]["AdditionalText"] = Convert.ToString(dsResult.
                            Tables[0].Rows[i]["AdditionalText"]);
                    }
                }
                Utility.ErrorLOGInfra("5 .1. in before SubmitNoiseEliminationJob  ",
                     "AssociateID is" + AssociateID, 0);

                jobSuccess = SubmitNoiseEliminationJob(dsResult, Convert.ToInt32(ProjectID), AssociateID,
                    SupportTypeID);



            }

            catch (Exception ex)
            {
                Utility.ErrorLOGInfra("5 .1. in COUNT RECEIVED  ",
                     "EXCEPTION  IS =" + ex.Message, 0);
                throw ex;
            }
            return jobSuccess;
        }


        /// <summary>
        /// This Method is used to UpdateNoiseSkipAndContinue
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string UpdateNoiseInfraSkipAndContinue(Int32 ProjectID, string EmployeeID)
        {
            string CriteriaMet = string.Empty;

            try
            {
                SqlParameter[] prms1 = new SqlParameter[2];
                prms1[0] = new SqlParameter("@ProjectID", ProjectID);
                prms1[1] = new SqlParameter("@UserID", EmployeeID);
                DataTable dt = (new DBHelper()).GetTableFromSP("ML_UpdateNoiseEliminationInfraSkipOrCont", prms1, ConnectionString);
                if (dt != null)
                {
                    CriteriaMet = dt.Rows[0]["CriteriaMet"].ToString();
                }


            }

            catch (Exception ex)
            {
                throw ex;
            }
            return CriteriaMet;

        }

        /// <summary>
        ///  Used to get the Pattern details of a project
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="Datefrom">Date from</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="ID">Employee ID</param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternValidationReport(int projectID, string ID)
        {

            List<SpDebtMLPatternValidationModel> debtMLPatternValidationList =
                new List<SpDebtMLPatternValidationModel>();

            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", projectID);
            prms[1] = new SqlParameter("@DateFrom", string.Empty);
            prms[2] = new SqlParameter("@DateTo", string.Empty);
            prms[3] = new SqlParameter("@UserID", ID);
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("ML_MLPatternValidation", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    debtMLPatternValidationList = dt.AsEnumerable().Select(row =>
                    new SpDebtMLPatternValidationModel
                    {
                        Id = row["ID"] != null ? Convert.ToInt32(row["ID"]) : 0,
                        InitialLearningId = row["InitialLearningID"] != null ? Convert.
                        ToInt32(row["InitialLearningID"]) : 0,
                        ApplicationId = row["ApplicationID"] != null ? Convert.
                        ToInt32(row["ApplicationID"]) : 0,
                        ApplicationName = (Convert.ToString(row["ApplicationName"] !=
                        DBNull.Value ? Convert.ToString(row["ApplicationName"]) : string.Empty)),
                        ApplicationTypeId = row["ApplicationTypeID"] != null ? Convert.
                        ToInt32(row["ApplicationTypeID"]) : 0,
                        ApplicationTypeName = (Convert.ToString(row["ApplicationTypeName"] !=
                        DBNull.Value ? Convert.ToString(row["ApplicationTypeName"]) : string.Empty)),
                        TechnologyId = row["TechnologyID"] != null ? Convert.ToInt32(row["TechnologyID"]) : 0,
                        TechnologyName = (Convert.ToString(row["TechnologyName"] != DBNull.Value ? Convert.
                        ToString(row["TechnologyName"]) : string.Empty)),
                        TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                        ToString(row["TicketPattern"]) : string.Empty)),

                        SubPattern = (Convert.ToString(row["SubPattern"] != DBNull.Value
                                    && Convert.ToString(row["SubPattern"]) != "0" ? Convert.ToString(row["SubPattern"])
                                    : string.Empty)),
                        AdditionalTextPattern = (Convert.ToString(row["AdditionalPattern"] != DBNull.Value &&
                                      Convert.ToString(row["AdditionalPattern"]) != "0" ?
                        Convert.ToString(row["AdditionalPattern"]) : string.Empty)),
                        AdditionalTextsubPattern = (Convert.ToString(row["AdditionalSubPattern"] !=
                                 DBNull.Value && Convert.ToString(row["AdditionalSubPattern"]) != "0"
                        ? Convert.ToString(row["AdditionalSubPattern"]) : string.Empty)),
                        MLDebtClassificationId = row["MLDebtClassificationID"] != null ? Convert.
                        ToInt32(row["MLDebtClassificationID"]) : 0,
                        MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] !=
                        DBNull.Value ? Convert.ToString(row["MLDebtClassificationName"]) : string.Empty)),
                        MLResidualFlagId = row["MLResidualFlagID"] != null ? Convert.
                        ToInt32(row["MLResidualFlagID"]) : 0,
                        MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] != DBNull.Value ?
                        Convert.ToString(row["MLResidualFlagName"]) : string.Empty)),
                        MLAvoidableFlagId = row["MLAvoidableFlagID"] != null ? Convert.
                        ToInt32(row["MLAvoidableFlagID"]) : 0,
                        MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] !=
                        DBNull.Value ? Convert.ToString(row["MLAvoidableFlagName"]) : string.Empty)),
                        MLCauseCodeId = row["MLCauseCodeID"] != null ? Convert.ToInt32(row["MLCauseCodeID"]) : 0,
                        MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ? Convert.
                        ToString(row["MLCauseCodeName"]) : string.Empty)),
                        MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ?
                        Convert.ToString(row["MLResolutionCodeName"]) : string.Empty)),
                        MLResolutionCodeId = row["MLResolutionCodeID"] != null ? Convert.
                        ToInt32(row["MLResolutionCodeID"]) : 0,
                        MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                        ToString(row["MLAccuracy"]) : string.Empty)),
                        TicketOccurence = (Convert.ToInt32(row["TicketOccurence"] != DBNull.Value ?
                        Convert.ToInt32(row["TicketOccurence"]) : 0)),
                        AnalystResolutionCodeId = row["AnalystResolutionCodeID"] != null ? Convert.
                        ToInt32(row["AnalystResolutionCodeID"]) : 0,
                        AnalystResolutionCodeName = (Convert.ToString(row["AnalystResolutionCodeName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystResolutionCodeName"]) : string.Empty)),
                        AnalystCauseCodeId = row["AnalystCauseCodeID"] != null ? Convert.
                        ToInt32(row["AnalystCauseCodeID"]) : 0,
                        AnalystCauseCodeName = (Convert.ToString(row["AnalystCauseCodeName"] != DBNull.Value ?
                        Convert.ToString(row["AnalystCauseCodeName"]) : string.Empty)),
                        AnalystDebtClassificationId = row["AnalystDebtClassificationID"] != null ? Convert.
                        ToInt32(row["AnalystDebtClassificationID"]) : 0,
                        AnalystDebtClassificationName = (Convert.ToString(row["AnalystDebtClassificationName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystDebtClassificationName"]) : string.Empty)),
                        AnalystAvoidableFlagId = row["AnalystAvoidableFlagID"] != null ? Convert.
                        ToInt32(row["AnalystAvoidableFlagID"]) : 0,
                        AnalystAvoidableFlagName = (Convert.ToString(row["AnalystAvoidableFlagName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystAvoidableFlagName"]) : string.Empty)),
                        SMEComments = (Convert.ToString(row["SMEComments"] != DBNull.Value ? Convert.
                        ToString(row["SMEComments"]) : string.Empty)),
                        SMEResidualFlagId = row["SMEResidualFlagID"] != null ? Convert.
                        ToInt32(row["SMEResidualFlagID"]) : 0,
                        SMEResidualFlagName = (Convert.ToString(row["SMEResidualFlagName"] != DBNull.Value ?
                        Convert.ToString(row["SMEResidualFlagName"]) : string.Empty)),
                        SMEDebtClassificationId = row["SMEDebtClassificationID"] != null ? Convert.
                        ToInt32(row["SMEDebtClassificationID"]) : 0,
                        SMEDebtClassificationName = (Convert.ToString(row["SMEDebtClassificationName"] !=
                        DBNull.Value ? Convert.ToString(row["SMEDebtClassificationName"]) : string.Empty)),
                        SMEAvoidableFlagId = row["SMEAvoidableFlagID"] != null ? Convert.
                        ToInt32(row["SMEAvoidableFlagID"]) : 0,
                        SMEAvoidableFlagName = (Convert.ToString(row["SMEAvoidableFlagName"] != DBNull.
                        Value ? Convert.ToString(row["SMEAvoidableFlagName"]) : string.Empty)),
                        SMECauseCodeId = row["SMECauseCodeID"] != null ? Convert.ToInt32(row["SMECauseCodeID"]) : 0,
                        SMECauseCodeName = (Convert.ToString(row["SMECauseCodeName"] != DBNull.Value ? Convert.
                        ToString(row["SMECauseCodeName"]) : string.Empty)),
                        IsApprovedOrMute = row["IsApprovedOrMute"] != null ? Convert.ToInt32(row["IsApprovedOrMute"])
                        : 0,
                        IsApproved = row["IsApproved"] != null ? Convert.ToInt32(row["IsApproved"]) : 0,
                        OverridenTotalCount = row["OverridenPatternTotalCount"] != null ? Convert.
                        ToInt32(row["OverridenPatternTotalCount"]) : 0,
                        IsMLSignoff = row["ISMLSignoff"] != null ? Convert.ToBoolean(row["ISMLSignoff"]) :
                        false,
                        IsRegenerated = row["IsRegenerated"] != null ? Convert.ToInt32(row["IsRegenerated"]) : 0


                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                ErrorLOG(ex.Message, "GetDebtMLPatternValidationReport", Convert.ToInt64(projectID));
            }
            return debtMLPatternValidationList;
        }

        /// <summary>
        ///  Used to get details for view all screen
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public SpDebtMLPatternValidationModelForViewAll GetDebtMLPatternValidationReportForViewAll(int projectID,
            int supportTypeID)
        {

            SpDebtMLPatternValidationModelForViewAll DebtMLPatternValidationList1 =
                new SpDebtMLPatternValidationModelForViewAll();

            try
            {
                DataSet ds = new DataSet();
                ds.Locale = CultureInfo.InvariantCulture;
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                if (supportTypeID == 1)
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[dbo].[ML_GetViewAllPattern]", prms, ConnectionString);
                }
                else
                {
                    ds = (new DBHelper()).GetDatasetFromSP("[dbo].[ML_GetViewAllInfraPattern]", prms, ConnectionString);
                }
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    DebtMLPatternValidationList1.ExistingPatternsModel = dt.AsEnumerable().Select(row =>
                    new SpDebtMLPatternValidationModel
                    {
                        Id = row["ID"] != null ? Convert.ToInt32(row["ID"]) : 0,
                        InitialLearningId = row["InitialLearningID"] != null ? Convert.
                        ToInt32(row["InitialLearningID"]) : 0,
                        ApplicationId = supportTypeID == 1 ? (row["ApplicationID"] != null ? Convert.
                        ToInt32(row["ApplicationID"]) : 0) : 0,
                        ApplicationName = supportTypeID == 1 ? ((Convert.ToString(row["ApplicationName"]
                        != DBNull.Value ? Convert.
                        ToString(row["ApplicationName"]) : string.Empty))) : string.Empty,
                        ApplicationTypeId = supportTypeID == 1 ? (row["ApplicationTypeID"] != null ? Convert.
                        ToInt32(row["ApplicationTypeID"]) : 0) : 0,
                        ApplicationTypeName = supportTypeID == 1 ? ((Convert.ToString(row["ApplicationTypeName"] !=
                        DBNull.Value ? Convert.ToString(row["ApplicationTypeName"]) : string.Empty))) : string.Empty,
                        TechnologyId = supportTypeID == 1 ? (row["TechnologyID"] != null ?
                        Convert.ToInt32(row["TechnologyID"]) : 0) : 0,
                        TechnologyName = supportTypeID == 1 ? ((Convert.ToString(row["TechnologyName"] !=
                        DBNull.Value ? Convert.
                        ToString(row["TechnologyName"]) : string.Empty))) : string.Empty,
                        TowerId = supportTypeID == 2 ? (row["TowerID"] != null ? Convert.
                        ToInt32(row["TowerID"]) : 0) : 0,
                        TowerName = supportTypeID == 2 ? ((Convert.ToString(row["TowerName"] != DBNull.Value ? Convert.
                        ToString(row["TowerName"]) : string.Empty))) : string.Empty,
                        TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                        ToString(row["TicketPattern"]) : string.Empty)),
                        SubPattern = (Convert.ToString(row["SubPattern"] != DBNull.Value ? Convert.
                        ToString(row["SubPattern"]) : string.Empty)),
                        AdditionalTextPattern = (Convert.ToString(row["AdditionalPattern"] != DBNull.Value ?
                        Convert.ToString(row["AdditionalPattern"]) : string.Empty)),
                        AdditionalTextsubPattern = (Convert.ToString(row["AdditionalSubPattern"] !=
                        DBNull.Value ? Convert.ToString(row["AdditionalSubPattern"]) : string.Empty)),
                        MLDebtClassificationId = row["MLDebtClassificationID"] != null ? Convert.
                        ToInt32(row["MLDebtClassificationID"]) : 0,
                        MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] !=
                        DBNull.Value ? Convert.ToString(row["MLDebtClassificationName"]) : string.Empty)),
                        MLResidualFlagId = row["MLResidualFlagID"] != null ? Convert.
                        ToInt32(row["MLResidualFlagID"]) : 0,
                        MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] !=
                        DBNull.Value ? Convert.ToString(row["MLResidualFlagName"]) : string.Empty)),
                        MLAvoidableFlagId = row["MLAvoidableFlagID"] != null ? Convert.
                        ToInt32(row["MLAvoidableFlagID"]) : 0,
                        MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] !=
                        DBNull.Value ? Convert.ToString(row["MLAvoidableFlagName"]) : string.Empty)),
                        MLCauseCodeId = row["MLCauseCodeID"] != null ? Convert.
                        ToInt32(row["MLCauseCodeID"]) : 0,
                        MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ?
                        Convert.ToString(row["MLCauseCodeName"]) : string.Empty)),
                        MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ?
                        Convert.ToString(row["MLResolutionCodeName"]) : string.Empty)),
                        MLResolutionCodeId = row["MLResolutionCodeID"] != null ? Convert.
                        ToInt32(row["MLResolutionCodeID"]) : 0,
                        MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                        ToString(row["MLAccuracy"]) : string.Empty)),
                        TicketOccurence = (Convert.ToInt32(row["TicketOccurence"] != DBNull.Value ?
                        Convert.ToInt32(row["TicketOccurence"]) : 0)),
                        AnalystResolutionCodeId = row["AnalystResolutionCodeID"] != null ? Convert.
                        ToInt32(row["AnalystResolutionCodeID"]) : 0,
                        AnalystResolutionCodeName = (Convert.ToString(row["AnalystResolutionCodeName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystResolutionCodeName"]) : string.Empty)),
                        AnalystCauseCodeId = row["AnalystCauseCodeID"] != null ? Convert.
                        ToInt32(row["AnalystCauseCodeID"]) : 0,
                        AnalystCauseCodeName = (Convert.ToString(row["AnalystCauseCodeName"] != DBNull.Value ?
                        Convert.ToString(row["AnalystCauseCodeName"]) : string.Empty)),
                        AnalystDebtClassificationId = row["AnalystDebtClassificationID"] != null ?
                        Convert.ToInt32(row["AnalystDebtClassificationID"]) : 0,
                        AnalystDebtClassificationName = (Convert.ToString(row["AnalystDebtClassificationName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystDebtClassificationName"]) :
                        string.Empty)),
                        AnalystAvoidableFlagId = row["AnalystAvoidableFlagID"] != null ?
                        Convert.ToInt32(row["AnalystAvoidableFlagID"]) : 0,
                        AnalystAvoidableFlagName = (Convert.ToString(row["AnalystAvoidableFlagName"] !=
                        DBNull.Value ? Convert.ToString(row["AnalystAvoidableFlagName"]) : string.Empty)),
                        SMEComments = (Convert.ToString(row["SMEComments"] != DBNull.Value ?
                        Convert.ToString(row["SMEComments"]) : string.Empty)),
                        SMEResidualFlagId = row["SMEResidualFlagID"] != null ? Convert.
                        ToInt32(row["SMEResidualFlagID"]) : 0,
                        SMEResidualFlagName = (Convert.ToString(row["SMEResidualFlagName"] != DBNull.Value ?
                        Convert.ToString(row["SMEResidualFlagName"]) : string.Empty)),
                        SMEDebtClassificationId = row["SMEDebtClassificationID"] != null ? Convert.
                        ToInt32(row["SMEDebtClassificationID"]) : 0,
                        SMEDebtClassificationName = (Convert.ToString(row["SMEDebtClassificationName"]
                        != DBNull.Value ? Convert.ToString(row["SMEDebtClassificationName"]) : string.Empty)),
                        SMEAvoidableFlagId = row["SMEAvoidableFlagID"] != null ? Convert.
                        ToInt32(row["SMEAvoidableFlagID"]) : 0,
                        SMEAvoidableFlagName = (Convert.ToString(row["SMEAvoidableFlagName"] != DBNull.Value ?
                        Convert.ToString(row["SMEAvoidableFlagName"]) : string.Empty)),
                        SMECauseCodeId = row["SMECauseCodeID"] != null ? Convert.ToInt32(row["SMECauseCodeID"]) :
                        0,
                        SMECauseCodeName = (Convert.ToString(row["SMECauseCodeName"] != DBNull.Value ?
                        Convert.ToString(row["SMECauseCodeName"]) : string.Empty)),
                        IsApprovedOrMute = row["IsApprovedOrMute"] != null ? Convert.
                        ToInt32(row["IsApprovedOrMute"]) : 0,
                        IsApproved = row["IsApproved"] != null ? Convert.ToInt32(row["IsApproved"]) : 0,
                        OverridenTotalCount = row["OverridenPatternTotalCount"] != null ? Convert.
                        ToInt32(row["OverridenPatternTotalCount"]) : 0,
                        IsMLSignoff = row["ISMLSignoff"] != null ? Convert.ToBoolean(row["ISMLSignoff"]) : false,
                        IsRegenerated = row["IsRegenerated"] != null ? Convert.ToInt32(row["IsRegenerated"]) : 0


                    }).ToList();


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DebtMLPatternValidationList1;
        }
        /// <summary>
        ///  Used to get pattern occurence Report
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="subPattern"></param>
        /// <param name="AdditionalTextPattern"></param>
        /// <param name="AdditionalTextsubPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternOccurenceReport(int projectID, string AppIds,
            string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextsubPattern,
            int causeCodeId, int ResolutionCodeID)
        {

            List<SpDebtMLPatternValidationModel> debtMLPatternValidationList =
                new List<SpDebtMLPatternValidationModel>();
            SqlParameter[] prms = new SqlParameter[8];
            prms[0] = new SqlParameter("@projectID", projectID);
            prms[1] = new SqlParameter("@AppID", AppIds);
            prms[2] = new SqlParameter("@TicketPattern", TicketPattern);
            prms[3] = new SqlParameter("@subPattern", subPattern);
            prms[4] = new SqlParameter("@AdditionalTextPattern", AdditionalTextPattern);
            prms[5] = new SqlParameter("@AdditionalTextSubPattern", AdditionalTextsubPattern);
            prms[6] = new SqlParameter("@causeCodeId", causeCodeId);
            prms[7] = new SqlParameter("@ResolutionCodeID", ResolutionCodeID);
            DataSet dt = new DataSet();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Tables.Add((new DBHelper()).GetTableFromSP("ML_MLGetPatternOccurence", prms, ConnectionString).Copy());
            if (dt.Tables[0] != null)
            {
                debtMLPatternValidationList = dt.Tables[0].AsEnumerable().Select(row =>
                new SpDebtMLPatternValidationModel
                {
                    Id = row["ID"] != null ? Convert.ToInt32(row["ID"]) : 0,
                    ApplicationName = (Convert.ToString(row["ApplicationName"] != DBNull.Value ? Convert.
                    ToString(row["ApplicationName"]) : string.Empty)),
                    ApplicationId = row["ApplicationID"] != null ? Convert.ToInt32(row["ApplicationID"]) : 0,
                    TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                    ToString(row["TicketPattern"]) : string.Empty)),
                    TicketOccurence = row["TicketOccurence"] != null ? Convert.ToInt32(row["TicketOccurence"]) : 0,
                    MLDebtClassificationId = row["MLDebtClassificationID"] != null ? Convert.
                    ToInt32(row["MLDebtClassificationID"]) : 0,
                    MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] != DBNull.Value ?
                    Convert.ToString(row["MLDebtClassificationName"]) : string.Empty)),
                    MLResidualFlagId = row["MLResidualCodeID"] != null ? Convert.ToInt32(row["MLResidualCodeID"]) : 0,
                    MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] != DBNull.Value ? Convert.
                    ToString(row["MLResidualFlagName"]) : string.Empty)),
                    MLAvoidableFlagId = row["MLAvoidableFlagID"] != null ? Convert.
                    ToInt32(row["MLAvoidableFlagID"]) : 0,
                    MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] != DBNull.Value ?
                    Convert.ToString(row["MLAvoidableFlagName"]) : string.Empty)),
                    MLCauseCodeId = row["MLCauseCodeID"] != null ? Convert.ToInt32(row["MLCauseCodeID"]) : 0,
                    MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ? Convert.
                    ToString(row["MLCauseCodeName"]) : string.Empty)),
                    MLResolutionCodeId = row["MLResolutionCodeID"] != null ? Convert.
                    ToInt32(row["MLResolutionCodeID"]) : 0,
                    MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ? Convert.
                    ToString(row["MLResolutionCodeName"]) : string.Empty)),
                    SubPattern = (Convert.ToString(row["subPattern"] != DBNull.Value ? Convert.
                    ToString(row["subPattern"]) : string.Empty)),
                    AdditionalTextPattern = (Convert.ToString(row["additionalPattern"] != DBNull.Value ? Convert.
                    ToString(row["additionalPattern"]) : string.Empty)),
                    AdditionalTextsubPattern = (Convert.ToString(row["additionalSubPattern"] != DBNull.Value ?
                    Convert.ToString(row["additionalSubPattern"]) : string.Empty)),
                    MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                    ToString(row["MLAccuracy"]) : string.Empty))

                }).ToList();
            }

            return debtMLPatternValidationList;
        }

        /// <summary>
        ///  To save ml pattern details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtMLPatternModel">ML Details</param>
        /// <param name="UserId">User Id</param>
        /// <returns></returns>
        public string SaveDebtPatternValidationDetails(int ProjectID,
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel, string UserId, int SupportType)
        {

            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);

            var objCollection = from i in lstDebtMLPatternModel
                                select new
                                {
                                    ID = i.Id,
                                    TicketPattern = i.TicketPattern,
                                    SMEComments = i.SMEComments,
                                    SMEResidualFlagID = i.SMEResidualFlagId.ToString(),
                                    SMEDebtClassificationID = i.SMEDebtClassificationId.ToString(),
                                    SMEAvoidableFlagID = i.SMEAvoidableFlagId.ToString(),
                                    MLResidualFlagID = i.MLResidualFlagId == null ? "" : Convert.ToString(i.MLResidualFlagId),
                                    MLDebtClassificationID = i.MLDebtClassificationId == null ? "" : Convert.ToString(i.MLDebtClassificationId),
                                    MLAvoidableFlagID = i.MLAvoidableFlagId == null ? "" : Convert.ToString(i.MLAvoidableFlagId),
                                    SMECauseCodeID = i.SMECauseCodeId.ToString(),
                                    IsApprovedOrMute = i.IsApprovedOrMute,
                                    OveriddenPatternCount = i.OverriddenPatternCount,
                                    ML_Accuracy = i.MLAccuracy,
                                    TicketOccurence = i.TicketOccurence
                                };

            prms[1] = new SqlParameter("@lstApprovedPatternValidation", objCollection.ToList().ToDT());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = InitialLearningConstants.TypeSaveApprovedPatternValidation;
            prms[2] = new SqlParameter("@UserId", UserId);
            prms[3] = new SqlParameter("@SupportType", SupportType);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_SavePatternValidation", prms, ConnectionString);
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
        /// Used to generate patterns functionality
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="lstGeneratePatternApps">Application details</param>
        /// <param name="UserId">User Id</param>
        /// <param name="CustomerID">Customer ID</param>
        /// <returns></returns>
        public string Generatepatterns(int ProjectID, List<RegenerateApplicationDetails> lstGeneratePatternApps,
            string UserId, int CustomerID)
        {
            List<RegenerateApplicationDetails> objApplicationsCollection = new List<RegenerateApplicationDetails>();
            SqlParameter[] prms = new SqlParameter[4];
            for (int i = 0; i < lstGeneratePatternApps.Count; i++)
            {
                objApplicationsCollection.Add(new RegenerateApplicationDetails
                {
                    ApplicationId = lstGeneratePatternApps[i].ApplicationId,
                });

            }
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@lstRegenerateApps",
                objApplicationsCollection.ToDataTable<RegenerateApplicationDetails>());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = ApplicationConstants.TypeRegenerateApplicationDetails;
            prms[2] = new SqlParameter("@UserId", UserId);
            prms[3] = new SqlParameter("@CustomerID", CustomerID);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_SaveRegenerateApplicationDetails", prms, ConnectionString);
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
        /// Used to get copy patterns
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstCopyPatternsModel"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string CopyPatterns(int ProjectID, List<DebtMLPatternValidationModel> lstCopyPatternsModel,
            string UserId)
        {

            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            
            var objCollection = from i in lstCopyPatternsModel
                                select new
                                {
                                    ID = i.Id,
                                    TicketPattern = i.TicketPattern,
                                    SMEComments = i.SMEComments,
                                    SMEResidualFlagID = i.SMEResidualFlagId.ToString(),
                                    SMEDebtClassificationID = i.SMEDebtClassificationId.ToString(),
                                    SMEAvoidableFlagID = i.SMEAvoidableFlagId.ToString(),
                                    MLResidualFlagID = i.MLResidualFlagId == null ? "" : Convert.ToString(i.MLResidualFlagId),
                                    MLDebtClassificationID = i.MLDebtClassificationId == null ? "" : Convert.ToString(i.MLDebtClassificationId),
                                    MLAvoidableFlagID = i.MLAvoidableFlagId == null ? "" : Convert.ToString(i.MLAvoidableFlagId),
                                    SMECauseCodeID = i.SMECauseCodeId.ToString(),
                                    IsApprovedOrMute = i.IsApprovedOrMute,
                                    OveriddenPatternCount = i.OverriddenPatternCount,
                                    ML_Accuracy = i.MLAccuracy,
                                    TicketOccurence = i.TicketOccurence
                                };

            prms[1] = new SqlParameter("@lstCopyPatternsModel", objCollection.ToList().ToDT());
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[2] = new SqlParameter("@UserId", UserId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_CopyPatterns", prms, ConnectionString);

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
        ///  Used to get the Master value of Debt fields
        /// </summary>
        /// <param name="projectID">project ID</param>

        /// <returns></returns>
        public List<SpDebtMasterValues> GetDebtMasterValues(int projectID, int SupportTypeID)

        {
            List<SpDebtMasterValues> debtApprovalList = new List<SpDebtMasterValues>();

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", projectID);
            prms[1] = new SqlParameter("@SupportType", SupportTypeID);

            try
            {
                DataTable dtMasterValues = (new DBHelper()).GetTableFromSP("ML_MasterValuesForPatternValidation", prms, ConnectionString);
                if (dtMasterValues != null && dtMasterValues.Rows.Count > 0)
                {
                    debtApprovalList = dtMasterValues.AsEnumerable().Select(row => new SpDebtMasterValues
                    {
                        AttributeType = Convert.ToString(row["AttributeType"]),
                        AttributeTypeId = Convert.ToInt32(row["AttributeTypeId"]),
                        AttributeTypeValue = Convert.ToString(row["AttributeTypeValue"])

                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                ErrorLOG(ex.Message, "GetDebtMasterValues", Convert.ToInt64(projectID));
            }
            return debtApprovalList;
        }

        /// <summary>
        /// Class DebtPatternCollection
        /// </summary>
        public class DebtPatternCollection : List<DebtMLPatternSaveModel>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow1 = new SqlDataRecord(
                      new SqlMetaData("ApplicationName", SqlDbType.VarChar, SqlMetaData.Max),
                       new SqlMetaData("TowerName", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ApplicationType", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("Technology", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("DebtClassification", SqlDbType.VarChar, 500),
                      new SqlMetaData("AvoidableFlag", SqlDbType.VarChar, 500),
                      new SqlMetaData("ResidualDebt", SqlDbType.VarChar, 500),
                      new SqlMetaData("CauseCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ResolutionCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("MLDebtClassification", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLAvoidableFlag", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLResidualDebt", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLCauseCode", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("MLWorkPattern", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("DescSubPattern", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ResBasePattern", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ResSubPattern", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("MLRuleAccuracy", SqlDbType.VarChar, 500),
                      new SqlMetaData("SMEApproval", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLResolutionCode", SqlDbType.VarChar, 500),
                      new SqlMetaData("TicketOccurence", SqlDbType.Int),
                      new SqlMetaData("Classifiedby", SqlDbType.VarChar, 500)
       );

                foreach (DebtMLPatternSaveModel obj in this)
                {
                    sqlRow1.SetString(0, Convert.ToString(obj.ApplicationName) != null ? Convert.
                        ToString(obj.ApplicationName) : string.Empty);
                    sqlRow1.SetString(1, Convert.ToString(obj.TowerName) != null ? Convert.
                      ToString(obj.TowerName) : string.Empty);
                    sqlRow1.SetString(2, Convert.ToString(obj.ApplicationType) != null ? Convert.
                        ToString(obj.ApplicationType) : string.Empty);
                    sqlRow1.SetString(3, Convert.ToString(obj.Technology) != null ? Convert.
                        ToString(obj.Technology) : string.Empty);
                    sqlRow1.SetString(4, Convert.ToString(obj.DebtClassification) != null ? Convert.
                        ToString(obj.DebtClassification) : string.Empty);
                    sqlRow1.SetString(5, Convert.ToString(obj.AvoidableFlag) != null ? Convert.
                        ToString(obj.AvoidableFlag) : string.Empty);
                    sqlRow1.SetString(6, Convert.ToString(obj.ResidualDebt) != null ? Convert.
                        ToString(obj.ResidualDebt) : string.Empty);
                    sqlRow1.SetString(7, Convert.ToString(obj.CauseCode) != null ? Convert.
                        ToString(obj.CauseCode) : string.Empty);
                    sqlRow1.SetString(8, Convert.ToString(obj.ResolutionCode) != null ? Convert.
                        ToString(obj.ResolutionCode) : string.Empty);
                    sqlRow1.SetString(9, Convert.ToString(obj.MLDebtClassification) != null ? Convert.
                        ToString(obj.MLDebtClassification) : string.Empty);
                    sqlRow1.SetString(10, Convert.ToString(obj.MLAvoidableFlag) != null ? Convert.
                        ToString(obj.MLAvoidableFlag) : string.Empty);
                    sqlRow1.SetString(11, Convert.ToString(obj.MLResidualDebt) != null ? Convert.
                        ToString(obj.MLResidualDebt) : string.Empty);
                    sqlRow1.SetString(12, Convert.ToString(obj.MLCauseCode) != null ? Convert.
                        ToString(obj.MLCauseCode) : string.Empty);
                    sqlRow1.SetString(13, Convert.ToString(obj.MLWorkPattern) != null ? Convert.
                        ToString(obj.MLWorkPattern) : string.Empty);
                    sqlRow1.SetString(14, Convert.ToString(obj.DescSubWorkPattern) != null ? Convert.
                        ToString(obj.DescSubWorkPattern) : string.Empty);
                    sqlRow1.SetString(15, Convert.ToString(obj.ResBaseWorkPattern) != null ? Convert.
                        ToString(obj.ResBaseWorkPattern) : string.Empty);
                    sqlRow1.SetString(16, Convert.ToString(obj.ResSubWorkPattern) != null ? Convert.
                        ToString(obj.ResSubWorkPattern) : string.Empty);
                    sqlRow1.SetString(17, Convert.ToString(obj.MLRuleAccuracy) != null ? Convert.
                        ToString(obj.MLRuleAccuracy) : string.Empty);
                    sqlRow1.SetString(18, Convert.ToString(obj.SMEApproval) != null ? Convert.
                        ToString(obj.SMEApproval) : string.Empty);
                    sqlRow1.SetString(19, Convert.ToString(obj.MLResolutionCode) != null ? Convert.
                        ToString(obj.MLResolutionCode) : string.Empty);
                    sqlRow1.SetInt32(20, obj.TicketOccurence);
                    sqlRow1.SetString(21, Convert.ToString(obj.Classifiedby) != null ? Convert.
                        ToString(obj.Classifiedby) : string.Empty);
                    yield return sqlRow1;
                }
            }
        }
        /// <summary>
        /// Class DebtSampledTicketsCollection
        /// </summary>
        public class DebtSampledTicketsCollection : List<DebtSampledTicketsSaveModel>, IEnumerable<SqlDataRecord>
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
                      new SqlMetaData("ApplicationName", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ApplicationType", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("Technology", SqlDbType.VarChar, SqlMetaData.Max),
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
                    sqlRow1.SetString(4, Convert.ToString(obj.ApplicationName) != null ? Convert.
                        ToString(obj.ApplicationName) : string.Empty);
                    sqlRow1.SetString(5, Convert.ToString(obj.ApplicationType) != null ? Convert.
                        ToString(obj.ApplicationType) : string.Empty);
                    sqlRow1.SetString(6, Convert.ToString(obj.Technology) != null ? Convert.
                        ToString(obj.Technology) : string.Empty);
                    sqlRow1.SetString(7, Convert.ToString(obj.DebtClassification) != null ? Convert.
                        ToString(obj.DebtClassification) : string.Empty);
                    sqlRow1.SetString(8, Convert.ToString(obj.AvoidableFlag) != null ? Convert.
                        ToString(obj.AvoidableFlag) : string.Empty);
                    sqlRow1.SetString(9, Convert.ToString(obj.ResidualDebt) != null ? Convert.
                        ToString(obj.ResidualDebt) : string.Empty);
                    sqlRow1.SetString(10, Convert.ToString(obj.CauseCode) != null ? Convert.
                        ToString(obj.CauseCode) : string.Empty);
                    sqlRow1.SetString(11, Convert.ToString(obj.ResolutionCode) != null ? Convert.
                        ToString(obj.ResolutionCode) : string.Empty);
                    sqlRow1.SetString(12, Convert.ToString(obj.MLDebtClassification) != null ? Convert.
                        ToString(obj.MLDebtClassification) : string.Empty);
                    sqlRow1.SetString(13, Convert.ToString(obj.MLAvoidableFlag) != null ? Convert.
                        ToString(obj.MLAvoidableFlag) : string.Empty);
                    sqlRow1.SetString(14, Convert.ToString(obj.MLResidualDebt) != null ? Convert.
                        ToString(obj.MLResidualDebt) : string.Empty);
                    sqlRow1.SetString(15, Convert.ToString(obj.MLCauseCode) != null ? Convert.
                        ToString(obj.MLCauseCode) : string.Empty);
                    sqlRow1.SetString(16, Convert.ToString(obj.DescBaseWorkPattern) != null ? Convert.
                        ToString(obj.DescBaseWorkPattern) : string.Empty);
                    sqlRow1.SetString(17, Convert.ToString(obj.DescSubWorkPattern) != null ? Convert.
                        ToString(obj.DescSubWorkPattern) : string.Empty);
                    sqlRow1.SetString(18, Convert.ToString(obj.ResBaseWorkPattern) != null ? Convert.
                        ToString(obj.ResBaseWorkPattern) : string.Empty);
                    sqlRow1.SetString(19, Convert.ToString(obj.ResSubWorkPattern) != null ? Convert.
                        ToString(obj.ResSubWorkPattern) : string.Empty);


                    yield return sqlRow1;
                }
            }
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
        /// Class DebtApprovePatternTicketsCollection
        /// </summary>
        public class DebtApprovePatternTicketsCollection : List<DebtMLPatternValidationModel>,
            IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow1 = new SqlDataRecord(
                      new SqlMetaData("ID", SqlDbType.Int),
                      new SqlMetaData("TicketPattern", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("SMEComments", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("SMEResidualFlagID", SqlDbType.VarChar, 500),
                      new SqlMetaData("SMEDebtClassificationID", SqlDbType.VarChar, 500),
                      new SqlMetaData("SMEAvoidableFlagID", SqlDbType.VarChar, 500),
                       new SqlMetaData("MLResidualFlagID", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLDebtClassificationID", SqlDbType.VarChar, 500),
                      new SqlMetaData("MLAvoidableFlagID", SqlDbType.VarChar, 500),
                      new SqlMetaData("SMECauseCodeID", SqlDbType.VarChar, 500),
                      new SqlMetaData("IsApprovedOrMute", SqlDbType.Int),
                      new SqlMetaData("OveriddenPatternCount", SqlDbType.Int),
                      new SqlMetaData("ML_Accuracy", SqlDbType.VarChar, 500),
                      new SqlMetaData("TicketOccurence", SqlDbType.Int)
                      );

                foreach (DebtMLPatternValidationModel obj in this)
                {
                    sqlRow1.SetInt32(0, obj.Id != null ? obj.Id : 0);
                    sqlRow1.SetString(1, obj.TicketPattern != null ? obj.TicketPattern : string.Empty);
                    sqlRow1.SetString(2, obj.SMEComments != null ? obj.SMEComments : string.Empty);
                    sqlRow1.SetString(3, Convert.ToString(obj.SMEResidualFlagId) != null ? Convert.
                        ToString(obj.SMEResidualFlagId) : string.Empty);
                    sqlRow1.SetString(4, Convert.ToString(obj.SMEDebtClassificationId) != null ? Convert.
                        ToString(obj.SMEDebtClassificationId) : string.Empty);
                    sqlRow1.SetString(5, Convert.ToString(obj.SMEAvoidableFlagId) != null ? Convert.
                        ToString(obj.SMEAvoidableFlagId) : string.Empty);
                    sqlRow1.SetString(6, Convert.ToString(obj.MLResidualFlagId) != null ? Convert.
                        ToString(obj.MLResidualFlagId) : string.Empty);
                    sqlRow1.SetString(7, Convert.ToString(obj.MLDebtClassificationId) != null ? Convert.
                        ToString(obj.MLDebtClassificationId) : string.Empty);
                    sqlRow1.SetString(8, Convert.ToString(obj.MLAvoidableFlagId) != null ? Convert.
                        ToString(obj.MLAvoidableFlagId) : string.Empty);
                    sqlRow1.SetString(9, Convert.ToString(obj.SMECauseCodeId) != null ? Convert.
                        ToString(obj.SMECauseCodeId) : string.Empty);
                    sqlRow1.SetInt32(10, obj.IsApprovedOrMute != null ? obj.IsApprovedOrMute : 0);
                    sqlRow1.SetInt32(11, obj.OverriddenPatternCount != null ? obj.OverriddenPatternCount : 0);
                    sqlRow1.SetString(12, Convert.ToString(obj.MLAccuracy) != null ? Convert.
                        ToString(obj.MLAccuracy) : string.Empty);
                    sqlRow1.SetValue(13, obj.TicketOccurence != null ? obj.TicketOccurence : 0);
                    yield return sqlRow1;
                }
            }
        }
        /// <summary>
        /// DebtUploadExcelTicketsCollection
        /// </summary>
        public class DebtUploadExcelTicketsCollection : List<GetDebtFieldsForUploadExcel>, IEnumerable<SqlDataRecord>
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
                      new SqlMetaData("ApplicationName", SqlDbType.VarChar, 1000),
                      new SqlMetaData("DebtClassificationValue", SqlDbType.VarChar, 1000),
                      new SqlMetaData("AvoidableFlagValue", SqlDbType.VarChar, 1000),
                      new SqlMetaData("CauseCodeValue", SqlDbType.NVarChar, 1000),
                      new SqlMetaData("ResolutionCodeValue", SqlDbType.NVarChar, 1000),
                      new SqlMetaData("ResidualDebtValue", SqlDbType.VarChar, 1000),
                      new SqlMetaData("OptionalFieldProj", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("IsTicketDescriptionUpdated", SqlDbType.VarChar, 1000),
                       new SqlMetaData("IsTicketSummaryUpdated", SqlDbType.VarChar, 1000)
                      );
                foreach (GetDebtFieldsForUploadExcel obj in this)
                {
                    sqlRow.SetString(0, obj.TicketId);
                    sqlRow.SetString(1, obj.TicketDescription != null ? obj.TicketDescription : string.Empty);
                    sqlRow.SetString(2, obj.ApplicationName != null ? obj.ApplicationName : string.Empty);
                    sqlRow.SetString(3, obj.DebtClassificationValue != null ? obj.DebtClassificationValue :
                        string.Empty);
                    sqlRow.SetString(4, obj.AvoidableFlagValue != null ? obj.AvoidableFlagValue : string.Empty);
                    sqlRow.SetString(5, obj.CauseCodeValue != null ? obj.CauseCodeValue : string.Empty);
                    sqlRow.SetString(6, obj.ResolutionCodeValue != null ? obj.ResolutionCodeValue : string.Empty);
                    sqlRow.SetString(7, obj.ResidualDebtValue != null ? obj.ResidualDebtValue : string.Empty);
                    sqlRow.SetString(8, obj.OptionalFieldProj != null ? obj.OptionalFieldProj : string.Empty);
                    sqlRow.SetString(9, obj.IsTicketDescriptionUpdated != null ? obj.IsTicketDescriptionUpdated :
                        string.Empty);
                    sqlRow.SetString(10, obj.IsTicketSummaryUpdated != null ? obj.IsTicketSummaryUpdated :
                        string.Empty);
                    yield return sqlRow;
                }
            }
        }
        /// <summary>
        /// DebtTicketsProcessedCollection
        /// </summary>
        public class DebtTicketsProcessedCollection : List<GetDebtTicketsForValidation>, IEnumerable<SqlDataRecord>
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
                      new SqlMetaData("ApplicationID", SqlDbType.VarChar, 1000),
                      new SqlMetaData("DebtClassificationId", SqlDbType.VarChar, 1000),
                      new SqlMetaData("AvoidableFlagId", SqlDbType.VarChar, 1000),
                      new SqlMetaData("CauseCode", SqlDbType.VarChar, 1000),
                      new SqlMetaData("ResolutionCode", SqlDbType.VarChar, 1000),
                      new SqlMetaData("ResidualDebtId", SqlDbType.VarChar, 1000),
                      new SqlMetaData("OptionalField", SqlDbType.VarChar, 1000));
                foreach (GetDebtTicketsForValidation obj in this)
                {
                    sqlRow.SetString(0, obj.TicketId);
                    sqlRow.SetString(1, obj.TicketDescription != null ? obj.TicketDescription : string.Empty);
                    sqlRow.SetString(2, obj.ApplicationId != null ? obj.ApplicationId : string.Empty);
                    sqlRow.SetString(3, obj.DebtClassificationId != null ? obj.DebtClassificationId : string.Empty);
                    sqlRow.SetString(4, obj.AvoidableFlagId != null ? obj.AvoidableFlagId : string.Empty);
                    sqlRow.SetString(5, obj.CauseCode != null ? obj.CauseCode : string.Empty);
                    sqlRow.SetString(6, obj.ResolutionCode != null ? obj.ResolutionCode : string.Empty);
                    sqlRow.SetString(7, obj.ResidualDebtId != null ? obj.ResidualDebtId : string.Empty);
                    sqlRow.SetString(8, obj.OptionalField != null ? obj.OptionalField : string.Empty);
                    yield return sqlRow;
                }

            }
        }
        /// <summary>
        /// RegenerateApplicationCollection
        /// </summary>
        public class RegenerateApplicationCollection : List<GeneratePatternsApplications>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                     new SqlMetaData("ApplicationID", SqlDbType.Int),
                      new SqlMetaData("ApplicationName", SqlDbType.NVarChar, 1000),
                      new SqlMetaData("PortfolioID", SqlDbType.Int),
                      new SqlMetaData("PortfolioName", SqlDbType.NVarChar, SqlMetaData.Max),
                      new SqlMetaData("AppGroupID", SqlDbType.Int),
                      new SqlMetaData("AppGroupName", SqlDbType.NVarChar, 1000));
                foreach (GeneratePatternsApplications obj in this)
                {
                    sqlRow.SetInt32(0, obj.ApplicationId);
                    sqlRow.SetString(1, obj.ApplicationName != null ? obj.ApplicationName : string.Empty);
                    sqlRow.SetInt32(2, obj.PortfolioId);
                    sqlRow.SetString(3, obj.PortfolioName != null ? obj.PortfolioName : string.Empty);
                    sqlRow.SetInt32(4, obj.AppGroupId);
                    sqlRow.SetString(5, obj.AppGroupName != null ? obj.AppGroupName : string.Empty);
                    yield return sqlRow;
                }

            }
        }




        /// <summary>
        /// NoiseEliminationDeseDataCollection
        /// </summary>
        public class NoiseEliminationDeseDataCollection : List<NoiseEliminationTicketDescription>,
            IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("TicketDesFieldNoiseWord", SqlDbType.VarChar, 500),
                      new SqlMetaData("Frequency", SqlDbType.BigInt),
                      new SqlMetaData("IsActive", SqlDbType.Bit),
                      new SqlMetaData("IsDeleted", SqlDbType.Bit)
                     );

                if (this.Count == 0)
                {
                    sqlRow.SetString(0, string.Empty);
                    sqlRow.SetInt64(1, 0);
                    sqlRow.SetBoolean(2, false);
                    sqlRow.SetBoolean(3, false);
                    yield return sqlRow;
                }

                foreach (NoiseEliminationTicketDescription obj in this)
                {
                    sqlRow.SetString(0, obj.Keywords);
                    sqlRow.SetInt64(1, Convert.ToInt64(obj.Frequency) != 0 ? Convert.ToInt64(obj.Frequency) : 0);
                    sqlRow.SetBoolean(2, obj.IsActive != false ? obj.IsActive : false);
                    sqlRow.SetBoolean(3, obj.IsDeleted != false ? obj.IsDeleted : false);
                    yield return sqlRow;
                }

            }
        }
        /// <summary>
        /// NoiseEliminationResDataCollection
        /// </summary>
        public class NoiseEliminationResDataCollection : List<NoiseEliminationResolutionRemarks>,
            IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                      new SqlMetaData("OptionalFieldNoiseWord", SqlDbType.VarChar, 500),
                      new SqlMetaData("Frequency", SqlDbType.BigInt),
                      new SqlMetaData("IsActive", SqlDbType.Bit),
                      new SqlMetaData("IsDeleted", SqlDbType.Bit)
                     );
                if (this.Count == 0)
                {
                    sqlRow.SetString(0, string.Empty);
                    sqlRow.SetInt64(1, 0);
                    sqlRow.SetBoolean(2, false);
                    sqlRow.SetBoolean(3, false);
                    yield return sqlRow;
                }
                foreach (NoiseEliminationResolutionRemarks obj in this)
                {
                    sqlRow.SetString(0, obj.Keywords);
                    sqlRow.SetInt64(1, Convert.ToInt64(obj.Frequency) != 0 ? Convert.ToInt64(obj.Frequency) : 0);
                    sqlRow.SetBoolean(2, obj.IsActive != false ? obj.IsActive : false);
                    sqlRow.SetBoolean(3, obj.IsDeleted != false ? obj.IsDeleted : false);
                    yield return sqlRow;
                }

            }
        }
        /// <summary>
        /// This Method is used to SaveNoiseEliminationDetails
        /// </summary>
        /// <param name="NoiseData"></param>
        /// <param name="Projectid"></param>
        /// <param name="Choose"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public string SaveNoiseEliminationDetails(NoiseElimination NoiseData, int Projectid, int Choose,
            string EmployeeId)
        {
            string result = string.Empty;
            try
            {
                List<NoiseEliminationTicketDescription> objTicketDescriptionCollection =
                        new List<NoiseEliminationTicketDescription>();
                List<NoiseEliminationResolutionRemarks> objResolutionRemarksCollection =
                        new List<NoiseEliminationResolutionRemarks>();

                if (NoiseData.LstNoiseTicketDescription != null && NoiseData.LstNoiseTicketDescription.Any())
                {
                    for (int i = 0; i < NoiseData.LstNoiseTicketDescription.Count; i++)
                    {
                        objTicketDescriptionCollection.Add(new NoiseEliminationTicketDescription
                        {
                            Keywords = NoiseData.LstNoiseTicketDescription[i].Keywords,
                            Frequency = NoiseData.LstNoiseTicketDescription[i].Frequency,
                            IsActive = Choose == 1 ? true : NoiseData.LstNoiseTicketDescription[i].IsActive
                        });

                    }
                }
                else
                {
                    objTicketDescriptionCollection.Add(new NoiseEliminationTicketDescription
                    {
                        Keywords = string.Empty,
                        Frequency = "0",
                        IsActive = false,
                        IsDeleted = false
                    });
                }

                if (NoiseData.LstNoiseResolution != null && NoiseData.LstNoiseResolution.Any())
                {
                    for (int i = 0; i < NoiseData.LstNoiseResolution.Count; i++)
                    {
                        objResolutionRemarksCollection.Add(new NoiseEliminationResolutionRemarks
                        {
                            Keywords = NoiseData.LstNoiseResolution[i].Keywords,
                            Frequency = NoiseData.LstNoiseResolution[i].Frequency,
                            IsActive = Choose == 1 ? true : NoiseData.LstNoiseResolution[i].IsActive
                        });
                    }
                }
                else
                {
                    objResolutionRemarksCollection.Add(new NoiseEliminationResolutionRemarks
                    {
                        Keywords = string.Empty,
                        Frequency = "0",
                        IsActive = false,
                        IsDeleted = false
                    });
                }

                var objTicketCollection = from i in objTicketDescriptionCollection
                                          select new
                                          {

                                              TicketDesFieldNoiseWord = i.Keywords,
                                              Frequency = string.IsNullOrEmpty(i.Frequency) ? 0 : Convert.ToInt64(i.Frequency),
                                              IsActive = i.IsActive,
                                              IsDeleted = i.IsDeleted
                                          };

                var objRemarkCollection = from i in objResolutionRemarksCollection
                                          select new
                                          {
                                              OptionalFieldNoiseWord = i.Keywords,
                                              Frequency = string.IsNullOrEmpty(i.Frequency) ? 0 : Convert.ToInt64(i.Frequency),
                                              IsActive = i.IsActive,
                                              IsDeleted = i.IsDeleted
                                          };

                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@ProjectID", Projectid);
                prms[1] = new SqlParameter("@EmployeeID", EmployeeId);
                prms[2] = new SqlParameter("@lstTicketDescWordlist", objTicketCollection.ToList().ToDT());
                prms[2].SqlDbType = SqlDbType.Structured;
                prms[2].TypeName = TypeMLTicketDescWordList;
                prms[3] = new SqlParameter("@lstOptionalWordList", objRemarkCollection.ToList().ToDT());
                prms[3].SqlDbType = SqlDbType.Structured;
                prms[3].TypeName = TypeMLOptionalWordList;
                prms[4] = new SqlParameter("@Choose", Choose);
                DataTable dt = (new DBHelper()).GetTableFromSP("[dbo].[ML_SaveNoiseEliminationData]", prms, ConnectionString);

                if (dt != null)
                {
                    result = dt.Rows[0]["CriteriaMet"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;


        }
        /// <summary>
        /// This Method is used to GetFilteredNoiseEliminationData
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public NoiseElimination GetFilteredNoiseEliminationData(string Selection, int Filter, int ProjectID)
        {
            NoiseElimination objNoiseElimination = new NoiseElimination();
            DataTable dtnoiseticket = new DataTable();
            dtnoiseticket.Locale = CultureInfo.InvariantCulture;
            DataTable dtnoiseresolution = new DataTable();
            dtnoiseresolution.Locale = CultureInfo.InvariantCulture;
            var totaldese = 0;
            var totaldeseopt = 0;
            List<NoiseEliminationTicketDescription> lstticketdescription =
                new List<NoiseEliminationTicketDescription>();
            List<NoiseEliminationResolutionRemarks> lstResolution =
                new List<NoiseEliminationResolutionRemarks>();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@Selection", Selection);
            prms[1] = new SqlParameter("@Filter", Filter);
            prms[2] = new SqlParameter("@ProjectID", ProjectID);
            DataSet dt = (new DBHelper()).GetDatasetFromSP("[dbo].[ML_GetNoiseEliminationData]", prms, ConnectionString);
            if (dt != null)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    dtnoiseresolution = dt.Tables[0];
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    dtnoiseticket = dt.Tables[1];
                }
                if (dt.Tables[2].Rows.Count > 0)
                {
                    totaldeseopt = Convert.ToInt32(dt.Tables[2].Rows[0].ItemArray[0]);
                }
                if (dt.Tables[3].Rows.Count > 0)
                {
                    totaldese = Convert.ToInt32(dt.Tables[3].Rows[0].ItemArray[0]);
                }
                if (dtnoiseticket != null)
                {
                    for (int i = 0; i < dtnoiseticket.Rows.Count; i++)
                    {
                        NoiseEliminationTicketDescription objnoise = new NoiseEliminationTicketDescription();
                        objnoise.Keywords = (Convert.ToString(dtnoiseticket.Rows[i]["TicketDescNoiseWord"] !=
                            DBNull.Value
                            ? Convert.ToString(dtnoiseticket.Rows[i]["TicketDescNoiseWord"]) : string.Empty));
                        objnoise.Frequency = (Convert.ToString(dtnoiseticket.Rows[i]["Frequency"] != DBNull.Value
                            ? Convert.ToString(dtnoiseticket.Rows[i]["Frequency"]) : string.Empty));
                        objnoise.IsActive = (Convert.ToBoolean(dtnoiseticket.Rows[i]["Isactive"] != DBNull.Value
                            ? Convert.ToBoolean(dtnoiseticket.Rows[i]["Isactive"]) : true));
                        lstticketdescription.Add(objnoise);
                    }
                    objNoiseElimination.LstNoiseTicketDescription = lstticketdescription;
                }
                if (dtnoiseresolution != null)
                {
                    for (int i = 0; i < dtnoiseresolution.Rows.Count; i++)
                    {
                        NoiseEliminationResolutionRemarks objnoiseResolution =
                            new NoiseEliminationResolutionRemarks();
                        objnoiseResolution.Keywords = (Convert.ToString(dtnoiseresolution.
                            Rows[i]["OptionalFieldNoiseWord"] != DBNull.Value
                            ? Convert.ToString(dtnoiseresolution.Rows[i]["OptionalFieldNoiseWord"]) : string.Empty));
                        objnoiseResolution.Frequency = (Convert.ToString(dtnoiseresolution.
                            Rows[i]["Frequency"] != DBNull.Value
                            ? Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"]) : string.Empty));
                        objnoiseResolution.IsActive = (Convert.ToBoolean(dtnoiseresolution.
                            Rows[i]["Isactive"] != DBNull.Value
                            ? Convert.ToBoolean(dtnoiseresolution.Rows[i]["Isactive"]) : true));
                        lstResolution.Add(objnoiseResolution);
                    }
                    objNoiseElimination.LstNoiseResolution = lstResolution;
                }
            }
            objNoiseElimination.TotalDesc = totaldese;
            objNoiseElimination.TotalOpt = totaldeseopt;
            return objNoiseElimination;

        }
        /// <summary>
        /// This Method is used to GetIconDetails
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IconDetails GetIconDetails(string Choose, int ProjectId, int SupportID)
        {
            DataTable dt;
            IconDetails objIconDetails = new IconDetails();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@Choice", Choose);
            prms[1] = new SqlParameter("@ProjectID", ProjectId);
            if (SupportID == 1)
            {
                dt = (new DBHelper()).GetTableFromSP("[dbo].[ML_GetIconDetailsByChoice]", prms, ConnectionString);
            }
            else
            {
                dt = (new DBHelper()).GetTableFromSP("[AVL].[MLInfraGetIconDetailsByChoice]", prms, ConnectionString);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                objIconDetails.TicketAnalysed = Convert.ToInt32(dt.Rows[0]["TicketAnalysed"]);
                objIconDetails.TicketConsidered = Convert.ToInt32(dt.Rows[0]["TicketConsidered"]);
                objIconDetails.SamplingCount = Convert.ToInt32(dt.Rows[0]["SamplingCount"]);
                objIconDetails.PatternCount = Convert.ToInt32(dt.Rows[0]["PatternCount"]);
                objIconDetails.ApprovedCount = Convert.ToInt32(dt.Rows[0]["ApprovedCount"]);
                objIconDetails.MuteCount = Convert.ToInt32(dt.Rows[0]["MuteCount"]);
            }
            return objIconDetails;
        }


        /// <summary>
        /// This Method is used to CreateInitialLearningID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public string CreateInitialLearningID(int ProjectID, string employeeID)
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@EmployeeID", employeeID);
            DataSet ds;
            try
            {
                ds = (new DBHelper()).GetDatasetFromSP("AVL.ML_CreateInitialLearningIDForRegenerate", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Flag;

        }
        /// <summary>
        /// Method to Validate ML
        /// </summary>
        /// <param name="CriteriaMet"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        public ILValidationResult ValidateML(string CriteriaMet, Int64 ProjectID, string UserID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            Utility.ErrorLOGInfra("5 .1. in validate ML ",
         "CriteriaMet =" + CriteriaMet, Convert.ToInt32(ProjectID));
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                switch (CriteriaMet)
                {
                    case ApplicationConstants.MultiLingual_Flag:
                        result = ProcessMultiLingualResult();
                        break;
                    case ApplicationConstants.ML_Flag:
                        result = ProcessMLResult(Convert.ToInt32(ProjectID), UserID, StartDate, EndDate, OptionalFieldProj,
                            SupportTypeID);
                        break;
                    case ApplicationConstants.Sampling_Flag:
                        result = ProcessSamplingResult();
                        break;
                    case ApplicationConstants.Excel_Flag:
                    case ApplicationConstants.OExcel_Flag:
                    case ApplicationConstants.TExcel_Flag:
                        result = ProcessExcelResult();
                        result.ILMessage = CriteriaMet;
                        break;
                    case ApplicationConstants.Noise_Flag:
                        result = ProcessNoiseResult(ProjectID, UserID, StartDate, EndDate, OptionalFieldProj,
                            SupportTypeID);
                        break;
                    case ApplicationConstants.NotEnough_Flag:
                        result = ProcessNotEnoughResult();
                        break;
                    case ApplicationConstants.ConstantN:
                        result = ProcessNotAutoClassifiedResult();
                        break;
                    default:
                        result.IsError = true;
                        result.ILMessage = ApplicationConstants.DefaultErrorMessage;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.ErrorLOGInfra("5 .1. in validate ML ",
         "ex IS =" + ex.Message, Convert.ToInt32(ProjectID));

                result = null;
                throw ex;
            }
            return result;

        }
        /// <summary>
        /// processMultiLingualResult - function to handle the MultiLingual Result
        /// </summary>
        /// <returns></returns>
        private ILValidationResult ProcessMultiLingualResult()
        {
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                result.ILMessage = ApplicationConstants.MultiLingualMessage;
                result.ILMessageKey = ApplicationConstants.MultiLingualMessageKey;
                result.ILValidationResultCode = 1;
                result.ProgressbarMessage = new ILProgressBarMessage();
                result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(1,
                    ApplicationConstants.ConstantLevel1,
                    ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel2,
                    ApplicationConstants.ConstantNoiseElimination, false, "Not Started",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel3,
                    ApplicationConstants.ConstantSampling, false, "Not Started",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel4,
                    ApplicationConstants.ConstantML, false, "Not Started",
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel5,
                    ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                    ApplicationConstants.ConstantLevel5);
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// ProcessMLResult - This function will do all the stuffs once the validation result equals to ML
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        private ILValidationResult ProcessMLResult(Int32 ProjectID, string UserID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            ILValidationResult result;
            GetMLDetails UpdateInitialLearningStateDetails = null;
            try
            {
                result = new ILValidationResult();

                if (MLDatSetBindingForCSVCreation(ProjectID, UserID, SupportTypeID) == ApplicationConstants.ConstantY)
                {
                    UpdateInitialLearningStateDetails = MLUpdateInitialLearningStateDetails(ProjectID, UserID,
                        StartDate, EndDate, OptionalFieldProj, SupportTypeID).FirstOrDefault();
                    if (UpdateInitialLearningStateDetails != null
                        && UpdateInitialLearningStateDetails.MLStatus == "Sent")
                    {
                        //TODO - Bind the Start DATE and End DATE in DatePickers
                        result.ILMessage = ApplicationConstants.MLMessage;
                        result.ILMessageKey = ApplicationConstants.MLMessageKey;
                        result.ILValidationResultCode = 2;
                        result.ProgressbarMessage = new ILProgressBarMessage();
                        result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                            ApplicationConstants.ConstantLevel1,
                   ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                   ApplicationConstants.ConstantSuccess);
                        result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                            ApplicationConstants.ConstantLevel2,
                            ApplicationConstants.ConstantNoiseElimination, true, DateTimeOffset.Now.DateTime,
                            ApplicationConstants.ConstantPending);
                        result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                            ApplicationConstants.ConstantLevel3,
                            ApplicationConstants.ConstantSampling, false, "Not Started",
                            ApplicationConstants.ConstantSuccess);
                        result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(1,
                            ApplicationConstants.ConstantLevel4,
                            ApplicationConstants.ConstantML, false, "Not Started",
                            ApplicationConstants.ConstantPending);
                        result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                            ApplicationConstants.ConstantLevel5,
                            ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                            ApplicationConstants.ConstantLevel5);
                    }
                    result.ProgressbarMessage = new ILProgressBarMessage();
                    result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                       ApplicationConstants.ConstantLevel1,
                       ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                       ApplicationConstants.ConstantSuccess);
                    result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                        ApplicationConstants.ConstantLevel2,
                        ApplicationConstants.ConstantNoiseElimination, true, DateTimeOffset.Now.DateTime,
                        ApplicationConstants.ConstantSuccess);
                    result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                        ApplicationConstants.ConstantLevel3,
                        ApplicationConstants.ConstantSampling, true, DateTimeOffset.Now.DateTime,
                        ApplicationConstants.ConstantSuccess);
                    result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(1,
                        ApplicationConstants.ConstantLevel4,
                        ApplicationConstants.ConstantML, true, DateTimeOffset.Now.DateTime,
                        ApplicationConstants.ConstantPending);
                    result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                        ApplicationConstants.ConstantLevel5,
                        ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                        ApplicationConstants.ConstantLevel5);

                }
                else
                {
                    result.IsError = true;
                    result.ILMessage = ApplicationConstants.DefaultErrorMessage;
                    result.ILMessageKey = ApplicationConstants.DefaultErrorMessageKey;
                }

            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// ProcessSamplingResult - function used to set the message once the ML validation result is Sampling
        /// </summary>
        /// <returns></returns>
        private ILValidationResult ProcessSamplingResult()
        {
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                result.ILMessage = ApplicationConstants.SamplingMessage;
                result.ILMessageKey = ApplicationConstants.SamplingMessageKey;
                result.ILValidationResultCode = 3;
                result.ProgressbarMessage = new ILProgressBarMessage();
                result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                    ApplicationConstants.ConstantLevel1,
                  ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                  ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                    ApplicationConstants.ConstantLevel2,
                    ApplicationConstants.ConstantNoiseElimination, true, DateTimeOffset.Now.DateTime,
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(1,
                    ApplicationConstants.ConstantLevel3,
                    ApplicationConstants.ConstantSampling, true, DateTimeOffset.Now.DateTime,
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel4,
                    ApplicationConstants.ConstantML, false, "Not Started",
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel5,
                    ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                    ApplicationConstants.ConstantLevel5);
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// funcion to validate when the MLvalidationResult is Excel
        /// </summary>
        /// <returns></returns>
        private ILValidationResult ProcessExcelResult()
        {
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                result.ILValidationResultCode = 4;
                result.ProgressbarMessage = new ILProgressBarMessage();
                result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(1, ApplicationConstants.ConstantLevel1,
                    ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(-1, ApplicationConstants.ConstantLevel2,
                    ApplicationConstants.ConstantNoiseElimination, false, "Not Started",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1, ApplicationConstants.ConstantLevel3,
                    ApplicationConstants.ConstantSampling, false, "Not Started",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1, ApplicationConstants.ConstantLevel4,
                    ApplicationConstants.ConstantML, false, "Not Started",
                    ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1, ApplicationConstants.ConstantLevel5,
                    ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                    ApplicationConstants.ConstantLevel5);
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// function to validate the noiseresult
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        private ILValidationResult ProcessNoiseResult(Int64 ProjectID, string UserID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj, int SupportTypeID)
        {
            Utility.ErrorLOGInfra("5 .1. in ProcessNoiseResult ML ",
       "ProjectID IS =" + ProjectID, Convert.ToInt32(ProjectID));

            ILValidationResult result;
            GetMLDetails UpdateNoiseEliminationFlagDetails = null;
            try
            {
                result = new ILValidationResult();
                if (!string.IsNullOrEmpty(GetTicketsForNoiseElimination(Convert.ToString(ProjectID), UserID,
                    SupportTypeID)))
                {
                    Utility.ErrorLOGInfra("5 .1. in ProcessNoiseResult  AFTER GET tICKETS ML ",
                     "ProjectID IS =" + ProjectID, Convert.ToInt32(ProjectID));
                    UpdateNoiseEliminationFlagDetails = UpdateNoiseEliminationFlag(ProjectID, UserID, StartDate,
                        EndDate, OptionalFieldProj, SupportTypeID).FirstOrDefault();
                    if (UpdateNoiseEliminationFlagDetails.NoiseEliminationSent == ApplicationConstants.DefaultSent)
                    {
                        result.ILMessage = ApplicationConstants.NoiseMessage;
                        result.ILMessageKey = ApplicationConstants.NoiseMessageKey;

                    }
                    result.ILValidationResultCode = 5;
                    result.ProgressbarMessage = new ILProgressBarMessage();
                    result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                        ApplicationConstants.ConstantLevel1,
                     ApplicationConstants.ConstantDataAvailablity, true, DateTimeOffset.Now.DateTime,
                     ApplicationConstants.ConstantSuccess);
                    result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(1,
                        ApplicationConstants.ConstantLevel2,
                        ApplicationConstants.ConstantNoiseElimination, true, DateTimeOffset.Now.DateTime,
                        ApplicationConstants.ConstantPending);
                    result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1,
                        ApplicationConstants.ConstantLevel3,
                        ApplicationConstants.ConstantSampling, false, "Not Started",
                        ApplicationConstants.ConstantSuccess);
                    result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                        ApplicationConstants.ConstantLevel4,
                        ApplicationConstants.ConstantML, false, "Not Started",
                        ApplicationConstants.ConstantPending);
                    result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                        ApplicationConstants.ConstantLevel5,
                        ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                        ApplicationConstants.ConstantLevel5);
                }
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Displays when the result is Not Enough
        /// </summary>
        /// <returns></returns>
        private ILValidationResult ProcessNotEnoughResult()
        {
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                result.ILMessage = ApplicationConstants.NotEnoughMessage;
                result.ILMessageKey = ApplicationConstants.NotEnoughMessageKey;
                result.ILValidationResultCode = 6;
                result.ProgressbarMessage = new ILProgressBarMessage();
                result.ProgressbarMessage.Level1 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel1,
                      ApplicationConstants.ConstantDataAvailablity, false, "",
                      ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level2 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel2,
                    ApplicationConstants.ConstantNoiseElimination, false, "",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel3,
                    ApplicationConstants.ConstantSampling, false, "",
                    ApplicationConstants.ConstantSuccess);
                result.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel4,
                    ApplicationConstants.ConstantML, false, "", ApplicationConstants.ConstantPending);
                result.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                    ApplicationConstants.ConstantLevel5,
                    ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                    ApplicationConstants.ConstantLevel5);
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Displays when the project is not autoclassfied
        /// </summary>
        /// <returns></returns>
        private ILValidationResult ProcessNotAutoClassifiedResult()
        {
            ILValidationResult result;
            try
            {
                result = new ILValidationResult();
                result.ILMessage = ApplicationConstants.NotEnoughMessage;
                result.ILMessageKey = ApplicationConstants.NotEnoughMessageKey;
                result.ILValidationResultCode = 7;
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Method to ConstructProgressBarMessage
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="level"></param>
        /// <param name="stage"></param>
        /// <param name="isDate"></param>
        /// <param name="displaydate"></param>
        /// <param name="status"></param>
        /// <param name="extrastatus"></param>
        /// <returns></returns>
        private string ConstructProgressBarMessage(int symbol, string level, string stage, bool isDate,
            object displaydate, string status, string extrastatus = "")
        {
            StringBuilder result = new StringBuilder();
            DateTime displaydateresult;
            try
            {

                if (symbol == 1)
                {
                    result.Append(ApplicationConstants.ConstantQuestionMark);
                }
                else if (symbol == 2)
                {
                    result.Append(ApplicationConstants.ConstantPercentage);
                }
                else
                {
                    //mandatory else
                }

                result.Append(level);
                result.Append(ApplicationConstants.TriperHash);
                result.Append(stage);
                result.Append(ApplicationConstants.TriperHash);
                if (!isDate)
                {
                    result.Append(Convert.ToString(displaydate));
                }
                else
                {
                    if (DateTime.TryParse(Convert.ToString(displaydate), out displaydateresult))
                    {
                        result.Append(displaydateresult.Month.ToString());
                        result.Append(ApplicationConstants.Constantbacklash);
                        result.Append(displaydateresult.Day.ToString());
                        result.Append(ApplicationConstants.Constantbacklash);
                        result.Append(displaydateresult.Year.ToString());
                    }
                }
                result.Append(ApplicationConstants.TriperHash);
                result.Append(status);
                if (!string.IsNullOrEmpty(extrastatus))
                {
                    result.Append(ApplicationConstants.TriperHash);
                    result.Append(extrastatus);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.ToString();
        }

        /// <summary>
        /// ReWrite Module for Jquery Method GetMLDetailsOnLoad
        /// </summary>
        /// <param name="MLDetails"></param>
        public ILValidationResult MLDetailsLoad(List<GetMLDetails> MLDetails)
        {
            //TODO Items
            //Need to globally set the ddlOptionalFieldByProject & IsApprovedOrMute Identification

            ILValidationResult message = new ILValidationResult();
            try
            {
                if (MLDetails != null && MLDetails.Count > 0)
                {
                    if (MLDetails[0].IsAutoClassified == ApplicationConstants.ConstantN)
                    {
                        //IN UI - We need to hide the SearchFilter for this result
                        message = ProcessNotAutoClassifiedResult();
                    }
                    else
                    {
                        //In case if the Regeneated SignOff is not done , then consider the entire SignOff is 0
                        if (MLDetails[0].RegenerateCount > 0 && MLDetails[0].IsRegMLsignOff == 0)
                        {
                            //IN UI - show the patternsView and call ViewALL()
                            MLDetails[0].MLSignoff = 0;
                            message.ProgressbarMessage = null;
                        }
                        if (MLDetails[0].MLSignoff == 1)
                        {
                            //IN UI - show the approvemutediv
                            message.ILValidationResultCode = 8;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1, ApplicationConstants.ConstantDataAvailablity,
                                true, MLDetails[0].DataValidationDate, ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel2, ApplicationConstants.ConstantNoiseElimination,
                                true, MLDetails[0].NoiseSentDate, ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel3, ApplicationConstants.ConstantSampling,
                                true, MLDetails[0].SamplingSentDate, ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel4, ApplicationConstants.ConstantML,
                                true, MLDetails[0].MLSentDate, ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel5, "MLPatternApproval",
                                true, MLDetails[0].AutoclassificationDatestring, ApplicationConstants.ConstantSuccess);
                        }
                        if (!string.IsNullOrEmpty(MLDetails[0].ErrorMessage))
                        {
                            message.IsError = true;
                            message.ILMessage = MLDetails[0].ErrorMessage;
                        }
                        else if (MLDetails[0].MLStatus == ApplicationConstants.DefaultSent)
                        {
                            message.ILMessage = ApplicationConstants.MLMessage;
                            message.ILMessageKey = ApplicationConstants.MLMessageKey;
                            message.ILValidationResultCode = 13;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].SamplingSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MLSentDate,
                                ApplicationConstants.ConstantPending);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel5,
                                ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                                ApplicationConstants.ConstantLevel5);
                        }
                        else if (MLDetails[0].NoiseEliminationSent == ApplicationConstants.DefaultSent)
                        {
                            message.ILValidationResultCode = 9;
                            message.ILMessage = ApplicationConstants.NoiseMessage;
                            message.ILMessageKey = ApplicationConstants.NoiseMessageKey;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantPending);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].MLSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MLSentDate,
                                ApplicationConstants.ConstantPending);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel5,
                                ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                                ApplicationConstants.ConstantLevel5);
                        }
                        else if (MLDetails[0].NoiseEliminationSent == ApplicationConstants.constantSaved)
                        {
                            message.ILValidationResultCode = 10;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantPending);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].MLSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MLSentDate,
                                ApplicationConstants.ConstantPending);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel5,
                                ApplicationConstants.ConstantNA, false, ApplicationConstants.ConstantNA,
                                ApplicationConstants.ConstantLevel5);
                        }
                        else if (MLDetails[0].MLStatus == ApplicationConstants.constantReceived
                            && MLDetails[0].MLSignoff != 1)
                        {
                            message.ILValidationResultCode = 8;
                            message.ILMessage = ApplicationConstants.MLMessage;
                            message.ILMessageKey = ApplicationConstants.MLMessageKey;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].SamplingSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MlReceiveddate,
                                ApplicationConstants.ConstantSuccess, ApplicationConstants.ConstantLevel4);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel5,
                                "MLPattern Approval", true, MLDetails[0].MlReceiveddate,
                                ApplicationConstants.ConstantPending);
                        }
                        else if (MLDetails[0].SamplingSentOrReceivedStatus == ApplicationConstants.DefaultSent
                            && MLDetails[0].MLSignoff != 1)
                        {
                            message.ILValidationResultCode = 11;
                            message.ILMessage = ApplicationConstants.SamplingUnderProcessMessage;
                            message.ILMessageKey = ApplicationConstants.SamplingUnderProcessMessageKey;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].SamplingSentDate,
                                ApplicationConstants.ConstantPending, ApplicationConstants.ConstantLevel3);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MlReceiveddate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel5,
                               ApplicationConstants.ConstantNA, true, MLDetails[0].MlReceiveddate,
                               ApplicationConstants.ConstantPending);
                        }
                        else if (MLDetails[0].SamplingSentOrReceivedStatus == ApplicationConstants.constantReceived
                             && MLDetails[0].MLSignoff != 1)
                        {
                            message.ILValidationResultCode = 11;
                            message.ILMessage = ApplicationConstants.SamplingUnderProcessMessage;
                            message.ILMessageKey = ApplicationConstants.SamplingUnderProcessMessageKey;
                            message.ProgressbarMessage = new ILProgressBarMessage();
                            message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel1,
                                ApplicationConstants.ConstantDataAvailablity, true, MLDetails[0].DataValidationDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                ApplicationConstants.ConstantLevel2,
                                ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(1,
                                ApplicationConstants.ConstantLevel3,
                                ApplicationConstants.ConstantSampling, true, MLDetails[0].SamplingSentDate,
                                ApplicationConstants.ConstantPending, ApplicationConstants.ConstantLevel3);
                            message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel4,
                                ApplicationConstants.ConstantML, true, MLDetails[0].MlReceiveddate,
                                ApplicationConstants.ConstantSuccess);
                            message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                ApplicationConstants.ConstantLevel5,
                               ApplicationConstants.ConstantNA, true, MLDetails[0].MlReceiveddate,
                               ApplicationConstants.ConstantPending);
                            if (MLDetails[0].SamplingInProgressStatus == ApplicationConstants.constantSaved)
                            {
                                message.ILValidationResultCode = 12;

                            }
                            else if (MLDetails[0].SamplingInProgressStatus == ApplicationConstants.constantSubmitted)
                            {
                                message.ILValidationResultCode = 13;
                                message.ILMessage = ApplicationConstants.MLUnderProcessSceenMessage;
                                message.ILMessageKey = ApplicationConstants.MLUnderProcessSceenMessageKey;
                                message.ProgressbarMessage = new ILProgressBarMessage();
                                message.ProgressbarMessage.Level1 = ConstructProgressBarMessage(2,
                                    ApplicationConstants.ConstantLevel1,
                                    ApplicationConstants.ConstantDataAvailablity, true,
                                    MLDetails[0].DataValidationDate,
                                    ApplicationConstants.ConstantSuccess);
                                message.ProgressbarMessage.Level2 = ConstructProgressBarMessage(2,
                                    ApplicationConstants.ConstantLevel2,
                                    ApplicationConstants.ConstantNoiseElimination, true, MLDetails[0].NoiseSentDate,
                                    ApplicationConstants.ConstantSuccess);
                                message.ProgressbarMessage.Level3 = ConstructProgressBarMessage(2,
                                    ApplicationConstants.ConstantLevel3,
                                    ApplicationConstants.ConstantSampling, true, MLDetails[0].SamplingSentDate,
                                    ApplicationConstants.ConstantSuccess, ApplicationConstants.ConstantLevel3);
                                message.ProgressbarMessage.Level4 = ConstructProgressBarMessage(-1,
                                    ApplicationConstants.ConstantLevel4,
                                    ApplicationConstants.ConstantML, true, MLDetails[0].MlReceiveddate,
                                    ApplicationConstants.ConstantSuccess);
                                message.ProgressbarMessage.Level5 = ConstructProgressBarMessage(-1,
                                    ApplicationConstants.ConstantLevel5,
                                   ApplicationConstants.ConstantNA, true, MLDetails[0].MlReceiveddate,
                                   ApplicationConstants.ConstantPending);
                            }
                            else
                            {
                                message.ILValidationResultCode = 12;

                            }
                        }
                        else
                        {
                            //mandatory else
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = null;
                throw ex;
            }
            return message;
        }
        /// <summary>
        /// This Method is used to Get FilteredNoiseEliminationData for Infra
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public NoiseEliminationInfra GetFilteredNoiseEliminationDataInfra(string Selection, int Filter, int ProjectID)
        {
            NoiseEliminationInfra objNoiseElimination = new NoiseEliminationInfra();
            DataTable dtnoiseticket = new DataTable();
            dtnoiseticket.Locale = CultureInfo.InvariantCulture;
            DataTable dtnoiseresolution = new DataTable();
            dtnoiseresolution.Locale = CultureInfo.InvariantCulture;
            var totaldese = 0;
            var totaldeseopt = 0;
            List<NoiseEliminationTicketDescription> lstticketdescription =
                new List<NoiseEliminationTicketDescription>();
            List<NoiseEliminationResolutionRemarks> lstResolution =
                new List<NoiseEliminationResolutionRemarks>();
            SqlParameter[] prms = new SqlParameter[4];
            prms[0] = new SqlParameter("@Selection", Selection);
            prms[1] = new SqlParameter("@Filter", Filter);
            prms[2] = new SqlParameter("@ProjectID", ProjectID);
            prms[3] = new SqlParameter("@Mode", "Infra");
            DataSet dt = (new DBHelper()).GetDatasetFromSP("[dbo].[ML_GetNoiseEliminationData_Infra]", prms, ConnectionString);
            if (dt != null)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    dtnoiseresolution = dt.Tables[0];
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    dtnoiseticket = dt.Tables[1];
                }
                if (dt.Tables[2].Rows.Count > 0)
                {
                    totaldeseopt = Convert.ToInt32(dt.Tables[2].Rows[0].ItemArray[0]);
                }
                if (dt.Tables[3].Rows.Count > 0)
                {
                    totaldese = Convert.ToInt32(dt.Tables[3].Rows[0].ItemArray[0]);
                }
                if (dtnoiseticket != null)
                {
                    for (int i = 0; i < dtnoiseticket.Rows.Count; i++)
                    {
                        NoiseEliminationTicketDescription objnoise = new NoiseEliminationTicketDescription();
                        objnoise.Keywords = (Convert.ToString(dtnoiseticket.Rows[i]["TicketDescNoiseWord"] !=
                            DBNull.Value
                            ? Convert.ToString(dtnoiseticket.Rows[i]["TicketDescNoiseWord"]) : string.Empty));
                        objnoise.Frequency = (Convert.ToString(dtnoiseticket.Rows[i]["Frequency"] != DBNull.Value
                            ? Convert.ToString(dtnoiseticket.Rows[i]["Frequency"]) : string.Empty));
                        objnoise.IsDeleted = (Convert.ToBoolean(dtnoiseticket.Rows[i]["IsDeleted"] != DBNull.Value
                            ? Convert.ToBoolean(dtnoiseticket.Rows[i]["IsDeleted"]) : true));
                        lstticketdescription.Add(objnoise);
                    }
                    objNoiseElimination.LstNoiseTicketDescriptionInfra = lstticketdescription;
                }
                if (dtnoiseresolution != null)
                {
                    for (int i = 0; i < dtnoiseresolution.Rows.Count; i++)
                    {
                        NoiseEliminationResolutionRemarks objnoiseResolution =
                            new NoiseEliminationResolutionRemarks();
                        objnoiseResolution.Keywords = (Convert.ToString(dtnoiseresolution.
                            Rows[i]["OptionalFieldNoiseWord"] != DBNull.Value
                            ? Convert.ToString(dtnoiseresolution.Rows[i]["OptionalFieldNoiseWord"]) : string.Empty));
                        objnoiseResolution.Frequency = (Convert.ToString(dtnoiseresolution.
                            Rows[i]["Frequency"] != DBNull.Value
                            ? Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"]) : string.Empty));
                        objnoiseResolution.IsDeleted = (Convert.ToBoolean(dtnoiseresolution.
                            Rows[i]["IsDeleted"] != DBNull.Value
                            ? Convert.ToBoolean(dtnoiseresolution.Rows[i]["IsDeleted"]) : true));
                        lstResolution.Add(objnoiseResolution);
                    }
                    objNoiseElimination.LstNoiseResolutionInfra = lstResolution;
                }
            }
            objNoiseElimination.TotalDesc = totaldese;
            objNoiseElimination.TotalOpt = totaldeseopt;
            return objNoiseElimination;

        }

        /// <summary>
        /// To get noise data and save to Infra table in DB
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="NoiseEliminationJobId">NoiseElimination JobId</param>
        /// <returns></returns>
        public NoiseEliminationInfra GetNoiseEliminationInfraData(int projectID, string NoiseEliminationJobId)
        {
            NoiseEliminationInfra noiseEliminationDetails = new NoiseEliminationInfra();
            noiseEliminationDetails = CheckIfNoiseOutputFileGeneratedInfra(projectID, NoiseEliminationJobId, 2);
            if (noiseEliminationDetails != null && noiseEliminationDetails.LstNoiseTicketDescriptionInfra.Count > 0)
            {
                SaveNoiseEliminationInfraDetails(noiseEliminationDetails, projectID, 1, "System");
            }
            return noiseEliminationDetails;

        }


        /// <summary>
        /// To check if noise file is generated for Infra
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="NoiseEliminationJobId">Noise Elimination Job Id </param>
        /// <returns></returns>
        public NoiseEliminationInfra CheckIfNoiseOutputFileGeneratedInfra(int ProjectID, string NoiseEliminationJobId,
            int SupportId)
        {
            NoiseEliminationInfra objNoiseElimination = new NoiseEliminationInfra();

            DataTable dtnoiseticket = new DataTable();
            dtnoiseticket.Locale = CultureInfo.InvariantCulture;
            DataTable dtnoiseresolution = new DataTable();
            dtnoiseresolution.Locale = CultureInfo.InvariantCulture;
            List<NoiseEliminationTicketDescription> lstticketdescription =
                new List<NoiseEliminationTicketDescription>();
            List<NoiseEliminationResolutionRemarks> lstResolution =
                new List<NoiseEliminationResolutionRemarks>();
            string output = string.Empty;
            string HiveFilepathdesc = string.Empty;
            string HiveFilepathopt = string.Empty;
            string ErrorPath = string.Empty;
            bool PresenceOfOptField;
            FilePathNoiseEl objFilePath = new FilePathNoiseEl();
            DataTable dtErrordata = new DataTable();
            dtErrordata.Locale = CultureInfo.InvariantCulture;
            objFilePath = GetHivePathnameByJobIdForInfraNoiseEl(NoiseEliminationJobId);
            HiveFilepathdesc = objFilePath.OutputPathDesc;
            HiveFilepathopt = objFilePath.OutputPathOpt;
            PresenceOfOptField = objFilePath.PresenceOfOptField;
            ErrorPath = objFilePath.ErrorPath;
            string errorData = string.Empty;
            string DownloadPath = new AppSettings().AppsSttingsKeyValues["DownloadTempPath"];

            //Checking for Error Output File
            string FileErrorOutputPresent = Utility.CheckIfFileExists(ErrorPath, DownloadPath, SupportId);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtErrordata = Utility.GetDataTabletFromCSVFile(FileErrorOutputPresent);
                try
                {
                    if (dtErrordata != null)
                    {
                        errorData = Convert.ToString(dtErrordata.Rows[0][1]);
                        if (string.Compare(errorData, FileStatSuccess) != 0)
                        {
                            output = UpdateErrorStatus(ProjectID, NoiseEliminationJobId, errorData, 2, SupportId);
                        }
                        else if (string.Compare(errorData, FileStatSuccess) == 0)
                        {
                            output = UpdateErrorStatus(ProjectID, NoiseEliminationJobId, errorData, 1, SupportId);
                        }
                        else
                        {
                            //mandatory else
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //Checking for Output File

            string FileOutputPresentdesc = Utility.CheckIfFileExists(HiveFilepathdesc, DownloadPath, SupportId);
            string FileOutputPresentOpt = Utility.CheckIfFileExists(HiveFilepathopt, DownloadPath, SupportId);
            if (string.Compare(FileErrorOutputPresent, FileStatNotFound) != 0)
            {
                dtnoiseticket = Utility.GetDataTabletFromCSVFile(FileOutputPresentdesc);
                if (PresenceOfOptField)
                {

                    dtnoiseresolution = Utility.GetDataTabletFromCSVFile(FileOutputPresentOpt);
                }


            }


            if (dtnoiseticket != null)
            {
                for (int i = 0; i < dtnoiseticket.Rows.Count; i++)
                {
                    NoiseEliminationTicketDescription objnoise = new NoiseEliminationTicketDescription();
                    objnoise.Keywords = (Convert.ToString(dtnoiseticket.Rows[i]["Word"] != DBNull.Value ?
                        Convert.ToString(dtnoiseticket.Rows[i]["Word"]) : string.Empty));
                    objnoise.Frequency = (Convert.ToString(dtnoiseticket.Rows[i]["Frequency"] != DBNull.Value ?
                        Convert.ToString(dtnoiseticket.Rows[i]["Frequency"]) : string.Empty));
                    objnoise.IsDeleted = false;
                    lstticketdescription.Add(objnoise);
                }
                objNoiseElimination.LstNoiseTicketDescriptionInfra = lstticketdescription;
            }
            if (dtnoiseresolution != null)
            {
                for (int i = 0; i < dtnoiseresolution.Rows.Count; i++)
                {
                    NoiseEliminationResolutionRemarks objnoiseResolution = new NoiseEliminationResolutionRemarks();
                    objnoiseResolution.Keywords = (Convert.ToString(dtnoiseresolution.Rows[i]["Word"] !=
                        DBNull.Value ? Convert.ToString(dtnoiseresolution.Rows[i]["Word"]) : string.Empty));
                    objnoiseResolution.Frequency = (Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"] !=
                        DBNull.Value ? Convert.ToString(dtnoiseresolution.Rows[i]["Frequency"]) : string.Empty));
                    objnoiseResolution.IsDeleted = false;
                    lstResolution.Add(objnoiseResolution);
                }
                objNoiseElimination.LstNoiseResolutionInfra = lstResolution;
            }
            return objNoiseElimination;
        }

        /// <summary>
        /// get hive path details for  noise elimination
        /// </summary>
        /// <param name="NoiseEliminationJobId">Noise Elimination JobId</param>
        /// <returns></returns>
        public FilePathNoiseEl GetHivePathnameByJobIdForInfraNoiseEl(string NoiseEliminationJobId)
        {
            FilePathNoiseEl objFilePath = new FilePathNoiseEl();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@NoiseEliminationJobId", NoiseEliminationJobId);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("ML_GetNoiseOutPutInfrapath", prms, ConnectionString);
                if (ds != null)
                {
                    objFilePath.OutputPathDesc = ds.Tables[0].Rows[0]["HiveDataPathDesc"].ToString();
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["PresenceOfOptField"].ToString()))
                    {

                        objFilePath.OutputPathOpt = ds.Tables[0].Rows[0]["HiveDataPathOpt"].ToString();
                    }

                    objFilePath.PresenceOfOptField = Convert.ToBoolean(ds.Tables[0].Rows[0]["PresenceOfOptField"].
                        ToString());
                    objFilePath.ErrorPath = ds.Tables[0].Rows[0]["FileErrorPath"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objFilePath;
        }

        /// <summary>
        /// This Method is used to Save NoiseElimination Infra Details in Infra Table
        /// </summary>
        /// <param name="NoiseData"></param>
        /// <param name="Projectid"></param>
        /// <param name="Choose"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public string SaveNoiseEliminationInfraDetails(NoiseEliminationInfra NoiseData, int Projectid, int Choose,
            string EmployeeId)
        {
            string result = string.Empty;
            try
            {
                List<NoiseEliminationTicketDescription> objTicketDescriptionCollection =
                        new List<NoiseEliminationTicketDescription>();
                List<NoiseEliminationResolutionRemarks> objResolutionRemarksCollection =
                        new List<NoiseEliminationResolutionRemarks>();
                if (NoiseData.LstNoiseTicketDescriptionInfra != null && NoiseData.LstNoiseTicketDescriptionInfra.Any())
                {
                    for (int i = 0; i < NoiseData.LstNoiseTicketDescriptionInfra.Count; i++)
                    {
                        objTicketDescriptionCollection.Add(new NoiseEliminationTicketDescription
                        {
                            Keywords = NoiseData.LstNoiseTicketDescriptionInfra[i].Keywords,
                            Frequency = NoiseData.LstNoiseTicketDescriptionInfra[i].Frequency,
                            IsDeleted = Choose == 1 ? false : NoiseData.LstNoiseTicketDescriptionInfra[i].IsDeleted
                        });

                    }
                }
                else
                {
                    objTicketDescriptionCollection.Add(new NoiseEliminationTicketDescription
                    {
                        Keywords = string.Empty,
                        Frequency = "0",
                        IsDeleted = false,
                        IsActive = false
                    });
                }

                if (NoiseData.LstNoiseResolutionInfra != null && NoiseData.LstNoiseResolutionInfra.Any())
                {
                    for (int i = 0; i < NoiseData.LstNoiseResolutionInfra.Count; i++)
                    {
                        objResolutionRemarksCollection.Add(new NoiseEliminationResolutionRemarks
                        {
                            Keywords = NoiseData.LstNoiseResolutionInfra[i].Keywords,
                            Frequency = NoiseData.LstNoiseResolutionInfra[i].Frequency,
                            IsDeleted = Choose == 1 ? false : NoiseData.LstNoiseResolutionInfra[i].IsDeleted
                        });

                    }
                }

                var objTicketCollection = from i in objTicketDescriptionCollection
                                          select new
                                          {
                                              TicketDesFieldNoiseWord = i.Keywords,
                                              Frequency = string.IsNullOrEmpty(i.Frequency) ? 0 : Convert.ToInt64(i.Frequency),
                                              IsActive = i.IsActive,
                                              IsDeleted = i.IsDeleted
                                          };

                var objRemarkCollection = from i in objResolutionRemarksCollection
                                          select new
                                          {
                                              OptionalFieldNoiseWord = i.Keywords,
                                              Frequency = string.IsNullOrEmpty(i.Frequency) ? 0 : Convert.ToInt64(i.Frequency),
                                              IsActive = i.IsActive,
                                              IsDeleted = i.IsDeleted
                                          };


                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@ProjectID", Projectid);
                prms[1] = new SqlParameter("@EmployeeID", EmployeeId);
                prms[2] = new SqlParameter("@lstTicketDescWordlist", objTicketCollection.ToList().ToDT());
                prms[2].SqlDbType = SqlDbType.Structured;
                prms[2].TypeName = TypeMLTicketDescWordList;
                prms[3] = new SqlParameter("@lstOptionalWordList", objRemarkCollection.ToList().ToDT());
                prms[3].SqlDbType = SqlDbType.Structured;
                prms[3].TypeName = TypeMLOptionalWordList;
                prms[4] = new SqlParameter("@Choose", Choose);
                DataTable dt = (new DBHelper()).GetTableFromSP("[dbo].[ML_SaveNoiseEliminationInfraData]", prms, ConnectionString);

                if (dt != null)
                {
                    result = dt.Rows[0]["CriteriaMet"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;


        }

        /// <summary>
        /// Get pattern Details for infra configured project
        /// </summary>
        /// <param name="projectID">project id</param>
        /// <param name="ID">User id</param>

        public List<MLPatternValidationInfra> GetMLpatternValidationReport(int projectID, string ID)
        {
            List<MLPatternValidationInfra> mlPatternValidationInfraDetails = new List<MLPatternValidationInfra>();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", projectID);

            prms[1] = new SqlParameter("@UserID", ID);

            DataTable dt = (new DBHelper()).GetTableFromSP("AVL.ML_MLPatternValidationInfra", prms, ConnectionString);
            if (dt != null && dt.Rows.Count > 0)
            {
                mlPatternValidationInfraDetails = dt.AsEnumerable().Select(row =>
                 new MLPatternValidationInfra
                 {
                     Id = row["ID"] != null ? Convert.ToInt32(row["ID"]) : 0,
                     InitialLearningId = row["InitialLearningID"] != null ? Convert.
                     ToInt32(row["InitialLearningID"]) : 0,
                     TowerId = row["TowerID"] != null ? Convert.
                     ToInt32(row["TowerID"]) : 0,
                     TowerName = (Convert.ToString(row["TowerName"] !=
                     DBNull.Value ? Convert.ToString(row["TowerName"]) : string.Empty)),
                     IsRegenerated = row["IsRegenerated"] != null ? Convert.ToBoolean(row["IsRegenerated"])
                     : false,
                     TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                     ToString(row["TicketPattern"]) : string.Empty)),
                     SubPattern = (Convert.ToString(row["SubPattern"] != DBNull.Value &&
                     Convert.ToString(row["SubPattern"]) != "0" ? Convert.
                     ToString(row["SubPattern"]) : string.Empty)),
                     AdditionalTextPattern = (Convert.ToString(row["AdditionalPattern"] !=
                     DBNull.Value && Convert.ToString(row["AdditionalPattern"]) != "0" ?
                     Convert.ToString(row["AdditionalPattern"]) : string.Empty)),
                     AdditionalTextsubPattern = (Convert.ToString(row["AdditionalSubPattern"] !=
                     DBNull.Value && Convert.ToString(row["AdditionalSubPattern"]) != "0" ?
                     Convert.ToString(row["AdditionalSubPattern"]) : string.Empty)),
                     MLDebtClassificationId = row["MLDebtClassificationID"] != null ? Convert.
                     ToInt32(row["MLDebtClassificationID"]) : 0,
                     MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] !=
                     DBNull.Value ? Convert.ToString(row["MLDebtClassificationName"]) : string.Empty)),
                     MLResidualFlagId = row["MLResidualFlagID"] != null ? Convert.
                     ToInt32(row["MLResidualFlagID"]) : 0,
                     MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] != DBNull.Value ?
                     Convert.ToString(row["MLResidualFlagName"]) : string.Empty)),
                     MLAvoidableFlagId = row["MLAvoidableFlagID"] != null ? Convert.
                     ToInt32(row["MLAvoidableFlagID"]) : 0,
                     MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] !=
                     DBNull.Value ? Convert.ToString(row["MLAvoidableFlagName"]) : string.Empty)),
                     MLCauseCodeId = row["MLCauseCodeID"] != null ? Convert.ToInt32(row["MLCauseCodeID"]) : 0,
                     MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ? Convert.
                     ToString(row["MLCauseCodeName"]) : string.Empty)),
                     MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ?
                     Convert.ToString(row["MLResolutionCodeName"]) : string.Empty)),
                     MLResolutionCodeId = row["MLResolutionCodeID"] != null ? Convert.
                     ToInt32(row["MLResolutionCodeID"]) : 0,
                     MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                     ToString(row["MLAccuracy"]) : string.Empty)),
                     TicketOccurence = (Convert.ToInt32(row["TicketOccurence"] != DBNull.Value ?
                     Convert.ToInt32(row["TicketOccurence"]) : 0)),
                     AnalystResolutionCodeId = row["AnalystResolutionCodeID"] != null ? Convert.
                     ToInt32(row["AnalystResolutionCodeID"]) : 0,
                     AnalystResolutionCodeName = (Convert.ToString(row["AnalystResolutionCodeName"] !=
                     DBNull.Value ? Convert.ToString(row["AnalystResolutionCodeName"]) : string.Empty)),
                     AnalystCauseCodeId = row["AnalystCauseCodeID"] != null ? Convert.
                     ToInt32(row["AnalystCauseCodeID"]) : 0,
                     AnalystCauseCodeName = (Convert.ToString(row["AnalystCauseCodeName"] != DBNull.Value ?
                     Convert.ToString(row["AnalystCauseCodeName"]) : string.Empty)),
                     AnalystDebtClassificationId = row["AnalystDebtClassificationID"] != null ? Convert.
                     ToInt32(row["AnalystDebtClassificationID"]) : 0,
                     AnalystDebtClassificationName = (Convert.ToString(row["AnalystDebtClassificationName"] !=
                     DBNull.Value ? Convert.ToString(row["AnalystDebtClassificationName"]) : string.Empty)),
                     AnalystAvoidableFlagId = row["AnalystAvoidableFlagID"] != null ? Convert.
                     ToInt32(row["AnalystAvoidableFlagID"]) : 0,
                     AnalystAvoidableFlagName = (Convert.ToString(row["AnalystAvoidableFlagName"] !=
                     DBNull.Value ? Convert.ToString(row["AnalystAvoidableFlagName"]) : string.Empty)),
                     SMEComments = (Convert.ToString(row["SMEComments"] != DBNull.Value ? Convert.
                     ToString(row["SMEComments"]) : string.Empty)),
                     SMEResidualFlagId = row["SMEResidualFlagID"] != null ? Convert.
                     ToInt32(row["SMEResidualFlagID"]) : 0,
                     SMEResidualFlagName = (Convert.ToString(row["SMEResidualFlagName"] != DBNull.Value ?
                     Convert.ToString(row["SMEResidualFlagName"]) : string.Empty)),
                     SMEDebtClassificationId = row["SMEDebtClassificationID"] != null ? Convert.
                     ToInt32(row["SMEDebtClassificationID"]) : 0,
                     SMEDebtClassificationName = (Convert.ToString(row["SMEDebtClassificationName"] !=
                     DBNull.Value ? Convert.ToString(row["SMEDebtClassificationName"]) : string.Empty)),
                     SMEAvoidableFlagId = row["SMEAvoidableFlagID"] != null ? Convert.
                     ToInt32(row["SMEAvoidableFlagID"]) : 0,
                     SMEAvoidableFlagName = (Convert.ToString(row["SMEAvoidableFlagName"] != DBNull.
                     Value ? Convert.ToString(row["SMEAvoidableFlagName"]) : string.Empty)),
                     SMECauseCodeId = row["SMECauseCodeID"] != null ? Convert.ToInt32(row["SMECauseCodeID"]) : 0,
                     SMECauseCodeName = (Convert.ToString(row["SMECauseCodeName"] != DBNull.Value ? Convert.
                     ToString(row["SMECauseCodeName"]) : string.Empty)),
                     IsApprovedOrMute = row["IsApprovedOrMute"] != null ? Convert.ToInt32(row["IsApprovedOrMute"])
                     : 0,

                     OverridenTotalCount = row["OverridenPatternTotalCount"] != null ? Convert.
                     ToInt32(row["OverridenPatternTotalCount"]) : 0,
                     IsMLSignoff = row["ISMLSignoff"] != null ? Convert.ToBoolean(row["ISMLSignoff"]) :
                     false
                         //IsRegenerated = row["IsRegenerated"] != null ? Convert.ToInt32(row["IsRegenerated"]) : 0


                     }).ToList();
            }
            return mlPatternValidationInfraDetails;
        }
        /// <summary>
        /// Method to get DebtMLPatternOccurenceReporInfra
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="TowerIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="subPattern"></param>
        /// <param name="AdditionalTextPattern"></param>
        /// <param name="AdditionalTextsubPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<MLPatternValidationInfra> GetDebtMLPatternOccurenceReporInfra(int projectID, string TowerIds,
    string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextsubPattern,
    int causeCodeId, int ResolutionCodeID)
        {
            List<MLPatternValidationInfra> debtMLPatternValidationList =
                new List<MLPatternValidationInfra>();
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@projectID", projectID);
            prms[1] = new SqlParameter("@AppID", "");

            prms[2] = new SqlParameter("@TicketPattern", TicketPattern);
            prms[3] = new SqlParameter("@subPattern", subPattern);
            prms[4] = new SqlParameter("@AdditionalTextPattern", AdditionalTextPattern);
            prms[5] = new SqlParameter("@AdditionalTextSubPattern", AdditionalTextsubPattern);
            prms[6] = new SqlParameter("@causeCodeId", causeCodeId);
            prms[7] = new SqlParameter("@ResolutionCodeID", ResolutionCodeID);
            prms[8] = new SqlParameter("@TowerID", TowerIds);
            DataSet dt = new DataSet();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Tables.Add((new DBHelper()).GetTableFromSP("ML_MLGetPatternOccurencetestInfra", prms, ConnectionString).Copy());
            if (dt.Tables[0] != null)
            {
                debtMLPatternValidationList = dt.Tables[0].AsEnumerable().Select(row =>
                new MLPatternValidationInfra
                {
                    Id = row["ID"] != null ? Convert.ToInt32(row["ID"]) : 0,
                    TowerName = (Convert.ToString(row["TowerName"] != DBNull.Value ? Convert.
                    ToString(row["TowerName"]) : string.Empty)),
                    TowerId = row["TowerID"] != null ? Convert.ToInt32(row["TowerID"]) : 0,
                    TicketPattern = (Convert.ToString(row["TicketPattern"] != DBNull.Value ? Convert.
                    ToString(row["TicketPattern"]) : string.Empty)),
                    TicketOccurence = row["TicketOccurence"] != null ? Convert.ToInt32(row["TicketOccurence"]) : 0,
                    MLDebtClassificationId = row["MLDebtClassificationID"] != null ? Convert.
                    ToInt32(row["MLDebtClassificationID"]) : 0,
                    MLDebtClassificationName = (Convert.ToString(row["MLDebtClassificationName"] != DBNull.Value ?
                    Convert.ToString(row["MLDebtClassificationName"]) : string.Empty)),
                    MLResidualFlagId = row["MLResidualCodeID"] != null ? Convert.ToInt32(row["MLResidualCodeID"]) : 0,
                    MLResidualFlagName = (Convert.ToString(row["MLResidualFlagName"] != DBNull.Value ? Convert.
                    ToString(row["MLResidualFlagName"]) : string.Empty)),
                    MLAvoidableFlagId = row["MLAvoidableFlagID"] != null ? Convert.
                    ToInt32(row["MLAvoidableFlagID"]) : 0,
                    MLAvoidableFlagName = (Convert.ToString(row["MLAvoidableFlagName"] != DBNull.Value ?
                    Convert.ToString(row["MLAvoidableFlagName"]) : string.Empty)),
                    MLCauseCodeId = row["MLCauseCodeID"] != null ? Convert.ToInt32(row["MLCauseCodeID"]) : 0,
                    MLCauseCodeName = (Convert.ToString(row["MLCauseCodeName"] != DBNull.Value ? Convert.
                    ToString(row["MLCauseCodeName"]) : string.Empty)),
                    MLResolutionCodeId = row["MLResolutionCodeID"] != null ? Convert.
                    ToInt32(row["MLResolutionCodeID"]) : 0,
                    MLResolutionCode = (Convert.ToString(row["MLResolutionCodeName"] != DBNull.Value ? Convert.
                    ToString(row["MLResolutionCodeName"]) : string.Empty)),

                    SubPattern = (Convert.ToString(row["subPattern"] != DBNull.Value &&
                    Convert.ToString(row["subPattern"]) != "0" ? Convert.
                    ToString(row["subPattern"]) : string.Empty)),
                    AdditionalTextPattern = (Convert.ToString(row["additionalPattern"] != DBNull.Value &&
                    Convert.ToString(row["additionalPattern"]) != "0" ? Convert.
                    ToString(row["additionalPattern"]) : string.Empty)),
                    AdditionalTextsubPattern = (Convert.ToString(row["additionalSubPattern"] != DBNull.Value
                    && Convert.ToString(row["additionalSubPattern"]) != "0" ?
                    Convert.ToString(row["additionalSubPattern"]) : string.Empty)),
                    MLAccuracy = (Convert.ToString(row["MLAccuracy"] != DBNull.Value ? Convert.
                    ToString(row["MLAccuracy"]) : string.Empty))

                }).ToList();
            }

            return debtMLPatternValidationList;
        }
        /// <summary>
        /// GetRegenerateILDetails - used to get the Application , Tower Details based on the Support type
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        public RegenerateILDetails GetRegenerateILDetails(Int32 projectID, Int16 SupportTypeID)
        {
            RegenerateILDetails result;
            string SPName = "[AVL].[ML_GetRegeneratedApplication]";
            if (SupportTypeID == 2)
            {
                SPName = "[AVL].[ML_GetRegeneratedTower]";
            }
            try
            {
                result = new RegenerateILDetails();
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                DataTable dt = (new DBHelper()).GetTableFromSP(SPName, prms, ConnectionString);

                if (dt != null)
                {
                    List<RegenerateInfraDetails> lstRegenerateInfraDetails = null;
                    List<RegenerateAVMDetails> lstRegenerateAVMDetails = null;
                    if (SupportTypeID == 2)
                    {
                        lstRegenerateInfraDetails = dt.AsEnumerable().Select(row =>
                      new RegenerateInfraDetails
                      {
                          TowerId = row["TowerID"] != null ? Convert.ToInt64(row["TowerID"]) : 0,
                          TowerName = row["TowerName"] != null ? Convert.ToString(row["TowerName"]) : string.Empty,
                      }).ToList();
                    }
                    else
                    {
                        lstRegenerateAVMDetails = dt.AsEnumerable().Select(row =>
                      new RegenerateAVMDetails
                      {
                          ApplicationId = row["ApplicationID"] != null ? Convert.ToInt64(row["ApplicationID"]) : 0,
                          ApplicationName = row["ApplicationName"] != null ? Convert.ToString(row["ApplicationName"])
                          : string.Empty,
                      }).ToList();
                    }
                    result.SupportTypeId = SupportTypeID;
                    result.LstTower = lstRegenerateInfraDetails;
                    result.LstApplication = lstRegenerateAVMDetails;
                }
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Generate Infra Pattern
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstGeneratePatternApps"></param>
        /// <param name="UserId"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GenerateInfrapatterns(int ProjectID, List<RegenerateApplicationDetails> lstGeneratePatternApps,
            string UserId, int CustomerID)
        {
            List<RegenerateApplicationDetails> objApplicationsCollection = new List<RegenerateApplicationDetails>();
            SqlParameter[] prms = new SqlParameter[4];
            for (int i = 0; i < lstGeneratePatternApps.Count; i++)
            {
                objApplicationsCollection.Add(new RegenerateApplicationDetails
                {
                    ApplicationId = lstGeneratePatternApps[i].ApplicationId,
                });

            }
            var dtparam = objApplicationsCollection.ToDataTable<RegenerateApplicationDetails>();
            //This is specific to infra support
            dtparam.Columns[0].ColumnName = "ID";
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@lstRegenerateTower", dtparam);
            prms[1].SqlDbType = SqlDbType.Structured;
            prms[1].TypeName = ApplicationConstants.TypeRegenerateTowerDetails;
            prms[2] = new SqlParameter("@UserId", UserId);
            prms[3] = new SqlParameter("@CustomerID", CustomerID);
            try
            {
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[ML_SaveRegenerateTowerDetails]", prms, ConnectionString);
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
    }
}

