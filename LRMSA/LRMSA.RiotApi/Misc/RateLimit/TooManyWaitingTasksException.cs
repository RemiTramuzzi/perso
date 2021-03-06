using System;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class TooManyWaitingTasksException : Exception
    {
        public int Limit { get; set; }

        public TooManyWaitingTasksException(int limit)
            : base(string.Format("Il y a trop de t�ches en attente ({0})", limit))
        {
            Limit = limit;
        }
    }
}