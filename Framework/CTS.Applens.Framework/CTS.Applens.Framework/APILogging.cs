/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.Framework
{
    public static class ApiLogging
    {
        public static string LogApi { get; set; }
        public static string ProjectId { get; set; }
        public static string AssociateId { get; set; }
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="info"></param>
        public static async Task Log(string apiURL, ErrorLogDetails info, string accessToken, bool KeycloakEnabled)
        {
            ErrorLogDetails infoDetail = new ErrorLogDetails();
            if (info != null)
            {
                infoDetail.LogSeverity = info.LogSeverity;
                infoDetail.LogLevel = info.LogLevel;
                infoDetail.HostName = info.HostName;
                infoDetail.AssociateId = info.AssociateId;
                infoDetail.CreatedDate = info.CreatedDate;
                infoDetail.ProjectId = info.ProjectId;
                infoDetail.Technology = info.Technology;
                infoDetail.ModuleName = info.ModuleName;
                infoDetail.FeatureName = info.FeatureName;
                infoDetail.ClassName = info.ClassName;
                infoDetail.MethodName = info.MethodName;
                infoDetail.ProcessId = info.ProcessId;
                infoDetail.ErrorCode = info.ErrorCode;
                infoDetail.ErrorMessage = info.ErrorMessage;
                infoDetail.StackTrace = info.StackTrace;
                infoDetail.AdditionalField_1 = info.AdditionalField_1;
                infoDetail.AdditionalField_2 = info.AdditionalField_2;
            }

            var url = apiURL;
            if (!String.IsNullOrEmpty(url))
            {
                string strPayload = JsonConvert.SerializeObject(infoDetail);
                HttpContent c = new StringContent(strPayload, Encoding.UTF8, "application/json");
                using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                {
                    KeyCloakTokenHelper.SetTokenOnHeader(client,accessToken, KeycloakEnabled);
                    client.BaseAddress = new Uri(apiURL);
                    HttpResponseMessage result = await client.PostAsync("Logger", c).ConfigureAwait(false);
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

            }
        }
    }
}
