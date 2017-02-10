using System.ComponentModel.DataAnnotations.Schema;

namespace LRMSA.AppData.Entities.RiotApi
{
    public class RateLimit
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public int Time { get; set; }

        public int Limit { get; set; }
    }
}