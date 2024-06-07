/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*Copyright [2018] – [2021] Cognizant. All rights reserved.
*NOTICE: This unpublished material is proprietary to Cognizant and
*its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information. 
  
*This material may be covered by U.S. and/or foreign patents or patent applications. 
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace CTS.Applens.Framework
{
    public class GraphAPI : IGraphAPI
    {
        public string GetProfilePicture(string accessToken, IConfiguration configuration)
        {
            string userImage = "";
            string MicrosoftToken = TokenExchange(accessToken, configuration).AccessToken;
            string GraphAPI = configuration.GetValue<string>(KeyCloakConstants.GraphAPI);
            var content = ApiHelper.Get(GraphAPI, MicrosoftToken, true);
            var responseResult = content?.Result.Content.ReadAsByteArrayAsync().Result;
            if (content.Result.IsSuccessStatusCode)
            {
                string profilePicture = Convert.ToBase64String(responseResult);
                userImage = KeyCloakConstants.ImageType + profilePicture;
            }
            return userImage;

        }
        private static KeyCloakToken TokenExchange(string accessToken, IConfiguration configuration)
        {
            KeyCloakToken tokenDetails = new();
            string tokenUrl = string.Concat(configuration["KeyCloakOidc:Authority"], configuration["KeyCloakOidc:Realm"], KeyCloakConstants.IdentityProvider);
            var content = ApiHelper.Get(tokenUrl, accessToken, true);
            var responseResult = content?.Result.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(responseResult))
            {
                tokenDetails = JsonConvert.DeserializeObject<KeyCloakToken>(responseResult);
            }
            return tokenDetails;
        }
    }
}
