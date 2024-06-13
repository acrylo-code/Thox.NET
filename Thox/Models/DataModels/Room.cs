using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Thox.Data;

namespace Thox.Models.DataModels
{
    public class Room
    {

        [Key]
        public int RoomID { get; set; }

        [Required]
        public RoomName RoomName { get; set; } // Enum

        [Required]
        public int MaxGroupSize { get; set; }

        [Required]
        public int MinGroupSize { get; set; }

        public ICollection<RoomPrice>? RoomPrices { get; set; } //prices for the different group sizes

    }

    public class RoomPrice
    {
        [Key]
        public int RoomPriceID { get; set; }

        [Required]
        public int GroupSize { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        // Foreign key property
        [Required]
        public int RoomID { get; set; }

        // Navigation property
        public Room? Room { get; set; }
    }

    public enum RoomName
    {
        RedPlanetRescue,
    }
}
