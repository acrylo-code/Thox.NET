using System.ComponentModel.DataAnnotations;

namespace Thox.Models
{
    public class ApiKeys
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Key { get; set; } 
        [Required]
        public string Owner { get; set; }
    }
}
