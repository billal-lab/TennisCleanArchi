using System.ComponentModel.DataAnnotations;
using TennisCleanArchi.Shared;
namespace TennisCleanArchi.Domain
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string LastName { get; set; }
        [MaxLength(255)]
        public string ShortName { get; set; }
        public Sex Sex { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        [MaxLength(255)]
        public string Picture { get; set; }
        public PlayerStats Data { get; set; }
    }
}
