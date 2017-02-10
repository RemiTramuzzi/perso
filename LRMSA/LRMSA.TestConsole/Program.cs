using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LRMSA.AppData.Entities.RiotApi;
using LRMSA.RiotApi.League;
using LRMSA.RiotApi.League.DTO;
using LRMSA.RiotApi.League.Parameters;
using LRMSA.RiotApi.MatchList;
using LRMSA.RiotApi.MatchList.DTO;
using LRMSA.RiotApi.MatchList.Parameters;
using LRMSA.RiotApi.StaticData;
using LRMSA.RiotApi.Summoner;
using DbContext = LRMSA.AppData.DbContext;

namespace LRMSA.TestConsole
{
    class Program
    {
        private static DbContext _dbContext;

        static async Task UpdateChallengers()
        {
            var challengerLeague = await new LeagueApi(_dbContext)
                .Challenger(new ChallengerParameters { Type = GameQueueType.RANKED_SOLO_5x5 });

            var challengers = challengerLeague.Entries
                .OrderByDescending(c => c.LeaguePoints)
                .Select(c => c.PlayerOrTeamName)
                .ToArray();

            var dbSummoners = await _dbContext.Summoners.ToArrayAsync();
            var remainingSummoners = new List<Summoner>();

            var summonerApi = new SummonerApi(_dbContext);
            var summoners = await summonerApi.ByName(challengers);
            foreach (var summoner in summoners)
            {
                var dbSummoner = dbSummoners.SingleOrDefault(s => s.RiotId == summoner.Value.Id);
                if (dbSummoner == null)
                {
                    _dbContext.Summoners.Add(new Summoner
                    {
                        Name = summoner.Value.Name,
                        LastRevisedOn = summoner.Value.RevisionOn,
                        RiotId = summoner.Value.Id,
                        IsChallenger = true
                    });
                }
                else
                {
                    remainingSummoners.Add(dbSummoner);
                    dbSummoner.Name = summoner.Value.Name;
                    dbSummoner.LastRevisedOn = summoner.Value.RevisionOn;
                    dbSummoner.IsChallenger = true;
                }
            }

            dbSummoners.Except(remainingSummoners).ToList().ForEach(s => s.IsChallenger = false);

            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Challengers : OK");
        }

        static async Task UpdateChampions()
        {
            var staticDataApi = new StaticDataApi(_dbContext);
            var champions = await staticDataApi.Champion();
            foreach (var champion in champions.Select(c => c.Value).ToArray())
            {
                var championId = champion.Id;
                var dbChampion = await _dbContext.Champions.SingleOrDefaultAsync(c => c.RiotId == championId);
                if (dbChampion == null)
                    _dbContext.Champions.Add(new Champion
                    {
                        Name = champion.Name,
                        RiotId = champion.Id
                    });
            }
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Champions : OK");
        }

        static async void DailyUpdate()
        {
            await UpdateChampions();
            await UpdateChallengers();
        }

        static async void UpdateLastDaySummonersMatches()
        {
            var matchListApi = new MatchListApi(_dbContext);
            matchListApi.Waiting += (sender, args) => Console.WriteLine("Waiting...");

            var dbSummoners = await _dbContext.Summoners.ToListAsync();

            //TODO Ne sélectionner que les challengers (paramètre ?)
            // 200
            foreach (var dbSummoner in dbSummoners)
            {
                Console.WriteLine("Summoner: " + dbSummoner.Name);

                // 200 MatchListApi.BySummoner
                var matchList = await matchListApi.BySummoner(dbSummoner.RiotId, new BySummonerParameters
                {
                    BeginTime = DateTime.Now.AddDays(-1),
                    RankedQueues =
                        new[]
                        {
                            RankedQueues.RANKED_FLEX_SR, RankedQueues.RANKED_SOLO_5x5, RankedQueues.RANKED_TEAM_5x5,
                            RankedQueues.TEAM_BUILDER_DRAFT_RANKED_5x5, RankedQueues.TEAM_BUILDER_RANKED_SOLO
                        }
                });

                if (matchList.Matches == null)
                {
                    Console.WriteLine("No match");
                    continue;
                }

                foreach (var match in matchList.Matches)
                {
                    var matchId = match.MatchId;
                    var dbMatch = await _dbContext.Matches.SingleOrDefaultAsync(m => m.RiotId == matchId) ?? _dbContext.Matches.Add(new Match
                    {
                        RiotId = matchId,
                        PlayedOn = DateTimeOffset.FromUnixTimeMilliseconds(match.Timestamp).DateTime,
                        Summoners = new Collection<SummonerMatch>()
                    });

                    if (dbMatch.Id == 0)
                        Console.WriteLine("New match: " + dbMatch.RiotId);

                    if (dbMatch.Summoners.Any(s => s.SummonerId == dbSummoner.Id))
                        continue;

                    var championId = (int)match.Champion;
                    dbMatch.Summoners.Add(new SummonerMatch
                    {
                        ChampionId = _dbContext.Champions.Single(c => c.RiotId == championId).Id,
                        SummonerId = dbSummoner.Id
                    });

                    if (dbMatch.Id == 0)
                        Console.WriteLine("New player");
                }
            }

            await _dbContext.SaveChangesAsync();
            Console.WriteLine("OK");
        }

        static void Main(string[] args)
        {
            _dbContext = new DbContext();

            DailyUpdate();

            //UpdateLastDaySummonersMatches();

            Console.ReadLine();
            _dbContext.Dispose();
        }
    }
}