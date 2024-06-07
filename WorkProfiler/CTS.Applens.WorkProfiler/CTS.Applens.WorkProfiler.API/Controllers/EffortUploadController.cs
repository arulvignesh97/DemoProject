using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models.EffortUpload;
using CTS.Applens.WorkProfiler.DAL;
using CTS.Applens.WorkProfiler.Entities.Base;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using CTS.Applens.WorkProfiler.API.Utilities;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Common;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class EffortUploadAPIController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        bool Iscognizant;
        Int32 ProjectID;
        string TrackID;
        bool IsEffortTrackActivityWise;
        bool IsDaily;
        bool isinfraproject;
        string result;
        EfforUploadTracker objTrack = new EfforUploadTracker();
        EffortUploadRespository obj = new EffortUploadRespository();
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="_httpContextAccessor"></param>
        public EffortUploadAPIController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor, 
            IWebHostEnvironment hostingEnvironment) : base(configuration, _httpContextAccessor, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this._httpContextAccessor = _httpContextAccessor;
        }




        [HttpGet]
        [Route("Ispace/GetOpportunityTicketdetails")]

        public OpportunityDetail GetOpportunityTicketdetails()
        {
            try
            {
                EffortUploadRespository effresp = new EffortUploadRespository();

                OpportunityDetail opportunityDetail = new OpportunityDetail();
                opportunityDetail = effresp.GetOpportunityTicketdetails();
                return opportunityDetail;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
                return null;
            }
        }
        [HttpPost]
        [Route("Ispace/ISpaceStatusPost")]
        public string ISpaceStatusPost(IspaceOpportunityDetail obj)
        {
            try
            {
                EffortUploadRespository effresp = new EffortUploadRespository();
                var result = effresp.UpdateIdeaOpportunity(obj);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
                return null;
            }
        }
        [HttpPost]
        [Route("TriggerSharepath")]
        public string TriggerSharepath(inputparam finalData)
        {
            try
            {
                string EmployeeID = "SharePath";
                return new EffortUploadRespository().TriggerSharepath(finalData,  EmployeeID);
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
                return null;
            }
        }


        [HttpGet]
        [Route("BulkInsertEffort/{LstEffortUploadDetails}")]        
        private void BulkInsertEffort(
            List<EffortUploadDet> LstEffortUploadDetails, int ProjectID, bool IsDaily, string EmployeeID)
        {
            try
            { 
                 new EffortUploadRespository().BulkInsertEffortRepo(LstEffortUploadDetails, ProjectID, IsDaily, EmployeeID);
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);                
            }
        }
        
        //Code block for Effort Ticket IsAttribute Updated
        [HttpGet]
        [Route("UpdateTicketIsAttributeFlag")]
        public string UpdateTicketIsAttributeFlag(Int64 ProjectID)
        {
            try
            {
                return new EffortUploadRespository().UpdateTicketIsAttributeFlagRepo(ProjectID);
            }
            catch (Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);
                return null;
            }
        }
      
    }
}
