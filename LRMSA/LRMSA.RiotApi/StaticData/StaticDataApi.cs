using System.Collections.Generic;
using System.Threading.Tasks;
using LRMSA.AppData;

namespace LRMSA.RiotApi.StaticData
{
    public class StaticDataApi : RiotApiBase
    {
        public StaticDataApi(DbContext dbContext)
            : base(dbContext)
        {
        }

        protected override string RequestFormat
        {
            get { return "https://global.api.pvp.net/api/lol/{{Api}}/euw/v{{Version}}/{{Method}}{{PathParameters}}?{{QueryParameters}}"; }
        }

        protected override string Version
        {
            get { return "1.2"; }
        }

        protected override bool HasRateLimit
        {
            get { return false; }
        }

        protected override string Api
        {
            get { return "static-data"; }
        }

        public async Task<Dictionary<string, DTO.Champion>> Champion()
        {
            var championList = await Call<DTO.ChampionList>("champion");
            return championList.Data;
        }
    }
}