/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET

 *Copyright [2018] – [2022] Cognizant. All rights reserved.

 *NOTICE: This unpublished material is proprietary to Cognizant and

 *its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information. 
  

 *This material may be covered by U.S. and/or foreign patents or patent applications. 

 *Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/
using CTS.Applens.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace CTS.Applens.Framework
{
    public static class KeyCloakTokenHelper
    {
        public static string GetAccessToken(HttpContext httpContext, bool keyCloakEnabled)
        {
            string accessToken = string.Empty;
            if (keyCloakEnabled && httpContext != null)
            {
                StringValues token = "";
                if (httpContext.Request.Headers.TryGetValue("Authorization", out token))
                {
                    var tokenData = token.ToString().Split(' ')?.LastOrDefault().Trim();
                    accessToken = tokenData;
                }
            }
            if(!keyCloakEnabled)
            {
                accessToken = AzureADMiddleware.accesstoken;
            }
            return accessToken;
        }
        public static void SetTokenOnHeader(HttpClient httpClient, string accessToken, bool keyCloakEnabled)
        {
            if (keyCloakEnabled && !string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        /// <summary>
        /// To get access token by passing NameValueCollection
        /// </summary>
        /// <param name="KeyCloakEnabled"></param>
        /// <param name="config"></param>
        /// <returns>Key Cloak Token</returns>
        public static KeyCloakToken GetAccessTokenForJob(bool KeyCloakEnabled, NameValueCollection config)
        {
            return GetAccessTokenByConfig(KeyCloakEnabled, config);
        }
        /// <summary>
        /// To get access token by passing IConfiguration
        /// </summary>
        /// <param name="KeyCloakEnabled"></param>
        /// <param name="config"></param>
        /// <returns>Key Cloak Token</returns>
        public static KeyCloakToken GetAccessTokenForJob(bool KeyCloakEnabled, IConfiguration config)
        {
            return GetAccessTokenByConfig(KeyCloakEnabled, config);
        }

        private static KeyCloakToken GetAccessTokenByConfig(bool KeyCloakEnabled, dynamic config)
        {
            KeyCloakToken tokenDetails = new();
            if (config != null && KeyCloakEnabled)
            {
                Uri myUri = new($"{config[KeyCloakConstants.KeyCloakAuthority]}{config[KeyCloakConstants.Realm]}/protocol/openid-connect/token");
                var form = new Dictionary<string, string> {
                    { "grant_type", config[KeyCloakConstants.GrantType]},
                    { "client_id",config[KeyCloakConstants.ClientId]},
                    { "client_secret",config[KeyCloakConstants.ClientSecret]},
                    { "scope",config[KeyCloakConstants.Scope] } };
                using (var client = new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true,
                }))
                {
                    var content = client.PostAsync(myUri, new FormUrlEncodedContent(form));
                    var responseResult = content?.Result.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(responseResult))
                    {
                        tokenDetails = JsonConvert.DeserializeObject<KeyCloakToken>(responseResult);
                    }
                }
            }
            return tokenDetails;
        }
    }
}
