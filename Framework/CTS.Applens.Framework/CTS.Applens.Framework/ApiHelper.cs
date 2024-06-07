/***************************************************************************
Author: Arulkumar Sivaraj (663960)
***************************************************************************/
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CTS.Applens.Framework
{
    public class ApiHelper
    {
        private const int defaultTimeout = 5;
        /// <summary>
        /// Defulat api call without keycloak
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post<T>(T model, string apiUrl, string mediaType = "application/json", int timeoutInMinutes = defaultTimeout)
        {
            return await Post(model, apiUrl, null, false, mediaType, timeoutInMinutes);
        }
        /// <summary>
        /// make async post api call with keycloak 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="apiUrl"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post<T>(T model, string apiUrl, string accessToken, bool keyCloakEnabled, string mediaType = "application/json", int timeoutInMinutes = defaultTimeout)
        {
            using (var client = new HttpClient(ClientHandler(true)))
            {
                client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);
                KeyCloakTokenHelper.SetTokenOnHeader(client, accessToken, keyCloakEnabled);
                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                client.BaseAddress = new Uri(apiUrl);
                return await client.PostAsync(apiUrl, httpContent);
            }
        }
        /// <summary>
        /// async get api call without keyclaoak 
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(string apiUrl, int timeoutInMinutes = defaultTimeout)
        {
            using (var client = new HttpClient(ClientHandler(true)))
            {
                client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);
                KeyCloakTokenHelper.SetTokenOnHeader(client, null, false);
                return await client.GetAsync(new SanitizeString(apiUrl).Value);
            }
        }
        /// <summary>
        /// make async get api call with keycloak option
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(string apiUrl, string accessToken, bool keyCloakEnabled, int timeoutInMinutes = defaultTimeout)
        {
            using (var client = new HttpClient(ClientHandler(true)))
            {
                client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);
                SanitizeString sanitizedApiUrl = new (apiUrl);
                KeyCloakTokenHelper.SetTokenOnHeader(client, accessToken, keyCloakEnabled);
                return await client.GetAsync(sanitizedApiUrl.Value);
            }
        }
        /// <summary>
        /// get client handler
        /// </summary>
        /// <param name="useDefaultCredentials"></param>
        /// <returns></returns>
        public static HttpClientHandler ClientHandler(bool useDefaultCredentials)
        {
            if (AzureADMiddleware.cookie?.Count>0)
            {
                return new HttpClientHandler
                {
                    UseDefaultCredentials = useDefaultCredentials,
                    CookieContainer = AzureADMiddleware.cookie
                };
            }
            return new HttpClientHandler
            {
                UseDefaultCredentials = useDefaultCredentials,
            };

        }
    }
}
