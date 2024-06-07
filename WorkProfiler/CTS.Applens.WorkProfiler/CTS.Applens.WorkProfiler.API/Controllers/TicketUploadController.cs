using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Web;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketUploadController : BaseController
    {
        private static string mailstrresult;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// TicketUploadController 
        /// </summary>
        public TicketUploadController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor,
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
            this._configuration = configuration;
        }

        public static string MailstrResult
        {
            get { return mailstrresult; }
            set { mailstrresult = value; }
        }

        /// <summary>
        /// ExportExcelClick
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="IsCognizant"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportExcelClick")]
        public FileResult ExportExcelClick(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(inputparam.ProjectID));
            string ExcelPath = string.Empty;
            string path = new ApplicationConstants().ExcelEffortUploadPath;
            string customerpath = new ApplicationConstants().ExcelEffortUploadPathCustomer;
            try
            {
                if (value)
                {
                    if (inputparam.IsCognizant == "1")
                    {
                        if (inputparam.IsApp)
                        {
                            ExcelPath = path + "\\App\\" + inputparam.FileName.Replace("..", string.Empty);
                        }
                        else
                        {
                            ExcelPath = path + "\\Infra\\" + inputparam.FileName.Replace("..", string.Empty);
                        }
                    }
                    else
                    {
                        ExcelPath = customerpath + inputparam.FileName.Replace("..", string.Empty);
                    }
                    Int32 ESAProjectID = (new TicketUploadRepository().EffortUploadEsaProjectID(Int32.Parse(inputparam.ProjectID, CultureInfo.CurrentCulture)));
                    string EsaProjectID = ESAProjectID.ToString(CultureInfo.CurrentCulture);
                    //Int32 ESAProjectID = (new TicketUploadRepository().EffortUploadEsaProjectID(Int32.Parse(inputparam.ProjectID, CultureInfo.CurrentCulture)));
                    string fileName = (new ErrorLogRepository().GetFileName(inputparam.FileName.Replace("..", string.Empty)));
                    string dirctoryName = Path.GetDirectoryName(ExcelPath);
                    string FName = Path.GetFileNameWithoutExtension(inputparam.FileName);
                    string validatePath = Path.Combine(dirctoryName, FName, ".xlsx");
                    validatePath = RemoveLastIndexCharacter(validatePath);
                    validatePath = RegexPath(validatePath);
                    fileName = ESAProjectID + "_" + fileName;
                    if (System.IO.File.Exists(validatePath))
                    {
                        byte[] fileBook = System.IO.File.ReadAllBytes(validatePath);
                        var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel") { FileDownloadName = fileName };
                        return fileContentResult;
                    }
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
            }

            return null;
        }


        string excelExportPath = new ApplicationConstants().DownloadExcelTemp;
        // GET: TicketUpload 
        [HttpPost]
        [Route("ProjectDetails")]
        public ActionResult<UserDetailsBaseModel> ProjectDetails(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), Convert.ToInt64(inputparam.CustomerID), null);
            if (value)
            {
                UserDetailsBaseModel userDetailsBaseModel = new UserDetailsBaseModel();
                try
                {
                    string cognizantID = CognizantID;
                    if (inputparam.EmployeeID == "" || inputparam.EmployeeID == null || inputparam.EmployeeID == "undefined")
                    {
                        cognizantID = CognizantID;
                    }
                    else
                    {
                        inputparam.EmployeeID = HttpUtility.HtmlEncode(inputparam.EmployeeID);
                        cognizantID = inputparam.EmployeeID;
                    }

                    userDetailsBaseModel.AutoClassificationMessage = new AppSettings().AppsSttingsKeyValues["AutoClassificationMessge"].ToString(CultureInfo.CurrentCulture);
                    userDetailsBaseModel.UserDetails = (new TicketUploadRepository()).GetUserProjectDetail(inputparam.EmployeeID,
                        Int32.Parse(inputparam.CustomerID, CultureInfo.CurrentCulture), inputparam.MenuRole);
                }
                catch (Exception ex)
                {
                    ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                    errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                    errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                    errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                    LogError(errorLogDetails, ex);
                }

                return userDetailsBaseModel;
            }
            return Unauthorized();
        }
        /// <summary>
        /// Gets Onboarding Percentage Details
        /// </summary>
        /// <param name="inputparam"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("GetOnboardingPercentageDetails")]
        public ActionResult<int> GetOnboardingPercentageDetails(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(), null, Convert.ToInt64(inputparam.ProjectID));
            if (value)
            {
                int onboardingPercentage = 0;
                inputparam.EmployeeID = HttpUtility.HtmlEncode(inputparam.EmployeeID);
                inputparam.ProjectID = HttpUtility.HtmlEncode(inputparam.ProjectID);
                TicketUploadRepository ticketdownload = new TicketUploadRepository();
                onboardingPercentage = ticketdownload.
                      GetOnboardingPercentageDetails(Convert.ToInt64(inputparam.ProjectID, CultureInfo.CurrentCulture), inputparam.EmployeeID);
                return onboardingPercentage;
            }
            return Unauthorized();
        }
        /// <summary>
        /// This Method is used to DownloadTicketTemplate
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DownloadTicketTemplate")]
        public FileResult DownloadTicketTemplate(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(), null, Convert.ToInt64(inputparam.ProjectID));
            inputparam.EmployeeID = HttpUtility.HtmlEncode(inputparam.EmployeeID);
            inputparam.ProjectID = HttpUtility.HtmlEncode(inputparam.ProjectID);
            TicketUploadRepository ticketdownload = new TicketUploadRepository();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (value)
                {
                    string Excelpath = ticketdownload.
                        DownloadTicketDumpTemplate(inputparam.EmployeeID.Replace("..", ""), inputparam.ProjectID);
                    string FName = System.IO.Path.GetFileNameWithoutExtension(Excelpath.ToString(CultureInfo.CurrentCulture));
                    string fileName = (new ErrorLogRepository().GetFileName(FName.Replace("..", string.Empty)));
                    string dirctoryName = Path.GetDirectoryName(Excelpath);
                    string fName = Path.GetFileNameWithoutExtension(FName);
                    string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                    validatePath = RemoveLastIndexCharacter(validatePath);
                    validatePath = RegexPath(validatePath);
                    if (System.IO.File.Exists(validatePath))
                    {
                        byte[] fileBook = System.IO.File.ReadAllBytes(validatePath);
                        var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel") { FileDownloadName = fileName };
                        return fileContentResult;
                    }
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
            }
            return null;
        }


        /// <summary>
        /// Method to remove last index character
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string RemoveLastIndexCharacter(string path)
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
        /// <summary>
        /// This Method is used to TicketExcelUpload
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="EmployeeName"></param>
        /// <param name="CustomerId"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TicketExcelUpload")]
        public TicketExcelUploadTrack TicketExcelUpload()
        {
            bool isKeyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            string access = KeyCloakTokenHelper.GetAccessToken(HttpContext, isKeyCloakEnabled);
            string validatePath = null;
            string accountname = Request.Form["accountname"].ToString();
            string esaprojectid = Request.Form["esaprojectid"].ToString();
            var ProjectID = Request.Form["projectID"].ToString();
            var EmployeeID = Request.Form["employeeID"].ToString();
            var CustomerId = Request.Form["customerID"].ToString();
            var IsCognizant = Request.Form["isCognizant"].ToString();
            var EmployeeName = Request.Form["employeeName"].ToString();
            var supervisorLists = new TicketUploadRepository().GetSupervisorAndEmployeeList(ProjectID);
            
            TicketUploadRepository ticketupload = new TicketUploadRepository();
            TicketExcelUploadTrack trackTicketUpload = new TicketExcelUploadTrack();
            EmployeeID = string.IsNullOrEmpty(EmployeeID) ? string.Empty : new SanitizeStringInput(EmployeeID).Value;
            ProjectID = string.IsNullOrEmpty(ProjectID) ? string.Empty : new SanitizeStringInput(ProjectID).Value;
            EmployeeName = string.IsNullOrEmpty(EmployeeName) ? string.Empty : new SanitizeStringInput(EmployeeName).Value;
            CustomerId = string.IsNullOrEmpty(CustomerId) ? string.Empty : new SanitizeStringInput(CustomerId).Value;
            IsCognizant = string.IsNullOrEmpty(IsCognizant) ? string.Empty : new SanitizeStringInput(IsCognizant).Value;
            bool isMultilingualProject = ticketupload.CheckIfMultilingualEnabled(ProjectID, EmployeeID);
            try
            {
                string filename = new ApplicationConstants().EmptyString;
                string fileSavePath = new ApplicationConstants().EmptyString;
                foreach (IFormFile httpPostedFile in Request.Form.Files)
                {
                    if (httpPostedFile.Length == 0)
                    {
                        continue;
                    }
                    filename = Path.GetFileName(httpPostedFile.FileName);
                    MailstrResult = filename;
                    if (filename != null)
                    {
                        fileSavePath = Path.Combine(excelExportPath, filename);
                        string dirctoryName = System.IO.Path.GetDirectoryName(fileSavePath);
                        string fName = System.IO.Path.GetFileNameWithoutExtension(fileSavePath);
                        validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        {
                            using (var stream = new FileStream(validatePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(stream);
                            }
                        }
                    }
                }
                string ext = Path.GetExtension(filename);
                var result = "";
                if (System.IO.File.Exists(validatePath))
                {
                    if (ext == ".xlsx")
                    {
                        CacheManager _cacheManager = new CacheManager();
                        bool allowEncrypt = Convert.ToBoolean(_cacheManager.GetOrCreate<String>("AllowEncrypt" + CognizantID, () => "false", CacheDuration.Long), CultureInfo.CurrentCulture);
                        result = ticketupload.ProcessFileforTicketUpload(filename, excelExportPath, EmployeeID,
                            EmployeeName, CustomerId, ProjectID, "TicketeDumptoTemp", IsCognizant, accountname, supervisorLists, esaprojectid, access, allowEncrypt);
                    }
                    else
                    {
                        result = "Please upload Valid template, valid file is .xlsx";
                    }
                }

                trackTicketUpload.result = result;
                trackTicketUpload.isMultilingual = isMultilingualProject;
                return trackTicketUpload;
            }
            catch (IOException ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                trackTicketUpload.result = new ApplicationConstants().CloseExcel;
                trackTicketUpload.isMultilingual = isMultilingualProject;
                return trackTicketUpload;
            }
        }

        /// <summary>
        /// This Method is used to CheckITSM
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckITSM")]
        public ActionResult<TicketUploadCheckITSM> CheckITSM(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), Convert.ToInt64(inputparam.CustomerID), Convert.ToInt64(inputparam.ProjectID));
            try
            {
                if (value)
                {
                    inputparam.CustomerID = HttpUtility.HtmlEncode(inputparam.CustomerID);
                    inputparam.ProjectID = HttpUtility.HtmlEncode(inputparam.ProjectID);
                    TicketUploadCheckITSM objCheckITSM = new TicketUploadCheckITSM();
                    Int32 percentage = (new TicketUploadRepository().ChekcITSM(inputparam.CustomerID, inputparam.ProjectID));
                    string manualOrAuto = (new TicketUploadRepository().CheckIsManualOrAuto(inputparam.ProjectID));
                    string mandateColumn = (new TicketUploadRepository().CheckMandatecolumns(inputparam.ProjectID));
                    string buttonResponce = string.Empty;
                    bool IsTicketDescriptionOpted = (new TicketUploadRepository().CheckIsTicketDescriptionOpted(inputparam.ProjectID));

                    if (percentage == 100 && manualOrAuto == "M")
                    {
                        buttonResponce = "Y";
                    }
                    else
                    {
                        buttonResponce = "N";
                    }

                    objCheckITSM.Percentage = percentage;
                    objCheckITSM.ManualOrAuto = manualOrAuto;
                    objCheckITSM.Responce = buttonResponce;
                    objCheckITSM.MandateColumn = mandateColumn;
                    objCheckITSM.IsTicketDescriptionOpted = IsTicketDescriptionOpted;

                    return objCheckITSM;
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
        /// This Method is used to GetUploadConfigDetails
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUploadConfigDetails")]
        public ActionResult<ConfigList> GetUploadConfigDetails(TicketUploadParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(), null, Convert.ToInt64(inputparam.ProjectID));
            try
            {
                if (value)
                {
                    inputparam.EmployeeID = HttpUtility.HtmlEncode(inputparam.EmployeeID);
                    ConfigList UploadConfig = new ConfigList();
                    UploadConfig = (new TicketUploadRepository()).GetUploadConfigDetails(Int64.Parse(inputparam.ProjectID, CultureInfo.CurrentCulture), inputparam.EmployeeID);
                    return UploadConfig;
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
        /// This Method is used to Upload EffortUpload Template
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TriggerEffortUpload")]
        public TicketExcelUploadTrack TriggerEffortUpload()
        {
            string validatePath = null;
            string EffortBulkUploadPath = new AppSettings().AppsSttingsKeyValues["EffortBulkUpload"];
            var ProjectID = Request.Form["projectID"].ToString();
            var EmployeeID = Request.Form["employeeID"].ToString();
            var CustomerId = Request.Form["customerID"].ToString();
            var IsCognizant = Request.Form["isCognizant"].ToString();
            var EmployeeName = Request.Form["employeeName"].ToString();
            var IsDaily = Request.Form["isDaily"].ToString();
            var IsEffortConfigured = Request.Form["isEffortConfigured"].ToString();
            var IsApp = Request.Form["isApp"].ToString();

            TicketUploadRepository ticketupload = new TicketUploadRepository();
            TicketExcelUploadTrack trackTicketUpload = new TicketExcelUploadTrack();
            EmployeeID = string.IsNullOrEmpty(EmployeeID) ? string.Empty : new SanitizeStringInput(EmployeeID).Value;
            ProjectID = string.IsNullOrEmpty(ProjectID) ? string.Empty : new SanitizeStringInput(ProjectID).Value;
            EmployeeName = string.IsNullOrEmpty(EmployeeName) ? string.Empty : new SanitizeStringInput(EmployeeName).Value;
            CustomerId = string.IsNullOrEmpty(CustomerId) ? string.Empty : new SanitizeStringInput(CustomerId).Value;
            IsCognizant = string.IsNullOrEmpty(IsCognizant) ? string.Empty : new SanitizeStringInput(IsCognizant).Value;
            IsDaily = string.IsNullOrEmpty(IsDaily) ? string.Empty : new SanitizeStringInput(IsDaily).Value;
            IsEffortConfigured = string.IsNullOrEmpty(IsEffortConfigured) ? string.Empty : new SanitizeStringInput(IsEffortConfigured).Value;
            IsApp = string.IsNullOrEmpty(IsApp) ? string.Empty : new SanitizeStringInput(IsApp).Value;
            bool isMultilingualProject = ticketupload.CheckIfMultilingualEnabled(ProjectID, EmployeeID);
            try
            {
                string filename = new ApplicationConstants().EmptyString;
                string fileSavePath = new ApplicationConstants().EmptyString;
                foreach (IFormFile httpPostedFile in Request.Form.Files)
                {
                    if (httpPostedFile.Length == 0)
                    {
                        continue;
                    }
                    filename = Path.GetFileName(httpPostedFile.FileName);
                    MailstrResult = filename;
                    if (filename != null)
                    {
                        fileSavePath = Path.Combine(EffortBulkUploadPath, filename);
                        string dirctoryName = System.IO.Path.GetDirectoryName(fileSavePath);
                        string fName = System.IO.Path.GetFileNameWithoutExtension(fileSavePath);
                        validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        {
                            using (var stream = new FileStream(validatePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(stream);
                            }
                        }
                    }
                }
                string ext = Path.GetExtension(filename);
                var result = "";
                if (System.IO.File.Exists(validatePath))
                {
                    if (ext == ".xlsx")
                    {
                        result = ticketupload.ProcessFileforEffortUpload(filename, fileSavePath, IsCognizant,
                            Convert.ToInt32(ProjectID, CultureInfo.CurrentCulture), IsEffortConfigured, IsDaily, IsApp, EmployeeID);
                    }
                    else
                    {
                        result = "Please upload Valid template, valid file is .xlsx";
                    }
                }

                trackTicketUpload.result = result;
                trackTicketUpload.isMultilingual = isMultilingualProject;
                return trackTicketUpload;
            }
            catch (IOException ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                trackTicketUpload.result = new ApplicationConstants().CloseExcel;
                trackTicketUpload.isMultilingual = isMultilingualProject;
                return trackTicketUpload;
            }
        }

        /// <summary>
        /// Download the Debt unclassified template
        /// </summary>
        /// <param name="getDownloadAsignment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DownloadDebtTemplate")]
        public FileResult DownloadDebtTemplate(GetUserInfo getDownloadAsignment)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), null, Convert.ToInt64(getDownloadAsignment.ProjectID));
            try
            {
                if (value)
                {
                    string FName = string.Empty;
                    //string filename = string.Empty;
                    var DAL = new TicketUploadRepository();
                    if (getDownloadAsignment != null)
                    {
                        FName = DAL.DownloadDebtTemplate
                        (getDownloadAsignment.EsaProjectID, getDownloadAsignment.ProjectID, getDownloadAsignment.ProjectName,
                        getDownloadAsignment.ClosedDateFrom, getDownloadAsignment.ClosedDateTo, getDownloadAsignment.AppTowerId,
                        getDownloadAsignment.ispureApp, getDownloadAsignment.userID);
                    }
                    //string filename = fileparam.fileName;
                    var _path = FName;
                    _path = RegexPath(FName);
                    if (!string.IsNullOrEmpty(_path))
                    {
                        _path = RegexPath(_path);
                        SanitizeStringInput sanitize = new SanitizeStringInput(_path);
                        string file = sanitize.Value;
                        string dirctoryName = Path.GetDirectoryName(file);
                        string fName = Path.GetFileNameWithoutExtension(file);
                        string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        string validatedPath = RegexPath(validatePath);
                        if (System.IO.File.Exists(validatePath))
                        {
                            //string validatedPath1 = RegexPath(validatedPath);
                            byte[] fileBook = System.IO.File.ReadAllBytes(validatedPath);
                            var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                            { FileDownloadName = fName };
                            return fileContentResult;
                        }
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("DownloadDebtUnClassifiedTickets")]
        public FileResult DownloadDebtUnClassifiedTickets(Debtparam fileparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), null, Convert.ToInt64(fileparam.ProjectID));
            try
            {
                if (value)
                {
                    string filename = fileparam.fileName;
                    string path = string.Empty;
                    var DAL = new TicketUploadRepository();
                    if (fileparam != null)
                    {
                        path = DAL.DownloadDebtTemplate
                        (fileparam.EsaProjectID, fileparam.ProjectID, fileparam.ProjectName,
                        fileparam.ClosedDateFrom, fileparam.ClosedDateTo, fileparam.AppTowerId,
                        fileparam.ispureApp, fileparam.userID);
                    }
                    // var _path = RegexPath(filename);
                    var _path = path;
                    if (!string.IsNullOrEmpty(_path))
                    {
                        _path = RegexPath(_path);
                        SanitizeStringInput sanitize = new SanitizeStringInput(_path);
                        string file = sanitize.Value;
                        string dirctoryName = Path.GetDirectoryName(file);
                        string fName = Path.GetFileNameWithoutExtension(file);
                        string validatePath = Path.Combine(dirctoryName, fName, ".xlsm");
                        validatePath = OpenXMLOperations.RemoveLastIndexCharacter(validatePath);
                        //string validatedPath = RegexPath(validatePath);
                        string validatedPath = validatePath;
                        validatedPath = RegexPath(validatePath);
                        if (System.IO.File.Exists(validatedPath))
                        {
                            //string validatedPath1 = RegexPath(validatedPath);
                            string validatedPath1 = validatedPath;
                            validatedPath1 = RegexPath(validatedPath);
                            byte[] fileBook = System.IO.File.ReadAllBytes(validatedPath1);
                            var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel")
                            {
                                // FileDownloadName = validatedPath1
                            };
                            return fileContentResult;
                        }
                    }
                    var fileContentResults = new FileContentResult(new byte[0], "application/vnd.ms-excel")
                    {
                        // FileDownloadName = validatedPath1
                    };
                    return fileContentResults;
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
        /// <summary>
        /// Get the Regexpath
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static string RegexPath(string fullPath)
        {
            if (fullPath != null)
            {
                fullPath = fullPath.Replace(">", "", StringComparison.CurrentCulture);
                fullPath = fullPath.Replace("<", "", StringComparison.CurrentCulture);
                fullPath = fullPath.Replace("..", "", StringComparison.CurrentCulture);
                return fullPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Upload for the debt unclassified ticket
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadforDebtUnclassifiedTicket")]
        public object UploadforDebtUnclassifiedTicket()
        {
            TicketUploadRepository ticketupload = new TicketUploadRepository();
            try
            {
                string validatePath = string.Empty;
                var file = Request.Form.Files[0];
                string fullPath = string.Empty;
                string folderName = Constants.UploadFolderName;
                string webRootPath = new
                    AppSettings().AppsSttingsKeyValues["DebtExcelsaveTemplatePath"];
                string newPath = Path.Combine(webRootPath, folderName);
                string fileName =
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fileExt = "." + fileName.Split(".")[1];
                string fileNameOnly = System.IO.Path.GetFileNameWithoutExtension
                    (ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0 && Constants.XlsmFileExtension.Contains(Path.GetExtension(fileName)))
                {
                    string fileNamewithtExtension = Path.GetFileNameWithoutExtension(fileName);
                    Regex rgx = new Regex(@"(.+?)(\.[^\.]+$|$)");
                    if (rgx.IsMatch(fileNamewithtExtension))
                    {
                        fullPath = System.IO.Path.Combine(newPath, fileNameOnly, fileExt);
                        validatePath = RemoveLastIndexCharacter(fullPath);
                        Regex rgxPath = new Regex("(\\\\?([^\\/]*[\\/])*)([^\\/]+)");
                        if (validatePath != null)
                        {
                            validatePath = validatePath.Replace(">", "", StringComparison.CurrentCulture);
                            validatePath = validatePath.Replace("<", "", StringComparison.CurrentCulture);
                            validatePath = validatePath.Replace("..", "", StringComparison.CurrentCulture);
                            if (rgxPath.IsMatch(validatePath))
                            {
                                using (var stream = new FileStream(validatePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                            }
                        }
                    }
                }

                var primaryValidation = OpenXMLOperations.ExcelPrimaryValidation(validatePath);
                if (!string.IsNullOrEmpty(primaryValidation))
                {
                    return primaryValidation;
                }
                else
                {
                    string result = ticketupload.ProcessFileUploadforDebt(fileName, validatePath,
                        Request.Form["projectID"].ToString(), Request.Form["ispureApp"].ToString(), Request.Form["UserId"].ToString());
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
