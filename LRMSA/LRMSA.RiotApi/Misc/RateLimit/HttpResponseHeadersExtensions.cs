using System;
using System.Linq;
using System.Net.Http.Headers;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public static class HttpResponseHeadersExtensions
    {
        public static string GetValue(this HttpResponseHeaders headers, string key)
        {
            var values = headers.GetValues(key);
            return values == null ? null : values.ToArray()[0];
        }

        public static T GetValue<T>(this HttpResponseHeaders headers, string key, Func<string, T> convert) where T : class
        {
            var result = headers.GetValue(key);
            return result == null ? null : convert(result);
        }
    }
}