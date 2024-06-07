using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Common.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
            await JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync().ConfigureAwait(false));
    }
}
