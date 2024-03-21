using System.ComponentModel.DataAnnotations;
using Thox.Data;

namespace Thox.Models
{
    public class Group
    {
        [Key]
        public int GroupID { get; set; }

        [Required]
        public int GroupSize { get; set; }

        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string GroupStory { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupCode { get; set; }

        [Required]
        public Experience Experience { get; set; }
    }

    public enum Experience
    {
        FirstTime,
        Beginner,
        Intermediate,
        Expert
    }
}
