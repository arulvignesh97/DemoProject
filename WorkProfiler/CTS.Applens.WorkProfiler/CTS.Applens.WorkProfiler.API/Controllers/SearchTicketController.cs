using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models.SearchTicket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using CTS.Applens.WorkProfiler.Common;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchTicketController : BaseController
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        CacheManager _cacheManager = new CacheManager();

        /// <summary>
        /// SearchTicketController 
        /// </summary>
        public SearchTicketController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor, 
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// Used to get Ticket Details from search parameter
        /// </summary>
        /// <param name="searchTicketParameters"> Selected Parameter </param>
        /// <returns>Ticket details</returns>
        [HttpPost]
        [Route("GetSearchTickets")]
        public ActionResult<List<SearchTicketDetails>> GetSearchTickets(SearchTicketParameters searchTicketParameters)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,null,Convert.ToInt64(searchTicketParameters.CustomerId),null);
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<SearchTicketDetails> lstTicketDetails = new List<SearchTicketDetails>();
                    SearchTicketRepository objRepository = new SearchTicketRepository();
                    string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                    byte[] aesKeyconstvalAPI = new byte[32];
                    if (encryptionEnabled == "Enabled")
                    {
                        aesKeyconstvalAPI = AseKeyDetail.AesKeyConstVal;
                    }

                    if (searchTicketParameters != null)
                    {
                        lstTicketDetails = objRepository.
                        GetSearchTickets(searchTicketParameters, aesKeyconstvalAPI, encryptionEnabled);
                    }

                    return lstTicketDetails;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }

        /// <summary>
        /// Used to Get Hierarchy List
        /// </summary>
        /// <param name="customerID"> Customer ID </param>
        /// <param name="associateID"> Associate ID </param>
        /// <returns> HHierarchyList </returns>
        [HttpPost]
        [Route("GetHierarchyList")]
        public ActionResult<ProjectBusinessCluster> GetHierarchyList(HierarchyListModel HierarchyListModel)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, HierarchyListModel.associateID.ToString(), Convert.ToInt64(HierarchyListModel.customerID),null);

            try
            {
                if (value)
                {
                    ProjectBusinessCluster projCluster;
                    SearchTicketRepository objRepository = new SearchTicketRepository();
                    string customerID = HttpUtility.HtmlEncode(HierarchyListModel.customerID);
                    string associateID = HttpUtility.HtmlEncode(HierarchyListModel.associateID);
                    projCluster = objRepository.GetBusinessClusterData(customerID, associateID);
                    return projCluster;
                }
                return Unauthorized();  
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }

        /// <summary>
        /// Used to Get TicketTypes
        /// </summary>
        /// <param name="objSearchTicket"> Search Parameters </param>
        /// <returns>TicketTypeList</returns>
        [HttpPost]
        [Route("GetTicketTypes")]
        public ActionResult<List<TicketTypes>> GetTicketTypes(SearchTicketTypeParameter objSearchTicket)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,CognizantID.ToString(),null,null);

            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<TicketTypes> lstTicketTypes = new List<TicketTypes>();
                    SearchTicketRepository objRepository = new SearchTicketRepository();
                    if (objSearchTicket != null)
                    {
                        lstTicketTypes = objRepository.GetTicketTypes(objSearchTicket);
                    }

                    return lstTicketTypes;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }
        /// <summary>
        /// Used to Get TicketStatus
        /// </summary>
        /// <param name="projectID"> Project ID </param>
        /// <returns>Ticket Status List </returns>
        [HttpPost]
        [Route("GetTicketStatus")]
        public ActionResult<List<TicketStatus>> GetTicketStatus(ProjectIDs searchTicketParameters)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser,CognizantID.ToString(),null,null);

            if (!ModelState.IsValid)
            {
                ModelState.Clear();
            }
            try
            {
                if (value)
                {
                    List<TicketStatus> lstTicketStatus = new List<TicketStatus>();
                    SearchTicketRepository objRepository = new SearchTicketRepository();
                    if (!string.IsNullOrEmpty(searchTicketParameters.ProjectId))
                    {
                        lstTicketStatus = objRepository.GetTicketStatus(searchTicketParameters);
                    }

                    return lstTicketStatus;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }

        /// <summary>
        /// This Method is used to WriteDataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="writer"></param>
        /// <param name="includeHeaders"></param>
        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            try
            {
                if (includeHeaders)
                {
                    IEnumerable<String> headerValues = sourceTable.Columns
                        .OfType<DataColumn>()
                        .Select(column => QuoteValue(column.ColumnName));

                    writer.WriteLine(String.Join(",", headerValues));
                }

                IEnumerable<String> items = null;

                foreach (DataRow row in sourceTable.Rows)
                {
                    items = row.ItemArray.Select(o => QuoteValue(o.ToString()));
                    writer.WriteLine(String.Join(",", items));
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            writer.Flush();
        }

        /// <summary>
        /// Method to convert CsvData
        /// </summary>
        /// <param name="CSVdata"></param>
        /// <returns>Method returns converted CSV</returns>

        private DataTable ConvertCsvData(string CSVdata)
        {
            DataTable dt1 = new DataTable();
            dt1.Locale = CultureInfo.InvariantCulture;
            try
            {
                Common.SanitizeStringInput sInput = CSVdata;
                if (sInput != null)
                {
                    string[] Lines = sInput.Value.Split(new char[] { '\r', '\n' });
                    if (Lines == null)
                    {
                        return dt1;
                    }
                    if (Lines.GetLength(0) == 0)
                    {
                        return dt1;
                    }

                    string[] HeaderText = Lines[0].Split('\t');

                    int numOfColumns = HeaderText.Count();


                    foreach (string header in HeaderText)
                    {
                        dt1.Columns.Add(header, typeof(string));
                    }

                    DataRow Row;
                    int length = Lines.GetLength(0);
                    for (int i = 1; i < length; i++)
                    {
                        string[] Fields = Lines[i].Split('\t');
                        if (Fields.GetLength(0) == numOfColumns)
                        {
                            Row = dt1.NewRow();
                            for (int f = 0; f < numOfColumns; f++)
                                if (Fields[f] == "undefined" || Fields[f] == "null")
                                {
                                    Row[f] = "";
                                }
                                else
                                {
                                    Row[f] = Fields[f];
                                }

                            dt1.Rows.Add(Row);
                        }
                    }
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
                return null;
            }
        }
        /// <summary>
        /// This Method is used to QuoteValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }
        [HttpPost]
        [Route("DownloadSearchTicket")]
        public FileResult DownloadSearchTicket(SearchTicketParameters searchTicketParameters)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(),null,null);

            try
            {
                if (value)
                {
                    SearchTicketRepository objRepository = new SearchTicketRepository();
                    string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                    byte[] aesKeyconstvalAPI = new byte[32];
                    if (encryptionEnabled == "Enabled")
                    {
                        aesKeyconstvalAPI = _cacheManager.GetOrCreate<byte[]>("aesKeyconst", null, CacheDuration.Long);
                    }
                    string filename = objRepository.DownloadSearchTicket(searchTicketParameters, aesKeyconstvalAPI, encryptionEnabled);
                    filename = RegexPath(filename);
                    byte[] fileBook = System.IO.File.ReadAllBytes(new SanitizeString(filename).Value);
                    var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                    {
                        FileDownloadName = Path.GetFileName(filename)
                    };

                    return fileContentResult;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return null;
            }
        }
        private static string RegexPath(string fullPath)
        {
            if (fullPath != null)
            {
                fullPath = fullPath.Replace(">", "");
                fullPath = fullPath.Replace("<", "");
                fullPath = fullPath.Replace("..", "");
                return fullPath;
            }
            else
            {
                return "";
            }
        }
    }
}
