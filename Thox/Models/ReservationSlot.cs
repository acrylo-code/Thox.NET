using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Thox.Data;

namespace Thox.Models
{
    public class ReservationSlot
    {
        [Key]
        public int SlotID { get; set; }

        [Required]
        [ForeignKey("RoomID")]
        public Room Room { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public SlotState? State { get; set; } // Enum
    }
    public enum SlotState
    {
        Available,
        Reserved
    }
}
