using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradesApp.Models
{
    public class FinalResult
    {
        [Key]
        public int StudentId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }
        public string? Result { get; set; }

        [ForeignKey("StudentId")]
        public required Student Student { get; set; }
    }
}
