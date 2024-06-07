namespace AVMDartLiteAPI.Controllers
{
    using CTS.Applens.WorkProfiler.API.Controllers;
    using CTS.Applens.WorkProfiler.Repository;
    using CTS.Applens.WorkProfiler.Entities.ViewModels;
    using CTS.Applens.Framework;
    using CTS.Applens.WorkProfiler.Models.ServiceClassification;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("AzureADAuth")]
    /// <summary>
    /// ServiceClassificationController
    /// </summary>
    public class ServiceClassificationController : BaseController
    {        
        private readonly int ReturnResponseCodeOk = 200;
        private readonly int ReturnResponseCodeNoContent = 204;

        /// <summary>
        /// EffortTrackingController 
        /// </summary>
        public ServiceClassificationController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor, 
            IWebHostEnvironment _hostingEnvironment) : base(configuration, _httpContextAccessor, _hostingEnvironment)
        {
            
        }

        [HttpGet]
        [Route("TicketDetailsForServiceClassification")]
        public Int32 TicketDetailsForServiceClassification()
        {
            try
            {

                List<ServiceTicketDetails> tickketDetails = new List<ServiceTicketDetails>();
                ServiceClassificationRepository objService = new ServiceClassificationRepository();
                tickketDetails = objService.GetTicketDetails();
                List<ServiceAutoClassifiedTicketDetails> deleteTempTable;
                deleteTempTable = (from tickerDetails in tickketDetails
                                   select new ServiceAutoClassifiedTicketDetails
                                   {
                                       TimeTickerId = tickerDetails.TimeTickerId
                                   }).ToList();
                List<UpdateServiceName> updServiceName = ServiceAutoClassification(tickketDetails);
                objService.ServiceNameUpdation(updServiceName, deleteTempTable);
                return Convert.ToInt32(ReturnResponseCodeOk);                
            }
            catch(Exception ex)
            {
                ErrorLogDetails errorLogDetails = new ErrorLogDetails();
                errorLogDetails.FeatureName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                errorLogDetails.MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                errorLogDetails.AssociateId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                LogError(errorLogDetails, ex);

                return ReturnResponseCodeNoContent;
            }
        }
                
        private List<UpdateServiceName> ServiceAutoClassification(List<ServiceTicketDetails> tickketDetails)
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

    }
}
        
        
   
