using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.WorkProfiler.Entities.ViewModels;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utils = CTS.Applens.WorkProfiler.DAL.Utility;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [Authorize("AzureADAuth")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// EffortTrackingController 
        /// </summary>
        public ErrorLogController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor,
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProjectDetails")]
        public ActionResult<List<UserDetails>> ProjectDetails(ErrorLogParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, inputparam.EmployeeID.ToString(), Convert.ToInt64(inputparam.CustomerID), null);
            if (value)
            {
                List<UserDetails> UserDetail = new List<UserDetails>();
                try
                {
                    UserDetail = (new TicketUploadRepository()).GetUserProjectDetail(inputparam.EmployeeID, Int32.Parse(inputparam.CustomerID, CultureInfo.CurrentCulture), inputparam.MenuRole);
                }
                catch (Exception ex)
                {
                    //Utils.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                    //
                    //  "On click Error Upload", 0);
                }

                return UserDetail;
            }
            return Unauthorized();
        }


        /// <summary>
        /// Download
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Download")]
        public FileResult Download(ErrorLogParam inputparam)
        {

            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, CognizantID.ToString(), null, null);
            if (value)
            {
                string path = "";
                if (inputparam.Choice == "1")
                {
                    path = new ApplicationConstants().ErrorLogExcel;
                }
                else if (inputparam.Choice == "0")
                {
                    path = new ApplicationConstants().ErrorLogEffortExcel;
                }
                else
                {
                    return null;
                    //mandatory else
                }
                string excelPath = string.Empty;
                string result = string.Empty;
                excelPath = path + inputparam.FileName.Replace("..", "");
                result = (new ErrorLogRepository()).CheckforFile(excelPath);
                string fileName = (new ErrorLogRepository()).GetFileName(result);
                string ext = System.IO.Path.GetExtension(result);
                string fName;
                string validatePath;
                string filestring = System.IO.Path.GetFileName(excelPath);
                string dirctoryName = System.IO.Path.GetDirectoryName(excelPath);
                fName = System.IO.Path.GetFileNameWithoutExtension(excelPath);
                //VeraCode Fix
                if (ext == ".xlsm")
                {
                    validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsm");
                }
                else
                {
                    validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
                }
                validatePath = RemoveLastIndexCharacter(validatePath);
                validatePath = Logger.RegexPath(validatePath);
                if (System.IO.File.Exists(validatePath))
                {
                    byte[] fileBook = System.IO.File.ReadAllBytes(validatePath);
                    var fileContentResult = new FileContentResult(fileBook, "application/vnd.ms-excel") { FileDownloadName = fileName };
                    return fileContentResult;
                }

                return null;
            }
            return null;
        }

        /// <summary>
        /// 
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
        /// Reload
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ErrorLog")]
        public ActionResult<List<Models.ErrorLog>> ErrorLog(ErrorLogParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(inputparam.ProjectID));
            try
            {
                if (value)
                {
                    List<Models.ErrorLog> errorLogList;
                    if (inputparam.Choice == "1")
                    {
                        errorLogList = (new ErrorLogRepository()).GetErrorLog("1", 1, Convert.ToInt32(inputparam.ProjectID, CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        errorLogList = (new ErrorLogRepository()).GetEffortUploadErrorLog(Int32.Parse(inputparam.ProjectID, CultureInfo.CurrentCulture));
                    }

                    return new List<Models.ErrorLog>(errorLogList);
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
        /// This Method is used to GetConfigDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetConfigDetails")]
        public ActionResult<List<Config>> GetConfigDetails(ErrorLogParam inputparam)
        {
            bool value = ValidUser.IsValidAccessUser(this.CurrentUser, null, null, Convert.ToInt64(inputparam.ProjectID));
            if (value)
            {
                List<Config> configDetails;
                configDetails = (new ErrorLogRepository()).GetCofigDetails(Int32.Parse(inputparam.ProjectID, CultureInfo.CurrentCulture));
                return configDetails;
            }
            return Unauthorized();
        }
    }
}

