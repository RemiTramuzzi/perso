using LRMSA.AppData.Entities.RiotApi;

namespace LRMSA.AppData.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DbContext context)
        {
            if (context.RateLimits.All(rl => rl.Time != 10))
                context.RateLimits.Add(new RateLimit
                {
                    Time = 10,
                    Limit = 10
                });
            if (context.RateLimits.All(rl => rl.Time != 600))
                context.RateLimits.Add(new RateLimit
                {
                    Time = 600,
                    Limit = 500
                });
        }
    }
}
