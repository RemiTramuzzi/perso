using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LRMSA.AppData;
using LRMSA.AppData.Entities.Logging;
using LRMSA.RiotApi.Misc.RateLimit;
using Newtonsoft.Json;

namespace LRMSA.RiotApi
{
    public abstract class RiotApiBase
    {
        protected virtual string RequestFormat
        {
            get { return "https://euw.api.pvp.net/api/lol/euw/v{{Version}}/{{Api}}/{{Method}}{{PathParameters}}?{{QueryParameters}}"; }
        }

        private const string ApiKeyParameter = "api_key=toto";

        protected abstract string Api { get; }
        protected abstract string Version { get; }
        protected abstract bool HasRateLimit { get; }

        private static readonly HttpRequestQueue Queue;
        private static readonly IList<AppData.Entities.RiotApi.RateLimit> DbRateLimits;

        private readonly DbContext _dbContext;

        static RiotApiBase()
        {
            using (var dbContext = new DbContext())
            {
                DbRateLimits = dbContext.RateLimits.ToList();
                Queue = new HttpRequestQueue(new RateLimitList(DbRateLimits.Select(rl => new RateLimit
                {
                    Limit = rl.Limit,
                    Time = rl.Time
                })));
            }
        }

        protected EventHandlerList EventDelegateCollection = new EventHandlerList();

        static readonly object WaitingEventKey = new object();
        public event EventHandler Waiting
        {
            add
            {
                EventDelegateCollection.AddHandler(WaitingEventKey, value);
            }
            remove
            {
                EventDelegateCollection.RemoveHandler(WaitingEventKey, value);
            }
        }
        private void OnWaiting()
        {
            var subscribedDelegates = (EventHandler)EventDelegateCollection[WaitingEventKey];
            if (subscribedDelegates == null)
                return;
            subscribedDelegates(this, EventArgs.Empty);
        }

        protected RiotApiBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            Queue.Waiting += (sender, args) => OnWaiting();
        }

        private Uri FormatUri(string method, string pathParameters, string queryParameters)
        {
            if (string.IsNullOrWhiteSpace(queryParameters))
                queryParameters = ApiKeyParameter;
            else
                queryParameters += "&" + ApiKeyParameter;

            return new Uri(RequestFormat
                .Replace("{{Version}}", Version)
                .Replace("{{Api}}", Api)
                .Replace("{{Method}}", method)
                .Replace("{{PathParameters}}", string.IsNullOrWhiteSpace(pathParameters) ? string.Empty : "/" + pathParameters)
                .Replace("{{QueryParameters}}", queryParameters));
        }

        private string FormatParameters<T>(T parameters)
        {
            return string.Join("&", typeof(T).GetProperties()
                .Where(p =>
                {
                    var value = p.GetValue(parameters);
                    var propertyType = p.PropertyType;
                    return propertyType.IsValueType ? !Activator.CreateInstance(p.PropertyType).Equals(value) : value != null;
                })
                .Select(p => Char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1) + "=" + p.GetValue(parameters)));
        }

        private async Task<TResult> InternalCall<TResult>(string method, string pathParameters, string queryParameters)
        {
            var uri = FormatUri(method, pathParameters, queryParameters);

            HttpResponseMessage response;
            if (HasRateLimit)
            {
                var result = await Queue.Request(uri);
                _dbContext.RiotApiRequests.Add(new RiotApiRequest
                {
                    AbsolutePath = uri.AbsolutePath,
                    RespondedOn = DateTime.Now,
                    RateLimitStates = DbRateLimits.Select(rl => new RateLimitState
                    {
                        RateLimitId = rl.Id,
                        CurrentState = result.HeaderRateLimits[rl.Time].Limit
                    }).ToList()
                });

                await _dbContext.SaveChangesAsync();

                response = result.HttpResponseMessage;
            }
            else
            {
                response = await Queue.RequestNoLimit(uri);
                _dbContext.RiotApiRequests.Add(new RiotApiRequest
                {
                    AbsolutePath = uri.AbsolutePath,
                    RespondedOn = DateTime.Now
                });

                await _dbContext.SaveChangesAsync();
            }

            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        protected async Task<TResult> Call<TResult>(string method)
        {
            return await InternalCall<TResult>(method, null, null);
        }

        protected async Task<TResult> Call<TResult>(string method, string pathParameters)
        {
            return await InternalCall<TResult>(method, pathParameters, null);
        }

        protected async Task<TResult> Call<TParameters, TResult>(string method, TParameters queryParameters)
        {
            return await InternalCall<TResult>(method, null, FormatParameters(queryParameters));
        }

        protected async Task<TResult> Call<TParameters, TResult>(string method, string pathParameters, TParameters queryParameters)
        {
            return await InternalCall<TResult>(method, pathParameters, FormatParameters(queryParameters));
        }
    }
}
