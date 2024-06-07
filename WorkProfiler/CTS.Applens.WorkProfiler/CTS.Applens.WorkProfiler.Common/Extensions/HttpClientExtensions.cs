using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Common.Extensions
{
    public static class HttpClientExtensions
    {
        #region "- Read/Write JSON Responses -"

        /// <summary>
        /// PostAsJsonAsync for Dotnet Core
        /// </summary>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(new Uri(url), content);
        }

        /// <summary>
        /// PutAsJsonAsync for Dotnet Core
        /// </summary>
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PutAsync(new Uri(url), content);
        }

        /// <summary>
        /// ReadAsJsonAsync for Dotnet Core
        /// </summary>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(dataAsString);
        }

        #endregion

        #region "- HeadAsync -"

        public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, string requestUri)
        {
            return client.HeadAsync(new Uri(requestUri));
        }

        public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri)
        {
            return client.HeadAsync(requestUri, HttpCompletionOption.ResponseContentRead);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            string requestUri,
            HttpCompletionOption completionOption
        )
        {
            return client.HeadAsync(new Uri(requestUri), completionOption);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            Uri requestUri,
            HttpCompletionOption completionOption
        )
        {
            return client.HeadAsync(requestUri, completionOption, CancellationToken.None);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            string requestUri,
            CancellationToken cancellationToken
        )
        {
            return client.HeadAsync(new Uri(requestUri), cancellationToken);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            Uri requestUri,
            CancellationToken cancellationToken
        )
        {
            return client.HeadAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            string requestUri,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken
        )
        {
            return client.HeadAsync(new Uri(requestUri), completionOption, cancellationToken);
        }

        public static Task<HttpResponseMessage> HeadAsync(
            this HttpClient client,
            Uri requestUri,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken
        )
        {
            return client.SendAsync(new HttpRequestMessage(HttpMethod.Head, requestUri), completionOption,
                cancellationToken);
        }

        #endregion

        #region "- SendAsync with Content and Headers -"

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            HttpMethod method,
            Uri requestUri,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null,
            HttpContent content = null
        )
        {
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            if (!(headers is null))
            {
                foreach (var (key, value) in headers)
                {
                    request.Headers.Add(key, value);
                }
            }

            return client.SendAsync(request);
        }

        #endregion
    }
}
