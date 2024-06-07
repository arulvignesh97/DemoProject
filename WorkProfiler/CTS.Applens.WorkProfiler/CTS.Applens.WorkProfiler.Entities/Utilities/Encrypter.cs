using CTS.SampleBaseTemplete.Framework;
using CTS.Applens.WorkProfiler.Entities.Base;
using System;
using System.IO;
using System.Net.Http;

namespace CTS.Applens.WorkProfiler.Entities.Utilities
{
    public class KeyValut
    {
        public string GetAzureKey()
        {
            string encryptionenabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];

            if (string.Compare(encryptionenabled, "enabled", true) == 0)
            {
                CacheManager cacheManager = new CacheManager();
                return cacheManager.GetOrCreate<string>("AzureKey", () => CallAzure(), CacheDuration.Long);
            }
            return null;
        }

        private string CallAzure()
        {
            string url = Path.Combine(new AppSettings().AppsSttingsKeyValues["MiddlewareUrl"], "values/get");
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(url).Result.Trim().Replace("\"", "");
            }
        }
    }
}
