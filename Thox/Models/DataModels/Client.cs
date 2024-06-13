using System.ComponentModel.DataAnnotations;

namespace Thox.Models.DataModels
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string Insertion { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
    }
}
