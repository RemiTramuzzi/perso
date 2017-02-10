using System.Threading.Tasks;
using LRMSA.AppData;
using LRMSA.RiotApi.League.Parameters;

namespace LRMSA.RiotApi.League
{
    public class LeagueApi : RiotApiBase
    {
        public LeagueApi(DbContext dbContext) : base(dbContext)
        {
        }

        protected override string Api
        {
            get { return "league"; }
        }

        protected override string Version
        {
            get { return "2.5"; }
        }

        protected override bool HasRateLimit
        {
            get { return true; }
        }

        /// <summary>
        /// https://developer.riotgames.com/api/methods#!/1215/4704
        /// </summary>
        /// <returns></returns>
        public async Task<DTO.League> Challenger(ChallengerParameters parameters)
        {
            return await Call<ChallengerParameters, DTO.League>("challenger", parameters);
        }
    }
}