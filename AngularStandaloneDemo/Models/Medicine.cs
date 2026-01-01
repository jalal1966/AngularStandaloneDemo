using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Packaging { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(500)]
        public string? Composition { get; set; }

        public string? Note { get; set; }
    }
}