/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*Copyright [2018] – [2021] Cognizant. All rights reserved.
*NOTICE: This unpublished material is proprietary to Cognizant and
*its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information.
 
*This material may be covered by U.S. and/or foreign patents or patent applications.
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/

using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Repository;
using CTS.Applens.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CTS.Applens.WorkProfiler.Common;
using System.Net;
using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.API.Utilities
{
    public static class LoggerAPI
    {
        public static async void LogError(Exception ex, ErrorLogDetails errorLogDetails)
        {
            try
            {
                string url =
                     new AppSettings().AppsSttingsKeyValues["LoggerUrl"].ToString(CultureInfo.CurrentCulture);
                if (!string.IsNullOrEmpty(url))
                {
                    Uri u = new Uri(url);
                    string strPayload = JsonConvert.SerializeObject(errorLogDetails);
                    HttpContent c = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = string.Empty;
                    string responseMessage;
                    using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                    //    var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                    //var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                    //using (var client = httpClientFactory.CreateClient())
                    {
                        client.BaseAddress = new Uri(url);
                        HttpResponseMessage result = await client.PostAsync("Logger", c);
                        result.EnsureSuccessStatusCode();
                        responseMessage = await result.Content.ReadAsStringAsync();
                    }
                }
                new ExceptionLogging().LogException(ex, errorLogDetails.AssociateId);
            }
            catch (Exception e)
            {
                new ExceptionLogging().LogException(e, errorLogDetails.AssociateId);
            }
        }

        public static  void LogError(Exception exception, string AssociateID)
        {
            if (exception != null)
            {
                try
                {
                    ErrorLogDetails info = new ErrorLogDetails();
                    info.LogSeverity = LogSeverity.High.ToString(CultureInfo.InvariantCulture);
                    info.LogLevel = LogLevels.Error.ToString(CultureInfo.InvariantCulture);
                    info.HostName = Environment.MachineName;
                    info.AssociateId = AssociateID;
                    info.CreatedDate = DateTimeOffset.Now.DateTime.ToString(CultureInfo.InvariantCulture);
                    info.Technology = Technology.DotNetCore.ToString(CultureInfo.InvariantCulture);
                    info.ModuleName = ApplicationConstants.ApplicationName;
                    info.FeatureName = exception.TargetSite.Name;
                    info.ClassName = exception.TargetSite.DeclaringType.Name;
                    info.MethodName = exception.TargetSite.Name;
                    info.ProcessId = Environment.ProcessId;
                    info.ErrorCode = Convert.ToString(HttpStatusCode.InternalServerError, CultureInfo.CurrentCulture);
                    info.ErrorMessage = exception.Message;
                    info.StackTrace = exception.Message;
                    info.AdditionalField_1 = string.Empty;
                    info.AdditionalField_2 = string.Empty;
                    NameValueCollection config = ConfigurationManager.GetSection("KeyCloakConfig") as NameValueCollection;
                    KeyCloakToken TokenDetails = KeyCloakTokenHelper.GetAccessTokenForJob(Convert.ToBoolean(ConfigurationManager.AppSettings["KeyCloakEnabled"], CultureInfo.CurrentCulture), config);
                    ApiLogging.Log(ConfigurationManager.AppSettings["LoggerUrl"], info, TokenDetails?.AccessToken,
                        Convert.ToBoolean(ConfigurationManager.AppSettings["KeyCloakEnabled"], CultureInfo.CurrentCulture));

                }
                catch (InvalidOperationException ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}

