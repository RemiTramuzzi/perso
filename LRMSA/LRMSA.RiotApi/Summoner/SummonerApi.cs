using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LRMSA.AppData;
using RT.Extensions.System.Collections.Generic;

namespace LRMSA.RiotApi.Summoner
{
    public class SummonerApi : RiotApiBase
    {
        public SummonerApi(DbContext dbContext)
            : base(dbContext)
        {
        }

        protected override string Version
        {
            get { return "1.4"; }
        }

        protected override string Api
        {
            get { return "summoner"; }
        }

        protected override bool HasRateLimit
        {
            get { return true; }
        }

        private const int ByNameNamesLimit = 40;

        /// <summary>
        /// ATTENTION !
        /// X requ�tes seront ex�cut�es, X �tant (le nombre de noms donn�s en param�tre) divis� par (le nombre maximal de noms autoris� pour la requ�te) plus 1
        /// Il est donc possible que cette m�thode prenne du temps � cause du Rate Limiting
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, DTO.Summoner>> ByName(IEnumerable<string> names)
        {
            if (names == null)
                throw new ArgumentNullException("names");

            var result = new Dictionary<string, DTO.Summoner>();
            foreach (var limitedSummonersNames in names.ToArray().Split(ByNameNamesLimit))
            {
                var summoners = await Call<Dictionary<string, DTO.Summoner>>("by-name", string.Join(",", limitedSummonersNames));
                summoners.ToList().ForEach(s => result.Add(s.Key, s.Value));
            }
            return result;
        }
    }
}