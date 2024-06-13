using System.ComponentModel.DataAnnotations;

namespace Thox.Models.ViewModels.Reservation
{
    public class PersonalDetailsModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        public string? Insertion { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht")]
        public string Phone { get; set; }

        public string? Experience { get; set; }

        [Required(ErrorMessage = "Groepsnaam is verplicht")]
        public string GroupName { get; set; }

        public string? GroupDescription { get; set; }

        [Required(ErrorMessage = "Betaling is verplicht")]
        public string Payment { get; set; }
    }
}
