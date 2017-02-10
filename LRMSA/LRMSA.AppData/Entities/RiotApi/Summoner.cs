using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LRMSA.AppData.Entities.RiotApi
{
    public class Summoner
    {
        public long Id { get; set; }

        [Index(IsUnique = true)]
        public long RiotId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime LastRevisedOn { get; set; }

        public bool IsChallenger { get; set; }
    }
}