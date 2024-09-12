using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentGradesApp.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GradeTypeId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Not 0 ile 100 arasında olmalıdır.")]
        public int Score { get; set; }

        public Student? Student { get; set; }
        public GradeType? GradeType { get; set; }
    }
}
