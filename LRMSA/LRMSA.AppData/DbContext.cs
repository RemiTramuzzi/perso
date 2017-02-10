using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LRMSA.AppData.Entities.Logging;
using LRMSA.AppData.Entities.RiotApi;

namespace LRMSA.AppData
{
    public class DbContext : System.Data.Entity.DbContext
    {
        #region Logging
        public DbSet<RiotApiRequest> RiotApiRequests { get; set; }
        public DbSet<RateLimitState> RateLimitStates { get; set; }
        #endregion

        #region RiotApi
        public DbSet<RateLimit> RateLimits { get; set; }
        public DbSet<Summoner> Summoners { get; set; }
        public DbSet<Champion> Champions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<SummonerMatch> SummonersMatches { get; set; }
        #endregion

        public DbContext()
            : base("LRMSA.AppData")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new NamespaceBasedSchemaConvention());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.HasDefaultSchema("ef");
        }
    }

    public class NamespaceBasedSchemaConvention : Convention
    {
        public NamespaceBasedSchemaConvention()
        {
            Types().Configure(c => c.ToTable(c.ClrType.Name, (c.ClrType.Namespace ?? string.Empty).Substring((c.ClrType.Namespace ?? string.Empty).LastIndexOf('.') + 1)));
        }
    }
}
