using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Thox.Data;

namespace Thox.Models.DataModels
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]
        [ForeignKey("GroupId")]
        public Group Group { get; set; } //information about the group that made the reservation

        [Required]
        [ForeignKey("SlotId")]
        public ReservationSlot Slot { get; set; } //information about the slot that was reserved

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PaymentAmount { get; set; } //price of the reservation

        public PaymentType? PaymentType { get; set; } // Enum

        [Required]
        public PaymentStatus PaymentStatus { get; set; } // Enum

        [Required]
        [ForeignKey("ClientId")]
        public Client Client { get; set; } //information about the person who made the reservation

        [Required]
        public DateTime ReservationTime { get; set; } //when the reservation was made
    }

    public enum PaymentType
    {
        Pin,
        Cash,
        Ideal
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}
