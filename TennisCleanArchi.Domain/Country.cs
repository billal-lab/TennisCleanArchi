using System.ComponentModel.DataAnnotations;
namespace TennisCleanArchi.Domain
{
    public class Country
    {
        [Key]
        [MaxLength(3)]
        public string Code { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public ICollection<Player> Players { get; set; } = [];
    }
}
