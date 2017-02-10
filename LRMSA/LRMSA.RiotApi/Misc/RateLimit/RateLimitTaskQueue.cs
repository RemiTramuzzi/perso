using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class HttpRequestQueue
    {
        private readonly SemaphoreSlim _semaphore;

        private readonly RateLimitTimerList _timers;

        private int NumberOfWaitingTasks { get; set; }

        private readonly RateLimitList _rateLimits;

        private readonly HttpClient _httpClient;

        public HttpRequestQueue(RateLimitList rateLimits)
        {
            _rateLimits = rateLimits;
            _httpClient = new HttpClient();

            _semaphore = new SemaphoreSlim(1);
            _timers = new RateLimitTimerList();

            // Init Timers
            foreach (var rateLimit in _rateLimits)
                _timers.Add(new RateLimitTimer(rateLimit));
        }

        private void IncrementTimer(int time)
        {
            _timers[time].Increment();
        }

        private void IncrementTimers()
        {
            foreach (var timer in _timers)
                timer.Increment();
        }

        private async Task Wait(int time)
        {
            OnWaiting();
            await Task.Delay(time * 1000);
            IncrementTimer(time);
        }

        private int? TimeToWaitNext { get; set; }

        public void WaitNext(int time)
        {
            TimeToWaitNext = time;
        }

        protected EventHandlerList EventDelegateCollection = new EventHandlerList();

        static readonly object BeforeTaskStartEventKey = new object();
        public event EventHandler BeforeTaskStart
        {
            add
            {
                EventDelegateCollection.AddHandler(BeforeTaskStartEventKey, value);
            }
            remove
            {
                EventDelegateCollection.RemoveHandler(BeforeTaskStartEventKey, value);
            }
        }
        private void OnBeforeTaskStart()
        {
            var subscribedDelegates = (EventHandler)EventDelegateCollection[BeforeTaskStartEventKey];
            if (subscribedDelegates == null)
                return;
            subscribedDelegates(this, EventArgs.Empty);
        }

        static readonly object AfterTaskEndEventKey = new object();
        public event EventHandler AfterTaskEnd
        {
            add
            {
                EventDelegateCollection.AddHandler(AfterTaskEndEventKey, value);
            }
            remove
            {
                EventDelegateCollection.RemoveHandler(AfterTaskEndEventKey, value);
            }
        }
        private void OnAfterTaskEnd()
        {
            var subscribedDelegates = (EventHandler)EventDelegateCollection[AfterTaskEndEventKey];
            if (subscribedDelegates == null)
                return;
            subscribedDelegates(this, EventArgs.Empty);
        }

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

        public async Task<HttpRateLimitedResponseMessage> Request(Uri uri)
        {
            NumberOfWaitingTasks++;
            await _semaphore.WaitAsync();
            try
            {
                RateLimitException exception = null;

                try
                {
                    IncrementTimers();
                }
                catch (RateLimitException e)
                {
                    exception = e;
                }

                if (exception != null)
                {
                    await Wait(exception.Time);
                    TimeToWaitNext = null;
                }
                else if (TimeToWaitNext.HasValue)
                {
                    await Wait(TimeToWaitNext.Value);
                    TimeToWaitNext = null;
                }

                OnBeforeTaskStart();

                var result = new HttpRateLimitedResponseMessage(await _httpClient.GetAsync(uri));

                #region RateLimit
                #region Soft Alert

                var timeToWaitForNext = result.TimeToWaitForNext(_rateLimits);
                if (timeToWaitForNext.HasValue)
                    WaitNext(timeToWaitForNext.Value);

                #endregion
                #endregion

                OnAfterTaskEnd();
                return result;
            }
            finally
            {
                _semaphore.Release();
                NumberOfWaitingTasks--;
            }
        }

        public async Task<HttpResponseMessage> RequestNoLimit(Uri uri)
        {
            OnBeforeTaskStart();
            var result = await _httpClient.GetAsync(uri);
            OnAfterTaskEnd();
            return result;
        }
    }
}