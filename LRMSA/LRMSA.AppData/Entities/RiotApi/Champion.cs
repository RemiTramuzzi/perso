using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LRMSA.AppData.Entities.RiotApi
{
    public class Champion
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public int RiotId { get; set; }

        [Required, StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}