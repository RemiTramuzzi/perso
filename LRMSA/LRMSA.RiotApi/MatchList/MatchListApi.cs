using System;
using System.Globalization;
using System.Threading.Tasks;
using LRMSA.AppData;
using LRMSA.RiotApi.MatchList.Parameters;

namespace LRMSA.RiotApi.MatchList
{
    public class MatchListApi : RiotApiBase
    {
        public MatchListApi(DbContext dbContext) : base(dbContext)
        {
        }

        protected override string Api
        {
            get { return "matchlist"; }
        }

        protected override string Version
        {
            get { return "2.2"; }
        }

        protected override bool HasRateLimit
        {
            get { return true; }
        }

        private class InternalBySummonerParameters
        {
            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string ChampionIds { get; set; }
            public string RankedQueues { get; set; }
            public string Seasons { get; set; }
            public long BeginTime { get; set; }
            public long EndTime { get; set; }
            public int BeginIndex { get; set; }
            public int EndIndex { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
            // ReSharper restore MemberCanBePrivate.Local

            public InternalBySummonerParameters(BySummonerParameters parameters)
            {
                if (parameters.ChampionIds != null)
                    ChampionIds = string.Join(",", parameters.ChampionIds);
                if (parameters.RankedQueues != null)
                    RankedQueues = string.Join(",", parameters.RankedQueues);
                if (parameters.Seasons != null)
                    Seasons = string.Join(",", parameters.Seasons);
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                if (parameters.BeginTime.HasValue)
                    BeginTime = (long)((parameters.BeginTime.Value.ToUniversalTime() - origin).TotalMilliseconds);
                if (parameters.EndTime.HasValue)
                    EndTime = (long)((parameters.EndTime.Value.ToUniversalTime() - origin).TotalMilliseconds);
                if (parameters.BeginIndex.HasValue)
                    BeginIndex = parameters.BeginIndex.Value;
                if (parameters.EndIndex.HasValue)
                    EndIndex = parameters.EndIndex.Value;
            }
        }

        public async Task<DTO.MatchList> BySummoner(long summonerId, BySummonerParameters parameters)
        {
            return await Call<InternalBySummonerParameters, DTO.MatchList>("by-summoner", summonerId.ToString(CultureInfo.InvariantCulture), new InternalBySummonerParameters(parameters));
        }
    }
}