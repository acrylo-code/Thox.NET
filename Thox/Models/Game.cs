using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Thox.Data;

namespace Thox.Models
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }

        [Required]
        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        [Required]
        [ForeignKey("RoomID")]
        public Room Room { get; set; }

        [Required]
        public DateTime playTime { get; set; }

        public int? FinalTime { get; set; }

        [Required]
        public bool Success { get; set; }

        [Required]
        public Reservation Reservation { get; set; }
    }
}
