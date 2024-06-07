using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Extensions;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// This class holds Translation details
    /// </summary>
    public class Translation:DBContext
    {
        private static string myTaskURL = new AppSettings().AppsSttingsKeyValues["WebApiURL"];
        private readonly string commonAPIURL = new AppSettings().AppsSttingsKeyValues["CommonAPIURL"];
        private static string trustedHosts = new AppSettings().AppsSttingsKeyValues["MyTaskHost"];
        readonly string apiKeyHandler = new AppSettings().AppsSttingsKeyValues["ApiKeyHandler"];
        readonly string apiValueHandler = new AppSettings().AppsSttingsKeyValues["APIValueHandler"];
        readonly string apiAuthKeyHandler = new AppSettings().AppsSttingsKeyValues["APIAuthKeyHandler"];
        readonly string apiAuthValueHandler = new AppSettings().AppsSttingsKeyValues["APIAuthValueHandler"];

        private List<MultilingualTranslatedValues> multilingualTranslatedValues;
        public List<MultilingualTranslatedValues> MultilingualTranslatedValue
        {
            get { return multilingualTranslatedValues; }
            set { multilingualTranslatedValues = value; }
        }
        private List<MultilingualTicketValues> multilingualTranslatedTickets = new List<MultilingualTicketValues>();
        public List<MultilingualTicketValues> MultilingualTranslatedTicket
        {
            get { return multilingualTranslatedTickets; }
            set { multilingualTranslatedTickets = value; }
        }
        /// <summary>
        /// This method is used to Get project multiligual translate fields
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="projectId">projectId</param>
        /// <param name="employeeId">employeeId</param>
        /// <returns>List of project multiligual translate fields</returns>
        public MultilingualConfigModel GetProjectMultilinugalTranslateFields(string customerId,
            string projectId)
        {
            MultilingualConfigModel config = new MultilingualConfigModel();
            try
            {
                SqlParameter[] prmsObj = new SqlParameter[2];
                prmsObj[0] = new SqlParameter("@ProjectID", projectId);
                prmsObj[1] = new SqlParameter("@CustomerID", customerId);

                DataSet dsMultilingualConfig =
                    new DBHelper().GetDatasetFromSP("[AVL].[GetMultilingualConfigDetails]", prmsObj,ConnectionString);

                if (dsMultilingualConfig.Tables[0] != null && dsMultilingualConfig.Tables[0].Rows.Count > 0)
                {
                    config.SubscriptionKey = !string.IsNullOrEmpty(Convert.ToString
                        (dsMultilingualConfig.Tables[0].Rows[0]["MSubscriptionKey"])) ?
                        Convert.ToString(dsMultilingualConfig.Tables[0].Rows[0]["MSubscriptionKey"]) : string.Empty;
                    config.IsSingleOrMulti = !string.IsNullOrEmpty
                        (Convert.ToString(dsMultilingualConfig.Tables[0].Rows[0]["IsSingleORMulti"])) ?
                        Convert.ToInt16(dsMultilingualConfig.Tables[0].Rows[0]["IsSingleORMulti"]) : 0;
                    config.IsMultilingualEnable = !string.IsNullOrEmpty
                        (Convert.ToString(dsMultilingualConfig.Tables[0].Rows[0]["IsMultilingualEnabled"])) ?
                        Convert.ToInt16(dsMultilingualConfig.Tables[0].Rows[0]["IsMultilingualEnabled"]) : 0;
                }

                var lstTranslateFields = dsMultilingualConfig.Tables[1] != null ?
                    dsMultilingualConfig.Tables[1].ToList<MultilinugalTranslateFieldsModel>() : null;
                if (lstTranslateFields != null)
                {
                    lstTranslateFields = lstTranslateFields.Count > 0 ?
                    lstTranslateFields.Where(translateField => translateField.IsSelected.Equals(1)).ToList() : null;
                }
                

                var lstTranslateLanguage = dsMultilingualConfig.Tables[2] != null ?
                    dsMultilingualConfig.Tables[2].ToList<LanguageModel>() : null;
                lstTranslateLanguage = lstTranslateLanguage.Count > 0 ?
                    lstTranslateLanguage.Where(translateLanguage
                    => translateLanguage.IsSelected.Equals(true)).ToList() : null;

                config.ListTranslateFields = lstTranslateFields;
                config.ListTranslateLanguage = lstTranslateLanguage;
            }
            catch (CustomException ex)
            {
                throw ex;
            }
            return config;
        }

        ///<summary>
        /// Get MultiLingual Tickets by Ticket ID
        /// </summary>
        /// <param name="TicketID">TicketID</param>
        /// <param name="SupportTypeID">SupportTypeID</param>        
        public void GetTickets(string TicketID, int SupportTypeID)
        {

            List<LanguageProjectMapping> lstLanPro = new List<LanguageProjectMapping>();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@TicketID", TicketID);
                prms[1] = new SqlParameter("@SupportTypeID", SupportTypeID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP("AVL.GetMultilingualTicketsByTicketID", prms,ConnectionString);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtTickets = ds.Tables[0];
                    DataTable dtLanguages = ds.Tables[1];
                    if (dtLanguages != null && dtLanguages.Rows.Count > 0)
                    {

                        lstLanPro =
                          (from item in dtLanguages.AsEnumerable()
                           select new LanguageProjectMapping
                           {
                               LanguageValue = item["LanguageValue"] != DBNull.Value
                               ? item.Field<string>("LanguageValue") : string.Empty,
                               ProjectId = item["ProjectID"] != DBNull.Value ? item.Field<long>("ProjectID") : 0,
                               SubKey = item["Key"] != DBNull.Value ? item.Field<string>("Key") : string.Empty
                           }).ToList();
                    }
                    if (dtTickets != null && dtTickets.Rows.Count > 0)
                    {
                        AESEncryption aesMod = new AESEncryption();
                        multilingualTranslatedValues =
                          (from item in dtTickets.AsEnumerable()
                           select new MultilingualTranslatedValues
                           {
                               TimeTickerId = item["TimeTickerID"] != DBNull.Value ? item.Field<long>("TimeTickerID")
                               : 0,
                               ProjectId = item["ProjectID"] != DBNull.Value ? item.Field<long>("ProjectID") : 0,
                               IsTicketDescriptionUpdated = item["isTicketDescriptionUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isTicketDescriptionUpdated") : false,
                               TicketDescription = item["TicketDescription"] != DBNull.Value ?
                               item.Field<string>("TicketDescription") != "" ?
                               aesMod.DecryptStringFromBytes(Convert.FromBase64String
                               (item.Field<string>("TicketDescription")),
                               AseKeyDetail.AesKeyConstVal) : string.Empty : string.Empty,
                               IsResolutionRemarksUpdated = item["isResolutionRemarksUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isResolutionRemarksUpdated") : false,
                               ResolutionRemarks = item["ResolutionRemarks"] != DBNull.Value ?
                               item.Field<string>("ResolutionRemarks") : string.Empty,
                               IsTicketSummaryUpdated = item["isTicketSummaryUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isTicketSummaryUpdated") : false,
                               TicketSummary = item["TicketSummary"] != DBNull.Value ?
                               item.Field<string>("TicketSummary") != "" ?
                               aesMod.DecryptStringFromBytes(Convert.FromBase64String
                               (item.Field<string>("TicketSummary")),
                               AseKeyDetail.AesKeyConstVal) : string.Empty : string.Empty,
                               IsCommentsUpdated = item["isCommentsUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isCommentsUpdated") : false,
                               Comments = item["Comments"] != DBNull.Value ?
                               item.Field<string>("Comments") : string.Empty,
                               IsFlexField1Updated = item["isFlexField1Updated"] != DBNull.Value ?
                               item.Field<Boolean>("isFlexField1Updated") : false,
                               FlexField1 = item["FlexField1"] != DBNull.Value ?
                               item.Field<string>("FlexField1") : string.Empty,
                               IsFlexField2Updated = item["isFlexField2Updated"] != DBNull.Value ?
                               item.Field<Boolean>("isFlexField2Updated") : false,
                               FlexField2 = item["FlexField2"] != DBNull.Value ?
                               item.Field<string>("FlexField2") : string.Empty,
                               IsFlexField3Updated = item["isFlexField3Updated"] != DBNull.Value ?
                               item.Field<Boolean>("isFlexField3Updated") : false,
                               FlexField3 = item["FlexField3"] != DBNull.Value ?
                               item.Field<string>("FlexField3") : string.Empty,
                               IsFlexField4Updated = item["isFlexField4Updated"] != DBNull.Value ?
                               item.Field<Boolean>("isFlexField4Updated") : false,
                               FlexField4 = item["FlexField4"] != DBNull.Value ?
                               item.Field<string>("FlexField4") : string.Empty,
                               IsCategoryUpdated = item["isCategoryUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isCategoryUpdated") : false,
                               Category = item["Category"] != DBNull.Value ?
                               item.Field<string>("Category") : string.Empty,
                               IsTypeUpdated = item["isTypeUpdated"] != DBNull.Value ?
                               item.Field<Boolean>("isTypeUpdated") : false,
                               Type = item["Type"] != DBNull.Value ?
                               item.Field<string>("Type") : string.Empty,
                               Languages = lstLanPro.Where(x => x.ProjectId == (item["ProjectID"] != DBNull.Value ?
                               item.Field<long>("ProjectID") : 0)).Select(x => x.LanguageValue).ToList(),
                               Key = lstLanPro.Where(x => x.ProjectId == (item["ProjectID"] != DBNull.Value ?
                               item.Field<long>("ProjectID") : 0)).Select(x => x.SubKey).FirstOrDefault(),
                               SupportTypeId = SupportTypeID,
                           }).ToList();
                    }
                }
            }
            catch (CustomException ex)
            {
                //ErrorLogTranslationTicket ticketErrorLog = new ErrorLogTranslationTicket();
                //ticketErrorLog.TimeTickerID = 0;
                //ticketErrorLog.SupportTypeID = SupportTypeID;
                //LogTranslateAPIError(ticketErrorLog, string.Empty, string.Format(ApplicationConstants.ErrorScope,
                //this.GetType().Name), string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                throw ex;
            }

        }

        /// <summary>
        /// This Method Checks if a subscription key is active
        /// </summary>
        /// <returns></returns>
        public List<ConcatenateStrings> CheckIfProjectSubscriptionIsActive()
        {
            List<ConcatenateStrings> lstConcatStrings = new List<ConcatenateStrings>();
            TranslateValidation obj = new TranslateValidation();
            obj.Text = ApplicationConstants.TranslateValidateString;
            obj.LanguageFrom = ApplicationConstants.TranslateDestinationLanguageCode;
            obj.LanguageTo = ApplicationConstants.TranslateSpanishLanguageCode;
            bool hasNoError = true;
            try
            {
                if (multilingualTranslatedValues != null && multilingualTranslatedValues.Count > 0)
                {
                    List<MultilingualTranslatedValues> distinctKeyPrj = multilingualTranslatedValues
                        .GroupBy(p => new { p.ProjectId, p.Key })
                        .Select(g => g.First())
                        .ToList();
                    foreach (var value in distinctKeyPrj)
                    {
                        if ((value.Key != null && value.Key.Length > 0) && (value.ProjectId > 0))
                        {
                            obj.Key = value.Key;
                            obj.ProjectId = value.ProjectId;
                            hasNoError = CallProjectSubscriptionIsActive(obj).Result;
                            if (!hasNoError)
                            {
                                multilingualTranslatedValues.RemoveAll(x => x.ProjectId == value.ProjectId);
                            }
                        }
                        else
                        {
                            multilingualTranslatedValues.RemoveAll(x => x.ProjectId == value.ProjectId);
                        }
                    }
                    if (multilingualTranslatedValues.Count > 0)
                    {
                        lstConcatStrings = ProcessForTranslation();
                        ModifyListForUpdate();
                        UpdateMultilingualTable(multilingualTranslatedValues[0].TimeTickerId,
                            multilingualTranslatedValues[0].SupportTypeId);
                    }
                }

            }
            catch (CustomException ex)
            {
              //  ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
                //errorLogTicket.TimeTickerID = multilingualTranslatedValues[0].TimeTickerId;
               // errorLogTicket.SupportTypeID = multilingualTranslatedValues[0].SupportTypeId;

               // LogTranslateAPIError(errorLogTicket, string.Empty,
                   // string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                   // string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                throw ex;
            }
            return lstConcatStrings;

        }

        /// <summary>
        /// This Method Is Used To DataTableConvertion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        public DataTable DataTableConvertion<TSource>(IList<TSource> data)
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(TSource).Name);
                dataTable.Locale = CultureInfo.InvariantCulture;
                PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in props)
                {
                    dataTable.Columns.Add(prop.Name,
                        Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (TSource item in data)
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
            catch (Exception ex)
            {
               // ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
               // errorLogTicket.TimeTickerID = multilingualTranslatedValues[0].TimeTickerId;
               // errorLogTicket.SupportTypeID = multilingualTranslatedValues[0].SupportTypeId;
               // LogTranslateAPIError(errorLogTicket, string.Empty, string.Format(ApplicationConstants.ErrorScope,
                 //   this.GetType().Name), string.Format(ApplicationConstants.ErrorMessage,
                  //  ex.Message, ex.StackTrace));
                throw ex;
            }
        }
        /// <summary>
        /// Updates the translated data in Database
        /// </summary>
        /// <param name="TimeTickerID">TimeTickerID</param>
        /// <param name="SupportTypeID">SupportTypeID</param>       
        private void UpdateMultilingualTable(long TimeTickerID, int SupportTypeID)
        {
            try
            {
                DataTable dtTicket = new DataTable();
                dtTicket.Locale = CultureInfo.InvariantCulture;
                dtTicket.Columns.Add("TimeTickerID");
                dtTicket.Columns.Add("TicketDescription");
                dtTicket.Columns.Add("ResolutionRemarks");
                dtTicket.Columns.Add("TicketSummary");
                dtTicket.Columns.Add("Comments");
                dtTicket.Columns.Add("FlexField1");
                dtTicket.Columns.Add("FlexField2");
                dtTicket.Columns.Add("FlexField3");
                dtTicket.Columns.Add("FlexField4");
                dtTicket.Columns.Add("Category");
                dtTicket.Columns.Add("Type");
                dtTicket.Columns.Add("IsTicketDescriptionUpdated");
                dtTicket.Columns.Add("HasTicketDescriptionError");
                dtTicket.Columns.Add("IsResolutionRemarksUpdated");
                dtTicket.Columns.Add("HasResolutionRemarksError");
                dtTicket.Columns.Add("IsTicketSummaryUpdated");
                dtTicket.Columns.Add("HasTicketSummaryError");
                dtTicket.Columns.Add("IsCommentsUpdated");
                dtTicket.Columns.Add("HasCommentsError");
                dtTicket.Columns.Add("HasFlexField1Error");
                dtTicket.Columns.Add("IsFlexField1Updated");
                dtTicket.Columns.Add("HasFlexField2Error");
                dtTicket.Columns.Add("IsFlexField2Updated");
                dtTicket.Columns.Add("HasFlexField3Error");
                dtTicket.Columns.Add("IsFlexField3Updated");
                dtTicket.Columns.Add("HasFlexField4Error");
                dtTicket.Columns.Add("IsFlexField4Updated");
                dtTicket.Columns.Add("HasCategoryError");
                dtTicket.Columns.Add("IsCategoryUpdated");
                dtTicket.Columns.Add("HasTypeError");
                dtTicket.Columns.Add("IsTypeUpdated");
                dtTicket.Columns.Add("TicketType");


                foreach (var val in multilingualTranslatedTickets)
                {
                    DataRow _row = dtTicket.NewRow();
                    _row["TimeTickerID"] = TimeTickerID;
                    _row["TicketDescription"] = val.TicketDescription;
                    _row["TicketSummary"] = val.TicketSummary;
                    _row["ResolutionRemarks"] = val.ResolutionRemarks;
                    _row["Comments"] = val.Comments;
                    _row["Category"] = val.Category;
                    _row["Type"] = val.Type;
                    _row["FlexField1"] = val.FlexField1;
                    _row["FlexField2"] = val.FlexField2;
                    _row["FlexField3"] = val.FlexField3;
                    _row["FlexField4"] = val.FlexField4;
                    _row["IsTicketDescriptionUpdated"] = val.IsTicketDescriptionUpdated;
                    _row["HasTicketDescriptionError"] = val.HasTicketDescriptionError;
                    _row["IsResolutionRemarksUpdated"] = val.IsResolutionRemarksUpdated;
                    _row["HasResolutionRemarksError"] = val.HasResolutionRemarksError;
                    _row["IsTicketSummaryUpdated"] = val.IsTicketSummaryUpdated;
                    _row["HasTicketSummaryError"] = val.HasTicketSummaryError;
                    _row["IsCommentsUpdated"] = val.IsCommentsUpdated;
                    _row["HasCommentsError"] = val.HasCommentsError;
                    _row["HasFlexField1Error"] = val.HasFlexField1Error;
                    _row["IsFlexField1Updated"] = val.IsFlexField1Updated;
                    _row["HasFlexField2Error"] = val.HasFlexField2Error;
                    _row["IsFlexField2Updated"] = val.IsFlexField2Updated;
                    _row["HasFlexField3Error"] = val.HasFlexField3Error;
                    _row["IsFlexField3Updated"] = val.IsFlexField3Updated;
                    _row["HasFlexField4Error"] = val.HasFlexField4Error;
                    _row["IsFlexField4Updated"] = val.IsFlexField4Updated;
                    _row["HasCategoryError"] = val.HasCategoryError;
                    _row["IscategoryUpdated"] = val.IsCategoryUpdated;
                    _row["HasTypeError"] = val.HasTypeError;
                    _row["IsTypeUpdated"] = val.IsTypeUpdated;
                    _row["TicketType"] = SupportTypeID;

                    dtTicket.Rows.Add(_row);
                    dtTicket.AcceptChanges();
                }
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@MultiLingualTable", dtTicket);
                sqlParams[0].SqlDbType = SqlDbType.Structured;
                sqlParams[0].TypeName = "dbo.TVP_EditTicketMultiLingual";
                new DBHelper()
                    .ExecuteNonQuery("[AVL].[UpdateMultilingualTranslatedTicketsForEditTicket]", sqlParams,ConnectionString);
            }

            catch (Exception ex)
            {
               // ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
               // errorLogTicket.SupportTypeID = SupportTypeID;
               // errorLogTicket.TimeTickerID = TimeTickerID;
               // LogTranslateAPIError(errorLogTicket, string.Empty,
                 //   string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                  //  string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                throw ex;
            }
        }

        /// <summary>
        /// Processes tickets for translation
        /// </summary>
        /// <returns></returns> 
        public List<ConcatenateStrings> ProcessForTranslation()
        {
            List<ConcatenateStrings> lstConcatStrings = new List<ConcatenateStrings>();
            try
            {

                foreach (var val in multilingualTranslatedValues)
                {

                    lstConcatStrings = TranslateDatatableFromLanguageRow(val);
                }

            }
            catch (Exception ex)
            {
               // ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
               // errorLogTicket.TimeTickerID = multilingualTranslatedValues[0].TimeTickerId;
                //errorLogTicket.SupportTypeID = multilingualTranslatedValues[0].SupportTypeId;
                //LogTranslateAPIError(errorLogTicket, string.Empty,
                   // string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                   // string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                throw ex;
            }
            return lstConcatStrings;
        }


        /// <summary>
        /// Updates the translated data in Database
        /// </summary>
        /// <param name="objTranslate">ObjTranslate</param>
        /// <returns></returns>
        public async Task<Boolean> CallProjectSubscriptionIsActive(TranslateValidation objTranslate)
        {
            HttpResponseMessage response = null;
            bool isActive = false;
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };


                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    EnableTrustedHosts();
                    JObject Jobj = new JObject();
                    Jobj["TranslateValidation"] = JToken.FromObject(objTranslate);
                    string url = Path.Combine(commonAPIURL, "Translate/CheckIfMSSubscriptionIsActive");
                    var dataAsString = JsonConvert.SerializeObject(Jobj);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response =await client.PostAsync(new SanitizeString(url).Value, content);
                    if (response.IsSuccessStatusCode)
                    {
                        isActive = response.IsSuccessStatusCode;
                    }
                    else
                    {
                        ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
                        errorLogTicket.SupportTypeID = multilingualTranslatedValues[0].SupportTypeId;
                        errorLogTicket.TimeTickerID = multilingualTranslatedValues[0].TimeTickerId;
                        LogTranslateAPIError(errorLogTicket, objTranslate.Key,
                            string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                            string.Format(ApplicationConstants.ApiErrorMessage, response.StatusCode,
                            response.Content.ReadAsStringAsync().Result));
                    }
                    return isActive;
                }
            }
            catch (CustomException ex)
            {
               // ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
               // errorLogTicket.TimeTickerID = multilingualTranslatedValues[0].TimeTickerId;
               // errorLogTicket.SupportTypeID = multilingualTranslatedValues[0].SupportTypeId;
                //new CustomException().CreateErrorLog(ex, string.Empty, string.Empty, this.GetType().Name);
                //LogTranslateAPIError(errorLogTicket,
                    //string.Empty, string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                   // string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                throw ex;
            }
        }

        /// <summary>
        /// Translates each value in the object 
        /// </summary>
        /// <param name="translatedValues">TranslatedValues</param>
        /// <returns></returns>
        private List<ConcatenateStrings> TranslateDatatableFromLanguageRow(MultilingualTranslatedValues
            translatedValues)
        {
            List<ConcatenateStrings> concatStrings = new List<ConcatenateStrings>();
            if (translatedValues.Languages != null && translatedValues.Languages.Count > 0
                && translatedValues.Key != null && translatedValues.Key.Length > 0)
            {

                try
                {

                    if (translatedValues.IsCategoryUpdated && translatedValues.Category != null
                        && translatedValues.Category.Trim().Length>0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.Category),
                            nameof(translatedValues.TranslatedCategory), translatedValues.Category,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasCategoryError), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsCommentsUpdated && translatedValues.Comments != null
                        && translatedValues.Comments.Trim().Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.Comments),
                            nameof(translatedValues.TranslatedComments), translatedValues.Comments,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasCommentsError), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsFlexField1Updated && translatedValues.FlexField1 != null
                        && translatedValues.FlexField1.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.FlexField1),
                            nameof(translatedValues.TranslatedFlexField1), translatedValues.FlexField1,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasFlexField1Error), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsFlexField2Updated &&
                        translatedValues.FlexField2 != null && translatedValues.FlexField2.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.FlexField2),
                            nameof(translatedValues.TranslatedFlexField2), translatedValues.FlexField2,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasFlexField2Error), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsFlexField3Updated && translatedValues.FlexField3 != null &&
                        translatedValues.FlexField3.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.FlexField3),
                            nameof(translatedValues.TranslatedFlexField3), translatedValues.FlexField3,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasFlexField3Error), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsFlexField4Updated && translatedValues.FlexField4 != null &&
                        translatedValues.FlexField4.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.FlexField4),
                            nameof(translatedValues.TranslatedFlexField4), translatedValues.FlexField4,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasFlexField4Error), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsResolutionRemarksUpdated && translatedValues.ResolutionRemarks != null &&
                        translatedValues.ResolutionRemarks.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.ResolutionRemarks),
                            nameof(translatedValues.TranslatedResolutionRemarks), translatedValues.ResolutionRemarks,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasResolutionRemarksError), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsTicketDescriptionUpdated && translatedValues.TicketDescription != null &&
                        translatedValues.TicketDescription.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.TicketDescription),
                            nameof(translatedValues.TranslatedTicketDescription), translatedValues.TicketDescription,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasTicketDescriptionError), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsTicketSummaryUpdated && translatedValues.TicketSummary != null &&
                        translatedValues.TicketSummary.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.TicketSummary),
                            nameof(translatedValues.TranslatedTicketSummary), translatedValues.TicketSummary,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasTicketSummaryError), translatedValues.SupportTypeId));
                    }
                    if (translatedValues.IsTypeUpdated && translatedValues.Type != null &&
                        translatedValues.Type.Length > 0)
                    {
                        concatStrings.Add(GetValuesToBeTranslated(nameof(translatedValues.Type),
                            nameof(translatedValues.TranslatedType), translatedValues.Type,
                            translatedValues.TimeTickerId,
                            nameof(translatedValues.HasTypeError), translatedValues.SupportTypeId));
                    }
                    concatStrings = RecursiveTranslate(concatStrings, translatedValues.Languages.ToArray(),
                        translatedValues.Key);
                    foreach (var val in concatStrings)
                    {
                        PropertyInfo myPropInfo = translatedValues.GetType().GetProperty(val.TranslatedColumn);
                        myPropInfo.SetValue(translatedValues, val.TranslatedText, null);
                        PropertyInfo myPropInfoError = translatedValues.GetType().GetProperty(val.ErrorCol);
                        myPropInfoError.SetValue(translatedValues, val.HasError, null);

                    }
                }
                catch (CustomException ex)
                {
                    ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
                    errorLogTicket.TimeTickerID = translatedValues.TimeTickerId;
                    errorLogTicket.SupportTypeID = translatedValues.SupportTypeId;
                    translatedValues.HasCategoryError = true;
                    translatedValues.HasCommentsError = true;
                    translatedValues.HasFlexField1Error = true;
                    translatedValues.HasFlexField2Error = true;
                    translatedValues.HasFlexField3Error = true;
                    translatedValues.HasFlexField4Error = true;
                    translatedValues.IsResolutionRemarksUpdated = true;
                    translatedValues.IsTicketDescriptionUpdated = true;
                    translatedValues.IsTicketSummaryUpdated = true;
                    translatedValues.HasTypeError = true;
                  //  LogTranslateAPIError(errorLogTicket, string.Empty, string.Format(ApplicationConstants.ErrorScope,
                    //    this.GetType().Name), string.Format(ApplicationConstants.ErrorMessage, ex.Message,
                      //  ex.StackTrace));
                    throw ex;
                }
            }
            return concatStrings;
        }

        /// <summary>
        /// Translates all the values in the object
        /// </summary>
        /// <param name="conStr">Concatenate Strings</param>
        /// <param name="languageTo">LanguageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        private List<ConcatenateStrings> RecursiveTranslate(List<ConcatenateStrings> conStr,
            string[] from, string key)
        {
            try
            {
                int maxlen = 5000;
                int curlen = 0;
                string languageTo = ApplicationConstants.TranslateDestinationLanguageCode;
                List<ConcatenateStrings> strTemp = new List<ConcatenateStrings>();
                strTemp = conStr.Where(x => x.IsTranslated == false
                && x.TextLength < maxlen && (curlen = x.TextLength + curlen) < maxlen).ToList();
                if (strTemp.Count == 0)
                {
                    foreach (var lengthTextColumn in conStr)
                    {
                        if (!lengthTextColumn.IsTranslated && lengthTextColumn.TextLength > 4999)
                        {
                            int translateLength = 0;
                            var result = BreakSentence(lengthTextColumn.Text, key, lengthTextColumn);
                            result.Wait();
                            if (result.Result != null && result.Result.GetType().IsArray)
                            {
                                int startLength;
                                string splitLengthText;
                                List<int> limitLength = new List<int>();
                                var breakSentenceArray = new List<int>(result.Result);
                                while (breakSentenceArray.Count > 0 && lengthTextColumn.TextLength > translateLength)
                                {
                                    startLength = 0;
                                    limitLength = breakSentenceArray.Where(x =>
                                    (startLength = x + startLength) < 4999).ToList();
                                    splitLengthText = lengthTextColumn.Text.Substring(translateLength,
                                        limitLength.Sum(x => x));
                                    lengthTextColumn.DelimiterText.Add(splitLengthText);
                                    translateLength += limitLength.Sum(x => x);
                                    breakSentenceArray.RemoveRange(0, limitLength.Count);
                                }
                            }
                            strTemp.Add(lengthTextColumn);

                        }
                    }
                }
                strTemp = CallTranslator(strTemp, languageTo, from, key);

                foreach (var val in conStr)
                {
                    try
                    {
                        if (strTemp.Where(x => x.OriginalColumn == val.OriginalColumn).Count() > 0)
                        {
                            val.IsTranslated = strTemp.Where(x => x.OriginalColumn ==
                            val.OriginalColumn).Select(x => x.IsTranslated).First();
                            val.TranslatedText = strTemp.Where(x => x.OriginalColumn ==
                            val.OriginalColumn).Select(x => x.TranslatedText).FirstOrDefault() ?? val.TranslatedText;
                            val.HasError = strTemp.Where(x => x.OriginalColumn ==
                            val.OriginalColumn).Select(x => x.HasError).First();
                        }

                    }
                    catch (Exception)
                    {
                        val.IsTranslated = true;
                        val.TranslatedText = null;
                        val.HasError = true;

                    }
                }
                if (conStr.Any(x => x.IsTranslated == false))
                {
                    RecursiveTranslate(conStr, from, key);
                }
                return conStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Logs Translate API Error and assignes to object
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="position">Position</param>
        /// <param name="message">Message</param>
        /// <param name="stackTrace">StackTrace</param>
        /// <param name="hasError">HasError</param>
        /// <returns>Creates Log</returns>
        private List<ConcatenateStrings> ModifyConcatenateStringsOnException(List<ConcatenateStrings> conStr,
            string position, string message, string stackTrace, bool hasError = true)
        {
            ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
            StringBuilder errorMsg = new StringBuilder();
            errorMsg.Append(message);
            errorMsg.Append(" Stack Trace :");
            errorMsg.Append(stackTrace);
            foreach (var val in conStr)
            {
                val.HasError = true;
                val.IsTranslated = true;
                val.TranslatedText = null;

                if (hasError)
                {
                    errorLogTicket.TimeTickerID = val.TimeTickerId;
                    errorLogTicket.SupportTypeID = val.SupportType;
                    LogTranslateAPIError(errorLogTicket, val.Text, position, errorMsg.ToString());
                }
            }
            return conStr;
        }
        /// <summary>
        /// Calls The trabslantor API based on language
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="languageTo">languageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public List<ConcatenateStrings> CallTranslator(List<ConcatenateStrings> conStr,
            string languageTo, string[] from, string key)
        {
            try
            {

                conStr = PostToMSTranslatorList(conStr, languageTo, from, key).Result;
                return conStr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calls The trabslantor API
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="languageTo">LanguageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public async Task<List<ConcatenateStrings>> PostToMSTranslatorList(List<ConcatenateStrings> conStr,
            string languageTo, string[] from, string key)
        {
            HttpResponseMessage response = null;
            List<ConcatenateStrings> constrValue = new List<ConcatenateStrings>();
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
               
                {
                    EnableTrustedHosts();
                    RootObject rootObj = new RootObject();
                    rootObj.Key = key;
                    rootObj.LanguageTo = languageTo;
                    rootObj.ConcatenateStrings = conStr;
                    rootObj.Languages = from.ToList();
                    JObject Jobj = new JObject();
                    Jobj["RootObject"] = JToken.FromObject(rootObj);
                    string url = Path.Combine(commonAPIURL, "Translate/TranslateMultiText");
                    var dataAsString = JsonConvert.SerializeObject(Jobj);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(new SanitizeString(url).Value, content);
                    if (response.IsSuccessStatusCode)
                    {

                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        constrValue = JsonConvert.DeserializeObject<List<ConcatenateStrings>>(responseBody);
                    }
                    else
                    {
                        constrValue = ModifyConcatenateStringsOnException(conStr,
                            string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                            response.StatusCode.ToString(), response.ReasonPhrase);
                    }
                    return constrValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Modifies the list for database update
        /// </summary>
        /// <returns></returns>
        private void ModifyListForUpdate()
        {
            if (multilingualTranslatedValues != null && multilingualTranslatedValues.Count > 0)
            {

                foreach (var value in multilingualTranslatedValues)
                {
                    try
                    {
                        if ((value.IsCategoryUpdated && !value.HasCategoryError)
                            || (value.IsCommentsUpdated && !value.HasCommentsError)
                            || (value.IsFlexField1Updated && !value.HasFlexField1Error)
                            || (value.IsFlexField2Updated && !value.HasFlexField2Error)
                            || (value.IsFlexField3Updated && !value.HasFlexField3Error)
                            || (value.IsFlexField4Updated && !value.HasFlexField4Error)
                            || (value.IsResolutionRemarksUpdated && !value.HasResolutionRemarksError)
                            || (value.IsTicketDescriptionUpdated && !value.HasTicketDescriptionError)
                            || (value.IsTicketSummaryUpdated && !value.HasTicketSummaryError)
                            || (value.IsTypeUpdated && !value.HasTypeError))
                        {
                            MultilingualTicketValues ticket = new MultilingualTicketValues();
                            ticket.TimeTickerId = value.TimeTickerId;
                            ticket.CreatedBy = "SYSTEM";
                            ticket.CreatedDate = System.DateTime.Today;
                            if (value.IsCategoryUpdated)
                            {
                                ticket.Category = value.TranslatedCategory;
                                ticket.IsCategoryUpdated = value.HasCategoryError ? true : false;
                                ticket.HasCategoryError = value.HasCategoryError;

                            }
                            if (value.IsCommentsUpdated)
                            {
                                ticket.Comments = value.TranslatedComments;
                                ticket.IsCommentsUpdated = value.HasCommentsError ? true : false;
                                ticket.HasCommentsError = value.HasCommentsError;

                            }
                            if (value.IsFlexField1Updated)
                            {
                                ticket.FlexField1 = value.TranslatedFlexField1;
                                ticket.IsFlexField1Updated = value.HasFlexField1Error ? true : false;
                                ticket.HasFlexField1Error = value.HasFlexField1Error;

                            }
                            if (value.IsFlexField2Updated)
                            {
                                ticket.FlexField2 = value.TranslatedFlexField2;
                                ticket.IsFlexField2Updated = value.HasFlexField2Error ? true : false;
                                ticket.HasFlexField2Error = value.HasFlexField2Error;

                            }
                            if (value.IsFlexField3Updated)
                            {
                                ticket.FlexField3 = value.TranslatedFlexField3;
                                ticket.IsFlexField3Updated = value.HasFlexField3Error ? true : false;
                                ticket.HasFlexField3Error = value.HasFlexField3Error;

                            }
                            if (value.IsFlexField4Updated)
                            {
                                ticket.FlexField4 = value.TranslatedFlexField4;
                                ticket.IsFlexField4Updated = value.HasFlexField4Error ? true : false;
                                ticket.HasFlexField4Error = value.HasFlexField4Error;

                            }
                            if (value.IsResolutionRemarksUpdated)
                            {
                                ticket.ResolutionRemarks = value.TranslatedResolutionRemarks;
                                ticket.IsResolutionRemarksUpdated = value.HasResolutionRemarksError ? true : false;
                                ticket.HasResolutionRemarksError = value.HasResolutionRemarksError;

                            }
                            if (value.IsTicketDescriptionUpdated)
                            {
                                AESEncryption aesMod = new AESEncryption();

                                ticket.TicketDescription = string.IsNullOrEmpty(value.TranslatedTicketDescription) ? ""
                                                : Convert.ToBase64String(aesMod.EncryptStringAsBytes
                                                (value.TranslatedTicketDescription,
                                                AseKeyDetail.AesKeyConstVal));
                                ticket.IsTicketDescriptionUpdated = value.HasTicketDescriptionError ? true : false;
                                ticket.HasTicketDescriptionError = value.HasTicketDescriptionError;

                            }
                            if (value.IsTicketSummaryUpdated)
                            {
                                AESEncryption aesMod = new AESEncryption();
                                ticket.TicketSummary = string.IsNullOrEmpty(value.TranslatedTicketSummary) ? ""
                                : Convert.ToBase64String(aesMod.EncryptStringAsBytes
                                (value.TranslatedTicketSummary, AseKeyDetail.AesKeyConstVal));
                                ticket.IsTicketSummaryUpdated = value.HasTicketSummaryError ? true : false;
                                ticket.HasTicketSummaryError = value.HasTicketSummaryError;

                            }
                            if (value.IsTypeUpdated)
                            {
                                ticket.Type = value.TranslatedType;
                                ticket.IsTypeUpdated = value.HasTypeError ? true : false;
                                ticket.HasTypeError = value.HasTypeError;

                            }

                            multilingualTranslatedTickets.Add(ticket);
                        }
                    }
                    catch (Exception ex)
                    {
                      //  ErrorLogTranslationTicket errorLogTicket = new ErrorLogTranslationTicket();
                       // errorLogTicket.TimeTickerID = value.TimeTickerId;
                       // errorLogTicket.SupportTypeID = value.SupportTypeId;
                        //LogTranslateAPIError(errorLogTicket, string.Empty,
                           // string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                           // string.Format(ApplicationConstants.ErrorMessage, ex.Message, ex.StackTrace));
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// Assigns value to be translated to a object
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="languageTo">LanguageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        private ConcatenateStrings GetValuesToBeTranslated(string originalColumn,
            string translatedColumn, string text, long timeTickerID, string errorCol,
            int SupportTypeID)
        {
            ConcatenateStrings conStr = new ConcatenateStrings();
            conStr.OriginalColumn = originalColumn;
            conStr.TranslatedColumn = translatedColumn;
            conStr.Text = text;
            conStr.TextLength = text != null ? text.Length : 0;
            conStr.TimeTickerId = timeTickerID;
            conStr.ErrorCol = errorCol;
            conStr.DelimiterText = new List<string>();
            conStr.SupportType = SupportTypeID;
            return conStr;
        }

        /// <summary>
        /// Logs Translate API Error
        /// </summary>
        /// <param name="timeTickerID">TimeTickerID</param>
        /// <param name="translateText">TranslateText</param>
        /// <param name="errorScope">ErrorScope</param>
        /// <param name="errorMessage">ErrorMessage</param>
        /// <returns></returns>
        public void LogTranslateAPIError(ErrorLogTranslationTicket errorLogTicket,
            string translateText, string errorScope, string errorMessage)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[6];
                sqlParams[0] = new SqlParameter("@TimeTickerID", errorLogTicket.TimeTickerID);
                sqlParams[1] = new SqlParameter("@TranslateText", translateText == null ? string.Empty :
                    translateText);
                sqlParams[2] = new SqlParameter("@ErrorScope", errorScope);
                sqlParams[3] = new SqlParameter("@ErrorMessage", errorMessage);
                sqlParams[4] = new SqlParameter("@User", "SYSTEM");
                sqlParams[5] = new SqlParameter("@SupportType", errorLogTicket.SupportTypeID);
                new DBHelper()
                    .ExecuteNonQuery("AVL.LogTranslateAPIError", sqlParams,ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// BreakSentence if 5000 above character is sent to translate
        /// </summary>
        /// <param name="lengthText">TimeTickerID</param>
        /// <param name="key">Key</param>
        /// <param name="conStr">ConcatenateStrings</param>      
        /// <returns></returns>
        public async Task<dynamic> BreakSentence(string lengthText, string key, ConcatenateStrings conStr)
        {
            HttpResponseMessage response = null;
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))
                
                {
                    EnableTrustedHosts();
                    JObject Jobj = new JObject();
                    ServiceRootObject Rootobj = new ServiceRootObject();
                    Rootobj.SubcriptionKey = key;
                    Rootobj.LengthText = lengthText;
                    Jobj["ServiceRootObject"] = JToken.FromObject(Rootobj);
                    string url = Path.Combine(commonAPIURL, "Translate/BreakSentenceFromString");
                    var dataAsString = JsonConvert.SerializeObject(Jobj);
                    var content = new StringContent(dataAsString);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(new SanitizeString(url).Value, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        int[] constrValue = JsonConvert.DeserializeObject<int[]>(responseBody);
                        return constrValue;
                    }
                    else
                    {
                        conStr = ModifyConcatenateStringsOnException(new List<ConcatenateStrings> { conStr },
                            string.Format(ApplicationConstants.ErrorScope, this.GetType().Name),
                            response.StatusCode.ToString(),
                            response.Content.ReadAsStringAsync().Result).FirstOrDefault();
                    }
                    return conStr;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None)
                {
                    return true;
                }

                var request = sender as HttpWebRequest;
                if (request != null)
                {
                    return commonAPIURL.Contains(request.RequestUri.Host);
                }

                return false;
            };
        }
    }
}