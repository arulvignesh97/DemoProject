using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.DAL;
using Microsoft.AspNetCore.Authorization;
using CTS.Applens.WorkProfiler.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CTS.Applens.WorkProfiler.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize("AzureADAuth")]
    public class WorkProfilerCommonController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly AssociateLensCertificationRepository objAssociateLensCertificationRepository;
        public WorkProfilerCommonController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor) : base(configuration, _httpContextAccessor)
        {
            objAssociateLensCertificationRepository = new AssociateLensCertificationRepository();
        }
        #region Associate Lens
        [HttpPost]
        [Route("GetHealticketdetails")]

        public List<AssociateLensModel> GetHealticketdetails(AssociateLensParam objAssociateParam)
        {
            try
            {
                return new DAL.TicketUploadRepository().GetHealticketdetails(objAssociateParam.StartDate, objAssociateParam.EndDate);
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

        [HttpPost]
        [Route("GetAutomationticketdetails")]
        public List<AssociateLensModel> GetAutomationticketdetails(AssociateLensParam objAssociateParam)
        {
            try
            {
                return new DAL.TicketUploadRepository().GetAutomationticketdetails(objAssociateParam.StartDate, objAssociateParam.EndDate);
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

        [HttpGet]
        [Route("GetCertificationDetails")]
        public DataTable GetCertificationDetails()
        {
            try
            {
                return new AssociateLensCertificationRepository().GetCertificationDetails();
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
        #endregion

        #region Common API
        [HttpGet]
        [Route("GetDebtRelatedDetails")]
        public DataTable GetDebtRelatedDetails()
        {
            try
            {
                return new AssociateLensCertificationRepository().GetDebtRelatedDetails();
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
        #endregion
    }
}
