namespace LRMSA.AppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "RiotApi.Champion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RiotId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RiotId, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "RiotApi.Match",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RiotId = c.Long(nullable: false),
                        PlayedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RiotId, unique: true);
            
            CreateTable(
                "RiotApi.SummonerMatch",
                c => new
                    {
                        MatchId = c.Long(nullable: false),
                        SummonerId = c.Long(nullable: false),
                        ChampionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MatchId, t.SummonerId })
                .ForeignKey("RiotApi.Champion", t => t.ChampionId)
                .ForeignKey("RiotApi.Match", t => t.MatchId)
                .ForeignKey("RiotApi.Summoner", t => t.SummonerId)
                .Index(t => t.MatchId)
                .Index(t => t.SummonerId)
                .Index(t => t.ChampionId);
            
            CreateTable(
                "RiotApi.Summoner",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RiotId = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                        LastRevisedOn = c.DateTime(nullable: false),
                        IsChallenger = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RiotId, unique: true);
            
            CreateTable(
                "RiotApi.RateLimit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Int(nullable: false),
                        Limit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Time, unique: true);
            
            CreateTable(
                "Logging.RateLimitState",
                c => new
                    {
                        RiotApiRequestId = c.Long(nullable: false),
                        RateLimitId = c.Int(nullable: false),
                        CurrentState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RiotApiRequestId, t.RateLimitId })
                .ForeignKey("RiotApi.RateLimit", t => t.RateLimitId)
                .ForeignKey("Logging.RiotApiRequest", t => t.RiotApiRequestId)
                .Index(t => t.RiotApiRequestId)
                .Index(t => t.RateLimitId);
            
            CreateTable(
                "Logging.RiotApiRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AbsolutePath = c.String(),
                        RespondedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Logging.RateLimitState", "RiotApiRequestId", "Logging.RiotApiRequest");
            DropForeignKey("Logging.RateLimitState", "RateLimitId", "RiotApi.RateLimit");
            DropForeignKey("RiotApi.SummonerMatch", "SummonerId", "RiotApi.Summoner");
            DropForeignKey("RiotApi.SummonerMatch", "MatchId", "RiotApi.Match");
            DropForeignKey("RiotApi.SummonerMatch", "ChampionId", "RiotApi.Champion");
            DropIndex("Logging.RateLimitState", new[] { "RateLimitId" });
            DropIndex("Logging.RateLimitState", new[] { "RiotApiRequestId" });
            DropIndex("RiotApi.RateLimit", new[] { "Time" });
            DropIndex("RiotApi.Summoner", new[] { "RiotId" });
            DropIndex("RiotApi.SummonerMatch", new[] { "ChampionId" });
            DropIndex("RiotApi.SummonerMatch", new[] { "SummonerId" });
            DropIndex("RiotApi.SummonerMatch", new[] { "MatchId" });
            DropIndex("RiotApi.Match", new[] { "RiotId" });
            DropIndex("RiotApi.Champion", new[] { "Name" });
            DropIndex("RiotApi.Champion", new[] { "RiotId" });
            DropTable("Logging.RiotApiRequest");
            DropTable("Logging.RateLimitState");
            DropTable("RiotApi.RateLimit");
            DropTable("RiotApi.Summoner");
            DropTable("RiotApi.SummonerMatch");
            DropTable("RiotApi.Match");
            DropTable("RiotApi.Champion");
        }
    }
}
